using Evaluation.DAL.DTOs;
using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Evaluation.Services.Integration;

public class HRService: ApiBase
{
    public HRService(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
    : base(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
    {
    }

    int recordsPerPage = 50;//TODO: This value should be retrive from system settings table

    public async Task<List<HREmployeeInfoDto>> GetAllHRUsersAsync(int page)
    {
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
                            employees.Add(MapToHREmployeeInfoDto(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading employees: {ex.Message}");
            }
        }

        return employees;
    }

    public async Task<List<HROrganizationInfoDto>> GetAllHROrgAsync(int page)
    {
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
                            orgs.Add(MapToOrganizationInfoDto(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading orgs: {ex.Message}");
            }
        }

        return orgs;
    }

    private HROrganizationInfoDto MapToOrganizationInfoDto(OracleDataReader reader)
    {
        return new HROrganizationInfoDto
        {
            SecFlag = reader["sec_flag"] == DBNull.Value ? false : Convert.ToBoolean(reader["sec_flag"]),
            OrgNo = reader["orgno"]?.ToString(),
            OrgType = reader["org_type"]?.ToString(),
            OrgClass =  reader["org_class"]?.ToString(),
            OrgNoParent = reader["orgno_parent"]?.ToString(),
            ManagerIdNo =  reader["manager_idno"]?.ToString(),
            OrgSectorCode = reader["ORG_SECTOR_CODE"]?.ToString(),
            OrgSectorDesc = reader["org_sector_desc"]?.ToString(),
            OrgDescA = reader["org_desc_a"]?.ToString(),
            OrgDescE = reader["org_desc_e"]?.ToString(),
            OrgTypeDescA = reader["org_type_desc_a"]?.ToString(),
            OrgClassDescA = reader["org_class_desc_a"]?.ToString(),
            Address = reader["address"]?.ToString(),
            Email = reader["email"]?.ToString(),
            ManagerEngName = reader["ManagerNameE"]?.ToString(),
            ManagerAraName = reader["ManagerNameA"]?.ToString(),
        };
    }

    private HREmployeeInfoDto MapToHREmployeeInfoDto(OracleDataReader reader)
    {
        return new HREmployeeInfoDto
        {
            OrgSectorCode = reader["MINISTRY_SECTORS_CODE"]?.ToString(),
            OrgSectorDescA = reader["MINISTRY_SECTORS_A"]?.ToString(),
            OrgSectorDescE = reader["MINISTRY_SECTORS_E"]?.ToString(),
            OrgNo = reader["ORGNO"]?.ToString(),
            OrgDescA = reader["DEPARTMENT_A"]?.ToString(),
            OrgDescE = reader["DEPARTMENT_E"]?.ToString(),
            SubOrgNo = reader["SUBORGNO"]?.ToString(),
            SubOrgDescA = reader["SECTION_A"]?.ToString(),
            SubOrgDescE = reader["SECTION_E"]?.ToString(),
            EmpName = reader["EMPLOYEE_A"]?.ToString(),
            EngName = reader["EMPLOYEE_E"]?.ToString(),
            EmpNo = reader["EMPLOYEE_NUMBER"]?.ToString(),
            SecEmail = reader["EMAIL"]?.ToString(),
            JobNo = reader["JOBNO"]?.ToString(),
            JobNameA = reader["JOB_TITLE_A"]?.ToString(),
            JobNameE = reader["JOB_TITLE_E"]?.ToString(),
            MobileNo = reader["PHONE_NUMBER"]?.ToString(),
            IdNo = reader["QID"]?.ToString(),
            SecAppMdt = reader["DATE_OF_JOINING"]?.ToString(),
            OrgLocNo = reader["ORG_LOC_NO"]?.ToString(),
            OrgLocDesc = reader["ORG_LOC_DESC"]?.ToString(),
            Phone = reader["PHONE"]?.ToString(),
            ManagerId = reader["MANAGER_ID"]?.ToString(),
            ManagerName = reader["MANAGER_NAME_A"]?.ToString(),
            ManagerNameE = reader["MANAGER_NAME_E"]?.ToString(),
            ManagerMail = reader["MANAGER_MAIL"]?.ToString(),
            ManagerEmpNo = reader["MANAGER_EMPNO"]?.ToString(),
            Ht03Empst = reader["HT03_EMPST"] == DBNull.Value ? false : Convert.ToBoolean(reader["HT03_EMPST"]),
            EmpstDesc = reader["EMPST_DESC"]?.ToString(),
            EmpstDescE = reader["EMPST_DESC_E"]?.ToString(),
            SalStopDate = reader["SAL_STOP_DATE"]?.ToString(),
            GradeSlideNo = reader["GRADE_SLIDE_NO"]?.ToString(),
            GradeSlideDesc = reader["GRADE_SLIDE_DESC"]?.ToString(),
            GradeSlideDescE = reader["GRADE_SLIDE_DESC_E"]?.ToString(),
            NatCode = reader["NAT_CODE"]?.ToString(),
            Nationality = reader["NATIONALITY"]?.ToString(),
            NationalityE = reader["NATIONALITY_E"]?.ToString(),
            MarStat = reader["MARSTAT"]?.ToString(),
            MarStatDesc = reader["MARSTAT_DESC"]?.ToString(),
            MarStatDescE = reader["MARSTAT_DESC_E"]?.ToString(),
            EqLevelDescA = reader["EQLEVEL_DESC_A"]?.ToString(),
            EqLevelDescE = reader["EQLEVEL_DESC_E"]?.ToString(),
            CertNameA = reader["CERT_NAME_A"]?.ToString(),
            CertNameE = reader["CERT_NAME_E"]?.ToString(),
            SpcDescA = reader["SPC_DESC_A"]?.ToString(),
            SpcDescE = reader["SPC_DESC_E"]?.ToString(),
            ContractType = reader["CONTRACT_TYPE"]?.ToString(),
            ContractTypeDesc = reader["CONTRACT_TYPE_DESC"]?.ToString(),
            ContractTypeDescE = reader["CONTRACT_TYPE_DESC_E"]?.ToString(),
        };
    }

}