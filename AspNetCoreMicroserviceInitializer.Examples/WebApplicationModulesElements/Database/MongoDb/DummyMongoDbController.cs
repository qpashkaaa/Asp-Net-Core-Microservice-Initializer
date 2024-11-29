using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.MongoDb;

[Route("api/[controller]")]
[ApiController]
public class DummyMongoDbController : ControllerBase
{
    private readonly IMongoDbRepository<DummyMongoDbEntity> _repository;

    public DummyMongoDbController(IMongoDbRepository<DummyMongoDbEntity> repository)
    {
        _repository = repository;
    }

    [HttpPost("InsertAsync")]
    public async Task InsertAsync(DummyMongoDbEntity entity)
    {
        await _repository.InsertAsync(entity);
    }

    [HttpGet("GetAllAsync")]
    public async Task<IEnumerable<DummyMongoDbEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
}
