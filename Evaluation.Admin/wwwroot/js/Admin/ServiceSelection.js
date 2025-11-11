const SetDropdownAttribute = (dropdown) => {
    const selectedValue = $(dropdown).val();  // Get the selected value
    const dropdownId = $(dropdown).attr('id');  // Get the dropdown ID (optional)

    // Example: Update a linked textbox with the selected value
    const linkedTextbox = $(`input[data-control-id="${dropdownId}"]`);
    if (linkedTextbox.length) {
        linkedTextbox.val(selectedValue);
    }
}


const systemmodule_select2 = SystemModuleList.map(item => (
    {
        id: item.id,
        text: item.name
    }
));



$("#SystemModuleId").select2({
    width: '100%',
    allowClear: true,
    data: systemmodule_select2,
    dropdownCssClass: "manageselect2zindex",
    placeholder: selectPlaceHolder

});

const services_div_select2 = $("#ServiceId");
const systemmodule_div_select2 = $("#SystemModuleId");
if (systemmodule_select2.length > 1) {
    systemmodule_div_select2.val('').trigger('change');
}
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

$('#SystemModuleId').trigger('change');
$('#ServiceId').trigger('change');





