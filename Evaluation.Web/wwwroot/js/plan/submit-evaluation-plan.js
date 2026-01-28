const ns = window.planUtility;

/*  Helper selector with prefix support */
function $p(selector) {
    if (!ns?.fieldId) {
        return window.jQuery(selector);
    }

    if (typeof selector === 'string' && selector.startsWith('#')) {
        const id = selector.substring(1);
        return window.jQuery(`#${ns.fieldId}_${id}`);
    }

    return window.jQuery(selector);
}

/**
 * Submit Evaluation Plan Handler
 * Updated to integrate with plan.constants.js, plan.generate-components.js, and plan.main-handler.js
 */

(function () {
    const { API_ENDPOINTS, PLAN_TYPE_BACKEND, ACTION_TYPE } = window.PlanConstants || {};
    const ns = window.planUtility;

    /**
     * Collects and validates evaluation plan data from the form
     * @param {string} fieldId - Field ID prefix
     * @returns {Object|null} Evaluation data object or null if validation fails
     */
    function getFormPlanJson(fieldId) {
        try {
            // Get plan name from the title input
            var fieldScore = `${fieldId}_`;
            const name = $('#' + fieldScore + 'planTitle').val()?.trim() || '';
            // Get plan type from the select dropdown
            const planTypeSelect = $('#' + fieldScore + 'ddlPlanType');
            const selectedPlanType = planTypeSelect.select2('data')[0];
            const PlanTypeDepId = selectedPlanType?.id || '';
            const planTypeBackendName = selectedPlanType?.backendName ||
                selectedPlanType?.element?.dataset?.backendname || '';

            // Get date range and parse start/end dates
            const dateRangeInput = $('#' + fieldScore + 'parentDate');
            let startDate = '';
            let endDate = '';

            if (dateRangeInput.val()) {
                const parsedDates = parseDateRange(dateRangeInput.val());
                startDate = parsedDates.startDate;
                endDate = parsedDates.endDate;
            }

            // Get semester Id if plan type is semester
            let semesterId = null;
            if (planTypeBackendName === PLAN_TYPE_BACKEND.SEMESTER) {
                const semesterSelect = $('#' + fieldScore + 'ddlSemester');
                semesterId = semesterSelect.val() || null;
            }

            // Get selected schools only
            const selectedSchools = getSchools(fieldScore);

            // Return structured data object
            const evaluationData = {
                name,
                PlanTypeDepId,
                startDate,
                endDate,
                schools: selectedSchools
            };

            if (planTypeBackendName === PLAN_TYPE_BACKEND.SEMESTER && semesterId) {
                evaluationData.semesterId = semesterId;
            }

            // Add plan ID if editing an existing plan
            if (ns.currentPlanId) {
                evaluationData.id = ns.currentPlanId;
            }

            return evaluationData;

        } catch (error) {
            console.error('Error collecting evaluation data:', error);
            return null;
        }
    }

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

        // Split by " to " (Flatpickr's default separator)
        let dates = trimmedValue.split(' to ');

        if (dates.length === 2) {
            const date1 = dates[0].trim();
            const date2 = dates[1].trim();

            // Convert to Date objects for comparison
            const dateObj1 = new Date(date1);
            const dateObj2 = new Date(date2);

            // Ensure startDate is earlier and endDate is later
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
     * Validates evaluation data before submission
     * @param {Object} data - Evaluation data object
     * @returns {Object} Validation result with isValid flag and errors array
     */
    function validatePlan(data) {
        const errors = [];

        if (!data) {
            errors.push('Failed to collect form data');
            return { isValid: false, errors };
        }

        if (!data.name || data.name.length < 3) {
            errors.push('Plan title is required and must be at least 3 characters');
        }

        if (!data.PlanTypeDepId) {
            errors.push('Plan type is required');
        }

        if (!data.startDate || !data.endDate) {
            errors.push('Date range is required');
        }

        if (!data.schools || data.schools.length === 0) {
            errors.push('At least one school must be selected');
        }

        // Validate each school has required fields
        if (data.schools && data.schools.length > 0) {
            data.schools.forEach((school, index) => {
                if (!school.startEvaluationDate || !school.endEvaluationDate) {
                    errors.push(`School ${index + 1}: Visit date is required`);
                }
                if (!school.visitTypeId) {
                    errors.push(`School ${index + 1}: Visit type is required`);
                }
            });
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
    function displayErrors(fieldId, errors) {
        alert('Please fix the following errors:\n\n' + errors.join('\n'));
        console.error('Validation errors:', errors);
        var fieldScoure = `${fieldId}_`;
        if (errors.some(e => e.includes('title'))) {
            $('#' + fieldScoure + 'planTitle').addClass('is - invalid');
        }
        if (errors.some(e => e.includes('Plan type'))) {
            $('#' + fieldScoure + 'ddlPlanType').next('.select2-container').addClass('is-invalid');
        }
        if (errors.some(e => e.includes('Date range'))) {
            $('#' + fieldScoure + 'parentDate').addClass('is-invalid');
        }
    }

    /**
     * Clears all validation error states
     */
    function clearErrors() {
        $('.is-invalid').removeClass('is-invalid');
        $('.invalid-feedback').hide();
    }

    /**
     * Submits evaluation data to the server
     * @param {Object} data - Validated evaluation data
     */
    async function submitEvaluationData(data) {
        const submitBtn = $('#btn-submit');
        const originalText = submitBtn.text();

        try {
            submitBtn.prop('disabled', true).text('Saving...');

            let endpoint = API_ENDPOINTS.INSERTORUPDATEPLAN;
            // let endpoint = API_ENDPOINTS.CREATE_PLAN;

            if (ns.currentActionType === ACTION_TYPE.EDIT ||
                ns.currentActionType === ACTION_TYPE.EDIT_DRAFT) {
                endpoint = API_ENDPOINTS.UPDATE_PLAN;
            } else if (ns.currentActionType === ACTION_TYPE.APPROVE ||
                ns.currentActionType === ACTION_TYPE.APPROVE_WITH_CHANGES) {
                endpoint = API_ENDPOINTS.INSERTORUPUDATEPLAN;
            }

            const result = await jqClient().Post(endpoint, data);

            if (result.success) {
                const modal = bootstrap.Modal.getInstance(document.getElementById('confirmation-modal'));
                if (modal) modal.hide();

                alert('Plan saved successfully');
                window.location.href = '/Plan/Index';
            } else {
                throw new Error(result.message || 'Failed to save the plan');
            }

        } catch (error) {
            console.error('Submission error:', error);
            alert('An error occurred while saving the plan. Please try again.');
        } finally {
            submitBtn.prop('disabled', false).text(originalText);
        }
    }

    /**
     * Gets selected schools from the table with their visit details
     * @param {string} fieldScore - Field prefix for selectors
     * @returns {Array} Array of selected school objects
     */
    function getSchools(fieldScore) {
        const schools = [];

        // Get only selected schools
        $('#' + fieldScore + 'planTable tbody .selectRow:checked').each(function () {
            const checkbox = $(this);
            const schoolId = checkbox.data('school-id');
            const schoolName = checkbox.data('name');
            const row = checkbox.closest('tr');

            const dateRangeInput = row.find('.childDate');
            const dateRangeValue = dateRangeInput.val() || '';
            const parsedDates = parseDateRange(dateRangeValue);

            const visitTypeSelect = row.find('.visitTypeSelect');
            const visitTypeId = visitTypeSelect.val() || null;

            const schoolData = {
                id: schoolId,
                startEvaluationDate: parsedDates.startDate || null,
                endEvaluationDate: parsedDates.endDate || null,
                visitTypeId: visitTypeId,
                name: schoolName
            };

            schools.push(schoolData);
        });

        return schools;
    }


    /**
     * Main submit handler
     */
    async function handleSubmit(event) {
        event.preventDefault();

        clearErrors();

        // Get fieldId from namespace
        const fieldId = ns?.fieldId || '';

        const evaluationData = getFormPlanJson(fieldId);
        //const validation = validatePlan(evaluationData);

        //if (!validation.isValid) {
        //    displayErrors(validation.errors);
        //    return;
        //}

        await submitEvaluationData(evaluationData);
    }

    /**
     * NEW: Validation BEFORE opening modal (when clicking "Save")
     */
    $(document).on('click', '#openSaveModal', function (e) {
        e.preventDefault();

        clearErrors();

        // Get fieldId from namespace
        const fieldId = ns?.fieldId || '';

        const evaluationData = getFormPlanJson(fieldId);
        const validation = validatePlan(evaluationData);

        if (!validation.isValid) {
            displayErrors(fieldId, validation.errors);
            return;
        }

        const selectedCount = evaluationData.schools.length;
        $('#confirmationMessage').text(`(${selectedCount}) school(s) selected for adding to the plan`);

        const modal = new bootstrap.Modal(document.getElementById('confirmation-modal'));
        modal.show();
    });

    // ================== EVENT BINDING ==================

    $(document).on('click', '#btn-submit', handleSubmit);
    $(document).on('click', '#confirmSubmitBtn', handleSubmit);

    // ================== EXPORTS ==================
    window.getFormPlanJson = getFormPlanJson;
    window.validatePlan = validatePlan;

    window.SubmitPlanHandler = {
        getFormPlanJson,
        validatePlan,
        submitEvaluationData,
        getSchools
    };

})();