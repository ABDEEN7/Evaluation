
let showMore = false, table = null, dialogElem = null;
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let currentPage = 0;
let isSearch = false;
let isLoading = true;
let AllStatusList = [];

const btnAddContentId = 'btn-add-content',
    btnSubmitId = "btn-submit",
    $formSection = $('#form-section')
    ;

const gridContainerId = "view-container",
    tblContentContainerId = "tbl-template-container",
    $tblContentContainer = $('#' + tblContentContainerId),
    $btnAddContent = $('#' + btnAddContentId);


$tblContentContainer.hide();

$('#submitBtn').click(function () {
    currentPage = 0;
    table.setData([]);
    loadData();
    $tblContentContainer.show();
    $formSection.hide();
    $btnAddContent.removeAttr("disabled");
    
});


function ClearControlByPage() {
    sharedFn().SetValueToDropdown();
    
}


const loadData = (isSearch) => {
    const serviceId = services_div_select2.val();
    isLoading = true;
const options = {
    success: function (data) {
        if (data) {


            if (data && data.length > 0) {
                if (isSearch) {
                    table.setData([]).then(function () {
                        setAllColumnWidths(table, columnWidths);
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

    jqClientAdvanced(options).Get("ServiceStatusConfiguration/GetAllServiceStatusConfiguration".concat('?ServiceId=', serviceId).concat('&page=', currentPage));
};


const deleteData = (id) => {
    
    notificationUtil.confirmation({ title: sharedFn().GetUiControlText('ADMIN_WARNING_DELETE'), okText: sharedFn().GetUiControlText('DELETE_BUTTON'), cancelText: sharedFn().GetUiControlText('ADMIN_CANCEL') }, result => {
        if (!id) return;



        const options = {
            success: function (data) {
                if (data) {
                    if (data.responseStatus == '3') {
                        table.deleteRow(id);
                        notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));
                    }
                    else {
                        notificationUtil.error(data.message);
                    }
                }
            }
        };
        jqClientAdvanced(options).Post("ServiceStatusConfiguration/DeleteServiceStatusConfiguration".concat('?Id=', id));

    });
};

$(window).scroll(function () {
    if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
        if (!isLoading) {
            loadData();
        }
    }
});
$(document).ready(function () {
    

    table = tableUtil.createTabulator({
        id: gridContainerId,
        config: {
            textDirection: txtDir,
            paginationSize: 10,
            placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
            headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),
            movableRows: true,
        },
        uniqueRowId: 'id',
        sortColumn: "updateDate",
        sortDir: "desc",
        columns: TableColumns,
       
    });

    dialogElem = commonUtil.createDailog({ dailogId: dailogId });

    loadData(false);
   
    
    $(`#${btnAddContentId}`).click(function (e) {
        table = Tabulator.prototype.findTable("#" + gridContainerId)[0];
        sharedFn().ClearForm();
        sharedFn().EditMode();
        sharedFn().SetDefaultValueFromConfig();
        sharedFn().SetValueToDropdown();
       
    });
   

    $("#btn-submit").click(function (e) {


        if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {


            commonUtil.btnProgress(btnSubmitId);
            var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());
            const options = {
                success: function (response) {
                    commonUtil.btnProgress(btnSubmitId, true);

                    let { data } = response;
                    if (data.responseStatus == '1') {
                        table.addData([data], true);
                        if (response) {
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));


                        }
                    }
                    else if (data.responseStatus == '2') {
                        table.updateData([data]);
                        if (response) {
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));

                        }

                    }
                    else {
                        notificationUtil.error(data.message);

                    }
                    sharedFn().ViewMode();
                }
            };

            let url = '';
            let id = $('#Id').val();

            if (id) {
                url = "ServiceStatusConfiguration/UpdateServiceStatusConfiguration";
            } else {
                url = "ServiceStatusConfiguration/SaveServiceStatusConfiguration";

            }
            jqClientAdvanced(options).PostFormData(url, requestdata);
         


        }
    });

   

});



