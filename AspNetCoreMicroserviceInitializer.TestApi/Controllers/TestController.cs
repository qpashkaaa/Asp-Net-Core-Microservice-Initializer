using AspNetCoreMicroserviceInitializer.TestApi.TestElements.AutoMappers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroserviceInitializer.TestApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    #region AutoMappers (works)
    //private readonly IMapper _mapper;

    //public TestController(IMapper mapper)
    //{
    //    _mapper = mapper;
    //}

    //[HttpGet("GetModelBase")]
    //public ModelBase GetModelBase()
    //{
    //    var dto = new ModelDTO
    //    {
    //        Message = "FromDTOToBase",
    //        Flag = false,
    //        Number = 52,
    //        Array = new[ ] { "test", "test2", "test3" }
    //    };

    //    return _mapper.Map<ModelBase>(dto);
    //}

    //[HttpGet("GetModelDTO")]
    //public ModelDTO GetModelDTO()
    //{
    //    var modelBase = new ModelBase
    //    {
    //        Flag = true,
    //        NotMappedProperty = "Property",
    //        Number = 1
    //    };

    //    return _mapper.Map<ModelDTO>(modelBase);
    //}
    #endregion

    [HttpGet("TestCors")]
    public string TestCors()
    {
        return "Cors is working";
    }
}
