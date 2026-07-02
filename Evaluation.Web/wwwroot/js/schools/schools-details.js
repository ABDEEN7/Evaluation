/**
 * schoolDetailsModal
 * -------------------
 * Standalone JS component that replaces SchoolDetailsModalViewComponent (Razor).
 * Single DOM instance (singleton) — safe to call from any page, any number of times.
 *
 * Usage:
 *   schoolDetailsModal.open(orgId, 'SCHOOL');
 *   schoolDetailsModal.open(employeeId, 'EMPLOYEE');
 *
 * Requires (already in your codebase):
 *   - jqClient()
 *   - uiControlsSetup().GetUiControlText(key)
 *   - evaluationListing.createListing({...})
 *   - departmentRoutePath (global)
 *   - Bootstrap 5 modal (data api / $.modal)
 */

async function GetSchoolDetails(guid) {
    return new Promise((resolve, reject) => {
        jqClient().Get(`/Org/${departmentRoutePath}/GetOrgDetails?OrgID=${guid}`)
            .done((res) => resolve(res))
            .fail((err) => reject(err));
    });
}

async function initSchoolDetailsPage(guid) {
    return await GetSchoolDetails(guid);
}

const schoolDetailsModal = (function () {

    const MODAL_ID = 'schoolDetailsModal';
    let evalRequestsList = null;
    let isShellRendered = false;

    function label(key) {
        return uiControlsSetup().GetUiControlText(key);
    }

    // ---------- Templates ----------

    function schoolTemplate() {
        return `
            <div class="col-md-6">
                <div class="card card-table">
                    <div class="card-body">
                        <h5 class="fw-bold mb-4 @ConstantKeys.WebAppOrgDetails.lblOrgDetailsBasicInformation"></h5>
                        <div>
                            <div class="row my-3">
                                <div class="col-md-6">
                                    <h6 class="fw-bold">
                                        <span class="primary-bg">
                                            <i class="las la-user color-primary"></i>
                                        </span>

                                        <span class="mx-2 @ConstantKeys.WebAppOrgDetails.lblOrgDetailsManager"></span>
                                    </h6>
                                </div>
                                <div class="col-md-6">
                                    <h6 class="text-light" id="managerName@(suffix)">
                                    </h6>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-md-6">
                                    <h6 class="fw-bold">
                                        <span class="primary-bg">
                                            <i class="las la-calendar-check color-primary"></i>
                                        </span>

                                        <span class="mx-2 @ConstantKeys.WebAppOrgDetails.lblOrgDetailsEstablishmentDate"></span>
                                    </h6>
                                </div>
                                <div class="col-md-6 justify-items-end">
                                    <h6 class="text-light" id="establishmentDate@(suffix)"></h6>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-md-6">
                                    <h6 class="fw-bold">
                                        <span class="primary-bg">
                                            <i class="las la-user-friends color-primary"></i>
                                        </span>

                                        <span class="mx-2 @ConstantKeys.WebAppOrgDetails.lblOrgDetailsLevels"></span>
                                    </h6>
                                </div>
                                <div class="col-md-6 justify-items-end">
                                    <h6 class="text-light" id="teachers@(suffix)"></h6>
                                </div>
                            </div>

                        </div>
                        <hr class="my-4">
                        <div class=" d-flex gap-2 align-items-center mb-3">
                            <span class="soft-gray-bg mp2">
                                <i class="las la-phone-volume color-primary"></i>
                            </span>
                            <div><a id="phone@(suffix)" href="#" class="text-dark"></a></div>
                        </div>

                        <div class=" d-flex gap-2 align-items-center mb-3">
                            <span class="soft-gray-bg mp2">
                                <i class="las la-envelope color-primary"></i>
                            </span>
                            <div><a id="email@(suffix)" href="" class="text-dark"></a></div>
                        </div>

                        <div class=" d-flex gap-2 align-items-center mb-3">
                            <span class="soft-gray-bg mp2">
                                <i class="las la-location-arrow color-primary"></i>
                            </span>
                            <div><p id="address@(suffix)" class="text-dark m-0"></p></div>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    function employeeTemplate() {
        return `
          <div class="col-md-12">
                <div class="card card-table mb-3">
                    <div class="card-body">
                        <h5 class="fw-bold mb-4">معلومات أساسية</h5>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <p class="text-light">الرقم الوظيفي</p>
                                <h5 class="fw-bold" id="employeeNo@(suffix)"></h5>
                            </div>
                            <div class="col-md-6 mb-3">
                                <p class="text-light">تاريخ الميلاد</p>
                                <h5 class="fw-bold" id="birthDate@(suffix)"></h5>
                            </div>
                            <div class="col-md-6 mb-3">
                                <p class="text-light">الجنسية</p>
                                <h5 class="fw-bold" id="nationalityCode@(suffix)"></h5>
                            </div>
                            <div class="col-md-6 mb-3">
                                <p class="text-light">تاريخ الالتحاق</p>
                                <h5 class="fw-bold" id="joinDate@(suffix)"></h5>
                            </div>
                        </div>

                        <hr class="mt-0">
                        <div class="row">
                            <div class="col-md-12">
                                <div class=" d-flex gap-2 align-items-center mb-3">
                                    <div class="social-icon"><i class="la la-phone-volume"></i></div>
                                    <div><a id="jobTitle@(suffix)" href="#" class="text-dark"></a></div>
                                </div>
                                <div class=" d-flex gap-2 align-items-center mb-3">
                                    <div class="social-icon"><i class="la la-envelope"></i></div>
                                    <div><a id="email@(suffix)" href="" class="text-dark"></a></div>
                                </div>
                                <div class=" d-flex gap-2 align-items-center mb-3">
                                    <div class="social-icon"><i class="la la-location-arrow"></i></div>
                                    <div><p id="qID@(suffix) class="text-dark m-0"></p></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    function statsCardTemplate() {
        // NOTE: percentage bars / pill colors (success|info|danger) and the second
        // history record ("السابق — 2") are placeholders — the API doesn't return
        // a numeric score or status category yet. Only the two dynamic text/date
        // fields below (current + last evaluation) are wired to real data.
        return `
            <div class="card col-md-6">
            <div class="stats-ui-card">

                <div class="stats-section">
                    <div class="stats-header">
                        <div class="stats-title">
                            <i class="las la-chart-bar"></i>
                            <!-- <span class="@ConstantKeys.WebAppOrgDetails.lblOrgDetailsCurrentSituation"></span>-->
                            <span>التقييم الحالي</span>
                        </div>
                    </div>

                    <div class="stats-status-row">
                        <div class="stats-pill stats-pill-success">
                            <!--   <span id="current_evaluation_value@(suffix)"></span>-->
                            <span>ممتاز</span>
                            <span class="stats-dot"></span>
                        </div>
                        <div class="stats-improve-pill">
                            <i class="las la-arrow-up"></i>
                            تحسن كبير
                        </div>
                    </div>

                    <div class="stats-progress-row">
                        <div class="stats-progress">
                            <div class="stats-progress-fill stats-success-fill" style="width: 95%;"></div>
                        </div>
                        <span class="stats-percent">95%</span>
                    </div>

                    <div class="stats-date">
                        <!--  <span id="current_evaluation_date@(suffix)"></span>-->
                        <i class="las la-calendar"></i>
                        <span>10-02-2025</span>
                    </div>
                </div>

                <div class="stats-section">
                    <div class="stats-history-title">
                        <i class="las la-history"></i>
                        <span>آخر تقييمين</span>
                    </div>

                    <div class="stats-history-grid">

                        <div class="stats-history-card">
                            <div class="stats-history-top">
                                <span>السابق — 1</span>

                                <div class="stats-pill stats-pill-danger">
                                    <span class="stats-dot"></span>
                                    <span>ضعيف</span>
                                </div>
                            </div>

                            <div class="stats-progress-row">

                                <div class="stats-progress">
                                    <div class="stats-progress-fill stats-danger-fill" style="width: 32%;"></div>
                                </div>
                                <span class="stats-percent">32%</span>
                            </div>

                            <div class="stats-date">
                                <i class="las la-calendar"></i>
                                <span>10-02-2024</span>

                            </div>
                        </div>

                        <div class="stats-history-card">
                            <div class="stats-history-top">
                                <span>السابق — 2</span>

                                <div class="stats-pill stats-pill-info">
                                    <span class="stats-dot"></span>
                                    <span>جيد</span>
                                    <!--<span id="last_evaluation_value@(suffix)"></span>-->
                                </div>
                            </div>

                            <div class="stats-progress-row">

                                <div class="stats-progress">
                                    <div class="stats-progress-fill stats-info-fill" style="width: 65%;"></div>
                                </div>
                                <span class="stats-percent">65%</span>
                            </div>

                            <div class="stats-date">
                                <i class="las la-calendar"></i>
                                <!--<span id="last_evaluation_date@(suffix)"></span>-->
                                <span>10-02-2023</span>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
        `;
    }

    function schoolRequestsAccordionTemplate() {
        return `
            <div class="accordion" id="customAccordion">
            <div class="accordion-item mb-3 rounded">
                <h2 class="accordion-header">
                    <button class="accordion-button d-flex align-items-center justify-content-between"
                            type="button" data-bs-toggle="collapse"
                            data-bs-target="#schoolEvalRequests" aria-expanded="true">
                        <div class="d-flex align-items-center gap-2 fs-18">
                            <i class="la la-building text-primary fs-25"></i>
                            <span class="fw-semibold @ConstantKeys.WebAppRequest.lblSchoolEvalRequests"></span>
                        </div>
                        <span class="toggle-icon"><i class="la la-angle-up fs-22"></i></span>
                    </button>
                </h2>
                <div id="schoolEvalRequests" class="accordion-collapse collapse show">
                    <div class="accordion-body">
                        <div class="table-responsive" id="evalRequestsTableWrapper">
                            <table id="evalRequestsTable" class="table table-striped table-bordered w-100 application-request card">
                                <thead>
                                    <tr>
                                        <th class=" @ConstantKeys.WebAppRequest.lblRequestNumber"></th>
                                        <th class=" @ConstantKeys.WebAppRequest.lblStatus"></th>
                                        <th class=" @ConstantKeys.WebAppRequest.lblFromDate"></th>
                                        <th class=" @ConstantKeys.WebAppRequest.lblToDate"></th>
                                        <th class=" @ConstantKeys.WebAppRequest.lblEvaluationDate"></th>
                                        <th class=" @ConstantKeys.WebAppRequest.lblNextEvaluationDate"></th>
                                        <th class=" @ConstantKeys.WebAppRequest.lblEvaluationResult"></th>
                                        <th class=""></th>
                                    </tr>
                                </thead>
                                <tbody class="requeststableBody w-100"></tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        `;
    }

    function buildContent(type, includeAccordion) {
        let html = statsCardTemplate();
        html += type === 'SCHOOL' ? schoolTemplate() : employeeTemplate();

        // Accordion shows for both types, but only in modal context (isModal = true
        // in the old Razor version). Table binding still only runs for SCHOOL,
        // since that's the only type with a working eval-requests endpoint.
        if (includeAccordion) {
            html += schoolRequestsAccordionTemplate();
        }
        return html;
    }

    // ---------- Shell (rendered once, reused forever) ----------

    function ensureModalShell() {
        if (isShellRendered || $(`#${MODAL_ID}`).length) {
            isShellRendered = true;
            return;
        }

        const shellHtml = `
        <div class="modal fade" id="${MODAL_ID}" tabindex="-1" aria-labelledby="${MODAL_ID}Label" aria-hidden="true">
            <div class="modal-dialog modal-fullscreen modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header align-items-start border-0">
                        <div>
                            <h4 class="modal-title fw-semibold mb-2" id="${MODAL_ID}_title"></h4>
                        </div>

                        <button type="button" id="${MODAL_ID}_closeBtn" class="btn btn-outline-secondary btn-sm" data-bs-dismiss="modal" aria-label="Close">
                            <i class="la la-close me-1 fs-14"></i><span class="close-text">Close</span>
                        </button>
                    </div>
                    <div class="modal-body py-0">
                        <div class="row" id="${MODAL_ID}_content"></div>
                    </div>
                </div>
            </div>
        </div>
    `;

        $('body').append(shellHtml);

        $(document).on('click', `#${MODAL_ID}_closeBtn`, function () {
            window.history.back();
        });

        isShellRendered = true;
    }

    // ---------- Data binding ----------

    function fillSchoolData(details, hasAccordion) {
        $(`#${MODAL_ID}_title`).text(details.name);
        $('#managerName').text(details.managerName);
        $('#establishmentDate').text(details.establishmentDate);
        $('#phone').text(details.phone);
        $('#email').text(details.orgEmail);
        $('#address').text(details.address);
        $('#teachers').text('Primary, Preparatory');

        $('#current_evaluation_date').text(details.currentEvaluationDate);
        $('#current_evaluation_value').text(details.currentEvaluationResult);
        $('#last_evaluation_date').text(details.lastEvaluationDate);
        $('#last_evaluation_value').text(details.lastEvaluationResult);

        if (hasAccordion) {
            bindEvalRequestsTable(details.id);
        }
    }

    function fillEmployeeData(details) {
        $(`#${MODAL_ID}_title`).text(details.name);
        $('#employeeNo').text(details.employeeNo);
        $('#birthDate').text(details.birthDate);
        $('#nationalityCode').text(details.nationalityCode);
        $('#joinDate').text(details.joinDate);
        $('#jobTitle').text(details.jobTitle);
        $('#email').text(details.email);
        $('#qID').text(details.qID);

        $('#current_evaluation_date').text(details.currentEvaluationDate);
        $('#current_evaluation_value').text(details.currentEvaluationResult);
        $('#last_evaluation_date').text(details.lastEvaluationDate);
        $('#last_evaluation_value').text(details.lastEvaluationResult);
    }

    function bindEvalRequestsTable(orgId) {
        evalRequestsList = evaluationListing.createListing({
            tableId: 'evalRequestsTable',
            ajaxUrl: `/EvaluationRequest/${departmentRoutePath}/GetEvaluationRequestsBySchoolId?Id=${orgId}`,
            enableCardView: false,
            columns: [
                {
                    data: 'requestNumber',
                    title: label('lblRequestNumber'),
                    className: 'td-left name',
                    render: function (data, type, row) {
                        const safe = data || '';
                        return `<a href="javascript:void(0)" class="text-decoration-none text-primary fw-bold school-details-link" data-id="${row.id}">${safe}</a>`;
                    }
                },
                { data: 'status', title: label('lblStatus'), className: 'td-left code' },
                { data: 'fromDate', title: label('lblFromDate'), className: 'td-left code' },
                { data: 'toDate', title: label('lblToDate'), className: 'td-left code' },
                { data: 'evaluationDate', title: label('lblEvaluationDate'), className: 'td-left code' },
                { data: 'nextEvaluationDate', title: label('lblNextEvaluationDate'), className: 'td-left code' },
                { data: 'evaluationResult', title: label('lblEvaluationResult'), className: 'td-left code' },
                {
                    data: null,
                    orderable: false,
                    searchable: false,
                    render: function (data, type, row) {
                        return `<button class="btn btn-sm btn-primary" onclick="openEvaluationRequestDetails('${row.id}')">
                                    ${label('lblEvaluationRequestDetails')}
                                </button>`;
                    }
                }
            ]
        });

        evalRequestsList.reload();
    }

    // ---------- Public API ----------

    async function open(id, type = 'SCHOOL') {
        ensureModalShell();

        $(`#${MODAL_ID}_content`).html(buildContent(type, true));

        try {
            const details = await initSchoolDetailsPage(id);

            if (type === 'SCHOOL') {
                fillSchoolData(details, true);
            } else {
                fillEmployeeData(details);
            }

            $(`#${MODAL_ID}`).modal('show');
        } catch (err) {
            console.error('Failed to load details:', err);
        }
    }

    /**
     * Renders the same content inline, directly inside a container on the page
     * (no modal wrapper, no history.back on close — used for isModal = false cases,
     * e.g. inside EvaluationRequestDetails page).
     *
     * @param {string} containerId - id of the target container (without '#')
     * @param {string} id - org/employee id
     * @param {string} type - 'SCHOOL' | 'EMPLOYEE'
     */
    async function renderInline(containerId, id, type = 'SCHOOL') {
        const $container = $(`#${containerId}`);

        if (!$container.length) {
            console.error(`renderInline: container #${containerId} not found`);
            return;
        }

        $container.addClass('row').html(buildContent(type, false));

        try {
            const details = await initSchoolDetailsPage(id);

            if (type === 'SCHOOL') {
                fillSchoolData(details, false);
            } else {
                fillEmployeeData(details);
            }
        } catch (err) {
            console.error('Failed to load details:', err);
        }
    }
    $(document).on('click', '.school-details-view-btn', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        const type = $(this).data('type') || 'SCHOOL';
        open(id, type);
    });
    return { open, renderInline };

})();