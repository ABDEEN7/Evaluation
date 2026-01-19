window.formUtility = window.formUtility || {};
(function (ns) {
    "use strict";

    const jq = () => (typeof jqClient === "function" ? jqClient() : null);

    const escapeHtml = (s) =>
        String(s ?? "")
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");

    const getSelect2Parent = ($root) => {
        const $modal = $root.closest(".modal");
        return $modal.length ? $modal : $(document.body);
    };

    const jsonTryParse = (val) => {
        if (!val) return null;
        try { return JSON.parse(val); } catch { return null; }
    };

    const formatDateISO = (date) => {
        const y = date.getFullYear();
        const m = String(date.getMonth() + 1).padStart(2, "0");
        const d = String(date.getDate()).padStart(2, "0");
        return `${y}-${m}-${d}`;
    };

    const splitRange = (rangeStr) => {
        if (!rangeStr) return null;
        let parts = rangeStr.split(" to ");
        if (parts.length !== 2) return null;
        return { startDate: parts[0], endDate: parts[1] };
    };

    const API = {
        GET_PLAN_TYPES: "/PlanType/GetPlanTypes",
        GET_SEMESTERS: "/Plan/GetSemesters",
        GET_VACATION_DATES: "/AcademicYear/GetVcationDate",
        GET_SCHOOLS: "/School/GetSchools"
    };

    const PLAN_TYPE_BACKEND = {
        YEAR: "Year",
        MONTH: "Month",
        SEMESTER: "Semester",
        CUSTOM: "Custom"
    };

    ns._evaluationPlanCache = ns._evaluationPlanCache || {
        loaded: false,
        planTypes: [],
        semesters: [],
        holidays: [],
        schools: [],
        loadingPromise: null
    };

    const loadAllPlanLookupsOnce = () => {
        const cache = ns._evaluationPlanCache;
        if (cache.loaded) return $.Deferred().resolve().promise();
        if (cache.loadingPromise) return cache.loadingPromise;

        const d = $.Deferred();
        const getReq = (url) => jq()?.Get ? jq().Get(url) : $.get(url);

        cache.loadingPromise = $.when(
            getReq(API.GET_PLAN_TYPES).then(r => cache.planTypes = r?.result || r || []),
            getReq(API.GET_SEMESTERS).then(r => cache.semesters = r?.result || r || []),
            getReq(API.GET_VACATION_DATES).then(r => cache.holidays = (r?.result || r || []).map(x => ({ date: x.date }))),
            getReq(API.GET_SCHOOLS).then(r => cache.schools = r?.result || r || [])
        ).always(() => {
            cache.loaded = true;
            d.resolve();
        });

        return d.promise();
    };

    const buildPlanFormScoped = (planData, readonly, prefix) => {
        const $form = $(`<div class="row g-3"></div>`);

        const title = $(`<input class="form-control" id="${prefix}planTitle">`).val(planData?.title || "");
        const type = $(`<select class="form-control" id="${prefix}ddlPlanType"></select>`);
        const sem = $(`<select class="form-control" id="${prefix}ddlSemester"></select>`);
        const date = $(`<input class="form-control" id="${prefix}parentDate">`).val(planData?.dateRange || "");

        if (readonly) title.add(type).add(sem).add(date).prop("disabled", true);

        $form.append(
            $('<div class="col-md-4"></div>').append(title),
            $('<div class="col-md-4"></div>').append(type),
            $('<div class="col-md-4" style="display:none;"></div>').attr("id", `${prefix}semesterContainer`).append(sem),
            $('<div class="col-md-4"></div>').append(date)
        );

        return $form;
    };

    const buildSchoolsSectionScoped = (planData, readonly, prefix) => {
        const schools = planData?.schools || [];
        const $wrap = $(`<div class="mt-3" id="${prefix}schoolsSection"></div>`);

        const $table = $(`<table class="table table-sm" id="${prefix}tblSchools"><tbody></tbody></table>`);
        const renderRows = ($root) => {
            const $tbody = $table.find("tbody").empty();
            const list = $root.data("evp_schools") || [];
            if (!list.length) return $tbody.append(`<tr><td>لا توجد مدارس</td></tr>`);
            list.forEach((s, i) => {
                $tbody.append(`<tr data-id="${s.schoolId}">
                    <td>${i + 1}</td>
                    <td>${escapeHtml(s.schoolName)}</td>
                    <td><button data-remove>×</button></td>
                </tr>`);
            });
        };

        $wrap.data("renderRows", renderRows);
        $wrap.append($table);
        if (readonly) $wrap.find("button").prop("disabled", true);
        return { $wrap, schools };
    };

    const fillPlanTypes = ($root, prefix, selected) => {
        const ddl = $root.find(`#${prefix}ddlPlanType`).empty();
        ns._evaluationPlanCache.planTypes.forEach(t => {
            const opt = $('<option>')
                .val(t.id)
                .text(t.name)
                .attr("data-backendname", t.backendName);
            if (t.id === selected) opt.prop("selected", true);
            ddl.append(opt);
        });
    };

    const wireSync = ($root, field, prefix) => {
        const $hidden = $root.find(`#field_${field.fieldId}`);
        const sync = () => {
            const payload = {
                title: $root.find(`#${prefix}planTitle`).val(),
                planTypeId: $root.find(`#${prefix}ddlPlanType`).val(),
                semesterId: $root.find(`#${prefix}ddlSemester`).val(),
                dateRange: $root.find(`#${prefix}parentDate`).val(),
                schools: $root.data("evp_schools") || []
            };
            $hidden.val(JSON.stringify(payload));
        };
        $root.on("change keyup", "input,select", sync);
        sync();
    };

    function generateEvaluationPlanField(field, readonly) {
        const prefix = `evp_${field.fieldId}_`;
        const $root = $(`<div id="${prefix}root"></div>`);
        const $hidden = $(`<input type="hidden" id="field_${field.fieldId}">`);
        const planData = jsonTryParse(field.value) || {};
        $root.append($hidden);

        loadAllPlanLookupsOnce().then(() => {
            const $form = buildPlanFormScoped(planData, readonly, prefix);
            fillPlanTypes($root, prefix, planData.planTypeId);
            $root.append($form);

            const schoolSection = buildSchoolsSectionScoped(planData, readonly, prefix);
            $root.data("evp_schools", schoolSection.schools || []);
            $root.append(schoolSection.$wrap);
            schoolSection.$wrap.data("renderRows")($root);

            wireSync($root, field, prefix);
        });

        return $root;
    }

    ns.generateEvaluationPlanField = generateEvaluationPlanField;
    window.generateEvaluationPlanField = generateEvaluationPlanField;

})(window.formUtility);
