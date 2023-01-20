using budz_backend.Exceptions;
using budz_backend.Models.Settings;
using budz_backend.Models.User;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Opw.HttpExceptions;
using Newtonsoft.Json;

namespace budz_backend.Services.MongoServices.User;


public class UserService
{
    private readonly MongoConfig _config;
    private readonly IMongoCollection<MongoUser> col;

    public UserService(IOptions<MongoConfig> config)
    {
        _config = config.Value;

        var client = new MongoClient(_config.Connection);
        col = client.GetDatabase(_config.Database).GetCollection<MongoUser>(_config.Collection);
    }

    public async Task<bool> DocumentExists<TField>(string field, TField value)
    {
        var DocumentFilter = Builders<MongoUser>.Filter.Eq(field, value);
        return await col.CountDocumentsAsync(DocumentFilter) > 0;
    }

    public async Task<MongoUser> GetAsync(string id)
    {
        if (!await DocumentExists("_id", id))
        {
            throw new BadRequestException("user does not exist");
        }
        var matchedDocuments = await col.FindAsync(Builders<MongoUser>.Filter.Eq("_id", id));
        return matchedDocuments.First();
    }

    public async Task<MongoUser> GetAsync<TField>(string field, TField value)
    {
        var searchQuery = Builders<MongoUser>.Filter.Eq(field, value);
        if (!await DocumentExists(field, value))
        {
            throw new BadRequestException("cannot find any documents with provided query");
        }

        var foundDocuments = await col.FindAsync(searchQuery);
        return foundDocuments.First();
    }

    public async Task InsertAsync(MongoUser user)
    {
        await col.InsertOneAsync(user);
    }

    public async Task<MongoUser> FindExclude<TField>(string field, TField value, string excludeField)
    {
        var excludeFields = Builders<MongoUser>.Projection.Exclude(excludeField);

        var documentFilter = Builders<MongoUser>.Filter.Eq(field, value);
        if (!await DocumentExists(field, value))
        {
            throw new BadRequestException("cannot find document with provided query");
        }

        var foundDocuments = col.Find(documentFilter).Project<MongoUser>(excludeFields);
        return foundDocuments.First();
    }

    public async Task DeleteAsync(string id)
    {
        var filter = Builders<MongoUser>.Filter.Eq("_id", id);
        if (!await DocumentExists("_id", id))
        {
            throw new BadHttpRequestException("provided id does not exist");
        }

        await col.DeleteOneAsync(filter);
    }

    public async Task<bool> Update<TUpdate>(string id, string field, TUpdate newValue)
    {
        var userFilter = Builders<MongoUser>.Filter.Eq("_id", id);
        var updateFilter = Builders<MongoUser>.Update.Set(field, newValue);

        if (!await DocumentExists("_id", id))
        {
            throw new InvalidRecordException($"{id} is not a valid id");
        }

        var updateResult = await col.UpdateOneAsync(userFilter, updateFilter);
        return updateResult.ModifiedCount == 1;
    }

    public async Task<bool> PushArray<TField>(string id, string arrayField, IEnumerable<TField> mergeArray)
    {
        var userFilter = Builders<MongoUser>.Filter.Eq("_id", id);
        var updateFilter = Builders<MongoUser>.Update.PushEach(arrayField, mergeArray);

        if (!await DocumentExists("_id", id))
        {
            throw new InvalidRecordException("provided id does not exist");
        }

        var pushResult = await col.UpdateOneAsync(userFilter, updateFilter);
        return pushResult.ModifiedCount == 1;
    }

    public async Task<T?> GetField<T>(string id, string field)
    {
        var foundUser = col.AsQueryable().Where(x => x.Id == id).First();
        return JsonConvert.DeserializeObject<T>(foundUser.ToString());
    }

}
