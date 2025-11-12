using Evaluation.DAL.DTOs;
using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Mapping;
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
    public HRService(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
    : base(serviceScopeFactory, cacheDataProvider, uow, loggingServices,  userInfo, serviceProvider, requestInfo)
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
}