using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace Evaluation.Services.Integration;

public class HRService: ApiBase
{
    public HRService(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
    : base(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
    {
    }
    public async Task<List<Employee>> GetAllHRUsersAsync(int skip = 0, int top = 50)
    {
        var employees = new List<Employee>();

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
                            var emp = new Employee
                            {
                                NameEn = reader.IsDBNull(reader.GetOrdinal("EMPLOYEE_E")) ? "" : reader.GetString(reader.GetOrdinal("EMPLOYEE_E")),
                                NameAr = reader.IsDBNull(reader.GetOrdinal("EMPLOYEE_A")) ? "" : reader.GetString(reader.GetOrdinal("EMPLOYEE_A")),
                                EmployeeNo = reader.IsDBNull(reader.GetOrdinal("EMPLOYEE_NUMBER")) ? "" : reader.GetString(reader.GetOrdinal("EMPLOYEE_NUMBER")),

                                //TODO: Need check here !!

                                // Nationality = reader.IsDBNull(reader.GetOrdinal("NATIONALITY_E")) ? "" : reader.GetString(reader.GetOrdinal("NATIONALITY_E")),
                                // JoinDate = reader.IsDBNull(reader.GetOrdinal("DATE_OF_JOINING")) ? "" : reader.GetDateTime(reader.GetOrdinal("DATE_OF_JOINING")),
                                // JobTitle = reader.IsDBNull(reader.GetOrdinal("JOB_TITLE_E")) ? reader.GetString(reader.GetOrdinal("JOB_TITLE_A")) : reader.GetString(reader.GetOrdinal("JOB_TITLE_E")),
                                //_obj.Gender = 
                                //_obj.BirthDate =
                                //_obj.HrCode =
                            };

                            employees.Add(emp);
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

    public async Task<List<OrgTree>> GetAllHROrgAsync(int skip = 0, int top = 50)
    {
        var employees = new List<OrgTree>();

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
                            var emp = new OrgTree
                            {
                                NameEn = reader.IsDBNull(reader.GetOrdinal("ORG_DESC_E")) ? "" : reader.GetString(reader.GetOrdinal("ORG_DESC_E")),
                                NameAr = reader.IsDBNull(reader.GetOrdinal("ORG_DESC_A")) ? "" : reader.GetString(reader.GetOrdinal("ORG_DESC_A")),
                                

                                //TODO: Need check here to add more!!

                            };

                            employees.Add(emp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading orgs: {ex.Message}");
            }
        }

        return employees;
    }
}