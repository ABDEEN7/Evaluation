
let showMore = false, table = null, dialogElem = null, tree = null, selectedtree = null, searchParams = {}, pageNumber = 0;
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let currentPage = 0;
let isSearch = false;
let isLoading = true;
let isfirstload = true;
var popupname = "";

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

function SetDropDown() {
    $("#UserPartyTypeSignatureSignature_image").on('load', function () {

        var upl = document.getElementById("UserPartyTypeSignatureSignature");
        var fileheight = settingList.find(x => x.settingKey == 'UserPartyTypeSignatureHeight').settingValue;
        var filewidth = settingList.find(x => x.settingKey == 'UserPartyTypeSignatureWidth').settingValue;

        if (this.width > filewidth) {
            
            notificationUtil.error(sharedFn().GetUiControlText('UserPartyTypeSignatureWidthError'));

            upl.value = "";
            $("#UserPartyTypeSignatureSignature_image").attr("src", "");
            return;
        }
        if (this.height > fileheight) {
            notificationUtil.error(sharedFn().GetUiControlText('UserPartyTypeSignatureHeightError'));
            upl.value = "";
            $("#UserPartyTypeSignatureSignature_image").attr("src", "");
            return;
        }
    });
}

function SignatureUpdateClick (event){
    notificationUtil.confirmation({ title: sharedFn().GetUiControlText('ADMIN_WARNING_UPDATE'), okText: sharedFn().GetUiControlText('ADMIN_UPDATE'), cancelText: sharedFn().GetUiControlText('ADMIN_CANCEL') }, result => {
        const cellElem = event.closest('section');
        const Id = cellElem.getAttribute('data-key');
        const obj = table.getData().find(f => f.id == Id);
        var formData = new FormData();
        const data = {
            IsActive: obj.isActive,
            Id: obj.id,
            Signature: obj.signature
        };
        formData.append('request', JSON.stringify(data));
        const options = {
            success: function (response) {
                if (response.data.stateStatus == false) {
                    notificationUtil.error(response.data.message);
                    $('#btn-submit_popup').removeAttr("disabled");
                    return;
                }
                commonUtil.btnProgress("btn-submit_popup", true);

                let { data } = response;

                if (response) {
                    if (data.responseStatus == '2') {
                        var tablerow = table.getRows()
                            .filter(row => row.getData().id == obj.id)[0];
                        table.updateRow(tablerow, { isActive: data.isActive });
                        var datanew = table.getData();
                        table.setData(datanew);

                        notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
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
        jqClientAdvanced(options).PostFormData("UserPartyType/UpdateUserPartyTypeSignature", formData);
    });
};

const loadData = (reqData, isScroll) => {
    if (isScroll) {
        showMore = false;
        commonUtil.createLoader(progressBarAppendId, true);
    } else {
        Showloader(true);
    }

    jqClient.Post({
        url: "UserPartyType/GetAllUserPartyType",

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



const deleteData = (id, urlname = null) => {
    if (urlname == null) {
        if (popupname == "Signature") {
            urlname = "UserPartyType/DeleteUserPartyTypeSignature";
        }
        else {
            urlname = "UserPartyType/DeleteUserPartyType";

        }
    }
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
        jqClientAdvanced(options).Post(urlname.concat('?Id=', id));

    });


};

function Loadtabledata() {
    var userpartytypeid = $("#userpartytypeid").val();
    if (popupname == "Signature") {
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

        jqClientAdvanced(options).Get("UserPartyType/GetAllUserPartyTypeSignature".concat('?userpartytypeid=', userpartytypeid));
    }

}
function SignatureClick(event) {
    const cellElem = event.closest('section');
    const userpartytypeid = cellElem.getAttribute('data-key');
    var maintable = Tabulator.prototype.findTable("#" + gridContainerId)[0];
    const obj = maintable.getData().find(f => f.id == userpartytypeid);
    IsEdit = '';
    IsDelete = IsDelete_Signature;
    IsView = '';
    containsOrderNo = 'False';
    var IsEditItem = IsEdit_Signature == "True" ?
        `<span class="Signature pointer" title="` + sharedFn().GetUiControlText('ADMIN_TOOLTIP_EDIT_SIGNATURE') + `"><i class="save fa fa-save" onclick="SignatureUpdateClick(this)">
</i></span>`
        : '';
    var Signaturetabulatorcolumns = sharedFn().PopulateColumn(SignaturecolumnList, IsEditItem);
    var modaltitle = sharedFn().GetUiControlText('SignatureHeader');
    var name = lang == "ar" ? obj.nameAr : obj.nameEn;
    var title = commonUtil.stringFormat(modaltitle, name);
    sharedFn().OpenFormPopup(title, Signaturecontrolvalidationlist, null, Signaturetabulatorcolumns, settingList);
    $("#userpartytypeid").val(userpartytypeid);
    popupname = "Signature";
   

}

$('#btn-submit_popup').click(function () {
    if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {
        commonUtil.btnProgress("btn-submit_popup");
        var requestdata = sharedFn().GetSaveObject(Signaturecontrolvalidationlist, $('#Id').val());
            const options = {
                success: function (response) {
                    if (response.data.stateStatus == false) {
                        notificationUtil.error(response.data.message);
                        $('#btn-submit_popup').removeAttr("disabled");
                        return;
                    }
                    commonUtil.btnProgress("btn-submit_popup", true);

                    let { data } = response;

                    if (response) {
                        if (data.responseStatus == '1') {
                            sharedFn().ResetVisibleControls("PopupForm");
                            table.setData([]);
                            Loadtabledata();
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                        }
                        else {
                            notificationUtil.error(data.message);
                            $('#btn-submit_popup').removeAttr("disabled");
                        }

                    }

                },
                error: function (xhr) {
                    notificationUtil.error(xhr.responseJSON.Message);
                    $('#btn-submit_popup').removeAttr("disabled");
                }
            };
        jqClientAdvanced(options).PostFormData("UserPartyType/SaveUserPartyTypeSignature", requestdata);





    }


});




$(window).scroll(function () {
    if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
        if (!isLoading) {
            loadData();
        }
    }
});
const getLookup = () => {

    if (controlvalidationlist) {
        var dropdownlist = controlvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN');
        if (dropdownlist.length > 0) {
            dropdownlist.forEach(item => {
                var constrain = item.constraint;
                if (constrain.controlName == "UserId") {
                    const options1 = {
                        success: function (result) {
                            if (result) {
                                const { data } = result;
                                if (data) {
                                    const { User } = data;
                                    const ddlData = User.map(item => (
                                        {
                                            id: item.id,
                                            text: txtDir === "RTL" ? item.nameAr : item.nameEn
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
                                    })
                                    $dropdown.val('').trigger('change');
                                }
                            }
                        }
                    };
                    jqClientAdvanced(options1).Get("UserPartyType/GetAllUserList");
                }

            });
        }
    }
    const options = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { SystemModule } = data;
                    const ddlData = SystemModule.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.nameAr : item.nameEn
                        }
                    ));
                    const ddlElm = document.querySelector(`[data-key="systemModuleId"]`);
                    if (ddlElm) {
                        const dropdown = '#' + ddlElm.getAttribute('id');
                        $(dropdown).select2({
                            width: 'resolve',
                            allowClear: true,
                            data: ddlData,
                            placeholder: sharedFn().GetUiControlText('UserPartyTypeSystemModuleId'),
                            dropdownCssClass: "manageselect2zindex"
                        })
                        $(dropdown).val('').trigger('change');
                    }


                }
            }
        }
    };
    jqClientAdvanced(options).Get("UserPartyType/GetSystemModule");

};
const searchColsDef = () => {
    return [
        {
            field: 'systemModuleId',
            header: sharedFn().GetUiControlText('UserPartyTypeSystemModuleId'),
            type: 'DROPDOWN',
            collections: []
        },
        {
            field: 'userName',
            header: sharedFn().GetUiControlText('UserPartyTypeUserName'),
            type: 'TEXT_BOX'
        },
        {
            field: 'title',
            header: sharedFn().GetUiControlText('UserPartyTypeTitle'),
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


$(document).ready(function () {
    getLookup();
    columnSearch('search-panel-userrole', searchColsDef(), {});
    var viewItem = IsView_Signature == "True" ?
        `<span class="Signature pointer" title="` + sharedFn().GetUiControlText('ADMIN_TOOLTIP_VIEW_SIGNATURE') + `"><i class="signature fa fa-file-signature" onclick="SignatureClick(this)">
</i></span>`
        : '';

    let TableColumns = sharedFn().PopulateColumn(columnList, viewItem);
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
        sharedFn().SetDefaultValueFromConfig();
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
                    else if (data.responseStatus == '9') {
                        notificationUtil.error(sharedFn().GetUiControlText('NAVBARS_EXCEEDED'));
                        return;
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
                url = "UserPartyType/UpdateUserPartyType";
            } else {
                url = "UserPartyType/SaveUserPartyType";

            }
            jqClientAdvanced(options).PostFormData(url, requestdata);
           


        }
    });



});



