using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.UserEntiy;
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
    public class SrvUserPartyTypeBL : AdminBase
    {
        public SrvUserPartyTypeBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }

       
       
        public async Task<List<UserPartyTypeDTO>> GetUserPartyTypeList(AdminSearchDTO message)
        {

           

            var list = await uow.GetRepository<UserPartyType>()
                .GetAllNonDeleted()
                .Include(x => x.PartyType)
            .Include(x => x.User)
            .Include(x => x.CreateBy)
                .OrderByDescending(x=>x.CreateDate)
                .ToListAsync();
            if (message.DepartmentId != null)
            {
                list = list.Where(c => c.PartyType!.DepartmentId == message.DepartmentId).ToList();
            }
            if (!string.IsNullOrEmpty(message.UserName))
            {
                list = list.Where(c =>
    (!string.IsNullOrEmpty(c.User?.NameAr) && c.User.NameAr.IndexOf(message.UserName, StringComparison.OrdinalIgnoreCase) >= 0) ||
    (!string.IsNullOrEmpty(c.User?.NameEn) && c.User.NameEn.IndexOf(message.UserName, StringComparison.OrdinalIgnoreCase) >= 0) ||
    (!string.IsNullOrEmpty(c.User?.Email) && c.User.Email.IndexOf(message.UserName, StringComparison.OrdinalIgnoreCase) >= 0)
).ToList();
            }
            if (!string.IsNullOrEmpty(message.Title))
            {
                list = list.Where(c =>
    !string.IsNullOrEmpty(c.PartyType!.NameAr) &&
    c.PartyType.NameAr.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||

    !string.IsNullOrEmpty(c.PartyType.NameEn) &&
    c.PartyType.NameEn.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0
).ToList();


            }
            list = list.Skip(message.PageNum!.Value * message.PageSize!.Value).Take(message.PageSize.Value).ToList();
            var result = mapper.Map<List<UserPartyTypeDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;

        }
        public async Task<List<UserPartyTypeSignatureDTO>> GetUserPartyTypeSignatureList(Guid userpartytypeid)
        {


            var list = await uow.GetRepository<UserPartyTypeSignature>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
               .Where(x=>x.UserPartyTypeId==userpartytypeid)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            var result = mapper.Map<List<UserPartyTypeSignatureDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;

        }
        public async Task<List<MinistryUser>> GetAllUserList()
        {

            var result = await uow.GetRepository<MinistryUser>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .Select(x=>new MinistryUser()
                {
                    NameAr=x.Email+" _ "+x.NameAr,
                    NameEn=x.Email+" _ "+x.NameEn,
                    Id=x.Id
                })
                .ToListAsync();

            return result;

        }
        public async Task<List<DropdownItem>> GetDepartment()
        {



            var result = await uow.GetRepository<Department>()
               .GetAllNonDeleted()
               .Include(x => x.CreateBy)
               .OrderByDescending(x => x.CreateDate)
               .Distinct()
               .Select(x=>new DropdownItem
               {
                   Id=x.Id,
                   Text=(_requestInfo.Lang=="ar"?x.NameAr:x.NameEn)
               })
               .ToListAsync();

            return result;

        }
        public async Task<UserPartyTypeDTO> SaveUserPartyType(UserPartyTypeDTO message)
        {
           
            
            var exist=await uow.GetRepository<UserPartyType>() .GetAllNonDeleted().Where(x=>x.UserId==message.UserId && x.PartyTypeId==message.PartyTypeId).ToListAsync();
            if(exist.Count>0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.SameUserpartyTypeExists);
            }
            UserPartyType obj = new UserPartyType();
                obj.UserId = message.UserId;
                obj.PartyTypeId = message.PartyTypeId;
                obj.SignaturePlaceHolder = message.SignaturePlaceHolder;
                obj.IsActive = message.IsActive;

                uow.GetRepository<UserPartyType>().Insert(obj);
            

            await uow.CommitAsync();
            var result = mapper.Map<UserPartyTypeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.DepartmentId = await uow.GetRepository<PartyType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == obj.PartyTypeId)
                                      .Select(x => x.DepartmentId)
                                      .FirstAsync();
           
            result.ResponseStatus = DBResult.Inserted;
                return result;
            
        }
        public async Task<UserPartyTypeSignatureDTO> SaveUserPartyTypeSignature(UserPartyTypeSignatureDTO message)
        {
            var result =new UserPartyTypeSignatureDTO();
            if (message.Signature != null)
            {
                if (message.IsActive==true)
                {
                    var oldobj = await uow.GetRepository<UserPartyTypeSignature>() .GetAllNonDeleted().Where(x=>x.UserPartyTypeId==message.UserPartyTypeId).ToListAsync();
                    if (oldobj.Count > 0)
                    {
                        foreach (var item in oldobj)
                        {
                            item.IsActive = false;
                        }
                        uow.GetRepository<UserPartyTypeSignature>().UpdateRange(oldobj);
                    }
                }

                UserPartyTypeSignature obj = new UserPartyTypeSignature();
                obj.Signature = message.Signature;
                obj.UserPartyTypeId = message.UserPartyTypeId;
                obj.IsActive = message.IsActive;
                uow.GetRepository<UserPartyTypeSignature>().Insert(obj);
                await uow.CommitAsync();
                 result = mapper.Map<UserPartyTypeSignatureDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);

                result.ResponseStatus = DBResult.Inserted;
                
            }
            return result;

        }
        public async Task<UserPartyTypeSignatureDTO> UpdateUserPartyTypeSignature(UserPartyTypeSignatureDTO message)
        {
            var result =new UserPartyTypeSignatureDTO();
            if (message.Signature != null)
            {
                var oldobj = await uow.GetRepository<UserPartyTypeSignature>() .GetAllNonDeleted().Where(x=>x.Id==message.Id).FirstOrDefaultAsync();
                oldobj!.IsActive = message.IsActive;
                uow.GetRepository<UserPartyTypeSignature>().Update(oldobj);
                await uow.CommitAsync();
                result = mapper.Map<UserPartyTypeSignatureDTO>(oldobj, opts => opts.Items["Language"] = _requestInfo.Lang);

                result.ResponseStatus = DBResult.Updated;

            }
            return result;

        }
        public async Task<UserPartyTypeDTO> UpdateUserPartyType(UserPartyTypeDTO message)
        {

            var result = new UserPartyTypeDTO();

                if (message.Id is not null)
                {


                UserPartyType obj = await uow.GetRepository<UserPartyType>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                var exist=await uow.GetRepository<UserPartyType>() .GetAllNonDeleted().Where(x=>x.UserId==message.UserId && x.PartyTypeId==message.PartyTypeId && x.Id!=obj.Id).ToListAsync();
                if (exist.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SameUserpartyTypeExists);
                }

                obj.UserId = message.UserId;
                obj.PartyTypeId = message.PartyTypeId;
                obj.SignaturePlaceHolder = message.SignaturePlaceHolder;
                obj.IsActive = message.IsActive;
                uow.GetRepository<UserPartyType>().Update(obj);
                
                await uow.CommitAsync();
                result = mapper.Map<UserPartyTypeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.DepartmentId= await uow.GetRepository<PartyType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == obj.PartyTypeId)
                                      .Select(x=>x.DepartmentId)
                                      .FirstAsync();
               
                result.ResponseStatus = DBResult.Updated;
                }

                return result;
           
        }
        
        public async Task<UserPartyTypeDTO> DeleteUserPartyType(Guid? Id)
        {



           
            var result = new UserPartyTypeDTO();
                if (Id is not null)
                {
                UserPartyType obj = await uow.GetRepository<UserPartyType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                    uow.GetRepository<UserPartyType>().Delete(obj);

                var UserPartyTypeSignature = await uow.GetRepository<UserPartyTypeSignature>()
.GetAllNonDeleted()
                      .Where(x => x.UserPartyTypeId == obj.Id)
                      .ToListAsync();
                if (UserPartyTypeSignature.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.UserPartyTypeExistsUserPartyTypeSignature);
                }
                await uow.CommitAsync();
                result = mapper.Map<UserPartyTypeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }

        public async Task<UserPartyTypeSignatureDTO> DeleteUserPartyTypeSignature(Guid? Id)
        {




            var result = new UserPartyTypeSignatureDTO();
            if (Id is not null)
            {
                UserPartyTypeSignature obj = await uow.GetRepository<UserPartyTypeSignature>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                uow.GetRepository<UserPartyTypeSignature>().Delete(obj);
                await uow.CommitAsync();
                result = mapper.Map<UserPartyTypeSignatureDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;


        }

    }
}
