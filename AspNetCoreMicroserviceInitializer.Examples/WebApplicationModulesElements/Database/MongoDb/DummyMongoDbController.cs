using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.MongoDb;

[Route("api/[controller]")]
[ApiController]
public class DummyMongoDbController : ControllerBase
{
    private readonly IMongoRepository<DummyMongoDbEntity> _firstRepository;
    private readonly DummySecondMongoDbRepository _secondRepository;

    public DummyMongoDbController(
        IMongoRepository<DummyMongoDbEntity> firstRepository,
        DummySecondMongoDbRepository secondRepository)
    {
        _firstRepository = firstRepository;
        _secondRepository = secondRepository;
    }

    [HttpPost("InsertAsyncFirst")]
    public async Task InsertAsync(DummyMongoDbEntity entity)
    {
        await _firstRepository.InsertAsync(entity);
    }

    [HttpGet("GetAllAsyncFirst")]
    public async Task<IEnumerable<DummyMongoDbEntity>> GetAllAsync()
    {
        return await _firstRepository.GetAllAsync();
    }

    [HttpGet("GetAllAsyncSecond")]
    public async Task<IEnumerable<DummyMongoDbEntity>> GetAllAsyncSecond()
    {
        return await _secondRepository.GetAllAsync();
    }

    [HttpPost("InsertAsyncSecond")]
    public async Task InsertAsyncSecond(DummyMongoDbEntity entity)
    {
        await _secondRepository.InsertAsync(entity);
    }
}
