using AspNetCoreMicroserviceInitializer.Database.Interfaces.Sql;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.Sql;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController : ControllerBase
{
    private readonly ISqlRepository<DummyModel> _dummyRepository;

    public DatabaseController(ISqlRepository<DummyModel> dummyRepository)
    {
        _dummyRepository = dummyRepository;
    }

    [HttpGet("GetAllAsync")]
    public async Task<IEnumerable<DummyModel>> GetAllAsync()
    {
        return await _dummyRepository.GetAllAsync();
    }

    [HttpPost("GetByIdAsync")]
    public async Task<DummyModel?> GetByIdAsync(long id)
    {
        return await _dummyRepository.GetByIdAsync(id);
    }

    [HttpPost("GetByIdsAsync")]
    public async Task<IEnumerable<DummyModel>> GetByIdsAsync(IEnumerable<long> ids)
    {
        return await _dummyRepository.GetByIdsAsync(ids);
    }

    [HttpPost("GetPageAsync")]
    public async Task<IEnumerable<DummyModel>> GetPageAsync(int pageNumber, int pageSize)
    {
        return await _dummyRepository.GetPageAsync(pageNumber, pageSize);
    }

    [HttpPost("IsExistsAsync")]
    public async Task<bool> IsExistsAsync(long id)
    {
        return await _dummyRepository.IsExistsAsync(id);
    }

    [HttpPost("DeleteByIdAsync")]
    public async Task DeleteByIdAsync(long id)
    {
        await _dummyRepository.DeleteByIdAsync(id);
    }

    [HttpPost("DeleteByIdsAsync")]
    public async Task<int> DeleteByIdsAsync(IEnumerable<long> ids)
    {
        return await _dummyRepository.DeleteByIdsAsync(ids);
    }

    [HttpPost("InsertAsync")]
    public async Task InsertAsync(DummyModel entity)
    {
        await _dummyRepository.InsertAsync(entity);
    }

    [HttpPost("InsertBatchAsync")]
    public async Task InsertBatchAsync(IEnumerable<DummyModel> entities)
    {
        await _dummyRepository.InsertBatchAsync(entities);
    }

    [HttpPost("UpdateAsync")]
    public async Task UpdateAsync(DummyModel entity)
    {
        await _dummyRepository.UpdateAsync(entity);
    }

    [HttpPost("UpdateBatchAsync")]
    public async Task UpdateBatchAsync(IEnumerable<DummyModel> entities)
    {
        await _dummyRepository.UpdateBatchAsync(entities);
    }
}
