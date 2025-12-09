

let pageNumber = 0, searchParams = {},
    showMore = false, table = null, dialogElem = null;
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let currentPage = 0;
let isSearch = false;
let isLoading = true;
var filesetting = controlvalidationlist.find(c => c.constraint.controlType == 'DROPZONE').constraint;
let DropzoneFileCount = filesetting.fileCount == null ? 1 : filesetting.fileCount, DropzoneFileSize = filesetting.fileSize == null ? 1025 : filesetting.fileSize, DropzoneFileUploadType = filesetting.fileExtention == null ? "jpg, jpeg, jpe, jif, jfif, jfi,pdf,png" : filesetting.fileExtention, DropzoneFileName = filesetting.controlName;

let hasfile = false;

const btnAddContentId = 'btn-add-content',
    btnSubmitId = "btn-submit",
    $btnSubmit = $('#' + btnSubmitId),
    formSectionId = "form-section",
    $formSection = $('#' + formSectionId),
    thumbnailId = "thumbnail";



const gridContainerId = "view-container",
    tblContentContainerId = "tbl-template-container",
    $tblContentContainer = $('#' + tblContentContainerId),
    $thumbnail = $('#' + thumbnailId),
    $btnAddContent = $('#' + btnAddContentId);



IconPicker.Init({
    jsonUrl: '../lib/iconpicker/dist/iconpicker-1.5.0.json',
    searchPlaceholder: 'Search Icon',
    showAllButton: 'Show All',
    cancelButton: 'Cancel',
    noResultsFound: 'No results found.',
    borderRadius: '20px',
});


Dropzone.autoDiscover = false;
var myDropzone = new Dropzone("#dropzonejs", {
    url: "Department/SaveDepartment",
    paramName: DropzoneFileName,
    maxFiles: 1,
    maxFilesize: DropzoneFileSize,
    acceptedFiles: commonUtil.getMimeTypes(DropzoneFileUploadType),

    addRemoveLinks: true,
    autoProcessQueue: false,
    init: function () {
        var self = this;
        document.getElementById("btn-submit").addEventListener("click", function (e) {
            if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {

                e.preventDefault();
                e.stopPropagation();



                if (self.getQueuedFiles().length > 0) {
                    self.processQueue();
                } else {
                    var blob = new Blob();
                    blob.upload = { 'chunked': self.options.chunking };
                    self.uploadFile(blob);
                }
            }


        });
        self.options.addRemoveLinks = true;
        self.options.dictRemoveFile = "Delete";
        //New file added
        this.on("addedfile", function (file) {
            const fileName = file.name;
            const extension = fileName.split('.').pop().toLowerCase();
            DropzoneFileCount = 1;
            if (this.files.length > DropzoneFileCount) {
                var errormsg = commonUtil.stringFormat(sharedFn().GetUiControlText('MAX_FILE_UPLOAD_COUNT'), file.name)
                notificationUtil.error(errormsg);
                this.removeFile(file, true);
            }
            if (this.files.length) {
                if (file.size <= 0) {
                    var errormsg = commonUtil.stringFormat(sharedFn().GetUiControlText('EMPTY_FILE_ERROR'), file.name)
                    notificationUtil.error(errormsg);
                    this.removeFile(file, true);
                }

                var _i, _len;
                for (_i = 0, _len = this.files.length; _i < _len - 1; _i++) // -1 to exclude current file
                {
                    const sFile = this.files[_i];
                    if (sFile.name === file.name
                        && sFile.size === file.size
                        && sFile.lastModifiedDate.toString() === file.lastModifiedDate.toString()) {
                        var errormsg = commonUtil.stringFormat(sharedFn().GetUiControlText('DUPLICATE_ENTRY_ERROR'), file.name)
                        notificationUtil.error(errormsg);
                        this.removeFile(file);
                    }
                }
            }
        });
        this.on("error", function (xhr, file, response) {
            Showloader(false);
            commonUtil.serverError(file);
            const btnId = `BTN_SAVE_EVENT_${file.type}`;
            commonUtil.btnProgress(btnSubmitId, true);


        });
        this.on("removedfile", function (file) {
            if (!file.upload || !file.upload.uuid) { return; }

        });
        this.on("sending", function (file, response, formData) {
            var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());
            commonUtil.btnProgress(btnSubmitId);
            requestdata.forEach((value, key) => {
                formData.append(key, value);
            });




        });
        this.on('success', function (file, response) {
            commonUtil.btnProgress(btnSubmitId, true);
            let { data } = response;
            if (data.responseStatus == '1') {
                table.addData([data], true);
                notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                commonUtil.btnProgress(btnSubmitId, true);
                sharedFn().ViewMode();
            }
            else if (data.responseStatus == '2') {
                table.updateData([data]);
                notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                commonUtil.btnProgress(btnSubmitId, true);
                sharedFn().ViewMode();
            }
            else if (data.responseStatus == '14') {
                notificationUtil.error(sharedFn().GetUiControlText('SERVICE_FREEZED'));
                return;
            }
            else if (data.responseStatus == '4') {

                if (response) {
                    notificationUtil.error(data.responseMessage);

                }

            }
            else {
                notificationUtil.error(data.responseMessage);
                return false;
            }


        });
        this.on("complete", function (file, response) {

            self.removeFile(file);
        });
    }
});




function ClearControlByPage() {
    $('#dropzonejs').show();
    var attachmentid = $("#DepartmentWebsiteAttachmentId").val();
    if (attachmentid) {
        hasfile = true;

        const options = {
            success: function (data) {
                if (data) {


                    const icon = commonUtil.getFileIcon(data.uiFileName);

                    if (icon.trim() === commonUtil.FILES_TYPE.IMAGE) {
                        var img = `<div class="card">
                                                                <img src="${data.blobUrl}" class="card-img-top" style="max-height:200px">
                                                                <div class="card-body"><p class="card-text">${data.uiFileName}</p></div>
                                                </div>
                                        `;

                        $('#thumbnail').append(img);
                    }
                    else if (icon.trim() === commonUtil.FILES_TYPE.PDF) {
                        var title = $('<div>').addClass('title').text(data.uiFileName);
                        $('#thumbnail').append(icon).append(title);
                    }
                    else if (icon.trim() === commonUtil.FILES_TYPE.VIDEO) {
                        var vedio = `<div class="card">
                                                                <video src="${data.blobUrl}" class="card-img-top"  type = "video/mp4" style="max-height:200px"></video>
                                                                <div class="card-body"><p class="card-text">${data.uiFileName}</p></div>
                                                </div>
                                        `;

                        $('#thumbnail').append(vedio);

                    }
                    else {
                        var title = $('<div>').addClass('title').text(data.uiFileName);
                        $('#thumbnail').append(icon).append(title);

                    };

                    $('#thumbnail').addClass('review');
                    $('#thumbnail').data('url', `${data.blobUrl}`);
                    $('#thumbnail').data('name', `${data.uiFileName}`);

                }
            }
        };

        jqClientAdvanced(options).Get("Department/GetAttachmentById".concat('?Id=', attachmentid));

       
    }
    else {
        $('#thumbnail').empty();
    }
    myDropzone.options.url = "Department/UpdateDepartment";

}

function ClearForViewMode() {

    $('#dropzonejs').hide();
}


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
        if (!isLoading && currentPage > 0) {
            loadData();
        }
    }
});



$(document).ready(function () {
    loadData();
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



    $(`#${btnAddContentId}`).click(function (e) {
        $thumbnail.empty();
        sharedFn().ClearForm();
        sharedFn().EditMode();
        sharedFn().SetDefaultValueFromConfig();
        hasfile = false;
        myDropzone.options.url = "Department/SaveDepartment";
        sharedFn().SetValueToDropdown();

    });

    $("#thumbnail").on("click", function () {

        var url = $('#thumbnail').data('url');
        var name = $('#thumbnail').data('name');
        sharedFn().OpenFileModal('modal-xl', name, url);
    });
});

