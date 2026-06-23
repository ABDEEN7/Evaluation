(function () {
    "use strict";

    const modalId = "outputsAnalysisModal";
    const bodyId = "outputsAnalysisBody";

    let apiResponse = null;
    let DATA = null;
    let _charts = {};
    let currentTab = null;

    const TRACK_LABEL_MAP = {
        "Science - Religious": "علمي شرعي",
        "Humanities - Religious": "أدبي شرعي"
    };

    const TYPE_COLORS = {
        AcademicAchievement: { primary: "#8B1538", light: "#FBE9EF", dark: "#6B0F2A" },
        StudentsWithDisabilities: { primary: "#4E35A0", light: "#EFECFB", dark: "#3A277A" },
        LowPerformanceStudents: { primary: "#8A5000", light: "#FEF3E2", dark: "#6A3D00" },
        FailedStudents: { primary: "#9A1E1E", light: "#FDEAEA", dark: "#7A1515" },
    };

    function getTypeColor(backendName) {
        return TYPE_COLORS[backendName] || { primary: "#1A5FA0", light: "#E8F1FB", dark: "#134880" };
    }

    // ─── Utilities ────────────────────────────────────────────────────────────
    function getText(key, fallback) {
        try { return uiControlsSetup().GetUiControlText(key) || fallback; }
        catch { return fallback; }
    }

    function esc(v) {
        return (v ?? "").toString()
            .replaceAll("&", "&amp;").replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;").replaceAll('"', "&quot;").replaceAll("'", "&#039;");
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

    function normalizeLevelName(name) {
        if (!name) return null;
        const v = name.toString().toLowerCase();
        if (v.includes("primary")) return "primary";
        if (v.includes("middle") || v.includes("preparatory")) return "middle";
        if (v.includes("secondary")) return "secondary";
        return name;
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
        return r.educationLevelNameAr || r.educationLevelNameEn || lvlName(getRowLevel(r));
    }

    function getRowGradeName(r) {
        return r.gradeNameAr || r.gradeNameEn || r.grade;
    }

    function getRowSubjectName(r) {
        if (r.track) return trackLabel(r.track);
        return r.subjectNameAr || r.subjectNameEn || r.subjectCode || "—";
    }

    function getMatrixName(r) {
        return r.matrixValue?.nameAr || r.matrixValue?.nameEn || r.martixTextValue || "";
    }

    // ReportText comes exclusively from the matrix value
    function getMatrixReportText(r) {
        return r.matrixValue?.reportTextAr || r.matrixValue?.reportTextEn || "";
    }

    function judgeClass(score) {
        score = Number(score || 0);
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
        if (n > 0) return "pos"; if (n < 0) return "neg"; return "neu";
    }

    function diffArrow(v) {
        const n = Number(v || 0);
        if (n > 0) return "▲"; if (n < 0) return "▼"; return "–";
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

    function getTypeId(type) {
        return type.analysisTypeId || type.id || type.backendName;
    }

    function safeId(str) {
        return (str || "").toString().replace(/[^a-zA-Z0-9_-]/g, "_");
    }

    // ─── Year helpers ─────────────────────────────────────────────────────────
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
        return ["السنة السابقة", "السنة الأخيرة"];
    }

    function buildData() { return { years: buildYears() }; }

    // ─── Chart.js loader ──────────────────────────────────────────────────────
    function ensureChartJs(callback) {
        if (window.Chart) { callback(); return; }
        const existing = document.querySelector("script[data-chartjs='1']");
        if (existing) { existing.addEventListener("load", callback, { once: true }); return; }
        const script = document.createElement("script");
        script.src = "https://cdnjs.cloudflare.com/ajax/libs/Chart.js/4.4.1/chart.umd.js";
        script.setAttribute("data-chartjs", "1");
        script.onload = callback;
        document.head.appendChild(script);
    }

    function destroyCharts() {
        Object.keys(_charts).forEach(k => { try { _charts[k].destroy(); } catch { } });
        _charts = {};
    }

    function makeChart(id, config) {
        if (!window.Chart) return;
        const el = document.getElementById(id);
        if (!el) return;
        if (_charts[id]) { _charts[id].destroy(); delete _charts[id]; }
        _charts[id] = new Chart(el, config);
    }

    // ─── Styles ───────────────────────────────────────────────────────────────
    function injectStyles() {
        if (document.getElementById("output-analysis-style")) return;
        $("head").append(`
<style id="output-analysis-style">
#${modalId} .modal-body{background:#F0F2F5;padding:0!important}
.oa-wrap{font-family:'IBM Plex Sans Arabic',Tahoma,Arial,sans-serif;background:#F0F2F5;color:#111118;min-height:100%;direction:rtl}

/* Topbar */
.oa-topbar{height:64px;background:linear-gradient(135deg,#6B0F2A 0%,#8B1538 100%);color:#fff;display:flex;align-items:center;justify-content:space-between;padding:0 28px;box-shadow:0 2px 12px rgba(139,21,56,.35)}
.oa-title{display:flex;align-items:center;gap:14px}
.oa-logo{width:34px;height:34px;background:rgba(255,255,255,.15);border-radius:8px;display:flex;align-items:center;justify-content:center;font-size:18px}
.oa-title h1{font-size:16px;font-weight:700;margin:0}
.oa-title p{font-size:11px;color:rgba(255,255,255,.65);margin:1px 0 0}
.oa-print{background:rgba(255,255,255,.12);color:#fff;border:1px solid rgba(255,255,255,.22);border-radius:8px;padding:7px 13px;cursor:pointer;font-size:13px}
.oa-container{width:100%;max-width:none;margin:0;padding:22px 28px 60px}

/* Hero */
.oa-hero{background:#fff;border:1px solid #E4E4EA;border-radius:12px;padding:22px 26px;margin-bottom:18px;display:grid;grid-template-columns:1fr auto;gap:20px;align-items:center;box-shadow:0 1px 3px rgba(0,0,0,.06),0 4px 16px rgba(0,0,0,.05);border-top:3px solid #8B1538}
.oa-eyebrow{font-size:11px;font-weight:700;color:#8B1538;text-transform:uppercase;letter-spacing:.08em;margin-bottom:5px}
.oa-hero h2{font-size:22px;font-weight:700;margin:0}
.oa-sub{font-size:13px;color:#5A5A72;margin-top:5px}
.oa-school{background:#F8F8FA;border:1px solid #E4E4EA;border-radius:8px;padding:13px 16px;min-width:180px}
.oa-school-label{font-size:11px;color:#9A9AB0}
.oa-school-name{font-size:15px;font-weight:700}

/* Filters */
.oa-filters{background:#fff;border:1px solid #E4E4EA;border-radius:12px;padding:14px 20px;margin-bottom:18px;display:flex;gap:18px;align-items:flex-end;flex-wrap:wrap;box-shadow:0 1px 3px rgba(0,0,0,.06)}
.oa-field{display:flex;flex-direction:column;gap:5px}
.oa-field label{font-size:12px;color:#5A5A72;font-weight:600}
.oa-field select{border:1px solid #D8D8E4;border-radius:8px;padding:8px 12px;background:#fff;font-size:13px;color:#111118;min-width:180px}

/* Tabs */
.oa-tabs-shell{background:#fff;border:1px solid #E4E4EA;border-radius:12px;box-shadow:0 1px 3px rgba(0,0,0,.06),0 4px 16px rgba(0,0,0,.05);overflow:hidden}
.oa-tabs{display:flex;overflow-x:auto;border-bottom:2px solid #F0F2F5;background:#FAFAFA}
.oa-tab{flex-shrink:0;border:none;background:transparent;padding:14px 20px;cursor:pointer;color:#5A5A72;font-size:13px;font-weight:700;white-space:nowrap;border-bottom:3px solid transparent;margin-bottom:-2px;transition:color .15s,border-color .15s}
.oa-tab.active{color:#8B1538;border-bottom-color:#8B1538;background:#fff}
.oa-panel{display:none;padding:24px}.oa-panel.active{display:block}

/* Section header */
.oa-sec-head{display:flex;justify-content:space-between;align-items:center;margin-bottom:20px;padding-bottom:12px;border-bottom:2px solid #F0F2F5}
.oa-sec-head h3{font-size:18px;font-weight:800;margin:0}
.oa-sec-sub{font-size:12px;color:#9A9AB0}

/* Level summary */
.oa-level-summary{display:grid;grid-template-columns:repeat(3,1fr);gap:10px;margin-bottom:20px}
.oa-level-card{border-radius:10px;padding:14px 16px;border:1px solid transparent}
.oa-level-card .lc-label{font-size:11px;font-weight:700;opacity:.7;margin-bottom:4px}
.oa-level-card .lc-val{font-size:24px;font-weight:800;line-height:1}
.oa-level-card .lc-sub{font-size:11px;margin-top:4px;font-weight:700;opacity:.75}
.oa-level-card.primary{background:#FBE9EF;border-color:#F0C0CE;color:#6B0F2A}
.oa-level-card.middle{background:#E8F1FB;border-color:#C0D5F0;color:#134880}
.oa-level-card.secondary{background:#E5F5EE;border-color:#B0DFC8;color:#0A5F3A}

/* ═══════════════════
   ACCORDION
   ═══════════════════ */
.oa-acc-group{margin-bottom:10px}

/* Track header (outer grouping) */
.oa-track-header{background:linear-gradient(135deg,#1A1A2E 0%,#2D2D4E 100%);color:#fff;border-radius:10px;padding:12px 18px;margin-bottom:8px;display:flex;align-items:center;gap:10px;font-size:14px;font-weight:800;letter-spacing:.02em}
.oa-track-header .track-icon{font-size:16px;opacity:.8}
.oa-track-badge{margin-right:auto;background:rgba(255,255,255,.15);border-radius:20px;padding:2px 10px;font-size:11px;font-weight:700}

/* Accordion item: Grade/Track combo */
.oa-acc-item{border:1px solid #E4E4EA;border-radius:10px;overflow:hidden;margin-bottom:6px;box-shadow:0 1px 3px rgba(0,0,0,.04)}
.oa-acc-trigger{width:100%;background:#fff;border:none;padding:12px 16px;cursor:pointer;display:flex;align-items:center;gap:12px;text-align:right;transition:background .15s}
.oa-acc-trigger:hover{background:#FAFAFA}
.oa-acc-trigger.open{background:#FAFAFA;border-bottom:1px solid #E4E4EA}
.oa-acc-arrow{font-size:11px;color:#9A9AB0;transition:transform .2s;flex-shrink:0;margin-right:auto}
.oa-acc-trigger.open .oa-acc-arrow{transform:rotate(180deg)}
.oa-acc-grade-label{font-size:14px;font-weight:800;color:#111118}
.oa-acc-meta{font-size:12px;color:#5A5A72;margin-top:1px}
.oa-acc-pills{display:flex;gap:6px;align-items:center;flex-wrap:wrap}
.oa-acc-body{display:none;padding:16px;background:#FAFAFA}
.oa-acc-body.open{display:block}

/* Table */
.oa-card{background:#fff;border:1px solid #E4E4EA;border-radius:10px;padding:16px;box-shadow:0 1px 3px rgba(0,0,0,.04)}
.oa-table-wrap{overflow-x:auto}
.oa-table{width:100%;border-collapse:collapse;font-size:13px}
.oa-table th{background:#F8F8FA;color:#5A5A72;font-weight:700;font-size:11px;padding:10px 12px;text-align:center;border-bottom:1px solid #E4E4EA;white-space:nowrap;text-transform:uppercase;letter-spacing:.04em}
.oa-table td{padding:11px 12px;text-align:center;border-bottom:1px solid #F0F2F5}
.oa-table tbody tr:last-child td{border-bottom:none}
.oa-table tbody tr:hover td{background:#FAFAFA}
.oa-table td.tl,.oa-table th.tl{text-align:right}
.oa-table tr.avg-row td{background:#F0F2F5;font-weight:700;border-top:2px solid #E4E4EA}

/* Report text block inside table */
.oa-report-text{font-size:11px;color:#5A5A72;margin-top:3px;padding:4px 8px;background:#F8F8FA;border-radius:4px;border-right:2px solid #D0D0DC;line-height:1.5}

/* diff colours */
.pos{color:#0D7A4E;font-weight:800}.neg{color:#9A1E1E;font-weight:800}.neu{color:#9A9AB0;font-weight:800}

/* Badges */
.oa-badge{display:inline-flex;align-items:center;gap:4px;font-size:11px;font-weight:700;padding:4px 10px;border-radius:6px;white-space:nowrap}
.kb-red{background:#FDEAEA;color:#9A1E1E}.kb-green{background:#E5F5EE;color:#0D7A4E}
.kb-amber{background:#FEF3E2;color:#8A5000}.kb-blue{background:#E8F1FB;color:#1A5FA0}.kb-gray{background:#F8F8FA;color:#5A5A72}

.oa-mini-bar{display:inline-flex;align-items:center;gap:4px;font-size:12px;font-weight:800}
.oa-small{font-size:11px;color:#9A9AB0;margin-top:2px}

/* Empty */
.oa-empty{background:#FEF3E2;color:#8A5000;border:1px solid rgba(138,80,0,.2);border-radius:8px;padding:14px;font-size:13px}

/* ═══════════════════
   CHARTS SECTION
   ═══════════════════ */
.oa-charts-section{margin-top:28px;border-top:2px dashed #E4E4EA;padding-top:20px}
.oa-charts-section-title{font-size:15px;font-weight:800;color:#5A5A72;margin-bottom:16px;display:flex;align-items:center;gap:8px}
.oa-charts-grid{display:grid;grid-template-columns:2fr 1fr;gap:16px;margin-bottom:16px}
.oa-chart-box{background:#fff;border:1px solid #E4E4EA;border-radius:12px;padding:18px;box-shadow:0 1px 3px rgba(0,0,0,.05)}
.oa-chart-title{font-size:13px;font-weight:700;color:#111118;margin-bottom:2px}
.oa-chart-sub{font-size:11px;color:#9A9AB0;margin-bottom:14px}
.oa-chart-wrap{position:relative}
.oa-chart-wrap.tall{height:300px}
.oa-chart-wrap.donut{height:220px}
.oa-chart-wrap.diff{height:220px}
.oa-legend{display:flex;flex-wrap:wrap;gap:10px;margin-top:12px}
.oa-legend-item{display:flex;align-items:center;gap:5px;font-size:11px;color:#5A5A72}
.oa-legend-dot{width:10px;height:10px;border-radius:50%;flex-shrink:0}

/* Heatmap */
.oa-heatmap-box{background:#fff;border:1px solid #E4E4EA;border-radius:12px;padding:18px;box-shadow:0 1px 3px rgba(0,0,0,.05);margin-bottom:16px}
.oa-heatmap{display:flex;gap:6px;flex-wrap:wrap;margin-top:10px}
.oa-heatmap-cell{border-radius:8px;padding:8px 12px;font-size:11px;font-weight:700;min-width:90px;text-align:center;cursor:default;flex:1}
.oa-heatmap-cell .hc-label{white-space:nowrap;overflow:hidden;text-overflow:ellipsis;font-size:10px;opacity:.8;margin-bottom:3px}
.oa-heatmap-cell .hc-val{font-size:16px;font-weight:800}

@media(max-width:900px){
  .oa-charts-grid{grid-template-columns:1fr}
  .oa-level-summary{grid-template-columns:1fr 1fr}
  .oa-hero{grid-template-columns:1fr}
}
@media(max-width:560px){
  .oa-level-summary{grid-template-columns:1fr}
  .oa-container{padding:14px 12px 50px}
}
</style>`);
    }

    // ─── Modal shell ──────────────────────────────────────────────────────────
    function ensureModal() {
        injectStyles();
        if ($("#" + modalId).length) return;
        $("body").append(`
<div class="modal fade" id="${modalId}" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-fullscreen">
        <div class="modal-content">
            <div class="modal-header bg-primary text-white">
                <h5 class="modal-title"><i class="las la-chart-line"></i> ${getText("lblOutputsAnalysis", "تحليل المخرجات")}</h5>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body" id="${bodyId}"></div>
        </div>
    </div>
</div>`);
    }

    function renderLoading() {
        return `<div class="text-center py-5"><div class="spinner-border text-primary"></div><div class="mt-2">جارٍ التحميل...</div></div>`;
    }

    // ─── Filters ──────────────────────────────────────────────────────────────
    function getFilteredRows(rows) {
        const level = $("#oaLevelFilter").val() || "all";
        const grade = $("#oaGradeFilter").val() || "all";
        return rows.filter(r =>
            (level === "all" || r.level === level) &&
            (grade === "all" || String(r.grade) === String(grade))
        );
    }

    function fillGradeFilter() {
        const level = $("#oaLevelFilter").val() || "all";
        const grades = new Set();
        (apiResponse.analysisTypes || []).forEach(t =>
            (t.outputAnalysisData || []).forEach(r => {
                if (level === "all" || getRowLevel(r) === level)
                    grades.add(getRowGradeName(r));
            })
        );
        $("#oaGradeFilter").html(
            `<option value="all">كل الصفوف</option>` +
            Array.from(grades)
                .sort((a, b) => String(a).localeCompare(String(b), "ar", { numeric: true }))
                .map(g => `<option value="${esc(g)}">${esc(g)}</option>`)
                .join("")
        );
    }

    // ─── Layout ───────────────────────────────────────────────────────────────
    function renderLayout() {
        const years = DATA.years;
        $("#" + bodyId).html(`
<div class="oa-wrap">
    <div class="oa-topbar">
        <div class="oa-title">
            <div class="oa-logo">🏫</div>
            <div>
                <h1>${getText("lblSchoolOutputsAnalysis", "تحليل المخرجات المدرسية")}</h1>
                <p>${getText("lblMainBreadcrumb", "")} › ${getText("lblPeriodicEvaluation", "")} › ${getText("lblSchool", "")} › ${getText("lblOutputsAnalysis", "")}</p>
            </div>
        </div>
        <button class="oa-print" type="button" onclick="window.print()">🖨 ${getText("lblPrint", "طباعة")}</button>
    </div>

    <div class="oa-container">
        <div class="oa-hero">
            <div>
                <div class="oa-eyebrow">${getText("lblFinalOutputsReport", "تقرير المخرجات النهائية")}</div>
                <h2>${getText("lblSchoolPerformanceForYears", "الأداء المدرسي")} — <span style="color:#8B1538">${esc(years[0])} & ${esc(years[1])}</span></h2>
                <div class="oa-sub">${getText("lblIncludesAcademicOutputs", "يشمل نتائج التحصيل الأكاديمي والمؤشرات التعليمية الأساسية")}</div>
            </div>
            <div class="oa-school">
                <div class="oa-school-label">${getText("lblSchoolName", "اسم المدرسة")}</div>
                <div class="oa-school-name">-</div>
            </div>
        </div>

        <div class="oa-filters">
            <div class="oa-field">
                <label>المرحلة</label>
                <select id="oaLevelFilter">
                    <option value="all">كل المراحل</option>
                    <option value="primary">الابتدائية</option>
                    <option value="middle">الإعدادية</option>
                    <option value="secondary">الثانوية</option>
                </select>
            </div>
            <div class="oa-field">
                <label>الصف</label>
                <select id="oaGradeFilter"><option value="all">كل الصفوف</option></select>
            </div>
            <div class="oa-field">
                <label>نوع التحليل</label>
                <select id="oaAnalysisTypeFilter">
                    <option value="all">كل التحليلات</option>
                    ${(apiResponse.analysisTypes || []).map(x =>
            `<option value="${esc(x.backendName)}">${esc(x.analysisTypeNameAr || x.analysisTypeNameEn)}</option>`
        ).join("")}
                </select>
            </div>
        </div>

        ${renderAnalysisTabs()}
    </div>
</div>`);

        bindEvents();
        fillGradeFilter();
        renderAll();
    }

    // ─── Tabs shell ───────────────────────────────────────────────────────────
    function renderAnalysisTabs() {
        const types = apiResponse?.analysisTypes || [];
        const tabs = types.map((t, i) =>
            `<button class="oa-tab ${i === 0 ? "active" : ""}" type="button"
                data-tab="analysis_${safeId(getTypeId(t))}" data-analysis="${esc(t.backendName)}">
                ${esc(t.analysisTypeNameAr || t.analysisTypeNameEn || t.backendName)}
            </button>`
        ).join("");
        const panels = types.map((t, i) =>
            `<div id="analysis_${safeId(getTypeId(t))}" class="oa-panel ${i === 0 ? "active" : ""}"></div>`
        ).join("");
        return `<div class="oa-tabs-shell"><div class="oa-tabs">${tabs}</div>${panels}</div>`;
    }

    // ─── Per-type panel ───────────────────────────────────────────────────────
    function renderAnalysisType(type) {
        const panelId = `analysis_${safeId(getTypeId(type))}`;
        const color = getTypeColor(type.backendName);

        // Normalise rows
        const allRows = (type.outputAnalysisData || []).map(r => ({
            ...r,
            level: getRowLevel(r),
            levelName: getRowLevelName(r),
            grade: getRowGradeName(r),
            subject: getRowSubjectName(r),
            prev: Number(r.previousYearValue || 0),
            last: Number(r.lastYearValue || 0),
            diff: Number(r.difference || 0),
            matrixName: getMatrixName(r),
            reportText: getMatrixReportText(r)   // from matrix only
        }));

        const rows = getFilteredRows(allRows);

        let html = `
<div class="oa-sec-head">
    <h3 style="color:${color.primary}">${esc(type.analysisTypeNameAr || type.analysisTypeNameEn || type.backendName)}</h3>
    <span class="oa-sec-sub">بحسب المسار والصف والمادة</span>
</div>`;

        if (!rows.length) {
            $("#" + panelId).html(html + `<div class="oa-empty">لا توجد بيانات تطابق الفلتر المحدد</div>`);
            return;
        }

        // ── Level summary ────────────────────────────────────────────────────
        html += buildLevelSummary(rows);

        // ── Accordion grouped by Track → Grade ───────────────────────────────
        html += buildAccordion(rows, color, panelId);

        // ── Charts section (below the tables) ───────────────────────────────
        html += buildChartsSection(rows, panelId, color);

        $("#" + panelId).html(html);

        // Draw charts after DOM insertion
        drawBarChart(rows, panelId, color);
        drawDonutChart(rows, panelId, color);
        drawDiffChart(rows, panelId, color);
        drawHeatmap(rows, panelId);

        // Accordion toggle
        $(`#${panelId} .oa-acc-trigger`).on("click", function () {
            const bodyEl = $(this).next(".oa-acc-body");
            const isOpen = bodyEl.hasClass("open");
            bodyEl.toggleClass("open", !isOpen);
            $(this).toggleClass("open", !isOpen);
        });

        // Open first item by default
        $(`#${panelId} .oa-acc-item:first-child .oa-acc-trigger`).trigger("click");
    }

    // ─── Level summary ────────────────────────────────────────────────────────
    function buildLevelSummary(rows) {
        const levels = [
            { key: "primary", label: "الابتدائية", cls: "primary" },
            { key: "middle", label: "الإعدادية", cls: "middle" },
            { key: "secondary", label: "الثانوية", cls: "secondary" }
        ];
        return `<div class="oa-level-summary">` +
            levels.map(lv => {
                const lr = rows.filter(r => r.level === lv.key);
                if (!lr.length) return `<div class="oa-level-card ${lv.cls}"><div class="lc-label">${lv.label}</div><div class="lc-val">—</div></div>`;
                const avg = lr.reduce((a, r) => a + r.last, 0) / lr.length;
                const diff = avg - lr.reduce((a, r) => a + r.prev, 0) / lr.length;
                return `
                <div class="oa-level-card ${lv.cls}">
                    <div class="lc-label">${lv.label}</div>
                    <div class="lc-val">${f2(avg)}%</div>
                    <div class="lc-sub">${diff >= 0 ? "▲" : "▼"} ${Math.abs(diff).toFixed(2)}% مقارنةً بالعام السابق</div>
                </div>`;
            }).join("") +
            `</div>`;
    }

    // ─── Accordion: Track → Grade ─────────────────────────────────────────────
    function buildAccordion(rows, color, panelId) {
        let html = "";

        // Separate rows with a track from rows without
        const withTrack = rows.filter(r => r.track);
        const withoutTrack = rows.filter(r => !r.track);

        // Group-by track (for rows with a track)
        const byTrack = groupBy(withTrack, r => r.track || "__none__");

        // Helper: render one accordion item for a grade group
        function gradeAccordionItem(gradeRows, accId) {
            const r0 = gradeRows[0];
            const avg = gradeRows.reduce((a, r) => a + r.last, 0) / gradeRows.length;
            const prevAvg = gradeRows.reduce((a, r) => a + r.prev, 0) / gradeRows.length;
            const diff = avg - prevAvg;
            const badgeCls = judgeClass(avg);

            return `
<div class="oa-acc-item">
    <button class="oa-acc-trigger" type="button" data-acc="${accId}">
        <div>
            <div class="oa-acc-grade-label">${esc(r0.grade)}</div>
            <div class="oa-acc-meta">${esc(r0.levelName || lvlName(r0.level))} — ${gradeRows.length} مادة / مسار</div>
        </div>
        <div class="oa-acc-pills">
            <span class="oa-badge ${badgeCls}">${f2(avg)}%</span>
            <span class="oa-mini-bar ${diffClass(diff)}">${diffArrow(diff)} ${Math.abs(diff).toFixed(2)}%</span>
        </div>
        <span class="oa-acc-arrow">▼</span>
    </button>
    <div class="oa-acc-body" id="${accId}">
        <div class="oa-card">
            ${buildGradeTable(gradeRows, color)}
        </div>
    </div>
</div>`;
        }

        // ── Rows WITH a track: one header per track ───────────────────────────
        for (const [track, trackRows] of byTrack) {
            const byGrade = groupBy(trackRows, r => r.grade);

            html += `
<div class="oa-acc-group">
    <div class="oa-track-header">
        <span class="track-icon">🔀</span>
        ${esc(trackLabel(track))}
        <span class="oa-track-badge">${trackRows.length} صف</span>
    </div>`;

            for (const [, gradeRows] of byGrade) {
                const accId = `acc_${safeId(panelId)}_${safeId(track)}_${safeId(gradeRows[0].grade)}`;
                html += gradeAccordionItem(gradeRows, accId);
            }

            html += `</div>`;
        }

        // ── Rows WITHOUT a track (grouped only by grade) ──────────────────────
        if (withoutTrack.length) {
            const byGrade = groupBy(withoutTrack, r => r.grade);
            html += `<div class="oa-acc-group">`;
            for (const [, gradeRows] of byGrade) {
                const accId = `acc_${safeId(panelId)}_notrack_${safeId(gradeRows[0].grade)}`;
                html += gradeAccordionItem(gradeRows, accId);
            }
            html += `</div>`;
        }

        return html;
    }

    // ─── Grade table ──────────────────────────────────────────────────────────
    function buildGradeTable(rows, color) {
        const avg = rows.reduce((a, r) => a + r.last, 0) / rows.length;
        const prevAvg = rows.reduce((a, r) => a + r.prev, 0) / rows.length;

        return `
<div class="oa-table-wrap">
    <table class="oa-table">
        <thead>
            <tr>
                <th class="tl">المادة / المسار</th>
                <th>${esc(DATA.years[0])}</th>
                <th>${esc(DATA.years[1])}</th>
                <th>الفرق</th>
                <th>التقييم</th>
                <th>عدد الطلاب</th>
            </tr>
        </thead>
        <tbody>
            ${rows.map(r => {
            const reportText = r.reportText;
            return `
            <tr>
                <td class="tl">
                    <strong>${esc(r.subject)}</strong>
                    ${reportText
                    ? `<div class="oa-report-text">${esc(reportText)}</div>`
                    : ""}
                </td>
                <td>${f2(r.prev)}</td>
                <td><strong>${f2(r.last)}</strong></td>
                <td>
                    <span class="oa-mini-bar ${diffClass(r.diff)}">
                        ${diffArrow(r.diff)} ${r.diff > 0 ? "+" : ""}${f2(r.diff)}
                    </span>
                </td>
                <td>
                    <span class="oa-badge ${judgeClass(r.last)}">
                        ${esc(r.matrixName || judgeLabel(r.last))}
                    </span>
                </td>
                <td style="color:#5A5A72;font-size:12px">${r.lastYearStudentCount || "—"}</td>
            </tr>`;
        }).join("")}
            <tr class="avg-row">
                <td class="tl">المتوسط</td>
                <td>${f2(prevAvg)}</td>
                <td><strong>${f2(avg)}%</strong></td>
                <td>
                    <span class="oa-mini-bar ${diffClass(avg - prevAvg)}">
                        ${diffArrow(avg - prevAvg)} ${Math.abs(avg - prevAvg).toFixed(2)}%
                    </span>
                </td>
                <td><span class="oa-badge ${judgeClass(avg)}">${judgeLabel(avg)}</span></td>
                <td></td>
            </tr>
        </tbody>
    </table>
</div>`;
    }

    // ─── Charts section wrapper (appears BELOW all accordion tables) ──────────
    function buildChartsSection(rows, panelId, color) {
        return `
<div class="oa-charts-section">
    <div class="oa-charts-section-title">📊 الرسوم البيانية</div>

    <div class="oa-charts-grid">
        <div class="oa-chart-box">
            <div class="oa-chart-title">المقارنة بين العامين — بالصف</div>
            <div class="oa-chart-sub">${esc(DATA.years[0])} مقابل ${esc(DATA.years[1])}</div>
            <div class="oa-chart-wrap tall"><canvas id="${panelId}_bar"></canvas></div>
            <div class="oa-legend" id="${panelId}_bar_legend"></div>
        </div>
        <div class="oa-chart-box">
            <div class="oa-chart-title">توزيع مستوى الأداء</div>
            <div class="oa-chart-sub">السنة الأخيرة — ${esc(DATA.years[1])}</div>
            <div class="oa-chart-wrap donut"><canvas id="${panelId}_donut"></canvas></div>
            <div class="oa-legend" id="${panelId}_donut_legend"></div>
        </div>
    </div>

    <div class="oa-heatmap-box">
        <div class="oa-chart-title">خريطة أداء المواد</div>
        <div class="oa-chart-sub">متوسط القيمة الفعلية لكل مادة — مرتبة تنازلياً</div>
        <div class="oa-heatmap" id="${panelId}_heatmap"></div>
    </div>

    <div class="oa-chart-box">
        <div class="oa-chart-title">اتجاه الفروق بين العامين — بالصف</div>
        <div class="oa-chart-sub">الفرق بين السنة الأخيرة والسنة السابقة (أخضر = تحسّن ، أحمر = تراجع)</div>
        <div class="oa-chart-wrap diff"><canvas id="${panelId}_diff"></canvas></div>
    </div>
</div>`;
    }

    // ─── Chart: grouped bar (prev vs last by grade) ───────────────────────────
    function drawBarChart(rows, panelId, color) {
        const byGrade = groupBy(rows, r => r.grade);
        const labels = [], prevArr = [], lastArr = [];
        for (const [g, rs] of byGrade) {
            labels.push(g);
            prevArr.push(+(rs.reduce((a, r) => a + r.prev, 0) / rs.length).toFixed(2));
            lastArr.push(+(rs.reduce((a, r) => a + r.last, 0) / rs.length).toFixed(2));
        }
        makeChart(`${panelId}_bar`, {
            type: "bar",
            data: {
                labels,
                datasets: [
                    { label: DATA.years[0], data: prevArr, backgroundColor: "rgba(90,90,114,.3)", borderColor: "rgba(90,90,114,.6)", borderWidth: 1, borderRadius: 4 },
                    { label: DATA.years[1], data: lastArr, backgroundColor: color.primary + "CC", borderColor: color.primary, borderWidth: 1, borderRadius: 4 }
                ]
            },
            options: {
                responsive: true, maintainAspectRatio: false,
                plugins: { legend: { display: false }, tooltip: { callbacks: { label: ctx => `${ctx.dataset.label}: ${ctx.parsed.y.toFixed(2)}%` } } },
                scales: {
                    y: { beginAtZero: true, max: 100, grid: { color: "rgba(0,0,0,.05)" }, ticks: { callback: v => v + "%", font: { size: 11 } } },
                    x: { grid: { display: false }, ticks: { font: { size: 11 } } }
                }
            }
        });
        $(`#${panelId}_bar_legend`).html(`
            <div class="oa-legend-item"><div class="oa-legend-dot" style="background:rgba(90,90,114,.6)"></div>${esc(DATA.years[0])}</div>
            <div class="oa-legend-item"><div class="oa-legend-dot" style="background:${color.primary}"></div>${esc(DATA.years[1])}</div>
        `);
    }

    // ─── Chart: donut (performance distribution) ──────────────────────────────
    function drawDonutChart(rows, panelId) {
        const bins = { "ممتاز": 0, "جيد جداً": 0, "جيد": 0, "مقبول": 0, "ضعيف": 0 };
        rows.forEach(r => { const l = judgeLabel(r.last); bins[l] = (bins[l] || 0) + 1; });
        const labels = Object.keys(bins).filter(k => bins[k] > 0);
        const data = labels.map(k => bins[k]);
        const bg = ["#0D7A4E", "#1A5FA0", "#8B1538", "#8A5000", "#9A1E1E"];
        makeChart(`${panelId}_donut`, {
            type: "doughnut",
            data: { labels, datasets: [{ data, backgroundColor: bg, borderWidth: 2, borderColor: "#fff" }] },
            options: {
                responsive: true, maintainAspectRatio: false, cutout: "65%",
                plugins: { legend: { display: false }, tooltip: { callbacks: { label: ctx => `${ctx.label}: ${ctx.parsed}` } } }
            }
        });
        $(`#${panelId}_donut_legend`).html(
            labels.map((l, i) => `<div class="oa-legend-item"><div class="oa-legend-dot" style="background:${bg[i]}"></div>${esc(l)} (${data[i]})</div>`).join("")
        );
    }

    // ─── Chart: diff bars ─────────────────────────────────────────────────────
    function drawDiffChart(rows, panelId) {
        const byGrade = groupBy(rows, r => r.grade);
        const labels = [], diffs = [];
        for (const [g, rs] of byGrade) {
            labels.push(g);
            diffs.push(+(rs.reduce((a, r) => a + r.diff, 0) / rs.length).toFixed(2));
        }
        const colors = diffs.map(d => d >= 0 ? "#0D7A4E" : "#9A1E1E");
        makeChart(`${panelId}_diff`, {
            type: "bar",
            data: { labels, datasets: [{ label: "الفرق", data: diffs, backgroundColor: colors.map(c => c + "99"), borderColor: colors, borderWidth: 2, borderRadius: 4 }] },
            options: {
                responsive: true, maintainAspectRatio: false,
                plugins: { legend: { display: false }, tooltip: { callbacks: { label: ctx => `الفرق: ${ctx.parsed.y > 0 ? "+" : ""}${ctx.parsed.y.toFixed(2)}%` } } },
                scales: {
                    y: { grid: { color: ctx => ctx.tick.value === 0 ? "rgba(0,0,0,.25)" : "rgba(0,0,0,.05)" }, ticks: { callback: v => (v > 0 ? "+" : "") + v + "%", font: { size: 11 } } },
                    x: { grid: { display: false }, ticks: { font: { size: 11 } } }
                }
            }
        });
    }

    // ─── Heatmap cells ────────────────────────────────────────────────────────
    function drawHeatmap(rows, panelId) {
        const bySubject = groupBy(rows, r => r.subject);
        const avgs = Array.from(bySubject.entries())
            .map(([s, rs]) => ({ subject: s, avg: rs.reduce((a, r) => a + r.last, 0) / rs.length }))
            .sort((a, b) => b.avg - a.avg);

        function heatColor(v) {
            if (v >= 80) return { bg: "#E5F5EE", color: "#0D7A4E" };
            if (v >= 70) return { bg: "#E8F1FB", color: "#1A5FA0" };
            if (v >= 50) return { bg: "#FEF3E2", color: "#8A5000" };
            return { bg: "#FDEAEA", color: "#9A1E1E" };
        }

        $(`#${panelId}_heatmap`).html(
            avgs.map(s => {
                const c = heatColor(s.avg);
                return `
                <div class="oa-heatmap-cell" style="background:${c.bg};color:${c.color}">
                    <div class="hc-label" title="${esc(s.subject)}">${esc(s.subject)}</div>
                    <div class="hc-val">${f2(s.avg)}%</div>
                </div>`;
            }).join("") || `<span style="color:#9A9AB0;font-size:12px">لا توجد بيانات</span>`
        );
    }

    // ─── Events ───────────────────────────────────────────────────────────────
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

        $("#oaGradeFilter,#oaAnalysisTypeFilter").off("change").on("change", renderAll);
    }

    // ─── Render all ───────────────────────────────────────────────────────────
    function renderAll() {
        destroyCharts();
        const selectedBackend = $("#oaAnalysisTypeFilter").val() || "all";
        const visibleTypes = (apiResponse.analysisTypes || [])
            .filter(t => selectedBackend === "all" || t.backendName === selectedBackend);

        visibleTypes.forEach(type => renderAnalysisType(type));

        if (!currentTab) {
            const first = visibleTypes[0];
            currentTab = first ? `analysis_${safeId(getTypeId(first))}` : null;
        }

        if (currentTab) {
            $(".oa-panel").removeClass("active");
            $("#" + currentTab).addClass("active");
            $(".oa-tab").removeClass("active");
            $(`.oa-tab[data-tab="${currentTab}"]`).addClass("active");
        }
    }

    // ─── Entry point ──────────────────────────────────────────────────────────
    window.openOutputsAnalysis = function (evaluationRequestId) {
        if (!evaluationRequestId) {
            notificationUtil?.error?.("Missing evaluation request id");
            return;
        }
        ensureModal();
        $("#" + bodyId).html(renderLoading());
        $("#" + modalId).modal("show");

        jqClient({
            success(response) {
                apiResponse = response || { analysisTypes: [] };
                DATA = buildData();
                const first = apiResponse.analysisTypes?.[0];
                currentTab = first ? `analysis_${safeId(getTypeId(first))}` : null;
                ensureChartJs(renderLayout);
            },
            error() {
                $("#" + bodyId).html(`<div class="alert alert-danger m-3">حدث خطأ أثناء تحميل تحليل المخرجات</div>`);
            }
        }).Get(`/ServiceRequest/${departmentRoutePath}/GetOutputAnalysis?evaluationRequestId=${evaluationRequestId}`);
    };

})();
