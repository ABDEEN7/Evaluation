
let showMore = false, table = null, searchParams = {}, dialogElem = null, pageNumber = 0;
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let currentPage = 0;
let isSearch = false;
let isLoading = true;


const btnAddContentId = 'btn-add-content',
    btnSubmitId = "btn-submit",
    $formSection = $('#form-section'),
    searchPanelSectionId = "search-panel-section",
    progressBarAppendId = "scroll-progress-bar"
    ;

const gridContainerId = "view-container",
    tblContentContainerId = "tbl-template-container",
    $tblContentContainer = $('#' + tblContentContainerId),
    $btnAddContent = $('#' + btnAddContentId);


const loadData = (reqData, isScroll) => {
    if (isScroll) {
        showMore = false;
        commonUtil.createLoader(progressBarAppendId, true);
    } else {
        Showloader(true);
    }

    jqClient.Post({
        url: "Employees/GetAllEmployees",

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
const searchColsDef = () => {
    return [
        {
            field: 'orgClassId',
            header: sharedFn().GetUiControlText('EmployeesOrgClass'),
            type: 'DROPDOWN',
            collections: []
        },
        {
            field: 'title',
            header: sharedFn().GetUiControlText('EmployeesTitle'),
            type: 'TEXT_BOX'
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
            },
            error: function (xhr) {
                notificationUtil.error(xhr.responseJSON.Message);

            }
        };
        jqClientAdvanced(options).Post("Employees/DeleteEmployees".concat('?Id=', id));

    });


};




const gettreedata = () => {


    const options = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { OrgClass } = data;
                    const ddlData = OrgClass.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.nameAr : item.nameEn
                        }
                    ));
                    const ddlElm = document.querySelector(`[data-key="orgClassId"]`);
                    if (ddlElm) {
                        const dropdown = '#' + ddlElm.getAttribute('id');
                        $(dropdown).select2({
                            width: 'resolve',
                            allowClear: true,
                            data: ddlData,
                            placeholder: sharedFn().GetUiControlText('EmployeesOrgClass'),
                            dropdownCssClass: "manageselect2zindex"
                        })
                        $(dropdown).val('').trigger('change');

                        $('#EmployeesOrgClassId').select2({
                            width: 'resolve',
                            allowClear: true,
                            data: ddlData,
                            placeholder: sharedFn().GetUiControlText('EmployeesOrgClassId'),
                            dropdownCssClass: "manageselect2zindex"
                        })
                        $('#EmployeesOrgClassId').val('').trigger('change');

                    }


                }
            }
        }
    };
    jqClientAdvanced(options).Get("Employees/GetOrgClass");

}




$(window).scroll(function () {
    if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
        if (!isLoading) {
            loadData();
        }
    }
});



$(document).ready(function () {


    gettreedata();
    columnSearch('search-panel-userrole', searchColsDef(), {});
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



    $(`#${btnAddContentId}`).click(function (e) {
        sharedFn().ClearForm();
        sharedFn().EditMode();
        sharedFn().SetValueFromDropdown();
        sharedFn().SetValueToDropdown();

    });
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
                        table.deselectRow();
                        table.getRows()[0].select();
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
                url = "Employees/UpdateEmployees";
            } else {
                url = "Employees/SaveEmployees";

            }
            jqClientAdvanced(options).PostFormData(url, requestdata);



        }
    });



});



