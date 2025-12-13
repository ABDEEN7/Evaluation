 

        let  pageNumber = 0, searchParams = {},
            showMore = false,  table = null, dialogElem = null;
        const dailogId = commonUtil.CONTENT_DAILOG_ID;
        let currentPage = 0;
        let isSearch = false;
let isLoading = true;
var filesetting = controlvalidationlist.find(c => c.constraint.controlType == 'DROPZONE').constraint;
let DropzoneFileCount = filesetting.fileCount == null ? 1 : filesetting.fileCount, DropzoneFileSize = filesetting.fileSize == null ? 1025 : filesetting.fileSize, DropzoneFileUploadType = filesetting.fileExtention == null ? "jpg, jpeg, jpe, jif, jfif, jfi,pdf,png" : filesetting.fileExtention, DropzoneFileName = filesetting.controlName;


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


        //   const txtDir = commonUtil.getTextDirection();


        Dropzone.autoDiscover = false;
var myDropzone = new Dropzone("#dropzonejs", {
    url:  "SiteDocument/SaveSiteDocument",
    paramName: DropzoneFileName, 
    maxFiles: DropzoneFileCount,
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
                        } else if ($('#Id').val() !='') {
                            var blob = new Blob();
                            blob.upload = { 'chunked': self.options.chunking };
                            self.uploadFile(blob);
                        }
                        else {
                            notificationUtil.error(sharedFn().GetUiControlText('UPLOAD_FILE_MSG'));

                        }
                    }


                });
                self.options.addRemoveLinks = true;
                self.options.dictRemoveFile = "Delete";
                //New file added
                this.on("addedfile", function (file) {
                    const fileName = file.name;
                    const extension = fileName.split('.').pop().toLowerCase();

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
                     else
                     {
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
    $('#SiteDocumentSkey').parent().parent().show();
    $('#SiteDocumentSkey').attr("readonly", "readonly");
    $('#dropzonejs').hide();
    $("#dropzonejs").prev().prev().hide();
    myDropzone.options.url = "SiteDocument/UpdateSiteDocument";
   
}
      

      

        const loadData = () => {
            isLoading = true;
            const options = {
                success: function (data) {
                    if (data) {


                        if (data && data.length > 0) {
                            if (isSearch) {
                                table.setData([]);
                                currentPage = 1;
                            }

                            table.addData(data);
                            currentPage = currentPage + 1;
                            isLoading = false;
                        }
                        else {
                            currentPage = 0;
                        }
                    }
                }
            };
            jqClientAdvanced(options).Get("SiteDocument/GetAllSiteDocument".concat('?page=', currentPage));
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
                jqClientAdvanced(options).Post("SiteDocument/DeleteSiteDocument".concat('?Id=', id));

            });
        };

      

      

        
       


        $(window).scroll(function () {
            if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
                if (!isLoading && currentPage > 0) {
                    loadData();
                }
            }
        });
function copyTextToClipboard(elementId) {
    // Create a temporary textarea element
    const tempTextArea = document.createElement('textarea');
    tempTextArea.value = WebAppSitePath + $(`#${elementId}`).val();
    document.body.appendChild(tempTextArea);

    // Copy the text from the textarea
    tempTextArea.select();
    document.execCommand('copy');

    // Remove the temporary textarea
    document.body.removeChild(tempTextArea);
    // Optionally, provide some feedback to the user
    notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_COPIED'));

}

        function openNewModel(elementId) {
            var Skey = $(`#${elementId}`).val();
            var url = WebAppSitePath + Skey;
            // Open the URL in a new tab/window
            window.open(url, '_blank');

        }
        $(document).ready(function () {

            table = tableUtil.createTabulator({
                id: gridContainerId,
                config: {
                    textDirection: txtDir,
                    paginationSize: 10,
                    placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
                    headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),
                },
                uniqueRowId: 'id',
                sortColumn: "updateDate",
                sortDir: "desc",
                columns: TableColumns,

            });

            dialogElem = commonUtil.createDailog({ dailogId: dailogId });

            loadData();

           
          
            $(`#${btnAddContentId}`).click(function (e) {
                $thumbnail.empty();
                    sharedFn().ClearForm();
                sharedFn().EditMode();
                sharedFn().SetDefaultValueFromConfig();
                    $('#SiteDocumentSkey').parent().parent().hide();
                $('#dropzonejs').show();
                $("#dropzonejs").prev().prev().show();
                myDropzone.options.url = "SiteDocument/SaveSiteDocument";
               
                });

            $("#thumbnail").on("click", function () {

                var url = $('#thumbnail').data('url');
                var name = $('#thumbnail').data('name');
                sharedFn().OpenFileModal('modal-xl', name, url);
            });
        });

