using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Evaluation.Services.Models.Admin
{
    public class SrvUiControlBL : AdminBase
    {
        public SrvUiControlBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }
        public async Task<List<UiControlDTO>> GetUiControlList(AdminSearchDTO message)
        {
           
           

            var list = await uow.GetRepository<UiControl>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x=>x.CreateDate)
                .ToListAsync();

            if (!string.IsNullOrEmpty(message.PageName))
            {
                list = list.Where(c => c.PageName == message.PageName).ToList();
            }
            if (!string.IsNullOrEmpty(message.Title))
            {
                list = list.Where(c =>
    !string.IsNullOrEmpty(c.UserUiname) && c.UserUiname.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.ControlName) && c.ControlName.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.ValueEn) && c.ValueEn.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.ValueAr) && c.ValueAr.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.Url) && c.Url.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0
).ToList();

            }
            list = list.Skip(message.PageNum!.Value * message.PageSize!.Value).Take(message.PageSize.Value).ToList();
            var result = mapper.Map<List<UiControlDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;
            
        }
        public async Task<List<UiControl>> GetPageNameList(bool includeSubContent = false)
        {



            var result = await uow.GetRepository<UiControl>()
               .GetAllNonDeleted()
               .Include(x => x.CreateBy)
               .Select(x=>new UiControl
               {
                   PageName=x.PageName
               })
               .Distinct()
               .ToListAsync();

            return result.OrderByDescending(x => x.CreateDate).ToList();

        }

       
      
        public async Task<UiControlDTO> UpdateUiControl(UiControlDTO message)
        {
         
            
            var result = new UiControlDTO();
            
           
                
                
                if (message.Id is not null)
                {
                UiControl? Obj = await uow.GetRepository<UiControl>()
                                        .GetAllNonDeleted()
                                        .Where(x =>  x.Id==message.Id)
                                        .FirstOrDefaultAsync();
                if(Obj!=null)
                {
                    Obj.ControlName = message.ControlName??string.Empty;
                    Obj.ValueEn = message.EnValue;
                    Obj.ValueAr = message.ArValue;
                    Obj.Url = message.Url;
                    Obj.IsActive = message.IsActive;
                    uow.GetRepository<UiControl>().Update(Obj);
                    await uow.CommitAsync();
                    result = mapper.Map<UiControlDTO>(Obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                    result.UpdateBy = userInfo.DBName;
                    result.ResponseStatus = DBResult.Updated;

                }

               
                }

                return result;
           
        }
      
       
    }
}
