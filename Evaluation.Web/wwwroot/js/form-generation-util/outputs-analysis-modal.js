(function () {
    "use strict";

    const modalId = "outputsAnalysisModal";
    const bodyId = "outputsAnalysisBody";

    function getText(key, fallback) {
        try {
            return uiControlsSetup().GetUiControlText(key) || fallback;
        } catch {
            return fallback;
        }
    }

    function escapeHtml(value) {
        return (value ?? "").toString()
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");
    }

    function formatNumber(value, digits = 2) {
        if (value === null || value === undefined || value === "") return "-";
        const n = Number(value);
        if (Number.isNaN(n)) return "-";
        return n.toFixed(digits);
    }

    function getBadgeClass(value) {
        const n = Number(value || 0);

        if (n >= 90) return "bg-success";
        if (n >= 80) return "bg-info";
        if (n >= 70) return "bg-primary";
        if (n >= 50) return "bg-warning text-dark";

        return "bg-danger";
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

                        <div class="modal-body bg-light p-3" id="${bodyId}"></div>
                    </div>
                </div>
            </div>
        `);
    }

    function renderEmpty() {
        return `
            <div class="alert alert-warning">
                لا توجد بيانات تحليل مخرجات لهذا الطلب
            </div>
        `;
    }

    function renderLoading() {
        return `
            <div class="text-center py-5">
                <div class="spinner-border text-primary"></div>
                <div class="mt-2">Loading...</div>
            </div>
        `;
    }

    function renderError() {
        return `
            <div class="alert alert-danger">
                حدث خطأ أثناء تحميل تحليل المخرجات
            </div>
        `;
    }

    function renderSummaryCards(data) {
        return `
            <div class="row g-3 mb-4">
                ${data.map(item => `
                    <div class="col-xl-3 col-lg-4 col-md-6">
                        <div class="card shadow-sm h-100 border-0">
                            <div class="card-body text-center">
                                <div class="fw-bold mb-2">
                                    ${escapeHtml(item.analysisTypeNameAr || item.analysisTypeNameEn || "-")}
                                </div>

                                <div class="display-6 fw-bold mb-2">
                                    ${formatNumber(item.actualValue)}%
                                </div>

                                <span class="badge ${getBadgeClass(item.actualValue)}">
                                    ${escapeHtml(item.note || "-")}
                                </span>
                            </div>
                        </div>
                    </div>
                `).join("")}
            </div>
        `;
    }

    function renderDetailsTable(rows) {
        if (!rows || !rows.length) {
            return `<div class="text-muted">لا توجد تفاصيل</div>`;
        }

        return `
            <div class="table-responsive">
                <table class="table table-bordered table-striped table-hover text-center align-middle mb-0">
                    <thead class="table-light">
                        <tr>
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
                            <th>Note</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${rows.map(row => {
                            const diff = Number(row.difference || 0);

                            return `
                                <tr>
                                    <td>${escapeHtml(row.grade)}</td>
                                    <td>${escapeHtml(row.subjectCode || "-")}</td>
                                    <td>${escapeHtml(row.track || "-")}</td>
                                    <td>${escapeHtml(row.termCode || "-")}</td>
                                    <td>${escapeHtml(row.previousYear)}</td>
                                    <td>${escapeHtml(row.lastYear)}</td>
                                    <td>${formatNumber(row.previousYearValue)}</td>
                                    <td>${formatNumber(row.lastYearValue)}</td>
                                    <td class="${diff >= 0 ? "text-success" : "text-danger"} fw-bold">
                                        ${formatNumber(row.difference)}
                                    </td>
                                    <td>
                                        <span class="badge ${getBadgeClass(row.actualValue)}">
                                            ${formatNumber(row.actualValue)}%
                                        </span>
                                    </td>
                                    <td>${escapeHtml(row.martixTextValue || "-")}</td>
                                    <td>${escapeHtml(row.note || "-")}</td>
                                </tr>
                            `;
                        }).join("")}
                    </tbody>
                </table>
            </div>
        `;
    }

    function renderSection(item, index) {
        const collapseId = `outputsAnalysisCollapse_${index}`;
        const headingId = `outputsAnalysisHeading_${index}`;
        const title = item.analysisTypeNameAr || item.analysisTypeNameEn || "-";

        return `
            <div class="accordion-item mb-3 border rounded shadow-sm">
                <h2 class="accordion-header" id="${headingId}">
                    <button class="accordion-button ${index === 0 ? "" : "collapsed"}"
                            type="button"
                            data-bs-toggle="collapse"
                            data-bs-target="#${collapseId}"
                            aria-expanded="${index === 0 ? "true" : "false"}"
                            aria-controls="${collapseId}">
                        <div class="w-100 d-flex justify-content-between align-items-center pe-3">
                            <span class="fw-bold">
                                ${escapeHtml(title)}
                            </span>

                            <span class="badge ${getBadgeClass(item.actualValue)}">
                                ${formatNumber(item.actualValue)}%
                            </span>
                        </div>
                    </button>
                </h2>

                <div id="${collapseId}"
                     class="accordion-collapse collapse ${index === 0 ? "show" : ""}"
                     aria-labelledby="${headingId}"
                     data-bs-parent="#outputsAnalysisAccordion">
                    <div class="accordion-body bg-white">
                        ${renderDetailsTable(item.details)}
                    </div>
                </div>
            </div>
        `;
    }

    function renderOutputAnalysis(data) {
        if (!Array.isArray(data) || !data.length) {
            return renderEmpty();
        }

        return `
            ${renderSummaryCards(data)}

            <div class="accordion" id="outputsAnalysisAccordion">
                ${data.map((item, index) => renderSection(item, index)).join("")}
            </div>
        `;
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
                $("#" + bodyId).html(renderOutputAnalysis(response));
            },
            error: function () {
                $("#" + bodyId).html(renderError());
            }
        };

        jqClient(options).Get(
            `/ServiceRequest/${departmentRoutePath}/GetOutputAnalysis?evaluationRequestId=${evaluationRequestId}`
        );
    };

})();