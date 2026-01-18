(function (w, $) {
    var DepartmentRouting = sharedUtility().extractDepartmentName();

    const fu = w.formUtility || {};
    const ActionTypes = (w.FormConstants && w.FormConstants.ACTION_TYPE) || {};
    w.dropdowns = w.dropdowns || [];

    let PlanId;
    let SchoolId;
    let initialAction;

    const GetUrlParam = (param) => {
        const params = new URLSearchParams(w.location.search);
        return params.get(param);
    };

    function redirectToDefault() {
        //if (typeof w.sharedUtility === "function") {
        //    w.sharedUtility().RedirectToModuleOrDefault();
        //} else if (w.locationReferrer) {
        //    w.location.href = document.referrer;
        //} else {
        //    w.location.href = "/";
        //}
    }

    async function InitializeCreatePlanRequest() {
        const createPlanService =  await fu.fetchJSON(`/FormRender/GetCreatePlanService`);
        if (!createPlanService) {
            redirectToDefault();
            return;
        }

        const serviceName = (w.currentLang === "ar" ? createPlanService.nameAr : createPlanService.nameEn) || "";
        const headerEl = document.getElementById("CreateRequestModalLabel");
        if (headerEl) headerEl.textContent = serviceName ? " - " + serviceName : "";

        const actions = createPlanService.actions || [];

        if (actions && actions.filter(a => a.isInitialAction === true).length === 1) {
            const firstAction = actions.find(a => a.actionTypeBackEndKey !== ActionTypes.SaveAsDraft);
            initialAction = firstAction ? firstAction.bakendName : initialAction;
            $("#ActionsDropDown").hide();
            $("label[for='ActionsDropDown']").hide();
            await RenderActionFields(createPlanService.serviceRequestDTO);
        } else {
            fillActionDropDown(actions);
        }
    }
    async function InitializeCreateEvaluationPartRequest(serviceId) {
        if (!serviceId) {
            console.error("ServiceId is required");
            redirectToDefault();
            return;
        }
        const CreateEvaluationPartyService = await fu.fetchJSON(
            `/FormRender/GetCreateEvaluationPartyService?serviceId=${encodeURIComponent(serviceId)}`
        );

        if (!CreateEvaluationPartyService) {
            redirectToDefault();
            return;
        }

        const serviceName =
            (w.currentLang === "ar"
                ? CreateEvaluationPartyService.nameAr
                : CreateEvaluationPartyService.nameEn) || "";

        const headerEl = document.getElementById("CreateRequestModalLabel");
        if (headerEl) headerEl.textContent = serviceName ? " - " + serviceName : "";

        const actions = CreateEvaluationPartyService.actions || [];

        if (actions.filter(a => a.isInitialAction === true).length === 1) {
            const firstAction = actions.find(
                a => a.actionTypeBackEndKey !== ActionTypes.SaveAsDraft
            );

            initialAction = firstAction ? firstAction.bakendName : initialAction;

            $("#ActionsDropDown").hide();
            $("label[for='ActionsDropDown']").hide();

            await RenderActionFields(CreateEvaluationPartyService.serviceRequestDTO);
        } else {
            fillActionDropDown(actions);
        }
    }

    const fillActionDropDown = (actions) => {
        if (!actions || actions.length < 2) {
            $("#ActionsDropDown").hide();
            $("label[for='ActionsDropDown']").hide();
            return;
        }

        actions.forEach(action => {
            const text = action.title || (w.currentLang === "ar" ? action.nameAr : action.nameEn);
            const option = new Option(text, action.bakendName);
            $("#ActionsDropDown").append($(option));
        });

        initialAction = $("#ActionsDropDown").val();

        $("#ActionsDropDown").on("change", function () {
            initialAction = $(this).val();
        });
    };

    const fillSchoolsDropDown = (schools) => {
        (schools || []).forEach(s => {
            const text = `${s.schoolName || ""} - ${s.code || ""} - ${s.typeName || ""} - ${s.stageName || ""}`;
            const option = new Option(text, s.id);
            $("#School").append($(option));
        });

        $("#School").select2({
            width: "100%",
            dropdownCssClass: "manageselect2zindex",
            dropdownParent: $("#SchoolModal"),
            allowClear: true,
            placeholder: (w.uiControlsSetup && uiControlsSetup().GetUiControlText("lblSelect")) || "Select…",
            minimumResultsForSearch: 0
        });

        if (SchoolId) $("#School").val(SchoolId).trigger("change");

        $("#School")
            .removeAttr("hidden")
            .off("change")
            .on("change", function () {
                SchoolId = $(this).val();
            });

        $("label[for='School']").removeAttr("hidden");
    };

    const configureSchoolModal = () => {
        $("#SchoolModal").modal({ backdrop: "static", keyboard: false });
        $("#SchoolModal").modal("show");

        $("#submitButtonModal").off("click").on("click", async function () {
            if ($("#School").val()) {
                $("#School").closest(".mb-3").find(".messages").removeClass("text-danger").text("");
                await GetActionFields();
                $("#SchoolModal").modal("hide");
            } else {
                $("#School").closest(".mb-3").find(".messages")
                    .addClass("text-danger")
                    .text((w.uiControlsSetup && uiControlsSetup().GetUiControlText("lblIsRequired")) || "Required");
            }
        });

        $("#cancelButtonModal, .btn-close").off("click").on("click", function () {
            redirectToDefault();
        });
    };

    $("#Requestbtns1, #CloseRequestup").off("click").on("click", function () {
        redirectToDefault();
    });

    $("#goBackBtn").off("click").on("click", function () {
        redirectToDefault();
    });

    async function CheckCanCreateingDraft() {
        try {
            const url = `/EvaluationPlanRequest/${DepartmentRouting}/CheckCanCreateingDraft?planId=${PlanId}`;
            const response = await jqClient().SyncGet(url);
            return response;
        } catch (e) {
            return false;
        }
    }

    async function GetActionFields() {
        try {
            let url = `/EvaluationPlanRequest/${DepartmentRouting}/GetActionField?planId=${PlanId}`;
            if (initialAction) url += `&actionBackendKey=${encodeURIComponent(initialAction)}`;
            if (SchoolId) url += `&schoolId=${encodeURIComponent(SchoolId)}`;

            const ddIds = Array.isArray(fu.dropDownTypeIds) ? fu.dropDownTypeIds : [];
            url += `&dropDownTypeIds=${encodeURIComponent(ddIds.join(","))}`;

            const request = null;// await fu.fetchJSON(url);
            await RenderActionFields(request);
        } catch (e) {
            console.error(e);
        }
    }

    async function RenderActionFields(request) {
        try {
            if (!request || !request.actionCustom) return;

            const actionDetails = request.actionCustom;
            const formGroups = actionDetails.formGroups || [];
            const dropdownsData = request.dropDownValues || [];

            if (dropdownsData && Array.isArray(dropdownsData)) {
                dropdownsData.forEach(item => {
                    if (!w.dropdowns.some(x => x.id === item.id && x.dropDownTypeId === item.dropDownTypeId)) {
                        w.dropdowns.push(item);
                        fu.dropDownTypeIds = fu.dropDownTypeIds || [];
                        if (!fu.dropDownTypeIds.includes(item.dropDownTypeId)) {
                            fu.dropDownTypeIds.push(item.dropDownTypeId);
                        }
                    }
                });
            }

            const attachments = request.planAttachments || request.attachments;
            if (attachments && attachments.length > 0) {
                fu.attachments = fu.attachments || [];
                fu.attachments.push(...attachments);
            }

            fu.renderActionView("content-container", formGroups, actionDetails);

            if (typeof w.InitializeTooltip === "function") w.InitializeTooltip();
        } catch (e) {
            console.error(e);
        }
    }

    w.InitializeCreatePlanRequest = InitializeCreatePlanRequest;
    w.InitializeCreateEvaluationPartRequest = InitializeCreateEvaluationPartRequest;
    w.GetActionFields = GetActionFields;
    w.RenderActionFields = RenderActionFields;
    w.configureSchoolModal = configureSchoolModal;
    w.fillSchoolsDropDown = fillSchoolsDropDown;
    w.CheckCanCreateingDraft = CheckCanCreateingDraft;

    $(function () {
        PlanId = GetUrlParam("planId") || GetUrlParam("PlanId") || PlanId;
        SchoolId = GetUrlParam("schoolId") || GetUrlParam("SchoolId") || SchoolId;
        //InitializeCreatePlanRequest();
    });

})(window, jQuery);
