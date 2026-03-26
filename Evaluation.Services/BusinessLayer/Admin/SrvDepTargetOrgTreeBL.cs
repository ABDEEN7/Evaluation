using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Org;
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
    public class SrvDepTargetOrgTreeBL : AdminBase
    {
        public SrvDepTargetOrgTreeBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<DepTargetOrgTreeDTO>> GetDepTargetOrgTreeList(int Page, int PageSize)
        {

            var list = await uow.GetRepository<DepTargetOrgTree>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<DepTargetOrgTreeDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
       
      
        public async Task<DepTargetOrgTreeDTO> SaveDepTargetOrgTree(DepTargetOrgTreeDTO message)
        {



            DepTargetOrgTree obj = new DepTargetOrgTree();

                obj.DepartmentId = message.DepartmentId;
                obj.CategoryId = message.CategoryId;
                obj.TargetOrgTreeId = message.TargetOrgTreeId;
                obj.PredicateFuncion = message.PredicateFuncion;
                obj.DepTargetConfig = message.DepTargetConfig;
                obj.IsActive = message.IsActive;

                uow.GetRepository<DepTargetOrgTree>().Insert(obj);
            
            await uow.CommitAsync();
            var result = mapper.Map<DepTargetOrgTreeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            return result;
           
           
            
        }
        public async Task<DepTargetOrgTreeDTO> UpdateDepTargetOrgTree(DepTargetOrgTreeDTO message)
        {


           

            var result = new DepTargetOrgTreeDTO();

            if (message.Id is not null)
            {
                DepTargetOrgTree obj = await uow.GetRepository<DepTargetOrgTree>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.DepartmentId = message.DepartmentId;
                obj.CategoryId = message.CategoryId;
                obj.TargetOrgTreeId = message.TargetOrgTreeId;
                obj.PredicateFuncion = message.PredicateFuncion;
                obj.DepTargetConfig = message.DepTargetConfig;
                obj.IsActive = message.IsActive;

                uow.GetRepository<DepTargetOrgTree>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<DepTargetOrgTreeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }
                return result;
           
        }

       

        public async Task<DepTargetOrgTreeDTO> DeleteDepTargetOrgTree(Guid? Id)
        {



           
            var result = new DepTargetOrgTreeDTO();
                if (Id is not null)
                {
                DepTargetOrgTree obj = await uow.GetRepository<DepTargetOrgTree>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                
                uow.GetRepository<DepTargetOrgTree>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<DepTargetOrgTreeDTO>(obj);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
