using AutoMapper;
using Evaluation.DAL.Helper;
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
    public class SrvScopeTypeBL : AdminBase
    {
        public SrvScopeTypeBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }


        public async Task<List<ScopeTypeDTO>> GetScopeTypeList(int Page, int PageSize)
        {


            var list = await uow.GetRepository<ScopeType>()
                .GetAllNonDeleted()
                .Include(x => x.Parent)
                .Include(x => x.Department)
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<ScopeTypeDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
        public async Task<ScopeTypeDTO> SaveScopeType(ScopeTypeDTO message)
        {
            var BackendName = await GenerateBackendNameByTitle(message.NameEn);
            var existBackendName = await uow
             .GetRepository<FormEvalMatrix>()
                  .GetAllNonDeleted(x => x.BackenName == BackendName)
                  .FirstOrDefaultAsync();

            if (existBackendName != null)
            {
                message.ResponseStatus = DBResult.BackendExist;
                return message;
            }

            ScopeType obj = new ScopeType();

            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.BackendName = BackendName;
            obj.ParentId = message.ParentId;
            obj.DepartmentId = message.DepartmentId;

            uow.GetRepository<ScopeType>().Insert(obj);
            await uow.CommitAsync();
            var result = mapper.Map<ScopeTypeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            return result;


        }
        public async Task<ScopeTypeDTO> UpdateScopeType(ScopeTypeDTO message)
        {
            var result = new ScopeTypeDTO();

            if (message.Id is not null)
            {

                ScopeType obj = await uow.GetRepository<ScopeType>()
                                  .GetAllNonDeleted()
                                  .Include(x => x.CreateBy)
                                  .Where(x => x.Id == message.Id)
                                  .FirstAsync();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.ParentId = message.ParentId;
                obj.DepartmentId = message.DepartmentId;

                uow.GetRepository<ScopeType>().Update(obj);
                await uow.CommitAsync();
                result = mapper.Map<ScopeTypeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }

        public async Task<ScopeTypeDTO> DeleteScopeType(Guid? Id)
        {
            var result = new ScopeTypeDTO();
            if (Id is not null)
            {
                ScopeType obj = await uow.GetRepository<ScopeType>()
                                  .GetAllNonDeleted()
                                  .Where(x => x.Id == Id)
                                  .FirstAsync();
                if (obj == null)
                {
                    result.ResponseStatus = DBResult.NotFound;
                    return result;
                }

                var parentScopeType = await uow.GetRepository<ScopeType>()
                                         .GetAllNonDeleted()
                                         .Where(x => x.ParentId == obj.Id)
                                         .ToListAsync();

                if (parentScopeType.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ParentScopeTypeExistsScopeType);
                }

                uow.GetRepository<ScopeType>().Delete(obj);
                await uow.CommitAsync();
                result = mapper.Map<ScopeTypeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;

        }

    }
}
