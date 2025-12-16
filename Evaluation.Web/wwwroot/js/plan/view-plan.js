/**
 * View Plan Handler - Loads and displays plan data in read-only mode
 */
const RATING_CLASSES = {
    'Perfect': 'bg-success',
    'VeryGood': 'bg-info',
    'Good': 'bg-primary',
    'Acceptable': 'bg-secondary',
    'Week': 'bg-danger'
};
$(document).ready(function () {
    // Get plan ID from URL or other source
    const urlParams = new URLSearchParams(window.location.search);
    const planId = urlParams.get('planId');

    if (planId) {
        loadPlanData(planId);
    } else {
        console.error('No plan ID provided');
        showError('لم يتم توفير معرف الخطة');
    }
});

/**
 * Load plan data from API
 */
function loadPlanData(planId) {
    const API_ENDPOINTS = {
        GET_PLAN_DETAILS: '/Plan/GetPlanDetails',
        GET_PLAN_TYPES: '/PlanType/GetPlanTypes',
        GET_SEMESTERS: '/Plan/GetSemesters',
        GET_VISITS: '/School/GetVisits'
    };
    // Show loading state
    showLoading();

    // Call API to get plan details
    jqClient().Get(`${API_ENDPOINTS.GET_PLAN_DETAILS}/${planId}`)
        .done(function (response) {
            if (response && response.result) {
                populatePlanData(response.result);
                makeFormReadOnly();
            } else {
                showError('لم يتم العثور على بيانات الخطة');
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.error('Error loading plan:', textStatus, errorThrown);
            showError('حدث خطأ أثناء تحميل بيانات الخطة');
        });
}

/**
 * Populate form fields with plan data
 */
function populatePlanData(planData) {
    // Fill basic plan information
    $('#planTitle').val(planData.name || '');
    $('#ddlPlanType').val(planData.planTypeId || '').trigger('change');

    // Fill semester if exists
    if (planData.semesterId) {
        $('#semesterContainer').show();
        $('#ddlSemester').val(planData.semesterId).trigger('change');
    }

    // Fill date range
    if (planData.startDate && planData.endDate) {
        const dateRange = `${planData.startDate} إلى ${planData.endDate}`;
        $('#parentDate').val(dateRange);

        // If using flatpickr, set the dates properly
        if ($('#parentDate')[0]._flatpickr) {
            $('#parentDate')[0]._flatpickr.setDate([planData.startDate, planData.endDate]);
        }
    }

    // Populate schools table
    if (planData.schools && planData.schools.length > 0) {
        populateSchoolsTable(planData.schools, planData.startDate, planData.endDate);
        loadSchoolDetails(planData.schools);
    } else {
        showEmptyTable();
    }
}

/**
 * Populate schools table with data
 */
function populateSchoolsTable(schools, planStartDate, planEndDate) {
    const tbody = $('#planTable tbody');
    tbody.empty();

    // Keep track of blocked dates for calendar
    const blockedDates = [];

    schools.forEach(function (school, index) {
        // Get school details (you may need to call another API or have this data included)
        const row = createSchoolRow(school, index, planStartDate, planEndDate);
        tbody.append(row);

        // Collect blocked dates
        if (school.startEvaluationDate && school.endEvaluationDate) {
            blockedDates.push({
                start: school.startEvaluationDate,
                end: school.endEvaluationDate
            });
        }
    });

    // Initialize or update flatpickr with blocked dates
    initializeDatePickers(blockedDates, planStartDate, planEndDate);

    // Update confirmation message
    $('#confirmationMessage').text(`تم تحديد (${schools.length}) مدرسة للإضافة للخطة`);
}

/**
 * Create table row for school
 */
function createSchoolRow(school, index, planStartDate, planEndDate) {
    const generateSchoolNameCell = (school) => {
        const ratingClass = RATING_CLASSES?.[school.rating] || 'bg-light';

        const container = $('<div>')
            .addClass('d-flex align-items-center justify-content-between');

        const infoDiv = $('<div>');

        infoDiv.append(
            $('<h6>').text(school.name || '-')
        );

        const levelBadge = $('<div>').addClass('square-bullet');

        const levelText = (school.schoolLevel && school.schoolLevel.length > 0)
            ? school.schoolLevel.map(l => l.name).join(', ')
            : '-';

        levelBadge.append(
            $('<div>').text(levelText)
        );

        infoDiv.append(levelBadge);

        const ratingBadge = $('<span>')
            .addClass(`badge ${ratingClass}`)
            .text(school.rating || '');

        container.append(infoDiv, ratingBadge);

        return container;
    };

    const startDate = formatDate(school.startEvaluationDate);
    const endDate = formatDate(school.endEvaluationDate);
    const dateRange = `${startDate} - ${endDate}`;

    // Generate unique ID for date picker
    const datePickerId = `schoolDate_${index}`;

    const $row = $(`
    <tr data-school-id="${school.id}">
        <td>
            <label class="custom-checkbox">
                <input type="checkbox" class="school-checkbox" checked disabled>
                <span class="checkmark"></span>
            </label>
        </td>

        <td class="school-name-cell"></td>

        <td>
            <div class="input-group">
                <input type="text"
                       class="form-control school-date-picker"
                       id="${datePickerId}"
                       value="${dateRange}"
                       readonly
                       data-school-id="${school.id}"
                       data-start="${school.startEvaluationDate}"
                       data-end="${school.endEvaluationDate}">
                <span class="input-group-text">
                    <i class="la la-calendar"></i>
                </span>
            </div>
        </td>

        <td class="last-eval-date">-</td>
        <td class="visit-type" data-visit-type-id="${school.visitTypeId}">
            <span class="spinner-border spinner-border-sm" role="status"></span>
        </td>
        <td class="academic-year">-</td>
        <td>
            <button class="btn btn-sm btn-outline-primary"
                    onclick="viewSchoolDetails('${school.id}')"
                    disabled>
                <i class="la la-eye"></i>
            </button>
        </td>
    </tr>
`);
    $row.find('.school-name-cell')
        .append(generateSchoolNameCell(school));
    return $row;

}

/**
 * Initialize date pickers with blocked dates
 */
function initializeDatePickers(blockedDates, minDate, maxDate) {
    $('.school-date-picker').each(function () {
        const $input = $(this);
        const startDate = $input.data('start');
        const endDate = $input.data('end');

        // Initialize flatpickr in read-only mode
        flatpickr($input[0], {
            mode: 'range',
            dateFormat: 'Y-m-d',
            defaultDate: [startDate, endDate],
            minDate: minDate,
            maxDate: maxDate,
            locale: 'ar',
            disable: blockedDates.map(d => {
                return {
                    from: d.start,
                    to: d.end
                };
            }),
            clickOpens: false, // Make it non-interactive
            allowInput: false
        });
    });
}

/**
 * Make entire form read-only
 */
function makeFormReadOnly() {
    // Disable all form inputs
    $('#planForm input, #planForm select').prop('disabled', true).addClass('readonly');

    // Disable checkboxes
    $('#selectAll, .school-checkbox').prop('disabled', true);

    // Disable filter form
    $('#filterForm input, #filterForm select').prop('disabled', true);

    // Disable action buttons
    $('.btn-primary').not('[data-bs-dismiss="modal"]').prop('disabled', true);

    // Hide save button, show edit button instead
    const $saveBtn = $('a[data-bs-target="#confirmation-modal"]');
    $saveBtn.replaceWith(`
        <a href="#" class="btn btn-warning mw-200" id="editPlanBtn">
            <i class="la la-edit"></i> تعديل الخطة
        </a>
    `);

    // Change page title
    $('h3').first().text('عرض تفاصيل الخطة');
    $('h4').first().text('تفاصيل الخطة');
}

/**
 * Format date to readable format
 */
function formatDate(dateString) {
    if (!dateString) return '-';

    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
}

/**
 * Show loading state
 */
function showLoading() {
    $('#planTable tbody').html(`
        <tr>
            <td colspan="7" class="text-center py-5">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">جاري التحميل...</span>
                </div>
                <p class="mt-2 text-muted">جاري تحميل بيانات الخطة...</p>
            </td>
        </tr>
    `);
}

/**
 * Show empty table message
 */
function showEmptyTable() {
    $('#planTable tbody').html(`
        <tr>
            <td colspan="7" class="text-center py-5">
                <i class="la la-inbox la-3x text-muted"></i>
                <p class="mt-2 text-muted">لا توجد مدارس في هذه الخطة</p>
            </td>
        </tr>
    `);
}

/**
 * Show error message
 */
function showError(message) {
    $('#planTable tbody').html(`
        <tr>
            <td colspan="7" class="text-center py-5">
                <i class="la la-exclamation-triangle la-3x text-danger"></i>
                <p class="mt-2 text-danger">${message}</p>
            </td>
        </tr>
    `);

    // Show error notification if you have a notification system
    if (typeof showNotification === 'function') {
        showNotification('error', message);
    }
}

/**
 * View school details (placeholder)
 */
function viewSchoolDetails(schoolId) {
    // This function would open a modal or navigate to school details page
    console.log('View school details:', schoolId);
}

/**
 * Load additional school details (name, last eval date, etc.)
 * This should be called after table is populated
 */
function loadSchoolDetails(schools) {
    schools.forEach(function (school) {
        // Call API to get school details if needed
        // For now, you might need to call another endpoint
        jqClient().Get(`/School/GetDetails`, school.id)
            .done(function (response) {
                if (response && response.result) {
                    const $row = $(`tr[data-school-id="${school.id}"]`);
                    $row.find('.school-name').text(response.result.name || 'غير متوفر');
                    $row.find('.last-eval-date').text(formatDate(response.result.lastEvalDate));
                    $row.find('.academic-year').text(response.result.academicYear || '-');
                }
            })
            .fail(function () {
                const $row = $(`tr[data-school-id="${school.id}"]`);
                $row.find('.school-name').text('غير متوفر');
            });

        // Load visit type name
        jqClient().Get(`/VisitType/GetDetails`, school.visitTypeId)
            .done(function (response) {
                if (response && response.result) {
                    const $row = $(`tr[data-school-id="${school.id}"]`);
                    $row.find('.visit-type').html(`
                        <span class="badge bg-info">${response.result.name || '-'}</span>
                    `);
                }
            })
            .fail(function () {
                const $row = $(`tr[data-school-id="${school.id}"]`);
                $row.find('.visit-type').text('-');
            });
    });
}

// Export functions if needed
window.loadPlanData = loadPlanData;
window.viewSchoolDetails = viewSchoolDetails;