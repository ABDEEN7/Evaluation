// School Plan Management System - Main JavaScript Handler

// Global variables
let selectedSchools = [];
let allSchools = [];
let currentPage = 1;
let pageSize = 10;
let filterCriteria = {};

// Initialize the system when DOM is ready
$(document).ready(function () {
    initializePlanForm();
    initializeSchoolTable();
    initializeFilters();
    initializeModals();
    loadPlanTypes();
    loadSchoolsData();
});

// ============= PLAN FORM INITIALIZATION =============
function initializePlanForm() {
    // Initialize date range picker
    if (typeof flatpickr !== 'undefined') {
        flatpickr("#dateRange", {
            mode: "range",
            dateFormat: "Y-m-d",
            locale: "ar",
            onChange: function (selectedDates, dateStr, instance) {
                console.log('Date range selected:', dateStr);
            }
        });
    }

    // Initialize Select2 for plan type dropdown
    if (typeof $.fn.select2 !== 'undefined') {
        $('#ddlPlanType').select2({
            placeholder: 'اختر نوع الخطة',
            language: 'ar',
            dir: 'rtl'
        });
    }

    // Form validation
    $('#username, #ddlPlanType, #dateRange').on('change', function () {
        validatePlanForm();
    });
}

// ============= LOAD PLAN TYPES =============
function loadPlanTypes() {
    // Mock data - replace with actual API call
    const planTypes = [
        { id: 1, name: 'خطة دورية' },
        { id: 2, name: 'خطة استثنائية' },
        { id: 3, name: 'خطة زيارة' }
    ];

    let options = '<option value="">اختر نوع الخطة</option>';
    planTypes.forEach(function (type) {
        options += `<option value="${type.id}">${type.name}</option>`;
    });

    $('#ddlPlanType').html(options);
}

// ============= LOAD SCHOOLS DATA =============
function loadSchoolsData(page = 1, filters = {}) {
    currentPage = page;
    filterCriteria = filters;

    // Mock data - replace with actual API call
    const mockSchools = generateMockSchools();

    allSchools = mockSchools;
    renderSchoolsTable(mockSchools);
    renderPagination(mockSchools.length);
}

// Generate mock school data
function generateMockSchools() {
    const schools = [];
    const schoolTypes = ['ابتدائية', 'إعدادية', 'ثانوية'];
    const ratings = ['ممتاز', 'جيد جداً', 'جيد', 'مقبول'];
    const visitTypes = ['دوري', 'استثنائي', 'زيارة'];

    for (let i = 1; i <= 50; i++) {
        schools.push({
            id: i,
            name: `مدرسة ${['آمنة محمود الجيده', 'عائشة بنت أبي بكر', 'خديجة بنت خويلد', 'فاطمة الزهراء'][Math.floor(Math.random() * 4)]} للبنات ${i}`,
            type: schoolTypes[Math.floor(Math.random() * schoolTypes.length)],
            students: Math.floor(Math.random() * 500) + 100,
            rating: ratings[Math.floor(Math.random() * ratings.length)],
            lastEvalDate: generateRandomDate(),
            nextEvalDate: generateRandomDate(true),
            visitType: visitTypes[Math.floor(Math.random() * visitTypes.length)],
            visitDate: null
        });
    }

    return schools;
}

// ============= RENDER SCHOOLS TABLE =============
function renderSchoolsTable(schools) {
    let tbody = '';

    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = Math.min(startIndex + pageSize, schools.length);
    const paginatedSchools = schools.slice(startIndex, endIndex);

    paginatedSchools.forEach(function (school) {
        const isChecked = selectedSchools.includes(school.id) ? 'checked' : '';
        const ratingClass = getRatingClass(school.rating);

        tbody += `
            <tr data-school-id="${school.id}">
                <td>
                    <label class="custom-checkbox">
                        <input type="checkbox" class="row-select" data-school-id="${school.id}" ${isChecked}>
                        <span class="checkmark"></span>
                    </label>
                </td>
                <td>
                    <div class="d-flex align-items-center justify-content-between">
                        <div>
                            <h6>${school.name}</h6>
                            <div class="square-bullet">
                                <div>${school.type}</div>
                                <div><span>${school.students}</span> طالب</div>
                            </div>
                        </div>
                        <span class="badge ${ratingClass}">${school.rating}</span>
                    </div>
                </td>
                <td>
                    <div class="input-group datetime">
                        <input type="text" class="form-control visit-date-picker" 
                               data-school-id="${school.id}" 
                               placeholder="اختر تاريخ بداية ونهاية الزيارة"
                               value="${school.visitDate || ''}">
                        <span class="input-group-text"><i class="la la-calendar"></i></span>
                    </div>
                </td>
                <td><p class="m-0">${school.lastEvalDate}</p></td>
                <td>
                    <select class="form-select visit-type-select" data-school-id="${school.id}">
                        <option value="دوري" ${school.visitType === 'دوري' ? 'selected' : ''}>دوري</option>
                        <option value="استثنائي" ${school.visitType === 'استثنائي' ? 'selected' : ''}>استثنائي</option>
                        <option value="زيارة" ${school.visitType === 'زيارة' ? 'selected' : ''}>زيارة</option>
                    </select>
                </td>
                <td><p class="m-0">${school.nextEvalDate}</p></td>
                <td>
                    <p class="m-0">
                        <a href="#" class="text-dark view-school-details" data-school-id="${school.id}">
                            <i class="la la-eye"></i>
                        </a>
                    </p>
                </td>
            </tr>
        `;
    });

    $('#userTable tbody').html(tbody);

    // Reinitialize event handlers
    initializeTableEvents();
}

// ============= TABLE EVENT HANDLERS =============
function initializeTableEvents() {
    // Select all checkbox
    $('#selectAll').off('change').on('change', function () {
        const isChecked = $(this).is(':checked');
        $('.row-select').prop('checked', isChecked);

        if (isChecked) {
            $('.row-select').each(function () {
                const schoolId = parseInt($(this).data('school-id'));
                if (!selectedSchools.includes(schoolId)) {
                    selectedSchools.push(schoolId);
                }
            });
        } else {
            selectedSchools = [];
        }

        updateSelectionCount();
    });

    // Individual row checkboxes
    $('.row-select').off('change').on('change', function () {
        const schoolId = parseInt($(this).data('school-id'));

        if ($(this).is(':checked')) {
            if (!selectedSchools.includes(schoolId)) {
                selectedSchools.push(schoolId);
            }
        } else {
            selectedSchools = selectedSchools.filter(id => id !== schoolId);
        }

        updateSelectAllCheckbox();
        updateSelectionCount();
    });

    // Visit date pickers
    if (typeof flatpickr !== 'undefined') {
        $('.visit-date-picker').each(function () {
            const schoolId = $(this).data('school-id');
            flatpickr(this, {
                mode: "range",
                dateFormat: "Y-m-d",
                locale: "ar",
                onChange: function (selectedDates, dateStr) {
                    updateSchoolVisitDate(schoolId, dateStr);
                }
            });
        });
    }

    // Visit type selects
    $('.visit-type-select').off('change').on('change', function () {
        const schoolId = $(this).data('school-id');
        const visitType = $(this).val();
        updateSchoolVisitType(schoolId, visitType);
    });

    // View details buttons
    $('.view-school-details').off('click').on('click', function (e) {
        e.preventDefault();
        const schoolId = $(this).data('school-id');
        viewSchoolDetails(schoolId);
    });
}

// ============= SEARCH FUNCTIONALITY =============
function initializeSearch() {
    let searchTimeout;

    $('#customSearch').on('input', function () {
        clearTimeout(searchTimeout);
        const searchTerm = $(this).val().trim();

        searchTimeout = setTimeout(function () {
            performSearch(searchTerm);
        }, 300);
    });
}

function performSearch(searchTerm) {
    if (!searchTerm) {
        loadSchoolsData(1, filterCriteria);
        return;
    }

    const filteredSchools = allSchools.filter(school =>
        school.name.includes(searchTerm) ||
        school.type.includes(searchTerm)
    );

    renderSchoolsTable(filteredSchools);
    renderPagination(filteredSchools.length);
}

// ============= FILTER FUNCTIONALITY =============
function initializeFilters() {
    // Initialize filter date pickers
    if (typeof flatpickr !== 'undefined') {
        $('#filterOffcanvas #singleDate').each(function () {
            flatpickr(this, {
                dateFormat: "d/m/Y",
                locale: "ar"
            });
        });
    }

    // Apply filters button
    $('#filterOffcanvas form').on('submit', function (e) {
        e.preventDefault();
        applyFilters();
    });

    // Clear filters button
    $('#filterOffcanvas .btn-outline-primary').on('click', function (e) {
        e.preventDefault();
        clearFilters();
    });
}

function applyFilters() {
    const filters = {
        schoolName: $('#filterOffcanvas input[placeholder="أكتب هنا..."]').val(),
        lastEvalDate: $('#filterOffcanvas #singleDate').eq(0).val(),
        createdDate: $('#filterOffcanvas #singleDate').eq(1).val(),
        nextEvalDate: $('#filterOffcanvas #singleDate').eq(2).val(),
        previousResult: $('#filterOffcanvas input[placeholder="ضعيف"]').val(),
        visitType: $('#filterOffcanvas input[placeholder="دوري"]').val()
    };

    filterCriteria = filters;
    loadSchoolsData(1, filters);

    // Update filter badge
    const activeFilters = Object.values(filters).filter(v => v).length;
    $('.filterbtn .badge').text(activeFilters);

    // Close offcanvas
    const offcanvas = bootstrap.Offcanvas.getInstance($('#filterOffcanvas')[0]);
    if (offcanvas) offcanvas.hide();
}

function clearFilters() {
    $('#filterOffcanvas form')[0].reset();
    filterCriteria = {};
    loadSchoolsData(1, {});
    $('.filterbtn .badge').text('0');
}

// ============= PAGINATION =============
function renderPagination(totalRecords) {
    const totalPages = Math.ceil(totalRecords / pageSize);
    let paginationHtml = '';

    if (totalPages <= 1) {
        $('#dtPagination').html('');
        return;
    }

    paginationHtml += '<nav><ul class="pagination pagination-sm mb-0">';

    // Previous button
    if (currentPage > 1) {
        paginationHtml += `
            <li class="page-item">
                <a class="page-link" href="#" data-page="${currentPage - 1}">السابق</a>
            </li>
        `;
    }

    // Page numbers
    const maxPages = 5;
    let startPage = Math.max(1, currentPage - Math.floor(maxPages / 2));
    let endPage = Math.min(totalPages, startPage + maxPages - 1);

    if (endPage - startPage < maxPages - 1) {
        startPage = Math.max(1, endPage - maxPages + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
        const activeClass = i === currentPage ? 'active' : '';
        paginationHtml += `
            <li class="page-item ${activeClass}">
                <a class="page-link" href="#" data-page="${i}">${i}</a>
            </li>
        `;
    }

    // Next button
    if (currentPage < totalPages) {
        paginationHtml += `
            <li class="page-item">
                <a class="page-link" href="#" data-page="${currentPage + 1}">التالي</a>
            </li>
        `;
    }

    paginationHtml += '</ul></nav>';
    $('#dtPagination').html(paginationHtml);

    // Pagination click events
    $('#dtPagination .page-link').on('click', function (e) {
        e.preventDefault();
        const page = parseInt($(this).data('page'));
        loadSchoolsData(page, filterCriteria);
    });
}

// ============= MODAL HANDLERS =============
function initializeModals() {
    // Save button in main page
    $('.btn-primary.mw-200').on('click', function (e) {
        e.preventDefault();

        if (selectedSchools.length === 0) {
            showNotification('error', 'يرجى اختيار مدرسة واحدة على الأقل');
            return;
        }

        if (!validatePlanForm()) {
            showNotification('error', 'يرجى ملء جميع الحقول المطلوبة');
            return;
        }

        // Update modal text
        $('#confirmation-modal h5').text(`تم تحديد (${selectedSchools.length.toString().padStart(2, '0')}) مدرسة للإضافة للخطة`);

        // Show modal
        const modal = new bootstrap.Modal($('#confirmation-modal')[0]);
        modal.show();
    });

    // Confirm save button
    $('#btn-submit').on('click', function () {
        savePlan();
    });

    // Cancel button
    $('#confirmation-modal .btn-outline-primary').on('click', function () {
        const modal = bootstrap.Modal.getInstance($('#confirmation-modal')[0]);
        if (modal) modal.hide();
    });
}

// ============= VALIDATION =============
function validatePlanForm() {
    const title = $('#username').val().trim();
    const planType = $('#ddlPlanType').val();
    const dateRange = $('#dateRange').val().trim();

    return title && planType && dateRange;
}

// ============= SAVE FUNCTIONALITY =============
function savePlan() {
    const planData = {
        title: $('#username').val(),
        planType: $('#ddlPlanType').val(),
        dateRange: $('#dateRange').val(),
        schools: selectedSchools.map(schoolId => {
            const school = allSchools.find(s => s.id === schoolId);
            return {
                schoolId: schoolId,
                visitDate: school.visitDate,
                visitType: school.visitType
            };
        })
    };

    console.log('Saving plan:', planData);

    // Simulate API call
    setTimeout(function () {
        showNotification('success', 'تم حفظ الخطة بنجاح');

        // Close modal
        const modal = bootstrap.Modal.getInstance($('#confirmation-modal')[0]);
        if (modal) modal.hide();

        // Reset form
        resetForm();
    }, 1000);

    // Replace with actual API call:
    // $.ajax({
    //     url: '/api/plans/create',
    //     method: 'POST',
    //     data: JSON.stringify(planData),
    //     contentType: 'application/json',
    //     success: function(response) {
    //         showNotification('success', 'تم حفظ الخطة بنجاح');
    //         resetForm();
    //     },
    //     error: function(error) {
    //         showNotification('error', 'حدث خطأ أثناء حفظ الخطة');
    //     }
    // });
}

// ============= HELPER FUNCTIONS =============
function updateSchoolVisitDate(schoolId, dateStr) {
    const school = allSchools.find(s => s.id === schoolId);
    if (school) {
        school.visitDate = dateStr;
    }
}

function updateSchoolVisitType(schoolId, visitType) {
    const school = allSchools.find(s => s.id === schoolId);
    if (school) {
        school.visitType = visitType;
    }
}

function viewSchoolDetails(schoolId) {
    const school = allSchools.find(s => s.id === schoolId);
    if (school) {
        console.log('Viewing details for school:', school);
        // Implement modal or page navigation
        showNotification('info', `عرض تفاصيل ${school.name}`);
    }
}

function updateSelectionCount() {
    console.log(`Selected ${selectedSchools.length} schools`);
}

function updateSelectAllCheckbox() {
    const totalCheckboxes = $('.row-select').length;
    const checkedCheckboxes = $('.row-select:checked').length;
    $('#selectAll').prop('checked', totalCheckboxes === checkedCheckboxes);
}

function getRatingClass(rating) {
    const classes = {
        'ممتاز': 'bg-success',
        'جيد جداً': 'bg-info',
        'جيد': 'bg-primary',
        'مقبول': 'bg-secondary'
    };
    return classes[rating] || 'bg-secondary';
}

function generateRandomDate(future = false) {
    const start = future ? new Date() : new Date(2023, 0, 1);
    const end = future ? new Date(2025, 11, 31) : new Date();
    const date = new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime()));
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
}

function showNotification(type, message) {
    // Implement your notification system
    // Using toastr, sweetalert, or custom notification
    console.log(`[${type.toUpperCase()}] ${message}`);

    // Example with alert (replace with better notification)
    alert(message);
}

function resetForm() {
    $('#username').val('');
    $('#ddlPlanType').val('').trigger('change');
    $('#dateRange').val('');
    selectedSchools = [];
    $('.row-select').prop('checked', false);
    $('#selectAll').prop('checked', false);
    loadSchoolsData(1, {});
}

// Initialize search on load
$(document).ready(function () {
    initializeSearch();
});