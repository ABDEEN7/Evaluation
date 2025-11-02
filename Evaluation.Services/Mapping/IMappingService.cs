namespace Evaluation.Services.Mapping;

public interface IMappingService
{
    TDestination Adapt<TDestination>(object source);
    TDestination Adapt<TSource, TDestination>(TSource source);
    TDestination Adapt<TSource, TDestination>(TSource source, TDestination destination);
}
