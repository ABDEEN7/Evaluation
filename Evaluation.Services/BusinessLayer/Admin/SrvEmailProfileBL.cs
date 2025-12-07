using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Models.Template;
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
    public class SrvEmailProfileBL : AdminBase
    {
        

        public SrvEmailProfileBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }

        public async Task<List<EmailProfileDTO>> GetEmailProfileList(int Page, int PageSize)
        {

            var mapper = await CreateMapperForAdmin<EmailProfile, EmailProfileDTO>();
            var list = await uow.GetRepository<EmailProfile>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .Include(x => x.UpdateBy)
                 .Skip(Page*PageSize)
                .Take(PageSize)
            .ToListAsync();

            var result = mapper.Map<List<EmailProfileDTO>>(list);
            return result;

        }

        public async Task<EmailProfile> GetDefaultEmailProfile()
        {

            var result = await uow.GetRepository<EmailProfile>()
                .GetAllNonDeleted()
                .Where(x=>x.IsDefault==true)
                .Include(x => x.CreateBy)
                .Include(x => x.UpdateBy)
            .FirstOrDefaultAsync();

            
            return result?? new EmailProfile();

        }
        public async Task<EmailProfileDTO> SaveEmailProfile(EmailProfileDTO message)
        {
          
            
            var BackendName = await GenerateBackendNameByTitle(message.SenderDisplayName);
            var dbentity = await uow.GetRepository<EmailProfile>()
                 .GetAllNonDeleted().AsNoTracking()
                 .AnyAsync(x => x.BackendName == BackendName);

            if (dbentity)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.BackendNameAlreadyExists);
            }

            if (message.IsDefault)
            {
                var ExistData = await uow.GetRepository<EmailProfile>()
                     .GetAllNonDeleted().Where(x=>x.IsDefault==true)
                     .FirstOrDefaultAsync();

                if (ExistData!=null)
                {
                    ExistData.IsDefault = false;
                    uow.GetRepository<EmailProfile>().Update(ExistData);
                }
            }

            var mapper = await CreateMapperForAdmin<EmailProfile, EmailProfileDTO>();

            EmailProfile obj = new EmailProfile();

            obj.SenderAddress = message.SenderAddress;
            obj.SenderDisplayName = message.SenderDisplayName;
            obj.UserName = message.UserName;
            obj.Password = message.Password;
            obj.Host = message.Host;
            obj.Port = message.Port;
            obj.EnableSSL = message.EnableSSL;
            obj.UseDefaultCredentials = message.UseDefaultCredentials;
            obj.IsBodyHTML = message.IsBodyHTML;
            obj.IsDefault = message.IsDefault;
            obj.EmailRequestTimeout = message.EmailRequestTimeout;
            obj.BackendName = BackendName;
            obj.IsActive = message.IsActive;
            uow.GetRepository<EmailProfile>().Insert(obj);
            await uow.CommitAsync();
            var result = mapper.Map<EmailProfileDTO>(obj);
            result.UpdateBy = userInfo.DBName;
            result.ResponseStatus = DBResult.Inserted;
            return result;

        }
        public async Task<EmailProfileDTO> UpdateEmailProfile(EmailProfileDTO message)
        {

           

            if (message.IsDefault)
            {
                var ExistData = await uow.GetRepository<EmailProfile>()
                     .GetAllNonDeleted().Where(x=>x.IsDefault==true && x.Id != message.Id)
                     .FirstOrDefaultAsync();

                if (ExistData != null)
                {
                    ExistData.IsDefault = false;
                    uow.GetRepository<EmailProfile>().Update(ExistData);
                }
            }
            var EmailExists = await uow.GetRepository<EmailProfile>()
                 .GetAllNonDeleted().AsNoTracking()
                 .AnyAsync(x => x.SenderAddress == message.SenderAddress && x.Id != message.Id);

            if (EmailExists)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.EmailAlreadyExists);
            }

            var mapper = await CreateMapperForAdmin<EmailProfile, EmailProfileDTO>();
            var result = new EmailProfileDTO();

            if (message.Id is not null)
            {
                EmailProfile obj = await uow.GetRepository<EmailProfile>()
                                  .GetAllNonDeleted()
                                  .Include(x => x.CreateBy)
                                  .Where(x => x.Id == message.Id)
                                  .FirstAsync();

                obj.SenderAddress = message.SenderAddress;
                obj.SenderDisplayName = message.SenderDisplayName;
                obj.UserName = message.UserName;
                obj.Password = message.Password;
                obj.Host = message.Host;
                obj.Port = message.Port;
                obj.EnableSSL = message.EnableSSL;
                obj.UseDefaultCredentials = message.UseDefaultCredentials;
                obj.IsBodyHTML = message.IsBodyHTML;
                obj.IsDefault = message.IsDefault;
                obj.EmailRequestTimeout = message.EmailRequestTimeout;
                obj.IsActive = message.IsActive;
                uow.GetRepository<EmailProfile>().Update(obj);
                await uow.CommitAsync();
                result = mapper.Map<EmailProfileDTO>(obj);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }
        public async Task<bool> DeleteEmailProfile(Guid? Id)
        {

            var mapper = await CreateMapperForAdmin<EmailProfile, EmailProfileDTO>();
            var result = new EmailProfileDTO();
            if (Id is not null)
            {
                EmailProfile obj = await uow.GetRepository<EmailProfile>()
                                  .GetAllNonDeleted()
                                  .Where(x => x.Id == Id)
                                  .FirstAsync();
                var EmailTemplate = await uow.GetRepository<EmailTemplate>()
.GetAllNonDeleted()
                      .Where(x => x.EmailProfileId == obj.Id)
                      .ToListAsync();
                if (EmailTemplate.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.EmailProfileExistsEmailTemplate);
                }
                uow.GetRepository<EmailProfile>().Delete(obj);
                await uow.CommitAsync();
                result = mapper.Map<EmailProfileDTO>(obj);
                result.ResponseStatus = DBResult.Deleted;
            }
            return true;

        }

    }
}
