// form.render.js

window.formUtility = window.formUtility || {};
const formUtility = window.formUtility;

window.serviceRequestForm = window.serviceRequestForm || {};

(function (ns, fu) {

    const { RENDER_TYPE, ACTION_TYPE } = window.FormConstants || {};

    // #region 🧩 Helpers

    const getText = (key) =>
        (window.uiControlsSetup && uiControlsSetup().GetUiControlText(key)) || "";

    const getUrlParam = (key) => {
        const params = new URLSearchParams(window.location.search);
        return (params.get(key) || "").replace("#", "");
    };

    const getRequestId = () => getUrlParam("id");

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
        const actionName = lang === "ar" ? actionDetails.nameAr : actionDetails.nameEn;

        const $btn = $('<button>')
            .addClass('btn btn-success ms-2 btn-sm min-w-auto')
            .attr('type', 'button')
            .attr('id', 'submitButton')
            .text(actionName)
            .on('click', (e) => {
                e.preventDefault();
                e.stopPropagation();

                const doSubmit = () => {
                    if (typeof window.handleSubmit === "function") {
                        window.handleSubmit(requestId, formGroups, actionDetails, /*isDraft*/ false);
                    } else if (typeof ns.submitAction === "function") {
                        ns.submitAction(actionDetails, formGroups);
                    }
                };

                if (actionDetails.isConfirmationAction && window.notificationUtil?.confirmation) {
                    notificationUtil.confirmation(
                        {
                            title: lang === "ar"
                                ? actionDetails.confirmationTitleAr
                                : actionDetails.confirmationTitleEn,
                            body: lang === "ar"
                                ? actionDetails.confirmationBodyAr
                                : actionDetails.confirmationBodyEn,
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

    // #region 👁️‍🗨️ renderPreviewView ()

  
    function renderPreviewView(elementId, formGroups) {
        const groups = normalizeFormGroups(formGroups);
        const $container = $("#" + elementId);
        $container.empty();
        $("#user-wrapper").empty();

        const formGroupsContainer = fu.renderFormGroups
            ? fu.renderFormGroups(groups, RENDER_TYPE.PREVIEW, null)
            : $('<div class="alert alert-warning">renderFormGroups is not defined.</div>');

        $container.append(formGroupsContainer);

        fu.initializeFieldsAndConditions &&
            fu.initializeFieldsAndConditions(groups, elementId, RENDER_TYPE.PREVIEW, null);
    }

    // #endregion

    // #region 🧾 renderActionView (بدون Steps + مع Submit & Draft)

  
    function renderActionView(elementId, formGroups, actionDetails) {
        const groups = normalizeFormGroups(formGroups);
        const $container = $("#" + elementId);
        $container.empty();
        $("#user-wrapper").empty();

        const lang = window.currentLang || "en";
        const requestId = getRequestId();
        const actionTypeName = actionDetails?.actionType?.backEndName;

        if ([ACTION_TYPE.ASSIGN, ACTION_TYPE.Approve_And_Assign].includes(actionTypeName)) {
            if (typeof window.renderPartyTypeFilter === "function") {
                window.renderPartyTypeFilter(actionDetails.assignUsers);
            }
            if (typeof fu.renderAssignTable === "function") {
                fu.renderAssignTable(actionDetails.assignUsers || []);
            }
        }

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
                .addClass('btn btn-warning btn-sm min-w-auto')
                .attr('type', 'button')
                .text(draftText)
                .on('click', (e) => {
                    e.preventDefault();
                    e.stopPropagation();

                    if (typeof window.handleSubmit === "function") {
                        window.handleSubmit(requestId, groups, actionDetails, /*isDraft*/ true);
                    } else if (typeof ns.submitAction === "function") {
                        ns.submitAction(actionDetails, groups, { isDraft: true });
                    }
                });

            $buttonsWrapper.append($draftBtn);
        }

        const submitBtnOrContainer = generateSubmitButton(requestId, groups, actionDetails, false);
        $buttonsWrapper.append(submitBtnOrContainer);

        if (elementId === "Action-container-fields" && $("#Action-container-button").length) {
            const $btnContainer = $("#Action-container-button");
            $btnContainer.empty().append($buttonsWrapper);
        } else {
            $container.append($buttonsWrapper);
        }

        fu.initializeFieldsAndConditions &&
            fu.initializeFieldsAndConditions(groups, elementId, RENDER_TYPE.ACTION, actionTypeName);
    }

    // #endregion

    // #region 🌐 Expose

    ns.renderPreviewView = renderPreviewView;
    ns.renderActionView = renderActionView;
    ns.generateSubmitButton = generateSubmitButton;

    // #endregion

})(window.serviceRequestForm, formUtility);
