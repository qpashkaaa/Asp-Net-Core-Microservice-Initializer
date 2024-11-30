using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;
using AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.MongoDb;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.Redis;

[Route("api/[controller]")]
[ApiController]
public class DummyRedisController : ControllerBase
{
    private readonly IRedisRepository<DummyRedisEntity> _firstRepository;
    private readonly DummyRedisSecondRepository _secondRepository;

    public DummyRedisController(
        IRedisRepository<DummyRedisEntity> repository,
        DummyRedisSecondRepository secondRepository)
    {
        _firstRepository = repository;
        _secondRepository = secondRepository;
    }

    [HttpPost("InsertAsyncFirst")]
    public async Task InsertAsync(DummyRedisEntity entity)
    {
        await _firstRepository.InsertAsync(entity);
    }

    [HttpGet("GetAllAsyncFirts")]
    public async Task<DummyRedisEntity?> GetByKeyAsync(string key)
    {
        return await _firstRepository.GetByKeyAsync(key);
    }

    [HttpPost("InsertAsyncSecond")]
    public async Task InsertAsyncSecond(DummyRedisEntity entity)
    {
        await _secondRepository.InsertAsync(entity);
    }

    [HttpGet("GetAllAsyncSecond")]
    public async Task<DummyRedisEntity?> GetByKeyAsyncSecond(string key)
    {
        return await _secondRepository.GetByKeyAsync(key);
    }
}
