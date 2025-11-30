/**
 * Collects and validates evaluation plan data from the form
 * @returns {Object|null} Evaluation data object or null if validation fails
 */
function getEvaluationData() {
    try {
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

        //Get semester Id if paln type is semtster
        let semesterId = null;
        if (planTypeBackendName === 'Semester') {
            const semesterSelect = $('#ddlSemester');
            semesterId = semesterSelect.val() || null;
        }

        const selectedSchools = getSelectedSchools();

        // Return structured data object
        const evaluationData = {
            name,
            planTypeId,
            startDate,
            endDate,
            schools: selectedSchools
        };
        if (planTypeBackendName === 'Semester' && semesterId) {
            evaluationData.semesterId = semesterId;
        }
        return evaluationData;

    } catch (error) {
        console.error('Error collecting evaluation data:', error);
        return null;
    }
}

/**
 * Parses date range string into start and end dates
 * @param {string} dateRangeValue - Date range in format "DD/MM/YYYY to DD/MM/YYYY"
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

    // Split by " to " (Flatpickr's default separator)
    let dates = trimmedValue.split(' to ');

    if (dates.length === 2) {
        const date1 = dates[0].trim();
        const date2 = dates[1].trim();

        // Convert to Date objects for comparison
        const dateObj1 = new Date(date1);
        const dateObj2 = new Date(date2);

        // Ensure startDate is the earlier date and endDate is the later date
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

// Usage examples

/**
 * Validates evaluation data before submission
 * @param {Object} data - Evaluation data object
 * @returns {Object} Validation result with isValid flag and errors array
 */
function validateEvaluationData(data) {
    const errors = [];

    if (!data) {
        errors.push('Failed to collect form data');
        return { isValid: false, errors };
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

    return {
        isValid: errors.length === 0,
        errors
    };
}

/**
 * Displays validation errors to the user
 * @param {Array} errors - Array of error messages
 */
function displayErrors(errors) {
    // Option 1: Alert (simple but not ideal UX)
    alert('Please fix the following errors:\n\n' + errors.join('\n'));

    // Option 2: Console (for debugging)
    console.error('Validation errors:', errors);

    // Option 3: Custom error display (recommended)
    // You can implement a toast notification or error message div
}

/**
 * Submits evaluation data to the server
 * @param {Object} data - Validated evaluation data
 */
async function submitEvaluationData(data) {
    try {
        // Show loading state
        //const submitBtn = document.getElementById('btn-submit');
        //const originalText = submitBtn?.textContent;
        //if (submitBtn) {
        //    submitBtn.disabled = true;
        //    submitBtn.textContent = 'Submitting...';
        //}
        //jqClient().Post(`/Plan/Create`, data).fail((jqXHR, textStatus, errorThrown) => {
        //    console.error('Error: [Create Plan Condition]', textStatus, errorThrown);
        //});
        const result = await jqClient().Post(`/Plan/Approve`, data);

        // Handle success
        console.log('Object Json:', result);

        // Optional: Reset form or redirect
        // document.getElementById('evaluationForm')?.reset();

    } catch (error) {
        console.error('Submission error:', error);
        alert('Failed to submit evaluation plan. Please try again.');
    } finally {
        // Restore button state
        const submitBtn = document.getElementById('btn-submit');
        if (submitBtn) {
            submitBtn.disabled = false;
            submitBtn.textContent = originalText || 'Submit';
        }
    }
}

// Event handler for submit button
document.getElementById('btn-submit')?.addEventListener('click', async (event) => {
    event.preventDefault(); // Prevent default form submission if inside a form

    // Step 1: Collect data
    const evaluationData = getEvaluationData();

    // Step 2: Validate data
    const validation = validateEvaluationData(evaluationData);

    if (!validation.isValid) {
        displayErrors(validation.errors);
        return;
    }

    // Step 3: Submit data
    await submitEvaluationData(evaluationData);
});


function getSelectedSchools() {
    const selectedSchools = [];
    $('#planTable tbody .selectRow:checked').each(function () {
        const checkbox = $(this);
        const schoolId = checkbox.data('id');
        const row = checkbox.closest('tr');

        const dataRangeInput = row.find('.childDate');
        const dateRangeValue = dataRangeInput.val() || '';
        const parsedDates = parseDateRange(dateRangeValue);
        const visitTypeSelect = row.find('.visitTypeSelect');
        const visitTypeId = visitTypeSelect.val() || '';
        const visitTypeName = visitTypeSelect.find('option:selected').text() || '';

        // Get school name from the table
        const schoolData =
        {
            id: schoolId,
            startEvaluationDate: parsedDates.startDate,
            endEvaluationDate: parsedDates.endDate,
            visitTypeId: visitTypeId
        };
        selectedSchools.push(schoolData);
    });
    return selectedSchools;
}