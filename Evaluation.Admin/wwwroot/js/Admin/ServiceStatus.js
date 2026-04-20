var maintable = null;
var popupname = "";
$(document).ready(function () {
    $('.StatusDetails').hide();
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
        rowMoved: function (row) {
            var request = [];
            maintable.getData().map(function (d, index) {

                request.push({
                    "Id": d.id,
                    "OrderNo": index
                });
            });
            var formData = new FormData();
            //debugger
            formData.append('OrderObj', JSON.stringify(request));
            const options = {
                success: function (data) {
                    notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                }
            };
            jqClientAdvanced(options).PostFormData("ActionStatusConfiguration/UpdateActionStatusConfigurationOrder", formData);

        },
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

    document.getElementById("defaultOpen").click();

    $("#ServiceStatusColorCode").click(function () {
        $('#ServiceStatusColorCodepicker').focus();
        $('#ServiceStatusColorCodepicker').click();
    });
    $("#ServiceStatusColorCodepicker").on('change', function () {
        $('#ServiceStatusColorCode').val(this.value);
        $('#ServiceStatusColorCode').trigger('keyup');
    });



    let tabulatorTable;
    const GetServiceStatusPartyTypeDisplayNameList = (serviceStatusId, disableTable) => {



        const options = {
            success: function (result) {
                if (result) {

                    //debugger


                    tabulatorTable = new Tabulator("#StatusPartyTypeDisplayListTable", {

                        layout: "fitColumns",
                        data: result,
                        columns: [

                            { title: 'id', field: "id", visible: false },
                            { title: 'partyTypeId', field: "partyTypeId", visible: false },
                            { title: 'statusId', field: "statusId", visible: false },

                            { title: sharedFn().GetUiControlText('lblStatusPartyTypeDisplayPartyTypeNameEn'), field: "partyTypeNameEn" },
                            { title: sharedFn().GetUiControlText('lblStatusPartyTypeDisplayPartyTypeNameAr'), field: "partyTypeNameAr" },
                            {
                                title: sharedFn().GetUiControlText('lblStatusPartyTypeDisplayTitleArForTable'),
                                field: "titleAr",
                                editor: "input",
                                //validator: function (cell, value, params) {

                                //    var params = {
                                //        maxlength: statusPartyTypeDisplayTitleAr.constraint.maxLength,
                                //        error_messageAr: statusPartyTypeDisplayTitleAr.control.labelAr,
                                //        error_messageEn: statusPartyTypeDisplayTitleAr.control.labelEn,
                                //        error_messageDivId: 'StatusPartyTypeDisplayListTableErrorDiv',
                                //        elementToBeHiddenId: 'saveStatus',
                                //    };

                                //    if (value.length > params.maxlength) {

                                //        var message = '';

                                //        if (lang == "ar") {
                                //            message = commonUtil.stringFormat(params.error_messageAr, params.maxlength);
                                //        } else {
                                //            message = commonUtil.stringFormat(params.error_messageEn, params.maxlength);
                                //        }

                                //        $('#' + params.error_messageDivId).html(message);

                                //        $('#' + params.error_messageDivId).show();
                                //        $('#' + params.elementToBeHiddenId).hide();
                                //        return false;
                                //    }

                                //    $('#' + params.error_messageDivId).hide();
                                //    $('#' + params.elementToBeHiddenId).show();
                                //    return true;
                                //},

                            },
                            {
                                title: sharedFn().GetUiControlText('lblStatusPartyTypeDisplayTitleEnForTable'),
                                field: "titleEn",
                                editor: "input",
                                //validator: function (cell, value, params) {
                                //    //let minlength = parameters.minlength ;

                                //    var params = {
                                //        maxlength: statusPartyTypeDisplayTitleEn.constraint.maxLength,
                                //        error_messageAr: statusPartyTypeDisplayTitleEn.control.labelAr,
                                //        error_messageEn: statusPartyTypeDisplayTitleEn.control.labelEn,
                                //        error_messageDivId: 'StatusPartyTypeDisplayListTableErrorDiv',
                                //        elementToBeHiddenId: 'saveStatus',
                                //    };

                                //    if (value.length > params.maxlength) {

                                //        var message = '';

                                //        if (lang == "ar") {
                                //            message = commonUtil.stringFormat(params.error_messageAr, params.maxlength);
                                //        } else {
                                //            message = commonUtil.stringFormat(params.error_messageEn, params.maxlength);
                                //        }

                                //        $('#' + params.error_messageDivId).html(message);

                                //        $('#' + params.error_messageDivId).show();
                                //        $('#' + params.elementToBeHiddenId).hide();
                                //        return false;
                                //    }

                                //    $('#' + params.error_messageDivId).hide();
                                //    $('#' + params.elementToBeHiddenId).show();
                                //    return true;
                                //},

                            },
                            {
                                title: sharedFn().GetUiControlText('lblStatusPartyTypeDisplayIsActive'),
                                field: "isActive",
                                formatter: "tickCross",
                                editor: true, // Enable editing for the checkbox
                            },


                        ],

                    });

                    DisableAllElementsForTabulator(tabulatorTable, disableTable);

                }

            }
        };
        jqClientAdvanced(options).Get("ServiceStatus/GetServiceStatusPartyTypeDisplayNameList".concat("?serviceStatusId=", serviceStatusId));
    }




    const LoadAllStatuses = () => {

        const serviceId = $("#ServiceId").val();
        if (serviceId) {
            const options = {
                success: function (data) {
                    if (data) {
                        // debugger

                        $('#filteredList').html('');
                        if (data && data.length > 0) {
                            $.each(data, function (i, item) {
                                let li = `<li class="statuses-li list-group-item" id="${item.id}"><a href="javascript:void(0)">${lang == 'ar' ? item.nameAr : item.nameEn}</a></li>`;
                                $('#filteredList').append(li);


                                $(`#${item.id}`).click(function () {
                                    const id = $(this).attr("id");
                                    if (id) {
                                        // notificationUtil.success(id);
                                        LoadStatusDetails(id);
                                        GetServiceStatusPartyTypeDisplayNameList(id, true);

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
            jqClientAdvanced(options).Get(`/${lang}/ServiceStatus/GetAllServiceStatusList`.concat('?serviceId=', serviceId));
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


    $('#searchTxt').on('input', function () {
        // Declare variables
        var input = $('#searchTxt');
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
    });



    const restoreOrder = (order) => {
        // Restore the order of items in the list based on the old order
        var $listItems = $("#filteredList .statuses-li");

        // Sort items based on the order in oldOrder
        $.each(order, function (index, item) {
            var listItem = $("#" + item.id); // Find the list item by its ID
            var targetPosition = order[index].orderNo - 1; // Target position based on orderNo

            // Insert each item in the correct position
            if (targetPosition === 0) {
                $("#filteredList").prepend(listItem); // If it's the first item, prepend it
            } else {
                var previousItem = $("#" + order[targetPosition - 1].id); // Get the previous item
                listItem.insertAfter(previousItem); // Insert after the previous item
            }
        });
    }


    const LoadStatusDetails = (statusIdId) => {
        if (statusIdId) {
            if (IsView_ActionStatusConfig == 'True') {
                LoadAllActionStatusConfiguration(statusIdId);
            }
            const options = {
                success: function (result) {
                    if (result) {

                        FillForm(result.serviceStatus);
                        // DisableControls(true);
                        addStyleForTheSelectLi(statusIdId);

                        $('#CancelOrSave').hide();
                        $('#DeleteOrEdit').show();
                        DisableFormElements('UpdateStatusForm', true);
                        const ServiceStatusTypeList_select2 = ServiceStatusTypeList
                            .map(item => ({
                                id: item.id,
                                text: lang == "ar" ? item.nameAr : item.nameEn
                            }));
                        $("#ServiceStatusTypeId").select2({
                            width: 'resolve',
                            allowClear: true,
                            data: ServiceStatusTypeList_select2,
                            placeholder: sharedFn().GetUiControlText('ServiceStatusTypeId'),
                            dropdownCssClass: "manageselect2zindex",
                        })
                        //debugger
                        const partyTypesList_select2 = result.partyTypesList
                            .map(item => ({
                                id: item.id,
                                text: lang == "ar" ? item.nameAr : item.nameEn
                            }));
                        let uibackendName = "ServiceStatusPreventPartyType";
                        let mainuibackendName = "main_ServiceStatusPreventPartyType";
                        $("#" + mainuibackendName + " ul").empty();
                        $("#" + uibackendName + " ul").empty();
                        if (result.serviceStatusPreventPartyTypesList.length > 0) {
                            $.each(partyTypesList_select2, function (index, dualistitem) {

                                const exists = result.serviceStatusPreventPartyTypesList.find(x => x == dualistitem.id);
                                const listItem = "<li data-index='0' data-id='" + dualistitem.id + "'>" + dualistitem.text + "</li>";

                                if (exists) {
                                    $("#" + uibackendName + " ul").append(listItem);
                                } else {
                                    $("#main_" + uibackendName + " ul").append(listItem);
                                }
                            });
                        }
                        else {
                            $.each(partyTypesList_select2, function (index, item) {

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
                            $("#" + mainuibackendName + "  li.selected").appendTo('#' + uibackendName + ' ul').removeClass('selected');
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


                    } else {
                        $('.StatusDetails').hide();
                    }
                }
            };
            jqClientAdvanced(options).Get(`/${lang}/ServiceStatus/GetStatusDetails`.concat('?statusId=', statusIdId));

        }
    }

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

    const FillForm = (statusObj) => {
        $('.StatusDetails').show();

        $('#ServiceStatusId').val(statusObj.id);
        $('#ServiceStatusStatusGroupId').val(statusObj.statusGroupId);
        $('#ServiceStatusStatusGroupId').trigger('change');
        $('#ServiceStatusNameEn').val(statusObj.nameEn);
        $('#ServiceStatusNameAr').val(statusObj.nameAr);
        $('#ServiceStatusTypeId').val(statusObj.serviceStatusTypeId);
        $('#ServiceStatusTypeId').trigger('change');
        $('#ServiceStatusColorCode').val(statusObj.colorCode);
        $('#ServiceStatusColorCodepicker').val(statusObj.colorCode);
        $('#ServiceStatusIsActive').prop('checked', statusObj.isActive);
        $('#ServiceStatusIsOpen').prop('checked', statusObj.isOpen);
        $('#ServiceStatusIsInitial').prop('checked', statusObj.isInitial);

    }

    const EmptyForm = () => {

        $('#ServiceStatusId').val('');
        $('#ServiceStatusStatusGroupId').val('');
        $('#ServiceStatusStatusGroupId').trigger('change');
        $('#ServiceStatusTypeId').val('');
        $('#ServiceStatusTypeId').trigger('change');
        $('#ServiceStatusNameEn').val('');
        $('#ServiceStatusNameAr').val('');
        $('#ServiceStatusColorCode').val('');
        $('#ServiceStatusColorCodepicker').val('');
        $('#ServiceStatusIsActive').prop('checked', true);
        $('#ServiceStatusIsOpen').prop('checked', true);
        $('#ServiceStatusIsInitial').prop('checked', false);

    }


    const DisableAllElementsForTabulator = (tabulator, disable = true) => {
        //debugger


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

    //});


    $('#submitBtn').click(function () {
        LoadAllStatuses();




    });



    $('#openEditModeForm').click(function () {

        $('#CancelOrSave').show();
        $('#DeleteOrEdit').hide();
        DisableFormElements('UpdateStatusForm', false);
        DisableAllElementsForTabulator(tabulatorTable, false);

    });

    $('#cancelStatus').click(function () {
        $('.StatusDetails').hide();
        $('#CancelOrSave').hide();
        $('#DeleteOrEdit').show();
        DisableFormElements('UpdateStatusForm', true);
        DisableAllElementsForTabulator(tabulatorTable, true);
    });

    $('#DeleteStatus').click(function () {
        notificationUtil.confirmation({
            title: sharedFn().GetUiControlText('ADMIN_WARNING_DELETE'),
            okText: sharedFn().GetUiControlText('DELETE_BUTTON'),
            cancelText: sharedFn().GetUiControlText('ADMIN_CANCEL')
        }, result => {
            var id = $('#ServiceStatusId').val();
            if (!id) return;
            const options = {
                success: function (result) {
                    if (result) {

                        $(`#${id}`).remove();
                        // table.deleteRow(id);

                        $('.StatusDetails').hide();

                        notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));

                    }
                }
            };
            jqClientAdvanced(options).Get("ServiceStatus/DeleteServiceStatus".concat('?Id=', id));

        });
    });

    $('#saveStatus').click(function () {
        var valid = false;

        if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {


            let status = {
                Id: $('#ServiceStatusId').val(),
                ServiceId: $('#ServiceId').val(),
                NameEn: $("#ServiceStatusNameEn").val(),
                NameAr: $("#ServiceStatusNameAr").val(),
                ColorCode: $("#ServiceStatusColorCode").val(),
                StatusGroupId: $("#ServiceStatusStatusGroupId").val(),
                ServiceStatusTypeId: $("#ServiceStatusTypeId").val(),

                IsInitial: $("#ServiceStatusIsInitial").prop("checked"),
                IsActive: $("#ServiceStatusIsActive").prop("checked"),
                IsOpen: $("#ServiceStatusIsOpen").prop("checked"),
            }
            let listItems = document.querySelectorAll("#ServiceStatusPreventPartyType ul li");

            // Extract 'data-id' values and convert to an array
            let idsArray = Array.from(listItems).map(li => li.getAttribute("data-id"));
            var data = {
                status: status,
                json: JSON.stringify(tabulatorTable.getData()),

                serviceStatusPreventPartyTypesList: idsArray
            };
            var formData = new FormData();
            //debugger
            formData.append('request', JSON.stringify(data));
            //debugger

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
                                    LoadStatusDetails(id);
                                    GetServiceStatusPartyTypeDisplayNameList(id, true);
                                } else {
                                }
                            });

                            LoadStatusDetails(item.id);
                            GetServiceStatusPartyTypeDisplayNameList(item.id, true);

                        }
                        addStyleForTheSelectLi(item.id);
                        $('#CancelOrSave').hide();
                        $('#DeleteOrEdit').show();
                        // $('#StatusDetails').hide();
                        DisableFormElements('UpdateStatusForm', true);
                        DisableAllElementsForTabulator(tabulatorTable, true);



                    }
                },
                // error: function (jqXHR, textStatus, errorThrown) {

                //     $('#StatusDetails').html('');
                //     DisableFormElements('UpdateStatusForm', true);
                //     // DisableAllElementsForTabulator(tabulatorTable, true);
                //     ajaxSetup().HandleAjaxError(jqXHR, textStatus, errorThrown);
                // }
            };

            if (data.status.Id) {
                jqClientAdvanced(options).PostFormData(`/${lang}/ServiceStatus/UpdateServiceStatus`, formData);

            } else {
                jqClientAdvanced(options).PostFormData(`/${lang}/ServiceStatus/SaveServiceStatus`, formData);

            }
        }
        return;
    });


    var initialOrder = [];
    $("#filteredList").sortable({
        items: ".statuses-li", // Specify which items are sortable
        handle: "a", // Set the handle to the <a> element for the drag operation

        start: function (event, ui) {
            // Store the current order before the drag starts
            oldOrder = [];
            var index = 1;
            $("#filteredList .statuses-li").each(function () {
                oldOrder.push({ id: $(this).attr("id"), orderNo: index });
                index++;
            });
        },

        update: function (event, ui) {
            // Code to execute when the order changes
            var currentOrder = [];
            var index = 1;
            $("#filteredList .statuses-li").each(function () {
                currentOrder.push({ Id: $(this).attr("id"), OrderNo: index });
                index++;
            });

            if (currentOrder.length > 0) {
                var formData = new FormData();
                formData.append('OrderObj', JSON.stringify(currentOrder));
                const options = {
                    success: function (data) {
                        oldOrder = currentOrder;
                        notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                    },
                    error: function (data) {
                        restoreOrder(oldOrder);
                        // notificationUtil.error(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));

                        // // Handle error
                        commonUtil.serverError(xhr);
                    }
                };
                jqClientAdvanced(options).PostFormData("ServiceStatus/UpdateServiceStatusOrder", formData);

            }
        }
    });

    $('#AddStatus').click(function () {

        let serviceId = $('#ServiceId').val();
        if (serviceId) {
            EmptyForm();
            $('.StatusDetails').show();
            $('#CancelOrSave').show();
            $('#DeleteOrEdit').hide();
            DisableFormElements('UpdateStatusForm', false);

            GetServiceStatusPartyTypeDisplayNameList('', false);

            //let serviceStatusId = $('#ServiceStatusId').val();

            //if (serviceStatusId) {

            //} else {
            //    tabulatorTable.clearData();
            //}
            //debugger
            const partyTypesList_select2 = partyTypesList
                .map(item => ({
                    id: item.id,
                    text: lang == "ar" ? item.nameAr : item.nameEn
                }));
            const ServiceStatusTypeList_select2 = ServiceStatusTypeList
                .map(item => ({
                    id: item.id,
                    text: lang == "ar" ? item.nameAr : item.nameEn
                }));
            $("#ServiceStatusTypeId").select2({
                width: 'resolve',
                allowClear: true,
                data: ServiceStatusTypeList_select2,
                placeholder: sharedFn().GetUiControlText('ServiceStatusTypeId'),
                dropdownCssClass: "manageselect2zindex",
            })

            let uibackendName = "ServiceStatusPreventPartyType";
            let mainuibackendName = "main_ServiceStatusPreventPartyType";
            $("#" + mainuibackendName + " ul").empty();
            $("#" + uibackendName + " ul").empty();
            $.each(partyTypesList_select2, function (index, item) {

                $("#" + mainuibackendName + " ul").append("<li data-index='0' data-id='" + item.id + "'>" + item.text + "</li>");
            });
            $("#" + uibackendName + " ul").empty();

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
                $("#" + mainuibackendName + "  li.selected").appendTo('#' + uibackendName + ' ul').removeClass('selected');
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


        }
    });

    $('#ServiceId').trigger('change');




});
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
var LoadAllActionStatusConfiguration = (statusIdId) => {
    maintable.setData([]);
    const serviceId = $("#ServiceId").val();

    const options = {
        success: function (data) {
            if (data) {

                if (data && data.length > 0) {
                    maintable.addData(data);

                }
                else {

                    maintable.setData([]);

                }

            }
        }
    };
    jqClientAdvanced(options).Get("ActionStatusConfiguration/GetActionStatusConfigurationByStatus".concat('?serviceId=', serviceId).concat('&statusIdId=', statusIdId));
}
$("#btnActionStatusConfigAdd").click(function () {


    var modaltitle = sharedFn().GetUiControlText('ActionStatusConfigHeader');

    sharedFn().OpenFormPopup(modaltitle, ActionStatusConfigcontrolvalidationlist, null, null, null);
    popupname = "ActionStatusConfiguration";

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

}
function AfterDataBind() {
    if (popupname == "ActionStatusConfiguration") {
        $("#ActionStatusConfigurationServiceActionId").on("change", function () {

            if ($("#ActionStatusConfigurationServiceActionId").val() != "") {
                var Actiontype = AllActions.find(x => x.id == $("#ActionStatusConfigurationServiceActionId").val());
                if (Actiontype) {
                    if (Actiontype.backendName == "ASSIGN" || Actiontype.backendName == "APPROVE_AND_ASSIGN") {
                        $("#ActionStatusConfigurationShowIsDefaultAssigner").parent().parent().parent().show();
                    }
                    else {
                        $("#ActionStatusConfigurationShowIsDefaultAssigner").parent().parent().parent().hide();
                        $("#ActionStatusConfigurationShowIsDefaultAssigner").prop("checked", false);
                    }
                }
                else {
                    $("#ActionStatusConfigurationShowIsDefaultAssigner").parent().parent().parent().hide();
                    $("#ActionStatusConfigurationShowIsDefaultAssigner").prop("checked", false);
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
        var controlvalidation = (popupname == "ActionStatusConfiguration" ? ActionStatusConfigcontrolvalidationlist : popupname == "ActionStatusConfigurationNotification" ? ActionStatusConfigNotificationcontrolvalidationlist : null);
        var requestdata = sharedFn().GetSaveObject(controlvalidation, $('#Id').val());

        var url = '';
        if (popupname == "ActionStatusConfiguration") {
            url = ($('#Id').val() != '' ? "ActionStatusConfiguration/UpdateActionStatusConfiguration" : "ActionStatusConfiguration/SaveActionStatusConfiguration");
        }
        else if (popupname == "ActionStatusConfigurationNotification") {
            url = ($('#Id').val() != '' ? "ActionStatusConfiguration/UpdateActionStatusConfigurationNotification" : "ActionStatusConfiguration/SaveActionStatusConfigurationNotification");
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
                            else {
                                sharedFn().ResetVisibleControls("PopupForm");
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
                            else {
                                sharedFn().ResetVisibleControls("PopupForm");
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
    if (cityName == "scholarshipActionStatusConfigDiv") {
        popupname = "ActionStatusConfiguration"
    }
}
function SetDropDown() {

    if (popupname == "ActionStatusConfiguration") {
        DefaultSetUp();
        var StatusId = $('#ServiceStatusId').val();
        $("#ActionStatusConfigurationCurrentStatusId").val(StatusId).trigger('change');
        $("#ActionStatusConfigurationCurrentStatusId").attr("data-value", StatusId);
        $("#ActionStatusConfigurationCurrentStatusId").attr("disabled", "disabled");
        AfterDataBind();
        $("#ActionStatusConfigurationIsRemark").trigger('change');
        $("#ActionStatusConfigurationIsOtherAttachment").trigger('change');
    }




}

function SetPopupMode() {
    if (popupname == "ActionStatusConfiguration") {

        var StatusId = $('#ServiceStatusId').val();
        $("#ActionStatusConfigurationCurrentStatusId").val(StatusId).trigger('change');
        $("#ActionStatusConfigurationCurrentStatusId").attr("data-value", StatusId);
        $("#ActionStatusConfigurationCurrentStatusId").attr("disabled", "disabled");
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
}

const deleteData = (id, urlname = null) => {

    if (popupname == "ActionStatusConfiguration") {
        urlname = "ActionStatusConfiguration/DeleteActionStatusConfiguration";
    }
    if (popupname == "ActionStatusConfigurationNotification") {
        urlname = "ActionStatusConfiguration/DeleteActionStatusConfigurationNotification";
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
    if (popupname == "ActionStatusConfigurationNotification") {
        const obj = table.getData().find(f => f.id == pkId);

        if (ActionStatusConfigNotificationcontrolvalidationlist.length > 0) {
            ActionStatusConfigNotificationcontrolvalidationlist.forEach(item => {
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
                    fieldvalueselectedlist = obj[fieldname];
                    $('#' + contrains.uibackendName).val(obj[fieldname]).trigger('change');
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

