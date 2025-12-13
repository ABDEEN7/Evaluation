
let showMore = false, table = null, dialogElem = null, tree = null, selectedtree=null;
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let currentPage = 0;
let isSearch = false;
let isLoading = true;
let pagePermissionTreeview = null;
let treeviewList = [];



$("#PermissionRole").on("change", function (event) {
    const { value } = event.target;
    if (!value || value.trim().length <= 0) return;

    getRolePermissions(value);
});
$("#PermissionRole").on("select2:unselecting", function () {
    pagePermissionTreeview.uncheckAll();
});
$("#checkall").click(function () {
    $('input:checkbox').not(this).prop('checked', this.checked);
});
$("#treeexpand").click(function () {
    pagePermissionTreeview.expandAll();
});
$("#treecollapse").click(function () {
    pagePermissionTreeview.collapseAll();
});
const getLookup = () => {

    const options = {
        success: function (result) {
            if (result) {
                var pagesPermissions = result;
                const group = pagesPermissions.reduce((acc, item) => {
                    const key = item.pageId;
                    if (!acc[key]) {
                        acc[key] = [];
                    }
                    acc[key].push(item);
                    return acc;
                }, {});

               
                for (let key in group) {
                    if (group.hasOwnProperty(key)) {
                        const { id, name } = group[key][0].page;
                        let rootElem = {
                            id: `data-page-${id}`,
                            text: name,
                            checkedFieldName: false,
                            imageCssClass: 'fa fa-file-text text-primary'
                        };
                        const children = [];
                        group[key].forEach(item => {
                            const { id, nameEn, nameAr } = item.permission;
                            children.push({
                                id: `data-permission-${id}`,
                                text: txtDir === "RTL" ? nameAr : nameEn,
                                checkedFieldName: false,
                                imageCssClass: 'fa fa-bolt text-primary'
                            });
                        });
                        rootElem = { ...rootElem, children };
                        treeviewList.push(rootElem);
                    }
                }

                pagePermissionTreeview = $("#PermissionPagePermission").tree({
                    primaryKey: 'id',
                    uiLibrary: 'bootstrap4',
                    iconsLibrary: "fontawesome",
                    dataSource: treeviewList,
                    checkboxes: true,
                    checkedField: 'checkedFieldName',
                });
                pagePermissionTreeview.expandAll();
                var count = document.querySelectorAll("#PermissionPagePermission li li").length;
                var text = sharedFn().GetUiControlText('TreeviewTotalCount')
                $("#treeviewtotalcount").html(text + count);
                var text1 = sharedFn().GetUiControlText('TreeviewTotalSelectedCount')
                $("#treeviewselectedcount").html(text1 + "0");
            }
        }
    };
    jqClientAdvanced(options).Get("Permission/GetAllPagePermission");

};
const getRolePermissions = (id) => {


    const options = {

        success: function (data) {


            if (data) {
                treeviewList.forEach(d => {
                    const { id, children } = d;
                    if (children && children.length > 0) {
                        children.forEach(item => {
                            const { id } = item;
                            const isAvl = data.find(f => f.permissionId == id.replace("data-permission-", ""));
                            if (isAvl) {
                                item.checkedFieldName = true;
                            } else {
                                item.checkedFieldName = false;
                            }
                        });
                    }
                });

                pagePermissionTreeview.destroy();
                pagePermissionTreeview = $("#PermissionPagePermission").tree({
                    primaryKey: 'id',
                    uiLibrary: 'bootstrap4',
                    iconsLibrary: "fontawesome",
                    dataSource: treeviewList,
                    checkboxes: true,
                    checkedField: 'checkedFieldName',
                });
                pagePermissionTreeview.expandAll();
                var count = pagePermissionTreeview.getCheckedNodes().length;
                var text = sharedFn().GetUiControlText('TreeviewTotalSelectedCount')
                $("#treeviewselectedcount").html(text + count);
            }
        }
    };


    jqClientAdvanced(options).Get("Permission/GetRolePermission".concat("?roleId=", id));

};
const FilterActions = (query) => {
    // Declare variables
    var  filter,div, ul, li, a, i, txtValue;
    filter = query.toUpperCase();
    div = document.getElementById("PermissionPagePermission");
    ul = div.getElementsByTagName('ul')[0];
    li = ul.getElementsByTagName('li');

    // Loop through all list items, and hide those who don't match the search query
    for (i = 0; i < li.length; i++) {
        a = li[i].getElementsByTagName("span")[6];
        txtValue = a.textContent || a.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";
        }
    }
}
$("#PermissionPagePermissionsearchInput").on('input', function () {
        var query = $(this).val().toLowerCase(); // Get the search query (convert to lowercase)
        FilterActions(query); // Call the search function
    });
$(document).ready(function () {
    
    
    
    getLookup();
    

    dialogElem = commonUtil.createDailog({ dailogId: dailogId });

    $("#btn-submit").click(function (e) {
        
        
        if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {


            var formdata = new FormData();
            var request = [];
            var checkedNodeIdList = pagePermissionTreeview.getCheckedNodes();
            if (checkedNodeIdList.length > 0) {
                checkedNodeIdList.forEach(item => {
                    if (item.includes("data-permission")) {
                        const pkId = item.replace("data-permission-", "");
                        var RolePermissionDTO = {};
                        RolePermissionDTO.RoleId = $("#PermissionRole").val();
                        RolePermissionDTO.PermissionId = pkId;
                        request.push(RolePermissionDTO);
                    }
                   

                });

            }
            else {
                notificationUtil.error(sharedFn().GetUiControlText('SELECT_RECORD_TREEVIEW'));
                return;
            }

            formdata.append('request', JSON.stringify(request));

            
           

            const options = {
                success: function (response) {
                    if (response) {
                        var data = response.data;
                        if (data) {
                            var { responseStatus } = data[0];
                            //debugger
                            switch (responseStatus) {
                                case 1:

                                    
                                    notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                                    var count = pagePermissionTreeview.getCheckedNodes().length;
                                    var text = sharedFn().GetUiControlText('TreeviewTotalSelectedCount')
                                    $("#treeviewselectedcount").html(text + count);
                                    break;

                                case 2:
                                    
                                    notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                                    var count = pagePermissionTreeview.getCheckedNodes().length;
                                    var text = sharedFn().GetUiControlText('TreeviewTotalSelectedCount')
                                    $("#treeviewselectedcount").html(text + count);
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

            jqClientAdvanced(options).PostFormData("Permission/SaveRolePermission", formdata);


        }
    });



});



