using AutoMapper;
using Evaluation.SharedHelper.Models;

namespace Evaluation.Services.MappingProfiles;

public class PaginatedResultProfile : Profile
{
    public PaginatedResultProfile()
    {
        CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResult<>))
            .ConvertUsing(typeof(PaginatedResultConverter<,>));
    }
}
public class PaginatedResultConverter<TSource, TDestination>(IMapper mapper) :
    ITypeConverter<PaginatedResult<TSource>, PaginatedResult<TDestination>>
{
    public PaginatedResult<TDestination> Convert(PaginatedResult<TSource> source,
        PaginatedResult<TDestination> destination,
        ResolutionContext context)
    {
        var mappedData = mapper.Map<List<TDestination>>(source.Items);
        return new PaginatedResult<TDestination>(
          mappedData,
          source.TotalCount,
          source.PageNumber,
          source.PageSize
      );
    }
}
