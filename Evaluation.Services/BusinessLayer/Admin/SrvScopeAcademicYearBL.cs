using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Evaluation.Services.Models.Admin
{
    public class SrvScopeAcademicYearBL : AdminBase
    {
        public SrvScopeAcademicYearBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }


        public async Task<List<GetScopeAcademicYearDTO>> GetScopeAcademicYearList(ScopeFormItemRequest request, int PageSize)
        {
            var list = uow.GetRepository<ScopeAcademicYear>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip((request.Page - 1) * PageSize)
                .Take(PageSize);
            if (request.DepartmentId.HasValue)
            {
                list = list.Where(x => x.DepartmentId == request.DepartmentId);
            }
            var result = await list.Select(x => new GetScopeAcademicYearDTO
            {
                Id = x.Id,
                AcademicYear = LanguageStatic.SelectLang(_requestInfo.Lang, x.AcademicYear.NameAr, x.AcademicYear.NameEn),
                Scope = LanguageStatic.SelectLang(_requestInfo.Lang, x.Scope.NameAr, x.Scope.NameEn),
                ScopeAcademicYearScopeParent = LanguageStatic.SelectLang(_requestInfo.Lang, x.ScopeParent.NameAr, x.ScopeParent.NameEn),
                Department = LanguageStatic.SelectLang(_requestInfo.Lang, x.Department!.NameAr, x.Department!.NameEn),
                CreateBy =
                LanguageStatic.SelectLang(_requestInfo.Lang, x.CreateBy.NameAr, x.CreateBy.NameEn),
                UpdateDate = x.UpdateDate.ToString(),
                UpdateBy =x.UpdateById != null ? LanguageStatic.SelectLang(_requestInfo.Lang,x.UpdateBy.NameAr,x.UpdateBy.NameEn) : LanguageStatic.SelectLang(_requestInfo.Lang, x.CreateBy.NameAr, x.CreateBy.NameEn),
                IsActive = x.IsActive,
                ScopeParentId = x.ScopeParentId,
                ScopeId = x.ScopeId,
                AcademicYearId = x.AcademicYearId,
                DepartmentId = x.DepartmentId,
                ScopeAcademicYearScopeParentId = x.ScopeParentId
            }).ToListAsync();
            return result;
        }

        public async Task<ScopeAcademicYearDTO> SaveScopeAcademicYear(ScopeAcademicYearDTO message)
        {



            ScopeAcademicYear obj = new ScopeAcademicYear();

            obj.ScopeId = message.ScopeId;
            obj.DepartmentId = message.DepartmentId;
            obj.AcademicYearId = message.AcademicYearId;
            obj.IsActive = message.IsActive;
            obj.ScopeParentId = message.ScopeAcademicYearScopeParentId;
            uow.GetRepository<ScopeAcademicYear>().Insert(obj);
            await uow.CommitAsync();
            obj = await uow.GetRepository<ScopeAcademicYear>()
                           .GetAllActiveNonDeleted()
                           .Include(x => x.Department)
                           .Include(x => x.Scope)
                           .Include(x => x.ScopeParent)
                           .Include(x => x.AcademicYear)
                           .FirstAsync(x => x.Id == obj.Id);

            var result = mapper.Map<ScopeAcademicYearDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            return result;

        }
        public async Task<ScopeAcademicYearDTO> UpdateScopeAcademicYear(ScopeAcademicYearDTO message)
        {




            var result = new ScopeAcademicYearDTO();

            if (message.Id is not null)
            {
                ScopeAcademicYear obj = await uow.GetRepository<ScopeAcademicYear>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Include(x => x.Department)
                                      .Include(x => x.Scope)
                                      .Include(x => x.ScopeParent)
                                      .Include(x => x.AcademicYear)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();


                obj.ScopeId = message.ScopeId;
                obj.DepartmentId = message.DepartmentId;
                obj.AcademicYearId = message.AcademicYearId;
                obj.IsActive = message.IsActive;
                obj.ScopeParentId = message.ScopeAcademicYearScopeParentId;

                uow.GetRepository<ScopeAcademicYear>().Update(obj);
                await uow.CommitAsync();
                result = mapper.Map<ScopeAcademicYearDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }

        public async Task<GenericResponse> DeleteScopeAcademicYear(Guid? Id)
        {
            if (Id is not null)
            {
                ScopeAcademicYear obj = await uow.GetRepository<ScopeAcademicYear>()
                                  .GetAllNonDeleted()
                                  .Where(x => x.Id == Id)
                                  .FirstAsync();

                uow.GetRepository<ScopeAcademicYear>().Delete(obj);
                await uow.CommitAsync();

            }
            var result = new GenericResponse
            {
                ResponseStatus = DBResult.Deleted
            };
            return result;
        }
        public async Task<bool> UpdateDepartmentOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<ScopeAcademicYear>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }

    }
}
