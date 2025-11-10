// ============= SCHOOL TABLE MANAGEMENT SYSTEM (REAL API) =============

// Global variables
let allSchools = [];
let filteredSchools = [];
let currentPage = 1;
let pageSize = 10;
let visitTypes = [];

// Initialize when DOM is ready
$(document).ready(function () {
    loadVisitTypes();
    loadSchoolsData();
    initializeSearch();
    initializeFilters();
    //initializeFlatpickr(); // Initialize Flatpickr on page load
});

// ============= INITIALIZE FLATPICKR =============
function initializeFlatpickr() {
    flatpickr(".childDate", {
        mode: "range",
        locale: "ar",
        dateFormat: "Y-m-d",
        allowInput: true,
        onChange: function (selectedDates, dateStr, instance) {
            // Get the school ID from the input's closest row
            const schoolId = $(instance.input).closest('tr').find('.selectRow').data('id');
            console.log('Date selected for school:', schoolId, dateStr);
            // You can save the date here via API if needed
        },
        onReady: function (selectedDates, dateStr, instance) {
            const monthsContainer = instance.calendarContainer.querySelector('.flatpickr-months');

            // Create arrow stack container
            const prev = instance.calendarContainer.querySelector('.flatpickr-prev-month');
            const next = instance.calendarContainer.querySelector('.flatpickr-next-month');
            const arrowStack = document.createElement('div');
            arrowStack.className = 'fp-arrow-stack';
            arrowStack.appendChild(prev);
            arrowStack.appendChild(next);

            // Insert arrow stack at start (left side in RTL)
            monthsContainer.insertBefore(arrowStack, monthsContainer.firstChild);

            // Month/year container remains for dropdowns (right side in RTL)
            const monthYear = monthsContainer.querySelector('.flatpickr-current-month');
            monthsContainer.appendChild(monthYear);

            // Add Apply/Cancel buttons if not already added
            if (!instance.calendarContainer.querySelector('.fp-btns')) {
                const btns = document.createElement('div');
                btns.className = 'fp-btns';

                const cancel = document.createElement('button');
                cancel.type = 'button';
                cancel.className = 'fp-cancel';
                cancel.textContent = 'إلغاء';
                cancel.onclick = (e) => {
                    e.preventDefault();
                    instance.clear();
                    instance.close();
                };

                const apply = document.createElement('button');
                apply.type = 'button';
                apply.className = 'fp-apply';
                apply.textContent = 'تأكيد';
                apply.onclick = (e) => {
                    e.preventDefault();
                    instance.close();
                };

                btns.appendChild(cancel);
                btns.appendChild(apply);
                instance.calendarContainer.appendChild(btns);
            }
        }
    });
}

// ============= LOAD SCHOOLS DATA (REAL API) =============
function loadSchoolsData(page = 1) {
    currentPage = page;

    jqClient().Get(`/School/GetSchools?page=${page}&pageSize=${pageSize}`)
        .done((result) => {
            console.log("Schools data:", result);

            const data = (result && result.result) ? result.result : [];
            allSchools = data;
            filteredSchools = allSchools;

            renderSchoolsTable(filteredSchools);
            const totalRecords = result.totalCount || filteredSchools.length;
            renderPagination(totalRecords);

            // Re-initialize Flatpickr after table is rendered
            initializeFlatpickr();
        })
        .fail((jqXHR, textStatus, err) => {
            console.error('GetAll schools failed', textStatus, err);
        });
}

// ============= RENDER SCHOOLS TABLE =============
function renderSchoolsTable(schools) {
    const tbody = $('#planTable tbody');
    tbody.empty();

    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = Math.min(startIndex + pageSize, schools.length);
    const paginated = schools.slice(startIndex, endIndex);

    if (paginated.length === 0) {
        tbody.append(`<tr><td colspan="7" class="text-center text-muted">لا توجد بيانات</td></tr>`);
        return;
    }

    let rows = '';
    paginated.forEach(function (school) {
        const ratingClass = getRatingClass(school.rating);
        let visitOptions = visitTypes.map(v =>
            `<option value="${v.id}" ${v.name === school.visitType ? 'selected' : ''}>${v.name}</option>`
        ).join('');
        rows += `
        <tr>
            <td>
                <label class="custom-checkbox">
                    <input type="checkbox" class="selectRow" data-id="${school.id}">
                    <span class="checkmark"></span>
                </label>
            </td>
            <td>
                <div class="d-flex align-items-center justify-content-between">
                    <div>
                        <h6>${school.name || '-'}</h6>
                        <div class="square-bullet">
                            <div>${school.level || 'ابتدائية'}</div>
                        </div>
                    </div>
                    <span class="badge ${ratingClass}">${school.rating || ''}</span>
                </div>
            </td>
            <td>
                <input type="text" 
                       class="form-control form-control-sm childDate" 
                       placeholder="اختر تاريخ بداية ونهاية الزيارة"
                       data-school-id="${school.id}"
                       readonly>
            </td>
            <td>${school.lastEvaluationDate || '-'}</td>
            <td>
            <select class="form-select visitTypeSelect" data-school-id="${school.id}">
                    <option value="">اختر نوع الزيارة</option>
                    ${visitOptions}
                </select>
            </td>
            <td>${school.academicYear || '-'}</td>
            <td>
                <p class="m-0">
                    <a href="#" class="text-dark" type="button" data-bs-toggle="modal" data-bs-target="#exampleModal" data-id="${school.id}">
                        <i class="la la-eye"></i>
                    </a>
                </p>
            </td>
        </tr>`;
    });

    tbody.html(rows);
}


// ============= PAGINATION =============
function renderPagination(totalRecords) {
    const totalPages = Math.ceil(totalRecords / pageSize);
    const pagination = $('#dtPagination');
    pagination.empty();

    if (totalPages <= 1) return;

    let html = '<ul class="pagination pagination-sm mb-0">';

    if (currentPage > 1)
        html += `<li class="page-item"><a href="#" class="page-link" data-page="${currentPage - 1}">السابق</a></li>`;

    for (let i = 1; i <= totalPages; i++) {
        const active = i === currentPage ? 'active' : '';
        html += `<li class="page-item ${active}"><a href="#" class="page-link" data-page="${i}">${i}</a></li>`;
    }

    if (currentPage < totalPages)
        html += `<li class="page-item"><a href="#" class="page-link" data-page="${currentPage + 1}">التالي</a></li>`;

    html += '</ul>';
    pagination.html(html);

    pagination.find('.page-link').on('click', function (e) {
        e.preventDefault();
        const page = parseInt($(this).data('page'));
        loadSchoolsData(page);
    });
}

// ============= SEARCH FUNCTIONALITY =============
function initializeSearch() {
    let searchTimeout;
    $('#customSearch').on('input', function () {
        clearTimeout(searchTimeout);
        const term = $(this).val().trim();
        searchTimeout = setTimeout(function () {
            performSearch(term);
        }, 300);
    });
}

function performSearch(term) {
    if (!term) {
        filteredSchools = allSchools;
    } else {
        filteredSchools = allSchools.filter(s =>
            (s.name && s.name.includes(term)) ||
            (s.type && s.type.includes(term))
        );
    }
    currentPage = 1;
    renderSchoolsTable(filteredSchools);
    renderPagination(filteredSchools.length);
}

// ============= FILTER FUNCTIONALITY =============
function initializeFilters() {
    $('#filterForm').on('submit', function (e) {
        e.preventDefault();
        const type = $('#filterType').val();
        const rating = $('#filterRating').val();

        filteredSchools = allSchools.filter(school => {
            return (!type || school.type === type) &&
                (!rating || school.rating === rating);
        });

        currentPage = 1;
        renderSchoolsTable(filteredSchools);
        renderPagination(filteredSchools.length);
    });

    $('#clearFilters').on('click', function () {
        $('#filterForm')[0].reset();
        filteredSchools = allSchools;
        renderSchoolsTable(filteredSchools);
        renderPagination(filteredSchools.length);
    });
}

// ============= HELPERS =============
function getRatingClass(rating) {
    const map = {
        'Perfect': 'bg-success',
        'VeryGood': 'bg-info',
        'Good': 'bg-primary',
        'Aecctable': 'bg-secondary',
        'Week': 'bg-danger'
    };
    return map[rating] || 'bg-light';
}
function loadVisitTypes() {
    jqClient().Get('/School/GetVisits').done(result => {
        const data = (result && result.result) ? result.result : [];
        visitTypes = data;
    }).fail((jqXHR, textStatus, err) => {
        console.error('Get Visits failed', textStatus, err);
    });
}