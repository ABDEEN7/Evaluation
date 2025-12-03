 

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
     

function ClearControlByPage() {
    getLookup();
   
}
            const loadData = (reqData, isScroll) => {
                if (isScroll) {
                    showMore = false;
                    commonUtil.createLoader(progressBarAppendId, true);
                } else {
                    Showloader(true);
                }

                jqClient.Post({
                    url: "SiteContent/GetAllSiteContent",
                    
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
                jqClientAdvanced(options).Post("SiteContent/DeleteSiteContent".concat('?Id=', id));

            });
        };

          

            const getLookup = () => {
                if (controlvalidationlist) {

                    //initializing FILEUPLOAD
                    var dropdownlist = controlvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN');
                    if (dropdownlist.length > 0) {
                        dropdownlist.forEach(item => {
                            var constrain = item.constraint;
                            if (constrain.controlName == "NavbarId") {
                                const options = {
                                    success: function (result) {
                                        if (result) {
                                            const { data } = result;
                                            if (data) {
                                                const { Navbar } = data;
                                                const ddlData = Navbar.map(item => (
                                                    {
                                                        id: item.id,
                                                        text: txtDir === "RTL" ? item.titleAr : item.titleEn
                                                    }
                                                ));
                                                var $dropdown = $('#' + constrain.uibackendName);
                                                $dropdown.empty();
                                                $dropdown.select2({
                                                    width: 'resolve',
                                                    allowClear: true,
                                                    data: ddlData,
                                                    placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                                                    dropdownCssClass: "manageselect2zindex"
                                                });

                                                var datavalue = $dropdown.attr("data-value");
                                                if (datavalue) {
                                                    $dropdown.val(datavalue).trigger('change');
                                                }
                                                else {
                                                    $dropdown.val('').trigger('change');
                                                }
                                                
                                                
                                            }
                                        }
                                    }
                                };
                                jqClientAdvanced(options).Get("SiteContent/GetNavbar");
                                const options1 = {
                                    success: function (result) {
                                        if (result) {
                                            const { data } = result;
                                            if (data) {
                                                const { Navbar } = data;
                                                const ddlData = Navbar.map(item => (
                                                    {
                                                        id: item.id,
                                                        text: txtDir === "RTL" ? item.titleAr : item.titleEn
                                                    }
                                                ));
                                                const ddlElm = document.querySelector(`[data-key="navbarId"]`);
                                                if (ddlElm) {
                                                    const ddlId = ddlElm.getAttribute('id');
                                                    $('#' + ddlId).empty();
                                                    $('#' + ddlId).select2({
                                                        width: '100%',
                                                        dir: "ltr",
                                                        dropdownAutoWidth: true,
                                                        data: ddlData,
                                                        placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                                                        allowClear: true,
                                                        dropdownCssClass: "manageselect2zindex"
                                                    });
                                                    $('#' + ddlId).val('').trigger('change');
                                                }

                                            }
                                        }
                                    }
                                };
                                jqClientAdvanced(options1).Get("SiteContent/GetNavbarListFromSiteContent");
                            }
                            else if (constrain.controlName == "ParentId") {
                                const options = {
                                    success: function (result) {
                                        if (result) {
                                            const { data } = result;
                                            if (data) {
                                                const { Parent } = data;
                                                parentList = Parent;
                                                const ddlData = Parent.map(item => (
                                                    {
                                                        id: item.id,
                                                        text: txtDir === "RTL" ? item.titleAr : item.titleEn
                                                    }
                                                ));
                                                var $dropdown = $('#' + constrain.uibackendName);
                                                $dropdown.empty();
                                                $dropdown.select2({
                                                    width: 'resolve',
                                                    allowClear: true,
                                                    data: ddlData,
                                                    placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                                                    dropdownCssClass: "manageselect2zindex"
                                                }).on("change", event => {
                                                    const { value } = event.target;
                                                    if (!value || value.trim().length <= 0) return;
                                                    if (value > 0) {
                                                        $('#SiteContentNavbar').parent().hide();
                                                    }
                                                    else {
                                                        $('#SiteContentNavbar').parent().show();
                                                    }
                                                });
                                                
                                                var datavalue = $dropdown.attr("data-value");
                                                if (datavalue) {
                                                    $dropdown.val(datavalue).trigger('change');
                                                }
                                                else {
                                                    $dropdown.val('').trigger('change');
                                                }
                                                sharedFn().DisableDropdownOptions("SiteContentParent", $('#Id').val());
                                            }
                                        }
                                    }
                                };
                                jqClientAdvanced(options).Get("SiteContent/GetRawSiteContentList");
                            }
                            
                        });
                    }
                }
            };


         
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
                        field: 'title',
                        header: sharedFn().GetUiControlText('SiteContentTitle'),
                        type: 'TEXT_BOX'
                    },
                    {
                        field: 'navbarId',
                        header: sharedFn().GetUiControlText('SiteContentNavbar'),
                        type: 'DROPDOWN',
                        collections: []
                    },
                    {
                        field: 'userName',
                        header: sharedFn().GetUiControlText('SiteContentCreatedBy'),
                        type: 'TEXT_BOX'
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
                     jqClientAdvanced(options).PostFormData("SiteContent/UpdateSiteContentOrder", formData);

                    }
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
                        sharedFn().EnableDropdownOptions("SiteContentParent");
                        getLookup();

                    });
               

                $("#btn-submit").click(function (e) {

                    if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {
                        commonUtil.btnProgress(btnSubmitId);

                        $('#btn-submit').attr("disabled", "disabled");
                        var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());
                        const options = {
                            success: function (response) {
                                if (response.data.stateStatus == false) {
                                    notificationUtil.error(response.data.message);
                                    $('#btn-submit').removeAttr("disabled");
                                    return;
                                }
                                $('#btn-submit').removeAttr("disabled");

                                if (response) {

                                    let { data } = response;
                                    if (data.responseStatus == '1') {
                                        table.addData([data], true);
                                        table.deselectRow();
                                        table.getRows()[0].select();
                                        $formSection.hide();
                                        $tblContentContainer.show();
                                        $("#parentHint").text("");
                                        $('#btn-add-content').removeAttr("disabled");
                                        $('#btn-submit').removeAttr("disabled");
                                        if (response) {
                                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));

                                        }
                                    }
                                    else if (data.responseStatus == '2') {
                                        table.updateData([data]);
                                        $formSection.hide();
                                        $tblContentContainer.show();
                                        $("#parentHint").text("");
                                        $('#btn-add-content').removeAttr("disabled");
                                        $('#btn-submit').removeAttr("disabled");
                                        if (response) {
                                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                                        }

                                    }
                                    else if (data.responseStatus == '4') {
                                        notificationUtil.error(sharedFn().GetUiControlText('SITECONTENT_ROUTING_EXISTS'));
                                        return;
                                    }
                                    else if (data.responseStatus == '9') {
                                        notificationUtil.error(sharedFn().GetUiControlText('SITECONTENT_EXCEEDED'));
                                        return;
                                    }
                                    else if (data.responseStatus == '10') {
                                        notificationUtil.error(sharedFn().GetUiControlText('SITECONTENT_CANT_BE_PARENT'));
                                        return;
                                    }
                                    else if (data.responseStatus == '17') {
                                        notificationUtil.error(sharedFn().GetUiControlText('SAME_RECORD_CANNOT_ADD_AS_PARENT'));
                                        return;
                                    }
                                    else {
                                        $('#btn-submit').removeAttr("disabled");
                                        notificationUtil.error(data.statusmessage);

                                    }
                                }
                            },
                            error: function (xhr) {
                                notificationUtil.error(xhr.responseJSON.Message);
                                $('#btn-submit').removeAttr("disabled");
                                return;
                            }
                        };

                        let url = '';
                        let id = $('#Id').val();

                        if (id) {
                            url = "SiteContent/UpdateSiteContent";
                        } else {
                            url = "SiteContent/SaveSiteContent";

                        }
                        jqClientAdvanced(options).PostFormData(url, requestdata);
                      
                    }
                });
               

               
               

               



            });

   

   