
let showMore = false, table = null, dialogElem = null, tree = null, selectedtree=null;
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let currentPage = 0;
let isSearch = false;
let isLoading = true;
let isfirstload = true;


const btnAddContentId = 'btn-add-content',
    btnSubmitId = "btn-submit",
    $formSection = $('#form-section')
    ;

const gridContainerId = "view-container",
    tblContentContainerId = "tbl-template-container",
    $tblContentContainer = $('#' + tblContentContainerId),
    $btnAddContent = $('#' + btnAddContentId);

IconPicker.Init({
    jsonUrl: '../lib/iconpicker/dist/iconpicker-1.5.0.json',
    searchPlaceholder: 'Search Icon',
    showAllButton: 'Show All',
    cancelButton: 'Cancel',
    noResultsFound: 'No results found.',
    borderRadius: '20px',
});

const loadData = (isSearch) => {
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

    jqClientAdvanced(options).Get("Department/GetAllDepartment".concat('?page=', currentPage));
};




const deleteData = (id) => {
    if (!id) return;

    const obj = table.getData().find(f => f.id == id);
    if (!obj) return;
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
        jqClientAdvanced(options).Post("Department/DeleteDepartment".concat('?Id=', id));

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
            jqClientAdvanced(options).PostFormData("Department/UpdateDepartmentOrder", formData);

        }
    });
    
    dialogElem = commonUtil.createDailog({ dailogId: dailogId });
    if (controlvalidationlist) {

        //initializing ICON
        var iconlist = controlvalidationlist.filter(c => c.constraint.controlType == 'ICON');
        if (iconlist.length > 0) {

            iconlist.forEach(item => {
                var constrain = item.constraint;
                var id = '#' + constrain.uibackendName;
                IconPicker.Run(id, function (e) {
                    document.getElementById('IconPreview').className = document.getElementById(constrain.uibackendName).value;
                    var selectedIcon = document.getElementById(constrain.uibackendName).value;
                    if (selectedIcon) {
                        sharedFn().NewvalidateInput($('#' + constrain.uibackendName).attr('id'), sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'));


                    }
                });
            });
        }
    }
   

    document.getElementById(btnAddContentId).addEventListener('click', event => {
        sharedFn().ClearForm();
        sharedFn().EditMode();
        sharedFn().SetDefaultValueFromConfig();

    });

    loadData();

    
    $("#btn-submit").click(function (e) {
        
        
        if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {


            commonUtil.btnProgress(btnSubmitId);
            var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());

            const options = {
                success: function (response) {
                    if (response) {
                        var data = response.data;
                        if (data) {
                            var { responseStatus } = data;
                            //debugger
                            switch (responseStatus) {
                                case 1:

                                    table.addData([data], true);
                                    table.deselectRow();
                                    table.getRows()[0].select();
                                    notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                                    sharedFn().ViewMode();
                                    break;

                                case 2:
                                    table.updateData([data]);
                                    notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                                    sharedFn().ViewMode();
                                    break;
                                case 12:
                                    notificationUtil.error(sharedFn().GetUiControlText('BACKENDNAME_ALREADY_EXISTS'));
                                    $('#btn-submit').removeAttr("disabled");
                                    break;
                               
                                default:
                                    notificationUtil.error(data.message);
                                    $('#btn-submit').removeAttr("disabled");
                                    break;
                            }
                        }

                    }




                }
            };

            let url = '';
            let id = $('#Id').val();

            if (id) {
                url = "Department/UpdateDepartment";
            } else {
                url = "Department/SaveDepartment";

            }

            jqClientAdvanced(options).PostFormData(url, requestdata);


        }
    });



});



