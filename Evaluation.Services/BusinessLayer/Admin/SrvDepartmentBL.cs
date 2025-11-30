using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
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
    public class SrvDepartmentBL : AdminBase
    {
        public SrvDepartmentBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<DepartmentDTO>> GetDepartmentList(int Page, int PageSize)
        {
           

            var list = await uow.GetRepository<Department>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<DepartmentDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
   
        public async Task<DepartmentDTO> SaveDepartment(DepartmentDTO message)
        {
           
           

            var BackendName= await GenerateBackendNameByTitle(message.NameEn);
            var existBackendName = await uow
             .GetRepository<Department>()
                  .GetAllNonDeleted(x => x.BackendName == BackendName)
                  .FirstOrDefaultAsync();

            if (existBackendName != null)
            {

                message.ResponseStatus = DBResult.BackendExist;
                return message;
            }

            Department obj = new Department();

            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.BackendName = BackendName;
            obj.RoutingPath = message.RoutingPath;
            obj.DepIcon = message.DepIcon;
            obj.TargetOrgTreeId = message.TargetOrgTreeId;
            obj.CategoryId = message.CategoryId;
            obj.IsNDA = message.IsNDA;
            obj.DescAr = message.DescAr;
            obj.DescEn = message.DescEn;
            obj.IsActive = message.IsActive;

            uow.GetRepository<Department>().Insert(obj);
                await uow.CommitAsync();
            var result = mapper.Map<DepartmentDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                return result;
            
        }
        public async Task<DepartmentDTO> UpdateDepartment(DepartmentDTO message)
        {
           
            
          
               
                var result = new DepartmentDTO();

                if (message.Id is not null)
                {
                    

                    Department obj = await uow.GetRepository<Department>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();


                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.BackendName = obj.BackendName;
                obj.RoutingPath = message.RoutingPath;
                obj.DepIcon = message.DepIcon;
                obj.TargetOrgTreeId = message.TargetOrgTreeId;
                obj.CategoryId = message.CategoryId;
                obj.IsNDA = message.IsNDA;
                obj.DescAr = message.DescAr;
                obj.DescEn = message.DescEn;
                obj.IsActive = message.IsActive;

                uow.GetRepository<Department>().Update(obj);
                    await uow.CommitAsync();
                result = mapper.Map<DepartmentDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
                }

                return result;
           
        }
        public async Task<bool> UpdateDepartmentOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<Department>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }
        public async Task<DepartmentDTO> DeleteDepartment(Guid? Id)
        {

           

               
                var result = new DepartmentDTO();
                if (Id is not null)
                {
                    Department obj = await uow.GetRepository<Department>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                var SystemModule = await uow.GetRepository<SystemModule>()
 .GetAllNonDeleted()
                       .Where(x => x.DepartmentId == obj.Id)
                       .ToListAsync();
                if (SystemModule.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.DepartmentExistsSystemModule);
                }

                var AcademicYear = await uow.GetRepository<AcademicYear>()
 .GetAllNonDeleted()
                       .Where(x => x.DepartmentId == obj.Id)
                       .ToListAsync();
                if (AcademicYear.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.DepartmentExistsAcademicYear);
                }
                uow.GetRepository<Department>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<DepartmentDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
