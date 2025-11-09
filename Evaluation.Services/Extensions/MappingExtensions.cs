using Mapster;

namespace Evaluation.Services.Extensions;

public static class MappingExtensions
{
    public static TDestination AdaptTo<TDestination>(this object source)
    {
        return source.Adapt<TDestination>();
    }

    public static TDestination AdaptTo<TSource, TDestination>(this TSource source, TDestination destination)
    {
        return source.Adapt(destination);
    }

    public static List<TDestination> AdaptToList<TDestination>(this IEnumerable<object> source)
    {
        return source.Adapt<List<TDestination>>();
    }

    public static IEnumerable<TDestination> AdaptToEnumerable<TDestination>(this IEnumerable<object> source)
    {
        return source.Adapt<IEnumerable<TDestination>>();
    }

    public static IQueryable<TDestination> AdaptToQueryable<TDestination>(this IQueryable source)
    {
        return source.ProjectToType<TDestination>();
    }
}