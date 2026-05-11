
const uiControl = {
    applabel: [],
}

let popupdivcontent = "";
var deprouting = sharedUtility().extractDepartmentName();
const sharedFn = (options) => {

    //===========================================================
    const coverSpin = (show) => {
        if (show) {
            //$('body').hide();
            $('#cover-spin').show();
        } else {
            $('#cover-spin').hide();
            //$('body').show();
        }
    }
    const InitialPageControls = (ControlItems,objdata=null) => {
        let divcontent = '<div class="row">';
        if (ControlItems) {
            var formData = new FormData();
            formData.append('request', JSON.stringify(ControlItems));
            $.ajax({
                url: "/UiControl/UiControlList",
                type: "POST",
                dataType: "html",
                processData: false,
                contentType: false,
                data: formData,
                Mode: 'APP',
                success: function (response) {
                    if (response) {
                        divcontent += response + "</div>";
                        $formContent.empty();
                        $formContent.append(divcontent);
                        $formSection.show();
                        $tblContentContainer.hide();
                        if (typeof controlvalidationlist !== 'undefined') {
                            initializeControl(controlvalidationlist, settingList, true);
                            if (objdata != null) {
                                BindData(controlvalidationlist, objdata);
                            }
                           
                        }
                    }
                },
                error: function (xhr, status, error) {
                    console.error("UI Control load failed:", error);
                }
            });


        }

        return divcontent;
    }

    //===========================================================

    const displayAlert = (msg, icon = null) => {
        if (msg) {

            if (icon && icon == 'success') {
                notificationUtil.message(msg);
            } else {
                if (icon && icon == 'warning') {
                    notificationUtil.warning(msg);
                } else {
                    notificationUtil.error(msg);
                }
            }
            //notificationUtil.popup({ title: 'Server Message', body: msg, icon: icon });
        }
    }

    //===========================================================
    const getSaveObjectJson = (controlvalidationlist, Idvalue) => {

        var data = {};

        if (controlvalidationlist.length > 0) {

            data.Id = Idvalue || null;
            data.IsEdit = !!Idvalue;

            controlvalidationlist.forEach(item => {

                if (item.constraint.controlType == "DATE") {
                    data[item.controlName] =
                        sharedFn().GetActionDate($('#' + item.uibackendName).val(), commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD);
                }
                else if (item.constraint.controlType == "DATETIME") {
                    data[item.controlName] =
                        sharedFn().GetActionDate($('#' + item.uibackendName).val(), commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD_HH_mm_ss);
                }
                else if (["CHECK_BOX", "CHECK_BOX_HIDDEN", "CHECK_BOX_DISABLED", "RADIO_BUTTON"]
                    .includes(item.constraint.controlType)) {

                    data[item.controlName] = $('#' + item.uibackendName).prop("checked");
                }
                else if (item.constraint.controlType == "TEXT_TINY") {
                    data[item.controlName] = tinyMCE.get(item.uibackendName).getContent();
                }
                else if (item.constraint.controlType == "MULTIDROPDOWN") {
                    let val = $('#' + item.uibackendName).val();
                    data[item.controlName] = Array.isArray(val) ? val : [val];
                }
                else if (item.constraint.controlType == "DUAL_LIST") {
                    let idsArray = Array.from(
                        document.querySelectorAll("#" + item.uibackendName + " li")
                    ).map(li => li.getAttribute("data-id"));
                    data[item.controlName] = idsArray;
                }
                else if (item.constraint.controlType == "TAGS") {
                    data[item.controlName] = $('#' + item.uibackendName).val()?.join(',') ?? '';
                }
                else if (item.constraint.controlType == "CHECK_BOX_LIST") {
                    let selected = document.getElementById(item.uibackendName)
                        .querySelectorAll("input[type='checkbox']:checked");

                    data[item.controlName] = Array.from(selected).map(cb => cb.value);
                }
                else {
                    data[item.controlName] = $('#' + item.uibackendName).val();
                }
            });
        }

        return data;
    };

    const getSaveObject = (controlvalidationlist, Idvalue) => {
        var formData = new FormData();
        var data = {};
        if (controlvalidationlist.length > 0) {
            data.Id = (Idvalue != '' ? Idvalue : undefined);
            data.IsEdit = (Idvalue != '' ? true : false);
            controlvalidationlist.forEach(item => {
                if (item.constraint.controlType == "DATE") {
                    data[item.controlName] = sharedFn().GetActionDate($('#' + item.uibackendName).val(), commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD);
                }
                else if (item.constraint.controlType == "DATETIME") {
                    data[item.controlName] = sharedFn().GetActionDate($('#' + item.uibackendName).val(), commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD_HH_mm_ss);
                }
                else if (item.constraint.controlType == "CHECK_BOX" || item.constraint.controlType == "CHECK_BOX_HIDDEN" || item.constraint.controlType == "CHECK_BOX_DISABLED" || item.constraint.controlType == "RADIO_BUTTON") {
                    data[item.controlName] = $('#' + item.uibackendName).prop("checked");
                }
                else if (item.constraint.controlType == "FILEUPLOAD") {
                    const FileName = document.getElementById(item.uibackendName);
                    for (const file of FileName.files) {
                        formData.append(item.uibackendName, file);
                    }
                }
                else if (item.constraint.controlType == "TEXT_TINY") {
                    data[item.controlName] = tinyMCE.get(item.uibackendName).getContent();
                }
                
                else if (item.constraint.controlType == "MULTIDROPDOWN") {
                    if ($('#' + item.uibackendName).is('[multiple]')) {
                        data[item.controlName] = $('#' + item.uibackendName).val();
                    }
                    else {
                        data[item.controlName] = [$('#' + item.uibackendName).val()];
                    }

                }
                else if (item.constraint.controlType == "DUAL_LIST") {
                    let listItems = document.querySelectorAll("#" + item.uibackendName+" li");

                    // Extract 'data-id' values and convert to an array
                    let idsArray = Array.from(listItems).map(li => li.getAttribute("data-id"));
                    data[item.controlName] = idsArray;
                }
                else if (item.constraint.controlType == "TAGS") {
                    data[item.controlName] = $('#' + item.uibackendName).val().join(',');
                }
                else if (item.constraint.controlType == "CHECK_BOX_LIST") {
                    var selectedcheckbox = document.getElementById(item.uibackendName).querySelectorAll("input[type='checkbox']:checked");
                    if (selectedcheckbox.length > 0) {
                        var checkboxlist = Array.from(selectedcheckbox).map(cb => cb.value);
                        data[item.controlName] = checkboxlist;
                    }
                }
                else {
                    data[item.controlName] = $('#' + item.uibackendName).val();
                }

            });

        }
        formData.append('request', JSON.stringify(data));
        return formData;
    }
    //===========================================================
    const initializeControl = (controlvalidationlist, settingList, OnlyDropDown = false) => {
        if (OnlyDropDown == true && controlvalidationlist) {
            //initializing DROPDOWN
            var AlldropdownList = [];
            var dropdownlist = controlvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN' || c.constraint.controlType == 'MULTIDROPDOWN' || c.constraint.controlType == 'DROPDOWN_HIDDEN');
            if (dropdownlist.length > 0) {
                AlldropdownList = dropdownlist;

            }
            if (typeof searchcontrolvalidationlist !== 'undefined') {
                var searchdropdownlist = searchcontrolvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN' || c.constraint.controlType == 'MULTIDROPDOWN');
                if (searchdropdownlist.length > 0) {
                    AlldropdownList = AlldropdownList.concat(searchdropdownlist);
                }
            }
            if (AlldropdownList.length > 0) {
                
                let dropdownInitializer = jqDropdownInitializer({
                    dropdowns: AlldropdownList,
                    url: `/Home/GetDropDownValues`,
                    lang: lang,
                });
                dropdownInitializer.InitParents();
                dropdownInitializer.InitChildren();
                
            }
            //initiazing DualList
            var dualist = controlvalidationlist.filter(c => c.constraint.controlType == 'DUAL_LIST');
            if (dualist.length > 0) {
                resetduallist(dualist);
               
                // Search Function

            }
        }
        
        else {
            if (controlvalidationlist) {

                //initializing FILEUPLOAD
                var fileuploadlist = controlvalidationlist.filter(c => c.constraint.controlType == 'FILEUPLOAD');
                if (fileuploadlist.length > 0) {
                    fileuploadlist.forEach(item => {
                        var constrain = item.constraint;
                        let formattedFileTypes = settingList.find(x => x.settingKey == 'ADMIN_FILE_EXTENSION').settingValue;
                        if (constrain.fileExtention) {
                            formattedFileTypes = constrain.fileExtention.split(',').map(type => `.${type}`).join(',');
                        }


                        $('#' + constrain.uibackendName).attr("accept", formattedFileTypes);
                        $('#' + constrain.uibackendName).on('change', function () {
                            var upl = document.getElementById(constrain.uibackendName);
                            var filesize = settingList.find(x => x.settingKey == 'ADMIN_FILE_SIZE').settingValue;
                            if (constrain.fileSize) {
                                filesize = constrain.fileSize;
                            }

                            var filetypes = settingList.find(x => x.settingKey == 'ADMIN_FILE_EXTENSION').settingValue;
                            if (constrain.fileExtention) {
                                filetypes = constrain.fileExtention;
                            }
                            if ((upl.files[0].size / 1000) > filesize) {


                                var errormsg = commonUtil.stringFormat(sharedFn().GetUiControlText('ADMIN_MSG_FILE_SIZE'), filesize / 1024)
                                notificationUtil.error(upl.files[0].name + errormsg);
                                upl.value = "";
                                return;
                            }
                            var filetypelist = filetypes.split(',').map(type => type.trim());
                            var imagetype = upl.files[0].type.split('/')[1];
                            if (!filetypelist.includes(imagetype)) {

                                var errormsg = commonUtil.stringFormat(sharedFn().GetUiControlText('VALID_UPLOAD_TYPE'), filetypelist)
                                notificationUtil.error(errormsg);
                                upl.value = "";
                                return;
                            }
                            var imagename = '#' + constrain.uibackendName + '_image'
                            $(imagename).attr("src", window.URL.createObjectURL(upl.files[0]));
                        });
                    });

                }
                //initializing DATE
                var datelist = controlvalidationlist.filter(c => c.constraint.controlType == 'DATE');
                if (datelist.length > 0) {
                    datelist.forEach(item => {
                        var constrain = item.constraint;
                        var dateconfig = {
                            autoclose: true, todayHighlight: true, language: "pl", format: "yyyy-mm-dd"
                        };
                        if (constrain.controlJsonConfig) {
                            dateconfig = JSON.parse(constrain.controlJsonConfig);
                        }
                        $('#' + constrain.uibackendName).datepicker(dateconfig);
                        $('#' + constrain.uibackendName).on("change.datetimepicker", () => {

                            NewvalidateInput($('#' + constrain.uibackendName).attr('id'), getUiControlText('ADMIN_CNTRL_REQUIRED'), getUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), getUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'));

                        })
                    });
                }
                //initializing DATETIME
                var datelist = controlvalidationlist.filter(c => c.constraint.controlType == 'DATETIME');
                if (datelist.length > 0) {
                    datelist.forEach(item => {
                        var constrain = item.constraint;
                        var dateconfig = {
                            dayOfWeekStart: 1,
                            lang: 'en',
                            format: 'Y-m-d H:i',
                        };
                        if (constrain.controlJsonConfig) {
                            dateconfig = JSON.parse(constrain.controlJsonConfig);
                        }
                        $('#' + constrain.uibackendName).datetimepicker(dateconfig);
                        $('#' + constrain.uibackendName).on("change.datetimepicker", () => {

                            NewvalidateInput($('#' + constrain.uibackendName).attr('id'), getUiControlText('ADMIN_CNTRL_REQUIRED'), getUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), getUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'));

                        })
                    });
                }
                //initializing COLOR
                var colorlist = controlvalidationlist.filter(c => c.constraint.controlType == 'COLOR');
                if (colorlist.length > 0) {
                    colorlist.forEach(item => {
                        var constrain = item.constraint;
                        var colorpicker = constrain.uibackendName + "picker";
                        document.getElementById(constrain.uibackendName).addEventListener('click', event => {
                            document.getElementById(colorpicker).focus();
                            document.getElementById(colorpicker).click();
                        });
                        document.getElementById(colorpicker).onchange = function () {
                            $('#' + constrain.uibackendName).val(this.value);
                            $('#' + constrain.uibackendName).trigger('keyup');
                        }
                    });
                }



                //initializing TINY
                var tinylist = controlvalidationlist.filter(c => c.constraint.controlType == 'TEXT_TINY');
                if (tinylist.length > 0) {
                    tinylist.forEach(item => {
                        var constrain = item.constraint;
                        NewvalidateInput($('#' + constrain.uibackendName).attr('id'), getUiControlText('ADMIN_CNTRL_REQUIRED'), getUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), getUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'));
                    });
                }

                //initializing TAGS
                var tagslist = controlvalidationlist.filter(c => c.constraint.controlType == 'TAGS');
                if (tagslist.length > 0) {
                    tagslist.forEach(item => {
                        var constrain = item.constraint;
                        $('#' + constrain.uibackendName).select2({
                            tags: true,
                            createTag: function (params) {
                                return {
                                    id: params.term,
                                    text: params.term,
                                    newOption: true
                                };
                            },
                            templateResult: function (data) {
                                var $arresult = $("<span></span>");

                                $arresult.text(data.text);

                                if (data.newOption) {
                                    $arresult.append(" <em>(new)</em>");
                                }

                                return $arresult;
                            }
                        });
                    });
                }

                //initializing DROPZONE
                var dropzonelist = controlvalidationlist.filter(c => c.constraint.controlType == 'DROPZONE');
                if (dropzonelist.length > 0) {
                    dropzonelist.forEach(item => {
                        var constrain = item.constraint;
                        DropzoneFileCount = settingList.find(x => x.settingKey == 'ADMIN_FILE_COUNT').settingValue;
                        DropzoneFileSize = settingList.find(x => x.settingKey == 'ADMIN_FILE_SIZE').settingValue;
                        DropzoneFileUploadType = settingList.find(x => x.settingKey == 'ADMIN_FILE_EXTENSION').settingValue;
                        if (constrain.fileCount != null) {
                            DropzoneFileCount = constrain.fileCount;
                        }
                        if (constrain.fileExtention) {
                            DropzoneFileUploadType = constrain.fileExtention;
                        }
                        if (constrain.fileSize) {
                            DropzoneFileSize = constrain.fileSize;
                        }

                        $(".dropzone-button").text(getUiControlText('ADMIN_MSG_FILE_UPLOAD'));
                    });
                }

                //initializing JSON_AREA
                var jsonarealist = controlvalidationlist.filter(c => c.constraint.controlType == 'JSON_AREA');
                if (jsonarealist.length > 0) {

                    jsonarealist.forEach(item => {
                        var constrain = item.constraint;

                        $('#' + constrain.uibackendName).blur(function () {
                            var schemaText = $(this).val();
                            var valid = ValidateJSON(schemaText);
                            if (!valid) {
                                $(this).focus();
                            }

                        });



                    });
                }

                //initializing DROPDOWN
                var AlldropdownList = [];
                var dropdownlist = controlvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN' || c.constraint.controlType == 'MULTIDROPDOWN' || c.constraint.controlType == 'DROPDOWN_HIDDEN');
                if (dropdownlist.length > 0) {
                    AlldropdownList = dropdownlist;

                }
                if (typeof searchcontrolvalidationlist !== 'undefined') {
                    var searchdropdownlist = searchcontrolvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN' || c.constraint.controlType == 'MULTIDROPDOWN');
                    if (searchdropdownlist.length > 0) {
                        AlldropdownList = AlldropdownList.concat(searchdropdownlist);
                    }
                }
                if (AlldropdownList.length > 0) {
                    let dropdownInitializer = jqDropdownInitializer({
                        dropdowns: AlldropdownList,
                        url: `Home/GetDropDownValues`,
                        lang: lang,
                    });
                    dropdownInitializer.InitParents();
                    dropdownInitializer.InitChildren();
                }

                //initiazing DualList
                var dualist = controlvalidationlist.filter(c => c.constraint.controlType == 'DUAL_LIST');
                if (dualist.length > 0) {
                    resetduallist(dualist);
                    // Search Function

                }
                //initiazing CheckboxList
                var CheckboxList = controlvalidationlist.filter(c => c.constraint.controlType == 'CHECK_BOX_LIST');
                if (CheckboxList.length > 0) {
                    createCheckBoxList(CheckboxList);
                    // Search Function

                }
                //checking regex 
                var regexinputlist = controlvalidationlist.filter(c => c.constraint.regex != null);
                if (regexinputlist.length > 0) {

                    regexinputlist.forEach(item => {
                        var constrain = item.constraint;
                        $('#' + constrain.uibackendName).on('keypress', function (event) {
                            var regex = new RegExp(constrain.regex);
                            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                            if (!regex.test(key)) {
                                event.preventDefault();
                                return false;
                            }
                        });

                        //$('#' + constrain.uibackendName).on('input paste', function (e) {
                        //    var regex = new RegExp(constrain.regex);
                        //    var inputValue = e.target.value;
                        //    var sanitizedValue = inputValue.replace(/[^\w]/gi, '');
                        //    $(this).val(sanitizedValue);
                        //});
                    });
                }

            }
        }
        
    }
    //===========================================================
    const initializePopupControl = (controlvalidationlist, settingList, OnlyDropDown = false) => {
        if (OnlyDropDown == true && controlvalidationlist) {
            //initializing DROPDOWN
            var AlldropdownList = [];
            var dropdownlist = controlvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN' || c.constraint.controlType == 'MULTIDROPDOWN' || c.constraint.controlType == 'DROPDOWN_HIDDEN');
            if (dropdownlist.length > 0) {
                AlldropdownList = dropdownlist;

            }
            if (typeof searchcontrolvalidationlist !== 'undefined') {
                var searchdropdownlist = searchcontrolvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN' || c.constraint.controlType == 'MULTIDROPDOWN');
                if (searchdropdownlist.length > 0) {
                    AlldropdownList = AlldropdownList.concat(searchdropdownlist);
                }
            }
            if (AlldropdownList.length > 0) {
                let dropdownInitializer = jqDropdownInitializer({
                    dropdowns: AlldropdownList,
                    url: `Home/GetDropDownValues`,
                    lang: lang,
                });
                dropdownInitializer.InitParents(true);
                dropdownInitializer.InitChildren(true);
            }
            //initiazing DualList
            var dualist = controlvalidationlist.filter(c => c.constraint.controlType == 'DUAL_LIST');
            if (dualist.length > 0) {
                resetduallist(dualist);

                // Search Function

            }
        }

        else {
            if (controlvalidationlist) {

                //initializing FILEUPLOAD
                var fileuploadlist = controlvalidationlist.filter(c => c.constraint.controlType == 'FILEUPLOAD');
                if (fileuploadlist.length > 0) {
                    fileuploadlist.forEach(item => {
                        var constrain = item.constraint;
                        let formattedFileTypes = settingList.find(x => x.settingKey == 'ADMIN_FILE_EXTENSION').settingValue;
                        if (constrain.fileExtention) {
                            formattedFileTypes = constrain.fileExtention.split(',').map(type => `.${type}`).join(',');
                        }


                        $('#' + constrain.uibackendName).attr("accept", formattedFileTypes);
                        $('#' + constrain.uibackendName).on('change', function () {
                            var upl = document.getElementById(constrain.uibackendName);
                            var filesize = settingList.find(x => x.settingKey == 'ADMIN_FILE_SIZE').settingValue;
                            if (constrain.fileSize) {
                                filesize = constrain.fileSize;
                            }

                            var filetypes = settingList.find(x => x.settingKey == 'ADMIN_FILE_EXTENSION').settingValue;
                            if (constrain.fileExtention) {
                                filetypes = constrain.fileExtention;
                            }
                            if ((upl.files[0].size / 1000) > filesize) {


                                var errormsg = commonUtil.stringFormat(sharedFn().GetUiControlText('ADMIN_MSG_FILE_SIZE'), filesize / 1024)
                                notificationUtil.error(upl.files[0].name + errormsg);
                                upl.value = "";
                                return;
                            }
                            var filetypelist = filetypes.split(',');
                            var imagetype = upl.files[0].type.split('/')[1];
                            if (!filetypelist.includes(imagetype)) {

                                var errormsg = commonUtil.stringFormat(sharedFn().GetUiControlText('VALID_UPLOAD_TYPE'), filetypelist)
                                notificationUtil.error(errormsg);
                                upl.value = "";
                                return;
                            }
                            var imagename = '#' + constrain.uibackendName + '_image'
                            $(imagename).attr("src", window.URL.createObjectURL(upl.files[0]));
                        });
                    });

                }
                //initializing DATE
                var datelist = controlvalidationlist.filter(c => c.constraint.controlType == 'DATE');
                if (datelist.length > 0) {
                    datelist.forEach(item => {
                        var constrain = item.constraint;
                        var dateconfig = {
                            autoclose: true, todayHighlight: true, language: "pl", format: "yyyy-mm-dd"
                        };
                        if (constrain.controlJsonConfig) {
                            dateconfig = JSON.parse(constrain.controlJsonConfig);
                        }
                        $('#' + constrain.uibackendName).datepicker(dateconfig);
                        $('#' + constrain.uibackendName).on("change.datetimepicker", () => {

                            NewvalidateInput($('#' + constrain.uibackendName).attr('id'), getUiControlText('ADMIN_CNTRL_REQUIRED'), getUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), getUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'));

                        })
                    });
                }
                //initializing DATETIME
                var datelist = controlvalidationlist.filter(c => c.constraint.controlType == 'DATETIME');
                if (datelist.length > 0) {
                    datelist.forEach(item => {
                        var constrain = item.constraint;
                        var dateconfig = {
                            dayOfWeekStart: 1,
                            lang: 'en',
                            format: 'Y-m-d H:i',
                        };
                        if (constrain.controlJsonConfig) {
                            dateconfig = JSON.parse(constrain.controlJsonConfig);
                        }
                        $('#' + constrain.uibackendName).datetimepicker(dateconfig);
                        $('#' + constrain.uibackendName).on("change.datetimepicker", () => {

                            NewvalidateInput($('#' + constrain.uibackendName).attr('id'), getUiControlText('ADMIN_CNTRL_REQUIRED'), getUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), getUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'));

                        })
                    });
                }
                //initializing COLOR
                var colorlist = controlvalidationlist.filter(c => c.constraint.controlType == 'COLOR');
                if (colorlist.length > 0) {
                    colorlist.forEach(item => {
                        var constrain = item.constraint;
                        var colorpicker = constrain.uibackendName + "picker";
                        document.getElementById(constrain.uibackendName).addEventListener('click', event => {
                            document.getElementById(colorpicker).focus();
                            document.getElementById(colorpicker).click();
                        });
                        document.getElementById(colorpicker).onchange = function () {
                            $('#' + constrain.uibackendName).val(this.value);
                            $('#' + constrain.uibackendName).trigger('keyup');
                        }
                    });
                }



                //initializing TINY
                var tinylist = controlvalidationlist.filter(c => c.constraint.controlType == 'TEXT_TINY');
                if (tinylist.length > 0) {
                    tinylist.forEach(item => {
                        var constrain = item.constraint;
                        NewvalidateInput($('#' + constrain.uibackendName).attr('id'), getUiControlText('ADMIN_CNTRL_REQUIRED'), getUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), getUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'));
                    });
                }

                //initializing TAGS
                var tagslist = controlvalidationlist.filter(c => c.constraint.controlType == 'TAGS');
                if (tagslist.length > 0) {
                    tagslist.forEach(item => {
                        var constrain = item.constraint;
                        $('#' + constrain.uibackendName).select2({
                            tags: true,
                            createTag: function (params) {
                                return {
                                    id: params.term,
                                    text: params.term,
                                    newOption: true
                                };
                            },
                            templateResult: function (data) {
                                var $arresult = $("<span></span>");

                                $arresult.text(data.text);

                                if (data.newOption) {
                                    $arresult.append(" <em>(new)</em>");
                                }

                                return $arresult;
                            }
                        });
                    });
                }

                //initializing DROPZONE
                var dropzonelist = controlvalidationlist.filter(c => c.constraint.controlType == 'DROPZONE');
                if (dropzonelist.length > 0) {
                    dropzonelist.forEach(item => {
                        var constrain = item.constraint;
                        DropzoneFileCount = settingList.find(x => x.settingKey == 'ADMIN_FILE_COUNT').settingValue;
                        DropzoneFileSize = settingList.find(x => x.settingKey == 'ADMIN_FILE_SIZE').settingValue;
                        DropzoneFileUploadType = settingList.find(x => x.settingKey == 'ADMIN_FILE_EXTENSION').settingValue;
                        if (constrain.fileCount != null) {
                            DropzoneFileCount = constrain.fileCount;
                        }
                        if (constrain.fileExtention) {
                            DropzoneFileUploadType = constrain.fileExtention;
                        }
                        if (constrain.fileSize) {
                            DropzoneFileSize = constrain.fileSize;
                        }

                        $(".dropzone-button").text(getUiControlText('ADMIN_MSG_FILE_UPLOAD'));
                    });
                }

                //initializing JSON_AREA
                var jsonarealist = controlvalidationlist.filter(c => c.constraint.controlType == 'JSON_AREA');
                if (jsonarealist.length > 0) {

                    jsonarealist.forEach(item => {
                        var constrain = item.constraint;

                        $('#' + constrain.uibackendName).blur(function () {
                            var schemaText = $(this).val();
                            var valid = ValidateJSON(schemaText);
                            if (!valid) {
                                $(this).focus();
                            }

                        });



                    });
                }

                //initializing DROPDOWN
                var AlldropdownList = [];
                var dropdownlist = controlvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN' || c.constraint.controlType == 'MULTIDROPDOWN' || c.constraint.controlType == 'DROPDOWN_HIDDEN');
                if (dropdownlist.length > 0) {
                    AlldropdownList = dropdownlist;

                }
                if (typeof searchcontrolvalidationlist !== 'undefined') {
                    var searchdropdownlist = searchcontrolvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN' || c.constraint.controlType == 'MULTIDROPDOWN');
                    if (searchdropdownlist.length > 0) {
                        AlldropdownList = AlldropdownList.concat(searchdropdownlist);
                    }
                }
                if (AlldropdownList.length > 0) {
                    let dropdownInitializer = jqDropdownInitializer({
                        dropdowns: AlldropdownList,
                        url: `Home/GetDropDownValues`,
                        lang: lang,
                    });
                    dropdownInitializer.InitParents(true);
                    dropdownInitializer.InitChildren(true);
                    
                }

                //initiazing DualList
                var dualist = controlvalidationlist.filter(c => c.constraint.controlType == 'DUAL_LIST');
                if (dualist.length > 0) {
                    resetduallist(dualist);
                    // Search Function

                }
                //initiazing CheckboxList
                var CheckboxList = controlvalidationlist.filter(c => c.constraint.controlType == 'CHECK_BOX_LIST');
                if (CheckboxList.length > 0) {
                    createCheckBoxList(CheckboxList);
                    // Search Function

                }
                //initiazing 
                var textboxdisabled = controlvalidationlist.filter(c => c.constraint.controlType == 'TEXT_BOX_DISABLED_COPY' || c.constraint.controlType == 'TEXT_BOX_DISABLED' || c.constraint.controlType == 'NUMBER_DISABLED');
                if (textboxdisabled.length > 0) {
                    textboxdisabled.forEach(item => {
                        var constrain = item.constraint;

                        $('#' + constrain.uibackendName).attr("disabled", "disabled");



                    });

                }
                //checking regex 
                var regexinputlist = controlvalidationlist.filter(c => c.constraint.regex != null);
                if (regexinputlist.length > 0) {

                    regexinputlist.forEach(item => {
                        var constrain = item.constraint;
                        $('#' + constrain.uibackendName).on('keypress', function (event) {
                            var regex = new RegExp(constrain.regex);
                            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                            if (!regex.test(key)) {
                                event.preventDefault();
                                return false;
                            }
                        });

                        //$('#' + constrain.uibackendName).on('input paste', function (e) {
                        //    var regex = new RegExp(constrain.regex);
                        //    var inputValue = e.target.value;
                        //    var sanitizedValue = inputValue.replace(/[^\w]/gi, '');
                        //    $(this).val(sanitizedValue);
                        //});
                    });
                }

            }
        }

    }
    
    //===========================================================

    const ValidateJSON = (schemaText) => {
        if (schemaText) {
            if (typeof schemaText != 'string')
                schemaText = JSON.stringify(schemaText);

            try {
                JSON.parse(schemaText);
                return true;
            } catch (e) {
               
                notificationUtil.error("Invalid JSON Schema");
                return false;
            }
        }
        
    }
    //===========================================================
    const populateUiControl = (controlsList) => {

        if (controlsList) {


            $.each(controlsList, function (index, item) {

                var exist = uiControl.applabel.find(c => c.backEndName == item.backEndName);
                if (!exist) {
                    uiControl.applabel.push(item);
                }
            });


        }
    }
    //===========================================================

    const populateColumn = (columnList, dynamicaction = '', noupdatebydetails = false) => {
        let tabularcolumns = [];
        if (columnList) {

            //Adding Id
            tabularcolumns.push({
                field: "id", title: "Id", visible: false
            })
            //Adding order rowing option
            if (typeof IsEdit !== 'undefined' && typeof containsOrderNo !== 'undefined') {
                if (IsEdit == "True" && containsOrderNo == "True") {
                    tabularcolumns.push({
                        rowHandle: true, formatter: "handle", headerSort: false, frozen: false, width: 30, minWidth: 30
                    })
                }
            }

            //Adding Action column
            tabularcolumns.push({

                title: getUiControlText('ACTIONS'), field: "", cssClass: 'tbl-cell-actions',
                frozen: false, width: 120,
                formatter: function (cell) {
                    const { id } = cell.getRow().getData();
                    return getActionTemplate(id, dynamicaction);
                },
                cellClick: function (event, cell) {
                    actionCellClick(event, cell);
                }

            })
            //Adding Dynamic column
            $.each(columnList, function (index, item) {

                var fieldname = item.controlName.charAt(0).toLowerCase() + item.controlName.slice(1);
                var title = getUiControlText(item.uibackendName);
                var columnfield = {};
                columnfield.title = title;
                columnfield.headerTooltip = title;
                if (item.controlType == "DROPDOWN") {
                    columnfield.field = fieldname.replace(/Id$/, "").replace(/ID$/, "");
                }
                else {
                    columnfield.field = fieldname;
                }

                if (item.tabulatorConfig) {
                    const tabulatorConfigArray = JSON.parse(item.tabulatorConfig);
                    tabulatorConfigArray.forEach(configItem => {
                        for (const key in configItem) {

                            // Editor
                            if (key === "editor") {
                                columnfield.editor = configItem[key] == "true" ? true : configItem[key] == "false" ? false : configItem[key]; // "select", "input", etc.
                            }

                            // Formatter
                            else if (key === "formatter") {
                                if (configItem[key] === "customImageFormatter") {
                                    columnfield.formatter = customImageFormatter;
                                } else if (configItem[key] === "lookup") {
                                    // Lookup formatter for dropdowns
                                    columnfield.formatter = function (cell) {
                                        const valuesKey = columnfield.editorParams?.valuesKey;
                                        return lookupSources[valuesKey]?.[cell.getValue()] || "";
                                    };
                                } else {
                                    columnfield.formatter = configItem[key]; // built-in Tabulator formatter
                                }
                            }

                            // Other properties
                            else {
                                columnfield[key] = configItem[key];
                            }
                        }
                    });
                }

                // Special handling for DROPDOWN columns if editorParams are missing
                if (item.controlType === "DROPDOWN") {
                    if (columnfield.editor !== undefined) {

                        const keyName = columnfield.editorParams?.valuesKey;
                        const list = lookupSources[keyName];

                        if (!Array.isArray(list)) return;

                        const valuesMap = {};
                        list.forEach(x => {
                            valuesMap[x.id] = x.name;
                        });

                        // ---------- SELECT ----------
                        if (columnfield.editor === "select") {
                            columnfield.editorParams = {
                                values: valuesMap,
                                clearable: true
                            };

                            columnfield.formatter = function (cell) {
                                return valuesMap[cell.getValue()] || "";
                            };
                        }

                        // ---------- LIST (MULTISELECT) ----------
                        else if (columnfield.editor === "multiselect") {
                            columnfield.editor = "select";
                            columnfield.editable = true;
                            columnfield.editorParams = {
                                values: valuesMap,
                                multiselect: true,
                                clearable: true
                            };

                            columnfield.formatter = function (cell) {
                                const value = cell.getValue();
                                if (!Array.isArray(value)) return "";
                                return value
                                    .map(v => valuesMap[v])
                                    .filter(Boolean)
                                    .join(", ");
                            };
                            columnfield.cellClick = function (e, cell) {
                                const colDef = cell.getColumn().getDefinition();
                                console.log(colDef.editor, colDef.editable, colDef.editorParams);
                                if (cell.getColumn().getDefinition().editor) {
                                    cell.edit();
                                }
                            };
                            columnfield.mutator = function (value) {
                                if (Array.isArray(value)) return value;
                                if (typeof value === "string") return value.split(",").map(v => Number(v.trim()));
                                return [];
                            };
                        }
                    }
                }




                if (item.controlType == "DATE") {
                    columnfield.tooltip = function (cell) {

                        var dd = getActionDate(cell.getRow().getData()[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD);
                        return dd;
                    }
                    columnfield.formatter = function (cell) {

                        return getActionDate(cell.getRow().getData()[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD);
                    }
                }
                if (item.controlType == "DATETIME") {
                    columnfield.tooltip = function (cell) {

                        var dd = getActionDate(cell.getRow().getData()[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD_HH_mm_ss);
                        return dd;
                    }
                    columnfield.formatter = function (cell) {

                        return getActionDate(cell.getRow().getData()[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD_HH_mm_ss);
                    }
                }

                tabularcolumns.push(columnfield);
            });
            //Adding last updated columns
            if (tabularcolumns.length > 0 && noupdatebydetails == false) {
                var titlecreatedby = getUiControlText('LAST_UPDATED_BY');
                tabularcolumns.push({ title: titlecreatedby, headerTooltip: titlecreatedby, field: "updateBy", hozAlign: "center", headerFilter: "input" });
                var titlecreateddate = getUiControlText('LAST_UPDATED_DATE');
                tabularcolumns.push({
                    title: titlecreateddate, headerTooltip: titlecreateddate, field: "updateDate", width: 130, sorter: "datetime",
                    tooltip: function (cell) {
                        const { updateDate } = cell.getRow().getData();
                        return getActionDate(updateDate, commonUtil.DATE_FORMAT.lll);
                    },
                    formatter: function (cell) {
                        const { updateDate } = cell.getRow().getData();
                        return getActionDate(updateDate, commonUtil.DATE_FORMAT.lll);
                    }
                });
            }

        }
        return tabularcolumns;
    }

    function customImageFormatter(cell, formatterParams, onRendered) {
        // Extract base64-encoded data
        var base64Data = 'data:image/png;base64,' + cell.getValue();

        // Convert base64 to a byte array
        var byteCharacters = atob(base64Data.split(',')[1]);
        var byteNumbers = new Array(byteCharacters.length);

        for (var i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }

        var byteArray = new Uint8Array(byteNumbers);

        // You can now use the 'byteArray' as needed

        // Create an image element and set its source to display the image
        var img = document.createElement("img");
        img.src = base64Data;
        img.style.height = "34px";
        img.style.width = formatterParams.width;

        return img;
    }
    //===========================================================

    const getUiControlText = (backEndName) => {
        let uicontrolItem = uiControl.applabel.find(c => c.backEndName == backEndName);
        if (uicontrolItem) {
            return uicontrolItem.txtValue
        }
        else {
            return `Missing [${backEndName}]`;
        }
    }

    //===========================================================

    const getPageNameText = () => {

        //var result = uiControl.applabel.find(c => c.backEndName == 'lblPageName');
        //if (result && result.txtValue) {
        //    return result.txtValue;
        //}
        var result = $(".sidebaractive").html();
        if (result) {
            return result.trim();
        }
        return '';
    }

    //===========================================================

    const replaceLabelText = (uiControlList, lang, divToBeHiddenWhileReplacingTheContent, showLoader = false) => {

        if (uiControlList && uiControlList.length > 0) {

            $.each(uiControlList, function (index, item) {

                if (item.backEndName) {

                    $('.' + item.backEndName).each(function () {
                        $(this).hide();
                    });
                }
            });

            if (divToBeHiddenWhileReplacingTheContent) {
                $('#' + divToBeHiddenWhileReplacingTheContent).hide();
            }
            if (showLoader) {
                sharedFn().CoverSpin(true);
            }

            $.each(uiControlList, function (index, item) {

                if (item.backEndName && (item.enValue || item.arValue)) {
                    $('.' + item.backEndName).each(function () {
                        if (lang && lang == "ar") {
                            if (item.arValue)
                                $(this).text(item.arValue);
                        }
                        else
                            if (item.enValue)
                                $(this).text(item.enValue);

                    });
                }
            });

            $.each(uiControlList, function (index, labelBackendName) {
                $('.' + labelBackendName).each(function () {
                    $(this).show();
                });
            });

            if (showLoader) {
                sharedFn().CoverSpin(false);
            }
            if (divToBeHiddenWhileReplacingTheContent) {
                $('#' + divToBeHiddenWhileReplacingTheContent).show();
            }
        }

    }
    //===========================================================
    const edit = (id) => {

        if (!id) return;
        if (typeof popupname == 'undefined' || popupname == "") {

            const obj = table.getData().find(f => f.id == id);
            if (!obj) return;
            $('#Id').val(obj.id);
            if (typeof controlvalidationlist !== 'undefined') {
                InitialPageControls(controlvalidationlist, obj)
               
            }

            editMode();
        }
        else {
            $('#PopupId').val(id);
        }
       

    };

    const BindData = (controlvalidationlist, obj) => {
        if (controlvalidationlist.length > 0 && obj != null) {
            var othercontrollist = controlvalidationlist.filter(c => c.constraint.controlType != 'DUAL_LIST');
            var duallistcontrollist = controlvalidationlist.filter(c => c.constraint.controlType == 'DUAL_LIST');
            othercontrollist.forEach(item => {
                var contrains = item.constraint;
                var fieldname = item.controlName.charAt(0).toLowerCase() + item.controlName.slice(1);
                if (contrains.controlType == 'COLOR') {
                    $('#' + contrains.uibackendName).val(obj[fieldname]);
                    var colorpicker = '#' + contrains.uibackendName + 'picker';
                    $(colorpicker).val(obj[fieldname]);
                }
                else if (contrains.controlType == 'CHECK_BOX' || item.constraint.controlType == "CHECK_BOX_HIDDEN" || item.constraint.controlType == "RADIO_BUTTON") {
                    $('#' + contrains.uibackendName).prop("checked", obj[fieldname] ?? false);
                    $('#' + contrains.uibackendName).trigger("change");
                }
                else if (contrains.controlType == 'CHECK_BOX_DISABLED') {
                    $('#' + contrains.uibackendName).prop("checked", obj[fieldname] ?? false);
                    $('#' + contrains.uibackendName).attr("disabled", "disbaled");
                }
                else if (contrains.controlType == 'DROPDOWN') {
                    $('#' + contrains.uibackendName).val(obj[fieldname]).trigger('change');
                    $('#' + contrains.uibackendName).attr("data-value", obj[fieldname]);
                }
                else if (contrains.controlType == 'TAGS') {
                    if (obj[fieldname]) {
                        var tags = obj[fieldname].split(',');
                        tags.forEach(item => {
                            var newTags = new Option(item, item, true, true);
                            $('#' + contrains.uibackendName).append(newTags).trigger('change');
                        });
                    }
                }
                else if (contrains.controlType == 'MULTIDROPDOWN') {
                    if (obj[fieldname]) {
                        if (obj[fieldname].length == 1) {
                            var entity = obj[fieldname][0];
                            $('#' + contrains.uibackendName).val(entity).trigger('change');
                            $('#' + contrains.uibackendName).attr("data-value", entity);

                        }
                        else {
                            var entity = obj[fieldname].map(x => x);
                            $('#' + contrains.uibackendName).val(entity).trigger('change');
                            $('#' + contrains.uibackendName).attr("data-value", entity);

                        }
                    }

                }
                else if (contrains.controlType == 'TEXT_TINY') {
                    obj[fieldname] = obj[fieldname] == null ? "" : obj[fieldname];
                    tinyMCE.get(contrains.uibackendName).setContent(obj[fieldname]);
                }
                else if (contrains.controlType == 'DATE') {
                    var date = sharedFn().GetActionDate(obj[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD);
                    $('#' + contrains.uibackendName).val(date);
                }
                else if (contrains.controlType == 'DATETIME') {
                    var date = sharedFn().GetActionDate(obj[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD_HH_mm_ss);
                    $('#' + contrains.uibackendName).val(date);
                }
                else if (contrains.controlType == 'FILEUPLOAD') {
                    if (obj[fieldname]) {
                        var labelid = '#' + contrains.uibackendName + '_label';
                        var labelfield = fieldname + "_UiFileName";
                        var imageid = '#' + contrains.uibackendName + '_image';
                        var imagefield = fieldname + "_BlobURL";
                        $(labelid).html(obj[labelfield]);
                        $(imageid).attr("src", obj[imagefield]);
                    }
                }
                else if (contrains.controlType == 'FILEUPLOAD_ATTACH') {
                    if (obj[fieldname]) {
                        var labelid = '#' + contrains.uibackendName + '_label';
                        var labelfield = fieldname + "_UiFileName";
                        var imageid = '#' + contrains.uibackendName + '_image';
                        var imagefield = fieldname + "_BlobURL";
                        $(labelid).html(obj[labelfield]);
                        $(imageid).attr("src", obj[imagefield]);
                    }
                }
                else if (contrains.controlType == 'DROPZONE') {
                    if (obj[fieldname]) {
                        $('#thumbnail').empty();
                        var labelfield = fieldname + "_UiFileName";
                        var imagefield = fieldname + "_BlobURL";
                        if (obj[labelfield]) {
                            const icon = commonUtil.getFileIcon(obj[labelfield]);

                            if (icon.trim() === commonUtil.FILES_TYPE.IMAGE) {

                                var img = `<div class="card">
                                                <img src="${obj[imagefield]}" class="card-img-top" style="max-height:200px">
                                                        <div class="card-body"><p class="card-text">${obj[labelfield]}</p></div>
                                                </div>
                                        `;

                                $('#thumbnail').append(img);
                            }
                            else if (icon.trim() === commonUtil.FILES_TYPE.PDF) {
                                var title = $('<div>').addClass('title').text(obj[labelfield]);
                                $('#thumbnail').append(icon).append(title);
                            }
                            else if (icon.trim() === commonUtil.FILES_TYPE.VIDEO) {

                                var vedio = `<div class="card">
                                                        <video src="${obj[imagefield]}" class="card-img-top"  type = "video/mp4" style="max-height:200px"></video>
                                                        <div class="card-body"><p class="card-text">${obj[labelfield]}</p></div>
                                                </div>
                                        `;

                                $('#thumbnail').append(vedio);

                            }
                            else {
                                var title = $('<div>').addClass('title').text(obj?.uiFileName);
                                $('#thumbnail').append(icon).append(title);

                            };
                            $('#thumbnail').addClass('review');
                            $('#thumbnail').data('url', `${obj[imagefield]}`);
                            $('#thumbnail').data('name', `${obj[labelfield]}`);
                        }

                        $('#dropzonejs').hide();
                    }
                }
                else if (contrains.controlType == 'TEXT_BOX_DISABLED' || contrains.controlType == 'NUMBER_DISABLED' || contrains.controlType == 'TEXT_BOX_DISABLED_COPY') {
                    $('#' + contrains.uibackendName).val(obj[fieldname]);
                    $('#' + contrains.uibackendName).attr("disabled", "disbaled");
                }

                else if (contrains.controlType == 'CHECK_BOX_LIST') {
                    var checkboxlist = obj[fieldname];
                    if (checkboxlist) {
                        checkboxlist.forEach(function (value) {
                            $('#' + contrains.uibackendName + ' input[type="checkbox"][value="' + value + '"]').prop('checked', true);
                        });
                    }

                }
                else {
                    $('#' + contrains.uibackendName).val(obj[fieldname]);
                }
            });
            if (duallistcontrollist.length > 0) {
                processDualListControls(duallistcontrollist, obj).then(() => {
                });

            }
        }
        
    }
    //===========================================================
    async function processDualListControls(duallistcontrollist, obj) {
        for (const item of duallistcontrollist) {
            const contrains = item.constraint;
            const uibackendName = contrains.uibackendName;
            const fieldname = item.controlName.charAt(0).toLowerCase() + item.controlName.slice(1);

            if (contrains.controlType === 'DUAL_LIST') {
                const dualilistobj = obj[fieldname];
                
                const data = {
                    controlUibackendName: uibackendName,
                    parentReferenceValue: null
                };
                $("#main_" + uibackendName + " ul").empty();
                $("#" + uibackendName + " ul").empty();
                await new Promise((resolve, reject) => {
                    jqClient({
                        success: function (data) {
                            if (data) {
                                const dualistdata = data;
                               

                                var dualistMap = {};
                                $.each(dualistdata, function (index, item) {
                                    dualistMap[item.id] = item;
                                });

                                // Add ordered items to the first UL (those in dualilistobj)
                                $.each(dualilistobj, function (index, id) {
                                    var item = dualistMap[id];
                                    if (item) {
                                        var name = txtDir === "RTL" ? item.nameAr : item.nameEn;
                                        var listItem = "<li data-index='0' data-id='" + item.id + "'>" + name + "</li>";
                                        $("#" + uibackendName + " ul").append(listItem);
                                    }
                                });
                                $("#main_" + uibackendName + " ul").empty();
                                // Add the rest of the items (not in dualilistobj) to the second UL
                                $.each(dualistdata, function (index, item) {
                                    if (dualilistobj != undefined && !dualilistobj.includes(item.id)) {
                                        var name = txtDir === "RTL" ? item.nameAr : item.nameEn;
                                        var listItem = "<li data-index='0' data-id='" + item.id + "'>" + name + "</li>";
                                        $("#main_" + uibackendName + " ul").append(listItem);
                                    }
                                });
                            }
                            resolve(); // Resolve when success finishes
                        },
                        error: function (err) {
                            reject(err); // Reject on error
                        }
                    }).Post("Home/GetDropDownValues", data);
                });
            }
        }
    }

    //===========================================================
    const resetduallist = (dualist) => {
        $.each(dualist, function (i, dropdown) {
            if (dropdown.constraint.controlJsonConfig) {

                let uibackendName = dropdown.uibackendName;
                let mainuibackendName = "main_" + uibackendName;
                $("#" + mainuibackendName + " ul").empty();
                $("#" + uibackendName + " ul").empty();
                const options = {
                    success: function (data) {
                        if (data) {
                            //adding data to dual list
                            
                            $.each(data, function (index, item) {
                                var title = txtDir === "RTL" ? item.nameAr : item.nameEn;
                                $("#" + mainuibackendName + " ul").append("<li  data-index='0' data-id='" + item.id + "'>" + title + "</li>");
                            });
                            
                            //initialzing list select
                            $("#" + mainuibackendName).off('click', 'li').on('click', 'li', function () {
                                $(this).hasClass('selected') ? $(this).removeClass('selected') : $(this).addClass('selected');
                            });
                            $("#" + uibackendName).off('click', 'li').on('click', 'li', function () {
                                $(this).hasClass('selected') ? $(this).removeClass('selected') : $(this).addClass('selected');
                            });
                           
                            var rightbutton = "#" + uibackendName + "_rightbutton";
                            var rightallbutton = "#" + uibackendName + "_rightallbutton";
                            var leftbutton = "#" + uibackendName + "_leftbutton";
                            var leftallbutton = "#" + uibackendName + "_leftallbutton";
                            //initialzing button click 
                            $(rightbutton).click(function (event) {
                                $("#" + mainuibackendName + "  li.selected").appendTo('#' + uibackendName + ' ul').removeClass('selected');
                                event.preventDefault();
                            });
                            $(rightallbutton).click(function (event) {
                                $('#' + mainuibackendName + ' li').appendTo('#' + uibackendName + ' ul').removeClass('selected');
                                event.preventDefault();
                            });
                            $(leftbutton).click(function (event) {
                                $("#" + uibackendName + " li.selected").appendTo('#' + mainuibackendName + ' ul').removeClass('selected');
                                event.preventDefault();

                            });
                            $(leftallbutton).click(function (event) {
                                $('#' + uibackendName + ' li').appendTo('#' + mainuibackendName + ' ul').removeClass('selected');
                                event.preventDefault();
                            });

                        }
                    }
                };
                ////debugger
                var data = {
                    controlUibackendName: uibackendName,
                    parentReferenceValue: null
                };
                jqClient(options).Post("Home/GetDropDownValues", data);

            }
            else {

                let uibackendName = dropdown.uibackendName;
                let mainuibackendName = "main_" + uibackendName;

                //initialzing list select
                $("#" + mainuibackendName).off('click', 'li').on('click', 'li', function () {
                    $(this).hasClass('selected') ? $(this).removeClass('selected') : $(this).addClass('selected');
                });
                $("#" + uibackendName).off('click', 'li').on('click', 'li', function () {
                    $(this).hasClass('selected') ? $(this).removeClass('selected') : $(this).addClass('selected');
                });
                //initialzing button click 
                var rightbutton = "#" + uibackendName + "_rightbutton";
                var rightallbutton = "#" + uibackendName + "_rightallbutton";
                var leftbutton = "#" + uibackendName + "_leftbutton";
                var leftallbutton = "#" + uibackendName + "_leftallbutton";
                $(rightbutton).click(function (event) {
                    $("#" + mainuibackendName + "  li.selected").appendTo('#' + uibackendName + ' ul').removeClass('selected');
                    event.preventDefault();
                });
                $(leftbutton).click(function (event) {
                    $("#" + uibackendName + " li.selected").appendTo('#' + mainuibackendName + ' ul').removeClass('selected');
                    event.preventDefault();

                });
                $(rightallbutton).click(function (event) {
                    $('#' + mainuibackendName + ' li').appendTo('#' + uibackendName + ' ul').removeClass('selected');
                    event.preventDefault();
                });
                $(leftallbutton).click(function (event) {
                    $('#' + uibackendName + ' li').appendTo('#' + mainuibackendName + ' ul').removeClass('selected');
                    event.preventDefault();
                });
            }
            const sortable = new Sortable(document.getElementById('sortable-list'), {
                animation: 150,
                ghostClass: 'ghost',
                onEnd: function (evt) {
                }
            });
        });
    }
    //===========================================================
    const createCheckBoxList = (CheckBoxList) => {
        $.each(CheckBoxList, function (i, dropdown) {
            if (dropdown.constraint.controlJsonConfig) {

                let uibackendName = dropdown.uibackendName;

                const options = {
                    success: function (data) {
                        if (data) {
                            //adding data to dual list
                            
                            $.each(data, function (index, item) {
                                var title = txtDir === "RTL" ? item.nameAr : item.nameEn;
                                $("#" + uibackendName).append("<label class='switch-small'><input class='form-check-input' name='checkboxlist' type='checkbox' id='" + item.id + "' value='" + item.id + "' /><span class='slider-small round'></span></label><label class='form-check-label'>" + title + "</label>");
                            });
                           

                        }
                    }
                };
                ////debugger
                var data = {
                    controlUibackendName: uibackendName,
                    parentReferenceValue: null
                };
                jqClient(options).Post("Home/GetDropDownValues", data);

            }
           
        });
    }
    //===========================================================

    const disableControl = () => {

        $("#app-form .tinymceeditor").each(function () {
            tinymce.get(this.id).mode.set('readonly');

        });
        $('#app-form').find('input, select, textarea,li').prop('disabled', true);
        $('#btn-submit').hide();

    };
    //===========================================================
    const enableControl = () => {
        $("#app-form .tinymceeditor").each(function () {
            tinymce.get(this.id).mode.set('design');

        });
        $('#app-form').find('input, select, textarea,li').prop('disabled', false);
        $('#btn-submit').show();
    };
    //===========================================================
    const editMode = () => {
        showMore = false;
        if (typeof $tblContentContainer !== 'undefined') {
            $tblContentContainer.hide();
        }
        if (typeof $btnAddbutton !== 'undefined') {
            $btnAddbutton.attr("disabled", "disabled");
        }

        $('.ctrl-required').show();
        if (typeof $tblContentContainer !== 'undefined') {
            $formSection.show();
        }
       

    };
    //===========================================================
    const viewMode = () => {
        setTimeout(() => { showMore = true; }, 1000);
        $formSection.hide();
        var messagesElements = document.getElementsByClassName('messages');
        for (var i = 0; i < messagesElements.length; i++) {
            messagesElements[i].classList.remove("alert"); messagesElements[i].classList.remove("alert-danger");
            messagesElements[i].innerText = "";
        }
        $btnAddbutton.removeAttr("disabled");
        $('#btn-submit').removeAttr("disabled");
        $('.ctrl-required').hide();
        $tblContentContainer.show();
    };
    //===========================================================
    const goBack = () => {
        viewMode();
        popupname = "";
    };
    //===========================================================
    const actionCellClick = (event, cell) => {

        let tableId = cell.getTable().element.id;
        table = Tabulator.findTable("#" + tableId)[0];

        let pkId = '', loaderId = '';
        const cellElem = event.target.closest('section');
        if (cellElem) {
            pkId = cellElem.getAttribute('data-key');
            loaderId = cellElem.getAttribute('id');
        }
        const clazzList = event.target.classList;
        if (clazzList.contains('view')) {
            
            ClearvalidateForm("form-control");
            clearForm();
            edit(pkId);
            disableControl();
            if (window.hasOwnProperty("ClearControlByPage")) {
                ClearControlByPage();
            }
            if (window.hasOwnProperty("ClearForViewMode")) {
                ClearForViewMode();
            }
        }
        else if (clazzList.contains('edit')) {
           
            ClearvalidateForm("form-control");
            if (typeof popupname == 'undefined' || popupname == "") {
                clearForm();
            }
            enableControl();
            edit(pkId);
            if (window.hasOwnProperty("ClearControlByPage")) {
                ClearControlByPage();
            }
            if (window.hasOwnProperty("CreateEditForFormGroup")) {
                CreateEditForFormGroup(pkId);
            }


        }
        else if (clazzList.contains('delete')) {
            ClearvalidateForm("form-control");
            deleteData(pkId);
        }



    };
   
    //===========================================================
    const clearForm = () => {
        $('#Id').val('');
        $("#app-form").trigger("reset");
        $("#app-form select").each(function () {
            $(this).val('').trigger('change');
            $(this).removeAttr("data-value");
        });
        ClearvalidateForm("form-control");
        enableControl();
    };
    //===========================================================
    const getActionDate = (val, format) => {
        format = format ?? commonUtil.DATE_FORMAT.lll;
        return commonUtil.getLocalUtcDateStringEn(val, format);
    };
    //===========================================================
    const getActionTemplate = (pkId, dynamicaction) => {

        var deleteTmpl = '';
        if (typeof IsDelete != 'undefined') {
            deleteTmpl = IsDelete == true ?
                `<span class="delete pointer" title="${getUiControlText('ADMIN_TOOLTIP_DELETE')}"><i class="delete las la-trash"></i></span>`
                : '';
        }
        var editTmpl = '';
        if (typeof IsEdit != 'undefined') {
            editTmpl = IsEdit == true ?
                `<span class="edit pointer" title="${getUiControlText('ADMIN_TOOLTIP_EDIT')}"><i class="edit las la-edit"></i></span>`
                : '';
        }
        var ViewTmpl = '';
        if (typeof IsView != 'undefined') {
            ViewTmpl = IsView == true ?
                `<span class="view pointer" title ="${getUiControlText('ADMIN_TOOLTIP_VIEW')}"><i class="view las la-eye"></i></span>`
                : '';
        }
        return `
    <section class="sec-center" id="action__section__${pkId}" data-key="${pkId}">
        <div class="action-items">

            ${editTmpl}
            ${ViewTmpl}
            ${deleteTmpl} 
            ${dynamicaction}

        </div>
    </section>
    `;
    };
    //===========================================================
    const NewvalidateForm = (className, ADMIN_mesg_REQUIRED, ADMIN_MSG_MAX_CHAR_LENGTH, ADMIN_MSG_MIN_CHAR_LENGTH, ADMIN_MSG_EXP_CHAR = null, exceptionCharacters = null, ADMIN_MSG_Invalid_URL = null) => {

        let formIsValid = true;

        const formElements = document.querySelectorAll(`.${className}:not([hidden])`);
        let elementName = "";
        let firstErrorDiv = null;
        formElements.forEach((element) => {
            if (!$(`#${element.id}`).parent().is(":hidden") || element.type == "textarea") {

                let minLength = parseInt($(`#${element.id}`).attr('minlength'));
                let maxLength = parseInt($(`#${element.id}`).attr('maxlength'));
                let required = $(`#${element.id}`).attr('required');
                let value = ($(`#${element.id}`).val() == null ? '' : $(`#${element.id}`).val());
                let errorMessage = "";
                let errorDiv = "";
                if ($(`#${element.id}`).next().hasClass('tox-tinymce')) {
                    value = tinyMCE.get(element.id).getContent();
                    errorDiv = $(`#${element.id}`).next().next(".messages")[0];
                    elementName = $(`#${element.id}`).prev().parent().children(':first-child').text().replace(/\n/g, '').replace(/\*/g, '');

                }
                else if ($(`#${element.id}`).hasClass('date') || $(`#${element.id}`).hasClass('color') || $(`#${element.id}`).hasClass('icon')) {
                    errorDiv = $(`#${element.id}`).parent().next(".messages")[0];
                    elementName = $(`#${element.id}`).parent().parent().children(':first-child').text().replace(/\n/g, '').replace(/\*/g, '');
                }
                else if ($(`#${element.id}`).hasClass('select2') || $(`#${element.id}`).hasClass('file')) {
                    if ($(`#${element.id}`).hasClass('file')) {
                        var imagefile = element.id + "_image";
                        value = $(`#${imagefile}`).attr('src') == undefined ? '' : $(`#${imagefile}`).attr('src');
                    }
                    errorDiv = $(`#${element.id}`).next().next(".messages")[0];
                    elementName = $(`#${element.id}`).parent().children(':first-child').text().replace(/\n/g, '').replace(/\*/g, '');
                }
                else {
                    errorDiv = $(`#${element.id}`).next(".messages")[0];
                    errorDiv = $(`#${element.id}`).next(".messages")[0];
                    elementName = $(`#${element.id}`).parent().children(':first-child').text().replace(/\n/g, '').replace(/\*/g, '');

                }

                if (errorDiv != undefined && errorDiv != "") {
                    errorDiv.classList.remove("alert");
                    errorDiv.classList.remove("alert-danger");
                    element.classList.remove("error");
                    let label = document.createElement('label');
                    if (required && (value === '' || value === null || value.length == 0)) {
                        var error = commonUtil.stringFormat(ADMIN_mesg_REQUIRED, elementName);

                        errorDiv.innerText = error;
                        errorDiv.classList.add("alert");
                        errorDiv.classList.add("alert-danger");
                        element.classList.add("error");
                        formIsValid = false;
                        if (firstErrorDiv === null) {
                            firstErrorDiv = errorDiv;
                        }
                        return;
                    }

                    if (minLength && value.length > 0 && value.length < minLength) {
                        var error = commonUtil.stringFormat(ADMIN_MSG_MIN_CHAR_LENGTH, elementName, minLength);
                        errorDiv.innerText = error;
                        errorDiv.classList.add("alert");

                        errorDiv.classList.add("alert-danger");
                        element.classList.add("error");
                        if (firstErrorDiv === null) {
                            firstErrorDiv = errorDiv;
                        }
                        formIsValid = false;
                        return;
                    }

                    if (maxLength && value.length > maxLength) {
                        var error = commonUtil.stringFormat(ADMIN_MSG_MAX_CHAR_LENGTH, elementName, maxLength);
                        errorDiv.innerText = error;
                        errorDiv.classList.add("alert");

                        errorDiv.classList.add("alert-danger");
                        element.classList.add("error");
                        formIsValid = false;
                        if (firstErrorDiv === null) {
                            firstErrorDiv = errorDiv;
                        }
                        return;
                    }


                    errorDiv.innerText = '';

                }
            }

        });
        if (firstErrorDiv !== null) {
            $('html, body').animate({
                scrollTop: $(firstErrorDiv).offset().top
            }, 1000); // You can adjust the duration (in milliseconds) as needed
        }
        firstErrorDiv = '';
        return formIsValid;
    };
    //===========================================================
    const NewvalidateInput = (id, ADMIN_mesg_REQUIRED, ADMIN_MSG_MAX_CHAR_LENGTH, ADMIN_MSG_MIN_CHAR_LENGTH, ADMIN_MSG_EXP_CHAR = null, exceptionCharacters = null, ADMIN_MSG_Invalid_URL = null) => {

        let formIsValid = true;
        let minLength = 0;
        let maxLength = "";
        let required = "";
        let value = "";
        let errorMessage = "";
        let errorDiv = "";
        let elementName = "";
        const element = document.getElementById(`${id}`);

        minLength = parseInt($(`#${id}`).attr('minlength'));
        maxLength = parseInt($(`#${id}`).attr('maxlength'));
        required = $(`#${id}`).attr('required');
        value = ($(`#${element.id}`).val() == null ? '' : $(`#${element.id}`).val());
        errorMessage = "";
        errorDiv = "";
        if ($(`#${id}`).next().hasClass('tox-tinymce')) {
            value = tinyMCE.get(element.id).getContent();
            errorDiv = $(`#${id}`).next().next(".messages")[0];
            elementName = $(`#${id}`).prev().parent().children(':first-child').text().replace(/\n/g, '').replace(/\*/g, '');

        }
        else if ($(`#${id}`).hasClass('date') || $(`#${id}`).hasClass('color') || $(`#${id}`).hasClass('icon')) {
            errorDiv = $(`#${id}`).parent().next(".messages")[0];
            elementName = $(`#${id}`).parent().parent().children(':first-child').text().replace(/\n/g, '').replace(/\*/g, '');
        }
        else if ($(`#${id}`).hasClass('select2') || $(`#${id}`).hasClass('file')) {
            if ($(`#${id}`).hasClass('file')) {
                var imagefile = element.id + "_image";
                value = $(`#${imagefile}`).attr('src') == undefined ? '' : $(`#${imagefile}`).attr('src');
            }
            errorDiv = $(`#${id}`).next().next(".messages")[0];
            elementName = $(`#${id}`).parent().children(':first-child').text().replace(/\n/g, '').replace(/\*/g, '');
        }

        else {
            errorDiv = $(`#${id}`).next(".messages")[0];
            elementName = $(`#${id}`).parent().children(':first-child').text().replace(/\n/g, '').replace(/\*/g, '');;

        }

        if (errorDiv != undefined && errorDiv != "") {
            errorDiv.classList.remove("alert");
            errorDiv.classList.remove("alert-danger");
            element.classList.remove("error");
            let label = document.createElement('label');
            if (required && (value === '' || value === null || value.length == 0)) {
                var error = commonUtil.stringFormat(ADMIN_mesg_REQUIRED, elementName);
                errorDiv.innerText = error;
                errorDiv.classList.add("alert");
                errorDiv.classList.add("alert-danger");
                element.classList.add("error");
                formIsValid = false;
                return;
            }

            if (minLength && value.length > 0 && value.length < minLength) {
                var error = commonUtil.stringFormat(ADMIN_MSG_MIN_CHAR_LENGTH, elementName, minLength);
                errorDiv.innerText = error;
                errorDiv.classList.add("alert");

                errorDiv.classList.add("alert-danger");
                element.classList.add("error");
                formIsValid = false;
                return;
            }

            if (maxLength && value.length > maxLength) {
                var error = commonUtil.stringFormat(ADMIN_MSG_MAX_CHAR_LENGTH, elementName, maxLength);
                errorDiv.innerText = error;
                errorDiv.classList.add("alert");

                errorDiv.classList.add("alert-danger");
                element.classList.add("error");
                formIsValid = false;
                return;
            }

            errorDiv.innerText = '';
        }



        return formIsValid;
    };
    //===========================================================
    const ClearvalidateForm = (className) => {

        let formIsValid = true;

        const formElements = document.querySelectorAll(`.${className}:not([hidden])`);
        let elementName = "";
        let firstErrorDiv = null;
        formElements.forEach((element) => {


            let errorDiv = "";

            if ($(`#${element.id}`).next().hasClass('tox-tinymce') ||
                $(`#${element.id}`).hasClass('select2') || $(`#${element.id}`).hasClass('file')) {

                errorDiv = $(`#${element.id}`).next().next(".messages")[0];


            }
            else if ($(`#${element.id}`).hasClass('date') || $(`#${element.id}`).hasClass('color') || $(`#${element.id}`).hasClass('icon')) {
                errorDiv = $(`#${element.id}`).parent().next(".messages")[0];

            }


            else {
                errorDiv = $(`#${element.id}`).next(".messages")[0];
            }
            if (errorDiv != undefined && errorDiv != "") {
                errorDiv.classList.remove("alert");
                errorDiv.classList.remove("alert-danger");
                errorDiv.innerText = '';
                element.classList.remove("error");
            }
            


        });

        return formIsValid;
    };
    //===========================================================
    const OpenFileModal = (size, title, page) => {

        if (isDownload) {
            downloadFilewithname(page, title);
            return;
        }
        var isDownload = false;

        if (!size) {
            size = 'xl';
        }
        const icon = commonUtil.getFileIcon(title);
        if (icon.trim() === commonUtil.FILES_TYPE.IMAGE || icon.trim() === commonUtil.FILES_TYPE.PDF || icon.trim() === commonUtil.FILES_TYPE.VIDEO) {
            $('#FileModalContent').empty();
            var iframe = $('<iframe>');
            iframe.addClass('fileIframe')
            iframe.attr('src', page);
            iframe.css({ 'width': '100%', 'height': 'auto' });
            $('#FileModalContent').append(iframe);
        }
        else
            isDownload = true;

        if (!isDownload) {
            var elm = document.querySelector('#FileModalDialog');
            if (elm.classList.length > 0) {
                var classList = elm.classList.value.split(" ");
                classList.forEach(item => {
                    elm.classList.remove(`${item}`);
                });
            }

            var AddList = `modal-dialog ${size}`.split(" ");
            AddList.forEach(item => {
                elm.classList.add(`${item}`);
            })

            $("#FileModal-Title").text(title);
            //$("#file_frame").attr("src", page);



            $("#FileModal").modal('show');
        }
        else if (isDownload) {
            //const title = title;

            commonUtil.downloadFileUrl(page, title);
            var url = window.location.href;
            url = removeParameterFromUrl(url, '?file');
            window.history.replaceState(null, null, url);
            $("#js-preloader").removeClass("loaded");
            showloader(false);
        }
    };

    //===========================================================
    const InitialPopup = async (modaltitle, ControlItems, groupObject, tablecolumnlist, settingList, onlyTable = 0) => {
        $("#PopupId").val('');
        $('#ModalPopup .modal-body #PopupForm').empty();
        $('#ModalPopup .modal-title').empty();
        $('#ModalPopup .modal-title').html(modaltitle);
        $("#btn-back_popup").html(getUiControlText("BACK_BUTTON"));
        $("#btn-submit_popup").html(getUiControlText("SAVE_BUTTON"));
        $("#btn-clear_popup").html(getUiControlText("CLEAR_BUTTON"));
        let popupdivcontent = '<div class="row">';
        if (ControlItems) {
            var formData = new FormData();
            formData.append('request', JSON.stringify(ControlItems));
            $.ajax({
                url: `/UiControl/${deprouting}/UiControlList`,
                type: "POST",
                dataType: "html",
                processData: false,
                contentType: false,
                data: formData,
                Mode: 'APP',
                success: function (response) {
                    if (response) {
                        if (onlyTable == 0) {
                            popupdivcontent = popupdivcontent + `${response}` + `</div>`;

                        }

                        if (popupdivcontent) {

                            if (tablecolumnlist) {
                                popupdivcontent = popupdivcontent + '<div class="tabulator-wrapper"> <div id="divtable"></div> </div>';
                                $('#ModalPopup .modal-body #PopupForm').html(popupdivcontent);
                                table = tableUtil.createTabulator({
                                    id: "divtable",
                                    config: {
                                        textDirection: txtDir,
                                        pagination: "local",
                                        paginationSize: 10,
                                        placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
                                        headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),
                                    },
                                    isResponsiveLayout: false,
                                    uniqueRowId: 'id',
                                    sortColumn: "updateDate",
                                    sortDir: "desc",
                                    columns: tablecolumnlist,
                                });
                                if (window.hasOwnProperty("Loadtabledata")) {
                                    Loadtabledata();
                                }

                            }
                            else {
                                $('#ModalPopup .modal-body #PopupForm').html(popupdivcontent);
                            }
                            initializePopupControl(ControlItems, settingList);
                            if (window.hasOwnProperty("SetDropDown")) {
                                SetDropDown();
                            }

                            SetValueFromDropdown();
                            SetValueToDropdown();
                            SetPopupData(ControlItems, groupObject);

                            ValidateInput();

                        }
                    }
                },
                error: function (xhr, status, error) {
                    console.error("UI Control load failed:", error);
                }
            });
        }


       
    };
    //===========================================================
    const OpenFormPopup = (modaltitle, ControlItems, groupObject = null, tablecolumnlist = null, settingList = null, hasOnlyTable = 0) => {
        (async () => {
            await InitialPopup(modaltitle, ControlItems, groupObject, tablecolumnlist, settingList, hasOnlyTable);
        })();

        $("#ModalPopup").modal("show");
        $("#PopupForm").trigger("reset");
        SetValueToDropdown();
        
    }
    //===========================================================
    const SetValueFromDropdown = () => {
        var linkedTextbox = $(`input[data-control-id]`).toArray();
        linkedTextbox.forEach(item => {
            var dropdownid = $(item).attr('data-control-id');
            var textid = '#' + $(item).attr('id');
            var dropdownvalue = $('#' + dropdownid).val();
            $(textid).val(dropdownvalue);
        });


    }
    //===========================================================
    const SetValueToDropdown = () => {
        var linkeddropdown = $(`select[data-control-id]`).toArray();
        linkeddropdown.forEach(item => {
            var textid = $(item).attr('data-control-id');
            var dropdownid = '#' + $(item).attr('id');
            var textvalue = $('#' + textid).val();
            $(dropdownid).attr("data-value", textvalue);
            $(dropdownid).val(textvalue).trigger('change');

        });


    }
    //===========================================================
    const SetDefaultValueFromConfig = () => {
        $("#app-form :input").each(function () {
            var defaultvalue = $(this).attr("SetDefaultValue");
            if (this.type === "checkbox" || this.type === "radio") {

                if (defaultvalue === "true" || defaultvalue === "false") {
                    this.checked = defaultvalue === "true";
                } else {
                    this.checked = this.type === "checkbox"? true:false; // or true if that's your fallback
                }

                $(this).trigger('change');
            } else if (this.type === "select-one" || this.type === "select-multiple") {
                if (defaultvalue) {
                    $(this).val(defaultvalue).trigger('change');
                }
                else {
                    $(this).val('').trigger('change');
                }

                $(this).removeAttr("data-value");
            } else {
                if (defaultvalue) {
                    $(this).val(defaultvalue);
                }
                else {
                    $(this).val('');
                }
            }

            var ShowControl = $(this).attr("ShowControl");
            if (ShowControl) {
                if (ShowControl === "true") {
                    $(this).parent().show();
                }
                else if (ShowControl === "false")
                {
                    $(this).parent().hide();
                }
            }

            var DisableControl = $(this).attr("DisableControl");
            if (DisableControl) {
                if (DisableControl === "true") {
                    $(this).attr("disabled", "disabled");
                }
                else if (DisableControl === "false") {
                    $(this).removeAttr("disabled");
                }
            }
        });


    }
    //===========================================================
    const SetPopupData = (ControlItems, groupObject) => {

        const obj = groupObject;
        if (obj) {
            $('#PopupId').val(obj.id);
            if (ControlItems.length > 0) {
                var notduallistcontrollist = ControlItems.filter(c => c.constraint.controlType != 'DUAL_LIST');
                notduallistcontrollist.forEach(item => {
                    var contrains = item.constraint;
                    var fieldname = item.controlName.charAt(0).toLowerCase() + item.controlName.slice(1);

                    if (contrains.controlType == 'COLOR') {
                        $('#' + contrains.uibackendName).val(obj[fieldname]);
                        var colorpicker = '#' + contrains.uibackendName + 'picker';
                        $(colorpicker).val(obj[fieldname]);
                    }
                    else if (contrains.controlType == 'CHECK_BOX' || item.constraint.controlType == "CHECK_BOX_HIDDEN") {
                        $('#' + contrains.uibackendName).prop("checked", obj[fieldname] ?? false);
                    }
                    else if (contrains.controlType == 'DROPDOWN') {
                        $('#' + contrains.uibackendName).attr("data-value", obj[fieldname]);
                        $('#' + contrains.uibackendName).val(obj[fieldname]).trigger('change');
                    }
                    else if (contrains.controlType == 'MULTIDROPDOWN') {
                        $('#' + contrains.uibackendName).attr("data-value", obj[fieldname]);
                        $('#' + contrains.uibackendName).val(obj[fieldname]).trigger('change');
                    }
                    else if (contrains.controlType == 'TEXT_TINY') {
                        tinyMCE.get(contrains.uibackendName).setContent(obj[fieldname]);
                    }
                    else if (contrains.controlType == 'DATE') {
                        var date = sharedFn().GetActionDate(obj[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD);
                        $('#' + contrains.uibackendName).val(date);
                    }
                    else if (contrains.controlType == 'TEXT_BOX_DISABLED' || contrains.controlType == 'NUMBER_DISABLED' || contrains.controlType == 'TEXT_BOX_DISABLED_COPY') {
                        $('#' + contrains.uibackendName).val(obj[fieldname]);
                        $('#' + contrains.uibackendName).attr("disabled", "disbaled");
                    }
                    else if (contrains.controlType == 'DATETIME') {
                        var date = sharedFn().GetActionDate(obj[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD_HH_mm_ss);
                        $('#' + contrains.uibackendName).val(date);
                    }
                    else {
                        $('#' + contrains.uibackendName).val(obj[fieldname]);
                    }

                });
                var duallistcontrollist = ControlItems.filter(c => c.constraint.controlType == 'DUAL_LIST');
                if (duallistcontrollist.length > 0) {
                    sharedFn().ProcessDualListControls(duallistcontrollist, obj).then(() => {
                    });

                }
            }
        }
        
        //if we need any extra settings after loading data in popup
        if (window.hasOwnProperty("SetPopupMode")) {
            SetPopupMode();
        }

    }
    //===========================================================
    const ResetVisibleControls = (formname) => {
        $('#PopupId').val('');
        $("#btn-submit_popup").removeAttr("disabled");
        $("#" + formname + " :input:visible").each(function () {

            if (this.type === "checkbox" || this.type === "radio") {
                this.checked = true;
                $(this).trigger('change');
            } else if (this.type === "select-one" || this.type === "select-multiple") {
                $(this).val('').trigger('change');
            } else {
                $(this).val('');
            }
        });
        $("#PopupForm select.select2:visible").each(function () {

            $(this).val('').trigger('change');
            $(this).removeAttr("data-value");
        });
        $("#PopupForm img").each(function () {

            $(this).attr("src","");
        });
    }
    //===========================================================
    const ValidateInput = () => {
        //===========================================================
        $(".form-control").on("keyup", function () {
            sharedFn().NewvalidateInput($(this).attr('id'), sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'));

        });
        //===========================================================
        $('.select2').on('select2:select', function (e) {
            sharedFn().NewvalidateInput($(this).attr('id'), sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'));

        });
    }
    //===========================================================
    const DisableDropdownOptions = (DropDownId, optionvalue) => {
        EnableDropdownOptions(DropDownId);
        $("#" + DropDownId + " option[value='" + optionvalue+"']").prop("disabled", true);
    }
    //===========================================================
    const EnableDropdownOptions = (DropDownId) => {

        $("#" + DropDownId + " option").prop("disabled", false);
    }

    //===========================================================
    const TreeviewFilterActions = (query, controlname) => {
        const filter = query.toUpperCase();
        const div = document.getElementById(controlname);
        if (!div) return;

        const ul = div.querySelector('ul');
        if (!ul) return;

        const listItems = ul.querySelectorAll('li');

        // Step 1: First hide all items
        listItems.forEach(li => li.style.display = "none");

        // Step 2: Loop again to find and show matches + ancestors + children
        listItems.forEach(li => {
            const spans = Array.from(li.getElementsByTagName("span"));
            const textMatch = spans.some(span =>
                (span.textContent || span.innerText).toUpperCase().includes(filter)
            );

            if (textMatch) {
                // Show this li
                li.style.display = "";

                // Show all ancestors
                let parent = li.parentElement.closest('li');
                while (parent) {
                    parent.style.display = "";
                    parent = parent.parentElement.closest('li');
                }

                // Show all children
                const childLis = li.querySelectorAll('li');
                childLis.forEach(child => child.style.display = "");
            }
        });
    };


    //===========================================================
    const ListFilterActions = (query, controlname) => {
        // Declare variables
        var filter, div, li, i, txtValue;
        filter = query.toUpperCase();
        div = document.getElementById(controlname);
        li = div.getElementsByTagName('li');

        // Loop through all list items, and hide those who don't match the search query
        for (i = 0; i < li.length; i++) {
           
            txtValue = li[i].textContent || li[i].innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                li[i].style.display = "";
            } else {
                li[i].style.display = "none";
            }
        }
    }
    //===========================================================
    const ClearPopup = () => {
        $("#ModalPopup").modal("hide");
        $("#btn-submit_popup").removeAttr("disabled");
        $('#PopupId').val('');
    }
    let result = {};
    result.DisableDropdownOptions = DisableDropdownOptions;
    result.EnableDropdownOptions = EnableDropdownOptions;
    result.CoverSpin = coverSpin;
    result.DisplayAlert = displayAlert;
    result.PopulateUiControl = populateUiControl;
    result.GetUiControlText = getUiControlText;
    result.GetPageNameText = getPageNameText;
    result.ReplaceLabelText = replaceLabelText;
    result.DisableControl = disableControl;
    result.EnableControl = enableControl;
    result.EditMode = editMode;
    result.ViewMode = viewMode;
    result.GoBack = goBack;
    result.ActionCellClick = actionCellClick;
    result.ClearForm = clearForm;
    result.NewvalidateForm = NewvalidateForm;
    result.NewvalidateInput = NewvalidateInput;
    result.ClearvalidateForm = ClearvalidateForm;
    result.OpenFileModal = OpenFileModal;
    result.GetActionDate = getActionDate;
    result.GetActionTemplate = getActionTemplate;
    result.PopulateColumn = populateColumn;
    result.InitializeControl = initializeControl;
    result.InitializePopupControl = initializePopupControl;
    result.Edit = edit;
    result.GetSaveObject = getSaveObject;
    result.GetSaveObjectJson = getSaveObjectJson;
    result.InitialPopup = InitialPopup;
    result.OpenFormPopup = OpenFormPopup;
    result.SetValueFromDropdown = SetValueFromDropdown;
    result.SetValueToDropdown = SetValueToDropdown;
    result.SetPopupData = SetPopupData;
    result.ResetVisibleControls = ResetVisibleControls;
    result.ValidateInput = ValidateInput;
    result.ValidateJSON = ValidateJSON;
    result.TreeviewFilterActions = TreeviewFilterActions;
    result.ListFilterActions = ListFilterActions;
    result.CreateCheckBoxList = createCheckBoxList;
    result.ProcessDualListControls = processDualListControls;
    result.SetDefaultValueFromConfig = SetDefaultValueFromConfig;
    result.InitialPageControls = InitialPageControls;
    result.ClearPopup = ClearPopup;
  
   

    return result;
}
//===========================================================
$(".closeit").on("click", function () {
    sharedFn().EnableControl();
    sharedFn().GoBack();
});
//===========================================================
$("#btn-back_popup").on("click", function () {
    $("#ModalPopup").modal("hide");
    $("#PopupForm").trigger("reset");
    popupname = "";
    $("#btn-add-content").removeAttr("disabled");
    sharedFn().ClearvalidateForm("form-control");
});
//===========================================================
$('#ModalPopup').on('hidden.bs.modal', function () {
    if (window.hasOwnProperty("SetPopupCount")) {
        SetPopupCount();
    }
    sharedFn().ResetVisibleControls("PopupForm");
    if (window.hasOwnProperty("SetDropDown")) {
        SetDropDown();
    }
    popupname = "";
    $("#btn-add-content").removeAttr("disabled");
    sharedFn().ClearvalidateForm("form-control");
});
//===========================================================
$("#btn-clear_popup").on("click", function () {
    sharedFn().ResetVisibleControls("PopupForm");
    sharedFn().ResetVisibleControls("ModalPopup");
    if (window.hasOwnProperty("SetDropDown")) {
        SetDropDown();
    }
    sharedFn().ClearvalidateForm("form-control");
});
//===========================================================
$("#btn-close_popup").on("click", function () {
    $("#ModalPopup").modal("hide");
    $("#PopupForm").trigger("reset");
    $("#PopupId").val('');
    popupname = "";
    $("#btn-add-content").removeAttr("disabled");
    sharedFn().ClearvalidateForm("form-control");
});

//===========================================================
$("#btn-back").on("click", function () {
    sharedFn().EnableControl();
    sharedFn().GoBack();
});
//===========================================================
$(".form-control").on("keyup", function () {
    sharedFn().NewvalidateInput($(this).attr('id'), sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'));

});
//===========================================================
$('.select2').on('select2:select', function (e) {
    sharedFn().NewvalidateInput($(this).attr('id'), sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'));

});
//===========================================================
document.addEventListener("DOMContentLoaded", function () {
    document.addEventListener("keydown", function (event) {
        if (event.key === "Enter") {
            const button = document.getElementById("btn-seach-start");
            if (button) {
                button.click();
            }
        }
    });
});

function copyTextfromtextbox(elementId) {
    const text = $(`#${elementId}`).val();
    navigator.clipboard.writeText(text).then(() => {
        notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_COPIED'));
    }).catch(err => {
        console.error('Failed to copy: ', err);
    });
}


const RemoveElement = (elementId, isClass = false) => {
    if (elementId) {
        if (isClass) {

            $('.' + elementId).remove();
        } else {
            $('#' + elementId).remove();

        }

    }
}
