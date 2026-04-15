var maintable = null;
var actionconditiontable = null;
var popupname = "";
var ServiceFieldList = [];
var SystemFieldList = [];
var fieldselectvalue = '';
var actiontypebackendname = '';
var newstatusid = '';
function SetPopupCount() {
    // Get the Tabulator instance
    const tableInstance = maintable;

    if (tableInstance) {
        // Get the object from the Tabulator table data that matches the input value
        const targetId = $("#ActionStatusConfigurationNotificationConfigId").val();
        const targetObj = tableInstance.getData().find(f => f.id == targetId);

        if (targetObj) {
            // Update the notificationCount
            const tableCount = table.getDataCount();
            targetObj.notificationCount = tableCount;

            // Update the row data directly in Tabulator
            const row = tableInstance.getRow(targetId);
            if (row) {
                row.update({ notificationCount: tableCount });

                // Optional: Update the HTML manually if you have custom rendering
                const rowElement = row.getElement();
                const spanElements = rowElement.querySelectorAll("span.actionnotificationconfigcount");

                spanElements.forEach(span => {
                    $(span).text(tableCount);
                });
            }
        }
    }


}
$(document).ready(function () {
    $('.EvaluationActionDetails').hide();
    //Acton Config Table
    IsEdit = IsEdit_ActionStatusConfig;
    IsDelete = IsDelete_ActionStatusConfig;
    IsView = "";

    var viewItem = IsView_ActionStatusConfigNotification == "True" ?
        `<span class="Attr pointer" title="` + sharedFn().GetUiControlText('ADMIN_TOOLTIP_VIEW_NOTIFICATION') + `"><i class="Attr fa fa-bell" onclick="ActionStatusConfigClick(this)">
        <span class="actionnotificationconfigcount">0</span>
</i></span>`
        : '';
    let TableColumns = sharedFn().PopulateColumn(ActionStatusConfigcolumnList, viewItem);

    maintable = tableUtil.createTabulator({
        id: "ActionStatusConfigtable",
        config: {
            textDirection: txtDir,
            pagination: "local",
            paginationSize: 10,
            placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
            headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),
            movableRows: true,
        },
        uniqueRowId: 'id',
        sortColumn: "updateDate",
        sortDir: "desc",
        columns: TableColumns,
        rowFormatter: function () {
            var rows = maintable.getRows();

            rows.forEach(row => {
                // Get the row's HTML element
                var rowElement = row.getElement();
                var count = row.getData().notificationCount;

                // Get the first cell (first column)
                const spanElements = rowElement.querySelectorAll("span.actionnotificationconfigcount");

                spanElements.forEach(span => {
                    $(span).text(count);
                });
            });
        }

    });

    //Acton Condition
    IsEdit = IsView_ActionCondition;
    IsDelete = IsEdit_ActionCondition;
    IsView = "";

    let NewTableColumns = sharedFn().PopulateColumn(ActionConditioncolumnList);

    actionconditiontable = tableUtil.createTabulator({
        id: "ActionConditiontable",
        config: {
            textDirection: txtDir,
            pagination: "local",
            paginationSize: 10,
            placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
            headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),
            movableRows: true,
        },
        uniqueRowId: 'id',
        sortColumn: "updateDate",
        sortDir: "desc",
        columns: NewTableColumns,


    });


    let AttributeList = [];



    //Code start for Action Table

    document.getElementById("defaultOpen").click();




    const LoadAllEvaluationActions = () => {

        const serviceId = $("#ServiceId").val();
        if (serviceId) {
            const options = {
                success: function (data) {
                    if (data) {


                        $('#filteredList').html('');
                        if (data && data.length > 0) {
                            $.each(data, function (i, item) {
                                let li = `<li class="statuses-li list-group-item" id="${item.id}"><a href="javascript:void(0)">${lang == 'ar' ? item.nameAr : item.nameEn}</a></li>`;
                                $('#filteredList').append(li);


                                $(`#${item.id}`).click(function () {
                                    const id = $(this).attr("id");
                                    if (id) {
                                        // notificationUtil.success(id);
                                        LoadEvaluationActionDetails(id);
                                        LoadActionFieldTree(id);

                                    } else {
                                    }
                                });
                            });










                        }
                        else {
                            $('#filteredList').html('');
                        }
                    }
                }
            };
            jqClientAdvanced(options).Get(`ServiceAction/GetAllEvaluationActionList`.concat('?serviceId=', serviceId));
        } else {
        }
    }

    const DisableFormElements = (formId, disable = true) => {

        if (formId) {
            $('#' + formId + ' input, #' + formId + ' select').each(
                function (index) {
                    var input = $(this);
                    input.prop("disabled", disable);
                }

            );
            if (disable) {
                $(".dual-list-container").addClass("disabled-div");
            }
            else {
                $(".dual-list-container").removeClass("disabled-div");
            }
        }

    }

    const FilterEvaluationActions = (input) => {
        // Declare variables
        var filter = input.val().toUpperCase();
        var ul = $('#filteredList');
        var li = ul.find('li');

        // Loop through all list items, and hide those who don't match the search query
        li.each(function () {
            var a = $(this).find('a');
            var txtValue = a.text() || a.html();
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    }

    $('#searchTxt').on('input', function () {
        //FilterListItems('searchTxt', 'filteredList');

        FilterEvaluationActions($(this));
    });


    const LoadEvaluationActionDetails = (actionId) => {
        if (actionId) {
            const options = {
                success: function (result) {
                    if (result) {


                        FillForm(result.serviceAction);
                        InitializePartyTypes(result.serviceAction, result.partyTypesList, result.templateDocsList);
                        // DisableControls(true);
                        addStyleForTheSelectLi(actionId);

                        $('#CancelOrSave').hide();
                        $('#DeleteOrEdit').show();
                        DisableFormElements('UpdateEvaluationActionForm', true);

                    } else {
                        $('.EvaluationActionDetails').hide();
                    }
                }
            };
            jqClientAdvanced(options).Get(`ServiceAction/GetEvaluationActionDetails`.concat('?actionId=', actionId));

        }

    }

    const InitializePartyTypes = (EvaluationAction, partyTypesList, templateDocsList) => {


        const partyTypes_select2 = partyTypesList
            .map(item => ({
                id: item.id,
                text: lang == "ar" ? item.nameAr : item.nameEn
            }));

        const templateDocs_select2 = templateDocsList
            .map(item => ({
                id: item.id,
                text: lang == "ar" ? item.nameAr : item.nameEn
            }));



        $("#ActionTemplateDoc").empty();
        //ActionPartyType
        let uibackendName = "ActionPartyType";
        let mainuibackendName = "main_ActionPartyType";
        $("#" + mainuibackendName + " ul").empty();
        $("#" + uibackendName + " ul").empty();
        if (EvaluationAction.actionPartyTypeList.length > 0) {
            $.each(partyTypes_select2, function (index, dualistitem) {

                const exists = EvaluationAction.actionPartyTypeList.find(x => x == dualistitem.id);
                const listItem = "<li data-index='0' data-id='" + dualistitem.id + "'>" + dualistitem.text + "</li>";

                if (exists) {
                    $("#" + uibackendName + " ul").append(listItem);
                } else {
                    $("#main_" + uibackendName + " ul").append(listItem);
                }
            });
        }
        else {
            $.each(partyTypes_select2, function (index, item) {

                $("#" + mainuibackendName + " ul").append("<li data-index='0' data-id='" + item.id + "'>" + item.text + "</li>");
            });
            $("#" + uibackendName + " ul").empty();


        }

        //initialzing list select
        $("#" + mainuibackendName).on('click', 'li', function () {
            $(this).addClass('selected');
        });
        $("#" + uibackendName).on('click', 'li', function () {

            $(this).addClass("selected");
        });
        var rightbutton = "#" + uibackendName + "_rightbutton";
        var rightallbutton = "#" + uibackendName + "_rightallbutton";
        var leftbutton = "#" + uibackendName + "_leftbutton";
        var leftallbutton = "#" + uibackendName + "_leftallbutton";
        //initialzing button click 
        $(rightbutton).click(function (event) {
            $("#" + mainuibackendName + " li.selected").appendTo('#' + uibackendName + ' ul').removeClass('selected');
            event.preventDefault();
        });
        $(rightallbutton).click(function (event) {
            $('#' + mainuibackendName + ' li').appendTo('#' + uibackendName + ' ul').removeClass('selected');
            event.preventDefault();
        });
        $(leftbutton).click(function (event) {
            $("#" + uibackendName + " li.selected").appendTo('#' + mainuibackendName + ' ul').removeClass('selected');
            event.preventDefault();

        });
        $(leftallbutton).click(function (event) {
            $('#' + uibackendName + ' li').appendTo('#' + mainuibackendName + ' ul').removeClass('selected');
            event.preventDefault();
        });
        //AssignActionPartyType
        let uibackendName1 = "AssignActionPartyType";
        let mainuibackendName1 = "main_AssignActionPartyType";
        $("#" + mainuibackendName1 + " ul").empty();
        $("#" + uibackendName1 + " ul").empty();
        if (EvaluationAction.assignActionPartyTypeList.length > 0) {
            $.each(partyTypes_select2, function (index, dualistitem) {

                const exists = EvaluationAction.assignActionPartyTypeList.find(x => x == dualistitem.id);
                const listItem = "<li data-index='0' data-id='" + dualistitem.id + "'>" + dualistitem.text + "</li>";

                if (exists) {
                    $("#" + uibackendName1 + " ul").append(listItem);
                } else {
                    $("#main_" + uibackendName1 + " ul").append(listItem);
                }
            });
        }
        else {
            $.each(partyTypes_select2, function (index, item) {

                $("#" + mainuibackendName1 + " ul").append("<li data-index='0' data-id='" + item.id + "'>" + item.text + "</li>");
            });
            $("#" + uibackendName1 + " ul").empty();


        }


        //initialzing list select
        $("#" + mainuibackendName1).on('click', 'li', function () {
            $(this).addClass('selected');
        });
        $("#" + uibackendName1).on('click', 'li', function () {

            $(this).addClass("selected");
        });
        var rightbutton1 = "#" + uibackendName1 + "_rightbutton";
        var rightallbutton1 = "#" + uibackendName1 + "_rightallbutton";
        var leftbutton1 = "#" + uibackendName1 + "_leftbutton";
        var leftallbutton1 = "#" + uibackendName1 + "_leftallbutton";
        //initialzing button click 
        $(rightbutton1).click(function (event) {
            $("#" + mainuibackendName1 + "  li.selected").appendTo('#' + uibackendName1 + ' ul').removeClass('selected');
            event.preventDefault();
        });
        $(rightallbutton1).click(function (event) {
            $('#' + mainuibackendName1 + ' li').appendTo('#' + uibackendName1 + ' ul').removeClass('selected');
            event.preventDefault();
        });
        $(leftbutton1).click(function (event) {
            $("#" + uibackendName1 + " li.selected").appendTo('#' + mainuibackendName1 + ' ul').removeClass('selected');
            event.preventDefault();

        });
        $(leftallbutton1).click(function (event) {
            $('#' + uibackendName1 + ' li').appendTo('#' + mainuibackendName1 + ' ul').removeClass('selected');
            event.preventDefault();
        });

        //ActionShowLogPartyType
        let uibackendName2 = "ActionShowLogPartyType";
        let mainuibackendName2 = "main_ActionShowLogPartyType";
        $("#" + mainuibackendName2 + " ul").empty();
        $("#" + uibackendName2 + " ul").empty();
        if (EvaluationAction.actionShowLogPartyTypeList.length > 0) {
            $.each(partyTypes_select2, function (index, dualistitem) {

                const exists = EvaluationAction.actionShowLogPartyTypeList.find(x => x == dualistitem.id);
                const listItem = "<li data-index='0' data-id='" + dualistitem.id + "'>" + dualistitem.text + "</li>";

                if (exists) {
                    $("#" + uibackendName2 + " ul").append(listItem);
                } else {
                    $("#main_" + uibackendName2 + " ul").append(listItem);
                }
            });
        }
        else {
            $.each(partyTypes_select2, function (index, item) {

                $("#" + mainuibackendName2 + " ul").append("<li data-index='0' data-id='" + item.id + "'>" + item.text + "</li>");
            });
            $("#" + uibackendName2 + " ul").empty();


        }

        //initialzing list select
        $("#" + mainuibackendName2).on('click', 'li', function () {
            $(this).addClass('selected');
        });
        $("#" + uibackendName2).on('click', 'li', function () {

            $(this).addClass("selected");
        });
        var rightbutton2 = "#" + uibackendName2 + "_rightbutton";
        var rightallbutton2 = "#" + uibackendName2 + "_rightallbutton";
        var leftbutton2 = "#" + uibackendName2 + "_leftbutton";
        var leftallbutton2 = "#" + uibackendName2 + "_leftallbutton";
        //initialzing button click 
        $(rightbutton2).click(function (event) {
            $("#" + mainuibackendName2 + " li.selected").appendTo('#' + uibackendName2 + ' ul').removeClass('selected');
            event.preventDefault();
        });
        $(rightallbutton2).click(function (event) {
            $('#' + mainuibackendName2 + ' li').appendTo('#' + uibackendName2 + ' ul').removeClass('selected');
            event.preventDefault();
        });
        $(leftbutton2).click(function (event) {
            $("#" + uibackendName2 + " li.selected").appendTo('#' + mainuibackendName2 + ' ul').removeClass('selected');
            event.preventDefault();

        });
        $(leftallbutton2).click(function (event) {
            $('#' + uibackendName2 + ' li').appendTo('#' + mainuibackendName2 + ' ul').removeClass('selected');
            event.preventDefault();
        });


        $("#ActionTemplateDoc").select2({
            width: '100%',
            allowClear: true,
            multiple: true,
            data: templateDocs_select2,
            dropdownCssClass: "manageselect2zindex",
            placeholder: sharedFn().GetUiControlText('PleaseSelect'),
            //dropdownParent: $("#ModalPopup")

        });




        $("#ActionTemplateDoc").val(EvaluationAction.actionTemplateDocList);
        $("#ActionTemplateDoc").trigger('change');
        $("#EvaluationActionActionTypeId").trigger('change');

        const opt = {
            success: function (result) {
                if (result) {


                    var data = result.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.nameAr : item.nameEn
                        }
                    ));
                   
                    $("#EvaluationActionNewStatusId").select2({
                        width: 'resolve',
                        allowClear: true,
                        data: data,
                        placeholder: sharedFn().GetUiControlText('EvaluationActionNewStatusId'),
                        dropdownCssClass: "manageselect2zindex",
                    });
                   
                    if (newstatusid!='') {
                        $("#EvaluationActionNewStatusId").val(newstatusid).trigger('change');
                    }
                    else {
                        $("#EvaluationActionNewStatusId").val('').trigger('change');
                    }
                    

                }
            }
        };
        jqClientAdvanced(opt).Get("ServiceAction/GetNewStatusList");




    }
    $("#EvaluationActionActionTypeId").on("change", function () {
        actiontypebackendname = '';
        var thisvalue = this.value;
        $("#AssignActionPartyType").val('').trigger('change');
        if (thisvalue) {
            if (actionTypes.length > 0) {
                var type = actionTypes.find(x => x.id == thisvalue);
                if (type) {
                    actiontypebackendname = type.backendName;
                    if (type.backendName == "ASSIGN" || type.backendName == "APPROVE_AND_ASSIGN") {
                        $("#AssignActionPartyType").parent().parent().show();
                    }
                    else {
                        $("#AssignActionPartyType").parent().parent().hide();

                    }
                    if (type.backendName == "REQUEST_DATA_CHANGE" || type.backendName == "SUBMIT_MISSING_DATA") {
                        $("#defaultOpen").hide();
                        $("#EvaluationActionDtepDiv").hide();
                        SetActiveTabs();
                    }
                    else {
                        $("#defaultOpen").show();
                        $("#EvaluationActionDtepDiv").show();
                        SetActiveTabs();
                    }
                    if (type.backendName == "INFO_WITH_DRAFT") {
                        $('.ActionAllowDraftClass').show();
                    }
                    else {
                        if ($('#EvaluationActionAllowDraft').prop("checked")) {
                            $('.ActionAllowDraftClass').show();
                        }
                        else {
                            $('#EvaluationActionAllowDraft').prop("checked", false);
                            $('.ActionAllowDraftClass').hide();
                        }
                    }
                }
                else {
                    if ($('#EvaluationActionAllowDraft').prop("checked")) {
                        $('.ActionAllowDraftClass').show();
                    }
                    else {
                        $('#EvaluationActionAllowDraft').prop("checked", false);
                        $('.ActionAllowDraftClass').hide();
                    }
                    $("#AssignActionPartyType").parent().parent().hide();
                    $("#defaultOpen").show();
                    $("#EvaluationActionDtepDiv").show();
                    SetActiveTabs();

                }
            }


        }
        else {
            $("#AssignActionPartyType").parent().parent().hide();
            if ($('#EvaluationActionAllowDraft').prop("checked")) {
                $('.ActionAllowDraftClass').show();
            }
            else {
                $('#EvaluationActionAllowDraft').prop("checked", false);
                $('.ActionAllowDraftClass').hide();
            }
        }
    });
    $('#EvaluationActionIsInitialAction').on("change", function () {
        if (this.checked) {
            $('.ActionAllowDraftClass').show();
        }
        else {
            if (actiontypebackendname == "INFO_WITH_DRAFT") {
                $('.ActionAllowDraftClass').show();
            }
            else {
                $('#EvaluationActionAllowDraft').prop("checked", false);
                $('.ActionAllowDraftClass').hide();
            }

        }
    });
    const addStyleForTheSelectLi = (liId) => {

        if (liId) {

            ////remove style
            //$('#'.concat(liId, ' > a').attr('style', 'color: black; background-color:#f6f6f6;');

            var liList = $('#filteredList > li');

            if (liList.length > 0) {
                liList.each(function (index) {
                    var id = $(this).attr('id');
                    $('#'.concat(id, ' > a')).removeClass("active");//attr("style", 'color: black; background-color:#f6f6f6;');
                });
            }

            //add style
            $('#'.concat(liId, ' > a')).addClass('active');

        }


    }

    const FillForm = (obj) => {
        $('.EvaluationActionDetails').show();

        $('#EvaluationActionId').val(obj.id);

        $('#EvaluationActionNameEn').val(obj.nameEn);
        $('#EvaluationActionNameAr').val(obj.nameAr);
        $('#EvaluationActionIsConfirmationAction').prop('checked', obj.isConfirmationAction);
        $('#EvaluationActionIsConfirmationAction').trigger('change');

        $('#EvaluationActionActionTypeId').val(obj.actionTypeId);
        //$('#EvaluationActionSchStatusId').val(obj.schStatusId);
        $('#EvaluationActionActionTypeId').trigger('change');
        // $('#EvaluationActionSchStatusId').trigger('change');

        $('#EvaluationActionIsActive').prop('checked', obj.isActive);
        $('#EvaluationActionIsInitialAction').prop('checked', obj.isInitialAction);

        $('#EvaluationActionAllowDraft').prop('checked', obj.allowDraft);
        $('#EvaluationActionIsInitialAction').trigger('change');
        $('#EvaluationActionConfirmationBodyAr').val(obj.confirmationBodyAr);
        $('#EvaluationActionConfirmationBodyEn').val(obj.confirmationBodyEn);
        $('#EvaluationActionConfirmationTitleAr').val(obj.confirmationTitleAr);
        $('#EvaluationActionConfirmationTitleEn').val(obj.confirmationTitleEn);
        $('#EvaluationActionNewStatusId').val(obj.newStatusId);
        newstatusid = obj.newStatusId;
        $('#EvaluationActionNewStatusId').trigger('change');

        

    }


    $('#EvaluationActionIsConfirmationAction').change(function () {

        let checked = $(this).prop('checked');

        if (checked) {
            $('.ActionConfirmationClass').show();
        } else {
            $('.ActionConfirmationClass').hide();
        }



    });

    const EmptyForm = () => {

        $('#EvaluationActionId').val('');

        $('#EvaluationActionNameEn').val('');
        $('#EvaluationActionNameAr').val('');
        $('#EvaluationActionIsConfirmationAction').prop('checked', false);

        $('#EvaluationActionActionTypeId').val('');
        $('#EvaluationActionActionTypeId').trigger('change');

        ///$('#EvaluationActionSchStatusId').val('');
        // $('#EvaluationActionSchStatusId').trigger('change');

        $('#EvaluationActionIsActive').prop('checked', true);
        $('#EvaluationActionIsInitialAction').prop('checked', false);
        $('#EvaluationActionAllowDraft').prop('checked', false);
        $('#EvaluationActionIsInitialAction').trigger('change');

        $('#EvaluationActionConfirmationBodyAr').val('');
        $('#EvaluationActionConfirmationBodyEn').val('');
        $('#EvaluationActionConfirmationTitleAr').val('');
        $('#EvaluationActionConfirmationTitleEn').val('');
        $('#EvaluationActionNewStatusId').val('');
        newstatusid = '';
        $('#EvaluationActionNewStatusId').trigger('change');
    }


    const DisableAllElementsForTabulator = (tabulator, disable = true) => {



        if (tabulator) {

            if (disable) {
                tabulator.getRows().forEach(function (row) {


                    //row.getElement().classList.add("disabled-row");

                    row.getElement().style.pointerEvents = "none";
                    row.getElement().style.opacity = "0.6";
                });
            } else {
                tabulator.getRows().forEach(function (row) {

                    //row.getElement().classList.remove("disabled-row");

                    row.getElement().style.pointerEvents = "auto";
                    row.getElement().style.opacity = "1";
                });
            }
        }

    }

    //const services_select2 = services
    //    .map(item => ({
    //        id: item.id,
    //        text: lang == "ar" ? item.nameAr : item.nameEn
    //    }));


    //$("#ServiceId").select2({
    //    width: '100%',
    //    allowClear: false,
    //    data: services_select2,
    //    dropdownCssClass: "manageselect2zindex",
    //    placeholder: sharedFn().GetUiControlText('PleaseSelect'),
    //    //dropdownParent: $("#ModalPopup")

    //});



    $('#submitBtn').click(function () {
        LoadAllEvaluationActions();


    });

    const actionTypes_select2 = actionTypes
        .map(item => ({
            id: item.id,
            text: lang == "ar" ? item.nameAr : item.nameEn
        }));

    $("#EvaluationActionActionTypeId").select2({
        width: '100%',
        allowClear: false,
        data: actionTypes_select2,
        dropdownCssClass: "manageselect2zindex",
        placeholder: sharedFn().GetUiControlText('PleaseSelect'),
        // dropdownParent: $("#ModalPopup")

    });

    //const schStatus_select2 = schStatus
    //    .map(item => ({
    //        id: item.id,
    //        text: lang == "ar" ? item.nameAr : item.nameEn
    //    }));

    //$("#EvaluationActionSchStatusId").select2({
    //    width: '100%',
    //    allowClear: true,
    //    data: schStatus_select2,
    //    dropdownCssClass: "manageselect2zindex",
    //    placeholder: sharedFn().GetUiControlText('PleaseSelect'),
    //   // dropdownParent: $("#ModalPopup")
    //});


    $('#openEditModeForm').click(function () {

        $('#CancelOrSave').show();
        $('#DeleteOrEdit').hide();
        DisableFormElements('UpdateEvaluationActionForm', false);
        //DisableAllElementsForTabulator(tabulatorTable, false);

    });

    $('#cancelEvaluationAction').click(function () {
        $('.EvaluationActionDetails').hide();
        $('#CancelOrSave').hide();
        $('#DeleteOrEdit').show();
        DisableFormElements('UpdateEvaluationActionForm', true);
        //DisableAllElementsForTabulator(tabulatorTable, true);
    });

    $('#DeleteEvaluationAction').click(function () {
        notificationUtil.confirmation({
            title: sharedFn().GetUiControlText('ADMIN_WARNING_DELETE'),
            okText: sharedFn().GetUiControlText('DELETE_BUTTON'),
            cancelText: sharedFn().GetUiControlText('ADMIN_CANCEL')
        }, result => {
            var id = $('#EvaluationActionId').val();
            if (!id) return;
            const options = {
                success: function (result) {
                    if (result) {

                        $(`#${id}`).remove();
                        // table.deleteRow(id);

                        $('.EvaluationActionDetails').hide();

                        notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));

                    }
                }
            };
            jqClientAdvanced(options).Get("ServiceAction/DeleteEvaluationAction".concat('?Id=', id));

        });
    });

    $('#saveEvaluationAction').click(function () {
        var valid = false;

        if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {
            let listItemsActionPartyType = document.querySelectorAll("#ActionPartyType ul li");

            // Extract 'data-id' values and convert to an array
            let ActionPartyTypeArray = Array.from(listItemsActionPartyType).map(li => li.getAttribute("data-id"));
            let listItemsAssignActionPartyType = document.querySelectorAll("#AssignActionPartyType ul li");

            // Extract 'data-id' values and convert to an array
            let AssignActionPartyTypeArray = Array.from(listItemsAssignActionPartyType).map(li => li.getAttribute("data-id"));
            let listItemsActionShowLogPartyType = document.querySelectorAll("#ActionShowLogPartyType ul li");

            // Extract 'data-id' values and convert to an array
            let ActionShowLogPartyTypeArray = Array.from(listItemsActionShowLogPartyType).map(li => li.getAttribute("data-id"));
            let obj = {
                Id: $('#EvaluationActionId').val(),
                ServiceId: $('#ServiceId').val(),
                NameEn: $("#EvaluationActionNameEn").val(),
                NameAr: $("#EvaluationActionNameAr").val(),
                ActionTypeId: $("#EvaluationActionActionTypeId").val(),
                // SchStatusId: $("#EvaluationActionSchStatusId").val(),
                IsInitialAction: $("#EvaluationActionIsInitialAction").prop("checked"),
                AllowDraft: $("#EvaluationActionAllowDraft").prop("checked"),
                IsActive: $("#EvaluationActionIsActive").prop("checked"),
                IsConfirmationAction: $("#EvaluationActionIsConfirmationAction").prop("checked"),

                ConfirmationBodyAr: $("#EvaluationActionConfirmationBodyAr").val(),
                ConfirmationBodyEn: $("#EvaluationActionConfirmationBodyEn").val(),
                ConfirmationTitleAr: $("#EvaluationActionConfirmationTitleAr").val(),
                ConfirmationTitleEn: $("#EvaluationActionConfirmationTitleEn").val(),

                ActionPartyTypeList: ActionPartyTypeArray,
                AssignActionPartyTypeList: AssignActionPartyTypeArray,
                ActionShowLogPartyTypeList: ActionShowLogPartyTypeArray,
                ActionTemplateDocList: $("#ActionTemplateDoc").val(),
                NewStatusId: $("#EvaluationActionNewStatusId").val(),


            }
            var data = {
                action: obj,
                json: '',
            };

            var formData = new FormData();
            //debugger
            formData.append('request', JSON.stringify(data));

            const options = {

                success: function (result) {
                    if (result) {

                        // $('#CancelOrSave').hide();
                        // $('#DeleteOrEdit').show();

                        let item = result;
                        if (result.responseStatus == '2') { //update
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                            $(`#${item.id}`).html(`<a href="javascript:void(0)">${lang == 'ar' ? item.nameAr : item.nameEn}</a>`);
                        } else { //inserted
                            let li = `<li class="statuses-li list-group-item" id="${item.id}"><a href="javascript:void(0)">${lang == 'ar' ? item.nameAr : item.nameEn}</a></li>`;
                            $('#filteredList').prepend(li);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                            $(`#${item.id}`).click(function () {
                                const id = $(this).attr("id");
                                if (id) {
                                    // notificationUtil.success(id);
                                    LoadEvaluationActionDetails(id);
                                    LoadActionFieldTree(id);
                                } else {
                                }
                            });

                            LoadEvaluationActionDetails(item.id);

                        }

                        LoadActionFieldTree(item.id);

                        addStyleForTheSelectLi(item.id);
                        $('#CancelOrSave').hide();
                        $('#DeleteOrEdit').show();
                        // $('#EvaluationActionDetails').hide();
                        DisableFormElements('UpdateEvaluationActionForm', true);
                        //DisableAllElementsForTabulator(tabulatorTable, true);

                        //$('.messages').hide();
                        //$('.form-control').removeClass('error');

                    }
                },

            };

            if (data.action.Id) {
                jqClientAdvanced(options).PostFormData(`ServiceAction/UpdateEvaluationAction`, formData);

            } else {
                jqClientAdvanced(options).PostFormData(`ServiceAction/saveEvaluationAction`, formData);

            }
        }
        return;
    });




    $('#AddEvaluationAction').click(function () {

        let serviceId = $('#ServiceId').val();
        if (serviceId) {
            EmptyForm();
            $('.EvaluationActionDetails').show();
            $('#EvaluationActionsDivId').hide();
            $('#CancelOrSave').show();
            $('#DeleteOrEdit').hide();
            DisableFormElements('UpdateEvaluationActionForm', false);


            //model.PartyTypesList = await masterBL.GetAdminService < SrvEvaluationActionBL > ().GetPartyTypesList();
            //model.TemplateDocsList = await masterBL.GetAdminService < SrvEvaluationActionBL > ().GetTemplateDocsList();


            var EvaluationAction = {};
            EvaluationAction.actionPartyTypeList = [];
            EvaluationAction.assignActionPartyTypeList = [];
            EvaluationAction.actionShowLogPartyTypeList = [];
            EvaluationAction.actionTemplateDocList = [];

            const options = {

                success: function (result) {
                    if (result) {
                        //debugger

                        let partyTypesList = result.filter(c => c.type == 'PartyType');
                        let templateDocsList = result.filter(c => c.type == 'TemplateDoc');
                        InitializePartyTypes(EvaluationAction, partyTypesList, templateDocsList);

                        $('#EvaluationActionIsConfirmationAction').trigger('change');
                    }
                },
            };

            jqClientAdvanced(options).Get(`ServiceAction/GetTemplateDocs_PartyTypes_List`.concat('?serviceId=', serviceId));

            //$('.messages').hide();
            //$('.form-control').removeClass('error');

        }
    });

    $('#ServiceId').trigger('change');




    //----------------------------------------------------------------------------

    $('#ServiceId').on('change', function () {


    });


    var treeTabulator;
    const LoadActionFieldTree = (actionId) => {

        if (actionId) {
            if (IsView_ActionStatusConfig == 'True') {
                LoadAllActionStatusConfiguration(actionId);
            }
            if (IsView_ActionCondition == 'True') {
                LoadAllActionCondition(actionId);
            }

            const options = {

                success: function (result) {
                    if (result) {
                        //debugger

                        var treeConfig = {
                            container: '#tree-container',
                            data: result.formGroups,
                            searchInput: '#tree-search',
                            multipleSelection: true,

                        };

                        var tabulatorConfig = {
                            container: '#table',
                            useTabulator: true,
                            columns: [
                                { title: sharedFn().GetUiControlText('lblActionFieldname'), field: 'text', maxWidth: 250, minWidth: 250 },
                                { title: sharedFn().GetUiControlText('lblActionFieldIsEditable'), field: 'isEditable', formatter: "tickCross", editor: true, maxWidth: 100, minWidth: 100 },
                                { title: sharedFn().GetUiControlText('lblActionFieldIsActive'), field: 'isActive', formatter: "tickCross", editor: true, maxWidth: 100, minWidth: 100 },
                                {
                                    title: "Actions",
                                    field: "actionFieldId",
                                    maxWidth: 100,
                                    minWidth: 100,
                                    formatter: function (cell) {
                                        var value = cell.getValue();
                                        return value != null ? `<span class="Attr pointer" title="` + sharedFn().GetUiControlText('ADMIN_TOOLTIP_VIEW_ATTRIBUTE') + `"><i class="Attr fa fa-clipboard">
       
</i></span>`: '';
                                    },
                                    cellClick: function (e, cell) {
                                        var actionfieldid = cell.getValue();
                                        var fieldid = cell._cell.row.data.id;
                                        if (actionfieldid != null) {
                                            IsEdit = IsEdit_ActionFieldAttribute;
                                            IsDelete = IsDelete_ActionFieldAttribute;
                                            IsView = '';
                                            containsOrderNo = 'False';
                                            var ActionFieldAttributetabulatorcolumns = sharedFn().PopulateColumn(ActionFieldAttributecolumnList);
                                            var modaltitle = sharedFn().GetUiControlText('ActionFieldAttributeHeader'); sharedFn().OpenFormPopup(modaltitle, ActionFieldAttributecontrolvalidationlist, null, ActionFieldAttributetabulatorcolumns, settingList);
                                            $("#fieldid").val(fieldid);
                                            $("#actionfieldid").val(actionfieldid);
                                            popupname = "ActionFieldAttribute";
                                        }

                                    }
                                }
                            ],
                            height: '400px',
                            groupBy: 'parentId',
                            groupHeader:
                                function (value, count, data, group) {
                                    let rowData = data[0];
                                    return rowData.parentName + " <span style='margin-left:10px; color:#999;'>(" + count + " " + sharedFn().GetUiControlText('lblitems') + ")</span>";
                                },
                            searchInput: '#table-search',
                            placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
                        };



                        treeTabulator = new TreeWithTabulator(treeConfig, tabulatorConfig);


                    }
                },
            };

            jqClientAdvanced(options).Get(`ServiceAction/GetActionFieldTree`.concat('?actionId=', actionId));
        }



    }







    $('#saveActionFieldBtnId').click(function () {
        let actionId = $('#EvaluationActionId').val();

        if (actionId) {


            const options = {

                success: function (result) {
                    if (result) {
                        LoadActionFieldTree(actionId);
                        notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                    }
                },
            };
            let tableData = treeTabulator.fetchSelectedData();
            let json = tableData.length > 0 ? JSON.stringify(treeTabulator.fetchSelectedData()) : ''
            let data = {
                actionId: actionId,
                json: json,

            };

            jqClientAdvanced(options).Post(`ServiceAction/UpdateActionFieldList`, data);


        }
    });




    $('#btnshowdetailsDiv1').click(function () {
        $('.EvaluationActionDetailsDiv').toggle();
        $('#btnshowdetailsDiv1').toggleClass('fa-chevron-down fa-chevron-up');

    });

    $('#btnshowdetailsDiv2').click(function () {
        $('.EvaluationActionDtepDiv').toggle();
        $('#btnshowdetailsDiv2').toggleClass('fa-chevron-down fa-chevron-up');
    });


});
var LoadAllActionStatusConfiguration = (actionId) => {
    maintable.setData([]);
    const serviceId = $("#ServiceId").val();

    const options = {
        success: function (data) {
            if (data) {
                maintable.setData([]);
                if (data && data.length > 0) {

                    maintable.addData(data);

                }
                else {

                    maintable.setData([]);

                }

            }
        }
    };
    jqClientAdvanced(options).Get("ActionStatusConfiguration/GetActionStatusConfigurationByAction".concat('?serviceId=', serviceId).concat('&actionId=', actionId));
}

var LoadAllActionCondition = (actionId) => {
    actionconditiontable.setData([]);

    const options = {
        success: function (data) {
            if (data) {

                if (data && data.length > 0) {
                    actionconditiontable.addData(data);

                }
                else {

                    actionconditiontable.setData([]);

                }

            }
        }
    };
    jqClientAdvanced(options).Get("ServiceAction/GetActionConditionByAction".concat('?actionId=', actionId));
}
$("#btnActionStatusConfigAdd").click(function () {


    var modaltitle = sharedFn().GetUiControlText('ActionStatusConfigHeader');

    sharedFn().OpenFormPopup(modaltitle, ActionStatusConfigcontrolvalidationlist, null, null, null);
    popupname = "ActionStatusConfiguration";

});

$("#btnActionConditionAdd").click(function () {


    var modaltitle = sharedFn().GetUiControlText('ActionConditionHeader');

    sharedFn().OpenFormPopup(modaltitle, ActionConditioncontrolvalidationlist, null, null, null);
    popupname = "ActionCondition";

});
function DefaultSetUp() {
    if (popupname == "ActionStatusConfiguration") {
        $("#ActionStatusConfigurationIsRemark").prop("checked", false);
        $("#ActionStatusConfigurationRemarkLabelEn").parent().hide();
        $("#ActionStatusConfigurationRemarkLabelAr").parent().hide();
        $("#ActionStatusConfigurationIsRemarkRequired").parent().parent().parent().hide();
        $("#ActionStatusConfigurationIsOtherAttachment").prop("checked", false);
        $("#ActionStatusConfigurationAttachmentLabelAr").parent().hide();
        $("#ActionStatusConfigurationAttachmentLabelEn").parent().hide();
        $("#ActionStatusConfigurationIsOtherAttachmentRequired").parent().parent().parent().hide();
        $("#ActionStatusConfigurationIsRemarkRequired").prop("checked", false);
        $("#ActionStatusConfigurationIsOtherAttachmentRequired").prop("checked", false);
        $("#ActionStatusConfigurationShowIsDefaultAssigner").prop("checked", false);
        AfterDataBind();
    }
    if (popupname == "ActionStatusConfigurationNotification") {
        $("#ActionStatusConfigurationNotificationIsEmailSend").prop("checked", false);
        $("#ActionStatusConfigurationNotificationIsMessageSend").prop("checked", false);
        $("#ActionStatusConfigurationNotificationIsNotificationSend").prop("checked", false);

        $("#ActionStatusConfigurationNotificationIsEmailSend").on("change", function () {

            if (this.checked) {
                $("#ActionStatusConfigurationNotificationEmailTemplateId").parent().show();
            }
            else {
                $("#ActionStatusConfigurationNotificationEmailTemplateId").val('').trigger("change");
                $("#ActionStatusConfigurationNotificationEmailTemplateId").parent().hide();

            }
        })
        $("#ActionStatusConfigurationNotificationIsEmailSend").trigger("change");
        $("#ActionStatusConfigurationNotificationIsMessageSend").on("change", function () {

            if (this.checked) {
                $("#ActionStatusConfigurationNotificationSMSTemplateId").parent().show();
            }
            else {
                $("#ActionStatusConfigurationNotificationSMSTemplateId").val('').trigger("change");
                $("#ActionStatusConfigurationNotificationSMSTemplateId").parent().hide();

            }
        })
        $("#ActionStatusConfigurationNotificationIsMessageSend").trigger("change");
        $("#ActionStatusConfigurationNotificationIsNotificationSend").on("change", function () {

            if (this.checked) {
                $("#ActionStatusConfigurationNotificationNotificationTemplateId").parent().show();
            }
            else {
                $("#ActionStatusConfigurationNotificationNotificationTemplateId").val('').trigger("change");
                $("#ActionStatusConfigurationNotificationNotificationTemplateId").parent().hide();

            }
        })
        $("#ActionStatusConfigurationNotificationIsNotificationSend").trigger("change");

    }
    if (popupname == "ActionCondition") {
        var actionid = $('#EvaluationActionId').val();
        $("#ActionConditionServiceActionId").val(actionid);

    }
}
function SetActiveTabs() {
    const tabs = document.getElementsByClassName("tablinks");

    for (let i = 0; i < tabs.length; i++) {
        const style = window.getComputedStyle(tabs[i]);
        if (style.display !== "none") {
            tabs[i].click();
            break;
        }
    }
}
function AfterDataBind() {
    if (popupname == "ActionStatusConfiguration") {
        $("#ActionStatusConfigurationServiceActionId").on("change", function () {

            if ($("#ActionStatusConfigurationServiceActionId").val() != "") {
                var Actiontype = actionTypes.find(x => x.id == $("#EvaluationActionActionTypeId").val());
                if (Actiontype) {
                    if (Actiontype.backendName == "ASSIGN" || Actiontype.backendName == "APPROVE_AND_ASSIGN") {
                        $("#ActionStatusConfigurationShowIsDefaultAssigner").parent().parent().parent().show();
                    }
                    else {
                        $("#ActionStatusConfigurationShowIsDefaultAssigner").parent().parent().parent().hide();
                        $("#ActionStatusConfigurationShowIsDefaultAssigner").prop("checked", false);
                    }
                    //if (Actiontype.backendName == "REQUEST_DATA_CHANGE" || Actiontype.backendName == "SUBMIT_MISSING_DATA") {
                    //    $("#defaultOpen").hide();
                    //    $("#EvaluationActionDtepDiv").hide();
                    //    SetActiveTabs();
                    //}
                    //else {
                    //    $("#defaultOpen").show();
                    //    $("#EvaluationActionDtepDiv").show();
                    //    SetActiveTabs();
                    //}
                    //if (Actiontype.backendName == "APPROVE" || Actiontype.backendName == "REJECT" || Actiontype.backendName == "RETURNBACK") {
                    //    $(".stepdiv").hide();
                    //    $("#stepId").val('').trigger('change');
                    //}
                    //else {
                    //    $(".stepdiv").show();
                    //}
                }
                else {
                    $("#ActionStatusConfigurationShowIsDefaultAssigner").parent().parent().parent().hide();
                    $("#ActionStatusConfigurationShowIsDefaultAssigner").prop("checked", false);
                    //$("#defaultOpen").show();
                    //$("#EvaluationActionDtepDiv").show();
                    //SetActiveTabs();
                    //$(".stepdiv").show();
                }
            }
        })
        $("#ActionStatusConfigurationIsRemark").on("change", function () {

            if (this.checked) {
                $("#ActionStatusConfigurationRemarkLabelEn").parent().show();
                $("#ActionStatusConfigurationRemarkLabelAr").parent().show();
                $("#ActionStatusConfigurationIsRemarkRequired").parent().parent().parent().show();
            }
            else {
                $("#ActionStatusConfigurationRemarkLabelEn").val('');
                $("#ActionStatusConfigurationRemarkLabelAr").val('');
                $("#ActionStatusConfigurationIsRemarkRequired").prop("checked", false);
                $("#ActionStatusConfigurationRemarkLabelEn").parent().hide();
                $("#ActionStatusConfigurationRemarkLabelAr").parent().hide();
                $("#ActionStatusConfigurationIsRemarkRequired").parent().parent().parent().hide();

            }
        })
        $("#ActionStatusConfigurationIsOtherAttachment").on("change", function () {


            if (this.checked) {
                $("#ActionStatusConfigurationAttachmentLabelAr").parent().show();
                $("#ActionStatusConfigurationAttachmentLabelEn").parent().show();
                $("#ActionStatusConfigurationIsOtherAttachmentRequired").parent().parent().parent().show();
            }
            else {
                $("#ActionStatusConfigurationAttachmentLabelAr").val('');
                $("#ActionStatusConfigurationAttachmentLabelEn").val('');
                $("#ActionStatusConfigurationIsOtherAttachmentRequired").prop("checked", false);
                $("#ActionStatusConfigurationAttachmentLabelAr").parent().hide();
                $("#ActionStatusConfigurationAttachmentLabelEn").parent().hide();
                $("#ActionStatusConfigurationIsOtherAttachmentRequired").parent().parent().parent().hide();

            }
        })
    }

}
$('#btn-submit_popup').click(function () {
    if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {
        commonUtil.btnProgress("btn-submit_popup");
        var controlvalidation = (popupname == "ActionStatusConfiguration" ? ActionStatusConfigcontrolvalidationlist : popupname == "ActionFieldAttribute" ? ActionFieldAttributecontrolvalidationlist : popupname == "ActionStatusConfigurationNotification" ? ActionStatusConfigNotificationcontrolvalidationlist : popupname == "ActionCondition" ? ActionConditioncontrolvalidationlist : null);
        var requestdata = sharedFn().GetSaveObject(controlvalidation, $('#Id').val());

        var url = '';
        if (popupname == "ActionStatusConfiguration") {
            url = ($('#Id').val() != '' ? "ActionStatusConfiguration/UpdateActionStatusConfiguration" : "ActionStatusConfiguration/SaveActionStatusConfiguration");
        }
        else if (popupname == "ActionStatusConfigurationNotification") {
            url = ($('#Id').val() != '' ? "ActionStatusConfiguration/UpdateActionStatusConfigurationNotification" : "ActionStatusConfiguration/SaveActionStatusConfigurationNotification");
        }
        else if (popupname == "ActionFieldAttribute") {
            url = ($('#Id').val() != '' ? "ServiceAction/UpdateActionFieldAttribute" : "ServiceAction/SaveActionFieldAttribute");
        }
        else if (popupname == "ActionCondition") {
            url = ($('#Id').val() != '' ? "ServiceAction/UpdateActionCondition" : "ServiceAction/SaveActionCondition");
        }

        const options = {
            success: function (data) {
                if (data) {
                    commonUtil.btnProgress("btn-submit_popup", true);
                    var { responseStatus } = data;
                    //debugger
                    switch (responseStatus) {
                        case 1:
                            if (popupname == "ActionStatusConfiguration") {
                                $("#ModalPopup").modal("hide");
                                maintable.addData([data], true);
                            }
                            else if (popupname == "ActionCondition") {
                                $("#ModalPopup").modal("hide");
                                actionconditiontable.addData([data], true);
                                fieldselectvalue = '';
                            }
                            else {
                                sharedFn().ResetVisibleControls("PopupForm");
                                sharedFn().ResetVisibleControls("ModalPopup");
                                DefaultSetUp();
                                table.addData([data], true);
                            }


                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                            break;

                        case 2:
                            if (popupname == "ActionStatusConfiguration") {
                                $("#ModalPopup").modal("hide");
                                maintable.updateData([data]);
                            }
                            else if (popupname == "ActionCondition") {
                                $("#ModalPopup").modal("hide");
                                actionconditiontable.updateData([data]);
                                fieldselectvalue = '';
                            }
                            else {
                                sharedFn().ResetVisibleControls("PopupForm");
                                sharedFn().ResetVisibleControls("ModalPopup");
                                DefaultSetUp();
                                table.updateData([data]);
                            }

                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                            break;

                        default:
                            notificationUtil.error(data.message);

                            break;
                    }


                }




            }
            ,
            error: function (xhr) {
                notificationUtil.error(xhr.responseJSON.Message);
                $('#btn-submit_popup').removeAttr("disabled");
            }
        };

        jqClientAdvanced(options).PostFormData(url, requestdata);
    }


});
function opentab(evt, cityName) {
    // Declare all variables
    var i, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
    if (cityName == "EvaluationActionStatusConfigDiv") {
        popupname = "ActionStatusConfiguration";
    }
    if (cityName == "EvaluationActionConditionDiv") {
        popupname = "ActionCondition";
        GetFieldList();
    }
}
function SetDropDown() {
    if (popupname == "ActionFieldAttribute") {
        if (ActionFieldAttributecontrolvalidationlist) {
            var dropdownlist = ActionFieldAttributecontrolvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN' || c.constraint.controlType == 'MULTIDROPDOWN');
            if (dropdownlist.length > 0) {
                dropdownlist.forEach(item => {
                    var constrain = item.constraint;
                    if (constrain.controlName == "AttributeKeyId") {
                        const options1 = {
                            success: function (result) {
                                if (result) {
                                    const { data } = result;
                                    if (data) {
                                        const { Attribute } = data;
                                        AttributeList = Attribute.map(item => (
                                            {
                                                id: item.nameAr,
                                                text: item.nameAr
                                            }
                                        ));
                                        var $dropdown = $('#' + constrain.uibackendName);
                                        $dropdown.select2({
                                            width: 'resolve',
                                            allowClear: true,
                                            data: AttributeList,
                                            placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                                            dropdownCssClass: "manageselect2zindex",
                                            dropdownParent: $("#ModalPopup"),
                                        }).on("change", event => {
                                            var data = event.target.value;
                                            if (data) {
                                                if (data == "Others") {
                                                    $('#ActionFieldAttributeAttributeKey').val('');
                                                    $('#ActionFieldAttributeAttributeKey').removeAttr("disabled");
                                                }
                                                else {
                                                    $('#ActionFieldAttributeAttributeKey').val(data);
                                                    $('#ActionFieldAttributeAttributeKey').attr("disabled", "disabled");
                                                }
                                            }

                                        }).on("select2:unselecting", function (e) {
                                            $('#ActionFieldAttributeAttributeKey').val('');
                                            $('#ActionFieldAttributeAttributeKey').removeAttr("disabled");

                                        });
                                        $dropdown.val('').trigger('change');
                                    }
                                }
                            }
                        };
                        jqClientAdvanced(options1).Get("ServiceAction/GetAllAttribute");


                    };
                });
            }
        }
    }
    if (popupname == "ActionStatusConfiguration") {
        DefaultSetUp();
        var actionid = $('#EvaluationActionId').val();
        $("#ActionStatusConfigurationServiceActionId").val(actionid).trigger('change');
        $("#ActionStatusConfigurationServiceActionId").attr("data-value", actionid);
        $("#ActionStatusConfigurationServiceActionId").attr("disabled", "disabled");
        AfterDataBind();
        $("#ActionStatusConfigurationIsRemark").trigger('change');
        $("#ActionStatusConfigurationIsOtherAttachment").trigger('change');

    }
    if (popupname == "ActionCondition") {

        $("#ActionConditionFieldValue").parent().show();
        $("#ActionConditionFieldDropDownValueIds").parent().hide();
        $('#ActionConditionType').on("change", function (event) {
            var ActionConditionType = event.target.value;
            $("#ActionConditionRefID").empty();

            if (ActionConditionType) {
                if (ActionConditionType == "Service") {
                    var ddldata = ServiceFieldList.map(dpitem => (
                        {
                            id: dpitem.id,
                            text: txtDir === "RTL" ? dpitem.titleAr : dpitem.titleEn
                        }
                    ));
                    var $dropdown = $('#ActionConditionRefID');
                    $dropdown.select2({
                        width: 'resolve',
                        allowClear: true,
                        data: ddldata,
                        placeholder: sharedFn().GetUiControlText('ActionConditionRefID'),
                        dropdownCssClass: "manageselect2zindex",
                        dropdownParent: $("#ModalPopup"),
                    });
                    var ActionConditionRefID = $('#ActionConditionRefID').data("value");
                    if (ActionConditionRefID) {
                        $('#ActionConditionRefID').val(ActionConditionRefID).trigger('change');
                    }
                }
                else if (ActionConditionType == "Evaluation") {
                    var ddldata = SystemFieldList.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.titleAr : item.titleEn
                        }
                    ));
                    var $dropdown = $('#ActionConditionRefID');
                    $dropdown.select2({
                        width: 'resolve',
                        allowClear: true,
                        data: ddldata,
                        placeholder: sharedFn().GetUiControlText('ActionConditionRefID'),
                        dropdownCssClass: "manageselect2zindex",
                        dropdownParent: $("#ModalPopup"),
                    });
                    var ActionConditionRefID = $('#ActionConditionRefID').data("value");
                    if (ActionConditionRefID) {
                        $('#ActionConditionRefID').val(ActionConditionRefID).trigger('change');
                    }
                }
            }

        })

        $('#ActionConditionRefID').on("change", function (event) {
            FieldChangeEvent();
        });
        $('#ActionConditionoperators').on("change", function (event) {

            FieldChangeEvent();
        });
    }



}
function GetFieldList() {
    const serviceId = $("#ServiceId").val();
    const options = {
        success: function (result) {
            if (result) {
                ServiceFieldList = [];
                const { Field } = result.data;
                ServiceFieldList = Field;



            }
        }
    };
    jqClientAdvanced(options).Get("FormGroup/GetAllField".concat('?serviceid=', serviceId));

    const options1 = {
        success: function (result) {
            if (result) {
                SystemFieldList = [];
                const { SystemField } = result.data;
                SystemFieldList = SystemField;



            }
        }
    };
    jqClientAdvanced(options1).Get("ServiceAction/GetAllSystemField".concat('?serviceid=', serviceId));
}

function SetPopupMode() {
    if (popupname == "ActionStatusConfiguration") {

        var actionid = $('#EvaluationActionId').val();
        $("#ActionStatusConfigurationServiceActionId").val(actionid).trigger('change');
        $("#ActionStatusConfigurationServiceActionId").attr("data-value", actionid);
        $("#ActionStatusConfigurationServiceActionId").attr("disabled", "disabled");
        AfterDataBind();
        $("#ActionStatusConfigurationIsRemark").trigger('change');
        $("#ActionStatusConfigurationIsOtherAttachment").trigger('change');


    }
    if (popupname == "ActionStatusConfigurationNotification") {
        if (ActionStatusConfigNotificationcontrolvalidationlist) {
            var dropdownlist = ActionStatusConfigNotificationcontrolvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN' || c.constraint.controlType == 'MULTIDROPDOWN');
            if (dropdownlist.length > 0) {
                dropdownlist.forEach(item => {
                    var constrain = item.constraint;
                    if (constrain.controlName == "PartyTypeId") {
                        const options1 = {
                            success: function (result) {
                                if (result) {


                                    var data = result.map(item => (
                                        {
                                            id: item.id,
                                            text: txtDir === "RTL" ? item.nameAr : item.nameEn
                                        }
                                    ));
                                    var $dropdown = $('#' + constrain.uibackendName);
                                    $dropdown.select2({
                                        width: 'resolve',
                                        allowClear: true,
                                        data: data,
                                        placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                                        dropdownCssClass: "manageselect2zindex",
                                        dropdownParent: $("#ModalPopup"),
                                    });
                                    $dropdown.val('').trigger('change');

                                }
                            }
                        };
                        jqClientAdvanced(options1).Get("ServiceAction/GetAllpartyType");


                    };
                });
            }
        }
        DefaultSetUp();
    }
    if (popupname == "ActionCondition") {
        var actionid = $('#EvaluationActionId').val();
        $("#ActionConditionServiceActionId").val(actionid);
        var ActionConditionRefID = $('#ActionConditionRefID').data("value");
        var ActionConditionType = $('#ActionConditionType').data("value");
        fieldselectvalue = $('#ActionConditionFieldValue').val();

    }
}
function FieldChangeEvent() {
    var ActionConditionRefID = $('#ActionConditionRefID').val();
    var ActionConditionType = $('#ActionConditionType').val();
    if (ActionConditionRefID && ActionConditionType) {
        var fieldtypelist = (ActionConditionType == "Service" ? ServiceFieldList : SystemFieldList);
        var fieldtype = fieldtypelist.find(x => x.id == ActionConditionRefID);
        if (fieldtype) {
            var selectedvalue = fieldtype.type;
            var DropDownTypeId = fieldtype.dropDownTypeId;

            if (selectedvalue == "dropdown" || selectedvalue == "select2" || selectedvalue == "VacancySeat") {
                $("#ActionConditionFieldValue").val('');
                $("#ActionConditionFieldValue").parent().hide();
                $("#ActionConditionFieldDropDownValueIds").parent().show();
                const options = {
                    success: function (result) {
                        if (result) {
                            const { data } = result;
                            if (data) {
                                const { FieldDropDownValueList } = data;
                                DropDownValueList = FieldDropDownValueList.map(dpitem => (
                                    {
                                        id: dpitem.id,
                                        text: txtDir === "RTL" ? dpitem.titleAr : dpitem.titleEn
                                    }
                                ));
                                var $dropdown = $("#ActionConditionFieldDropDownValueIds");
                                if ($("#ActionConditionoperators").val() == "in" || $("#ActionConditionoperators").val() == "not in") {
                                    $dropdown.attr('multiple', 'multiple');
                                }
                                else {
                                    $dropdown.removeAttr('multiple');
                                }
                                $dropdown.empty();
                                $dropdown.select2({
                                    width: 'resolve',
                                    allowClear: true,
                                    data: DropDownValueList,
                                    placeholder: sharedFn().GetUiControlText('ActionConditionFieldDropDownValueIds'),
                                    dropdownCssClass: "manageselect2zindex",
                                    dropdownParent: $("#ModalPopup")
                                })
                                if (fieldselectvalue) {
                                    var Ids = String(fieldselectvalue).split(',').map(function (item) {
                                        return item.trim();
                                    });

                                    $dropdown.attr("data-value", Ids);
                                    $dropdown.val(Ids).trigger('change');
                                }

                            }
                        }
                    }
                };
                jqClientAdvanced(options).Get("FormGroup/GetFieldDropDownValues".concat('?DropDownTypeId=', DropDownTypeId));
            }
            else {
                $("#ActionConditionFieldValue").parent().show();
                $("#ActionConditionFieldDropDownValueIds").empty();
                $("#ActionConditionFieldDropDownValueIds").parent().hide();
            }
        }
    }
}
const deleteData = (id, urlname = null) => {
    if (popupname == "ActionFieldAttribute") {
        urlname = "ServiceAction/DeleteActionFieldAttribute";
    }
    if (popupname == "ActionStatusConfiguration") {
        urlname = "ActionStatusConfiguration/DeleteActionStatusConfiguration";
    }
    if (popupname == "ActionStatusConfigurationNotification") {
        urlname = "ActionStatusConfiguration/DeleteActionStatusConfigurationNotification";
    }
    if (popupname == "ActionCondition") {
        urlname = "ServiceAction/DeleteActionCondition";
    }
    notificationUtil.confirmation({ title: sharedFn().GetUiControlText('ADMIN_WARNING_DELETE'), okText: sharedFn().GetUiControlText('DELETE_BUTTON'), cancelText: sharedFn().GetUiControlText('ADMIN_CANCEL') }, result => {
        if (!id) return;



        const options = {
            success: function (data) {
                if (data) {
                    if (data.responseStatus == '3') {
                        if (popupname == "ActionStatusConfiguration") {
                            maintable.deleteRow(id);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));

                        }
                        else if (popupname == "ActionCondition") {
                            actionconditiontable.deleteRow(id);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));

                        }
                        else {
                            sharedFn().ResetVisibleControls("PopupForm");
                            table.deleteRow(id);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));

                        }
                        DefaultSetUp();
                    }
                    else {
                        notificationUtil.error(data.message);
                    }
                }
            }
        };
        jqClientAdvanced(options).Post(urlname.concat('?Id=', id));

    });
};

function Loadtabledata() {

    if (popupname == "ActionFieldAttribute") {
        var actionfieldid = $("#actionfieldid").val();
        const options = {
            success: function (data) {
                if (data) {


                    if (data && data.length > 0) {
                        table.addData(data);

                    }
                    else {
                        table.setData([]);
                    }

                }
            }
        };

        jqClientAdvanced(options).Get("ServiceAction/GetAllActionFieldAttribute".concat('?actionfieldid=', actionfieldid));
    }

    if (popupname == "ActionStatusConfigurationNotification") {
        var actionstatusconfigid = $("#actionstatusconfigid").val();
        const options = {
            success: function (data) {
                if (data) {


                    if (data && data.length > 0) {
                        table.addData(data);

                    }
                    else {
                        table.setData([]);
                    }

                }
            }
        };

        jqClientAdvanced(options).Get("ActionStatusConfiguration/GetAllActionStatusConfigurationNotification".concat('?actionstatusconfigid=', actionstatusconfigid));
    }
}

function CreateEditForFormGroup(pkId) {



    if (popupname == "ActionStatusConfiguration") {
        const obj = maintable.getData().find(f => f.id == pkId);
        var modaltitle = sharedFn().GetUiControlText('ActionStatusConfigHeader');
        sharedFn().OpenFormPopup(modaltitle, ActionStatusConfigcontrolvalidationlist, obj);

    }
    if (popupname == "ActionCondition") {
        const obj = actionconditiontable.getData().find(f => f.id == pkId);
        var modaltitle = sharedFn().GetUiControlText('ActionConditionHeader');
        sharedFn().OpenFormPopup(modaltitle, ActionConditioncontrolvalidationlist, obj);

    }
    if (popupname == "ActionFieldAttribute" || popupname == "ActionStatusConfigurationNotification") {
        const obj = table.getData().find(f => f.id == pkId);
        var controlvalidationlists = (popupname == "ActionFieldAttribute" ? ActionFieldAttributecontrolvalidationlist : popupname == "ActionStatusConfigurationNotification" ? ActionStatusConfigNotificationcontrolvalidationlist : null);
        if (controlvalidationlists.length > 0) {
            $('#Id').val(obj.id);
            controlvalidationlists.forEach(item => {
                var contrains = item.constraint;
                var fieldname = item.controlName.charAt(0).toLowerCase() + item.controlName.slice(1);
                if (contrains.controlType == 'TEXT_BOX') {
                    $('#' + contrains.uibackendName).val(obj[fieldname]);
                }
                if (contrains.controlType == 'COLOR') {
                    $('#' + contrains.uibackendName).val(obj[fieldname]);
                    var colorpicker = '#' + contrains.uibackendName + 'picker';
                    $(colorpicker).val(obj[fieldname]);
                }
                if (contrains.controlType == 'CHECK_BOX') {
                    $('#' + contrains.uibackendName).prop("checked", obj[fieldname] ?? false);
                    $('#' + contrains.uibackendName).trigger("change");
                }
                if (contrains.controlType == 'DROPDOWN') {
                    $('#' + contrains.uibackendName).attr("data-value", obj[fieldname]);
                    $('#' + contrains.uibackendName).val(obj[fieldname]).trigger('change');

                }
                if (contrains.controlType == 'MULTIDROPDOWN') {
                    if (obj[fieldname]) {
                        if (obj[fieldname].length == 1) {
                            var entity = obj[fieldname][0];
                            $('#' + contrains.uibackendName).val(entity).trigger('change');
                            $('#' + contrains.uibackendName).attr("data-value", entity);

                        }
                        else {
                            var entity = obj[fieldname].map(x => x);
                            $('#' + contrains.uibackendName).val(entity).trigger('change');
                            $('#' + contrains.uibackendName).attr("data-value", entity);

                        }
                    }

                }
                if (contrains.controlType == 'TEXT_TINY') {
                    obj[fieldname] = obj[fieldname] == null ? "" : obj[fieldname];
                    tinyMCE.get(contrains.uibackendName).setContent(obj[fieldname]);
                }
                if (contrains.controlType == 'DATE') {
                    var date = sharedFn().GetActionDate(obj[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD);
                    $('#' + contrains.uibackendName).val(date);
                }

            });
        }
        if (popupname == "ActionFieldAttribute") {
            var attribute = $('#ActionFieldAttributeAttributeKey').val();
            var exist = AttributeList.filter(x => x.id == attribute).length;
            if (exist > 0) {
                $('#ActionFieldAttributeAttributeKeyDropdown').val(attribute).trigger('change');
                $('#ActionFieldAttributeAttributeKey').attr("disabled", "disabled");

            }
            else {
                $('#ActionFieldAttributeAttributeKeyDropdown').val('Others').trigger('change');
                $('#ActionFieldAttributeAttributeKey').removeAttr("disabled");

            }
            $('#ActionFieldAttributeAttributeKey').val(attribute);
        }
    }


}
function ActionStatusConfigClick(event) {
    const cellElem = event.closest('section');
    const actionstatusconfigid = cellElem.getAttribute('data-key');
    IsEdit = IsEdit_ActionStatusConfigNotification;
    IsDelete = IsDelete_ActionStatusConfigNotification;
    IsView = '';
    containsOrderNo = 'False';
    var ActionStatusConfigNotificationtabulatorcolumns = sharedFn().PopulateColumn(ActionStatusConfigNotificationcolumnList);
    var modaltitle = sharedFn().GetUiControlText('ActionStatusConfigurationNotificationHeader');

    sharedFn().OpenFormPopup(modaltitle, ActionStatusConfigNotificationcontrolvalidationlist, null, ActionStatusConfigNotificationtabulatorcolumns, settingList);
    $("#actionstatusconfigid").val(actionstatusconfigid);
    popupname = "ActionStatusConfigurationNotification";


}