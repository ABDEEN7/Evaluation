const SetDropdownAttribute = (dropdown) => {
    const selectedValue = $(dropdown).val();  // Get the selected value
    const dropdownId = $(dropdown).attr('id');  // Get the dropdown ID (optional)

    // Example: Update a linked textbox with the selected value
    const linkedTextbox = $(`input[data-control-id="${dropdownId}"]`);
    if (linkedTextbox.length) {
        linkedTextbox.val(selectedValue);
    }
}




const department_select2 = DepartmentList.map(item => (
    {
        id: item.id,
        text: item.name
    }
));
$("#DepartmentId").select2({
    width: '100%',
    allowClear: true,
    data: department_select2,
    dropdownCssClass: "manageselect2zindex",
    placeholder: selectPlaceHolder

});

$("#SystemModuleId").select2({
    width: '100%',
    allowClear: true,
    dropdownCssClass: "manageselect2zindex",
    placeholder: selectPlaceHolder

});

const services_div_select2 = $("#ServiceId");
const systemmodule_div_select2 = $("#SystemModuleId");
const department_div_select2 = $("#DepartmentId");
if (department_select2.length > 1) {
    department_div_select2.val('').trigger('change');
}
$('#DepartmentId').on('change', function () {

    var systemmodule_select2 = $.map(SystemModuleList, function (item, index) {

        const departmentId = $("#DepartmentId").val();
        if (item.departmentId == departmentId) {
            return {
                id: item.id,
                text: item.name
            };
        }
    });

    systemmodule_div_select2.empty();
    systemmodule_div_select2.select2({
        data: systemmodule_select2,
    });
    systemmodule_div_select2.val('').trigger('change');
});
$('#SystemModuleId').on('change', function () {

    var services_select2 = $.map(ServiceList, function (item, index) {

        const systemmoduleId = $("#SystemModuleId").val();
        if (item.systemModuleId == systemmoduleId) {
            return {
                id: item.id,
                text: item.name
            };
        }
    });

    services_div_select2.empty();
    services_div_select2.select2({
        data: services_select2,
    });
    services_div_select2.val('').trigger('change');
    if (`@Html.Raw(ShowServiceDefault)` == "False") {
        $('#ServiceId').val('').trigger('change');
    }
    if ($('#ServiceId').val() != null) {
        var isFreeze = ServiceList.filter(x => x.id == $('#ServiceId').val())[0].isFreez;
        $("#IsFreezeHidden").val(isFreeze);
    }
    SetDropdownAttribute(this);
});

services_div_select2.select2({
    width: '100%',
    allowClear: true,
    dropdownCssClass: "manageselect2zindex",
    placeholder: selectPlaceHolder,

}).on('change', function () {
    if ($('#ServiceId').val() != null) {
        var isFreeze = ServiceList.filter(x => x.id == $('#ServiceId').val())[0].isFreez;
        $("#IsFreezeHidden").val(isFreeze);
    }
    SetDropdownAttribute(this);

});

$('#DepartmentId').trigger('change');
$('#SystemModuleId').trigger('change');
$('#ServiceId').trigger('change');





