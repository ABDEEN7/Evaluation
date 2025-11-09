using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.UserEntiy;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class  SrvUser(SrvDropdown SrvDropdown,  IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
           : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
        {
          
        public async Task<UserProfileCustomDTO?> GetApplicantStudent(ServiceRequest request)
        {
            string lang = requestInfo.Lang;

            if (request != null)
            {


                if (request.Student != null)
                {
                    var Nationality = await SrvAccreditedUniversity.GetCountryBycode(request.Student.NationalityCode);
                    return new UserProfileCustomDTO()
                    {
                        Email = request.Student.Email,
                        Name = lang == "ar"
                                            ? (string.IsNullOrEmpty(request.Student.FullNameAr) ? "" : request.Student.FullNameAr)
                                            : (string.IsNullOrEmpty(request.Student.FullNameEn) ? "" : request.Student.FullNameEn),
                        Mobile = request.Student.Mobile,
                        SecondMobile = request.Student.SecondMobile,
                        QID = request.Student.QID,
                        Nationality = Nationality != null
                                    ? (lang == "ar"
                                        ? (string.IsNullOrEmpty(Nationality.NameAr) ? "" : Nationality.NameAr)
                                        : (string.IsNullOrEmpty(Nationality.NameEn) ? "" : Nationality.NameEn))
                                    : "",
                        QIDExpiry = request.Student.QIDExpiryDate.ToString("yyyy-MM-dd"),  // Ensure full date format
                        DOB = request.Student.DOB.HasValue ? request.Student.DOB.Value.ToString("yyyy-MM-dd") : "", // Handle nullable DOB
                        Gender = lang == "ar"
                            ? (string.IsNullOrEmpty(request.Student.UserGender.TitleAr) ? "" : request.Student.UserGender.TitleAr)
                            : (string.IsNullOrEmpty(request.Student.UserGender.TitleEn) ? "" : request.Student.UserGender.TitleEn),
                    };

                }
                else
                    return null;
            }
            else
                return null;

        }

        public async Task<MinistryUser> GetByIDActiveNonDeleted(Guid userId)
        {

            var UserProfile = await serviceScopeFactory.CreateScopedUow()
                            .GetRepository<MinistryUser>().GetByIDActiveNonDeleted(userId);

            return UserProfile;
        }

        public async Task<MinistryUser> GetByStudentIDActiveNonDeleted(Guid userId)
        {

            var UserProfile = await serviceScopeFactory.CreateScopedUow()
                            .GetRepository<MinistryUser>().GetByIDActiveNonDeleted(userId);

            return UserProfile;
        }

        public async Task<Guid> GetUserGenderByuserId(Guid userId)
        {
            var UserProfile = await serviceScopeFactory.CreateScopedUow()
                            .GetRepository<MinistryUser>().GetByIDActiveNonDeleted(userId);
            if (UserProfile == null)
                throw new BusinessException(ConstantKeys.ExceptionMessage.UserInfoNotFound);
            return UserProfile.UserGenderId;
        }

        public async Task<bool> HasPermission(Guid userId, string permissionName)
        {
            // return  userInfo.PermissionList.Contains(permissionName);

            return await serviceScopeFactory.CreateScopedUow()
                            .GetRepository<UserRole>()
                            .GetAllQueryFiltered()
                            .Include(ur => ur.Role.RolePermission)
                            .AnyAsync(ur => ur.UserId == userId &&
                                            ur.Role.RolePermission.Any(rp => rp.Permission.BackendName == permissionName));
        }

        public async Task<MinistryUser?> GetStudentByIdAsync(Guid userId)
        {
            return await serviceScopeFactory.CreateScopedUow().GetRepository<MinistryUser>()
                .GetByIDActiveNonDeleted(userId);
        }

        public async Task<bool> UpdateUserPhotoAsync(Guid userId, string photo)
        {
            var user = await GetStudentByIdAsync(userId);
            if (user is null) return false;

            user.ProfilePhoto = photo;

            uow.GetRepository<MinistryUser>().Update(user);
            await uow.CommitAsync();
            return true;
        }
        public async Task<bool> UpdateUserMobileNumberAsync(Guid userId, string secondMobile)
        {
            var user = await GetStudentByIdAsync(userId);
            if (user is null) return false;

            user.SecondMobile = secondMobile;
            user.IsVerifiedSecondMobile = false;
            user.SecondMobileVerificationDate = null;

            uow.GetRepository<MinistryUser>().Update(user);
            await uow.CommitAsync();
            return true;
        }

        public async Task<PartyType?> GetFirstUserPartyTypeAsync( )
        {
            return await serviceScopeFactory.CreateScopedUow().GetRepository<PartyType>()
                .GetAllActiveNonDeleted()
                .AsNoTracking()
                .FirstOrDefaultAsync(pt => userInfo.PartyTypes.Contains(pt.Id));
        }

        public async Task<List<SelectListItemDTO>> GetMinistryUsersByModuleForScholarshipAsync(Guid moduleId, List<Guid> partyTypesIdsList)
        {
            string lang = requestInfo.Lang;
            var result = new List<SelectListItemDTO>();

            if (moduleId != Guid.Empty && partyTypesIdsList != null && partyTypesIdsList.Any())
            {
                var ministryUserIdsList = await serviceScopeFactory.CreateScopedUow().GetRepository<UserPartyType>()
                    .GetAllQueryFiltered(x => x.IsActive == true && partyTypesIdsList.Contains(x.PartyTypeId))
                    .Select(x => x.UserId)
                    .ToListAsync();

                var query =  serviceScopeFactory.CreateScopedUow().GetRepository<SchAssignment>()
                    .GetAll(x => x.IsActive == true)
                    .Include(x => x.PartyType)
                    .Include(x => x.MinistryUser)
                    .Include(x => x.Scholarship)
                    .Where(x => x.Scholarship.IsActive == true && x.Scholarship.IsDeleted == false)
                    .Where(x => x.PartyType.SystemModuleId == moduleId)
                    .Where(x => ministryUserIdsList.Contains(x.MinistryUserId.Value));

             
                result = (await query.Select(x => new { x.MinistryUser, x.Scholarship }).ToListAsync())
                    .GroupBy(x => x.MinistryUser)
                    .Select(x => new
                    {
                        Value = x.Key.Id,
                        Text = lang == "ar" ? x.Key.FullNameAr : x.Key.FullNameEn,
                        Count = x.Select(y => y.Scholarship).Distinct().Count(),
                    })
                    .Select(x => new SelectListItemDTO
                    {
                        Value = x.Value.ToString(),
                        Text = $"{SplitAssignedUserName(x.Text)} ({x.Count})"
                    })
                    .ToList();
            }

            return result;
        }
        public async Task<List<SelectListItemDTO>> GetMinistryUsersByDepartmentForRequestsAsync(Guid moduleId, List<Guid> partyTypesIdsList, string? StatusTypeId)
        {
            string lang = requestInfo.Lang;
            var result = new List<SelectListItemDTO>();

            if (moduleId != Guid.Empty && partyTypesIdsList != null && partyTypesIdsList.Any())
            {
                var ministryUserIdsList = await serviceScopeFactory.CreateScopedUow().GetRepository<UserPartyType>()
                    .GetAllQueryFiltered(x => x.IsActive == true && partyTypesIdsList.Contains(x.PartyTypeId))
                    .Select(x => x.UserId)
                    .ToListAsync();

                var query = serviceScopeFactory.CreateScopedUow().GetRepository<RequestAssignment>()
                    .GetAll(x => x.IsActive == true)
                    .Include(x => x.PartyType)
                    .Include(x => x.MinistryUser)
                    .Include(x => x.ServiceRequest)
                    .Include(x => x.ServiceRequest.Status)
                    .Where(x => x.ServiceRequest.IsActive == true && x.ServiceRequest.IsDeleted == false)
                    .Where(x => x.PartyType.SystemModuleId == moduleId)
                    .Where(x => ministryUserIdsList.Contains(x.MinistryUserId.Value));

                if (!string.IsNullOrEmpty(StatusTypeId))
                {
                    query = query.Where(x => StatusTypeId == "0" ? x.ServiceRequest.Status.IsOpen == false : x.ServiceRequest.Status.IsOpen == true);
                }

                result = (await query.Select(x => new { x.MinistryUser, x.ServiceRequest }).ToListAsync())
                    .GroupBy(x => x.MinistryUser)
                    .Select(x => new
                    {
                        Value = x.Key.Id,
                        Text = lang == "ar" ? x.Key.FullNameAr : x.Key.FullNameEn,
                        Count = x.Select(y => y.ServiceRequest).Distinct().Count(),
                    })
                    .Select(x => new SelectListItemDTO
                    {
                        Value = x.Value.ToString(),
                        Text = $"{SplitAssignedUserName(x.Text)} ({x.Count})"
                    })
                    .ToList();
            }

            return result;
        }

        private string SplitAssignedUserName(string assignedTo)
        {
            var output = string.Empty;
            var splitArray = assignedTo.Split(" ");
            if (splitArray.Count() >= 2)
            {
                output = $"{splitArray[0]} {splitArray[splitArray.Length - 1]}";
            }
            else
            {
                if (splitArray.Count() == 1)
                {
                    output = splitArray[0];
                }
            }

            return output;

        }

        public async Task<bool> IsUserEmailVerifiedAsync(Guid userProfileId)
        {
            var user = await serviceScopeFactory.CreateScopedUow().GetRepository<StudentUser>()
                .GetAllQueryFiltered()
                .Where(c => c.Id == userProfileId)
                .FirstOrDefaultAsync();

            if (user == null) { return true; } // Ministry User }
            else
                return user != null && user.IsEmailVerified;
        }


    }
}
