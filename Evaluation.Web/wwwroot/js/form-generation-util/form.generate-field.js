
window.formUtility = window.formUtility || {};
var formGenerateFieldUtility = window.formUtility;

(function (ns) {

    // ================== CONSTANTS / SHARED STATE ==================

    const { RENDER_TYPE, ACTION_TYPE, ReadOnly_ACTION_TYPES, uniqueIndexId } = window.FormConstants || {};

    ns.lang = ns.lang || window.currentLang || "en";
    ns.attachments = ns.attachments || [];
    ns.tempFileStorage = ns.tempFileStorage || {};
    ns.conditionalFields = ns.conditionalFields || [];
    ns.cssClasses = ns.cssClasses || [];
    ns.canViewFieldHistory = ns.canViewFieldHistory || false;
    ns.canViewAllFieldHistory = ns.canViewAllFieldHistory || false;

    // ====== external helpers (defined in other modules / global) ======
    const OpenFileModal = ns.OpenFileModal || window.OpenFileModal;
    const getFile = ns.getFile || window.getFile;

    // ================== COMMON HELPERS ==================

    const setReadOnlyAttribute = (inputElement, field) => {
        inputElement.attr('id', 'field_' + field.fieldId);
        inputElement.prop("readonly", true);
        inputElement.prop("disabled", true);
        if (field.type !== "file" && field.type !== "fileV2") {
            inputElement.val(field.value);
        }
    };

    ns.coverSpin = function (show) {
        if (show) {
            $('#cover-spin').show();
        } else {
            $('#cover-spin').hide();
        }
    };

    // ================== FIELD GENERATORS ==================

    const generateDefaultField = (field, readonly) => {
        const element = $('<input>')
            .attr('type', 'text')
            .addClass('form-control')
            .attr('placeholder', ` ${field.fieldName}`);

        if (readonly) setReadOnlyAttribute(element, field);

        element.on('keydown', (e) => {
            if (e.keyCode === 13) {
                e.preventDefault();
                return;
            }
        });
        return element;
    };

    const generateTextareaField = (field, readonly) => {
        const inputElement = $('<textarea>')
            .addClass('form-control')
            .attr('placeholder', field.fieldName)
            .attr('maxlength', field.maxlength || 2000)
            .text(field.value);

        if (readonly) setReadOnlyAttribute(inputElement, field);

        inputElement.on('keydown', (e) => {
            if (e.keyCode === 13) {
                e.preventDefault();
            }
        });

        return inputElement;
    };

    const generateNumberField = (field, readonly) => {
        const inputElement = $('<input>')
            .attr('type', field.type)
            .addClass('form-control number')
            .attr('placeholder', field.fieldName)
            .attr('aria-label', field.fieldName);

        inputElement.on('keydown', (e) => {
            if (e.keyCode === 13) {
                e.preventDefault();
                return;
            }
            if (
                $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                (e.keyCode >= 35 && e.keyCode <= 40)
            ) {
                return;
            }
            if (
                (e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) &&
                (e.keyCode < 96 || e.keyCode > 105)
            ) {
                e.preventDefault();
            }
        });

        if (readonly) setReadOnlyAttribute(inputElement, field);

        return inputElement;
    };

    const generatePhoneField = (field, readonly) => {
        const inputElement = $('<input>')
            .attr('type', 'tel')
            .addClass('form-control phone')
            .attr('placeholder', field.fieldName)
            .attr('aria-label', field.fieldName);

        inputElement.on('keydown', (e) => {
            if (e.keyCode === 13) {
                e.preventDefault();
                return;
            }
            if (
                $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190, 189, 187]) !== -1 ||
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                (e.keyCode >= 35 && e.keyCode <= 40)
            ) {
                return;
            }
            if (
                (e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) &&
                (e.keyCode < 96 || e.keyCode > 105)
            ) {
                e.preventDefault();
            }
        });

        if (readonly) setReadOnlyAttribute(inputElement, field);

        return inputElement;
    };

    const generateEvlFormField = (field, readonly, renderType) => {

        let prefield = renderType === RENDER_TYPE.PREVIEW ? "field_view_" : "field_";
        const fieldId = `${prefield}${field.fieldId}`;

        const container = $('<div>')
            .addClass('evl-form-wrapper')
            .attr('data-field-id', field.fieldId)
            .attr('id', fieldId);

        container.html(`<div class="text-muted py-2">Loading evaluation form...</div>`);

        (async () => {
            try {
                const formId =field.formId ;

                //if (!formId) {
                //    container.html(`<div class="text-danger">Missing formId for evl_Form.</div>`);
                //    return;
                //}
                const allowAddFormItem = field.attributes?.find(c => c.name === 'allowAddFormItem');
                const allowDeleteFormItem = field.attributes?.find(c => c.name === 'allowDeleteFormItem');
                const allowRenameFormItem = field.attributes?.find(c => c.name === 'allowRenameFormItem');
                const evaluateRenamedItems = field.attributes?.find(c => c.name === 'evaluateRenamedItems');
                //const allowRename = field.attributes?.find(c => c.name === 'allowRename');
                const params = new URLSearchParams(window.location.search);
                var evaluationRequestId = (params.get("Evlid")).replace("#", "");
                var serviceRequestId = null;
                if (params.get("id"))
                 serviceRequestId = (params.get("id") ).replace("#", "");
                const controlValues = JSON.parse(field.value);
 
                let allowRename = allowRenameFormItem != undefined && (!allowRenameFormItem || allowRenameFormItem.value == true || allowRenameFormItem.value == "true");
                let allowDelete = allowDeleteFormItem != undefined && (!allowDeleteFormItem || allowDeleteFormItem.value == true || allowDeleteFormItem.value == "true");
                let allowAdd = allowAddFormItem != undefined && (!allowAddFormItem || allowAddFormItem.value == true || allowAddFormItem.value == "true");
                let allowEvaluateRenamedItems = evaluateRenamedItems != undefined && (!evaluateRenamedItems || evaluateRenamedItems.value == true || evaluateRenamedItems.value == "true");


                const html = await initForm(formId, fieldId, readonly, controlValues, evaluationRequestId, serviceRequestId, allowRename, allowDelete, allowAdd, allowEvaluateRenamedItems);

                container.html(html);

                console.log('🔴 container rendered');

                bindFormEvents(fieldId);

                //if (allowRenameFormItem) {
                //    await fillRenameControls(fieldId, controlValues);
                //} else {
                //    await initializeControls(formId, fieldId, controlValues);
                //}


            } catch (err) {
                console.error('evl_Form render failed:', err);
                container.html(`<div class="text-danger">Failed to load evaluation form.</div>`);
            }
        })();

        return container;
    };

    const isBase64Value = (val) => {
        if (!val) return false;
        return typeof val === "string" && val.length > 100 && /^[A-Za-z0-9+/=]+$/.test(val);
    };

    const generateFileField = (field, readonly) => {
        const container = $('<div>').addClass('file-field-container');
        const acceptedExtension = field.attributes?.find(c => c.name === 'accept')?.value;

        const inputElement = $('<input>').attr({
            type: 'file',
            accept: acceptedExtension || '.pdf, .doc, .docx, .png, .jpg, .jpeg',
        }).addClass('form-control-file form-control');

        const previewContainer = $('<div>').addClass('file-preview-container');

        if (readonly) {
            setReadOnlyAttribute(inputElement, field);
        }

        if (!readonly) {
            container.append(inputElement);
        }

        if (field.value) {
            let attachment = ns.attachments.find(at => at.id === field.value);
            if (!attachment && ns.tempFileStorage[field.value]) {
                const tempfield = ns.tempFileStorage[field.value];
                attachment = {
                    uiName: tempfield.fieldName,
                    id: tempfield.file,
                    type: tempfield.type,
                    size: tempfield.size,
                    file: tempfield
                };
            }

            if (attachment) {
                const fileName = attachment.uiFileName || attachment.uiName || '';
                const fileType = fileName.toLowerCase();
                const extension = fileType.split('.').pop();

                const imageExtensions = ['jpg', 'jpeg', 'png'];
                const pdfExtensions = ['pdf'];
                const wordExtensions = ['doc', 'docx'];
                const excelExtensions = ['xls', 'xlsx'];

                const isImage = imageExtensions.includes(extension);
                const isPdf = pdfExtensions.includes(extension);
                const isWord = wordExtensions.includes(extension);
                const isExcel = excelExtensions.includes(extension);

                const card = $('<div>').addClass('d-inline-flex gap-2 border p-2 rounded fs-16 align-items-center uploaded-flie');

                const createIcon = (iconClass) => {
                    return $('<i>').addClass(`fa ${iconClass} d-inline fs-24 m-0 text-primary`).css({
                        fontSize: '50px',
                        cursor: 'pointer',
                        margin: 'auto',
                        display: 'block',
                    });
                };

                let previewElement;

                if (isImage) {
                    previewElement = createIcon('fa-solid fa-image');
                } else if (isPdf) {
                    previewElement = createIcon('fa-file-pdf');
                } else if (isWord) {
                    previewElement = createIcon('fa-file-word');
                } else if (isExcel) {
                    previewElement = createIcon('fa-file-excel');
                } else {
                    previewElement = createIcon('fa-file');
                }

                card.on('click', async (e) => {
                    e.preventDefault();
                    if (!getFile || !OpenFileModal) return;

                    const url = await getFile(attachment.id);
                    if (url) {
                        OpenFileModal('modal-fullscreen', fileName, url, fileName);
                    } else {
                        console.error('Failed to load file.');
                    }
                });

                card.append(previewElement);

                const cardBody = $('<div>').addClass('').text(fileName);
                card.append(cardBody);
                previewContainer.append(card);
            }
        }

        inputElement.on('change', function () {
            previewContainer.empty();
            const file = this.files[0];
            if (file) {
                const fileName = file.name.toLowerCase();
                const extension = fileName.split('.').pop();
                const invalidExtensions = ['txt', 'exe'];

                if (invalidExtensions.includes(extension)) {
                    alert(`File type .${extension} is not allowed.`);
                    $(this).val('');
                    return;
                }
                const card = $('<div>').addClass('d-inline-flex gap-2 border p-2 rounded fs-16 align-items-center uploaded-flie mt-3');
                if (file.type.startsWith('image/')) {
                    const reader = new FileReader();
                    reader.onload = function (e) {
                        const img = $('<img>')
                            .attr('src', e.target.result)
                            .addClass('img-responsive')
                            .css({ maxWidth: '150px', maxHeight: '150px' });
                        card.append(img);
                    };
                    reader.readAsDataURL(file);
                } else {
                    const icon = $('<i>').addClass('fa fa-file d-inline fs-24 m-0 text-primary');
                    card.append(icon);
                }

                const cardBody = $('<div>').text(file.name);
                card.append(cardBody);
                previewContainer.append(card);
            }
        });

        container.append(previewContainer);
        return container;
    };

    const generateFileFieldList = (fields, readonly) => {
        const container = $('<div>').addClass('file-fields-container d-flex flex-wrap gap-3');

        fields.forEach(field => {
            const acceptedExtension = field.attributes?.find(c => c.name === 'accept')?.value;

            const inputElement = $('<input>').attr({
                type: 'file',
                accept: acceptedExtension || '.pdf, .doc, .docx, .png, .jpg, .jpeg',
            }).addClass('form-control-file form-control');

            inputElement.on('change', function () {
                const invalidExtensions = ['txt', 'exe'];
                const files = this.files;

                for (let i = 0; i < files.length; i++) {
                    const fileName = files[i].name.toLowerCase();
                    const extension = fileName.split('.').pop();

                    if (invalidExtensions.includes(extension)) {
                        alert(`File type .${extension} is not allowed.`);
                        $(this).val('');
                        return;
                    }
                }
            });

            const previewContainer = $('<div>').addClass('file-preview-container mt-2');

            if (readonly) {
                setReadOnlyAttribute(inputElement, field);
            } else {
                container.append(inputElement);
            }

            if (field.value) {
                let attachment = ns.attachments.find(at => at.id === field.value);
                if (!attachment && ns.tempFileStorage[field.value]) {
                    const tempfield = ns.tempFileStorage[field.value];
                    attachment = {
                        uiName: tempfield.fieldName,
                        id: tempfield.file,
                        type: tempfield.type,
                        size: tempfield.size,
                        file: tempfield
                    };
                }

                if (attachment) {
                    const fileName = attachment.uiFileName || attachment.uiName || '';
                    const fileType = fileName.toLowerCase();
                    const fieldname = field.fieldName;
                    const extension = fileType.split('.').pop();

                    const imageExtensions = ['jpg', 'jpeg', 'png'];
                    const pdfExtensions = ['pdf'];
                    const wordExtensions = ['doc', 'docx'];
                    const excelExtensions = ['xls', 'xlsx'];

                    let iconClass = 'fa-file';
                    if (imageExtensions.includes(extension)) iconClass = 'fa-solid fa-file-image text-warning';
                    else if (pdfExtensions.includes(extension)) iconClass = 'fa-solid fa-file-pdf text-danger';
                    else if (wordExtensions.includes(extension)) iconClass = 'fa-solid fa-file-word text-primary';
                    else if (excelExtensions.includes(extension)) iconClass = 'fa-solid fa-file-excel text-success';

                    const card = $('<div>')
                        .addClass('d-flex flex-column align-items-center border p-2 rounded uploaded-file')
                        .css({
                            width: '120px',
                            height: '120px',
                            textAlign: 'center',
                            margin: '5px'
                        });

                    const icon = $('<i>')
                        .addClass(`fa ${iconClass} fs-32 text-primary mb-2`)
                        .css({ fontSize: '40px' });

                    card.on('click', async (e) => {
                        e.preventDefault();
                        if (!getFile || !OpenFileModal) return;

                        const url = await getFile(attachment.id);
                        if (url) {
                            OpenFileModal('modal-fullscreen', fileName, url, fileName);
                        } else {
                            console.error('Failed to load file.');
                        }
                    });

                    card.append(
                        $('<div>')
                            .css({
                                width: '100%',
                                whiteSpace: 'nowrap',
                                overflow: 'hidden',
                                textOverflow: 'ellipsis',
                                fontSize: '12px'
                            })
                            .attr('title', fieldname)
                            .text(fieldname)
                    );
                    card.append(icon);
                    card.append(
                        $('<div>')
                            .addClass('file-name')
                            .css({
                                width: '100%',
                                whiteSpace: 'nowrap',
                                overflow: 'hidden',
                                textOverflow: 'ellipsis',
                                fontSize: '12px'
                            })
                            .attr('title', fileName)
                            .text(fileName)
                    );

                    previewContainer.append(card);
                }
            }

            container.append(previewContainer);
        });

        return container;
    };

    const generateFileFieldWithButtons = (field, readonly) => {
        const container = $('<div>')
            .addClass('file-field-container')
            .attr('data-attachment-id',
                typeof field.value === 'string'
                    ? field.value
                    : field.value?.file || null
            );

        const acceptedExtention = field.attributes?.find(c => c.name === 'accept')?.value;

        const inputElement = $('<input>').attr({
            type: 'file',
            accept: acceptedExtention || '.pdf, .doc, .docx'
        }).addClass('form-control-file form-control');

        if (readonly) setReadOnlyAttribute(inputElement, field);

        const uploadButton = $('<button>')
            .addClass('btn btn-primary mx-2')
            .text('Upload New')
            .hide();

        const clearIcon = $('<i>')
            .addClass('fa-solid fa-times-circle clear-icon mx-2')
            .css('cursor', 'pointer')
            .attr('title', 'Clear File')
            .hide()
            .on('click', function () {
                if (field.value && field.value.file && ns.tempFileStorage[field.value.file]) {
                    ns.tempFileStorage[field.value.file] = null;
                }
                container.empty();
                container.attr('data-attachment-id', null);
                container.append(inputElement).append(uploadButton);
                inputElement.show();
                uploadButton.hide();
                clearIcon.hide();
            });

        if (isBase64Value(field.value)) {
            const link = $('<a>')
                .attr('href', 'javascript:void(0);')
                .text(field.fieldName)
                .on('click', function () {
                    try {
                        const byteCharacters = atob(field.value);
                        const byteNumbers = new Array(byteCharacters.length);
                        for (let i = 0; i < byteCharacters.length; i++) {
                            byteNumbers[i] = byteCharacters.charCodeAt(i);
                        }
                        const byteArray = new Uint8Array(byteNumbers);
                        const blob = new Blob([byteArray], { type: 'application/pdf' });
                        const fileUrl = URL.createObjectURL(blob);
                        if (OpenFileModal) {
                            OpenFileModal('modal-fullscreen', 'Base64.pdf', fileUrl, field.FieldName);
                        }
                    } catch (e) {
                        console.error('Invalid Base64 content', e);
                    }
                });

            link.prepend($('<i>').addClass('fa-solid fa-file-pdf mx-1'));
            container.append(link);

            if (!readonly) {
                container.append(clearIcon.show(), uploadButton.show());
                inputElement.hide();
            }
        } else if (field.value) {
            let attachment = ns.attachments.find(at => at.id === field.value);
            if (!attachment && field.value && ns.tempFileStorage[field.value.file]) {
                const tempfield = ns.tempFileStorage[field.value.file];
                attachment = {
                    uiName: tempfield.name,
                    id: tempfield.file,
                    type: tempfield.type,
                    size: tempfield.size,
                    file: tempfield
                };
            }
            if (attachment) {
                const name = attachment.uiFileName || attachment.uiName || attachment.fileName || 'Unnamed file';
                const lowerName = name.toLowerCase();

                let fileType = 'default';
                if (/\.(jpg|jpeg|png)$/i.test(lowerName)) fileType = 'image';
                else if (/\.(pdf)$/i.test(lowerName)) fileType = 'pdf';
                else if (/\.(doc|docx)$/i.test(lowerName)) fileType = 'word';
                else if (/\.(xls|xlsx)$/i.test(lowerName)) fileType = 'excel';

                const fileLink = $('<a>')
                    .attr('href', 'javascript:void(0);')
                    .text(name)
                    .on('click', async function (e) {
                        e.preventDefault();

                        if (field.value && field.value.file && ns.tempFileStorage[field.value.file]) {
                            const tempFile = ns.tempFileStorage[field.value.file];
                            const reader = new FileReader();
                            reader.onload = function (e) {
                                const fileUrl = e.target.result;
                                if (OpenFileModal) {
                                    OpenFileModal('modal-fullscreen', name, fileUrl, name);
                                }
                            };
                            reader.readAsDataURL(tempFile.file);
                        } else if (getFile && OpenFileModal) {
                            const url = await getFile(field.value);
                            if (url) {
                                OpenFileModal('modal-fullscreen', name, url, name);
                            } else {
                                console.error('Failed to load file.');
                            }
                        }
                    });

                const iconClass = {
                    pdf: 'fa-file-pdf',
                    image: 'fa-file-image',
                    word: 'fa-file-word',
                    excel: 'fa-file-excel',
                    default: 'fa-file'
                }[fileType];

                fileLink.prepend($('<i>').addClass(`fa-solid ${iconClass} mx-1`));
                container.append(fileLink);

                if (!readonly) {
                    container.append(clearIcon.show(), uploadButton.show());
                    inputElement.hide();
                }
            } else if (!readonly) {
                container.append(inputElement);
            }
        } else {
            container.append(inputElement);
            uploadButton.hide();
            clearIcon.hide();
        }

        uploadButton.on('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            uploadButton.hide();
            clearIcon.hide();
            inputElement.appendTo(container).show();
        });

        return container;
    };

    const generateDropzoneField = (field, readonly) => {
        const inputElement = $('<div>').addClass('dropzone');
        field.url = 'https://dummy-upload-url.com';
        if (readonly) setReadOnlyAttribute(inputElement, field);
        return inputElement;
    };

    const generateLabelField = (field) => {
        return $('<label>')
            .html(field.fieldName)
            .addClass('form-label');
    };

    const generateCheckboxField = (field, isReadonly) => {
        return $('<input>')
            .attr('type', 'checkbox')
            .attr('class', 'chk')
            .attr('id', `field_${field.fieldId}`)
            .attr('name', field.fieldId)
            .prop('checked', !!field.value)
            .prop('disabled', isReadonly);
    };

    const generateJqteField = (field, readonly) => {
        const jqteElement = $('<textarea>')
            .addClass('jqte-textarea')
            .attr('placeholder', field.fieldName);

        if (readonly) setReadOnlyAttribute(jqteElement, field);

        return jqteElement;
    };

    const generateDualSelectField = () => {
        return $('<div>').addClass('dualSelect');
    };

    const generateTinyMCEField = (field, readonly = false) => {
        const container = $('<div>').addClass('tinymce-container');

        const editorId = `tinymce-${field.fieldId || Math.random().toString(36).substring(2, 15)}`;

        const textarea = $('<textarea>')
            .attr('id', editorId)
            .attr('name', field.fieldName || 'editor')
            .text(field.value || '')
            .addClass('tinymce-editor');

        container.append(textarea);

        if (window.tinymce) {
            tinymce.init({
                selector: `#${editorId}`,
                menubar: false,
                toolbar: readonly
                    ? false
                    : 'bold italic underline | alignleft aligncenter alignright | bullist numlist | link',
                plugins: readonly ? '' : 'lists link',
                readonly: readonly,
                branding: false,
                height: 200
            });
        }

        return container;
    };

    const generateSelect2Field = (field, readonly) => {
        const inputElement = $('<select>')
            .addClass('form-select select2')
            .attr('placeholder', field.fieldName)
            .attr('aria-label', field.fieldName);

        const lang = ns.lang;
        let filteredDropdowns = (window.dropdowns || []).filter(dropdown => dropdown.dropDownTypeId === field.dropDownTypeId);

        const orderByDesc = field.attributes?.some(c => c.name === 'orderBydesc');
        const orderByAsc = field.attributes?.some(c => c.name === 'orderByAsc');

        let otherOption = null;
        filteredDropdowns = filteredDropdowns.filter(option => {
            const title = (lang === 'ar' ? option.titleAr : option.titleEn)?.toLowerCase();
            if (title === 'other' || title === 'أخرى') {
                otherOption = option;
                return false;
            }
            return true;
        });

        if (orderByDesc) {
            filteredDropdowns.sort((a, b) => {
                const titleA = lang === 'ar' ? a.titleAr : a.titleEn;
                const titleB = lang === 'ar' ? b.titleAr : b.titleEn;
                return titleB.localeCompare(titleA);
            });
        } else if (orderByAsc) {
            filteredDropdowns.sort((a, b) => {
                const titleA = lang === 'ar' ? a.titleAr : a.titleEn;
                const titleB = lang === 'ar' ? b.titleAr : b.titleEn;
                return titleA.localeCompare(titleB);
            });
        }

        filteredDropdowns.forEach(option => {
            const optionElement = $('<option>').val(option.id);
            optionElement.text(lang === 'ar' ? option.titleAr : option.titleEn);
            inputElement.append(optionElement);
        });

        if (otherOption) {
            const optionElement = $('<option>').val(otherOption.id);
            optionElement.text(lang === 'ar' ? otherOption.titleAr : otherOption.titleEn);
            inputElement.append(optionElement);
        }

        if (readonly) {
            setReadOnlyAttribute(inputElement, field);
        }

        return inputElement;
    };

    const generateInputField = (field, readonly) => {
        const inputElement = $('<input>')
            .attr('type', field.type)
            .addClass('form-control')
            .attr('placeholder', field.fieldName)
            .attr('aria-label', field.fieldName);

        if (readonly) setReadOnlyAttribute(inputElement, field);

        return inputElement;
    };

    const generateDateField = (field, readonly) => {
        field.type = 'text';
        const dateInputElement = generateInputField(field, readonly);
        field.type = 'date';
        return dateInputElement;
    };

    const generateDatetimeField = (field, readonly) => {
        field.type = 'text';
        const dateTimeInputElement = generateInputField(field, readonly);
        field.type = 'datetime';
        return dateTimeInputElement;
    };
    const generateTimeField = (field, readonly) => {
        const inputElement = $('<input>')
            .attr('type', 'time')
            .addClass('form-control')
            .attr('placeholder', field.fieldName)
            .attr('aria-label', field.fieldName);

        if (field.value) {
            inputElement.val(field.value);
        }

        if (readonly) {
            setReadOnlyAttribute(inputElement, field);
        }

        inputElement.on('keydown', (e) => {
            if (e.keyCode === 13) {
                e.preventDefault();
                return;
            }
        });

        return inputElement;
    };
    const generateListTable = (field, readonly, renderType) => {
        const tableId = renderType === RENDER_TYPE.PREVIEW
            ? `table_View_${field.fieldId}`
            : renderType === RENDER_TYPE.MAJOR
                ? `table_Major_View_${field.fieldId}`
                : `table_${field.fieldId}`;

        const ContainerDiv = $("<div>");
        const wrapperDiv = $("<div>").addClass("tabulator-wrapper");
        const tableDiv = $("<div>").attr("id", tableId);

        wrapperDiv.append(tableDiv);
        ContainerDiv.append(wrapperDiv);

        return ContainerDiv;
    };

    const generateDropdownField = (field, readonly) => {
        const inputElement = $('<select>')
            .addClass('form-select')
            .attr('placeholder', field.fieldName)
            .attr('aria-label', field.fieldName);

        const lang = ns.lang;
        let filteredDropdowns = (window.dropdowns || [])
            .filter(dropdown => dropdown.dropDownTypeId === field.dropDownTypeId)
            .sort((a, b) => a.orderNo - b.orderNo);

        filteredDropdowns.forEach(option => {
            const optionElement = $('<option>').val(option.id);
            if (lang === 'ar') {
                optionElement.text(option.titleAr);
            } else {
                optionElement.text(option.titleEn);
            }
            inputElement.append(optionElement);
        });

        if (readonly) setReadOnlyAttribute(inputElement, field);

        return inputElement;
    };

    const fieldElementGenerators = {
        'number': generateNumberField,
        'dropzone': generateDropzoneField,
        'label': generateLabelField,
        'checkbox': generateCheckboxField,
        'jqte': generateJqteField,
        'file': generateFileField,
        'fileV2': generateFileFieldWithButtons,
        'text': generateDefaultField,
        'textarea': generateTextareaField,
        'Tiny': generateTinyMCEField,
        'select2': generateSelect2Field,
        'dual_select': generateDualSelectField,
        'date': generateDateField,
        'datetime': generateDatetimeField,
        'dropdown': generateDropdownField,
        'list': generateListTable,
        'phone': generatePhoneField,
        'evaluationPlan': generateEvaluationPlanField,
        'evl_Form': generateEvlFormField,
        'Time': generateTimeField,
    };

    const generateField = (field, renderType, actionType = null, options = {}) => {
        const { isOld = false, disableAllForOld = false } = options;

        const hasPreventEditAndValue =
            field.value !== null &&
            field.value !== '' &&
            field.attributes?.some(attr => attr.name === 'preventEdit');

        const lockAllBecauseOld = !!disableAllForOld && isOld;
        const lockThisFieldBecauseOld =
            isOld &&
            field.attributes?.some(attr => attr.name === 'preventEditOldField');

        const isReadonly =
            renderType === RENDER_TYPE.PREVIEW ||
            (field.isApproved === true && actionType !== ACTION_TYPE.INFO_Override_Approve) ||
            (field.isEditable != null && !field.isEditable && actionType !== ACTION_TYPE.SubmitMissingData) ||
            (ReadOnly_ACTION_TYPES || []).includes(actionType) ||
            hasPreventEditAndValue ||
            lockAllBecauseOld ||
            lockThisFieldBecauseOld;

        const generator = fieldElementGenerators[field.type] || generateDefaultField;
        return field.type === 'list' || field.type === 'evaluationPlan' || field.type === 'evl_Form'
            ? generator(field, isReadonly, renderType)
            : generator(field, isReadonly);
    };

  
    // ================== LABEL + HISTORY ==================

    const createFieldLabel = (field) => {
        const isRequired = field.attributes?.some(a => (a.name || '').toLowerCase() === "required");

        const isRadioGroup = field.type === 'radio' || field.type === 'radio_inline';

        const fieldLabel = $('<label>')
            .addClass(`form-label ${isRadioGroup ? 'fw-bold d-block' : 'fw-semibold'}`);

        if (isRequired) {
            fieldLabel.append('<span class="text-danger">*</span> ');
        }

        if (field.type === 'label' || field.type === 'checkbox') {
            fieldLabel.append(field.fieldName);
        } 
      else {
            if ((field.fieldName || '').toLowerCase() !== 'plan details') {
                fieldLabel.append(document.createTextNode(field.fieldName || ''));
            }
        }

        if (field.type === 'checkbox') {
            fieldLabel.attr('for', `field_${field.fieldId}`);
        }

        if (field.fieldTooltip) {
            const fieldInfo = $('<span class="px-1">')
                .addClass('fa fa-info-circle')
                .attr('title', field.fieldTooltip)
                .attr('data-toggle', 'tooltip')
                .attr('data-bs-placement', 'top');
            fieldLabel.append(fieldInfo);
        }

        return fieldLabel;
    };


    const appendHistoryIcon = (field, fieldLabel) => {
        let showHistory_attribute = false;

        if (field.attributes && field.attributes.length > 0) {
            showHistory_attribute = field.attributes?.find(c => c.name.trim() === 'showHistory');
        }

        if (ns.canViewAllFieldHistory || (ns.canViewFieldHistory && showHistory_attribute)) {
            const historyIcon = $('<i>')
                .addClass('fa fa-history mx-1 cursor-pointer')
                .attr('title', 'View History')
                .attr('data-toggle', 'tooltip')
                .attr('data-bs-placement', 'top')
                .on('click', function () {
                    const fieldId = field.fieldId;
                    if (ns.getHistoryAndOpenModal) {
                        ns.getHistoryAndOpenModal(fieldId);
                    }
                });

            fieldLabel.append(historyIcon);
        }
    };

    // ================== CONDITIONS / VISIBILITY ==================

    const evaluateCondition = (fieldValue, operator, comparisonValue) => {
        fieldValue = String(fieldValue).toLowerCase();
        comparisonValue = String(comparisonValue).toLowerCase();
        switch (operator) {
            case 'equal':
                return fieldValue == comparisonValue;
            case 'lessthan':
                return Number(fieldValue) < Number(comparisonValue);
            case 'greaterthan':
                return Number(fieldValue) > Number(comparisonValue);
            case 'in':
                return comparisonValue.split(',').includes(fieldValue);
            case 'not in':
                return !comparisonValue.split(',').includes(fieldValue);
            case 'like':
                return fieldValue.includes(comparisonValue);
            case 'not null':
                return fieldValue !== null && fieldValue !== "null" && fieldValue !== undefined && fieldValue !== '';
            default:
                return false;
        }
    };

    const handleConditionalFields = () => {
        (ns.conditionalFields || []).forEach(({ inputElement, conditions }) => {
            inputElement.hide();

            const evaluateAllConditions = () => {
                let allTrue = true;

                for (const condition of conditions) {
                    let parentField = $(`#field_${condition.parentFieldId}`);
                    if (!parentField.length) {
                        parentField = $(`#field_view_${condition.parentFieldId}`);
                    }

                    let parentFieldValue;
                    if (parentField.is(':checkbox')) {
                        parentFieldValue = parentField.is(':checked');
                    } else {
                        parentFieldValue = parentField.val();
                    }

                    const result = evaluateCondition(parentFieldValue, condition.operators, condition.fieldValue);
                    if (!result) {
                        allTrue = false;
                        break;
                    }
                }

                if (allTrue) {
                    inputElement.show();
                } else {
                    inputElement.hide();
                }

                if (ns.handleFormGroupVisibility) {
                    ns.handleFormGroupVisibility();
                }
            };

            conditions.forEach(condition => {
                let parentField = $(`#field_${condition.parentFieldId}`);
                if (!parentField.length) {
                    parentField = $(`#field_view_${condition.parentFieldId}`);
                }
                parentField.on('change', evaluateAllConditions);
            });

            evaluateAllConditions();
        });
    };

    const evaluateConditionsAfterLoad = () => {
        (ns.conditionalFields || []).forEach(({ inputElement, conditions }) => {
            let displayField = conditions.some(condition => {
                let parentField = $(`#field_${condition.parentFieldId}`);
                if (!parentField || parentField.length === 0) {
                    parentField = $(`#field_view_${condition.parentFieldId}`);
                }

                if (!parentField.length) return false;

                let parentVal = parentField.val();
                if (parentField.is(':checkbox')) {
                    parentVal = parentField.is(':checked');
                }

                return evaluateCondition(parentVal, condition.operators, condition.fieldValue);
            });

            if (displayField) {
                inputElement.show();
            } else {
                inputElement.hide();
            }
        });
    };

    const handleListFieldConditionalFields = (formGroupDTOs) => {
        if (formGroupDTOs && !Array.isArray(formGroupDTOs)) {
            formGroupDTOs = [formGroupDTOs];
        }
        if (formGroupDTOs && formGroupDTOs.length > 0) {
            formGroupDTOs.forEach(formGroupDTO => {
                if (!formGroupDTO.fields) return;

                formGroupDTO.fields.forEach(field => {
                    if (!field.conditions || field.conditions.length === 0) return;

                    const inputElement = $(`#field_${field.fieldId}`);
                    if (!inputElement.length) return;

                    field.conditions.forEach(condition => {
                        let parentField = $(`#field_${condition.parentFieldId}`);
                        if (!parentField.length) return;

                        parentField.on('change input', function () {
                            const parentFieldValue = parentField.is(':checkbox') ? parentField.is(':checked') : parentField.val();
                            const conditionMet = evaluateCondition(parentFieldValue, condition.operators, condition.fieldValue);
                            const parentContainer = inputElement.closest('.colContainer');

                            if (conditionMet) {
                                parentContainer.show();
                            } else {
                                parentContainer.hide();
                            }

                            if (ns.handleFormGroupVisibility) {
                                ns.handleFormGroupVisibility();
                            }
                        });

                        parentField.trigger('change');
                    });
                });
            });
        }
    };

    const evaluateConditionsAfterLoadForList = (formGroupDTOs) => {
        if (!formGroupDTOs) return;

        if (!Array.isArray(formGroupDTOs)) {
            formGroupDTOs = [formGroupDTOs];
        }

        formGroupDTOs.forEach(formGroupDTO => {
            if (!formGroupDTO.fields) return;

            formGroupDTO.fields.forEach(field => {
                if (!field.conditions || field.conditions.length === 0) return;

                const inputElement = $(`#field_${field.fieldId}`);
                if (!inputElement.length) return;

                let displayField = field.conditions.some(condition => {
                    let parentField = $(`#field_${condition.parentFieldId}`);
                    if (!parentField.length) {
                        parentField = $(`#field_view_${condition.parentFieldId}`);
                    }
                    if (!parentField.length) return false;

                    let parentValue = parentField.val();
                    if (parentField.is(':checkbox')) {
                        parentValue = parentField.is(':checked');
                    }

                    switch (condition.operators.toLowerCase()) {
                        case 'equal':
                            return parentValue == condition.fieldValue;
                        case 'notequal':
                            return parentValue != condition.fieldValue;
                        case 'lessthan':
                            return parseFloat(parentValue) < parseFloat(condition.fieldValue);
                        case 'greaterthan':
                            return parseFloat(parentValue) > parseFloat(condition.fieldValue);
                        case 'lessthanorequal':
                            return parseFloat(parentValue) <= parseFloat(condition.fieldValue);
                        case 'greaterthanorequal':
                            return parseFloat(parentValue) >= parseFloat(condition.fieldValue);
                        case 'in':
                            return condition.fieldValue.split(',').includes(parentValue);
                        case 'notnull':
                            return parentValue !== null && parentValue !== undefined && parentValue !== '';
                        case 'null':
                            return parentValue === null || parentValue === undefined || parentValue === '';
                        default:
                            console.warn(`Unknown condition operator: ${condition.operators}`);
                            return false;
                    }
                });

                const parentContainer = inputElement.closest('.colContainer');

                if (displayField) {
                    parentContainer.show();
                } else {
                    parentContainer.hide();
                }
            });
        });
    };

    // ================== CSS / ATTRIBUTES HELPERS ==================

    function hasAttribute(field, attrName) {
        const attrs = Array.isArray(field?.attributes) ? field.attributes : [];
        const target = String(attrName).trim().toLowerCase();
        return attrs.some(a => String(a?.name || '').trim().toLowerCase() === target);
    }

    const applyAttributes = (inputElement, field) => {
        if (!field.attributes) return;

        field.attributes.forEach(attribute => {
            const name = attribute.name.trim();
            const value = attribute.value;

            if (value && value.trim()) {
                inputElement.attr(name, value);
            } else {
                inputElement.attr(name, true);
                if (name === "required") {
                    $("label[for='" + inputElement.attr('id') + "']").append(" *");
                }
            }

            if (name.toLowerCase() === "disablereturn") {
                inputElement.prop('disabled', true)
                    .attr('aria-disabled', 'true');
                inputElement.closest('.toggler, .form-group').addClass('is-disabled');
            }
        });
    };

    const applyCssClasses = (classNameString, elements) => {
        const { colContainer, inputElement, fieldLabel, bodyDiv } = elements;

        if (typeof classNameString === 'string' && classNameString.trim() !== '') {
            const classNames = classNameString.split(' ').map(c => c.trim());

            classNames.forEach(className => {
                const cssClass = (ns.cssClasses || []).find(c => (c.className || '').trim() === className);

                if (cssClass) {
                    switch (cssClass.type) {
                        case "ParentDiv":
                            colContainer.addClass(className);
                            break;
                        case "InputElement":
                            inputElement.addClass(className);
                            break;
                        case "InputElementLapel":
                            fieldLabel.addClass(className);
                            break;
                        case "FormGroup":
                            bodyDiv.addClass(className);
                            break;
                        default:
                            console.warn(`Unrecognized cssClass type: ${cssClass.type}`);
                    }
                }
            });
        }
    };

    // ================== TOGGLERS (Approve / Reject ReturnBack) ==================

    function createToggler(field, disableReturnFlag) {
        const inputvalue = field.isApproved ? '1' : '1';
        const togglerDiv = $('<div>').addClass('toggler');
        const inputElement = $('<input>').attr({
            id: `approvale_${field.fieldId}`,
            name: `approvale_${field.fieldId}`,
            type: 'checkbox',
            value: inputvalue,
            checked: true
        });
        const labelElement = $('<label>').attr('for', `approvale_${field.fieldId}`);

        if (disableReturnFlag) {
            inputElement
                .prop('disabled', true)
                .attr('aria-disabled', 'true')
                .attr('data-disablereturn', 'true');
            togglerDiv.addClass('is-disabled');
        }

        const svgOn = $('<svg>').addClass('toggler-on').attr({
            version: '1.1',
            xmlns: 'http://www.w3.org/2000/svg',
            viewBox: '0 0 130.2 130.2'
        });
        const polyline = $('<polyline>').addClass('path check').attr('points', '100.2,40.2 51.5,88.8 29.8,67.5 ');
        svgOn.append(polyline);

        const svgOff = $('<svg>').addClass('toggler-off').attr({
            version: '1.1',
            xmlns: 'http://www.w3.org/2000/svg',
            viewBox: '0 0 130.2 130.2'
        });
        const line1 = $('<line>').addClass('path line').attr({
            x1: '34.4',
            y1: '34.4',
            x2: '95.8',
            y2: '95.8'
        });
        const line2 = $('<line>').addClass('path line').attr({
            x1: '95.8',
            y1: '34.4',
            x2: '34.4',
            y2: '95.8'
        });
        svgOff.append(line1, line2);

        labelElement.append(svgOn, svgOff);
        togglerDiv.append(inputElement, labelElement);

        return togglerDiv;
    }

    function createMasterToggle(index, lblMasterTogglerApproveText, lblMasterTogglerRejectText) {
        const masterToggleDiv = $('<div>').addClass('master-toggler toggler w-auto d-flex gap-2');
        const inputElement = $('<input>').attr({
            id: `master_toggle_${index}`,
            type: 'checkbox',
            value: '1',
            checked: true
        });

        const textElement = $('<p>').addClass('master-toggle-text');

        function updateText(isChecked) {
            textElement.text(isChecked ? lblMasterTogglerRejectText : lblMasterTogglerApproveText);
        }

        updateText(inputElement.is(':checked'));

        const labelElement = $('<label>').attr('for', `master_toggle_${index}`);

        masterToggleDiv.append(inputElement, labelElement, textElement);

        inputElement.on('change', function () {
            const isChecked = $(this).is(':checked');
            updateText(isChecked);

            const closestCheckbox = $(this)
                .closest('.fieldset-section')
                .find('input[type="checkbox"]')
                .not(this)
                .not(':disabled');

            if (closestCheckbox.length > 0) {
                closestCheckbox.each(function () {
                    $(this).prop('checked', isChecked);
                });
            }
        });

        return masterToggleDiv;
    }

    const appendMasterToggleContainer = (legend, groupIndex) => {
        const lblMasterTogglerAppoveText = uiControlsSetup().GetUiControlText('lblMasterTogglerAppoveText');
        const lblMasterTogglerRejectText = uiControlsSetup().GetUiControlText('lblMasterTogglerRejectText');
        const masterToggleContainer = createMasterToggle(groupIndex, lblMasterTogglerAppoveText, lblMasterTogglerRejectText);
        legend.after(masterToggleContainer);
    };

   

    // ================== RENDER FORM GROUPS (ACTION / PREVIEW) ==================

    const renderFormGroups = (formGroups, renderType, actionType, options = {}) => {
        const container = $('<div>').addClass('form-groups-container');
        const canCollapse = renderType !== RENDER_TYPE.ACTION;

        if (!Array.isArray(formGroups)) {
            formGroups = [formGroups];
        }
        formGroups.sort((a, b) => a.order - b.order);

        const isOld = !!options.isOld;
        const schemaHasPreventEditOld = !!formGroups?.[0]?.fields?.some(f => hasAttribute(f, 'preventEditOld'));
        const disableAllForOld = isOld && schemaHasPreventEditOld;

        formGroups.forEach((group, index) => {
            const fieldset = $('<fieldset>').addClass('fieldset-section');
            const legend = $('<legend>')
                .addClass('cursor-pointer')
                .attr('role', canCollapse ? 'button' : 'heading')
                .text('');
            fieldset.append(legend);

            if (actionType === ACTION_TYPE.RETURNBACK || actionType === ACTION_TYPE.RequestDataChange) {
                appendMasterToggleContainer(legend, index);
            }

            const bodyDiv = $('<div>').addClass('fieldset-body');

            const maxRow = Math.max(...group.fields
                .map(field => Number(field.row))
                .filter(row => !isNaN(row)));

            for (let row = 1; row <= maxRow; row++) {
                const rowContainer = $('<div>').addClass('row');
                const rowFields = group.fields.filter(field => field.row === row);
                const maxColumns = Math.max(...rowFields.map(field => field.column));

                for (let col = 1; col <= maxColumns; col++) {
                    const colContainer = $('<div>').addClass(`colContainer col-md-${12 / maxColumns}`);
                    const field = group.fields.find(field => field.column === col && field.row === row);

                    if (field) {
                        const fieldLabel = createFieldLabel(field);
                        appendHistoryIcon(field, fieldLabel);

                        const fieldElement = generateField(field, renderType, actionType, {
                            isOld: options?.isOld === true,
                            disableAllForOld: options?.disableAllForOld === true || disableAllForOld
                        });
                        if (field.type != 'evl_Form') { 
                        fieldElement.attr(
                            'id',
                            renderType === RENDER_TYPE.PREVIEW
                                ? `field_view_${field.fieldId}`
                                : `field_${field.fieldId}`
                        );

                        if (field.attributes && field.attributes.some(attr => attr.name === "hiddenFieldEditList")) {
                            colContainer.hide();
                        }

                            applyAttributes(fieldElement, field);
                            fieldElement.on('keyup paste input', function () {
                                if (ns.validateInput) {
                                    ns.validateInput(this, field);
                                }
                            });

                        }
                        if (field.type === 'label') {
                            const block = $('<div>').addClass('mb-1');
                            block.append(fieldLabel);
                            colContainer.append(block);
                        } else {
                            const block = $('<div>').addClass('mb-1');

                            if (actionType === ACTION_TYPE.RETURNBACK || actionType === ACTION_TYPE.RequestDataChange) {
                                const DisableReturn = hasAttribute(field, 'disabled');

                                const approvalDiv = $('<div>').addClass('d-flex gap-4');
                                const radioElement = createToggler(field, DisableReturn);

                                const innerdiv = $('<div>').addClass('w-75');
                                innerdiv.append(fieldLabel, fieldElement);

                                approvalDiv.append(radioElement, innerdiv);

                                block.append(approvalDiv);
                                colContainer.append(block);
                            } else {
                                block.append(fieldLabel, fieldElement);
                                colContainer.append(block);
                            }
                        }

                        

                        if (field.conditions && field.conditions.length > 0) {
                            ns.conditionalFields.push({
                                inputElement: colContainer,
                                conditions: field.conditions
                            });
                        }

                      

                        const errorContainer = $('<div>')
                            .attr('id', `error_${field.fieldId}`)
                            .addClass('error-message text-danger');

                        if (renderType === RENDER_TYPE.ACTION) {
                            colContainer.append(errorContainer);
                        }

                        if (field.className) {
                            applyCssClasses(field.className, {
                                colContainer,
                                inputElement: fieldElement,
                                fieldLabel,
                                bodyDiv
                            });
                        }

                        rowContainer.append(colContainer);
                    }
                }

                bodyDiv.append(rowContainer);
            }

            legend.on('click', function () {
                if (canCollapse) {
                    bodyDiv.toggle();
                }
            });

            fieldset.append(bodyDiv);
            container.append(fieldset);
        });

        return container;
    };


    function generateEvaluationPlanField(field, readonly, renderType) {
        let prefield = renderType === RENDER_TYPE.PREVIEW ? "field_view_" : "field_";
        const fieldId = `${prefield}${field.fieldId}`;

        const container = $('<div>')
            .addClass('evaluation-plan-wrapper')
            .attr('data-field-id', fieldId);

        container.html(`<div class="text-muted py-2">Loading evaluation plan...</div>`);
        (async () => {
            try {
                const pu = window.planUtility || window.planutility;
                const PH = window.PlanHandler;
                if (!pu) {
                    container.html(`<div class="text-danger">planUtility not found on window.</div>`);
                    return;
                }
                pu.generatePlanFields(fieldId);

                    const wrapperId = `${fieldId}_wrapper`;
                    const moved = document.getElementById(wrapperId);

                    if (moved) {
                        container.empty().append($(moved));
                    } else {
                        container.html(`<div class="text-danger">Failed to render plan wrapper (${wrapperId}).</div>`);
                        return;
                    }
                //PH.init(
                    
                //    readonly,
                //    fieldId,
                //    JSON.parse(field.value)
                //);
                if (readonly) {
                    container
                        .find('input, select, textarea, button')
                        .prop('disabled', true)
                        .attr('aria-disabled', 'true');

                    container.find(`[id$="_btnSavePlan"], [id$="_btnSubmit"]`).hide();

                    container.find('[data-bs-toggle="offcanvas"]').addClass('disabled').attr('tabindex', '-1');
                }

            } catch (err) {
                console.error('evaluationPlan render failed:', err);
                container.html(`<div class="text-danger">Failed to load evaluation plan.</div>`);
            }
        })();

        return container;
    };

    // ================== EXPORT ON NAMESPACE ==================

    ns.fieldElementGenerators = fieldElementGenerators;
    ns.generateField = generateField;
    ns.createFieldLabel = createFieldLabel;
    ns.appendHistoryIcon = appendHistoryIcon;
    ns.evaluateCondition = evaluateCondition;
    ns.handleConditionalFields = handleConditionalFields;
    ns.evaluateConditionsAfterLoad = evaluateConditionsAfterLoad;
    ns.handleListFieldConditionalFields = handleListFieldConditionalFields;
    ns.evaluateConditionsAfterLoadForList = evaluateConditionsAfterLoadForList;
    ns.renderFormGroups = renderFormGroups;
    //ns.generateEvaluationPlanField = generateEvaluationPlanField;
    ns.applyAttributes = applyAttributes;
    ns.applyCssClasses = applyCssClasses;

})(window.formUtility);
