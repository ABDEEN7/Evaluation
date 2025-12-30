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

    const countOpenClosedInParty = (party) => {
        let open = 0;
        let closed = 0;

        const services = Array.isArray(party?.services) ? party.services : [];
        for (const s of services) {
            const reqs = Array.isArray(s?.requests) ? s.requests : [];
            for (const r of reqs) {
                if (!r) continue;
                if (r.statusISOPen === true) open++;
                else if (r.statusISOPen === false) closed++;
            }
        }

        return { open, closed };
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

        const openText = lang === "ar" ? "غير مكتمل" : "Open";
        const closedText = lang === "ar" ? "مكتمل" : "Closed";

        const $accordion = $(`<div class="accordion" id="${escapeHtml(parentAccordionId)}"></div>`);

        const sorted = parties.slice().sort((a, b) => (a.orderNo ?? 0) - (b.orderNo ?? 0));

        sorted.forEach((party, idx) => {
            const title =
                (lang === "ar" ? party.nameAr : party.nameEn) ||
                party.nameAr || party.nameEn ||
                `Party ${idx + 1}`;

            const headerId = `partyHeader_${idx}`;
            const collapseId = `partyCollapse_${idx}`;

            const services = Array.isArray(party.services) ? party.services : [];
            const { open, closed } = countOpenClosedInParty(party);

            const badgesHtml = `
                <span class="d-inline-flex align-items-center gap-2 ms-2">
                    <span class="badge bg-warning text-dark">${escapeHtml(openText)}: ${open}</span>
                    <span class="badge bg-success">${escapeHtml(closedText)}: ${closed}</span>
                </span>
            `;

            const bodyHtml = services.length
                ? `<ul class="list-group">
                    ${services.map(s => {
                    const sName =
                        (lang === "ar" ? s.nameAr : s.nameEn) ||
                        s.nameAr || s.nameEn || "";

                    const hasRequest = Array.isArray(s.requests) && s.requests.length > 0;
                    const disabledAttr = hasRequest ? "disabled" : "";

                    return `
                            <li class="list-group-item d-flex align-items-center justify-content-between">
                                <span>${escapeHtml(sName)}</span>
                                <button type="button"
                                        class="btn btn-sm btn-primary btn-add-eval-request"
                                        data-service-id="${escapeHtml(s.id)}"
                                       >
                                    + إنشاء طلب
                                </button>
                            </li>
                        `;
                }).join("")}
                   </ul>`
                : `<div class="text-muted">لا توجد خدمات</div>`;

            const expanded = expandFirst && idx === 0;

            $accordion.append(`
                <div class="accordion-item">
                    <h2 class="accordion-header" id="${headerId}">
                        <button class="accordion-button ${expanded ? "" : "collapsed"} d-flex align-items-center justify-content-between"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#${collapseId}"
                                aria-expanded="${expanded ? "true" : "false"}"
                                aria-controls="${collapseId}">
                            <span class="me-2">${escapeHtml(title)}</span>
                            ${badgesHtml}
                        </button>
                    </h2>

                    <div id="${collapseId}"
                         class="accordion-collapse collapse ${expanded ? "show" : ""}"
                         aria-labelledby="${headerId}"
                         data-bs-parent="#${escapeHtml(parentAccordionId)}">
                        <div class="accordion-body">
                            ${bodyHtml}
                        </div>
                    </div>
                </div>
            `);
        });

        $container.append($accordion);
    }

    $(document)
        .off("click", ".btn-add-eval-request")
        .on("click", ".btn-add-eval-request", async function (e) {
            e.preventDefault();
            e.stopPropagation();

            if ($(this).is(":disabled")) return;

            const serviceId = $(this).data("service-id");
            if (!serviceId) return;

            try {
                await InitializeCreateEvaluationPartRequest(serviceId);
                const el = document.getElementById("CreateRequestModal");
                const modal = bootstrap.Modal.getOrCreateInstance(el);
                modal.show();
            } catch (err) {
                console.error("Error initializing evaluation service request", err);
            }
        });

    ns.renderEvaluationParties = renderEvaluationParties;

})(window.formUtility);
