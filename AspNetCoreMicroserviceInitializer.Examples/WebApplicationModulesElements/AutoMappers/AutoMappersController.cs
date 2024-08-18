using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.AutoMappers;

[Route("api/[controller]")]
[ApiController]
public class AutoMappersController : ControllerBase
{
    private readonly IMapper _mapper;

    public AutoMappersController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost("GetDoubleModel")]
    public DoubleDto GetDoubleModel(NumbersModel numbersModel)
    {
        return _mapper.Map<DoubleDto>(numbersModel);
    }

    [HttpPost("GetIntModel")]
    public IntDto GetIntModel(NumbersModel numbersModel)
    {
        return _mapper.Map<IntDto>(numbersModel);
    }

    [HttpPost("GetNumbersModelByInt")]
    public NumbersModel GetNumbersModelByInt(IntDto intDto)
    {
        return _mapper.Map<NumbersModel>(intDto);
    }

    [HttpPost("GetNumbersModelByDouble")]
    public NumbersModel GetNumbersModelByDouble(DoubleDto doubleDto)
    {
        return _mapper.Map<NumbersModel>(doubleDto);
    }
}
