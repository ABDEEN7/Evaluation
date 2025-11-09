let schoolDetails; 
const options = {
    success: function (result) {
     schoolDetails = result;
    },
    error: function () {

    }
};

async function GetSchoolDetails(guid) {
    await jqClient(options).Get(`/School/GetSchoolDetails?SchoolID=${guid}`);

    console.log(schoolDetails);
};

async function initSchoolDetailsPage(guid) {
    await GetSchoolDetails(guid);
};