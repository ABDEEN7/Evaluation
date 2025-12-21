using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.EvalResult;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Evaluation.Services.Models.Admin
{
    public class SrvOrgTreeBL : AdminBase
    {
        public SrvOrgTreeBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<OrgTreeDTO>> GetOrgTreeList(int Page, int PageSize)
        {



           

            var list = await uow.GetRepository<OrgTree>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<OrgTreeDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
       
      
        public async Task<OrgTreeDTO> SaveOrgTree(OrgTreeDTO message)
        {


           

            OrgTree obj = new OrgTree();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.OrgParentId = message.OrgParentId;
                obj.HrCode = message.HrCode;
                obj.NSISCode = message.NSISCode;
                obj.OrgTypeId = message.OrgTypeId;
                obj.OrgClassId = message.OrgClassId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<OrgTree>().Insert(obj);
           
            await uow.CommitAsync();
            var result = mapper.Map<OrgTreeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            return result;
           
           
            
        }
        public async Task<OrgTreeDTO> UpdateOrgTree(OrgTreeDTO message)
        {


            

            var result = new OrgTreeDTO();


            OrgTree obj = await uow.GetRepository<OrgTree>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
            if(obj!=null)
            {
                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.OrgParentId = message.OrgParentId;
                obj.HrCode = message.HrCode;
                obj.NSISCode = message.NSISCode;
                obj.OrgTypeId = message.OrgTypeId;
                obj.OrgClassId = message.OrgClassId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<OrgTree>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<OrgTreeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }

                return result;
           
        }

        public async Task<OrgTreeDTO> DeleteOrgTree(Guid? id)
        {
            if (id is null)
                return new OrgTreeDTO();

            var orgTree = await uow.GetRepository<OrgTree>()
        .GetAllNonDeleted()
        .FirstAsync(x => x.Id == id);

            await ValidateOrgTreeCanBeDeleted(orgTree.Id);

            uow.GetRepository<OrgTree>().Delete(orgTree);
            await uow.CommitAsync();

            var result = mapper.Map<OrgTreeDTO>(
        orgTree,
        opts => opts.Items["Language"] = _requestInfo.Lang);

            result.ResponseStatus = DBResult.Deleted;
            return result;
        }

        private async Task ValidateOrgTreeCanBeDeleted(Guid orgTreeId)
        {
            if (await uow.GetRepository<DepartmentOrgTree>()
                .GetAllNonDeleted()
                .AnyAsync(x => x.OrganizationTreeId == orgTreeId))
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTreeExistsDepartmentOrgTree);

            if (await uow.GetRepository<OrgEvalResult>()
                .GetAllNonDeleted()
                .AnyAsync(x => x.OrgTreeId == orgTreeId))
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTreeExistsOrgEvalResult);

            if (await uow.GetRepository<EvalAttachment>()
                .GetAllNonDeleted()
                .AnyAsync(x => x.OrgTreeId == orgTreeId))
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTreeExistsEvalAttachment);

            if (await uow.GetRepository<ServiceRequest>()
                .GetAllNonDeleted()
                .AnyAsync(x => x.OrgTreeId == orgTreeId))
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTreeExistsServiceRequest);

            if (await uow.GetRepository<EvaluationRequestHistory>()
                .GetAllNonDeleted()
                .AnyAsync(x => x.OrgTreeId == orgTreeId))
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTreeExistsEvaluationRequestHistory);

            if (await uow.GetRepository<OrgTree>()
                .GetAllNonDeleted()
                .AnyAsync(x => x.OrgParentId == orgTreeId))
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTreeExistsParentOrgTree);

            if (await uow.GetRepository<Employee>()
                .GetAllNonDeleted()
                .AnyAsync(x => x.Id == orgTreeId))
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTreeExistsEmployee);

            if (await uow.GetRepository<Organization>()
                .GetAllNonDeleted()
                .AnyAsync(x => x.Id == orgTreeId))
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTreeExistsOrganization);

            if (await uow.GetRepository<School>()
                .GetAllNonDeleted()
                .AnyAsync(x => x.Id == orgTreeId))
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTreeExistsSchool);

            if (await uow.GetRepository<Department>()
                .GetAllNonDeleted()
                .AnyAsync(x => x.TargetOrgTreeId == orgTreeId))
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTreeExistsDepartment);

            if (await uow.GetRepository<EvaluationRequest>()
                .GetAllNonDeleted()
                .AnyAsync(x => x.OrgTreeId == orgTreeId))
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTreeExistsEvaluationRequest);

            if (await uow.GetRepository<OrgAcademicYear>()
                .GetAllNonDeleted()
                .AnyAsync(x => x.OrgTreeId == orgTreeId || x.ParentOrgTreeId == orgTreeId))
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTreeExistsOrgAcademicYear);
        }

    }
}
