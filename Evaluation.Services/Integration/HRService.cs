using Evaluation.DAL.DTOs;
using Evaluation.DAL.Entities.ActionEntities;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Extensions;
using Evaluation.Services.Mapping;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace Evaluation.Services.Integration;

public class HRService : ApiBase
{
    private readonly EmployeeService _employeeService;
    public HRService(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo, EmployeeService employeeService)
    : base(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
    {
        _employeeService = employeeService;
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
                            employees.Add(IntegrationMapping.MapToHREmployeeInfoDto(reader));
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
                            orgs.Add(IntegrationMapping.MapToOrganizationInfoDto(reader));
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
                            employees.Add(IntegrationMapping.MapToHREmployeeInfoDto(reader));
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
                var existEmployee = await _employeeService.GetEmployee(hrUser.SecEmail);
                if (existEmployee != null)
                {
                    existEmployee.Email = hrUser.SecEmail;
                    existEmployee.EmployeeNo = hrUser.EmpNo;
                    existEmployee.NameEn = hrUser.EngName;


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
                        JobTitleId = new Guid("49c138bf-05b0-49b1-a836-788057bb7004"),//Need review
                        OrgClassId = new Guid("49c138bf-05b0-49b1-a836-788057bb7004"),//Need review
                        OrgTypeId = new Guid("49c138bf-05b0-49b1-a836-788057bb7004"),//Need review
                        UserGenderId = new Guid("49c138bf-05b0-49b1-a836-788057bb7004"),//Need review
                    };

                    var newEmployee = await uow.GetRepository<Employee>().InsertAsync(emp);
                }
            }

            await uow.CommitAsync();

        }


        return true;
    }
}