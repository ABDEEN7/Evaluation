using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Evaluation.Services.Models.Admin
{
    public class SrvCssClassBL : AdminBase
    {
        public SrvCssClassBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<CssClassDTO>> GetCssClassList(int Page, int PageSize)
        {



           

            var list = await uow.GetRepository<CssClass>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result =  mapper.Map<List<CssClassDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
       
      
        public async Task<CssClassDTO> SaveCssClass(CssClassDTO message)
        {


            CssClass obj = new CssClass();

                obj.ClassName = message.ClassName;
                obj.Styles = message.Styles;
                obj.ApplyTypeId = message.ApplyTypeId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<CssClass>().Insert(obj);
            
            await uow.CommitAsync();
            var result = mapper.Map<CssClassDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
           
            
        }
        public async Task<CssClassDTO> UpdateCssClass(CssClassDTO message)
        {


           

            var result = new CssClassDTO();

            if (message.Id is not null)
            {
                CssClass obj = await uow.GetRepository<CssClass>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.ClassName = message.ClassName;
                obj.Styles = message.Styles;
                obj.ApplyTypeId = message.ApplyTypeId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<CssClass>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<CssClassDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }
                return result;
           
        }

       

        public async Task<CssClassDTO> DeleteCssClass(Guid? Id)
        {



           
            var result = new CssClassDTO();
                if (Id is not null)
                {
                CssClass obj = await uow.GetRepository<CssClass>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                uow.GetRepository<CssClass>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<CssClassDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
