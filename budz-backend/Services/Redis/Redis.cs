using budz_backend.Exceptions;

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

    public async Task<T?> Get<T>(string documentID)
    {
        if (!_database.KeyExists(documentID))
        {
            return (T)(object)null;
        }
        return _database.JsonGet<T>(documentID);
    }


    public async Task<bool> Set<T>(string key, T? jsonModel, bool overrwite)
    {
        if (_database.KeyExists(key) && !overrwite)
        {
            return false;
        }
        return _database.JsonSet(key, jsonModel).IsSuccess;
    }
    
}