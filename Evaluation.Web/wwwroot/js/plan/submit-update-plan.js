const { trim } = require("jquery");

/**
 * Collects and validates update plan data from the form
 * @returns {Object|null} Update data object or null if validation fails
*/
function getUpdatePlanData() {
    try {
        //Get Plan Id
        const planId = $('#planId').val()?.trim() || '';
        if (!planId) {
            console.error('Plan ID is missing');
            return null;
        }
        //Get plan name from the title input 
        const name = $('#planTitle').val()?.trim() || '';
        //Get plan Type from the select dropdown
        const planTypeSelect = $('#ddlPlanType');
        const selectedPlanType = planTypeSelect.select2('data')[0];
        const planTypeId = selectedPlanType?.Id || '';
        const planTypeBackendName = selectedPlanType?.backendName || '';

        //Get Date range and parse start/ end dates
        const dateRangeInput = $('#parentDate');
        let startDate = '';
        let endDate = '';
        if (dateRangeInput?.value) {
            const parsedDates = parseDateRange(dateRangeInput.value);
            startDate = parsedDates.startDate;
            endDate = parsedDates.endDate;
        }
        // Get semester Id if plan type is semester
        let semsterId = null;
        if (planTypeBackendName === 'Semster') {
            const semsterSelect = $('#ddlSemester');
            semsterId = semsterSelect.val() || null;
        }
        const selectedSchools = getSelectedSchools.val() || null;
        //Return structured data object
        const updateDate = {
            id: planId,
            name,
            planTypeId,
            startDate,
            endDate,
            schools: selectedSchools
        };
        if (planTypeBackendName === 'Semester' && semsterId) {
            updateDate.semsterId = semsterId;
        }
        return updateDate;
    } catch (error) {
        console.error('Error collecting update plan data:', error);
        return null;
    }
}
/**
 * Validates update plan data before submission
 * @param {Object} data - Update plan data object
 * @returns {Object} Validation result with isValid flag and errors array
 */
function validateUpdatePlanData(data) {
    const errors = [];

    if (!data) {
        errors.push('Failed to collect form data');
        return { isValid: false, errors };
    }

    if (!data.id) {
        errors.push('Plan ID is required');
    }

    if (!data.name) {
        errors.push('Plan name is required');
    }

    if (!data.planTypeId) {
        errors.push('Plan type is required');
    }

    if (!data.startDate || !data.endDate) {
        errors.push('Date range is required');
    }

    if (!data.schools || data.schools.length === 0) {
        errors.push('Please select at least one school');
    }

    return {
        isValid: errors.length === 0,
        errors
    };
}

/**
 * Collects and validates update plan data from the form
 * @returns {Object|null} Update data object or null if validation fails
 */
function getUpdatePlanData() {
    try {
        // Get plan ID
        const planId = document.getElementById('planId')?.value?.trim() || '';

        if (!planId) {
            console.error('Plan ID is missing');
            return null;
        }

        // Get plan name from the title input
        const name = document.getElementById('planTitle')?.value?.trim() || '';

        // Get plan type from the select dropdown
        const planTypeSelect = $('#ddlPlanType');
        const selectedPlanType = planTypeSelect.select2('data')[0];
        const planTypeId = selectedPlanType?.id || '';
        const planTypeBackendName = selectedPlanType?.backendName || '';

        // Get date range and parse start/end dates
        const dateRangeInput = document.getElementById('parentDate');
        let startDate = '';
        let endDate = '';

        if (dateRangeInput?.value) {
            const parsedDates = parseDateRange(dateRangeInput.value);
            startDate = parsedDates.startDate;
            endDate = parsedDates.endDate;
        }

        // Get semester Id if plan type is semester
        let semesterId = null;
        if (planTypeBackendName === 'Semester') {
            const semesterSelect = $('#ddlSemester');
            semesterId = semesterSelect.val() || null;
        }

        const selectedSchools = getSelectedSchools();

        // Return structured data object
        const updateData = {
            id: planId,
            name,
            planTypeId,
            startDate,
            endDate,
            schools: selectedSchools
        };

        if (planTypeBackendName === 'Semester' && semesterId) {
            updateData.semesterId = semesterId;
        }

        return updateData;

    } catch (error) {
        console.error('Error collecting update plan data:', error);
        return null;
    }
}

/**
 * Validates update plan data before submission
 * @param {Object} data - Update plan data object
 * @returns {Object} Validation result with isValid flag and errors array
 */
function validateUpdatePlanData(data) {
    const errors = [];

    if (!data) {
        errors.push('Failed to collect form data');
        return { isValid: false, errors };
    }

    if (!data.id) {
        errors.push('Plan ID is required');
    }

    if (!data.name) {
        errors.push('Plan name is required');
    }

    if (!data.planTypeId) {
        errors.push('Plan type is required');
    }

    if (!data.startDate || !data.endDate) {
        errors.push('Date range is required');
    }

    if (!data.schools || data.schools.length === 0) {
        errors.push('Please select at least one school');
    }

    return {
        isValid: errors.length === 0,
        errors
    };
}

/**
 * Displays validation errors to the user
 * @param {Array} errors - Array of error messages
 */

function displayUpdateErrors(errors) {
    alert('يرجى إصلاح الأخطاء التالية:\n\n' + errors.join('\n'));
    console.error('Validation errors:', errors);
}
/**
 * Submits update plan data to the server
 * @param {Object} data - Validated update plan data
 */
async function submitUpdatePlanData(data) {
    try {
        const submitBtn = document.getElementById('btn-submit');
        const originalText = submitBtn?.innerHTML;

        if (submitBtn) {
            submitBtn.disabled = true;
            submitBtn.innerHTML = '<i class="la la-spinner la-spin"></i> جاري الحفظ...';
        }

        const result = await jqClient().Post(`/Plan/Update`, data);

        console.log('Update result:', result);

        // Show success message
        alert('تم تحديث الخطة بنجاح');

        // Optional: Redirect to plans list
        // window.location.href = '/Plan/Index';

    } catch (error) {
        console.error('Update submission error:', error);
        alert('فشل في تحديث الخطة. يرجى المحاولة مرة أخرى.');
    } finally {
        const submitBtn = document.getElementById('btn-submit');
        if (submitBtn) {
            submitBtn.disabled = false;
            submitBtn.innerHTML = '<i class="la la-save"></i> تأكيد الحفظ';
        }
    }
}
// Event handler for submit button
document.getElementById('btn-submit')?.addEventListener('click', async (event) => {
    event.preventDefault();

    // Step 1: Collect data
    const updatePlanData = getUpdatePlanData();

    // Step 2: Validate data
    const validation = validateUpdatePlanData(updatePlanData);

    if (!validation.isValid) {
        displayUpdateErrors(validation.errors);
        return;
    }

    // Step 3: Submit data
    await submitUpdatePlanData(updatePlanData);
});

/**
 * Parses date range string into start and end dates
 * @param {string} dateRangeValue - Date range in format "YYYY-MM-DD to YYYY-MM-DD"
 * @returns {Object} Object with startDate and endDate properties
 */
function parseDateRange(dateRangeValue) {
    if (!dateRangeValue || typeof dateRangeValue !== 'string') {
        return {
            startDate: '',
            endDate: ''
        };
    }

    const trimmedValue = dateRangeValue.trim();
    let dates = trimmedValue.split(' to ');

    if (dates.length === 2) {
        const date1 = dates[0].trim();
        const date2 = dates[1].trim();

        const dateObj1 = new Date(date1);
        const dateObj2 = new Date(date2);

        if (dateObj1 <= dateObj2) {
            return {
                startDate: date1,
                endDate: date2
            };
        } else {
            return {
                startDate: date2,
                endDate: date1
            };
        }
    } else if (dates.length === 1) {
        const singleDate = dates[0].trim();
        return {
            startDate: singleDate,
            endDate: singleDate
        };
    }

    return {
        startDate: '',
        endDate: ''
    };
}

/**
 * Get selected schools with their data
 * @returns {Array} Array of selected school objects
 */
function getSelectedSchools() {
    const selectedSchools = [];

    $('#planTable tbody .selectRow:checked').each(function () {
        const checkbox = $(this);
        const schoolId = checkbox.data('id');
        const row = checkbox.closest('tr');

        const dateRangeInput = row.find('.childDate');
        const dateRangeValue = dateRangeInput.val() || '';
        const parsedDates = parseDateRange(dateRangeValue);

        const visitTypeSelect = row.find('.visitTypeSelect');
        const visitTypeId = visitTypeSelect.val() || '';

        const schoolData = {
            id: schoolId,
            startEvaluationDate: parsedDates.startDate,
            endEvaluationDate: parsedDates.endDate,
            visitTypeId: visitTypeId
        };

        selectedSchools.push(schoolData);
    });

    return selectedSchools;
}