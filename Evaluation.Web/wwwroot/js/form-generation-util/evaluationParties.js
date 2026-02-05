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
        const parentAccordionId = options.parentAccordionId || "customAccordionParties";
        const expandFirst = options.expandFirst === true;
        const lang = options.lang ||  window.currentLang || "ar";

        const $container = $("#" + containerId);
        if (!$container.length) return;

        $container.empty();

        if (!Array.isArray(parties) || parties.length === 0) {
            // Rename: "طلبات" => "استمارات"
            $container.html(`<div class="text-muted">لا توجد استمارات تقييم</div>`);
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
            const gridId = `gridBody_${idx}`;
            const partyId = party.id;

            const services = Array.isArray(party.services) ? party.services : [];
            const { open, closed } = countOpenClosedInParty(party);

            const grouped = services
                .flatMap(s => Array.isArray(s?.requests) ? s.requests : [])
                .reduce((acc, request) => {
                    if (!request) return acc;
                    const key = request.statusId || "unknown";
                    if (!acc[key]) acc[key] = [];
                    acc[key].push(request);
                    return acc;
                }, {});

            const badgesHtml = `
                <span class="badge bg-danger-light ms-auto me-2 fw-semibold br-0">
                    <i class="las la-times fs-14"></i> ${escapeHtml(openText)}: ${open}
                </span>
                <span class="badge bg-success-light me-2 fw-semibold br-0">
                    <i class="la la-check fs-14"></i> ${escapeHtml(closedText)}: ${closed}
                </span>
            `;

            const servicesHtml = services.length
                ? `
                  <div class="table-card rounded overflow-hidden">
                    <div class="table-responsive">
                      <div class="table-header">الخدمات</div>
                      <ul class="list-group list-group-flush">
                        ${services.map(s => {
                    const sName =
                        (lang === "ar" ? s.nameAr : s.nameEn) ||
                        s.nameAr || s.nameEn || "";

                    return `
                              <li class="list-group-item d-flex align-items-center justify-content-between">
                                <span>${escapeHtml(sName)}</span>
                                <button type="button"
                                        class="btn btn-sm btn-primary btn-add-eval-request"
                                        data-service-id="${escapeHtml(s.id)}">
                                  <i class="la la-plus"></i> إنشاء استمارة
                                </button>
                              </li>
                            `;
                }).join("")}
                      </ul>
                    </div>
                  </div>
                `
                : `<div class="text-muted">لا توجد خدمات</div>`;

            const cardsHtml = Object.keys(grouped).length
                ? `
                  <div class="table-card rounded overflow-hidden mt-3">
                    <div class="table-header">الاستمارات حسب الحالة</div>

                    <div class="status-cards-container p-3">
                      ${Object.entries(grouped).map(([statusId, requests]) => {
                    const count = requests.length;
                    const status = requests[0]?.status || "-";

                    const latestDate = requests.reduce((latest, r) => {
                        const created = new Date(r.createDate);
                        const updated = r.updateDate ? new Date(r.updateDate) : created;
                        const currentLatest = updated > created ? updated : created;
                        return currentLatest > latest ? currentLatest : latest;
                    }, new Date(requests[0]?.createDate));

                    return `
                            <div class="status-card"
                                 role="button"
                                 onclick='window.loadRequestsByStatus("${gridId}", ${JSON.stringify(requests)})'>
                              <div class="count">${count}</div>
                              <div class="label">${escapeHtml(status)}</div>
                              <div class="date">${formatEnglishDate(latestDate)}</div>
                            </div>
                          `;
                }).join("")}
                    </div>

                    <div class="table-responsive px-3 pb-3">
                      <table class="table table-bordered text-center align-middle m-0" id="${gridId}">
                        <thead class="table-primary">
                          <tr>
                            <th>الخدمة</th>
                            <th>الحالة</th>
                            <th>المنشئ</th>
                            <th>تاريخ الإنشاء</th>
                          </tr>
                        </thead>
                        <tbody></tbody>
                      </table>
                    </div>

                  </div>
                `
                : ``;

            const expanded = expandFirst && idx === 0;

            $accordion.append(`
              <div class="accordion-item mb-3 rounded">
                <h2 class="accordion-header" id="${headerId}" data-id="${escapeHtml(partyId)}">
                  <button class="accordion-button ${expanded ? "" : "collapsed"} d-flex align-items-center justify-content-between"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target="#${collapseId}"
                          aria-expanded="${expanded ? "true" : "false"}"
                          aria-controls="${collapseId}">

                    <div class="d-flex align-items-center gap-2 fs-18">
                      <i class="las la-layer-group text-primary fs-25"></i>
                      <span class="fw-semibold">${escapeHtml(title)}</span>
                    </div>

                    ${badgesHtml}

                    <span class="toggle-icon"><i class="la la-angle-up fs-22"></i></span>
                  </button>
                </h2>

                <div id="${collapseId}"
                     class="accordion-collapse collapse ${expanded ? "show" : ""}"
                     aria-labelledby="${headerId}"
                     data-bs-parent="#${escapeHtml(parentAccordionId)}">
                  <div class="accordion-body">

                    ${servicesHtml}

                    ${cardsHtml}

                  </div>
                </div>
              </div>
            `);
        });

        $container.append($accordion);
    }

    function formatEnglishDate(timestamp) {
        if (!timestamp) return "-";

        let dateObj;
        if (typeof timestamp === "string") {
            const cleanTimestamp = timestamp.includes(".") ? timestamp.split(".")[0] : timestamp;
            dateObj = new Date(cleanTimestamp);
        } else if (timestamp instanceof Date) {
            dateObj = timestamp;
        } else {
            return "-";
        }

        const dateStr = new Intl.DateTimeFormat("en-US", {
            day: "numeric",
            month: "long",
            year: "numeric"
        }).format(dateObj);

        const timeStr = new Intl.DateTimeFormat("en-US", {
            hour: "numeric",
            minute: "numeric",
            hour12: true
        }).format(dateObj);

        return `<div>
          <i class="las la-regular las la-calendar"></i>
          <span> ${dateStr} </span>
          <i class="las la-regular las la-clock"></i>
          <span> ${timeStr} </span>
        </div>`;
    }

    window.loadRequestsByStatus = function (gridId, requests) {
        const $tbody = $("#" + gridId + " tbody");
        $tbody.empty();

        let rows = "";

        (requests || []).forEach(r => {
            rows += `
                <tr class="cursor-pointer" onclick="openRequestDetails('${r.id}')">
                <td>${escapeHtml(r.service)}</td>
                <td>${escapeHtml(r.status)}</td>
                <td>${escapeHtml(r.createBy)}</td>
                <td>${formatDate(r.createDate)}</td>
              </tr>
            `;
        });

        $tbody.html(rows);
    };

    window.openRequestDetails = function (requestId) {
        const options = {
            success: function (response) {

                window.formUtility = window.formUtility || {};
                formUtility.attachments = response.attachments || [];
                formUtility.renderPreviewView(
                    'request-details-container',
                    response.formGroups,
                    response.actions,
                    response.actionTransactions,
                    response.attachments,
                    {
                        actionsContainerId: 'Request-actions-container',
                        templateContainerId: 'Request-divTemplates',
                        modalContainerId: 'Request-Action-container-fields',
                        requestId: requestId,
                        serviceId: response.serviceId,
                        ctx: { root: '#RequestModal' }
                    }
                );

                formUtility.addQueryParameter('id', requestId)
                formUtility.addQueryParameter('serviceId', response.serviceId)

                $('#RequestModalLabel').text(response.status || '');
                $('#RequestNoText').text(response.requestNumber || '');

                $('#RequestModal').modal('show');

                $('#RequestModal')
                    .off('shown.bs.modal.redraw')
                    .on('shown.bs.modal.redraw', function () {
                        if (window.Tabulator?.findTable) {
                            Tabulator.findTable("#RequestModal .tabulator")
                                .forEach(t => t.redraw(true));
                        }
                    });
            }
        };

        jqClient(options).Get(`/ServiceRequest/${departmentRoutePath}/GetApplicationDetails?requestId=${requestId}`);
    }
    function formatDate(timestamp) {
        if (!timestamp) return "-";
        const dateObj = new Date(timestamp.split(".")[0]);
        const day = String(dateObj.getDate()).padStart(2, "0");
        const month = String(dateObj.getMonth() + 1).padStart(2, "0");
        const year = dateObj.getFullYear();
        return `${day}/${month}/${year}`;
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
