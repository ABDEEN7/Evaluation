
let showMore = false, table = null, dialogElem = null, searchParams = {}, pageNumber = 0;
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
        url: "SystemSetting/GetAllSystemSetting",

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
            field: 'title',
            header: sharedFn().GetUiControlText('SystemSettingTitle'),
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





$(window).scroll(function () {
    if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
        if (!isLoading) {
            loadData(true);
        }
    }
});
$(document).ready(function () {

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
                    if(response)
                    {
                        var data=response.data;
                        if(data)
                        {
                            var {responseStatus}=data;
                            //debugger
                    switch (responseStatus) {
                        

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
                    
                    

                   
                }
            };

            jqClientAdvanced(options).PostFormData("SystemSetting/UpdateSystemSetting", requestdata);
           


        }
    });



});



