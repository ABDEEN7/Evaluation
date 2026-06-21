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
    Dropzone.autoDiscover = false;
    window.AddFileClick = function (partyId, requestId, isEvaluationRequestOpen) {

        if (!window.isEvaluationRequestOpen) {
            notificationUtil.error(
                uiControlsSetup().GetUiControlText("msgCannotAddSupportingFiles")
            );
            return;
        }
        $("#EvaluationfileModal").modal('show');
        $("#EvaluationfileSectionRequestId").val(requestId);

        const $tree = $('#EvaluationFileScopetree');

        if ($tree.jstree(true)) {
            $tree.jstree("destroy");
            $tree.empty();
        }

        const options = {
            success: function (response) {
                if (!response || response.length === 0) {
                    notificationUtil.error("No scopes found");
                    return;
                }

                console.log("Scopes:", response);

                $tree.jstree({
                    core: {
                        data: response,
                        check_callback: true
                    },
                    themes: {
                        dots: true,
                        icons: true
                    },
                    plugins: ["wholerow"]
                });

                $tree.on("ready.jstree", function () {
                    $tree.jstree("open_all");
                });
            }
        };

        jqClient(options).Get(`/ServiceRequest/${departmentRoutePath}/GetScopes?partyId=${partyId}`);


        const element = document.querySelector("#EvaluationfileSection");

        if (element.dropzone) {
            element.dropzone.destroy();
        }
        Dropzone.autoDiscover = false;

        const myDropzone = new Dropzone("#EvaluationfileSection", {
            url: decodeURIComponent(sharedUtility().BaseApiUrl()) + `/ServiceRequest/${departmentRoutePath}/SaveSupportFiles`,
            autoProcessQueue: false,
            maxFiles: 1,
            maxFilesize: 5,
            addRemoveLinks: true,
            headers: sharedUtility().SharedHeader(true),
            init: function () {
                var self = this;

                document.getElementById("btnSaveEvaluationFileScope")
                    .addEventListener("click", function (e) {
                        e.preventDefault();
                        e.stopPropagation();

                        if (self.getQueuedFiles().length === 0) {
                            notificationUtil.error("Please upload file");
                            $("#EvaluationfileModal").modal('show');
                            return;
                        }

                        if ($('#EvaluationFileScopetree').jstree('get_selected').length === 0) {
                            notificationUtil.error("Please Select Scope");
                            $("#EvaluationfileModal").modal('show');
                            return;
                        }

                        self.processQueue();
                    });

                this.on("sending", function (file, xhr, formData) {
                    formData.append("EvaluationRequestId",
                        $("#EvaluationfileSectionRequestId").val());

                    formData.append("ScopeId",
                        $('#EvaluationFileScopetree').jstree('get_selected')[0]);
                });

                this.on("success", function () {
                    notificationUtil.success("Uploaded successfully");
                    $("#EvaluationfileModal").modal('hide');
                    self.removeAllFiles(true);
                    GetSupportedFiles($("#EvaluationfileSectionRequestId").val());
                });

                this.on("error", function () {
                    notificationUtil.error("Upload failed");
                    $("#EvaluationfileModal").modal('show');
                    self.removeAllFiles(true);
                });
            }
        });

    };
    function getFileIcon(fileName) {
        if (!fileName) return 'las la-file';

        const extension = fileName.split('.').pop().toLowerCase();

        const imageExtensions = ['png', 'jpg', 'jpeg', 'gif', 'bmp', 'webp'];
        const pdfExtensions = ['pdf'];
        const wordExtensions = ['doc', 'docx'];
        const excelExtensions = ['xls', 'xlsx'];

        if (imageExtensions.includes(extension))
            return 'las la-file-image text-warning';

        if (pdfExtensions.includes(extension))
            return 'las la-file-pdf text-danger';

        if (wordExtensions.includes(extension))
            return 'las la-file-word text-primary';

        if (excelExtensions.includes(extension))
            return 'las la-file-excel text-success';

        return 'las la-file';
    }


    function GetSupportedFiles(requestId) {
        const options = {
            success: function (response) {
                const container = $('#filedivid');
                container.empty();

                if (!response || response.length === 0) return;

                const groupedByScope = response.reduce((acc, file) => {
                    const scopeName = file.scopeName || file.scope || "بدون مجال";
                    if (!acc[scopeName]) acc[scopeName] = [];
                    acc[scopeName].push(file);
                    return acc;
                }, {});

                Object.entries(groupedByScope).forEach(([scopeName, files]) => {
                    const scopeHtml = `
                    <div class="w-100 mb-3">
                        <div class="fw-bold mb-2 text-primary">
                            ${escapeHtml(scopeName)}
                        </div>

                        <div class="d-flex flex-wrap gap-3">
                            ${files.map(file => {
                        const iconClass = getFileIcon(file.uiFileName);

                        return `
                                    <div class="file-box text-center p-2 border rounded">
                                        <a href="${file.fileUrl}" target="_blank">
                                            <i class="${iconClass} fa-3x mb-2"></i>
                                            <div class="file-name">${escapeHtml(file.uiFileName)}</div>
                                        </a>
                                    </div>
                                `;
                    }).join("")}
                        </div>
                    </div>
                `;

                    container.append(scopeHtml);
                });
            }
        };

        jqClient(options)
            .Get(`/ServiceRequest/${departmentRoutePath}/GetSupportedFiles?requestId=${requestId}`);
    }

    function renderEvaluationParties(parties, requestId, options = {}) {
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
            const showOutputsAnalysis = party?.evalPartyCategory?.toLowerCase() === "outputsanalysis";

            const grouped = services
                .flatMap(s => Array.isArray(s?.requests) ? s.requests : [])
                .reduce((acc, request) => {
                    if (!request) return acc;
                    const key = request.statusId || "unknown";
                    if (!acc[key]) acc[key] = [];
                    acc[key].push(request);
                    return acc;
                }, {});

            const badgesHtml = party.isSupportFiles || showOutputsAnalysis ? '' : `
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
                      <div class="table-header">${uiControlsSetup().GetUiControlText('lblServices')}</div>
                      <ul class="list-group list-group-flush">
                        ${services.map(s => {
                    const sName =
                        (lang === "ar" ? s.nameAr : s.nameEn) ||
                        s.nameAr || s.nameEn || "";

                            return `
                                  <li class="list-group-item d-flex align-items-center justify-content-between">
                                    <span>${escapeHtml(sName)}</span>

                                    ${s.canCreate === true ? `
                                        <button type="button"
                                                class="btn btn-sm btn-primary btn-add-eval-request"
                                                data-service-id="${escapeHtml(s.id)}">
                                                <i class="la la-plus"></i> ${uiControlsSetup().GetUiControlText('lblCreateEvaluationForm')}                                        </button>
                                    ` : ``}

                                  </li>
                                `;
                }).join("")}
                      </ul>
                    </div>
                  </div>
                `
                : `<div class="text-muted">${uiControlsSetup().GetUiControlText('lblNoServices')}</div>`;

            const cardsHtml = Object.keys(grouped).length
                ? `
                  <div class="table-card rounded overflow-hidden mt-3">
                    <div class="table-header">
                        ${uiControlsSetup().GetUiControlText('lblEvaluationFormsByStatus')}
                    </div>
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
                            <th>${uiControlsSetup().GetUiControlText('lblService')}</th>
                            <th>${uiControlsSetup().GetUiControlText('lblStatus')}</th>
                            <th>${uiControlsSetup().GetUiControlText('lblCreatedBy')}</th>
                            <th>${uiControlsSetup().GetUiControlText('lblCreationDate')}</th>
                          </tr>
                        </thead>
                        <tbody></tbody>
                      </table>
                    </div>
                  </div>
                `
                : ``;
            const showFormAnalysis =party?.evalPartyCategory?.toLowerCase() === "classroomobservation";
            const formAnalysisHtml = showFormAnalysis? `<div class="mb-3 text-end">
                                        <a href="javascript:void(0)"
                                           class="btn btn-sm btn-outline-primary"
                                           onclick="openFormAnalysis('${requestId}')">
                                            <i class="las la-chart-bar"></i>
                                            ${uiControlsSetup().GetUiControlText('lblFormAnalysis')}
                                        </a>
                                    </div>`: '';

            
            const outputsAnalysisHtml = showOutputsAnalysis? `
                                        <div class="text-end">
                                            <a href="javascript:void(0)"
                                               class="btn btn-sm btn-outline-success"
                                               onclick="openOutputsAnalysis('${requestId}')">
                                                <i class="las la-chart-line"></i>
                                                ${uiControlsSetup().GetUiControlText('lblOutputsAnalysis')}
                                            </a>
                                        </div>`: '';
            const expanded = expandFirst && idx === 0;
            var filedivid = "Filediv_" + partyId;
            const filesHTML = `
           <div class="mb-3">
           ${options.isEvaluationRequestOpen === true ? `
                    <button type="button"
                            class="btn btn-sm btn-primary btn-add-support-file"
                            data-party-id="${escapeHtml(partyId)}"
                            onclick="AddFileClick('${partyId}','${requestId}', true)">
                        <i class="la la-plus"></i> ${uiControlsSetup().GetUiControlText('lblAddFiles')}
                    </button>
                                    ` : `
                                       <button type="button"
                            class="btn btn-sm btn-secondary"
                            disabled>
                        <i class="la la-lock"></i>
                        ${uiControlsSetup().GetUiControlText('lblAddFiles')}
                    </button>
                `}
                           </div>    
                  <div class="table-card rounded overflow-hidden">
                    <div class="table-responsive">
                      <div class="table-header">All Files</div>
                      <ul class="list-group list-group-flush">
                        <li class="list-group-item d-flex align-items-center justify-content-between">
                              
                                <div id="filedivid" class="file-container d-flex flex-wrap gap-3">
                              </li>
                      </ul>
                    </div>
                  </div>
                `;
          
            if (party.isSupportFiles) {
                GetSupportedFiles(requestId);
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

                     ${filesHTML}

                  </div>
                </div>
              </div>
            `);
               
            }
            else if (showOutputsAnalysis) {

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
                        <i class="las la-chart-line text-danger fs-25"></i>
                        <span class="fw-semibold">
                            ${escapeHtml(title)}
                        </span>
                    </div>

                    <span class="toggle-icon">
                        <i class="la la-angle-up fs-22"></i>
                    </span>

                </button>

            </h2>

            <div id="${collapseId}"
                 class="accordion-collapse collapse ${expanded ? "show" : ""}"
                 aria-labelledby="${headerId}"
                 data-bs-parent="#${escapeHtml(parentAccordionId)}">

                <div class="accordion-body text-center">

                    <button type="button"
                            class="btn btn-success btn-lg"
                            onclick="openOutputsAnalysis('${requestId}')">

                        <i class="las la-chart-line me-1"></i>

                        ${uiControlsSetup().GetUiControlText('lblOutputsAnalysis')}

                    </button>

                </div>

            </div>

        </div>
    `);
            }
            else {
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

                      ${formAnalysisHtml}

                    ${servicesHtml}

                    ${cardsHtml}

                  </div>
                </div>
              </div>
            `);
            }
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
                        root: '#RequestModal' 
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
