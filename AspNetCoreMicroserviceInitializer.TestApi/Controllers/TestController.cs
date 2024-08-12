//using AspNetCoreMicroserviceInitializer.TestApi.TestElements.AutoMappers;
//using AspNetCoreMicroserviceInitializer.TestApi.TestElements.Database;
//using AspNetCoreMicroserviceInitializer.TestApi.TestElements.EnvironmentVariables;
//using AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services;
//using AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services.Interfaces;
//using AspNetCoreMicroserviceInitializer.TestApi.TestElements.Settings;
//using AutoMapper;
//using Microsoft.Extensions.Options;

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

    #region Cors (works)
    //[HttpGet("TestCors")]
    //public string TestCors()
    //{
    //    return "Cors is working";
    //}
    #endregion

    #region Settings (works)
    //private readonly TestSetting _testSettings;

    //public TestController(IOptions<TestSetting> testSetting)
    //{
    //    _testSettings = testSetting.Value;
    //}

    //[HttpGet("TestSettings")]
    //public TestSetting TestCors()
    //{
    //    return _testSettings;
    //}
    #endregion

    #region Database (Works)
    //private readonly TestRepository _testRepository;

    //public TestController(TestRepository testRepository)
    //{
    //    _testRepository = testRepository;
    //}

    //[HttpGet("InsertOne")]
    //public async Task InsertOne()
    //{
    //    await _testRepository.InsertAsync(new TestTable
    //    {
    //        Message = $"CreatedFromController {Guid.NewGuid()}"
    //    });
    //}

    //[HttpGet("GetOne")]
    //public async Task<TestTable?> GetOne(long id)
    //{ 
    #endregion

    #region Serilog (Works)

    //private readonly ILogger<TestController> _logger;

    //public TestController(ILogger<TestController> logger)
    //{
    //    _logger = logger;
    //}

    //[HttpGet("WriteLog")]
    //public void WriteLog()
    //{
    //    _logger.LogError("Hello from ILogger");
    //    Serilog.Log.Error("Hello from Log");
    //}
    #endregion

    #region EnvironmentVariables (Works)
    //private readonly EnvironmentVariablesSettingTest _envVar;

    //public TestController(IOptions<EnvironmentVariablesSettingTest> envVar)
    //{
    //    _envVar = envVar.Value;
    //}


    //[HttpGet("GetModel")]
    //public EnvironmentVariablesSettingTest GetModel()
    //{
    //    return _envVar;
    //}
    #endregion

    #region Services
    //private readonly IServiceWithImplementationFactory _implementationFactoryService;
    //private readonly IServiceWithConstructorParams _serviceWithConstructorParams;
    //private readonly ServiceWithoutConstructorParams _serviceWithoutConstructorParams;
    //private readonly IFakeConverterService _fakeConvertService;

    //public TestController(
    //    IServiceWithImplementationFactory implementationFactoryService,
    //    IServiceWithConstructorParams serviceWithConstructorParams,
    //    ServiceWithoutConstructorParams serviceWithoutConstructorParams,
    //    IFakeConverterService fakeConvertService)
    //{
    //    _implementationFactoryService = implementationFactoryService;
    //    _serviceWithConstructorParams = serviceWithConstructorParams;
    //    _serviceWithoutConstructorParams = serviceWithoutConstructorParams;
    //    _fakeConvertService = fakeConvertService;
    //}

    //[HttpGet("IServiceWithImplementationFactoryMessage")]
    //public string IServiceWithImplementationFactoryMessage()
    //{
    //    return _implementationFactoryService.GetMessage();
    //}

    //[HttpGet("IServiceWithConstructorParamsMessageFromThisService")]
    //public string IServiceWithConstructorParamsMessageFromThisService()
    //{
    //    return _serviceWithConstructorParams.MessageFromThisService();
    //}

    //[HttpGet("IServiceWithConstructorParamsMessageFromOtherService")]
    //public string IServiceWithConstructorParamsMessageFromOtherService()
    //{
    //    return _serviceWithConstructorParams.MessageFromOtherService();
    //}

    //[HttpGet("ServiceWithoutConstructorParamsMessage")]
    //public string ServiceWithoutConstructorParamsMessage()
    //{
    //    return _serviceWithoutConstructorParams.MessageFromThisService();
    //}

    //[HttpGet("ConvertService")]
    //public string ConvertService()
    //{
    //    return _fakeConvertService.Convert();
    //}
    #endregion
}
