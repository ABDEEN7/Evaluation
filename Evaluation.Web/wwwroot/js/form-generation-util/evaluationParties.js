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
            const partyid = party.id;
            const gridId = `gridBody_${idx}`; 

            const grouped = (party.services || [])
                .flatMap(service => service.requests || [])
                .reduce((acc, request) => {
                    const key = request.statusId;

                    if (!acc[key]) acc[key] = [];
                    acc[key].push(request);

                    return acc;
                }, {});

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
           
            const cards = grouped
                ? `
    ${Object.entries(grouped).map(([statusId, requests]) => {
        const latestDate = requests.reduce((latest, r) => {
            // Pick the latest between createdDate and updatedDate
            const created = new Date(r.createDate);
            const updated = r.updateDate ? new Date(r.updateDate) : created; // if null, use createdDate
            const currentLatest = updated > created ? updated : created;

            // Compare with the latest found so far
            return currentLatest > latest ? currentLatest : latest;
        }, new Date(requests[0].createDate));
                   
                    const count = requests.length;
        const status = requests[0].status;
                    return `
        <div class="status-card" style="border-color:'#ccc'"  onclick='window.loadRequestsByStatus("${gridId}",${JSON.stringify(requests)})'>
          <div class="count">${count}</div>
          <div class="label">${status}</div>
          <div class="date">${formatEnglishDate(latestDate)}</div>
        </div>
      `;
                }).join("")}
    `
                : ``;
            const expanded = expandFirst && idx === 0;
           
            $accordion.append(`
                <div class="accordion-item">
                    <h2 class="accordion-header" id="${headerId}" data-id="${partyid}">
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
                     <div id="${collapseId}"
                         class="accordion-collapse collapse ${expanded ? "show" : ""}"
                         aria-labelledby="${headerId}"
                         data-bs-parent="#${escapeHtml(parentAccordionId)}">
                        <div class="accordion-body">
                            ${cards}
                            <table class="gridtable" id="${gridId}">
   <thead>
      <tr>
         <th>Service Name</th>
         <th>Status</th>
         <th>Created By</th>
         <th>Created Date</th>
      </tr>
   </thead>
   <tbody></tbody>
</table>
                        </div>
                        
                    </div>
                </div>
            `);
        });

        $container.append($accordion);
    }

    function formatEnglishDate(timestamp) {
        if (!timestamp) return "-";

        // Convert to Date if it's a string
        let dateObj;
        if (typeof timestamp === "string") {
            // Remove microseconds if present
            const cleanTimestamp = timestamp.includes(".") ? timestamp.split(".")[0] : timestamp;
            dateObj = new Date(cleanTimestamp);
        } else if (timestamp instanceof Date) {
            dateObj = timestamp;
        } else {
            return "-";
        }

        // Format date (English)
        const dateStr = new Intl.DateTimeFormat("en-US", {
            day: "numeric",
            month: "long",
            year: "numeric"
        }).format(dateObj);

        // Format time (English 12-hour)
        const timeStr = new Intl.DateTimeFormat("en-US", {
            hour: "numeric",
            minute: "numeric",
            hour12: true
        }).format(dateObj);

        return `<div>
  <i class="las la-regular las la-calendar"></i> <!-- Calendar Icon -->
  <span> ${dateStr} </span>
  <i class="las la-regular las la-clock"></i> <!-- Clock Icon -->
  <span> ${timeStr} </span>
</div>`;

    }



    window.loadRequestsByStatus = function (gridId,requests) {
        const $tbody = $("#" + gridId + " tbody");
        $tbody.empty();

        let rows = "";

        requests.forEach(r => {
            rows += `
            <tr>
                <td>${r.service}</td>
                <td>${r.status}</td>
                <td>${r.createBy}</td>
                 <td>${formatDate(r.createDate)}</td>
            </tr>
        `;
        });
        $tbody.html(rows)
       
    }
    function formatDate(timestamp) {
        if (!timestamp) return "-";
        // Remove microseconds if present
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
