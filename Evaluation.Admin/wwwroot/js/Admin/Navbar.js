
let showMore = false, table = null, dialogElem = null;
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let currentPage = 0;
let isSearch = false;
let isLoading = true;


const btnAddContentId = 'btn-add-content',
    btnSubmitId = "btn-submit",
    $formSection = $('#form-section')
    ;

const gridContainerId = "view-container",
    tblContentContainerId = "tbl-template-container",
    $tblContentContainer = $('#' + tblContentContainerId),
    $btnAddContent = $('#' + btnAddContentId);






function ClearControlByPage() {
 getLookup();
    if (controlvalidationlist) {

        //initializing FILEUPLOAD
        var IsInternal = controlvalidationlist.find(c => c.constraint.controlName == 'IsInternal');
        var Routing = controlvalidationlist.find(c => c.constraint.controlName == 'Routing');
        var UrlAr = controlvalidationlist.find(c => c.constraint.controlName == 'UrlAr');
        var UrlEn = controlvalidationlist.find(c => c.constraint.controlName == 'UrlEn');
        if ($('#' + IsInternal.constraint.uibackendName).prop("checked")) {
            $('#' + Routing.constraint.uibackendName).val($('#' + UrlAr.constraint.uibackendName).val()).trigger("change");
            $('#' + Routing.constraint.uibackendName).parent().show();
            $('#' + UrlAr.constraint.uibackendName).parent().hide();
            $('#' + UrlEn.constraint.uibackendName).parent().hide();
        }
        else {
            $('#' + Routing.constraint.uibackendName).val('').trigger("change");
            $('#' + Routing.constraint.uibackendName).parent().hide();
            $('#' + UrlAr.constraint.uibackendName).parent().show();
            $('#' + UrlEn.constraint.uibackendName).parent().show();
        }
        
        
    }
    
   
}




$("#NavbarIsAuthorized").on("change", function () {

    if (this.checked) {
        $("#NavbarPermissionId").parent().show();

    }
    else {
        $("#NavbarPermissionId").parent().hide();

        $("#NavbarPermissionId").val('').trigger('change');


    }
})

const loadData = (isScroll) => {
    var parent = $('#NavbarParentSearch').val() == null ? "0" : $('#NavbarParentSearch').val();
    isLoading = true;
    const options = {
        success: function (data) {
            if (isScroll) {
                showMore = true;
            } else {

                showMore = true;
            }
            if (data) {

                const _isInit = isScroll ? false : true;
                if (!data || data.length <= 0) {
                    showMore = false;

                    if (!isScroll) {
                        table.setData([]);
                    }
                    return;
                } else {
                    isLoading = false;
                    currentPage++;
                    showMore = true;
                    if (_isInit) {
                        table.setData(data);
                    } else {
                        table.addData(data);
                    }
                }
            }
        }
    };
    jqClientAdvanced(options).Get("Navbar/GetAllNavbar".concat('?id=', parent).concat('&Page=', currentPage));

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
        jqClientAdvanced(options).Post("Navbar/DeleteNavbar".concat('?Id=', id));

    });


};


const getLookup = () => {

    if (controlvalidationlist || SearchuiControlItems) {
        var newlist = $.merge($.merge([], controlvalidationlist), SearchuiControlItems);
        //initializing FILEUPLOAD
        var dropdownlist = newlist.filter(c => c.constraint.controlType == 'DROPDOWN');
        if (dropdownlist.length > 0) {
            dropdownlist.forEach(item => {
                var constrain = item.constraint;
                if (constrain.controlName == "Routing") {

                    const options = {
                        success: function (result) {
                            if (result) {
                                const { data } = result;
                                if (data) {
                                    const { Routing } = data;
                                    const ddlData = Routing.map(item => (
                                        {
                                            id: item.routing,
                                            text:( txtDir === "RTL" ? item.titleAr : item.titleEn) + " (  " + item.routing + " )"
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
                                    var datavalue = $("#NavbarUrlAr").val();
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
                    jqClientAdvanced(options).Get("Navbar/GetRoutingList");
                }
                else if (constrain.controlName == "ParentSearchId") {
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
                                        allowClear: true,
                                        width: 'resolve',

                                        data: ddlData,
                                        placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                                        dropdownCssClass: "manageselect2zindex",

                                    }).on("change", event => {
                                        currentPage = 0;
                                        loadData(false);
                                    }).on("select2:unselecting", function (e) {
                                        currentPage = 0;
                                        loadData(false);

                                    });
                                    $dropdown.val('').trigger('change');
                                }
                            }
                        }
                    };
                    jqClientAdvanced(options).Get("Navbar/GetAllParentNavbar");

                }
                else if (constrain.controlName == "ParentId") {
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
                                    var $dropdown = $('#' + constrain.uibackendName);
                                    $dropdown.empty();
                                    $dropdown.select2({
                                        width: 'resolve',
                                        allowClear: true,
                                        data: ddlData,
                                        placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                                        dropdownCssClass: "manageselect2zindex"
                                    })
                                    var datavalue = $dropdown.attr("data-value");
                                    if (datavalue) {
                                        $dropdown.val(datavalue).trigger('change');
                                    }
                                    else {
                                        $dropdown.val('').trigger('change');
                                    }
                                    sharedFn().DisableDropdownOptions("NavbarParent", $('#Id').val());
                                }
                            }
                        }
                    };
                    jqClientAdvanced(options1).Get("Navbar/GetNavbarListWithUpAndDownLevel");
                }
                else if (constrain.controlName == "PermissionId") {
                    const options1 = {
                        success: function (result) {
                            if (result) {
                                const { data } = result;
                                if (data) {
                                    const { NavBarPermission } = data;
                                    const ddlData = NavBarPermission.map(item => (
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
                    jqClientAdvanced(options1).Get("Navbar/GetNavbarPermissionList");
                }
            });
        }
    }


};

$(window).scroll(function () {
    if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
        if (!isLoading) {
            loadData(true);
        }
    }
});
$(document).ready(function () {

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
             jqClientAdvanced(options).PostFormData("Navbar/UpdateNavbarOrder", formData);

        }
    });

    dialogElem = commonUtil.createDailog({ dailogId: dailogId });

  
    if (controlvalidationlist) {

        //initializing FILEUPLOAD
        var IsInternal = controlvalidationlist.find(c => c.constraint.controlName == 'IsInternal');
        var Routing = controlvalidationlist.find(c => c.constraint.controlName == 'Routing');
        var UrlAr = controlvalidationlist.find(c => c.constraint.controlName == 'UrlAr');
        var UrlEn = controlvalidationlist.find(c => c.constraint.controlName == 'UrlEn');
        if (IsInternal) {
            $('#' + IsInternal.constraint.uibackendName).change(function () {

                $('#' + Routing.constraint.uibackendName).val('').trigger('change');
                $('#' + UrlAr.constraint.uibackendName).val('');
                $('#' + UrlEn.constraint.uibackendName).val('');
                if (this.checked) {
                    $('#' + Routing.constraint.uibackendName).parent().show();
                    $('#' + UrlAr.constraint.uibackendName).parent().hide();
                    $('#' + UrlEn.constraint.uibackendName).parent().hide();

                }
                else {
                    $('#' + Routing.constraint.uibackendName).parent().hide();
                    $('#' + UrlAr.constraint.uibackendName).parent().show();
                    $('#' + UrlEn.constraint.uibackendName).parent().show();
                }
            });
        }
        if (Routing) {
            $('#' + Routing.constraint.uibackendName).on('select2:select', function (e) {

                var routval = $('#' + Routing.constraint.uibackendName).val();
                $('#' + UrlAr.constraint.uibackendName).val(routval);
                $('#' + UrlEn.constraint.uibackendName).val(routval);
            });
        }
    }





    document.getElementById(btnAddContentId).addEventListener('click', event => {
        sharedFn().ClearForm();
        sharedFn().EditMode();
        sharedFn().SetDefaultValueFromConfig();
        ClearControlByPage();
        sharedFn().EnableDropdownOptions("NavbarParent");
        getLookup();

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
                    else if (data.responseStatus == '17') {
                        notificationUtil.error(sharedFn().GetUiControlText('SAME_RECORD_CANNOT_ADD_AS_PARENT'));
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
                url = "Navbar/UpdateNavbar";
            } else {
                url = "Navbar/SaveNavbar";

            }

            jqClientAdvanced(options).PostFormData(url, requestdata);
          

        }
    });



});



