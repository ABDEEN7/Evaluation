(function () {
    "use strict";

    /* ════════════════════════════════════════════════════════════════
       CONFIG — adjust these to match your environment
    ════════════════════════════════════════════════════════════════ */

    // Path where output-analysis-report.html is hosted (wwwroot/static).
    const REPORT_URL = "/Reports/OutputAnalysisReport.html";

    // grade (number) -> level bucket. Adjust the cutoffs if your school
    // stages differ.
    function gradeToLevel(grade) {
        const g = Number(grade);
        if (g <= 6) return "primary";
        if (g <= 9) return "middle";
        return "secondary";
    }

    // subjectCode -> human-readable Arabic subject name. The API only
    // gives us a code (e.g. "01200301"), not a name, so this map is
    // empty by default and falls back to the raw code. Fill it in with
    // your real CourseCode -> subject-name lookup if you have one
    // (e.g. from a Subjects table), or fetch it once and populate this
    // object before calling openOutputsAnalysis.
    const SUBJECT_NAME_MAP = {
        // "01200301": "اللغة العربية",
    };
    function subjectName(code) {
        if (!code) return "—";
        return SUBJECT_NAME_MAP[code] || code;
    }

    // track -> short Arabic label appended to the grade (used for grade
    // 11/12 rows, which the API splits by track instead of subjectCode).
    // Empty by default — falls back to the raw track string.
    const TRACK_LABEL_MAP = {
        // "Science - Religious": "علمي شرعي",
        // "Humanities - Religious": "أدبي شرعي",
    };
    function trackLabel(track) {
        if (!track) return "";
        return TRACK_LABEL_MAP[track] || track;
    }

    function gradeLabel(grade, track) {
        const t = trackLabel(track);
        return t ? `${grade} ${t}` : String(grade);
    }

    // For analysisTypes whose rows repeat per grading-period (the API
    // sample shows termCode values like "S1","S2","1SR1".."1SR4" for
    // LowPerformanceStudents — multiple rows per grade+subject), we
    // need to pick ONE representative row per (grade, subjectCode/track)
    // group. This is the preference order, most-final first. Adjust if
    // your termCode convention is different.
    const TERM_PREFERENCE = ["S2", "S1", "1S"];

    function pickPreferredRow(rows) {
        for (const term of TERM_PREFERENCE) {
            const found = rows.find(r => r.termCode === term);
            if (found) return found;
        }
        return rows[0];
    }

    function groupBy(rows, keyFn) {
        const map = new Map();
        rows.forEach(r => {
            const k = keyFn(r);
            if (!map.has(k)) map.set(k, []);
            map.get(k).push(r);
        });
        return map;
    }

    /* ════════════════════════════════════════════════════════════════
       ADAPTER — raw API JSON  →  report's DATA / SEC_DATA shape
       Raw shape expected (per analysisTypes[i]):
         { analysisTypeId, analysisTypeNameAr, analysisTypeNameEn,
           backendName, outputAnalysisFinalResult: {actualValue,...}|null,
           outputAnalysisData: [ {grade, lastYear, previousYear,
             lastYearValue, previousYearValue, difference, subjectCode,
             track, actualValue, note, lastYearStudentCount,
             previousYearStudentCount, termCode, ...} ] }
    ════════════════════════════════════════════════════════════════ */

    function findType(apiJson, backendName) {
        return (apiJson.analysisTypes || []).find(t => t.backendName === backendName) || null;
    }

    function finalScore(apiJson, backendName) {
        const t = findType(apiJson, backendName);
        const v = t && t.outputAnalysisFinalResult ? t.outputAnalysisFinalResult.actualValue : null;
        return v === null || v === undefined ? 0 : Number(v);
    }

    // 3.1.4 — Academic Achievement: one row per grade+subject (or
    // grade+track for grades 11/12), termCode "1S" in the sample → no
    // de-duplication needed, map straight across.
    function buildAcademic(apiJson) {
        const t = findType(apiJson, "AcademicAchievement");
        if (!t) return [];
        return (t.outputAnalysisData || []).map(r => ({
            level: gradeToLevel(r.grade),
            grade: gradeLabel(r.grade, r.track),
            students1: r.previousYearStudentCount || 0,
            students2: r.lastYearStudentCount || 0,
            subject: r.track ? trackLabel(r.track) : subjectName(r.subjectCode),
            prev: Number(r.previousYearValue || 0),
            last: Number(r.lastYearValue || 0),
            diff: Number(r.difference || 0),
            judge: r.note || ""
        }));
    }

    // 3.2.2 — Low Performance Students: the API can carry several rows
    // per (grade, subject/track) — one per grading period (S1, S2,
    // 1SR1..1SR4). We keep only the most-final one per group, see
    // TERM_PREFERENCE above.
    function buildLow(apiJson) {
        const t = findType(apiJson, "LowPerformanceStudents");
        if (!t) return [];
        const rows = t.outputAnalysisData || [];
        const grouped = groupBy(rows, r => `${r.grade}__${r.subjectCode || ""}__${r.track || ""}`);
        const out = [];
        grouped.forEach(group => {
            const r = pickPreferredRow(group);
            out.push({
                level: gradeToLevel(r.grade),
                grade: gradeLabel(r.grade, r.track),
                subject: r.track ? trackLabel(r.track) : subjectName(r.subjectCode),
                count: r.lastYearStudentCount || 0,
                pct: Number(r.lastYearValue || 0)
            });
        });
        return out;
    }

    // الراسبون — Failed Students: one row per grade (or grade+track for
    // 11/12), termCode "1S" in the sample → map straight across.
    function buildFailed(apiJson) {
        const t = findType(apiJson, "FailedStudents");
        if (!t) return [];
        return (t.outputAnalysisData || []).map(r => ({
            level: gradeToLevel(r.grade),
            grade: gradeLabel(r.grade, r.track),
            prev: Number(r.previousYearValue || 0),
            last: Number(r.lastYearValue || 0)
        }));
    }

    // 3.2.4 — Students With Disabilities: group whatever rows exist by
    // level. There is no sample data with real rows for this type yet,
    // so when outputAnalysisData is empty we fall back to a 0-filled
    // skeleton for the three levels (same as the report's own default)
    // rather than guessing a shape.
    function buildDisability(apiJson) {
        const t = findType(apiJson, "StudentsWithDisabilities");
        const skeleton = ["primary", "middle", "secondary"].map(level => ({ level, students: 0, score: 0, sub: [] }));
        if (!t || !t.outputAnalysisData || !t.outputAnalysisData.length) return skeleton;

        const byLevel = groupBy(t.outputAnalysisData, r => gradeToLevel(r.grade));
        return skeleton.map(s => {
            const rows = byLevel.get(s.level) || [];
            if (!rows.length) return s;
            const students = rows.reduce((a, r) => a + (r.lastYearStudentCount || 0), 0);
            const score = rows.reduce((a, r) => a + Number(r.actualValue || 0), 0) / rows.length;
            return { level: s.level, students, score, sub: rows.map(r => Number(r.actualValue || 0)) };
        });
    }

    // مستويات الطلبة (متفوقون/جيد جداً/جيد/مقبول/راسبون): the
    // StudentLevelsAnalysis analysisType is empty in every sample we've
    // seen, and there's no documented way to map its rows to the 5
    // bands yet. Left at the report's own zeroed default rather than
    // guessing — wire this up once that analysisType actually returns
    // rows and you know their shape.
    function buildLevels(apiJson) {
        return undefined; // let the report keep its own zeroed default
    }

    // Derive the grade pickers (DATA.grades.primary/middle/secondary)
    // from whatever grades actually showed up in academic/low/failed.
    function buildGrades(academic, low, failed) {
        const sets = { primary: new Set(), middle: new Set(), secondary: new Set() };
        [...academic, ...low, ...failed].forEach(r => {
            sets[r.level] && sets[r.level].add(r.grade);
        });
        return {
            primary: [...sets.primary],
            middle: [...sets.middle],
            secondary: [...sets.secondary]
        };
    }

    // "2024-2025" style labels from whatever lastYear/previousYear show
    // up first in the payload.
    function buildYears(apiJson) {
        for (const t of apiJson.analysisTypes || []) {
            const r = (t.outputAnalysisData || [])[0];
            if (r && r.lastYear && r.previousYear) {
                return [`${r.previousYear - 1}-${r.previousYear}`, `${r.lastYear - 1}-${r.lastYear}`];
            }
        }
        return ["", ""];
    }

    function buildSummary(apiJson) {
        return {
            std314: finalScore(apiJson, "AcademicAchievement"),
            std322: finalScore(apiJson, "LowPerformanceStudents"),
            std324: finalScore(apiJson, "StudentsWithDisabilities")
            // std311 (international assessments) and std312 (national
            // tests) have no source in this API — left at 0.
        };
    }

    // Builds the full postMessage payload from one raw API response.
    function buildReportPayload(apiJson) {
        const academic = buildAcademic(apiJson);
        const low = buildLow(apiJson);
        const failed = buildFailed(apiJson);
        const disability = buildDisability(apiJson);
        const levels = buildLevels(apiJson);

        const data = {
            years: buildYears(apiJson),
            grades: buildGrades(academic, low, failed),
            academic,
            low,
            failed,
            disability
        };
        if (levels) data.levels = levels;

        return {
            DATA: data,
            summary: buildSummary(apiJson)
            // SEC_DATA / STAGES / SUPPORT_STAGES / kpi intentionally
            // omitted — no backend source for those yet, so the report
            // keeps its own zeroed defaults.
        };
    }

    /* ════════════════════════════════════════════════════════════════
       MODAL + FETCH — same pattern as the old accordion implementation
       (jqClient + bootstrap modal), but the body is now an <iframe>
       hosting the rich Chart.js report instead of server-rendered HTML.
    ════════════════════════════════════════════════════════════════ */

    const modalId = "outputsAnalysisModal";
    const frameId = "outputsAnalysisFrame";

    function getText(key, fallback) {
        try {
            return uiControlsSetup().GetUiControlText(key) || fallback;
        } catch {
            return fallback;
        }
    }

    function ensureModal() {
        if ($("#" + modalId).length) return;

        $("body").append(`
            <div class="modal fade" id="${modalId}" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog modal-fullscreen">
                    <div class="modal-content">
                        <div class="modal-header bg-primary text-white">
                            <h5 class="modal-title">
                                <i class="las la-chart-line"></i>
                                ${getText("lblOutputsAnalysis", "Outputs Analysis")}
                            </h5>
                            <button type="button"
                                    class="btn-close btn-close-white"
                                    data-bs-dismiss="modal"
                                    aria-label="Close"></button>
                        </div>

                        <div class="modal-body p-0" style="position:relative;">
                            <div id="${frameId}-loading"
                                 class="position-absolute top-50 start-50 translate-middle text-center">
                                <div class="spinner-border text-primary"></div>
                                <div class="mt-2">Loading...</div>
                            </div>
                            <iframe id="${frameId}"
                                    style="width:100%;height:100%;border:0;"></iframe>
                        </div>
                    </div>
                </div>
            </div>
        `);
    }

    function showLoading(show) {
        $("#" + frameId + "-loading").toggle(!!show);
    }

    window.openOutputsAnalysis = function (evaluationRequestId) {
        if (!evaluationRequestId) {
            notificationUtil?.error?.("Missing evaluation request id");
            return;
        }

        ensureModal();
        showLoading(true);
        $("#" + modalId).modal("show");

        const frame = document.getElementById(frameId);

        // Force a clean reload of the report each time the modal opens,
        // so chart instances from a previous open don't linger.
        frame.src = REPORT_URL + "?t=" + Date.now();

        const options = {
            success: function (apiJson) {
                const payload = buildReportPayload(apiJson);

                const postToFrame = function () {
                    frame.contentWindow.postMessage({ type: "OUTPUT_ANALYSIS_DATA", payload }, "*");
                    showLoading(false);
                };

                // The report posts an OUTPUT_ANALYSIS_READY message once
                // its own listener is attached. We send the data on
                // that signal, and also on the iframe's load event as a
                // fallback in case the message races past us.
                const onReady = function (ev) {
                    if (ev.source === frame.contentWindow && ev.data && ev.data.type === "OUTPUT_ANALYSIS_READY") {
                        window.removeEventListener("message", onReady);
                        postToFrame();
                    }
                };
                window.addEventListener("message", onReady);
                frame.addEventListener("load", function onLoad() {
                    frame.removeEventListener("load", onLoad);
                    postToFrame();
                }, { once: true });
            },
            error: function () {
                showLoading(false);
                notificationUtil?.error?.("حدث خطأ أثناء تحميل تحليل المخرجات");
            }
        };

        jqClient(options).Get(
            `/ServiceRequest/${departmentRoutePath}/GetOutputAnalysis?evaluationRequestId=${evaluationRequestId}`
        );
    };

})();
