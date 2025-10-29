const GetSchoolDetails = () => {
    jqClient(options).Get(`/School/GetSchoolDetails?SchoolID=${guid}`);

};

const initSchoolDetailsPage = () => {
    GetSchoolDetails();
};