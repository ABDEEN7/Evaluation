let schoolDetails; 
const options = {
    success: function (result) {
     schoolDetails = result;
    },
    error: function () {

    }
};

const GetSchoolDetails = (guid) => {
    jqClient(options).Get(`/School/GetSchoolDetails?SchoolID=${guid}`);

    console.log(schoolDetails);
};

const initSchoolDetailsPage = (guid) => {
    GetSchoolDetails(guid);

};