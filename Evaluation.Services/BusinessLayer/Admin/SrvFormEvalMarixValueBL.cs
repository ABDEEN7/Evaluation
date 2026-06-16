using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
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
    public class SrvFormEvalMarixValueBL : AdminBase
    {
        public SrvFormEvalMarixValueBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<FormEvalMarixValueDTO>> GetFormEvalMarixValueList(int Page, int PageSize)
        {



           

            var list = await uow.GetRepository<FormEvalMatrixValue>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result =  mapper.Map<List<FormEvalMarixValueDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
       
      
        public async Task<FormEvalMarixValueDTO> SaveFormEvalMarixValue(FormEvalMarixValueDTO message)
        {


            FormEvalMatrixValue obj = new FormEvalMatrixValue();

                obj.FormEvalMatrixId = message.FormEvalMatrixId;
                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.MinValue = message.MinValue;
                obj.MaxValue = message.MaxValue;
                obj.NextEvalDays = message.NextEvalDays;
                obj.ActualMatrixValue = message.ActualMatrixValue;
                obj.DescAr = message.DescAr;
                obj.DescEn = message.DescEn;
                obj.ColorCode = message.ColorCode;
                obj.IsActive = message.IsActive;

                uow.GetRepository<FormEvalMatrixValue>().Insert(obj);
            
            await uow.CommitAsync();
            var result = mapper.Map<FormEvalMarixValueDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
           
            
        }
        public async Task<FormEvalMarixValueDTO> UpdateFormEvalMarixValue(FormEvalMarixValueDTO message)
        {


           

            var result = new FormEvalMarixValueDTO();

            if (message.Id is not null)
            {
                FormEvalMatrixValue obj = await uow.GetRepository<FormEvalMatrixValue>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.FormEvalMatrixId = message.FormEvalMatrixId;
                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.MinValue = message.MinValue;
                obj.MaxValue = message.MaxValue;
                obj.DescAr = message.DescAr;
                obj.DescEn = message.DescEn;
                obj.NextEvalDays = message.NextEvalDays;
                obj.ActualMatrixValue = message.ActualMatrixValue;
                obj.ColorCode = message.ColorCode;
                obj.IsActive = message.IsActive;

                uow.GetRepository<FormEvalMatrixValue>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<FormEvalMarixValueDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }
                return result;
           
        }

        public async Task<bool> UpdateFormEvalMarixValueOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<FormEvalMatrixValue>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }

        public async Task<FormEvalMarixValueDTO> DeleteFormEvalMarixValue(Guid? Id)
        {



           
            var result = new FormEvalMarixValueDTO();
                if (Id is not null)
                {
                FormEvalMatrixValue obj = await uow.GetRepository<FormEvalMatrixValue>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                uow.GetRepository<FormEvalMatrixValue>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<FormEvalMarixValueDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
