using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AutoMapper;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.AutoMappers;

[AutoRegisterProfile]
public class NumbersProfile : Profile
{
    public NumbersProfile()
    {
        CreateMap<NumbersModel, IntDto>();
        CreateMap<NumbersModel, DoubleDto>();
        CreateMap<IntDto, NumbersModel>();
        CreateMap<DoubleDto, NumbersModel>();
    }
}
