using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.DepartmentHolidayLayer;

public class DepartmentHolidayBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, DepartmentHolidayService departmentHolidayService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<DepartmentHolidayDto>> GetDepartmentHolidayList(int page)
    {
        var result = await departmentHolidayService.GetDepartmentHolidayList(page);
        var response = mapper.Map<List<DepartmentHolidayDto>>(result, opts => opts.Items["Language"] = requestInfo.Lang);
        return response;
    }
}