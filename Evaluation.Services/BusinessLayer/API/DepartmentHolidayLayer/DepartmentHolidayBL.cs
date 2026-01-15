using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.AcademicYearLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.DepartmentHolidaysDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.DepartmentHolidayLayer;

public class DepartmentHolidayBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
            AcademicYearServices academicYearServices,
        IServiceProvider serviceProvider, RequestInfo requestInfo, DepartmentHolidayService departmentHolidayService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<DepartmentHolidayDto>> GetDepartmentHolidayList(int page)
    {
        var result = await departmentHolidayService.GetDepartmentHolidayList(page);
        var response = mapper.Map<List<DepartmentHolidayDto>>(result, opts => opts.Items["Language"] = requestInfo.Lang);
        return response;
    }
    public async Task<CreateDepartmentHolidayDto> AddDepartmentHoliday(CreateDepartmentHolidayDto model)
    {
        Guid academicYearId = await academicYearServices.GetAcademicYearId();
        DepartmentHoliday holiday = mapper.Map<DepartmentHoliday>(model);
        holiday.AcademicYearId = academicYearId;
        var result = await departmentHolidayService.InsertDepartmentHoliday(holiday);
        return model;
    }
    public async Task<UpdateDepartmentHolidayDto> UpdateDepartmentHoliday(UpdateDepartmentHolidayDto model)
    {
        Guid academicYearId = await academicYearServices.GetAcademicYearId();
        DepartmentHoliday departmentHoliday = await departmentHolidayService.GetDepartmentHoliday(model.Id);
        DepartmentHoliday holiday = mapper.Map<DepartmentHoliday>(model);
        var result = await departmentHolidayService.UpdateDepartmentHoliday(holiday);
        return model;
    }
    public async Task<bool> DeleteDepartmentHoliday(Guid holidayId)
    {
        Guid academicYearId = await academicYearServices.GetAcademicYearId();
        DepartmentHoliday departmentHoliday = await departmentHolidayService.GetDepartmentHoliday(holidayId);
        await departmentHolidayService.DeleteDepartmentHoliday(departmentHoliday);
        return true;
    }
}