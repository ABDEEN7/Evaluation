 

        let  pageNumber = 0, searchParams = {},
            showMore = false, table = null, dialogElem = null;
        const dailogId = commonUtil.CONTENT_DAILOG_ID;
        let currentPage = 0;
        let isSearch = false;
        let isLoading = true;
        let tabclick = 1;  
        let parentList = [];
       
       

        const btnAddContentId = 'btn-add-content',
            searchPanelSectionId = "search-panel-section",
            progressBarAppendId = "scroll-progress-bar",
               btnSubmitId = "btn-submit",
            $formSection = $('#form-section')
            ;

        const gridContainerId = "view-container",
            tblContentContainerId = "",
            $tblContentContainer = $('#tbl-template-container'),
            $btnSubmit = $('#'+ btnSubmitId),
            $btnAddContent = $('#' + btnAddContentId);
     

        
            const loadData = (reqData, isScroll) => {
                if (isScroll) {
                    showMore = false;
                    commonUtil.createLoader(progressBarAppendId, true);
                } else {
                    Showloader(true);
                }

                jqClient.Post({
                    url: "UserPermission/GetAllUser",
                    
                    config: {
                        dataType: 'json'
                    },
                    jsonData: reqData,
                    onError: (xhr) => {
                        if (isScroll) {
                            showMore = true;
                            commonUtil.createLoader(progressBarAppendId);
                        } else {
                            Showloader(false);
                        }
                    },
                    onSuccess: (data) => {
                        if (isScroll) {
                            showMore = true;
                            commonUtil.createLoader(progressBarAppendId);
                        } else {
                            Showloader(false);
                            showMore = true;
                        }
                        if (data) {
                            //const { data } = JSON.parse(result);
                            
                            const _isInit = isScroll ? false : true;
                            if (!data || data.length <= 0) {
                                showMore = false;
                                if (!isScroll) {
                                    table.setData([]);
                                }
                                commonUtil.createLoader(progressBarAppendId, false, true);
                                return;
                            } else {
                                showMore = true;
                                if (_isInit) {
                                    table.setData(data).then(function () {
                                        setAllColumnWidths(table, columnWidths); 
                                    });
                                } else {
                                    table.addData(data)
                    .then(function () {
                                            setAllColumnWidths(table, columnWidths); 
                    });

                                }
                            }
                        }
                    }
                });
                
            };


function ClearControlByPage() {
    $("#UserPermissionEmail").attr("disabled", "disabled");
    $("#UserPermissionQID").attr("disabled", "disabled");

}

          

           
         
            $(window).scroll(function () {
                if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
                    if (!isLoading && currentPage>0) {
                        loadData();
                    }
                }
            });
            const searchColsDef = () => {
                return [
                    {
                        field: 'email',
                        header: sharedFn().GetUiControlText('UserPermissionSearchEmail'),
                        type: 'TEXT_BOX'
                    },
                    {
                        field: 'title',
                        header: sharedFn().GetUiControlText('UserPermissionTitle'),
                        type: 'TEXT_BOX'
                    },
                    {
                        field: 'roleId',
                        header: sharedFn().GetUiControlText('UserPermissionSearchRole'),
                        type: 'DROPDOWN',
                        collections: []
                    },
                    {
                        field: 'isActive',
                        header: sharedFn().GetUiControlText('UserPermissionStatus'),
                        type: 'DROPDOWN',
                        collections: []
                    },
                    


                ];
            };
            const btnSeachEvent = (searchEvent) => {
                pageNumber = 0;
                const { data, event, params } = searchEvent;
                if ($.isEmptyObject(data)) {
                   
                    notificationUtil.error(sharedFn().GetUiControlText('SEARCHVALIDATION'));
                    return;
                }
                const reqData = { ...data, ...params, pageNum: pageNumber };
                searchParams = reqData;
                loadData(reqData, false);
            };

            const btnClearEvent = (event) => {
                searchParams = {};
                pageNumber = 0;
                loadData({ pageNum: pageNumber }, false);
            };
            const columnSearch = (ctrlId, colDef, params) => {
                const searchPanel = columnSearchUtil.createColumnSearch({
                    ctrlId, colDef, params,
                    actions: [
                        { btnId: 'btn-seach-clear', text: sharedFn().GetUiControlText('CLEAR_BUTTON'), eventName: btnClearEvent, type: 'CLEAR' },
                        { btnId: 'btn-seach-start', text: sharedFn().GetUiControlText('SEARCH_BUTTON'), eventName: btnSeachEvent, type: 'SEARCH' }
                    ]
                });

                $('#' + searchPanelSectionId).empty().append(searchPanel);
                

};

const getLookup = () => {
    const options = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { RoleList } = data;
                    const ddlData = RoleList.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.nameAr : item.nameEn
                        }
                    ));
                    const ddlElm = document.querySelector(`[data-key="roleId"]`);
                    if (ddlElm) {
                        const dropdown = '#' +ddlElm.getAttribute('id');
                        $(dropdown).select2({
                            width: 'resolve',
                            allowClear: true,
                            data: ddlData,
                            placeholder: sharedFn().GetUiControlText('UserPermissionSearchRole'),
                            dropdownCssClass: "manageselect2zindex"
                        })
                        $(dropdown).val('').trigger('change');
                    }
                   
                    
                }
            }
        }
    };
    jqClientAdvanced(options).Get("UserPermission/GetRoleList");
    const ddlstatus = document.querySelector(`[data-key="isActive"]`);
    if (ddlstatus) {
        const dropdownddlstatus = '#' +ddlstatus.getAttribute('id');
        $(dropdownddlstatus).append('<option value="1">Active</option>');
        $(dropdownddlstatus).append('<option value="0">InActive</option>');
        $(dropdownddlstatus).select2({
            width: 'resolve',
            allowClear: true,
            placeholder: sharedFn().GetUiControlText('UserPermissionStatus'),
            dropdownCssClass: "manageselect2zindex"
        })
        $(dropdownddlstatus).val('').trigger('change');
    }
};
            $(document).ready(function () {
               
                columnSearch('search-panel-userrole', searchColsDef(), {});

               

                getLookup();
                table = tableUtil.createTabulator({
                    id: gridContainerId,
                    config: {
                        textDirection: txtDir,
                        paginationSize: 10,
                        placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
                        headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),
                        movableRows: true,
                    },
                    isResponsiveLayout:false,
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
                    
                });

                dialogElem = commonUtil.createDailog({ dailogId: dailogId });

                loadData({ pageNum: pageNumber }, false);
               
                commonUtil.infiniteScroll(null, () => {
                    if (showMore) {
                        pageNumber = pageNumber + 1;
                        searchParams = {
                            ...searchParams, pageNum: pageNumber
                        };
                        
                        loadData(searchParams, true);
                    }
                });

                    document.getElementById(btnAddContentId).addEventListener('click', event => {
                        sharedFn().ClearForm();
                        sharedFn().EditMode();
                        sharedFn().SetDefaultValueFromConfig();
                    });
               

                $("#btn-submit").click(function (e) {

                    if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {
                        commonUtil.btnProgress(btnSubmitId);

                        $('#btn-submit').attr("disabled", "disabled");
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
                                            case 8:
                                               
                                                notificationUtil.error(sharedFn().GetUiControlText('QID_ALREADY_EXISTS'));
                                                $('#btn-submit').removeAttr("disabled");
                                                break;
                                            case 4:

                                                notificationUtil.error(sharedFn().GetUiControlText('EMAIL_ALREADY_EXISTS'));
                                                $('#btn-submit').removeAttr("disabled");
                                                break;
                                            default:
                                                notificationUtil.error(data.message);

                                                break;
                                        }
                                    }
                                    $('#btn-submit').removeAttr("disabled");
                                }




                            }
                        };

                        let url = '';
                        let id = $('#Id').val();

                        if (id) {
                            url = "UserPermission/UpdateUser";
                        } else {
                            url = "UserPermission/SaveUser";

                        }

                        jqClientAdvanced(options).PostFormData(url, requestdata);
                    }
                });
               

               
               

               



            });

   

   