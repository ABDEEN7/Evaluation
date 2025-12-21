(function (ns) {
    "use strict";

    if (!ns) return;

    const escapeHtml = (str) => {
        return (str ?? "").toString()
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");
    };

    function renderEvaluationParties(parties, options = {}) {
        const containerId = options.containerId || "evaluationPartiesContainer";
        const parentAccordionId = options.parentAccordionId || "evaluationRootAccordion";
        const expandFirst = options.expandFirst === true;
        const lang = options.lang || ns.lang || window.currentLang || "en";

        const $container = $("#" + containerId);
        if (!$container.length) return;

        $container.empty();

        if (!Array.isArray(parties) || parties.length === 0) {
            $container.html(`<div class="text-muted">لا توجد مراحل تقييم</div>`);
            return;
        }

        const sorted = parties.slice().sort((a, b) => (a.orderNo ?? 0) - (b.orderNo ?? 0));

        sorted.forEach((party, idx) => {
            const title =
                (lang === "ar" ? party.nameAr : party.nameEn) ||
                party.nameAr || party.nameEn ||
                `Party ${idx + 1}`;

            const headerId = `partyHeader_${idx}`;
            const collapseId = `partyCollapse_${idx}`;

            const services = Array.isArray(party.services) ? party.services : [];
            const bodyHtml = services.length
                ? `<ul class="list-group">
                    ${services.map(s => {
                    const sName =
                        (lang === "ar" ? s.nameAr : s.nameEn) ||
                        s.nameAr || s.nameEn || "";
                    return `<li class="list-group-item">${escapeHtml(sName)}</li>`;
                }).join("")}
                   </ul>`
                : `<div class="text-muted">لا توجد خدمات</div>`;

            const expanded = expandFirst && idx === 0;

            $container.append(`
                <div class="accordion-item">
                    <h2 class="accordion-header" id="${headerId}">
                        <button class="accordion-button ${expanded ? "" : "collapsed"}"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#${collapseId}"
                                aria-expanded="${expanded ? "true" : "false"}"
                                aria-controls="${collapseId}">
                            ${escapeHtml(title)}
                        </button>
                    </h2>

                    <div id="${collapseId}"
                         class="accordion-collapse collapse ${expanded ? "show" : ""}"
                         aria-labelledby="${headerId}"
                         data-bs-parent="#${parentAccordionId}">
                        <div class="accordion-body">
                            ${bodyHtml}
                        </div>
                    </div>
                </div>
            `);
        });
    }

    ns.renderEvaluationParties = renderEvaluationParties;

})(window.formUtility);
