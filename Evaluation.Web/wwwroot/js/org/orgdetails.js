

async function GetSchoolDetails(guid) {
    return new Promise((resolve, reject) => {
         jqClient().Get(`/Org/${departmentRoutePath}/GetOrgDetails?OrgID=${guid}`)
            .done((res) => {
                console.log(res);
                resolve(res);
            }).fail((err) => {
                reject(err);
            });
    });
};

async function initSchoolDetailsPage(guid) {
    return await GetSchoolDetails(guid);
};