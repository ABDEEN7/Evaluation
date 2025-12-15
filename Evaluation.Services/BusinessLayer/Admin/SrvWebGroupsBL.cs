using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Website;
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
    public class SrvWebGroupsBL : AdminBase
    {
        public SrvWebGroupsBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<WebGroupsDTO>> GetWebGroupsList(int Page, int PageSize)
        {
           

            
           

            var list = await uow.GetRepository<WebGroup>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<WebGroupsDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
       
      
        public async Task<WebGroupsDTO> SaveWebGroups(WebGroupsDTO message)
        {
            
                
               
           
            var BackendName= await GenerateBackendNameByTitle(message.NameEn);
                var existBackendName = await uow
             .GetRepository<WebGroup>()
                  .GetAllNonDeleted(x => x.BackendName == BackendName)
                  .FirstOrDefaultAsync();

                if (existBackendName != null)
                {

                throw new BusinessException(ConstantKeys.ExceptionMessage.BackendNameAlreadyExists);
            }

            WebGroup obj = new WebGroup();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.RoutingPath = message.RoutingPath;
                obj.BackendName = BackendName;
                obj.IsActive = message.IsActive;

                uow.GetRepository<WebGroup>().Insert(obj);
            //insert values to DepWebGroup
            if (message.DepWebGroup != null)
            {
                List<DepWebGroup> objentitylist=new List<DepWebGroup>();
                foreach (var item in message.DepWebGroup)
                {
                    DepWebGroup objentity = new DepWebGroup();
                    objentity.DepartmentId = item;
                    objentity.WebGroupId = obj.Id;
                    objentity.IsActive = true;
                    objentitylist.Add(objentity);
                }
                if (objentitylist.Count > 0)
                {
                    await uow.GetRepository<DepWebGroup>().InsertRange(objentitylist);
                }


            }
            await uow.CommitAsync();
            var result = mapper.Map<WebGroupsDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                result.DepWebGroup = message.DepWebGroup;
                return result;
           
           
            
        }
        public async Task<WebGroupsDTO> UpdateWebGroups(WebGroupsDTO message)
        {
           
            
          
               
                var result = new WebGroupsDTO();

               
                WebGroup obj = await uow.GetRepository<WebGroup>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.RoutingPath = message.RoutingPath;
            obj.BackendName = obj.BackendName;
                obj.IsActive = message.IsActive;

                uow.GetRepository<WebGroup>().Update(obj);
            //update values to DepWebGroup
            List<DepWebGroup>  objDepWebGroupdelete = await uow.GetRepository<DepWebGroup>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.WebGroupId == obj.Id)
                                      .ToListAsync();

            var DepWebGroupexistids =new List<Guid>();
            if (objDepWebGroupdelete.Count > 0)
            {
                foreach (var item in objDepWebGroupdelete)
                {
                    if (message.DepWebGroup != null && message.DepWebGroup.Contains(item.DepartmentId))
                    {
                        DepWebGroupexistids.Add(item.DepartmentId);
                    }
                    else
                    {
                        uow.GetRepository<DepWebGroup>().Delete(item);
                    }

                }
            }
            if (message.DepWebGroup != null)
            {
                var notInSelected = message.DepWebGroup.Except(DepWebGroupexistids).ToList();
                List<DepWebGroup> objentitylist=new List<DepWebGroup>();
                foreach (var item in notInSelected)
                {
                    DepWebGroup objentity = new DepWebGroup();
                    objentity.WebGroupId = obj.Id;
                    objentity.DepartmentId = item;
                    objentity.IsActive = true;
                    objentitylist.Add(objentity);
                }
                if (objentitylist.Count > 0)
                {
                    await uow.GetRepository<DepWebGroup>().InsertRange(objentitylist);
                }


            }
            await uow.CommitAsync();
                result = mapper.Map<WebGroupsDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
                result.DepWebGroup = message.DepWebGroup;
           

                return result;
           
        }
        
        public async Task<WebGroupsDTO> DeleteWebGroups(Guid? Id)
        {

           

                
                var result = new WebGroupsDTO();
                if (Id is not null)
                {
                    WebGroup obj = await uow.GetRepository<WebGroup>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                var DepWebGroup = await uow.GetRepository<DepWebGroup>()
.GetAllNonDeleted()
                      .Where(x => x.WebGroupId == obj.Id)
                      .ToListAsync();
                if (DepWebGroup.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.WebGroupExistsDepWebGroup);
                }
                
                uow.GetRepository<WebGroup>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<WebGroupsDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
