namespace budz_backend.Services.Redis;
using NReJSON;
using StackExchange.Redis;
using Microsoft.Extensions.Options;
using budz_backend.Models.Notification;
using budz_backend.Services.MongoServices.User;



public class RedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _database;

    private readonly UserService _col;

    public RedisService(IOptions<IConnectionMultiplexer> connectionMultiplexer, IOptions<UserService> service)
    {
        _connectionMultiplexer = connectionMultiplexer.Value;
        _database = _connectionMultiplexer.GetDatabase();
    }


    public async Task<string> Create<T>(T document)
    {
        var generatedID = Guid.NewGuid().ToString();
        _database.JsonSet<T>(generatedID, document);
        return generatedID;
    }

    public async Task<Dictionary<string, object>>? Get(string documentID)
    {
        if (!_database.KeyExists(documentID))
        {
            return null;
        }
        return _database.JsonGet<Dictionary<string, object>>(documentID);
    }


    public async Task Set<T>(string key, T? jsonModel)
    {
        if (_database.KeyExists(key))
        {
            throw new 
        }
    }

}