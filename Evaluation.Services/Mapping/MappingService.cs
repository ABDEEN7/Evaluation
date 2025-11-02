using System.Reflection;
using Mapster;
using MapsterMapper;

namespace Evaluation.Services.Mapping;

public class MappingService : IMappingService
{
    //private readonly IMapper _mapper;

    //public MappingService(IMapper mapster)
    //{
    //    var config = TypeAdapterConfig.GlobalSettings;
    //    config.Scan(Assembly.GetExecutingAssembly());

    //    _mapper = new Mapper(config);
    //}

    public TDestination Adapt<TDestination>(object source) => source.Adapt<TDestination>();
    public TDestination Adapt<TSource, TDestination>(TSource source) => source.Adapt<TDestination>();
    public TDestination Adapt<TSource, TDestination>(TSource source, TDestination destination) =>source.Adapt(destination);
}
