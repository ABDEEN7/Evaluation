$(function () { 
    const ddlPlanType = 'ddlPlanType';
    const $ddlPlanType = $('#' + ddlPlanType);

    const getPlanTypes = () => {
        jqClient().Get('/Plan/GetPlanType').done((result) => {
            console.log("Plan types:", result);
            const data = (result && result.result) ? result.result : [];
            $ddlPlanType.select2({
                placeholder: "Select an option",
                allowClear: true,
                width: '100%',
                dropdownCssClass: "manageselect2zindex",
                data: data.map(item => ({ id: item.id, text: item.name }))
            });
            $ddlPlanType.val(null).trigger('change');
        }).fail((jqXHR, textStatus, err) => {
            console.error('GetPlanType failed', textStatus, err);
        });
    };

    getPlanTypes();
});

const CreateEvaluationPlan = (data) => {
    //if(!data.NameEn || !)
}
    //$(document).ready(function () {
    //    $('#confirmSaveBtn').on('click', function () {
    //        //const planData = 
    //    });

    //});
const CreatePlan = (data) => {
    return jqClient().Post(`/Plan/CreatePlan`, data).fail((jqXHR, textStatus, errorThrown) => {
        console.error('Error: [Create Plan Condition]', textStatus, errorThrown);
    });
}
