using System;
using System.Linq.Expressions;
using Mapster;
using MapsterMapper;

namespace Evaluation.Services.Extensions
{
    public class ServiceMapper : IMapper
    {
        public TypeAdapterConfig Config { get; }

        public ServiceMapper(TypeAdapterConfig config)
        {
            Config = config ?? TypeAdapterConfig.GlobalSettings;
        }

        public ITypeAdapterBuilder<TSource> From<TSource>(TSource source)
        {
            return source.BuildAdapter(Config);
        }

        public TDestination Map<TDestination>(object source)
        {
            return source.Adapt<TDestination>(Config);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return source.Adapt<TSource, TDestination>(Config);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return source.Adapt(destination, Config);
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return source.Adapt(sourceType, destinationType, Config);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return source.Adapt(destination, sourceType, destinationType, Config);
        }
    }
}