/**
 * Collects and validates evaluation plan data from the form
 * @returns {Object|null} Evaluation data object or null if validation fails
 */
function getEvaluationData() {
    try {
        // Get plan name from the title input
        const planName = document.getElementById('username')?.value?.trim() || '';

        // Get plan type from the select dropdown
        const planTypeSelect = document.getElementById('userRole');
        const planType = planTypeSelect?.value || '';

        // Get date range and parse start/end dates
        const dateRangeInput = document.getElementById('dateRange');
        let startDate = '';
        let endDate = '';

        if (dateRangeInput?.value) {
            const parsedDates = parseDateRange(dateRangeInput.value);
            startDate = parsedDates.startDate;
            endDate = parsedDates.endDate;
        }

        // Return structured data object
        return {
            planName,
            planType,
            startDate,
            endDate
        };

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
    const dates = dateRangeValue.split(/\s*(?:to|-)\s*/);

    if (dates.length === 2) {
        return {
            startDate: dates[0].trim(),
            endDate: dates[1].trim()
        };
    } else if (dates.length === 1) {
        // If only one date is provided, use it as both start and end
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

    if (!data.planName) {
        errors.push('Plan name is required');
    }

    if (!data.planType) {
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
        const submitBtn = document.getElementById('btn-submit');
        const originalText = submitBtn?.textContent;
        if (submitBtn) {
            submitBtn.disabled = true;
            submitBtn.textContent = 'Submitting...';
        }

        // Make API call (replace with your actual API endpoint)
        const response = await fetch('/api/evaluation', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const result = await response.json();

        // Handle success
        console.log('Submission successful:', result);
        alert('Evaluation plan submitted successfully!');

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

// Alternative: jQuery version if you prefer
$("#btn-submit").click(async function (event) {
    event.preventDefault();

    const evaluationData = getEvaluationData();
    const validation = validateEvaluationData(evaluationData);

    if (!validation.isValid) {
        displayErrors(validation.errors);
        return;
    }

    await submitEvaluationData(evaluationData);
});