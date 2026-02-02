// form.render.js

window.formUtility = window.formUtility || {};

window.serviceRequestForm = window.serviceRequestForm || {};

(function (ns, fu) {

    const { RENDER_TYPE, ACTION_TYPE } = window.FormConstants || {};
    var DepartmentRouting = sharedUtility().extractDepartmentName();
    // #region 🧩 Helpers


    function getUrlParam(param) {
        const params = new URLSearchParams(window.location.search);
        return params.get(param);
    }

    function addQueryParameter(key, value) {
        const url = new URL(window.location.href);
        url.searchParams.set(key, value);
        history.pushState(null, "", url.toString());
    }

    function removeQueryParameter(keys) {
        const url = new URL(window.location.href);
        (keys || []).forEach(key => url.searchParams.delete(key));
        history.pushState(null, "", url.pathname + url.search);
    }
    const getText = (key) =>
        (window.uiControlsSetup && uiControlsSetup().GetUiControlText(key)) || "";

  
    const getRequestId = () => {
        const id = getUrlParam("id");
        return id ? id : getUrlParam("Evlid") ;
    };
    const getServiceId = () => getUrlParam("serviceId");

    const normalizeFormGroups = (formGroups) => {
        if (!formGroups) return [];
        if (Array.isArray(formGroups)) return formGroups;
        if (Array.isArray(formGroups.formGroups)) return formGroups.formGroups;
        return [formGroups];
    };

    // #endregion

    // #region 🔘 Generate Submit Button 

    const generateSubmitButton = (requestId, formGroups, actionDetails, wrapInContainer = true) => {
        const lang = window.currentLang || "en";
        const btnText = lang === "ar" ? actionDetails.nameAr : actionDetails.nameEn;

        const $btn = $('<button>')
            .addClass('btn btn-primary mw-200')
            .attr('type', 'button')
            .attr('id', 'submitButton')
            .text(btnText)
            .on('click', (e) => {
                e.preventDefault();
                e.stopPropagation();

                const doSubmit = () => {
                    ns.submitAction(actionDetails, formGroups,  saveAsDraft= false );
                };

                if (actionDetails.isConfirmationAction && window.notificationUtil?.confirmation) {
                    notificationUtil.confirmation(
                        {
                            title: lang === "ar" ? actionDetails.confirmationTitleAr : actionDetails.confirmationTitleEn,
                            body: lang === "ar" ? actionDetails.confirmationBodyAr : actionDetails.confirmationBodyEn,
                            okText: getText('lblOk'),
                            cancelText: getText('lblCancel'),
                        },
                        function () { doSubmit(); },
                        function () { }
                    );
                } else {
                    doSubmit();
                }
            });

        if (!wrapInContainer) return $btn;

        const $container = $('<div class="text-end mt-3">');
        $container.append($btn);
        return $container;
    };

    // #endregion



    // #region 🧾 render Action DropDown & actionTransactions

    const fetchAndRenderActionData = (backendKey, requestId, serviceId, modalContainer = 'Action-container-fields', UseactionModal =false, ctx = {}) => {

        if (UseactionModal) { 
            modalContainer = 'Action-container-fields';
            ctx.actionModalId = 'actionModal';
        }

        const actionModalId = ctx.actionModalId || 'actionModal';
        const actionModalRoot = $('#' + actionModalId);

        serviceId = getServiceId();
        requestId = getRequestId();

        const isEvaluationRequest = true;

            const url =
                `/FormRender/${DepartmentRouting}/GetActionField` +
                `?ActionbackendKey=${encodeURIComponent(backendKey)}` +
                `&requestId=${encodeURIComponent(requestId)}` +
            `&serviceId=${encodeURIComponent(serviceId || "")}`+
                    `&isEvaluationRequest=${isEvaluationRequest}`;


            jqClient({
                success: function (response) {

                    const actionDetails = response?.actionCustom || null;
                    const stepsData = response?.actionCustom?.formGroups || [];
                    const dropdownsData = response?.dropDownValues || [];
                    const attachments = response?.schAttachments || [];

                    if (attachments.length > 0) {
                        formUtility.attachments = formUtility.attachments || [];
                        formUtility.attachments.push(...attachments);
                    }

                    if (Array.isArray(dropdownsData)) {
                        window.dropdowns = window.dropdowns || [];
                        window.dropDownTypeIds = window.dropDownTypeIds || [];

                        dropdownsData.forEach(item => {
                            const exists = dropdowns.some(x => x.id === item.id && x.dropDownTypeId === item.dropDownTypeId);
                            if (!exists) dropdowns.push(item);
                        });
                    }

                    if (actionDetails) {
                        const lang = window.currentLang || "en";
                        const actionName = lang === "ar" ? (actionDetails.nameAr || "") : (actionDetails.nameEn || "");

                        actionModalRoot.find('#submitModalForm').text(actionName);
                        actionModalRoot.find('#ActionModalTitle').text(actionName);

                        if (typeof applyNotesToModal === "function") applyNotesToModal();
                    }

                    if (typeof renderActionView === "function") {
                        renderActionView(modalContainer, stepsData, actionDetails);
                    }

                    actionModalRoot.modal('show');

                    actionModalRoot
                        .off('shown.bs.modal.redraw')
                        .on('shown.bs.modal.redraw', function () {
                            if (window.Tabulator?.findTable) {
                                Tabulator.findTable(`#${actionModalId} .tabulator`).forEach(t => t.redraw(true));
                            }
                        });

                    $('.modal-backdrop').remove();
                }
            }).Get(url);
        
    };


    const renderActionsDropDown = (actions, containerId, templateContainerId, modalContainer, requestIdOverride, serviceIdOverride, ctx = {}) => {

        const root = ctx.root ? $(ctx.root) : $(document);

        const actionsContainer = root.find(`#${containerId}`);
        const templateContainer = root.find(`#${templateContainerId}`);

        actionsContainer.empty().removeAttr('aria-busy');
        templateContainer.empty();

        if (!actions?.length) return;

        const resolvedRequestId = requestIdOverride || null;
        const resolvedServiceId = serviceIdOverride || null;

        const renderDropdownUI = () => {
            const dropdownWrapper = $('<div>').addClass('dropdown');

            const buttonText =
                (window.uiControlsSetup && uiControlsSetup().GetUiControlText('lblProcedures')) || 'Procedures';

            const button = $('<button>', {
                class: 'btn btn-primary btn-sm dropdown-toggle',
                type: 'button',
                'data-bs-toggle': 'dropdown'
            }).text(buttonText);

            const dropdownMenu = $('<ul>', { class: 'dropdown-menu' });

            actions.forEach((action, index) => {
                dropdownMenu.append(
                    $('<li>').append(
                        $('<a>', {
                            class: 'dropdown-item py-0',
                            href: '#',
                            'data-backend': action.bakendName,
                            'data-actionTypeBackEndKey': action.actionTypeBackEndKey
                        }).html(`<small>${action.title}</small>`)
                    )
                );
            });

            dropdownMenu.off('click.actions').on('click.actions', 'a[data-backend]', function (e) {
                e.preventDefault();

                const backendName = $(this).data('backend');
                const actionTypeBackEndKey = $(this).data('actiontypebackendkey');

                fetchAndRenderActionData(
                    backendName,
                    resolvedRequestId,
                    resolvedServiceId,
                    modalContainer,
                    true,
                    ctx 
                );
            });

            dropdownWrapper.append(button, dropdownMenu);
            actionsContainer.append(dropdownWrapper);
        };

        renderDropdownUI();
    };


    function renderTransactionsSection(actionTransactions, ctx = {}) {

        const root = ctx.root ? $(ctx.root) : $(document);
        const notesSelector = ctx.notesSelector || '#divNotes';
        const logSelector = ctx.logTableContainerSelector || '#logTableContainer';

        actionTransactions = actionTransactions ?? [];
        if (!Array.isArray(actionTransactions)) actionTransactions = [actionTransactions];

        if (actionTransactions.length > 0) {
            createActionTransactionsTable(actionTransactions, logSelector, ctx);

            const lastRemark = actionTransactions[actionTransactions.length - 1]?.remarks || "";
            const $notes = root.find(notesSelector);

            if (lastRemark) $notes.text(lastRemark).addClass('warning');
            else $notes.text('').removeClass('warning');

        } else {
            root.find(notesSelector).text('').removeClass('warning');
            root.find(logSelector).empty();
        }
    }


    function createActionTransactionsTable(data, logContainerSelector = '#logTableContainer', ctx = {}) {

        const root = ctx.root ? $(ctx.root) : $(document);
        const $logWrap = root.find(logContainerSelector);

        const tableId = `History-table-${(ctx.requestId || 'x').toString().replaceAll('-', '')}`;

        const wrapperDiv = $('<div>').addClass('tabulator-wrapper');
        const tableDiv = $('<div>').attr('id', tableId).addClass('table table-bordered table-hover align-middle w-100 dataTable no-footer');

        wrapperDiv.append(tableDiv);
        $logWrap.html(wrapperDiv);

     
        let columns = [];

        //if (userProfileDetailsInfo.UserType === "") {
        //    columns = [
        //        { title: uiControlsSetup().GetUiControlText('lblTranslogActionName'), field: "action", tooltip: true },
        //        { title: uiControlsSetup().GetUiControlText('lblTranslogNote'), field: "remarks", tooltip: true },
        //        { title: uiControlsSetup().GetUiControlText('lblTranslogDate'), cssClass: 'dateClazz', field: "formattedCreatedDate" },
        //        {
        //            title: uiControlsSetup().GetUiControlText('lblViewDetails'),
        //            field: "",
        //            formatter: function (cell) {
        //                const { id } = cell.getRow().getData();
        //                return getActionTransactionsTemplate(id);
        //            },
        //            cellClick: actionTransactionsCellClick
        //        },
        //    ];
        //} else if (userProfileDetailsInfo.UserType === "Ministry") {
            columns = [
                { title: uiControlsSetup().GetUiControlText('lblTranslogActionName'), field: "action", tooltip: true },
                { title: uiControlsSetup().GetUiControlText('lblTranslogUserName'), field: "actor", tooltip: true },
                { title: uiControlsSetup().GetUiControlText('lblTranslogFrom'), field: "previousStatus", tooltip: true },
                { title: uiControlsSetup().GetUiControlText('lblTranslogTo'), field: "nextStatus", tooltip: true },
                { title: uiControlsSetup().GetUiControlText('lblTranslogDate'), cssClass: 'dateClazz', field: "formattedCreatedDate" },
                { title: uiControlsSetup().GetUiControlText('lblRemarks'),  field: "remarks" },
                //{
                //    title: uiControlsSetup().GetUiControlText('lblViewDetails'),
                //    field: "",
                //    formatter: function (cell) {
                //        const { id } = cell.getRow().getData();
                //        return getActionTransactionsTemplate(id);
                //    },
                //    cellClick: actionTransactionsCellClick
                //},
            ];
        //}

        new Tabulator(`#${tableId}`, {
            data,
            layout: "fitColumns",
            columns
        });
    }

    const actionTransactionsCellClick = (e, cell) => {
        const rowData = cell.getRow().getData();
        showDetailsModal(rowData.remarks, rowData.actionTransactionAttachments, rowData.action);
    };

    function showDetailsModal(remarks, attachments, action) {

        $('#testRemarks').text(remarks);
        $('#testRemarks').prop('disabled', true);
        $('#actionTransactionsdetailsLabel').text(action);
        const attachmentsSection = $('#attachmentsSection');
        attachmentsSection.empty();
        if (attachments && attachments.length) {

            const attacNameFormatter = (cell, formatterParams, onRendered) => {
                let cellValue = cell.getValue();
                let rowData = cell.getRow().getData();

                return `<a href="javascript:void(0);" class="view-attachment" data-id="${rowData.id}" data-name="${cellValue}">${cellValue}</a>`;
            };
            let table = new Tabulator("#attachmentsSection", {
                data: attachments,
                layout: "fitColumns",
                columns: [
                    {
                        title: uiControlsSetup().GetUiControlText('lblTranslogfilename'), field: "uiFileName", formatter: attacNameFormatter
                    },
                    {
                        title: uiControlsSetup().GetUiControlText('lblActions'), field: "id", formatter: function (cell, formatterParams, onRendered) {
                            return `<button class='btn btn-primary btn-sm' onclick='downloadAttach("${cell.getValue()}", "${cell.getRow().getData().uiFileName}")'>Download</button>`;
                        }
                    }
                ],
            });


            table.setData(attachments);
        } else {
            const noAttachmentsLabel = $('<label>')
                .text(uiControlsSetup().GetUiControlText('lblNoAttachmentsAvailable')).addClass('no-file-label');

            attachmentsSection.append(noAttachmentsLabel);
        }

        $('#actionTransactionsdetailsModal').modal('show');
    }
    // #endregion

    // #region 👁️‍🗨️ renderPreviewView ()
    function renderPreviewView(elementId, formGroups, actions, actionTransactions, attachments, ctx = {}) {

        const root = ctx.root ? $(ctx.root) : $(document);

        const groups = normalizeFormGroups(formGroups);
        const $container = root.find("#" + elementId);

        $container.empty();
        root.find("#user-wrapper").empty();

        actions = actions ?? [];
        actionTransactions = actionTransactions ?? [];
        attachments = attachments ?? [];

        if (!Array.isArray(actions)) actions = [actions];
        if (!Array.isArray(actionTransactions)) actionTransactions = [actionTransactions];

        formUtility.attachments = attachments;

        const actionsContainerId = ctx.actionsContainerId || 'actions-container';
        const templateContainerId = ctx.templateContainerId || 'divTemplates';
        const modalContainerId = ctx.modalContainerId || 'Action-container-fields';

        if (actions.length > 0 ) {
            renderActionsDropDown( actions,actionsContainerId,templateContainerId,modalContainerId,ctx.requestId,ctx.serviceId, ctx );
        } else {
            root.find(`#${actionsContainerId}`).empty();
            root.find(`#${templateContainerId}`).empty();
        }

        const formGroupsContainer = fu.renderFormGroups
            ? fu.renderFormGroups(groups, RENDER_TYPE.PREVIEW, null)
            : $('<div class="alert alert-warning">renderFormGroups is not defined.</div>');

        $container.append(formGroupsContainer);

          fu.initializeFormFieldsAndConditions(groups, elementId, RENDER_TYPE.PREVIEW, null);
      renderTransactionsSection(actionTransactions, ctx);
        
    }



    // #endregion

    // #region 🧾 renderActionView ( Submit & Draft)

  
    function renderActionView(elementId, formGroups, actionDetails) {

        const groups = normalizeFormGroups(formGroups);
        const $container = $("#" + elementId);
        const $modal = $container.closest(".modal"); 
        $container.empty();
        $("#user-wrapper").empty();

        const lang = window.currentLang || "en";

        const actionTypeName = actionDetails?.actionType?.titleEn;

        const requestId = actionDetails?.requestId || getRequestId();

        if ([ACTION_TYPE.ASSIGN, ACTION_TYPE.Approve_And_Assign].includes(actionTypeName)) {
            fu.renderAssignTable(actionDetails.assignUsers || []);
        }

        // =============== ASSIGN TEAM ===============
        if (actionTypeName === ACTION_TYPE.ASSIGNT_TEAM) {

            assignmentsUtility.generateAssignments('assign');

        }
        // ==========================================

        const formGroupsContainer = fu.renderFormGroups
            ? fu.renderFormGroups(groups, RENDER_TYPE.ACTION, actionTypeName)
            : $('<div class="alert alert-warning">renderFormGroups is not defined.</div>');

        $container.append(formGroupsContainer);

        if (actionDetails.isRemark && typeof window.createRemarkContainer === "function") {
            const remarkBlock = createRemarkContainer(
                actionDetails.isRemarkRequired,
                actionDetails.remarkLabel
            );
            $container.append(remarkBlock);
        }

        if (actionDetails.isOtherAttachment && typeof window.createOthersDropZone === "function") {
            createOthersDropZone(
                $container,
                actionDetails.isOtherAttachmentRequired,
                actionDetails.attachmentLabel
            );
        }

        const showDraft =
            actionDetails.allowDraft &&
            (actionDetails.isInitialAction ||
                actionTypeName === ACTION_TYPE.INFO_WITH_DRAFT);

        const $buttonsWrapper = $('<div class="d-flex justify-content-end gap-2 mt-3">');

        if (showDraft) {
            const draftText = getText('lblSaveAsDraft') || 'Save as Draft';
            const $draftBtn = $('<button>')
                .addClass('btn btn-warning btn-sm min-w-auto mw-200')
                .attr('type', 'button')
                .text(draftText)
                .on('click', (e) => {
                    e.preventDefault();
                    e.stopPropagation();
                    ns.submitAction(actionDetails, groups, saveAsDraft = true);
                });

            $buttonsWrapper.append($draftBtn);
        }

        const submitBtnOrContainer = generateSubmitButton(requestId, groups, actionDetails, false);
        $buttonsWrapper.append(submitBtnOrContainer);

        const isActionModal = $modal.attr("id") === "actionModal";
        const $btnContainer = isActionModal
            ? $modal.find("#Action-container-button")
            : $modal.find("#Requestbtns");   

        if ($btnContainer.length) {
            $btnContainer.empty().append($buttonsWrapper);
        } else {
            $container.append($buttonsWrapper);
        }

        
        fu.initializeFormFieldsAndConditions(groups, elementId, RENDER_TYPE.ACTION, actionTypeName);

        if (actionTypeName === ACTION_TYPE.ASSIGNT_TEAM) {

           
            const moveAndInit = () => {
                const $wrapper = $('#assign_wrapper');
                if (!$wrapper.length) return false;

                $container.empty().append($wrapper);

                assignmentsLogic.init('assign', requestId, elementId);
                return true;
            };

            if (!moveAndInit()) {
                setTimeout(() => moveAndInit(), 100);
            }

            // return; 
        }
    }


    // #endregion

    // #region 🌐 Expose

    ns.renderPreviewView = renderPreviewView;
    ns.renderActionView = renderActionView;
    ns.generateSubmitButton = generateSubmitButton;
    fu.addQueryParameter = addQueryParameter;
    fu.getUrlParam = getUrlParam;
    fu.removeQueryParameter = removeQueryParameter;

    // #endregion

})(window.serviceRequestForm, window.formUtility);
