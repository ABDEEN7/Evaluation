// Create main namespace if not exists
window.Evaluation = window.Evaluation || {};

// Create Loaders namespace
Evaluation.Loaders = (function () {
    var DepartmentRouting = sharedUtility().extractDepartmentName();
    const API_ENDPOINTS = {
        GET_Service_Status: `/ServiceRequest/${DepartmentRouting}/GetServiceStatus`,
        GET_Plans: `/Plan/${DepartmentRouting}/GetPlansDDL`,
        GET_Schools: `Schools/${DepartmentRouting}/GetSchoolsDDL`
    };
    function loadPlans(filterId) {
        const $ppl = $("#" + filterId);

        $ppl
            .empty()
            .append('<option value="">الكل</option>')
            .prop('disabled', true);

        jqClient()
            .Get(API_ENDPOINTS.GET_Plans)
            .done(function (response) {

                $ppl.prop('disabled', false);

                if (response) {
                    response.forEach(function (plans) {
                        $ppl.append(
                            `<option value="${plans.id}">${plans.planName}</option>`
                        );
                    });
                }
            })
            .fail(function (jqXHR, textStatus) {
                console.error('Failed to load plans:', textStatus);
                $ppl.prop('disabled', false);
            });
    }
    function loadServiceStatus(filterId) {
        const $ddl = $('#' + filterId);

        $ddl
            .empty()
            .append('<option value="">الكل</option>')
            .prop('disabled', true);

        jqClient()
            .Get(API_ENDPOINTS.GET_Service_Status)
            .done(function (response) {
                $ddl.prop('disabled', false);

                //if (response && response.result && response.result.length > 0) {
                if (response) {
                    response.forEach(function (status) {
                        $ddl.append(`
                            <option value="${status.id}">
                                ${status.nameAr}
                            </option>
                        `);
                    });
                }
            })
            .fail(function (jqXHR, textStatus) {
                console.error('Failed to load academic years:', textStatus);
                $ddl.prop('disabled', false);
            });
    }

    // public methods
    return {
        loadPlans: loadPlans,
        loadServiceStatus: loadServiceStatus
    };

})();