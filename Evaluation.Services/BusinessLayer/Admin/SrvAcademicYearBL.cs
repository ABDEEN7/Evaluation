using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Evaluation.Services.Models.Admin
{
    public class SrvAcademicYearBL : AdminBase
    {
        public SrvAcademicYearBL(IServiceProvider
            serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo
            , IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }


        public async Task<List<AcademicYearDTO>> GetAcademicYearList(int Page, int PageSize)
        {


            var list = await uow.GetRepository<AcademicYear>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<AcademicYearDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
        public async Task<List<AcademicYearDTO>> GetAcademicYearListByCureentDepartment()
        {
            var list = await uow.GetRepository<AcademicYear>()
                .GetAllNonDeleted(x => x.DepartmentId == _requestInfo.DepId)
                //.Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            var result = mapper.Map<List<AcademicYearDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
        public async Task<AcademicYearDTO> GetCurrentAcademicYearListByCureentDepartment()
        {
            var result = await uow.GetRepository<AcademicYear>()
                .GetAllNonDeleted(x => x.DepartmentId == _requestInfo.DepId)
                .OrderByDescending(x => x.CreateDate)
                .Select(x => new AcademicYearDTO { StartDate = x.StartDate, EndDate = x.EndDate })
                .FirstOrDefaultAsync();
            return result;
        }
        public async Task<AcademicYear> GetCurrentAcademicYear(Guid DepartmentId)
        {

            var result = await uow.GetRepository<AcademicYear>()
                .GetAllNonDeleted()
                .Where(x => x.IsCurrent == true && x.DepartmentId == DepartmentId)
                .Include(x => x.CreateBy)
                .Include(x => x.UpdateBy)
            .FirstOrDefaultAsync();


            return result ?? new AcademicYear();

        }
        public async Task<AcademicYearDTO> SaveAcademicYear(AcademicYearDTO message)
        {

            if (message.IsCurrent)
            {
                var OldAcademicYear =
                   await uow.GetRepository<AcademicYear>().GetAllNonDeleted().Where(x => x.IsCurrent == true && x.DepartmentId == message.DepartmentId)
                    .FirstOrDefaultAsync();
                if (OldAcademicYear != null)
                {
                    OldAcademicYear.IsCurrent = false;
                    uow.GetRepository<AcademicYear>().Update(OldAcademicYear);

                }
            }

            AcademicYear obj = new AcademicYear();

            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.StartDate = message.StartDate;
            obj.EndDate = message.EndDate;
            obj.Year = message.Year;
            obj.DepartmentId = message.DepartmentId;
            obj.IsCurrent = message.IsCurrent;
            obj.IsActive = message.IsActive;

            uow.GetRepository<AcademicYear>().Insert(obj);
            await uow.CommitAsync();
            var result = mapper.Map<AcademicYearDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            return result;

        }
        public async Task<AcademicYearDTO> UpdateAcademicYear(AcademicYearDTO message)
        {




            var result = new AcademicYearDTO();

            if (message.Id is not null)
            {
                AcademicYear obj = await uow.GetRepository<AcademicYear>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
                if (message.IsCurrent)
                {


                    var OldAcademicYear =
                   await uow.GetRepository<AcademicYear>().GetAllNonDeleted().Where(x => x.IsCurrent == true && x.Id != obj.Id && x.DepartmentId == message.DepartmentId)
                    .FirstOrDefaultAsync();
                    if (OldAcademicYear != null)
                    {
                        OldAcademicYear.IsCurrent = false;
                        uow.GetRepository<AcademicYear>().Update(OldAcademicYear);

                    }

                }

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.StartDate = message.StartDate;
                obj.EndDate = message.EndDate;
                obj.Year = message.Year;
                obj.DepartmentId = message.DepartmentId;
                obj.IsCurrent = message.IsCurrent;
                obj.IsActive = message.IsActive;

                uow.GetRepository<AcademicYear>().Update(obj);
                await uow.CommitAsync();
                result = mapper.Map<AcademicYearDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }

        public async Task<AcademicYearDTO> DeleteAcademicYear(Guid? Id)
        {




            var result = new AcademicYearDTO();
            if (Id is not null)
            {
                AcademicYear obj = await uow.GetRepository<AcademicYear>()
                                  .GetAllNonDeleted()
                                  .Where(x => x.Id == Id)
                                  .FirstAsync();
                var scopeAcademicYear = await uow.GetRepository<ScopeAcademicYear>()
 .GetAllNonDeleted()
                       .Where(x => x.AcademicYearId == obj.Id)
                       .ToListAsync();
                if (scopeAcademicYear.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.AcademicYearExistsScope);
                }
                uow.GetRepository<AcademicYear>().Delete(obj);
                await uow.CommitAsync();
                result = mapper.Map<AcademicYearDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;


        }

    }
}
