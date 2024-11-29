using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;
using AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.MongoDb;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.Redis;

[Route("api/[controller]")]
[ApiController]
public class DummyRedisController : ControllerBase
{
    private readonly IRedisRepository<DummyRedisEntity> _repository;

    public DummyRedisController(IRedisRepository<DummyRedisEntity> repository)
    {
        _repository = repository;
    }

    [HttpPost("InsertAsync")]
    public async Task InsertAsync(DummyRedisEntity entity)
    {
        await _repository.InsertAsync(entity);
    }

    [HttpGet("GetAllAsync")]
    public async Task<DummyRedisEntity?> GetByKeyAsync(string key)
    {
        return await _repository.GetByKeyAsync(key);
    }
}
