let orgDetails; 
//let departmentRoutePath = sharedUtility().extractDepartmentName();

const options = {
    success: function (result) {
        orgDetails = result;
    },
    error: function () {

    }
};

async function GetSchoolDetails(guid) {
    await jqClient(options).Get(`/Org/${departmentRoutePath}/GetOrgDetails?OrgID=${guid}`);

    console.log(orgDetails);
};

async function initSchoolDetailsPage(guid) {
    await GetSchoolDetails(guid);
};