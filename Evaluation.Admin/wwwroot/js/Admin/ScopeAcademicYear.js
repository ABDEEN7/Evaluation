let showMore = false, table = null, dialogElem = null, tree = null, selectedtree = null;
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let currentPage = 0;
let isSearch = false;
let isLoading = true;
let searchParams = {};
let pageNumber = 0;

const btnAddContentId = 'btn-add-content',
    btnSubmitId = "btn-submit",
    $formSection = $('#form-section'),
    searchPanelSectionId = "search-panel-section",
    progressBarAppendId = "scroll-progress-bar";

const gridContainerId = "view-container",
    tblContentContainerId = "tbl-template-container",
    $tblContentContainer = $('#' + tblContentContainerId),
    $btnAddContent = $('#' + btnAddContentId);


const loadData = (reqData, isScroll) => {
    isLoading = true;

    if (isScroll) {
        showMore = false;
        commonUtil.createLoader(progressBarAppendId, true);
    } else {
        Showloader(true);
    }

    const options = {
        success: function (data) {
            if (isScroll) {
                showMore = true;
                commonUtil.createLoader(progressBarAppendId);
            } else {
                Showloader(false);
                showMore = true;
            }

            if (data) {
                const _isInit = !isScroll;
                if (!data || data.length <= 0) {
                    showMore = false;
                    if (!isScroll) table.setData([]);
                    commonUtil.createLoader(progressBarAppendId, false, true);
                    return;
                } else {
                    isLoading = false;
                    showMore = true;
                    if (_isInit) {
                        table.setData(data);
                    } else {
                        table.addData(data);
                    }
                }
            }
        },
        error: function (xhr) {
            if (isScroll) {
                showMore = true;
                commonUtil.createLoader(progressBarAppendId);
            } else {
                Showloader(false);
            }
        }
    };

    jqClientAdvanced(options).Post("ScopeAcademicYear/GetAllScopeAcademicYear", reqData);
};


const searchColsDef = () => {
    return [
        {
            field: 'departmentId',
            header: sharedFn().GetUiControlText('DepartmentClass'),
            type: 'DROPDOWN',
            collections: []
        }
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


const getDepartmentClass = () => {
    const options = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { DepartmentClass } = data;
                    const ddlData = DepartmentClass.map(item => ({
                        id: item.id,
                        text: txtDir === "RTL" ? item.nameAr : item.nameEn
                    }));

                    
                    const ddlElm = document.querySelector(`[data-key="departmentId"]`);
                    if (ddlElm) {
                        const dropdown = '#' + ddlElm.getAttribute('id');
                        $(dropdown).select2({
                            width: 'resolve',
                            allowClear: true,
                            data: ddlData,
                            placeholder: sharedFn().GetUiControlText('DepartmentClass'),
                            dropdownCssClass: "manageselect2zindex"
                        });
                        $(dropdown).val('').trigger('change');
                    }
                    $('#DepartmentId').select2({
                        width: 'resolve',
                        allowClear: true,
                        data: ddlData,
                        placeholder: sharedFn().GetUiControlText('DepartmentClass'),
                        dropdownCssClass: "manageselect2zindex"
                    });
                    $('#DepartmentId').val('').trigger('change');
                }
            }
        }
    };
    jqClientAdvanced(options).Get("Department/GetDepartmentClass");
};


const deleteData = (id, event, cell) => {
    if (!id) return;

    const obj = table.getData().find(f => f.id == id);
    if (!obj) return;
    notificationUtil.confirmation({ title: sharedFn().GetUiControlText('ADMIN_WARNING_DELETE'), okText: sharedFn().GetUiControlText('DELETE_BUTTON'), cancelText: sharedFn().GetUiControlText('ADMIN_CANCEL') }, result => {
        if (!id) return;
        const options = {
            success: function (data) {
                table.deleteRow(id);
                notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));
            },
            error: function (xhr) {
                notificationUtil.error(xhr.responseJSON.Message);
            }
        };
        jqClientAdvanced(options).Post("ScopeAcademicYear/DeleteScopeAcademicYear".concat('?Id=', id));
    });
};


$(window).scroll(function () {
    if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
        if (!isLoading) {
            loadData(searchParams, true);
        }
    }
});


$(document).ready(function () {

    columnSearch('search-panel-scopeacademicyear', searchColsDef(), {});

    
    getDepartmentClass();

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
        columns: TableColumns
    });

    dialogElem = commonUtil.createDailog({ dailogId: dailogId });

    loadData({ pageNum: pageNumber }, false);

    commonUtil.infiniteScroll(null, () => {
        if (showMore) {
            pageNumber = pageNumber + 1;
            searchParams = { ...searchParams, pageNum: pageNumber };
            loadData(searchParams, true);
        }
    });

    $(`#${btnAddContentId}`).click(function (e) {
        sharedFn().ClearForm();
        sharedFn().EditMode();
        sharedFn().SetDefaultValueFromConfig();
    });

    $("#btn-submit").click(function (e) {

        if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {

            commonUtil.btnProgress(btnSubmitId);
            var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());

            const options = {
                success: function (response) {
                    commonUtil.btnProgress(btnSubmitId, true);

                    if (response) {
                        var data = response.data;
                        if (data) {
                            var { responseStatus } = data;
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

                                default:
                                    notificationUtil.error(data.message);
                                    $('#btn-submit').removeAttr("disabled");
                                    break;
                            }
                        }
                    }
                },
                error: function (xhr) {
                    commonUtil.btnProgress(btnSubmitId, true);
                    notificationUtil.error(xhr.responseJSON.Message);
                }
            };

            let url = '';
            let id = $('#Id').val();

            if (id) {
                url = "ScopeAcademicYear/UpdateScopeAcademicYear";
            } else {
                url = "ScopeAcademicYear/SaveScopeAcademicYear";
            }

            jqClientAdvanced(options).PostFormData(url, requestdata);
        }
    });

});