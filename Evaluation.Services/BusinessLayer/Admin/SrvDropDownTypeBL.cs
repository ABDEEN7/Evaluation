using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormBuilder;
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
    public class SrvDropDownTypeBL : AdminBase
    {
        private readonly CacheDataProvider _CacheDataProvider;
        public SrvDropDownTypeBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo, CacheDataProvider CacheDataProvider) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            _CacheDataProvider = CacheDataProvider;
        }


        public async Task<List<DropDownTypeDTO>> GetDropDownTypeList(Guid? parentid,int Page, int PageSize)
        {
           
            Guid specialGuid = new Guid("00000000-0000-0000-0000-000000000000");
            if (parentid.Equals(specialGuid))
            {
                parentid = null;
            }
            var list = await uow.GetRepository<DropDownType>()
                .GetAllNonDeleted()
               .Where(x => (parentid==null? x.ParentId== x.ParentId:x.ParentId==parentid))
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<DropDownTypeDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
        
        public async Task<DropDownTypeDTO> SaveDropDownType(DropDownTypeDTO message)
        {
           
            

            var BackendName= "DPT_"+await GenerateBackendNameByTitle(message.TitleEn);
            var existBackendName = await uow
             .GetRepository<DropDownType>()
                  .GetAllNonDeleted(x => x.BackendName == BackendName)
                  .FirstOrDefaultAsync();

            if (existBackendName != null)
            {

                message.ResponseStatus = DBResult.BackendExist;
                return message;
            }
            if (message.ParentId != null)
            {
                var parentexist = await uow.GetRepository<DropDownType>()
          .GetAllNonDeleted()
          .Where(x=>x.Id==message.ParentId)
          .AnyAsync();
                if (!parentexist)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ParentDoesNotExist);
                }
            }
            DropDownType obj = new DropDownType();

                obj.TitleAr = message.TitleAr;
                obj.TitleEn = message.TitleEn;
                obj.BackendName = BackendName;
                obj.ParentId = message.ParentId;
                obj.DataSourceTable = message.DataSourceTable;
                obj.IsActive = message.IsActive;
                uow.GetRepository<DropDownType>().Insert(obj);
                await uow.CommitAsync();
            var result = mapper.Map<DropDownTypeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
           // await _CacheDataProvider.ClearCacheByKey(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNTYPE);
            return result;
            
        }
        public async Task<DropDownTypeDTO> UpdateDropDownType(DropDownTypeDTO message)
        {
           
            
          
              
                var result = new DropDownTypeDTO();

                if (message.Id is not null)
                {
                   
                    DropDownType obj = await uow.GetRepository<DropDownType>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Include(x => x.Parent)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
                if (message.ParentId == obj.Id)
                {
                    result.ResponseStatus = DBResult.SameRecord;
                    return result;
                }
                if (message.ParentId != null)
                {
                    var parentexist = await uow.GetRepository<DropDownType>()
          .GetAllNonDeleted()
          .Where(x=>x.Id==message.ParentId)
          .AnyAsync();
                    if (!parentexist)
                    {
                        throw new BusinessException(ConstantKeys.ExceptionMessage.ParentDoesNotExist);
                    }
                }
                obj.TitleAr = message.TitleAr;
                obj.TitleEn = message.TitleEn;
                obj.BackendName = obj.BackendName;
                obj.ParentId = message.ParentId;
                obj.DataSourceTable = message.DataSourceTable;
                obj.IsActive = message.IsActive;
                uow.GetRepository<DropDownType>().Update(obj);
                    await uow.CommitAsync();
                result = mapper.Map<DropDownTypeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
               // await _CacheDataProvider.ClearCacheByKey(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNTYPE);
            }

                return result;
           
        }
      
        public async Task<DropDownTypeDTO> DeleteDropDownType(Guid? Id)
        {

           

              
                var result = new DropDownTypeDTO();
                if (Id is not null)
                {
                    DropDownType obj = await uow.GetRepository<DropDownType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                var objexist = await uow.GetRepository<FieldDropDownValue>()
                .GetAllNonDeleted()
                                      .Where(x => x.DropDownTypeId == obj.Id)
                                      .ToListAsync();
                if(objexist.Count>0)
                {
                    result.ResponseStatus = DBResult.Exist;
                    return result;
                }
                var parentexist = await uow.GetRepository<DropDownType>()
          .GetAllNonDeleted()
          .Where(x=>x.ParentId==obj.Id)
          .AnyAsync();
                if (parentexist)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.CannotDeleteItsParent);
                }
                var Field = await uow.GetRepository<Field>()
 .GetAllNonDeleted()
                       .Where(x => x.DropDownTypeId == obj.Id)
                       .ToListAsync();
                if (Field.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.DropdownTypeExistsField);
                }
               
                uow.GetRepository<DropDownType>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<DropDownTypeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                //await _CacheDataProvider.ClearCacheByKey(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNTYPE);
            }
                return result;
           

        }
       
    }
}
