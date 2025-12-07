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
