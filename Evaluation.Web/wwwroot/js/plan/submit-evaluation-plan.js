/**
 * Submit Evaluation Plan Handler
 * Updated to integrate with plan.constants.js, plan.generate-components.js, and plan.main-handler.js
 */

(function () {
    const { API_ENDPOINTS, PLAN_TYPE_BACKEND, ACTION_TYPE } = window.PlanConstants || {};
    const ns = window.planUtility;

    /**
     * Collects and validates evaluation plan data from the form
     * @returns {Object|null} Evaluation data object or null if validation fails
     */
    function getEvaluationData() {
        try {
            // Get plan name from the title input
            const name = $('#planTitle').val()?.trim() || '';

            // Get plan type from the select dropdown
            const planTypeSelect = $('#ddlPlanType');
            const selectedPlanType = planTypeSelect.select2('data')[0];
            const planTypeId = selectedPlanType?.id || '';
            const planTypeBackendName = selectedPlanType?.backendName ||
                selectedPlanType?.element?.dataset?.backendname || '';

            // Get date range and parse start/end dates
            const dateRangeInput = $('#parentDate');
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
                const semesterSelect = $('#ddlSemester');
                semesterId = semesterSelect.val() || null;
            }

            // Get selected schools from the utility namespace
            const selectedSchools = getSelectedSchools();

            // Return structured data object
            const evaluationData = {
                name,
                planTypeId,
                startDate,
                endDate,
                schools: selectedSchools
            };

            if (planTypeBackendName === PLAN_TYPE_BACKEND.SEMESTER && semesterId) {
                evaluationData.semesterId = semesterId;
            }

            // Add plan ID if editing existing plan
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

    /**
     * Validates evaluation data before submission
     * @param {Object} data - Evaluation data object
     * @returns {Object} Validation result with isValid flag and errors array
     */
    function validateEvaluationData(data) {
        const errors = [];

        if (!data) {
            errors.push('›‘· ›Ì Ã„⁄ »Ì«‰«  «·‰„Ê–Ã');
            return { isValid: false, errors };
        }

        if (!data.name || data.name.length < 3) {
            errors.push('⁄‰Ê«‰ «·Œÿ… „ÿ·Ê» ÊÌÃ» √‰ ÌﬂÊ‰ 3 √Õ—› ⁄·Ï «·√ﬁ·');
        }

        if (!data.planTypeId) {
            errors.push('‰Ê⁄ «·Œÿ… „ÿ·Ê»');
        }

        if (!data.startDate || !data.endDate) {
            errors.push('«·› —… «·“„‰Ì… „ÿ·Ê»…');
        }

        if (!data.schools || data.schools.length === 0) {
            errors.push('ÌÃ»  ÕœÌœ „œ—”… Ê«Õœ… ⁄·Ï «·√ﬁ·');
        }

        // Validate each school has required fields
        if (data.schools && data.schools.length > 0) {
            data.schools.forEach((school, index) => {
                if (!school.startEvaluationDate || !school.endEvaluationDate) {
                    errors.push(`«·„œ—”… ${index + 1}:  «—ÌŒ «·“Ì«—… „ÿ·Ê»`);
                }
                if (!school.visitTypeId) {
                    errors.push(`«·„œ—”… ${index + 1}: ‰Ê⁄ «·“Ì«—… „ÿ·Ê»`);
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
    function displayErrors(errors) {
        // Display errors as alert
        alert('Ì—ÃÏ ≈’·«Õ «·√Œÿ«¡ «· «·Ì…:\n\n' + errors.join('\n'));

        // Log to console for debugging
        console.error('Validation errors:', errors);

        // Add invalid class to form fields
        if (errors.some(e => e.includes('⁄‰Ê«‰'))) {
            $('#planTitle').addClass('is-invalid');
        }
        if (errors.some(e => e.includes('‰Ê⁄ «·Œÿ…'))) {
            $('#ddlPlanType').next('.select2-container').addClass('is-invalid');
        }
        if (errors.some(e => e.includes('«·› —… «·“„‰Ì…'))) {
            $('#parentDate').addClass('is-invalid');
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
            // Show loading state
            submitBtn.prop('disabled', true).text('Ã«—Ì «·Õ›Ÿ...');

            // Determine endpoint based on action type
            let endpoint = API_ENDPOINTS.APPROVE_PLAN;
            //let endpoint = API_ENDPOINTS.CREATE_PLAN;

            if (ns.currentActionType === ACTION_TYPE.EDIT ||
                ns.currentActionType === ACTION_TYPE.EDIT_DRAFT) {
                endpoint = API_ENDPOINTS.UPDATE_PLAN;
            } else if (ns.currentActionType === ACTION_TYPE.APPROVE ||
                ns.currentActionType === ACTION_TYPE.APPROVE_WITH_CHANGES) {
                endpoint = API_ENDPOINTS.APPROVE_PLAN;
            }

            // Submit data
            const result = await jqClient().Post(endpoint, data);

            if (result.success) {
                // Close confirmation modal if open
                const modal = bootstrap.Modal.getInstance(document.getElementById('confirmation-modal'));
                if (modal) modal.hide();

                // Show success message
                alert(' „ Õ›Ÿ «·Œÿ… »‰Ã«Õ');

                // Redirect to plans list
                window.location.href = '/Plan/Index';
            } else {
                throw new Error(result.message || '›‘· ›Ì Õ›Ÿ «·Œÿ…');
            }

        } catch (error) {
            console.error('Submission error:', error);
            alert('ÕœÀ Œÿ√ √À‰«¡ Õ›Ÿ «·Œÿ…. Ì—ÃÏ «·„Õ«Ê·… „—… √Œ—Ï.');
        } finally {
            // Restore button state
            submitBtn.prop('disabled', false).text(originalText);
        }
    }

    /**
     * Gets selected schools from the table with their visit details
     * @returns {Array} Array of selected school objects
     */
    function getSelectedSchools() {
        const selectedSchools = [];

        $('#planTable tbody .selectRow:checked').each(function () {
            const checkbox = $(this);
            const schoolId = checkbox.data('id');
            const row = checkbox.closest('tr');

            // Get visit date range
            const dateRangeInput = row.find('.childDate');
            const dateRangeValue = dateRangeInput.val() || '';
            const parsedDates = parseDateRange(dateRangeValue);

            // Get visit type
            const visitTypeSelect = row.find('.visitTypeSelect');
            const visitTypeId = visitTypeSelect.val() || '';

            // Create school data object
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

    /**
     * Main submit handler
     */
    async function handleSubmit(event) {
        event.preventDefault();

        // Clear previous errors
        clearErrors();

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
    }

    // ================== EVENT BINDING ==================

    $(document).ready(function () {
        // Bind submit button click event
        $(document).on('click', '#btn-submit', handleSubmit);

        // Alternative: Bind to confirmation modal's confirm button
        $(document).on('click', '#confirmSubmitBtn', handleSubmit);
    });

    // ================== EXPORTS ==================

    window.SubmitPlanHandler = {
        getEvaluationData,
        validateEvaluationData,
        submitEvaluationData,
        getSelectedSchools
    };

})();