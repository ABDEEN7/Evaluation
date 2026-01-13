using AutoMapper;
using Evaluation.DAL.Helper;
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
    public class SrvSchoolLevelBL : AdminBase
    {
        public SrvSchoolLevelBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<SchoolLevelDTO>> GetSchoolLevelList(int Page, int PageSize)
        {



            var list = await uow.GetRepository<SchoolLevel>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<SchoolLevelDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
       
      
        public async Task<SchoolLevelDTO> SaveSchoolLevel(SchoolLevelDTO message)
        {



           


            SchoolLevel obj = new SchoolLevel();

                obj.Year = message.Year;
                obj.SchoolId = message.SchoolId;
                obj.EducationLevelId = message.EducationLevelId;
                obj.NSISCode = message.NSISCode;
                obj.IsActive = message.IsActive;

                uow.GetRepository<SchoolLevel>().Insert(obj);
            
            await uow.CommitAsync();
            var result =mapper.Map<SchoolLevelDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
           
            
        }
        public async Task<SchoolLevelDTO> UpdateSchoolLevel(SchoolLevelDTO message)
        {


           

            var result = new SchoolLevelDTO();

            if (message.Id is not null)
            {
                SchoolLevel obj = await uow.GetRepository<SchoolLevel>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.Year = message.Year;
                obj.SchoolId = message.SchoolId;
                obj.EducationLevelId = message.EducationLevelId;
                obj.NSISCode = message.NSISCode;
                obj.IsActive = message.IsActive;

                uow.GetRepository<SchoolLevel>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<SchoolLevelDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }
                return result;
           
        }

       

        public async Task<SchoolLevelDTO> DeleteSchoolLevel(Guid? Id)
        {



            
            var result = new SchoolLevelDTO();
                if (Id is not null)
                {
                SchoolLevel obj = await uow.GetRepository<SchoolLevel>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

               
                uow.GetRepository<SchoolLevel>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<SchoolLevelDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
