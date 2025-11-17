using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.BusinessLayer.API.SemesterLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.SteamerLayer;

public class SemesterRequestServices(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo,
    SemesterRepostiory semesterRepostiory
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo,
        serviceProvider, requestInfo)
{
    public async Task<Result<List<SemesterDto>>> GetSemestersTemplateAsync(Guid academicYearId)
    {
        return await ExecuteWithResult(async () =>
        {
            List<Semester> semesters = await semesterRepostiory.GetSemesters(academicYearId);
            List<SemesterDto> semesterDtos = new SemesterDto().ConvertSemesterToDto(semesters);
            return semesterDtos;
        });
    }
    public async Task<List<SemesterDto>> GetSemestersAsync(Guid academicYearId)
    {
            List<Semester> semesters = await semesterRepostiory.GetSemesters(academicYearId);
            List<SemesterDto> semesterDtos = new SemesterDto().ConvertSemesterToDto(semesters);
            return semesterDtos;
    }
}