using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AutoMapper;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.AutoMappers;

[AutoRegisterProfile]
public class ModelProfile : Profile
{
    public ModelProfile()
    {
        CreateMap<ModelBase, ModelDTO>();
        CreateMap<ModelDTO, ModelBase>();
    }
}
