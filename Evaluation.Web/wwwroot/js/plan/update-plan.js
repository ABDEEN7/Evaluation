/**
 * Update Plan Loader
 * Loads existing plan data and populates the form
 */

let existingPlanData = null;
let selectedSchoolIds = [];

$(document).ready(function () {
    // Get planId from URL query parameter
    const urlParams = new URLSearchParams(window.location.search);
    const planId = urlParams.get('planId');

    if (planId) {
        loadPlanData(planId);
    } else {
        console.error('No planId provided in URL');
        alert('معرف الخطة غير موجود');
    }
});

/**
 * Load plan data by ID
 */
function loadPlanData(planId) {
    jqClient().Get(`/PlanMocks/GetPlanById?planId=${planId}`)
        .done((result) => {
            if (result && result.result) {
                existingPlanData = result.result;
                populatePlanForm(existingPlanData);
                loadSchoolsWithSelection(existingPlanData.schools || []);
            } else {
                console.error('No plan data returned');
                alert('لم يتم العثور على بيانات الخطة');
            }
        })
        .fail((jqXHR, textStatus, err) => {
            console.error('LoadPlanData failed', textStatus, err);
            alert('حدث خطأ أثناء تحميل بيانات الخطة');
        });
}

/**
 * Populate form fields with existing plan data
 */
function populatePlanForm(planData) {
    // Set hidden plan ID
    $('#planId').val(planData.id);

    // Set plan title
    $('#planTitle').val(planData.name || '');

    // Set plan type - wait for the dropdown to be populated
    const checkPlanTypeInterval = setInterval(() => {
        const planTypeOptions = $('#ddlPlanType option').length;
        if (planTypeOptions > 1) { // More than just the placeholder
            clearInterval(checkPlanTypeInterval);

            $('#ddlPlanType').val(planData.planTypeId).trigger('change');

            // If semester type, set semester after dropdown populates
            if (planData.semesterId) {
                setTimeout(() => {
                    $('#ddlSemester').val(planData.semesterId).trigger('change');
                }, 500);
            }
        }
    }, 100);

    // Set date range
    if (planData.startDate && planData.endDate) {
        const startDate = formatDate(planData.startDate);
        const endDate = formatDate(planData.endDate);
        $('#parentDate').val(`${startDate} to ${endDate}`);
    }
}

/**
 * Load schools and pre-select those in the plan
 */
function loadSchoolsWithSelection(planSchools) {
    // Store selected school IDs and their data
    selectedSchoolIds = planSchools.map(school => school.id);

    // Create a map for quick lookup
    const schoolDataMap = {};
    planSchools.forEach(school => {
        schoolDataMap[school.id] = {
            startEvaluationDate: school.startEvaluationDate,
            endEvaluationDate: school.endEvaluationDate,
            visitTypeId: school.visitTypeId
        };
    });

    // Override the renderSchoolsTable function to handle pre-selection
    const originalRender = window.renderSchoolsTable;
    window.renderSchoolsTable = function (schools) {
        originalRender(schools);

        // After rendering, select checkboxes and populate data
        schools.forEach(school => {
            if (selectedSchoolIds.includes(school.id)) {
                const checkbox = $(`.selectRow[data-id="${school.id}"]`);
                checkbox.prop('checked', true);

                const row = checkbox.closest('tr');
                const schoolData = schoolDataMap[school.id];

                if (schoolData) {
                    // Set date range
                    if (schoolData.startEvaluationDate && schoolData.endEvaluationDate) {
                        const startDate = formatDate(schoolData.startEvaluationDate);
                        const endDate = formatDate(schoolData.endEvaluationDate);
                        row.find('.childDate').val(`${startDate} to ${endDate}`);
                    }

                    // Set visit type
                    if (schoolData.visitTypeId) {
                        row.find('.visitTypeSelect').val(schoolData.visitTypeId);
                    }
                }
            }
        });

        // Update confirmation message
        updateConfirmationMessage();

        // Re-initialize flatpickr for the new inputs
        initializeFlatpickr();
    };

    // Load schools data (will use the overridden render function)
    loadSchoolsData();
}

/**
 * Format date to YYYY-MM-DD
 */
function formatDate(dateString) {
    if (!dateString) return '';

    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
}

/**
 * Update confirmation modal message with selected school count
 */
function updateConfirmationMessage() {
    const selectedCount = $('.selectRow:checked').length;
    $('#confirmationMessage').text(`تم تحديد (${selectedCount}) مدرسة للتحديث`);
}

// Update count when checkboxes change
$(document).on('change', '.selectRow, #selectAll', function () {
    updateConfirmationMessage();
});