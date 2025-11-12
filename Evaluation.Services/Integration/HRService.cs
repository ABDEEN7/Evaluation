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
using System.Text;

namespace Evaluation.Services.Integration;

public class HRService: ApiBase
{
    public HRService(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
    : base(serviceScopeFactory, cacheDataProvider, uow, loggingServices,  userInfo, serviceProvider, requestInfo)
    {
    }

    public List<Employee> GetAllHRUsers()
    {
        List<Employee> _Allobj = new List<Employee>();

        OracleConnection con = new OracleConnection(ClsAppSetting.OracleDBConnection);
        try
        {
            using (OracleCommand cmd = con.CreateCommand())
            {
                con.Open();

                cmd.BindByName = true;

                cmd.CommandText = "select * from TEMP_HR.MOE_EMPLOYEES_EVALAPP_V where Email is not null";
                //Execute the command and use DataReader to display the data
                OracleDataReader reader = cmd.ExecuteReader();
                var count = 0;
                while (reader.Read())
                {
                    Employee _obj = new Employee();
                    _obj.NameEn = reader.IsDBNull("EMPLOYEE_E") ? "" : reader.GetString("EMPLOYEE_E");
                    _obj.NameAr = reader.IsDBNull("EMPLOYEE_A") ? "" : reader.GetString("EMPLOYEE_A");
                    _obj.EmployeeNo = reader.IsDBNull("EMPLOYEE_NUMBER") ? "" : reader.GetString("EMPLOYEE_NUMBER");
                    //_obj.Gender = 
                    //_obj.BirthDate =
                    _obj.NationalityCode = reader.IsDBNull("NATIONALITY_E") ? "" : reader.GetString("NATIONALITY_E");
                    //_obj.JoinDate = reader.IsDBNull("DATE_OF_JOINING") ? "" : reader.GetString("DATE_OF_JOINING");
                    //_obj.JobTitle = reader.IsDBNull("JOB_TITLE_E") ? reader.GetString("JOB_TITLE_A") : reader.GetString("JOB_TITLE_E");
                    //_obj.HrCode =

                    _Allobj.Add(_obj);
                    count++;
                    if(count > 10)
                        break;
                }


                reader.Dispose();
                con.Dispose();
                con.Close();
                return _Allobj;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            con.Close();
        }
        return _Allobj;
    }
}
