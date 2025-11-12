using Evaluation.DAL.DTOs;
using Oracle.ManagedDataAccess.Client;

namespace Evaluation.Services.Mapping;

public static class IntegrationMapping
{
    public static HROrganizationInfoDto MapToOrganizationInfoDto(OracleDataReader reader)
    {
        return new HROrganizationInfoDto
        {
            SecFlag = reader["sec_flag"] == DBNull.Value ? false : Convert.ToBoolean(reader["sec_flag"]),
            OrgNo = reader["orgno"]?.ToString(),
            OrgType = reader["org_type"]?.ToString(),
            OrgClass = reader["org_class"]?.ToString(),
            OrgNoParent = reader["orgno_parent"]?.ToString(),
            ManagerIdNo = reader["manager_idno"]?.ToString(),
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

    public static HREmployeeInfoDto MapToHREmployeeInfoDto(OracleDataReader reader)
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
