(function () {
    "use strict";

    let _data = null;
    let _filtered = null;

    const lbl = key => uiControlsSetup().GetUiControlText(key) || key;
    const isAr = window.currentLang === "ar";

    window.openFormAnalysis = async function (requestId) {
        if (!requestId) {
            notificationUtil?.error?.(lbl("lblRequestIdIsRequired"));
            return;
        }

        const modal = bootstrap.Modal.getOrCreateInstance(
            document.getElementById("FormAnalysisModal")
        );

        modal.show();

        await loadAnalysis(requestId);
    };

    function loadAnalysis(requestId) {
        const container = document.getElementById("FormAnalysisContainer");

        container.innerHTML = `
            <div class="text-center p-5">
                <div class="spinner-border text-primary"></div>
                <div class="mt-2">${lbl("lblLoadingFormAnalysis")}</div>
            </div>
        `;

        const options = {
            success: function (response) {
                _data = response || {};
                _filtered = structuredClone(_data);

                renderMainLayout();
                bindEvents();
                applyFilters();
            },
            error: function (xhr) {
                console.error(xhr);

                container.innerHTML = `
                    <div class="alert alert-danger m-4">
                        ${lbl("lblFailedToLoadFormAnalysis")}
                    </div>
                `;
            }
        };

        jqClient(options).Get(
            `/ServiceRequest/${departmentRoutePath}/FormAnalysis?requestId=${requestId}`
        );
    }

    function renderMainLayout() {
        document.getElementById("FormAnalysisContainer").innerHTML = `
            <style>
                .fa-page{padding:24px;direction:rtl}
                .fa-title{font-size:22px;font-weight:700;margin-bottom:4px}
                .fa-breadcrumb{font-size:12px;color:#999;margin-bottom:18px}
                .fa-card{background:#fff;border:1px solid #e5e5e5;border-radius:12px;margin-bottom:20px;overflow:hidden}
                .fa-card-header{background:#97133f;color:#fff;padding:11px 16px;font-weight:700;display:flex;justify-content:space-between}
                .fa-filter{border-top:3px solid #97133f;padding:16px}
                .fa-grid{display:grid;grid-template-columns:repeat(3,1fr);gap:12px}
                .fa-summary{display:grid;grid-template-columns:repeat(5,1fr);gap:12px;margin-bottom:20px}
                .fa-summary-card{background:#fff;border:1px solid #ddd;border-radius:12px;padding:16px;border-top:3px solid #97133f}
                .fa-label{font-size:12px;color:#666;margin-bottom:6px}
                .fa-value{font-size:24px;font-weight:800;color:#97133f}
                .fa-tabs{display:flex;gap:4px;margin-bottom:18px;border-bottom:2px solid #ddd}
                .fa-tab{padding:10px 18px;cursor:pointer;font-weight:600;color:#666}
                .fa-tab.active{color:#97133f;border-bottom:3px solid #97133f}
                .fa-page-section{display:none}
                .fa-page-section.active{display:block}
                .fa-bar-track{height:8px;background:#eee;border-radius:10px;overflow:hidden;min-width:100px}
                .fa-bar-fill{height:100%;border-radius:10px}
                .fa-chart-row{display:flex;align-items:center;gap:12px;margin-bottom:12px}
                .fa-chart-label{width:220px;font-size:13px;color:#555}
                .fa-chart-track{flex:1;height:24px;background:#eee;border-radius:6px;overflow:hidden}
                .fa-chart-fill{height:100%;display:flex;align-items:center;color:#fff;font-weight:700;font-size:12px;padding:0 8px}
                .fa-badge{padding:4px 10px;border-radius:20px;font-size:12px;font-weight:700}
                .fa-ex{background:#e1f5ee;color:#0f6e56}
                .fa-vg{background:#e6f1fb;color:#185fa5}
                .fa-g{background:#faeeda;color:#854f0b}
                .fa-ac{background:#faece7;color:#993c1d}
                .fa-wk{background:#fcebeb;color:#a32d2d}
                .fa-table th,.fa-table td{text-align:center;vertical-align:middle}
                .fa-table th:last-child,.fa-table td:last-child{text-align:right}
                @media(max-width:992px){
                    .fa-grid{grid-template-columns:repeat(2,1fr)}
                    .fa-summary{grid-template-columns:repeat(2,1fr)}
                }
            </style>

            <div class="fa-page">
                <div class="fa-title">${lbl("lblClassroomObservationResults")}</div>
                <div class="fa-breadcrumb">${lbl("lblHome")} / ${lbl("lblFormAnalysis")}</div>

                ${renderFilters()}

                <div class="fa-tabs">
                    <div class="fa-tab active" data-tab="results">${lbl("lblObservationResults")}</div>
                    <div class="fa-tab" data-tab="criteria">${lbl("lblCriteriaAnalysis")}</div>
                    <div class="fa-tab" data-tab="subjects">${lbl("lblSubjectAnalysis")}</div>
                    <div class="fa-tab" data-tab="compare">${lbl("lblGradeComparison")}</div>
                </div>

                <div id="tab-results" class="fa-page-section active"></div>
                <div id="tab-criteria" class="fa-page-section"></div>
                <div id="tab-subjects" class="fa-page-section"></div>
                <div id="tab-compare" class="fa-page-section"></div>
            </div>
        `;
    }

    function renderFilters() {
        return `
            <div class="fa-card fa-filter">
                <div class="fa-grid">
                    <div>
                        <label class="form-label small fw-bold">${lbl("lblEducationLevel")}</label>
                        <select id="fa-stage" class="form-select">
                            <option value="">${lbl("lblAllEducationLevels")}</option>
                        </select>
                    </div>

                    <div>
                        <label class="form-label small fw-bold">${lbl("lblGrade")}</label>
                        <select id="fa-grade" class="form-select">
                            <option value="">${lbl("lblAllGrades")}</option>
                        </select>
                    </div>

                    <div>
                        <label class="form-label small fw-bold">${lbl("lblSubject")}</label>
                       <select id="fa-subject" class="form-select">
                            <option value="">${lbl("lblAllSubjects")}</option>
                        </select>
                    </div>
                </div>

                <div class="text-end mt-3">
                    <button class="btn btn-primary btn-sm" id="fa-search-btn">
                        <i class="las la-search"></i>
                        ${lbl("WebAppRequest_lblSearch")}
                    </button>

                    <button class="btn btn-secondary btn-sm" id="fa-reset-btn">
                        ${lbl("lblReset")}
                    </button>
                </div>
            </div>
        `;
    }

    function bindEvents() {
        document.querySelectorAll(".fa-tab").forEach(tab => {
            tab.addEventListener("click", function () {
                document.querySelectorAll(".fa-tab").forEach(x => x.classList.remove("active"));
                document.querySelectorAll(".fa-page-section").forEach(x => x.classList.remove("active"));

                this.classList.add("active");
                document.getElementById("tab-" + this.dataset.tab).classList.add("active");
            });
        });

        fillLookups();

        document.getElementById("fa-search-btn").addEventListener("click", applyFilters);

        document.getElementById("fa-reset-btn").addEventListener("click", function () {
            ["fa-stage", "fa-grade", "fa-subject"].forEach(id => {
                const el = document.getElementById(id);
                if (el) el.value = "";
            });

            applyFilters();
        });
    }

    function fillLookups() {
        fillSelect("fa-stage", uniqueFromObservations("educationLevelId", "educationLevelNameAr", "educationLevelNameEn"));
        fillSelect("fa-grade", uniqueFromObservations("gradeLevelId", "gradeLevelNameAr", "gradeLevelNameEn"));
        fillSelect("fa-subject", uniqueFromObservations("schoolCourseId", "schoolCourseNameAr", "schoolCourseNameEn"));
    }

    function uniqueFromObservations(idKey, arKey, enKey) {
        const map = new Map();

        (_data.observations || []).forEach(x => {
            if (x[idKey]) {
                map.set(
                    x[idKey],
                    isAr
                        ? (x[arKey] || x[enKey] || "-")
                        : (x[enKey] || x[arKey] || "-")
                );
            }
        });

        return [...map.entries()].map(([id, name]) => ({ id, name }));
    }

    function fillSelect(id, items) {
        const select = document.getElementById(id);

        items.forEach(x => {
            select.insertAdjacentHTML(
                "beforeend",
                `<option value="${escapeHtml(x.id)}">${escapeHtml(x.name)}</option>`
            );
        });
    }
    function applyFilters() {
        const stageId = document.getElementById("fa-stage").value;
        const gradeId = document.getElementById("fa-grade").value;
        const subjectId = document.getElementById("fa-subject").value;

        let observations = [...(_data.observations || [])]
            .filter(x => x.items && x.items.length > 0);

        if (stageId)
            observations = observations.filter(x => x.educationLevelId === stageId);

        if (gradeId)
            observations = observations.filter(x => x.gradeLevelId === gradeId);

        if (subjectId)
            observations = observations.filter(x => x.schoolCourseId === subjectId);

        _filtered = buildAnalysisFromObservations(observations);

        renderResultsTab();
        renderCriteriaTab();
        renderSubjectsTab();
        renderCompareTab();
    }

    function buildAnalysisFromObservations(observations) {
        const values = observations.flatMap(o =>
            (o.items || []).map(i => ({
                ...i,
                observationId: o.id,
                educationLevelId: o.educationLevelId,
                educationLevelName: isAr
                    ? (o.educationLevelNameAr || o.educationLevelNameEn)
                    : (o.educationLevelNameEn || o.educationLevelNameAr),
                gradeLevelId: o.gradeLevelId,
                gradeLevelName: isAr
                    ? (o.gradeLevelNameAr || o.gradeLevelNameEn)
                    : (o.gradeLevelNameEn || o.gradeLevelNameAr),
                schoolCourseId: o.schoolCourseId,
                schoolCourseName: isAr
                    ? (o.schoolCourseNameAr || o.schoolCourseNameEn)
                    : (o.schoolCourseNameEn || o.schoolCourseNameAr)
            }))
        );

        const criteria = groupBy(values, x => x.formItemId).map(g => {
            const first = g.items[0];
            const avg = average(g.items.map(x => num(x.actualValue)));
            const max = average(g.items.map(x => num(x.max || 5))) || 5;
            const pct = max > 0 ? avg / max * 100 : 0;

            return {
                formItemId: g.key,
                criteriaNameAr: isAr
                    ? (first.itemNameAr || first.itemNameEn)
                    : (first.itemNameEn || first.itemNameAr),
                average: avg,
                percentage: pct,
                rate: getRate(avg),
                byGrade: buildByGrade(g.items)
            };
        });

        const validCriteria = criteria.filter(x => x.average > 0);

        const overallAverage = average(validCriteria.map(x => x.average));
        const overallPercentage = overallAverage / 5 * 100;

        const highest = [...validCriteria].sort((a, b) => b.average - a.average)[0];
        const lowest = [...validCriteria].sort((a, b) => a.average - b.average)[0];

        return {
            observations,
            values,
            criteria,
            summary: {
                totalSessions: observations.length,
                overallAverage,
                overallPercentage,
                overallRate: getRate(overallAverage),
                bestCriteriaName: highest?.criteriaNameAr || "-",
                bestCriteriaAvg: highest?.average || 0,
                worstCriteriaName: lowest?.criteriaNameAr || "-",
                worstCriteriaAvg: lowest?.average || 0
            },
            subjects: buildSubjects(values),
            grades: buildGrades(values)
        };
    }

    function buildByGrade(items) {
        return groupBy(items, x => x.gradeLevelId).map(g => {
            const first = g.items[0];
            const avg = average(g.items.map(x => num(x.actualValue)));
            const max = average(g.items.map(x => num(x.max || 5))) || 5;
            const pct = max > 0 ? avg / max * 100 : 0;

            return {
                gradeLevelId: g.key,
                gradeLevelName: first.gradeLevelName || "-",
                average: avg,
                percentage: pct,
                rate: getRate(avg)
            };
        });
    }

    function buildSubjects(values) {
        return groupBy(values, x => x.schoolCourseId).map(g => {
            const first = g.items[0];

            const criteriaValues = {};
            groupBy(g.items, x => x.formItemId).forEach(cg => {
                const cFirst = cg.items[0];
                criteriaValues[
                    isAr
                        ? (cFirst.itemNameAr || cFirst.itemNameEn || "-")
                        : (cFirst.itemNameEn || cFirst.itemNameAr || "-")
                ] =
                    average(cg.items.map(x => num(x.actualValue)));
            });

            const avg = average(g.items.map(x => num(x.actualValue)));

            return {
                subjectId: g.key,
                subjectName: first.schoolCourseName || "-",
                average: avg,
                percentage: avg / 5 * 100,
                rate: getRate(avg),
                criteriaValues
            };
        });
    }

    function buildGrades(values) {
        return groupBy(values, x => x.gradeLevelId).map(g => {
            const first = g.items[0];
            const avg = average(g.items.map(x => num(x.actualValue)));

            return {
                gradeLevelId: g.key,
                gradeLevelName: first.gradeLevelName || "-",
                average: avg,
                percentage: avg / 5 * 100,
                rate: getRate(avg),
                sessionCount: new Set(g.items.map(x => x.observationId)).size
            };
        }).sort((a, b) => b.average - a.average)
            .map((x, i) => ({ ...x, rank: i + 1 }));
    }

    function renderResultsTab() {
        const s = _filtered.summary;

        document.getElementById("tab-results").innerHTML = `
           <div class="fa-summary">
            ${summaryCard(lbl("lblObservationsCount"), s.totalSessions, lbl("lblClassroomObservationSession"))}
            ${summaryCard(lbl("lblOverallResult"), `${fmt(s.overallAverage)} / 5`, `${lbl("lblPercentage")}: ${fmt(s.overallPercentage)}%`)}
            ${summaryCard(lbl("lblOverallRate"), s.overallRate?.text ?? "—", lbl("lblBasedOnOverallAverage"))}
            ${summaryCard(lbl("lblHighestCriteria"), s.bestCriteriaName, fmt(s.bestCriteriaAvg))}
            ${summaryCard(lbl("lblLowestCriteria"), s.worstCriteriaName, fmt(s.worstCriteriaAvg))}
        </div>

            <div class="fa-card">
                <div class="fa-card-header">
                    <span>📋 ${lbl("lblCriteriaResults")}</span>
                    <span>${_filtered.criteria.length} ${lbl("lblCriteriaCount")}</span>
                </div>

                <div class="table-responsive">
                    <table class="table fa-table table-hover m-0">
                        <thead>
                            <tr>
                                <th>${lbl("lblSerial")}</th>
                                <th>${lbl("lblCriteria")}</th>
                                <th>${lbl("lblAverage")}</th>
                                <th>${lbl("lblPercentage")}</th>
                                <th>${lbl("lblRate")}</th>
                                <th>${lbl("lblDetails")}</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${_filtered.criteria.map((c, i) => `
                                <tr>
                                    <td>${i + 1}</td>
                                    <td>${escapeHtml(c.criteriaNameAr)}</td>
                                    <td class="fw-bold" style="color:#97133f">${dash(c.average)}</td>
                                    <td>${progress(c.percentage, c.rate)}</td>
                                    <td>${badge(c.rate)}</td>
                                    <td>
                                        <button class="btn btn-sm btn-outline-danger"
                                                onclick="showCriteriaDetails('${c.formItemId}')">
                                            ${lbl("lblView")}
                                        </button>
                                    </td>
                                </tr>
                            `).join("")}
                        </tbody>
                    </table>
                </div>
            </div>

            <div class="fa-card p-4">
                <div class="fw-bold mb-3">${lbl("lblVisualCriteriaComparison")}</div>
                ${chart(_filtered.criteria, "criteriaNameAr")}
            </div>
        `;
    }

    function renderCriteriaTab() {
        document.getElementById("tab-criteria").innerHTML = `
            <div class="fa-card p-4">
                <div class="fw-bold mb-3">${lbl("lblAverageCriteriaAcrossStages")}</div>
                ${chart(_filtered.criteria, "criteriaNameAr")}
            </div>

            <div class="fa-card">
                <div class="fa-card-header">
                    <span>📋 ${lbl("lblCriteriaByGrade")}</span>
                </div>

                ${criteriaByGradeTable()}
            </div>
        `;
    }

    function renderSubjectsTab() {
        const criteriaNames = _filtered.criteria.map(x => x.criteriaNameAr);

        document.getElementById("tab-subjects").innerHTML = `
            <div class="fa-card">
                <div class="fa-card-header">
                    <span>📚 ${lbl("lblSubjectsAnalysis")}</span>
                </div>

                <div class="table-responsive">
                    <table class="table fa-table table-hover m-0">
                        <thead>
                            <tr>
                                <th>${lbl("lblSubject")}</th>
                                ${criteriaNames.map(x => `<th>${escapeHtml(shortName(x))}</th>`).join("")}
                                <th>${lbl("lblAverage")}</th>
                                <th>${lbl("lblRate")}</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${_filtered.subjects.map(s => `
                                <tr>
                                    <td>${escapeHtml(s.subjectName)}</td>
                                    ${criteriaNames.map(cn => `<td>${dash(s.criteriaValues[cn])}</td>`).join("")}
                                    <td class="fw-bold" style="color:#97133f">${dash(s.average)}</td>
                                    <td>${badge(s.rate)}</td>
                                </tr>
                            `).join("")}
                        </tbody>
                    </table>
                </div>
            </div>
        `;
    }

    function renderCompareTab() {
        document.getElementById("tab-compare").innerHTML = `
            <div class="fa-card p-4">
                <div class="fw-bold mb-3">${lbl("lblOverallResultAllGradesComparison")}</div>
                ${chart(_filtered.grades, "gradeLevelName")}
            </div>

            <div class="fa-card">
                <div class="fa-card-header">
                    <span>⚖️ ${lbl("lblComprehensiveComparisonTable")}</span>
                </div>

                <div class="table-responsive">
                    <table class="table fa-table table-hover m-0">
                        <thead>
                            <tr>
                                <th>${lbl("lblGradeCategory")}</th>
                                <th>${lbl("lblSessionsCount")}</th>
                                <th>${lbl("lblAverage")}</th>
                                <th>${lbl("lblPercentage")}</th>
                                <th>${lbl("lblRate")}</th>
                                <th>${lbl("lblRank")}</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${_filtered.grades.map(g => `
                                <tr>
                                    <td>${escapeHtml(g.gradeLevelName)}</td>
                                    <td>${g.sessionCount}</td>
                                    <td class="fw-bold" style="color:#97133f">${fmt(g.average)}</td>
                                    <td>${progress(g.percentage, g.rate)}</td>
                                    <td>${badge(g.rate)}</td>
                                    <td>#${g.rank}</td>
                                </tr>
                            `).join("")}
                        </tbody>
                    </table>
                </div>
            </div>
        `;
    }

    function criteriaByGradeTable() {
        const grades = _filtered.grades.map(x => x.gradeLevelName);

        return `
            <div class="table-responsive">
                <table class="table fa-table table-hover m-0">
                    <thead>
                        <tr>
                            <th>${lbl("lblCriteria")}</th>
                            ${grades.map(g => `<th>${escapeHtml(g)}</th>`).join("")}
                            <th>${lbl("lblGeneralAverage")}</th>
                            <th>${lbl("lblRate")}</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${_filtered.criteria.map(c => `
                            <tr>
                                <td>${escapeHtml(c.criteriaNameAr)}</td>
                                ${grades.map(g => {
            const row = c.byGrade.find(x => x.gradeLevelName === g);
            return `<td>${dash(row?.average)}</td>`;
        }).join("")}
                                <td class="fw-bold" style="color:#97133f">${dash(c.average)}</td>
                                <td>${badge(c.rate)}</td>
                            </tr>
                        `).join("")}
                    </tbody>
                </table>
            </div>
        `;
    }

    window.showCriteriaDetails = function (formItemId) {
        const c = _filtered.criteria.find(x => x.formItemId === formItemId);
        if (!c) return;

        document.getElementById("CriteriaDetailsTitle").innerText = c.criteriaNameAr;

        document.getElementById("CriteriaDetailsBody").innerHTML = `
            <div class="p-3 mb-3 rounded" style="background:#fbeaf0">
                <div class="row text-center">
                    <div class="col">
                        <div class="small text-muted">${lbl("lblGeneralAverage")}</div>
                        <div class="fs-3 fw-bold" style="color:#97133f">${fmt(c.average)}</div>
                    </div>
                    <div class="col">
                        <div class="small text-muted">${lbl("lblPercentage")}</div>
                        <div class="fs-4 fw-bold" style="color:#97133f">${fmt(c.percentage)}%</div>
                    </div>
                    <div class="col d-flex align-items-center justify-content-center">
                        ${badge(c.rate)}
                    </div>
                </div>
            </div>

            <div class="fw-bold mb-2">${lbl("lblResultsByGrade")}</div>

            ${c.byGrade.map(g => `
                <div class="d-flex align-items-center justify-content-between p-3 mb-2 rounded" style="background:#f8f8f8">
                    <strong>${escapeHtml(g.gradeLevelName)}</strong>
                    <div style="width:250px">${progress(g.percentage, g.rate)}</div>
                    <strong style="color:#97133f">${fmt(g.average)}</strong>
                    ${badge(g.rate)}
                </div>
            `).join("")}
        `;

        bootstrap.Modal.getOrCreateInstance(
            document.getElementById("CriteriaDetailsModal")
        ).show();
    };

    function summaryCard(label, value, sub) {
        return `
            <div class="fa-summary-card">
                <div class="fa-label">${escapeHtml(label)}</div>
                <div class="fa-value">${escapeHtml(value)}</div>
                <div class="text-muted small">${escapeHtml(sub || "")}</div>
            </div>
        `;
    }

    function chart(items, nameKey) {
        if (!items.length) {
            return `<div class="text-muted text-center p-4">${lbl("lblNoData")}</div>`;
        }

        return items
            .filter(x => x.average > 0)
            .map(x => `
                <div class="fa-chart-row">
                    <div class="fa-chart-label">${escapeHtml(x[nameKey])}</div>
                    <div class="fa-chart-track">
                        <div class="fa-chart-fill"
                             style="width:${num(x.percentage)}%;background:${rateColor(x.rate)}">
                            ${fmt(x.average)}
                        </div>
                    </div>
                    <div style="width:50px;color:#97133f;font-weight:700">
                        ${fmt(x.percentage)}%
                    </div>
                </div>
            `).join("");
    }

    function progress(pct, rate) {
        if (!pct) return "—";

        return `
            <div class="d-flex align-items-center gap-2">
                <span style="width:45px">${Math.round(pct)}%</span>
                <div class="fa-bar-track">
                    <div class="fa-bar-fill"
                         style="width:${num(pct)}%;background:${rateColor(rate)}">
                    </div>
                </div>
            </div>
        `;
    }

    function badge(rate) {
        if (!rate || rate === "—") return "—";
        return `<span class="fa-badge ${rate.key}">${escapeHtml(rate.text)}</span>`;
    }

    function getRate(avg) {

        avg = num(avg);

        const matrix = (_data.matrixValues || [])
            .find(x => avg >= x.minValue && avg <= x.maxValue);

        if (!matrix)
            return { key: "fa-wk", text: "—" };

        const name = isAr
            ? (matrix.nameAr || matrix.nameEn)
            : (matrix.nameEn || matrix.nameAr);

        return {
            key: getRateCss(name),
            text: name
        };
    }
    function getRateCss(rateName) {

        switch ((rateName || "").toString().trim()) {

            case "5":
            case "ممتاز":
            case "Excellent":
                return "fa-ex";

            case "4":
            case "جيد جداً":
            case "Very Good":
                return "fa-vg";

            case "3":
            case "جيد":
            case "Good":
                return "fa-g";

            case "2":
            case "مقبول":
            case "Acceptable":
                return "fa-ac";

            case "1":
            case "ضعيف":
            case "Weak":
                return "fa-wk";

            default:
                return "fa-wk";
        }
    }
    function rateColor(rate) {
        switch (rate?.key) {
            case "fa-ex": return "#1D9E75";
            case "fa-vg": return "#378ADD";
            case "fa-g": return "#EF9F27";
            case "fa-ac": return "#E24B4A";
            case "fa-wk": return "#999";
            default: return "#ccc";
        }
    }

    function groupBy(arr, keySelector) {
        const map = new Map();

        arr.forEach(item => {
            const key = keySelector(item) || "none";

            if (!map.has(key))
                map.set(key, []);

            map.get(key).push(item);
        });

        return [...map.entries()].map(([key, items]) => ({ key, items }));
    }

    function average(arr) {
        const valid = arr.filter(x => x > 0);
        if (!valid.length) return 0;

        return valid.reduce((a, b) => a + b, 0) / valid.length;
    }

    function fmt(v) {
        if (v === null || v === undefined || isNaN(v)) return "0.00";
        return Number(v).toFixed(2);
    }

    function dash(v) {
        if (v === null || v === undefined || isNaN(v) || Number(v) <= 0)
            return "—";

        return fmt(v);
    }

    function num(v) {
        if (v === null || v === undefined || isNaN(v)) return 0;
        return Number(v);
    }

    function shortName(name) {
        if (!name) return "-";
        return name.length > 18 ? name.substring(0, 18) + "..." : name;
    }

    function escapeHtml(str) {
        return (str ?? "").toString()
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");
    }

})();