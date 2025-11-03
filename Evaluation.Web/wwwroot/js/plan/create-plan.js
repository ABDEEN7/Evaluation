let ddlPlanType = 'ddlPlanType',
    $ddlPlanType = $('#' + ddlPlanType);

initTables = () => {
    getPlanTypes();
}
const getPlanTypes = () => {
    jqClient().Get('/Plan/GetPlanType').done((result) => {
        $ddlPlanType.select2({
            placeholder: "Select an options",
            allowClear: true,
            width: '100%',
            dropdownCssClass: "manageselect2zindex",
            data: result.value?.map((item) => ({ id: item.Id, text: item.name })) || []
        }).val('').trigger('change');
    });
}
const CreateEvaluationPlan = (data) => {
    //if(!data.NameEn || !)
}
getPlanTypes();
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
