(function () {
    "use strict";

    const modalId = "outputsAnalysisModal";
    const bodyId = "outputsAnalysisBody";

    let apiResponse = null;
    let DATA = null;
    let _charts = {};
    let currentTab = "summary";

    const TRACK_LABEL_MAP = {
        "Science - Religious": "علمي شرعي",
        "Humanities - Religious": "أدبي شرعي"
    };

    function getText(key, fallback) {
        try {
            return uiControlsSetup().GetUiControlText(key) || fallback;
        } catch {
            return fallback;
        }
    }

    function esc(v) {
        return (v ?? "").toString()
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");
    }

    function f2(v) {
        const n = Number(v || 0);
        return Number.isNaN(n) ? "0.00" : n.toFixed(2);
    }

    function gradeToLevel(grade) {
        const g = Number(grade);
        if (g <= 6) return "primary";
        if (g <= 9) return "middle";
        return "secondary";
    }

    function normalizeLevelName(backendName) {
        if (!backendName) return null;

        const v = backendName.toString().toLowerCase();

        if (v.includes("primary")) return "primary";
        if (v.includes("middle") || v.includes("preparatory")) return "middle";
        if (v.includes("secondary")) return "secondary";

        return backendName;
    }

    function lvlName(level) {
        if (level === "primary") return "الابتدائية";
        if (level === "middle") return "الإعدادية";
        if (level === "secondary") return "الثانوية";
        return "الكل";
    }

    function trackLabel(track) {
        if (!track) return "";
        return TRACK_LABEL_MAP[track] || track;
    }

    function getRowLevel(r) {
        return normalizeLevelName(r.educationLevelBackendName)
            || normalizeLevelName(r.educationLevelNameEn)
            || gradeToLevel(r.grade);
    }

    function getRowLevelName(r) {
        return r.educationLevelNameAr
            || r.educationLevelNameEn
            || lvlName(getRowLevel(r));
    }

    function getRowGradeName(r) {
        return r.gradeNameAr
            || r.gradeNameEn
            || r.grade;
    }

    function getRowSubjectName(r) {
        if (r.track) return trackLabel(r.track);

        return r.subjectNameAr
            || r.subjectNameEn
            || r.subjectCode
            || "—";
    }

    function getMatrixName(r) {
        return r.matrixValue?.nameAr
            || r.matrixValue?.nameEn
            || r.martixTextValue
            || r.note
            || "";
    }

    function getMatrixReportText(r) {
        return r.matrixValue?.reportTextAr
            || r.matrixValue?.reportTextEn
            || "";
    }

    function judgeClass(score) {
        score = Number(score || 0);
        if (score >= 90) return "kb-green";
        if (score >= 80) return "kb-green";
        if (score >= 70) return "kb-blue";
        if (score >= 50) return "kb-amber";
        return "kb-red";
    }

    function judgeLabel(score) {
        score = Number(score || 0);
        if (score >= 90) return "ممتاز";
        if (score >= 80) return "جيد جداً";
        if (score >= 70) return "جيد";
        if (score >= 50) return "مقبول";
        return "ضعيف";
    }

    function diffClass(v) {
        const n = Number(v || 0);
        if (n > 0) return "pos";
        if (n < 0) return "neg";
        return "neu";
    }

    function findType(backendName) {
        return (apiResponse?.analysisTypes || [])
            .find(x => x.backendName === backendName) || null;
    }

    function finalScore(backendName) {
        const t = findType(backendName);
        return Number(t?.outputAnalysisFinalResult?.actualValue || 0);
    }

    function finalMatrixName(backendName) {
        const t = findType(backendName);
        const final = t?.outputAnalysisFinalResult;

        return final?.matrixValue?.nameAr
            || final?.matrixValue?.nameEn
            || final?.note
            || "";
    }

    function finalReportText(backendName) {
        const t = findType(backendName);
        const final = t?.outputAnalysisFinalResult;

        return final?.matrixValue?.reportTextAr
            || final?.matrixValue?.reportTextEn
            || "";
    }

    function groupBy(rows, keyFn) {
        const map = new Map();

        rows.forEach(r => {
            const key = keyFn(r);

            if (!map.has(key)) {
                map.set(key, []);
            }

            map.get(key).push(r);
        });

        return map;
    }

    function pickPreferredRow(rows) {
        const order = ["S2", "S1", "1S"];

        for (const term of order) {
            const found = rows.find(x => x.termCode === term);

            if (found) {
                return found;
            }
        }

        return rows[0];
    }

    function buildAcademic() {
        const t = findType("AcademicAchievement");

        if (!t) return [];

        return (t.outputAnalysisData || []).map(r => ({
            level: getRowLevel(r),
            levelName: getRowLevelName(r),

            grade: getRowGradeName(r),
            gradeNo: Number(r.grade || 0),

            subject: getRowSubjectName(r),
            subjectCode: r.subjectCode,

            track: r.track,
            termCode: r.termCode,

            students1: Number(r.previousYearStudentCount || 0),
            students2: Number(r.lastYearStudentCount || 0),

            previousYear: r.previousYear,
            lastYear: r.lastYear,

            prev: Number(r.previousYearValue || 0),
            last: Number(r.lastYearValue || 0),
            diff: Number(r.difference || 0),
            actual: Number(r.actualValue || 0),

            judge: getMatrixName(r),
            reportText: getMatrixReportText(r),
            matrixValue: r.matrixValue || null,

            raw: r
        }));
    }

    function buildLow() {
        const t = findType("LowPerformanceStudents");

        if (!t) return [];

        const rows = t.outputAnalysisData || [];

        const grouped = groupBy(rows, r =>
            `${r.grade}__${r.subjectCode || ""}__${r.track || ""}`
        );

        const out = [];

        grouped.forEach(group => {
            const r = pickPreferredRow(group);

            out.push({
                level: getRowLevel(r),
                levelName: getRowLevelName(r),

                grade: getRowGradeName(r),
                gradeNo: Number(r.grade || 0),

                subject: getRowSubjectName(r),

                count: Number(r.lastYearStudentCount || 0),
                pct: Number(r.lastYearValue || r.actualValue || 0),

                termCode: r.termCode,

                judge: getMatrixName(r),
                reportText: getMatrixReportText(r),
                matrixValue: r.matrixValue || null,

                raw: r
            });
        });

        return out;
    }

    function buildFailed() {
        const t = findType("FailedStudents");

        if (!t) return [];

        return (t.outputAnalysisData || []).map(r => ({
            level: getRowLevel(r),
            levelName: getRowLevelName(r),

            grade: getRowGradeName(r),
            gradeNo: Number(r.grade || 0),

            prev: Number(r.previousYearValue || 0),
            last: Number(r.lastYearValue || 0),
            diff: Number(r.difference || 0),

            judge: getMatrixName(r),
            reportText: getMatrixReportText(r),
            matrixValue: r.matrixValue || null,

            raw: r
        }));
    }

    function buildDisability() {
        const t = findType("StudentsWithDisabilities");

        const skeleton = [
            { level: "primary", levelName: "الابتدائية", students: 0, score: 0, sub: [] },
            { level: "middle", levelName: "الإعدادية", students: 0, score: 0, sub: [] },
            { level: "secondary", levelName: "الثانوية", students: 0, score: 0, sub: [] }
        ];

        if (!t || !t.outputAnalysisData?.length) {
            return skeleton;
        }

        const byLevel = groupBy(t.outputAnalysisData, r => getRowLevel(r));

        return skeleton.map(s => {
            const rows = byLevel.get(s.level) || [];

            if (!rows.length) {
                return s;
            }

            const students = rows.reduce(
                (a, r) => a + Number(r.lastYearStudentCount || 0),
                0
            );

            const score = rows.reduce(
                (a, r) => a + Number(r.actualValue || 0),
                0
            ) / rows.length;

            return {
                level: s.level,
                levelName: s.levelName,
                students,
                score,
                sub: rows.map(r => Number(r.actualValue || 0)),
                judge: rows[0]?.matrixValue?.nameAr || rows[0]?.matrixValue?.nameEn || rows[0]?.note || "",
                reportText: rows[0]?.matrixValue?.reportTextAr || rows[0]?.matrixValue?.reportTextEn || ""
            };
        });
    }

    function buildYears() {
        for (const t of apiResponse?.analysisTypes || []) {
            const r = (t.outputAnalysisData || [])[0];

            if (r?.lastYear && r?.previousYear) {
                return [
                    `${r.previousYear - 1}-${r.previousYear}`,
                    `${r.lastYear - 1}-${r.lastYear}`
                ];
            }
        }

        return ["", ""];
    }

    function buildData() {
        const academic = buildAcademic();
        const low = buildLow();
        const failed = buildFailed();
        const disability = buildDisability();

        const grades = {
            primary: [],
            middle: [],
            secondary: []
        };

        [...academic, ...low, ...failed].forEach(r => {
            if (grades[r.level] && !grades[r.level].includes(r.grade)) {
                grades[r.level].push(r.grade);
            }
        });

        return {
            years: buildYears(),

            academic,
            low,
            failed,
            disability,

            grades,

            summary: {
                academic: finalScore("AcademicAchievement"),
                academicJudge: finalMatrixName("AcademicAchievement"),
                academicReportText: finalReportText("AcademicAchievement"),

                low: finalScore("LowPerformanceStudents"),
                lowJudge: finalMatrixName("LowPerformanceStudents"),
                lowReportText: finalReportText("LowPerformanceStudents"),

                failed: finalScore("FailedStudents"),
                failedJudge: finalMatrixName("FailedStudents"),
                failedReportText: finalReportText("FailedStudents"),

                disability: finalScore("StudentsWithDisabilities"),
                disabilityJudge: finalMatrixName("StudentsWithDisabilities"),
                disabilityReportText: finalReportText("StudentsWithDisabilities")
            }
        };
    }

    function ensureChartJs(callback) {
        if (window.Chart) {
            callback();
            return;
        }

        const existing = document.querySelector("script[data-chartjs='1']");

        if (existing) {
            existing.addEventListener("load", callback, { once: true });
            return;
        }

        const script = document.createElement("script");
        script.src = "https://cdnjs.cloudflare.com/ajax/libs/Chart.js/4.4.1/chart.umd.js";
        script.setAttribute("data-chartjs", "1");
        script.onload = callback;
        document.head.appendChild(script);
    }

    function destroyCharts() {
        Object.keys(_charts).forEach(k => {
            try {
                _charts[k].destroy();
            } catch { }
        });

        _charts = {};
    }

    function makeChart(id, config) {
        if (!window.Chart) return;

        const el = document.getElementById(id);

        if (!el) return;

        if (_charts[id]) {
            _charts[id].destroy();
            delete _charts[id];
        }

        _charts[id] = new Chart(el, config);
    }

    function injectStyles() {
        if (document.getElementById("output-analysis-style")) return;

        $("head").append(`
<style id="output-analysis-style">
#${modalId} .modal-body{background:#F4F4F6;padding:0!important}
.oa-wrap{font-family:'IBM Plex Sans Arabic',Tahoma,Arial,sans-serif;background:#F4F4F6;color:#111118;min-height:100%;direction:rtl}
.oa-topbar{height:64px;background:linear-gradient(135deg,#6B0F2A 0%,#8B1538 100%);color:#fff;display:flex;align-items:center;justify-content:space-between;padding:0 28px;box-shadow:0 2px 12px rgba(139,21,56,.35)}
.oa-title{display:flex;align-items:center;gap:14px}
.oa-logo{width:34px;height:34px;background:rgba(255,255,255,.15);border-radius:8px;display:flex;align-items:center;justify-content:center}
.oa-title h1{font-size:16px;font-weight:700;margin:0}
.oa-title p{font-size:11px;color:rgba(255,255,255,.65);margin:1px 0 0}
.oa-print{background:rgba(255,255,255,.12);color:#fff;border:1px solid rgba(255,255,255,.22);border-radius:8px;padding:7px 13px;cursor:pointer}
.oa-container{max-width:1180px;margin:0 auto;padding:22px 22px 60px}
.oa-hero{background:#fff;border:1px solid #E4E4EA;border-radius:12px;padding:22px 26px;margin-bottom:18px;display:grid;grid-template-columns:1fr auto;gap:20px;align-items:center;box-shadow:0 1px 3px rgba(0,0,0,.06),0 4px 16px rgba(0,0,0,.05);border-top:3px solid #8B1538}
.oa-eyebrow{font-size:11px;font-weight:700;color:#8B1538;text-transform:uppercase;letter-spacing:.08em;margin-bottom:5px}
.oa-hero h2{font-size:24px;font-weight:700;margin:0}
.oa-sub{font-size:13px;color:#5A5A72;margin-top:5px}
.oa-school{background:#F8F8FA;border:1px solid #E4E4EA;border-radius:8px;padding:13px 16px}
.oa-school-label{font-size:11px;color:#9A9AB0}
.oa-school-name{font-size:15px;font-weight:700}
.oa-filters{background:#fff;border:1px solid #E4E4EA;border-radius:12px;padding:14px 20px;margin-bottom:18px;display:flex;gap:18px;align-items:flex-end;flex-wrap:wrap;box-shadow:0 1px 3px rgba(0,0,0,.06),0 4px 16px rgba(0,0,0,.05)}
.oa-field{display:flex;flex-direction:column;gap:5px}
.oa-field label{font-size:12px;color:#5A5A72;font-weight:600}
.oa-field select{border:1px solid #E4E4EA;border-radius:8px;padding:8px 12px;background:#fff;font-size:13px;color:#111118;min-width:200px}
.oa-kpis{display:grid;grid-template-columns:repeat(4,1fr);gap:12px;margin-bottom:18px}
.oa-kpi{background:#fff;border:1px solid #E4E4EA;border-radius:12px;padding:16px 18px;box-shadow:0 1px 3px rgba(0,0,0,.06),0 4px 16px rgba(0,0,0,.05);position:relative;overflow:hidden}
.oa-kpi:before{content:'';position:absolute;top:0;right:0;width:4px;height:100%}
.oa-kpi.green:before{background:#0D7A4E}.oa-kpi.red:before{background:#9A1E1E}.oa-kpi.amber:before{background:#EF9F27}.oa-kpi.blue:before{background:#1A5FA0}
.oa-kpi-label{font-size:11px;color:#5A5A72;margin-bottom:6px;font-weight:600}
.oa-kpi-value{font-size:30px;font-weight:800;line-height:1;color:#111118}
.oa-kpi-sub{font-size:11px;color:#9A9AB0;margin-top:5px}
.oa-kpi-report{font-size:11px;color:#5A5A72;margin-top:5px}
.oa-tabs-shell{background:#fff;border:1px solid #E4E4EA;border-radius:12px;box-shadow:0 1px 3px rgba(0,0,0,.06),0 4px 16px rgba(0,0,0,.05);overflow:hidden}
.oa-tabs{display:flex;overflow-x:auto;border-bottom:1px solid #E4E4EA}
.oa-tab{flex-shrink:0;border:none;background:transparent;padding:14px 18px;cursor:pointer;color:#5A5A72;font-size:13px;font-weight:700;white-space:nowrap;border-bottom:3px solid transparent}
.oa-tab.active{color:#8B1538;border-bottom-color:#8B1538;background:#FFF7F9}
.oa-panel{display:none;padding:22px}.oa-panel.active{display:block}
.oa-sec-head{display:flex;justify-content:space-between;align-items:center;margin-bottom:16px;padding-bottom:10px;border-bottom:1px solid #E4E4EA}
.oa-sec-head h3{font-size:18px;font-weight:800;margin:0}
.oa-sec-sub{font-size:12px;color:#9A9AB0}
.oa-card{background:#fff;border:1px solid #E4E4EA;border-radius:12px;padding:18px;box-shadow:0 1px 3px rgba(0,0,0,.06),0 4px 16px rgba(0,0,0,.05)}
.oa-grid-2{display:grid;grid-template-columns:1fr 1fr;gap:14px}
.oa-grid-3{display:grid;grid-template-columns:repeat(3,1fr);gap:14px}
.oa-std-grid{display:grid;grid-template-columns:repeat(4,1fr);gap:12px}
.oa-std-card{background:#fff;border:1px solid #E4E4EA;border-radius:12px;padding:16px;box-shadow:0 1px 3px rgba(0,0,0,.06),0 4px 16px rgba(0,0,0,.05)}
.oa-std-num{font-size:10px;font-weight:800;color:#9A9AB0;text-transform:uppercase}
.oa-std-name{font-size:13px;font-weight:700;line-height:1.4;margin-top:4px}
.oa-bar{height:6px;border-radius:3px;background:#F8F8FA;overflow:hidden;margin:10px 0}
.oa-fill{height:100%;border-radius:3px}
.oa-std-footer{display:flex;justify-content:space-between;align-items:center}
.oa-std-pct{font-size:22px;font-weight:800}
.oa-badge{display:inline-block;font-size:11px;font-weight:700;padding:3px 9px;border-radius:4px}
.kb-red{background:#FDEAEA;color:#9A1E1E}.kb-green{background:#E5F5EE;color:#0D7A4E}.kb-amber{background:#FEF3E2;color:#8A5000}.kb-blue{background:#E8F1FB;color:#1A5FA0}.kb-gray{background:#F8F8FA;color:#5A5A72}
.oa-table-wrap{overflow-x:auto}
.oa-table{width:100%;border-collapse:collapse;font-size:13px}
.oa-table th{background:#F8F8FA;color:#5A5A72;font-weight:800;font-size:11px;padding:10px 12px;text-align:center;border-bottom:1px solid #E4E4EA;white-space:nowrap}
.oa-table td{padding:11px 12px;text-align:center;border-bottom:1px solid #E4E4EA}
.oa-table td.tl,.oa-table th.tl{text-align:right;font-weight:700}
.pos{color:#0D7A4E;font-weight:800}.neg{color:#9A1E1E;font-weight:800}.neu{color:#9A9AB0;font-weight:800}
.oa-stage{background:#FBE9EF;color:#6B0F2A;font-weight:800;font-size:13px;border-radius:8px;padding:9px 14px;margin:16px 0 10px;border-right:3px solid #8B1538}
.oa-chart{position:relative;height:260px}
.oa-empty{background:#FEF3E2;color:#8A5000;border:1px solid rgba(138,80,0,.2);border-radius:8px;padding:14px}
.oa-small{font-size:11px;color:#9A9AB0;margin-top:3px;font-weight:400}
@media(max-width:900px){.oa-kpis,.oa-grid-2,.oa-grid-3,.oa-std-grid{grid-template-columns:1fr 1fr}.oa-hero{grid-template-columns:1fr}}
@media(max-width:560px){.oa-kpis,.oa-grid-2,.oa-grid-3,.oa-std-grid{grid-template-columns:1fr}.oa-container{padding:14px 12px 50px}}
</style>`);
    }

    function ensureModal() {
        injectStyles();

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
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body" id="${bodyId}"></div>
        </div>
    </div>
</div>`);
    }

    function renderLoading() {
        return `
<div class="text-center py-5">
    <div class="spinner-border text-primary"></div>
    <div class="mt-2">Loading...</div>
</div>`;
    }

    function getFilteredRows(rows) {
        const level = $("#oaLevelFilter").val() || "all";
        const grade = $("#oaGradeFilter").val() || "all";

        return rows.filter(r =>
            (level === "all" || r.level === level) &&
            (grade === "all" || String(r.grade) === String(grade))
        );
    }

    function renderLayout() {
        const years = DATA.years || ["", ""];

        $("#" + bodyId).html(`
<div class="oa-wrap">
    <div class="oa-topbar">
        <div class="oa-title">
            <div class="oa-logo">🏫</div>
            <div>
                <h1>تحليل المخرجات المدرسية</h1>
                <p>الرئيسية › التقييم الدوري › المدرسة › تحليل المخرجات</p>
            </div>
        </div>
        <button class="oa-print" type="button" onclick="window.print()">🖨 طباعة</button>
    </div>

    <div class="oa-container">
        <div class="oa-hero">
            <div>
                <div class="oa-eyebrow">فريق التقييم · تقرير المخرجات النهائية</div>
                <h2>تحليل أداء المدرسة للعامين الأكاديميين <span>${esc(years[0])} & ${esc(years[1])}</span></h2>
                <div class="oa-sub">يشمل التقرير المخرجات الأكاديمية على مستوى المراحل الدراسية</div>
            </div>
            <div class="oa-school">
                <div class="oa-school-label">المدرسة الحالية</div>
                <div class="oa-school-name">-</div>
            </div>
        </div>

        <div class="oa-filters">
            <div class="oa-field">
                <label>المرحلة التعليمية</label>
                <select id="oaLevelFilter">
                    <option value="all">جميع المراحل</option>
                    <option value="primary">المرحلة الابتدائية</option>
                    <option value="middle">المرحلة الإعدادية</option>
                    <option value="secondary">المرحلة الثانوية</option>
                </select>
            </div>

            <div class="oa-field">
                <label>الصف الدراسي</label>
                <select id="oaGradeFilter">
                    <option value="all">جميع الصفوف</option>
                </select>
            </div>

            <div class="oa-field">
                <label>نوع التحليل</label>
                <select id="oaAnalysisTypeFilter">
                    <option value="all">كل التحليلات</option>
                    ${(apiResponse.analysisTypes || []).map(x => `
                        <option value="${esc(x.backendName)}">
                            ${esc(x.analysisTypeNameAr || x.analysisTypeNameEn)}
                        </option>
                    `).join("")}
                </select>
            </div>
        </div>

        <div id="oaKpis"></div>

        <div class="oa-tabs-shell">
            <div class="oa-tabs">
                <button class="oa-tab active" data-tab="summary">▣ الملخص التنفيذي</button>
                <button class="oa-tab" data-tab="academic">🎓 التحصيل الأكاديمي</button>
                <button class="oa-tab" data-tab="low">⚠ الأداء المتدني</button>
                <button class="oa-tab" data-tab="failed">✖ الراسبون</button>
                <button class="oa-tab" data-tab="disability">♿ ذوو الإعاقة</button>
                <button class="oa-tab" data-tab="raw">📋 كل البيانات</button>
            </div>

            <div id="summary" class="oa-panel active"></div>
            <div id="academic" class="oa-panel"></div>
            <div id="low" class="oa-panel"></div>
            <div id="failed" class="oa-panel"></div>
            <div id="disability" class="oa-panel"></div>
            <div id="raw" class="oa-panel"></div>
        </div>
    </div>
</div>`);

        bindEvents();
        fillGradeFilter();
        renderAll();
    }

    function bindEvents() {
        $(".oa-tab").off("click").on("click", function () {
            currentTab = $(this).data("tab");

            $(".oa-tab").removeClass("active");
            $(this).addClass("active");

            $(".oa-panel").removeClass("active");
            $("#" + currentTab).addClass("active");

            renderAll();
        });

        $("#oaLevelFilter").off("change").on("change", function () {
            fillGradeFilter();
            renderAll();
        });

        $("#oaGradeFilter,#oaAnalysisTypeFilter")
            .off("change")
            .on("change", renderAll);
    }

    function fillGradeFilter() {
        const level = $("#oaLevelFilter").val() || "all";
        const grades = new Set();

        const rows = [
            ...DATA.academic,
            ...DATA.low,
            ...DATA.failed
        ];

        rows.forEach(r => {
            if (level === "all" || r.level === level) {
                grades.add(r.grade);
            }
        });

        $("#oaGradeFilter").html(`
            <option value="all">جميع الصفوف</option>
            ${Array.from(grades)
                .sort((a, b) => String(a).localeCompare(String(b), "ar", { numeric: true }))
                .map(g => `
                    <option value="${esc(g)}">الصف ${esc(g)}</option>
                `).join("")}
        `);
    }

    function renderKpis() {
        $("#oaKpis").html(`
<div class="oa-kpis">
    <div class="oa-kpi green">
        <div class="oa-kpi-label">التحصيل الأكاديمي</div>
        <div class="oa-kpi-value">${f2(DATA.summary.academic)}%</div>
        <div class="oa-kpi-sub">${esc(DATA.summary.academicJudge || judgeLabel(DATA.summary.academic))}</div>
        ${DATA.summary.academicReportText ? `<div class="oa-kpi-report">${esc(DATA.summary.academicReportText)}</div>` : ""}
    </div>

    <div class="oa-kpi amber">
        <div class="oa-kpi-label">ذوو الأداء المتدني</div>
        <div class="oa-kpi-value">${f2(DATA.summary.low)}%</div>
        <div class="oa-kpi-sub">${esc(DATA.summary.lowJudge || judgeLabel(DATA.summary.low))}</div>
        ${DATA.summary.lowReportText ? `<div class="oa-kpi-report">${esc(DATA.summary.lowReportText)}</div>` : ""}
    </div>

    <div class="oa-kpi red">
        <div class="oa-kpi-label">الراسبون</div>
        <div class="oa-kpi-value">${f2(DATA.summary.failed)}%</div>
        <div class="oa-kpi-sub">${esc(DATA.summary.failedJudge || judgeLabel(DATA.summary.failed))}</div>
        ${DATA.summary.failedReportText ? `<div class="oa-kpi-report">${esc(DATA.summary.failedReportText)}</div>` : ""}
    </div>

    <div class="oa-kpi blue">
        <div class="oa-kpi-label">ذوو الإعاقة</div>
        <div class="oa-kpi-value">${f2(DATA.summary.disability)}%</div>
        <div class="oa-kpi-sub">${esc(DATA.summary.disabilityJudge || judgeLabel(DATA.summary.disability))}</div>
        ${DATA.summary.disabilityReportText ? `<div class="oa-kpi-report">${esc(DATA.summary.disabilityReportText)}</div>` : ""}
    </div>
</div>`);
    }

    function getAnalysisDisplayName(type) {
        const items = type.formItems || [];

        if (items.length) {
            return items
                .map(x => {
                    const no = x.itemNumber ? `${x.itemNumber} - ` : "";
                    return `${no}${x.nameAr || x.nameEn || ""}`;
                })
                .join("<br>");
        }

        return esc(type.analysisTypeNameAr || type.analysisTypeNameEn || type.backendName || "-");
    }

    function getFinalActualValue(type) {
        return Number(type?.outputAnalysisFinalResult?.actualValue || 0);
    }

    function getFinalMatrixName(type) {
        const final = type?.outputAnalysisFinalResult;

        return final?.matrixValue?.nameAr
            || final?.matrixValue?.nameEn
            || final?.note
            || judgeLabel(getFinalActualValue(type));
    }

    function getFinalReportText(type) {
        const final = type?.outputAnalysisFinalResult;

        return final?.matrixValue?.reportTextAr
            || final?.matrixValue?.reportTextEn
            || "";
    }

    function getAnalysisColor(type, index) {
        const backend = type.backendName || "";

        if (backend === "AcademicAchievement") return "#8B1538";
        if (backend === "LowPerformanceStudents") return "#8A5000";
        if (backend === "FailedStudents") return "#9A1E1E";
        if (backend === "StudentsWithDisabilities") return "#4E35A0";

        const colors = ["#8B1538", "#1A5FA0", "#0D7A4E", "#8A5000", "#4E35A0", "#0A6B78"];
        return colors[index % colors.length];
    }

    function renderSummary() {
        const analysisTypes = apiResponse?.analysisTypes || [];

        if (!analysisTypes.length) {
            $("#summary").html(`
            <div class="oa-sec-head">
                <h3>الملخص التنفيذي</h3>
                <span class="oa-sec-sub">${esc(DATA.years[0])} / ${esc(DATA.years[1])}</span>
            </div>
            <div class="oa-empty">لا توجد بيانات تحليل مخرجات</div>
        `);
            return;
        }

        $("#summary").html(`
        <div class="oa-sec-head">
            <h3>الملخص التنفيذي</h3>
            <span class="oa-sec-sub">${esc(DATA.years[0])} / ${esc(DATA.years[1])}</span>
        </div>

        <div class="oa-std-grid">
            ${analysisTypes.map((type, index) => {
            const score = getFinalActualValue(type);
            const color = getAnalysisColor(type, index);
            const matrixName = getFinalMatrixName(type);
            const reportText = getFinalReportText(type);

            return `
                    <div class="oa-std-card">
                        <div class="oa-std-num">
                            ${esc(type.backendName || "-")}
                        </div>

                        <div class="oa-std-name">
                            ${getAnalysisDisplayName(type)}
                        </div>

                        <div class="oa-bar">
                            <div class="oa-fill"
                                 style="width:${Math.min(score, 100)}%;background:${color}">
                            </div>
                        </div>

                        <div class="oa-std-footer">
                            <div class="oa-std-pct" style="color:${color}">
                                ${f2(score)}%
                            </div>

                            <span class="oa-badge ${judgeClass(score)}">
                                ${esc(matrixName)}
                            </span>
                        </div>

                        ${reportText
                    ? `<div class="oa-small">${esc(reportText)}</div>`
                    : ""}
                    </div>
                `;
        }).join("")}
        </div>
    `);
    }
    function getAnalysisTypeByBackend(backendName) {
        return (apiResponse?.analysisTypes || [])
            .find(x => x.backendName === backendName) || null;
    }

    function getFormItemsText(backendName) {
        const type = getAnalysisTypeByBackend(backendName);
        const items = type?.formItems || [];

        if (!items.length) return "";

        return items
            .map(x => `${x.itemNumber ? x.itemNumber + " - " : ""}${x.nameAr || x.nameEn}`)
            .join("<br>");
    }
    function gradeAverage(rows) {
        if (!rows.length) return 0;

        return rows.reduce((a, r) => a + Number(r.last || 0), 0) / rows.length;
    }

    function renderAcademic() {
        const rows = getFilteredRows(DATA.academic);
        const grouped = groupBy(rows, r => `${r.level}__${r.grade}`);

        let html = `
<div class="oa-sec-head">
    <h3>معيار 3.1.4 — التحصيل الأكاديمي</h3>
    <span class="oa-sec-sub">حسب المرحلة والصف والمادة</span>
</div>`;

        if (!rows.length) {
            $("#academic").html(html + `<div class="oa-empty">لا توجد بيانات حسب الفلتر المحدد</div>`);
            return;
        }

        grouped.forEach(grp => {
            const r0 = grp[0];
            const avg = gradeAverage(grp);
            const lbl = (r0.levelName || lvlName(r0.level)) + " — " + r0.grade;

            html += `
<div class="oa-stage">📚 ${esc(lbl)}</div>

<div class="oa-table-wrap">
<table class="oa-table">
<thead>
<tr>
    <th class="tl">المادة / المسار</th>
    <th>${esc(DATA.years[0])}</th>
    <th>${esc(DATA.years[1])}</th>
    <th>الفرق</th>
    <th>الحكم</th>
</tr>
</thead>
<tbody>
${grp.map(r => `
<tr>
    <td class="tl">
        <div><strong>${esc(r.subject)}</strong></div>
        ${r.reportText ? `<div class="oa-small">${esc(r.reportText)}</div>` : ""}
    </td>
    <td>${f2(r.prev)}</td>
    <td>${f2(r.last)}</td>
    <td class="${diffClass(r.diff)}">${r.diff > 0 ? "+" : ""}${f2(r.diff)}</td>
    <td>
        <span class="oa-badge ${judgeClass(r.last)}">
            ${esc(r.judge || judgeLabel(r.last))}
        </span>
    </td>
</tr>`).join("")}
<tr>
    <td class="tl"><strong>Average</strong></td>
    <td colspan="3"><strong>${f2(avg)}%</strong></td>
    <td>
        <span class="oa-badge ${judgeClass(avg)}">
            ${judgeLabel(avg)}
        </span>
    </td>
</tr>
</tbody>
</table>
</div>`;
        });

        html += `
<div class="oa-grid-2" style="margin-top:18px">
    <div class="oa-card">
        <h4>متوسط التحصيل حسب الصف</h4>
        <div class="oa-chart">
            <canvas id="academicChart"></canvas>
        </div>
    </div>
    <div class="oa-card">
        <h4>أفضل / أقل مواد</h4>
        <div id="academicTopBottom"></div>
    </div>
</div>`;

        $("#academic").html(html);

        renderAcademicChart(rows);
        renderAcademicTopBottom(rows);
    }

    function renderAcademicChart(rows) {
        const grouped = groupBy(rows, r => r.grade);

        const labels = [];
        const values = [];

        grouped.forEach((grp, key) => {
            labels.push("صف " + key);
            values.push(gradeAverage(grp));
        });

        makeChart("academicChart", {
            type: "bar",
            data: {
                labels,
                datasets: [{
                    label: "متوسط التحصيل",
                    data: values,
                    backgroundColor: "rgba(139,21,56,.75)",
                    borderRadius: 5,
                    borderSkipped: false
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        ticks: {
                            callback: v => v + "%"
                        },
                        min: 0,
                        max: 100
                    }
                }
            }
        });
    }

    function renderAcademicTopBottom(rows) {
        const sorted = [...rows].sort((a, b) => b.last - a.last);

        const top = sorted.slice(0, 5);
        const bottom = sorted.slice(-5).reverse();

        $("#academicTopBottom").html(`
<div class="oa-grid-2">
    <div>
        <h6>الأعلى</h6>
        ${top.map(r => `
            <div class="oa-badge kb-green" style="display:block;margin-bottom:6px">
                ${esc(r.subject)} - ${f2(r.last)}%
            </div>
        `).join("")}
    </div>

    <div>
        <h6>الأقل</h6>
        ${bottom.map(r => `
            <div class="oa-badge kb-red" style="display:block;margin-bottom:6px">
                ${esc(r.subject)} - ${f2(r.last)}%
            </div>
        `).join("")}
    </div>
</div>`);
    }

    function renderLow() {
        const rows = getFilteredRows(DATA.low);

        $("#low").html(`
<div class="oa-sec-head">
    <h3>معيار 3.2.2 — ذوو الأداء المتدني</h3>
    <span class="oa-sec-sub">الطلبة أقل من 70%</span>
</div>

<div class="oa-grid-2">
    <div class="oa-card">
        <div class="oa-table-wrap">
            <table class="oa-table">
                <thead>
                    <tr>
                        <th>المرحلة</th>
                        <th>الصف</th>
                        <th class="tl">المادة</th>
                        <th>عدد الطلاب</th>
                        <th>النسبة / الحكم</th>
                    </tr>
                </thead>
                <tbody>
                    ${rows.map(r => `
                        <tr>
                            <td>${esc(r.levelName || lvlName(r.level))}</td>
                            <td>${esc(r.grade)}</td>
                            <td class="tl">
                                <div><strong>${esc(r.subject)}</strong></div>
                                ${r.reportText ? `<div class="oa-small">${esc(r.reportText)}</div>` : ""}
                            </td>
                            <td>${r.count}</td>
                            <td>
                                <span class="oa-badge ${judgeClass(r.pct)}">
                                    ${esc(r.judge || judgeLabel(r.pct))}
                                </span>
                                <div class="${r.pct > 30 ? "neg" : r.pct > 10 ? "neu" : "pos"}">
                                    ${f2(r.pct)}%
                                </div>
                            </td>
                        </tr>
                    `).join("") || `<tr><td colspan="5">لا توجد بيانات</td></tr>`}
                </tbody>
            </table>
        </div>
    </div>

    <div class="oa-card">
        <div class="oa-chart">
            <canvas id="lowChart"></canvas>
        </div>
    </div>
</div>`);

        makeChart("lowChart", {
            type: "bar",
            data: {
                labels: rows.map(r => r.subject),
                datasets: [{
                    label: "نسبة الأداء المتدني",
                    data: rows.map(r => r.pct),
                    backgroundColor: rows.map(r =>
                        r.pct > 30
                            ? "rgba(154,30,30,.7)"
                            : r.pct > 10
                                ? "rgba(239,159,39,.7)"
                                : "rgba(13,122,78,.7)"
                    ),
                    borderRadius: 5,
                    borderSkipped: false
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        ticks: {
                            callback: v => v + "%"
                        },
                        min: 0,
                        max: 100
                    }
                }
            }
        });
    }

    function renderFailed() {
        const rows = getFilteredRows(DATA.failed);

        $("#failed").html(`
<div class="oa-sec-head">
    <h3>الطلاب الراسبون في الدور الأول</h3>
    <span class="oa-sec-sub">مقارنة العامين</span>
</div>

<div class="oa-grid-2">
    <div class="oa-card">
        <div class="oa-table-wrap">
            <table class="oa-table">
                <thead>
                    <tr>
                        <th>المرحلة</th>
                        <th>الصف</th>
                        <th>${esc(DATA.years[0])}</th>
                        <th>${esc(DATA.years[1])}</th>
                        <th>الفرق</th>
                        <th>الحكم</th>
                        <th>التقرير</th>
                    </tr>
                </thead>
                <tbody>
                    ${rows.map(r => {
            const rep = r.last < 10 ? "لا تكتب" : "تكتب";

            return `
                        <tr>
                            <td>${esc(r.levelName || lvlName(r.level))}</td>
                            <td>${esc(r.grade)}</td>
                            <td>${f2(r.prev)}%</td>
                            <td>${f2(r.last)}%</td>
                            <td class="${diffClass(r.diff)}">${r.diff > 0 ? "+" : ""}${f2(r.diff)}%</td>
                            <td>
                                <span class="oa-badge ${judgeClass(r.last)}">
                                    ${esc(r.judge || judgeLabel(r.last))}
                                </span>
                            </td>
                            <td>
                                <span class="oa-badge ${rep === "تكتب" ? "kb-red" : "kb-gray"}">
                                    ${rep}
                                </span>
                                ${r.reportText ? `<div class="oa-small">${esc(r.reportText)}</div>` : ""}
                            </td>
                        </tr>`;
        }).join("") || `<tr><td colspan="7">لا توجد بيانات</td></tr>`}
                </tbody>
            </table>
        </div>
    </div>

    <div class="oa-card">
        <div class="oa-chart">
            <canvas id="failedChart"></canvas>
        </div>
    </div>
</div>`);

        makeChart("failedChart", {
            type: "bar",
            data: {
                labels: rows.map(r => "صف " + r.grade),
                datasets: [
                    {
                        label: DATA.years[0],
                        data: rows.map(r => r.prev),
                        backgroundColor: "rgba(139,21,56,.7)",
                        borderRadius: 5
                    },
                    {
                        label: DATA.years[1],
                        data: rows.map(r => r.last),
                        backgroundColor: "rgba(154,30,30,.7)",
                        borderRadius: 5
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    y: {
                        ticks: {
                            callback: v => v + "%"
                        },
                        min: 0
                    }
                }
            }
        });
    }

    function renderDisability() {
        const rows = DATA.disability || [];

        $("#disability").html(`
<div class="oa-sec-head">
    <h3>معيار 3.2.4 — رعاية ذوي الإعاقة</h3>
    <span class="oa-sec-sub">حسب المرحلة</span>
</div>

<div class="oa-std-grid">
    ${rows.map(r => `
        <div class="oa-std-card">
            <div class="oa-std-num">${esc(r.levelName || lvlName(r.level))}</div>
            <div class="oa-std-name">عدد الطلاب: <strong>${r.students}</strong></div>

            <div class="oa-bar">
                <div class="oa-fill" style="width:${Math.min(r.score, 100)}%;background:#4E35A0"></div>
            </div>

            <div class="oa-std-footer">
                <div class="oa-std-pct" style="color:#4E35A0">${f2(r.score)}%</div>
                <span class="oa-badge ${judgeClass(r.score)}">
                    ${esc(r.judge || judgeLabel(r.score))}
                </span>
            </div>

            ${r.reportText ? `<div class="oa-small">${esc(r.reportText)}</div>` : ""}
        </div>
    `).join("")}
</div>`);
    }

    function renderRaw() {
        const selectedBackend = $("#oaAnalysisTypeFilter").val();

        const types = (apiResponse.analysisTypes || [])
            .filter(t => selectedBackend === "all" || t.backendName === selectedBackend);

        let rows = [];

        types.forEach(t => {
            rows.push(...(t.outputAnalysisData || []).map(r => ({
                type: t.analysisTypeNameAr || t.analysisTypeNameEn,
                ...r
            })));
        });

        $("#raw").html(`
<div class="oa-sec-head">
    <h3>كل البيانات</h3>
    <span class="oa-sec-sub">OutputAnalysisData</span>
</div>

<div class="oa-card">
<div class="oa-table-wrap">
<table class="oa-table">
<thead>
<tr>
    <th>Analysis</th>
    <th>Level</th>
    <th>Grade</th>
    <th>Subject</th>
    <th>Track</th>
    <th>Term</th>
    <th>Previous Year</th>
    <th>Last Year</th>
    <th>Previous Value</th>
    <th>Last Value</th>
    <th>Difference</th>
    <th>Actual</th>
    <th>Matrix</th>
    <th>Report Text</th>
    <th>Note</th>
</tr>
</thead>
<tbody>
${rows.map(r => `
<tr>
    <td>${esc(r.type)}</td>
    <td>${esc(r.educationLevelNameAr || r.educationLevelNameEn || "-")}</td>
    <td>${esc(r.gradeNameAr || r.gradeNameEn || r.grade)}</td>
    <td>${esc(r.subjectNameAr || r.subjectNameEn || r.subjectCode || "-")}</td>
    <td>${esc(r.track || "-")}</td>
    <td>${esc(r.termCode || "-")}</td>
    <td>${esc(r.previousYear)}</td>
    <td>${esc(r.lastYear)}</td>
    <td>${f2(r.previousYearValue)}</td>
    <td>${f2(r.lastYearValue)}</td>
    <td class="${diffClass(r.difference)}">${f2(r.difference)}</td>
    <td>${f2(r.actualValue)}</td>
    <td>${esc(r.matrixValue?.nameAr || r.matrixValue?.nameEn || r.martixTextValue || "-")}</td>
    <td>${esc(r.matrixValue?.reportTextAr || r.matrixValue?.reportTextEn || "-")}</td>
    <td>${esc(r.note || "-")}</td>
</tr>`).join("") || `<tr><td colspan="15">لا توجد بيانات</td></tr>`}
</tbody>
</table>
</div>
</div>`);
    }

    function renderAll() {
        destroyCharts();

        renderKpis();
        renderSummary();
        renderAcademic();
        renderLow();
        renderFailed();
        renderDisability();
        renderRaw();

        $(".oa-panel").removeClass("active");
        $("#" + currentTab).addClass("active");

        $(".oa-tab").removeClass("active");
        $(`.oa-tab[data-tab="${currentTab}"]`).addClass("active");
    }

    window.openOutputsAnalysis = function (evaluationRequestId) {
        if (!evaluationRequestId) {
            notificationUtil?.error?.("Missing evaluation request id");
            return;
        }

        ensureModal();

        $("#" + bodyId).html(renderLoading());
        $("#" + modalId).modal("show");

        const options = {
            success: function (response) {
                apiResponse = response || { analysisTypes: [] };
                DATA = buildData();

                ensureChartJs(function () {
                    renderLayout();
                });
            },
            error: function () {
                $("#" + bodyId).html(`
                    <div class="alert alert-danger m-3">
                        حدث خطأ أثناء تحميل تحليل المخرجات
                    </div>
                `);
            }
        };

        jqClient(options).Get(
            `/ServiceRequest/${departmentRoutePath}/GetOutputAnalysis?evaluationRequestId=${evaluationRequestId}`
        );
    };

})();