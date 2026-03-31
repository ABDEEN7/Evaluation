const dailogId = commonUtil.CONTENT_DAILOG_ID;
let showMore = false, table = null, tableCondition = null, dialogElem = null, popupname = "";
let currentPage = 0;
let isSearch = false;
let isLoading = true;
let lookupSources = {};
const btnAddContentId = 'btn-add-content',
    btnSubmitId = "btn-submit",
    $formSection = $('#form-section'),
    thumbailId = "thumbnail";

const gridContainerId = "view-container",
    $tblContentContainer = $('#tbl-template-container'),
    $thumbnail = $('#' + thumbailId),
    $btnAddContent = $('#' + btnAddContentId);

const loadData = (isSearch) => {
    isLoading = true;

    const options = {
        success: function (data) {
            if (data) {
                if (data && data.length > 0) {
                    if (isSearch) {
                        table.setData([]).then(function () {

                        });
                        currentPage = 1;
                    }
                    table.addData(data).then(function () {
                        setAllColumnWidths(table, columnWidths);
                    });
                    currentPage = currentPage + 1;
                    isLoading = false;
                }
            }
        }
    };
    jqClientAdvanced(options).Get("EvaluationParty/GetAllEvaluationParty".concat('?page=', currentPage));
};
const deleteData = (id) => {
    if (!id) return;
    var obj;
    var url = "";
    if (popupname == "PartyTypeEvalPartyStatus") {
        obj = tableCondition.getData().find(f => f.id == id);
        url = "EvaluationParty/DeletePartyTypeEvalPartyStatus";
    }
    else {
        obj = table.getData().find(f => f.id == id);
        url = "EvaluationParty/DeleteEvaluationParty";
    }
    
    if (!obj) return;


    notificationUtil.confirmation({ title: sharedFn().GetUiControlText('ADMIN_WARNING_DELETE'), okText: sharedFn().GetUiControlText('DELETE_BUTTON'), cancelText: sharedFn().GetUiControlText('ADMIN_CANCEL') }, result => {
        if (!id) return;



        const options = {
            success: function (data) {
                if (data) {


                    if (data.responseStatus == '3') {
                        if (popupname == "PartyTypeEvalPartyStatus") {
                            tableCondition.deleteRow(id);
                        }
                        else {
                            table.deleteRow(id);
                        }
                        notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));
                    }

                    else {
                        notificationUtil.error(data.message);
                    }
                }
            }
        };
        jqClientAdvanced(options).Post(url.concat('?Id=', id));

    });
};

function getlookup() {

    const options = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { PartyTypeList, ServiceStatusList } = data;
                    lookupSources["PartyTypeList"] = PartyTypeList;
                    lookupSources["ServiceStatusList"] = ServiceStatusList;
                }
            }
        }
    };
    jqClientAdvanced(options).Get("EvaluationParty/GetPartyTypeEvalPartyStatus");
}

function updateDataKeyAtPosition(position, newKey) {
    // Get all row components
    const rows = tableCondition.getRows();

    // Get the row at that position
    const row = rows[position];

    // Get current row data
    const rowData = row.getData();

    const rowElement = row.getElement();

    const section = rowElement.querySelector('.sec-center');

    section.setAttribute('data-key', newKey);
    section.id = `action__section__${newKey}`;

    // Update the dataKey (or id)
    rowData.id = newKey;

    // Push the update to the table
    row.update(rowData);
}

$(window).scroll(function () {
    if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
        if (!isLoading && currentPage > 0) {
            loadData();
        }
    }
});
function SavePartyTypeEvalPartyStatus(event) {
    const cellElem = event.closest('section');
    const id = cellElem.getAttribute('data-key');
    const obj = tableCondition.getData().find(f => f.id == id);
    var formData = new FormData();
    var data = {};
    data.Id = id;
    data.EvaluationPartyId = $("#Id").val();
    data.PartyTypeId = obj.partyType;
    data.ServiceStatusId = obj.serviceStatus;
    data.IsActive = obj.isActive;
    if (!data.EvaluationPartyId && !data.PartyTypeId && !data.ServiceStatusId) {
        notificationUtil.error('Data Not Added');
        return;
    }
    formData.append('request', JSON.stringify(data));
    const options = {
        success: function (data) {
            if (data) {

                var { responseStatus, id } = data;
                //debugger
                switch (responseStatus) {
                    case 2:
                        updateDataKeyAtPosition(currentrowclicked, id);
                        notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));

                        break;
                    default:
                        notificationUtil.error(data.message);

                        break;
                }


            }




        }
    };

    jqClientAdvanced(options).PostFormData("EvaluationParty/UpdatePartyTypeEvalPartyStatus", formData);
}
function ClearControlByPage() {
    var addbutton = sharedFn().GetUiControlText('PartyTypeEvalPartyStatusAddButton');
    if (IsAdd_PartyTypeEvalPartyStatus) {
        $("#EvaluationPartiesPartyTypeEvalPartyStatusbutton").show();
        
    }
    else {
        $("#EvaluationPartiesPartyTypeEvalPartyStatusbutton").hide();
    }
    $("#EvaluationPartiesPartyTypeEvalPartyStatusbutton").html(addbutton);
    if (IsView_PartyTypeEvalPartyStatus == "True") {
        $("#EvaluationPartiesPartyTypeEvalPartyStatusdiv").show();
        $("#EvaluationPartiesPartyTypeEvalPartyStatustabulator").css("pointer-events", "");
        var viewItem = IsAdd_PartyTypeEvalPartyStatus == "True" ?
            `<span class="Attr pointer" title="` + sharedFn().GetUiControlText('ADMIN_TOOLTIP_SAVE_PartyTypeEvalPartyStatus') + `"><i class="Attr fa fa-save" onclick="SavePartyTypeEvalPartyStatus(this)">
</i></span>`
            : '';
        IsEdit = '';
        IsDelete = IsDelete_PartyTypeEvalPartyStatus;
        IsView = '';
        let ConditionTableColumns = sharedFn().PopulateColumn(PartyTypeEvalPartyStatuscolumnList, viewItem, true);
        tableCondition = tableUtil.createTabulator({
            id: "EvaluationPartiesPartyTypeEvalPartyStatustabulator",
            config: {
                textDirection: txtDir,
                pagination: "local",
                paginationSize: 10,
                placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
                headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),
                movableRows: true,
                selectable: true,
                editable: true
            },
            uniqueRowId: 'id',
            sortColumn: "updateDate",
            sortDir: "desc",
            columns: ConditionTableColumns,
            rowClick: function (e, row) {
                currentrowclicked = row.getPosition();
            }

        });

        tableCondition.setData([]);
        const options = {
            success: function (data) {
                if (data) {


                    if (data && data.length > 0) {
                        tableCondition.setData(data);

                    }



                }
            }
        };

        jqClientAdvanced(options).Get("EvaluationParty/GetAllPartyTypeEvalPartyStatus".concat('?EvaluationPartyId=', $("#Id").val()));
    }
    else {
        $("#EvaluationPartiesPartyTypeEvalPartyStatusdiv").hide();
    }
    $("#EvaluationPartiesPartyTypeEvalPartyStatusdiv").parent().show();
}
$("#EvaluationPartiesPartyTypeEvalPartyStatusbutton").click(function () {
    tableCondition.addRow({
        id: "00000000-0000-0000-0000-000000000000",
        scope: null,
        user: null,
        isActive: true,
    });
});
function ClearForViewMode() {
    $("#EvaluationPartiesPartyTypeEvalPartyStatusbutton").hide();
    $("#EvaluationPartiesPartyTypeEvalPartyStatustabulator").css("pointer-events", "none");
}
$(document).ready(function () {
   
    
    IsEdit = IsEdit_EvaluationParty;
    IsDelete = IsDelete_EvaluationParty;
    IsView = IsView_EvaluationParty;
    let TableColumns = sharedFn().PopulateColumn(columnList);


    table = tableUtil.createTabulator({
        id: gridContainerId,
        config: {
            textDirection: txtDir,
            paginationSize: 10,
            placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
            headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),
            movableRows: true,
        },
        isResponsiveLayout: false,
        uniqueRowId: 'id',
        sortColumn: "updateDate",
        sortDir: "desc",
        columns: TableColumns,
        columnResized: function (column) {

            // Get the resized column width
            var columnField = column.getField();
            var columnWidth = column.getWidth();
            columnWidths[columnField] = columnWidth;
        },
        rowMoved: function (row) {
            var request = [];
            table.getData().map(function (d, index) {

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
            jqClientAdvanced(options).PostFormData("EvaluationParty/UpdateEvaluationPartyOrder", formData);

        }

    });


    dialogElem = commonUtil.createDailog({ dailogId: dailogId });
    loadData();
    getlookup();

    $(`#${btnAddContentId}`).click(function (e) {
        sharedFn().ClearForm();
        sharedFn().EditMode();
        $("#EvaluationPartiesPartyTypeEvalPartyStatusdiv").parent().hide();
       
    });


    $("#btn-submit").click(function (e) {
        popupname = "";
        IsEdit = IsEdit_EvaluationParty;
        IsDelete = IsDelete_EvaluationParty;
        IsView = IsView_EvaluationParty;
        if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {

            commonUtil.btnProgress(btnSubmitId);
            var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());

            const RESPONSE_STATUS = {
                CREATE: 1,
                UPDATE: 2
            };

            const options = {
                success: function (data) {

                    // Fail fast
                    if (!data) {
                        notificationUtil.error('Invalid server response');
                        $('#btn-submit').removeAttr('disabled');
                        return;
                    }

                    commonUtil.btnProgress(btnSubmitId, true);

                    const responseStatus = data?.responseStatus;

                    switch (responseStatus) {

                        case RESPONSE_STATUS.CREATE:
                            table.addData([data], true);
                            table.deselectRow();
                            table.getRows()[0]?.select();
                            notificationUtil.success(
                                sharedFn().GetUiControlText('ADMIN_MSG_SAVE')
                            );
                            sharedFn().ViewMode();
                            break;

                        case RESPONSE_STATUS.UPDATE:
                            table.updateData([data]);
                            notificationUtil.success(
                                sharedFn().GetUiControlText('ADMIN_MSG_UPDATE')
                            );
                            sharedFn().ViewMode();
                            break;

                        default:
                            notificationUtil.error(
                                data.responseMessage || 'Unexpected response status'
                            );
                            break;
                    }

                    $('#btn-submit').removeAttr('disabled');
                },

                error: function () {
                    notificationUtil.error('Request failed. Please try again.');
                    $('#btn-submit').removeAttr('disabled');
                }
            };

            let url = '';
            let id = $('#Id').val();

            if (id) {
                url = "EvaluationParty/UpdateEvaluationParty";
            } else {
                url = "EvaluationParty/SaveEvaluationParty";

            }

            jqClientAdvanced(options).PostFormData(url, requestdata);
        }

    })


})   