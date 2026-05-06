using AutoMapper;
using Evaluation.DAL.DTOs;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Extensions;
using Evaluation.Services.Mapping;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
using System.Text;
using System.Text.RegularExpressions;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.Services.Integration;

public class HRService : ApiBase
{
    private readonly EmployeeService _employeeService;
    public HRService(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo, EmployeeService employeeService)
    : base(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
    {
        _employeeService = employeeService;
    }

    public async Task<List<HREmployeeInfoDto>> GetAllHRUsersAsync(int page)
    {

        Int32.TryParse(await cacheDataProvider.GetSystemSettingValue(ConstantKeys.WebAppSettings.PAGE_SIZE), out int recordsPerPage);

        var top = recordsPerPage;
        var skip = (page - 1) * recordsPerPage;
        var employees = new List<HREmployeeInfoDto>();

        using (var con = new OracleConnection(ClsAppSetting.OracleDBConnection))
        {
            try
            {
                using (var cmd = con.CreateCommand())
                {
                    await con.OpenAsync();

                    cmd.BindByName = true;

                    cmd.CommandText = @"
                    SELECT *
                    FROM TEMP_HR.MOE_EMPLOYEES_EVALAPP_V
                    WHERE Email IS NOT NULL
                    OFFSET :Skip ROWS FETCH NEXT :Top ROWS ONLY";

                    cmd.Parameters.Add(new OracleParameter("Skip", skip));
                    cmd.Parameters.Add(new OracleParameter("Top", top));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employees.Add(reader.ToHREmployeeInfoDto());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading employees: {ex.Message}");
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }

        return employees;
    }

    public async Task<List<HROrganizationInfoDto>> GetAllHROrgAsync(int page)
    {
        Int32.TryParse(await cacheDataProvider.GetSystemSettingValue(ConstantKeys.WebAppSettings.PAGE_SIZE), out int recordsPerPage);

        var top = recordsPerPage;
        var skip = (page - 1) * recordsPerPage;
        var orgs = new List<HROrganizationInfoDto>();

        using (var con = new OracleConnection(ClsAppSetting.OracleDBConnection))
        {
            try
            {
                using (var cmd = con.CreateCommand())
                {
                    await con.OpenAsync();


                    cmd.BindByName = true;

                    cmd.CommandText = @"
                    SELECT *
                    FROM TEMP_HR.ORGANIZATION_EVALAPP_V
                    WHERE Email IS NOT NULL
                    OFFSET :Skip ROWS FETCH NEXT :Top ROWS ONLY";

                    cmd.Parameters.Add(new OracleParameter("Skip", skip));
                    cmd.Parameters.Add(new OracleParameter("Top", top));


                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orgs.Add(reader.ToOrganizationInfoDto());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading orgs: {ex.Message}");
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }

        return orgs;
    }

    public async Task<List<HROrganizationInfoDto>> GetAllHRSchoolsAsync()//(int page)
    {
        //Int32.TryParse(await cacheDataProvider.GetSystemSettingValue(ConstantKeys.WebAppSettings.PAGE_SIZE), out int recordsPerPage);

        //var top = recordsPerPage;
        //var skip = (page - 1) * recordsPerPage;
        var orgs = new List<HROrganizationInfoDto>();

        using (var con = new OracleConnection(ClsAppSetting.OracleDBConnection))
        {
            try
            {
                using (var cmd = con.CreateCommand())
                {
                    await con.OpenAsync();


                    cmd.BindByName = true;

                    cmd.CommandText = @"
                    SELECT *
                    FROM TEMP_HR.ORGANIZATION_EVALAPP_V
                    WHERE Email IS NOT NULL AND ORG_TYPE = 2";
                    //OFFSET :Skip ROWS FETCH NEXT :Top ROWS ONLY";

                    //cmd.Parameters.Add(new OracleParameter("Skip", skip));
                    //cmd.Parameters.Add(new OracleParameter("Top", top));


                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orgs.Add(reader.ToOrganizationInfoDto());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading orgs: {ex.Message}");
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }

        return orgs;
    }

    public async Task<List<HREmployeeInfoDto>> GetHRUsersAsync(long? qID = null, string? email = null, string? orgNo = null)
    {
        var employees = new List<HREmployeeInfoDto>();

        using (var con = new OracleConnection(ClsAppSetting.OracleDBConnection))
        {
            try
            {
                await con.OpenAsync();

                using (var cmd = con.CreateCommand())
                {
                    cmd.BindByName = true;

                    var query = new StringBuilder();
                    query.AppendLine("SELECT *");
                    query.AppendLine("FROM TEMP_HR.MOE_EMPLOYEES_EVALAPP_V");
                    query.AppendLine("WHERE Email IS NOT NULL");

                    // Add filters dynamically
                    if (qID.HasValue)
                        query.AppendLine("AND QID = :qID");
                    if (!string.IsNullOrWhiteSpace(email))
                        query.AppendLine("AND LOWER(Email) = LOWER(:Email)");
                    if (!string.IsNullOrWhiteSpace(orgNo))
                        query.AppendLine("AND OrgNo = :OrgNo");

                    cmd.CommandText = query.ToString();

                    // Add parameters safely
                    if (qID.HasValue)
                        cmd.Parameters.Add(new OracleParameter("qID", qID.Value));
                    if (!string.IsNullOrWhiteSpace(email))
                        cmd.Parameters.Add(new OracleParameter("Email", email));
                    if (!string.IsNullOrWhiteSpace(orgNo))
                        cmd.Parameters.Add(new OracleParameter("OrgNo", orgNo));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employees.Add(reader.ToHREmployeeInfoDto());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading employees: {ex.Message}");
            }
            finally
            {
                await con.CloseAsync();
                await con.DisposeAsync();
            }
        }

        return employees;
    }


    public async Task<List<HROrganizationInfoDto>> GetHROrgAsync(string? orgNo = null)
    {
        var employees = new List<HROrganizationInfoDto>();

        using (var con = new OracleConnection(ClsAppSetting.OracleDBConnection))
        {
            try
            {
                await con.OpenAsync();

                using (var cmd = con.CreateCommand())
                {
                    cmd.BindByName = true;

                    var query = new StringBuilder();
                    query.AppendLine("SELECT *");
                    query.AppendLine("FROM TEMP_HR.ORGANIZATION_EVALAPP_V");
                    query.AppendLine("WHERE Email IS NOT NULL");

                    // Add filters dynamically
                    if (!string.IsNullOrWhiteSpace(orgNo))
                        query.AppendLine("AND OrgNo = :OrgNo");

                    cmd.CommandText = query.ToString();

                    // Add parameters safely
                    if (!string.IsNullOrWhiteSpace(orgNo))
                        cmd.Parameters.Add(new OracleParameter("OrgNo", orgNo));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employees.Add(reader.ToOrganizationInfoDto());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Organizations: {ex.Message}");
            }
            finally
            {
                await con.CloseAsync();
                await con.DisposeAsync();
            }
        }

        return employees;
    }

    public async Task<bool> AddUpdateOrgTree(string? hrCode = null, long? qID = null)
    {
        var hrUsers = (await GetHRUsersAsync(qID, null, hrCode));

        if (hrUsers.Count == 0)
            return false;

        using (var uow = serviceScopeFactory.CreateScopedUow())
        {
            foreach (var hrUser in hrUsers)
            {
                Employee existEmployee = await _employeeService.GetEmployee(hrUser.IdNo);
                JobTitle jobTitle = await GetAndAddIfNotExistJobTitle(hrUser);
                OrgType orgType = await GetAndAddIfNotExistOrgType(hrUser);
                OrgClass orgClass = await GetAndAddIfNotExistOrgClass((await GetHROrgAsync(hrUser.OrgNo)).FirstOrDefault());
                if (existEmployee != null)
                {
                    existEmployee.Email = hrUser.SecEmail;
                    existEmployee.EmployeeNo = hrUser.EmpNo;
                    existEmployee.NameEn = hrUser.EngName;
                    existEmployee.QID = hrUser.IdNo;
                    existEmployee.BirthDate = DateOnly.FromDateTime(DateTime.Today);//TODO: Need to read DateOfBirth value from HR when be available
                    existEmployee.JoinDate = DateOnly.FromDateTime(DateTime.Parse(hrUser.JoiningDate));
                    existEmployee.NationalityCode = hrUser.NatCode;
                    existEmployee.JobTitleId = jobTitle.Id;
                    existEmployee.OrgTypeId = orgType.Id;
                    existEmployee.OrgClassId = orgClass.Id;
                    existEmployee.UserGenderId = new Guid("49c138bf-05b0-49b1-a836-788057bb7004");//TODO: Need to read Gender value from HR when be available
                    var updatedEmployee = uow.GetRepository<Employee>().Update(existEmployee);
                }
                else
                {
                    Employee emp = new Employee()
                    {
                        NameAr = hrUser.EmpName,
                        NameEn = hrUser.EngName,
                        EmployeeNo = hrUser.EmpNo,
                        Email = hrUser.SecEmail,
                        QID = hrUser.IdNo,
                        BirthDate = DateOnly.FromDateTime(DateTime.Today),//TODO: Need to read DateOfBirth value from HR when be available
                        JoinDate = DateOnly.FromDateTime(DateTime.Parse(hrUser.JoiningDate)),
                        NationalityCode = hrUser.NatCode,
                        JobTitleId = jobTitle.Id,
                        OrgClassId = orgClass.Id,
                        OrgTypeId = orgType.Id,
                        UserGenderId = new Guid("49c138bf-05b0-49b1-a836-788057bb7004"),//TODO: Need to read Gender value from HR when be available
                    };

                    var newEmployee = await uow.GetRepository<Employee>().InsertAsync(emp);
                }
            }

            await uow.CommitAsync();

        }


        return true;
    }
    public async Task<bool> AddUpdateAllSchools()
    {

        //////////////////CHECK WITH FATOUH///////////////////// 
        var OrgTypeBackend = await uow.GetRepository<SystemSetting>()
                                  .GetAllNonDeleted()
                                  .Where(x => x.SettingKey == AdminSettings.SchoolOrgType.ToString())
                                  .Select(x => x.SettingValue)
                                  .FirstOrDefaultAsync();
        if (OrgTypeBackend == null)
        {
            throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTypeDoesNotExists);
        }

        var orgtypeid = await uow.GetRepository<OrgType>()
                                 .GetAllNonDeleted()
                                 .Where(x => x.BackendName == OrgTypeBackend)
                                 .Select(x => x.Id)
                                 .FirstOrDefaultAsync();

        ////////////////////////CHECK WITH FATOUH////////////////////////
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        List<HROrganizationInfoDto> allHrSchools = await GetAllHRSchoolsAsync();

        if (allHrSchools.Count == 0)
            return false;

        using (var uow = serviceScopeFactory.CreateScopedUow())
        {
            List<School> schools = new List<School>();

            foreach (var school in allHrSchools)
            {

                School obj = new School();
                obj.NameAr = school.OrgDescA;
                obj.NameEn = school.OrgDescE;
                obj.OrgTypeId = orgtypeid;
                obj.OrgClassId = new Guid("3a1dac48-3d50-45a4-a1e9-10000c73a5b2");
                obj.EstablishmentDate = today;
                obj.TypeId = new Guid("11111111-1111-1111-1111-111111111111");
                obj.ManagerQID = school.ManagerIdNo;
                obj.ManageEmail = school.Email;
                obj.OrgEmail = school.SchoolEmail;
                obj.Address = school.Address;
                obj.Phone = school.Phone;
                obj.Mobile = school.Mobile;
                obj.IsAccredited = false;
                obj.SupportIdentity = false;
                obj.AcceditedDate = today;
                obj.SupportIdentityDate = today;
                obj.IsActive = true;
                obj.HrCode = school.OrgNo;

                schools.Add(obj);
            }

            var schoolsResult = await uow.GetRepository<School>().InsertRange(schools);


            await uow.CommitAsync();

        }
        return true;
    }

    private async Task<JobTitle> GetAndAddIfNotExistJobTitle(HREmployeeInfoDto hREmployeeInfoDto)
    {
        JobTitle existJobTitle = await _employeeService.GetJobTitle(hREmployeeInfoDto.JobNo);
        if (existJobTitle == null)
        {
            JobTitle newJobTitle = await uow.GetRepository<JobTitle>().InsertAsync(new JobTitle()
            { 
                HRCode = hREmployeeInfoDto.JobNo,
                BackendName = GenerateBackendName(hREmployeeInfoDto.JobNameE),
                NameAr = hREmployeeInfoDto.JobNameA,
                NameEn = hREmployeeInfoDto.JobNameE
            });
            await uow.CommitAsync();

            return newJobTitle;
        }
        return existJobTitle;
    }

    private async Task<OrgType> GetAndAddIfNotExistOrgType(HREmployeeInfoDto hREmployeeInfoDto)
    {
        OrgType existOrgType = await _employeeService.GetOrgType(hREmployeeInfoDto.OrgLocNo);
        if (existOrgType == null)
        {
            OrgType newOrgType = await uow.GetRepository<OrgType>().InsertAsync(new OrgType()
            {
                BackendName = hREmployeeInfoDto.OrgLocNo,
                NameAr = hREmployeeInfoDto.OrgLocDesc,
                NameEn = hREmployeeInfoDto.OrgLocDesc//TODO: Need to read English value from HR when be available
            });
            await uow.CommitAsync();

            return newOrgType;
        }
        return existOrgType;
    }
    private async Task<OrgClass> GetAndAddIfNotExistOrgClass(HROrganizationInfoDto hROrganizationInfoDto)
    {
        OrgClass existOrgClass = await _employeeService.GetOrgClass(hROrganizationInfoDto.OrgClass);
        if (existOrgClass == null)
        {
            OrgClass newOrgClass = await uow.GetRepository<OrgClass>().InsertAsync(new OrgClass()
            {
                HRCode = hROrganizationInfoDto.OrgClass,
                BackendName = hROrganizationInfoDto.OrgClass,//TODO: Need to read English value from HR when be available and use GenerateBackendName function to generate it
                NameAr = hROrganizationInfoDto.OrgClassDescA,
                NameEn = hROrganizationInfoDto.OrgClassDescA//TODO: Need to read English value from HR when be available
            });
            await uow.CommitAsync();

            return newOrgClass;
        }
        return existOrgClass;
    }

    private string GenerateBackendName(string titleEn) => Regex.Replace(titleEn, "[^a-zA-Z]", "");
}