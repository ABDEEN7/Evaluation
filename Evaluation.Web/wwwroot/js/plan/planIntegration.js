// ============= SCHOOL PLANNING FORM INTEGRATION =============
// This file integrates the school selection system with the dynamic form builder

(function (global) {
    'use strict';

    const SchoolPlanForm = {
        // Constants
        FORM_TYPE: {
            SCHOOL_SELECTION: 'school_selection',
            PLAN_CREATION: 'plan_creation'
        },

        // State management
        state: {
            selectedSchools: [],
            planData: null,
            formFields: []
        },

        // Initialize the form system
        init: function () {
            this.initializePlanFormFields();
            this.attachEventHandlers();
            this.syncWithExistingSystem();
        },

        // Define form fields based on the CSHTML structure
        initializePlanFormFields: function () {
            this.state.formFields = [
                {
                    fieldId: 'planTitle',
                    fieldName: 'عنوان الخطة',
                    type: 'text',
                    required: true,
                    row: 1,
                    column: 1,
                    attributes: [{ name: 'required', value: 'true' }],
                    validation: {
                        required: true,
                        message: 'يرجى إدخال عنوان الخطة'
                    }
                },
                {
                    fieldId: 'ddlPlanType',
                    fieldName: 'نوع الخطة',
                    type: 'select2',
                    required: true,
                    row: 1,
                    column: 2,
                    dropDownTypeId: 'planTypes',
                    attributes: [{ name: 'required', value: 'true' }],
                    validation: {
                        required: true,
                        message: 'يرجى اختيار نوع الخطة'
                    }
                },
                {
                    fieldId: 'ddlSemester',
                    fieldName: 'الفصل الدراسي',
                    type: 'select2',
                    required: false,
                    row: 1,
                    column: 3,
                    dropDownTypeId: 'semesters',
                    conditions: [
                        {
                            parentFieldId: 'ddlPlanType',
                            operators: 'equal',
                            fieldValue: 'Semester'
                        }
                    ],
                    attributes: [{ name: 'hiddenFieldEditList', value: 'true' }]
                },
                {
                    fieldId: 'parentDate',
                    fieldName: 'الفترة الزمنية',
                    type: 'date',
                    required: true,
                    row: 2,
                    column: 1,
                    attributes: [
                        { name: 'required', value: 'true' },
                        { name: 'data-input', value: 'true' }
                    ],
                    validation: {
                        required: true,
                        message: 'يرجى اختيار الفترة الزمنية'
                    }
                }
            ];
        },

        // Attach event handlers
        attachEventHandlers: function () {
            const self = this;

            // Handle select all checkbox
            $(document).on('change', '#selectAll', function () {
                const isChecked = $(this).is(':checked');
                $('.selectRow').prop('checked', isChecked);
                self.updateSelectedSchools();
            });

            // Handle individual row selection
            $(document).on('change', '.selectRow', function () {
                self.updateSelectedSchools();
                self.updateSelectAllState();
            });

            // Handle visit type changes
            $(document).on('change', '.visitTypeSelect', function () {
                const schoolId = $(this).data('school-id');
                const visitType = $(this).val();
                self.updateSchoolVisitType(schoolId, visitType);
            });

            // Handle date changes for individual schools
            $(document).on('change', '.childDate', function () {
                const schoolId = $(this).data('school-id');
                const dateRange = $(this).val();
                self.updateSchoolDateRange(schoolId, dateRange);
            });

            // Handle save button
            $(document).on('click', '#btn-submit', function (e) {
                e.preventDefault();
                self.submitPlan();
            });

            // Handle form validation
            $('#planForm').on('submit', function (e) {
                e.preventDefault();
                if (self.validatePlanForm()) {
                    self.showSchoolSelection();
                }
            });
        },

        // Sync with existing system
        syncWithExistingSystem: function () {
            // Hook into the existing loadSchoolsData function
            if (typeof loadSchoolsData === 'function') {
                const originalLoadSchools = loadSchoolsData;
                loadSchoolsData = function (...args) {
                    originalLoadSchools.apply(this, args);
                    SchoolPlanForm.onSchoolsLoaded();
                };
            }

            // Hook into the existing plan type change handler
            $('#ddlPlanType').on('change', function () {
                SchoolPlanForm.handlePlanTypeChange($(this).val());
            });
        },

        // Callback when schools are loaded
        onSchoolsLoaded: function () {
            this.updateConfirmationModal();
            this.restoreSelections();
        },

        // Update selected schools
        updateSelectedSchools: function () {
            this.state.selectedSchools = [];
            $('.selectRow:checked').each((index, element) => {
                const schoolId = $(element).data('id');
                const row = $(element).closest('tr');

                const schoolData = {
                    id: schoolId,
                    name: row.find('h6').text().trim(),
                    visitType: row.find('.visitTypeSelect').val(),
                    dateRange: row.find('.childDate').val(),
                    level: row.find('.square-bullet div').text().trim()
                };

                this.state.selectedSchools.push(schoolData);
            });

            this.updateConfirmationModal();
        },

        // Update select all checkbox state
        updateSelectAllState: function () {
            const totalCheckboxes = $('.selectRow').length;
            const checkedCheckboxes = $('.selectRow:checked').length;

            $('#selectAll').prop('checked', totalCheckboxes === checkedCheckboxes && totalCheckboxes > 0);
        },

        // Update school visit type
        updateSchoolVisitType: function (schoolId, visitType) {
            const school = this.state.selectedSchools.find(s => s.id === schoolId);
            if (school) {
                school.visitType = visitType;
            }
        },

        // Update school date range
        updateSchoolDateRange: function (schoolId, dateRange) {
            const school = this.state.selectedSchools.find(s => s.id === schoolId);
            if (school) {
                school.dateRange = dateRange;
            }
        },

        // Validate plan form
        validatePlanForm: function () {
            let isValid = true;
            const errors = [];

            // Validate title
            const title = $('#planTitle').val().trim();
            if (!title) {
                errors.push('يرجى إدخال عنوان الخطة');
                $('#planTitle').addClass('is-invalid');
                isValid = false;
            } else {
                $('#planTitle').removeClass('is-invalid');
            }

            // Validate plan type
            const planType = $('#ddlPlanType').val();
            if (!planType) {
                errors.push('يرجى اختيار نوع الخطة');
                $('#ddlPlanType').addClass('is-invalid');
                isValid = false;
            } else {
                $('#ddlPlanType').removeClass('is-invalid');
            }

            // Validate date range
            const dateRange = $('#parentDate').val();
            if (!dateRange) {
                errors.push('يرجى اختيار الفترة الزمنية');
                $('#parentDate').addClass('is-invalid');
                isValid = false;
            } else {
                $('#parentDate').removeClass('is-invalid');
            }

            // Validate semester if plan type is semester
            const selectedOption = $('#ddlPlanType').select2('data')[0];
            if (selectedOption && selectedOption.backendName === 'Semester') {
                const semester = $('#ddlSemester').val();
                if (!semester) {
                    errors.push('يرجى اختيار الفصل الدراسي');
                    $('#ddlSemester').addClass('is-invalid');
                    isValid = false;
                } else {
                    $('#ddlSemester').removeClass('is-invalid');
                }
            }

            if (!isValid) {
                this.showValidationErrors(errors);
            }

            return isValid;
        },

        // Show validation errors
        showValidationErrors: function (errors) {
            const errorHtml = errors.map(err => `<div class="alert alert-danger">${err}</div>`).join('');
            // You can append this to a specific container or use toastr
            console.error('Validation errors:', errors);
        },

        // Show school selection section
        showSchoolSelection: function () {
            // Scroll to schools table
            $('html, body').animate({
                scrollTop: $('#planTable').offset().top - 100
            }, 500);
        },

        // Update confirmation modal
        updateConfirmationModal: function () {
            const count = this.state.selectedSchools.length;
            $('#confirmationMessage').text(`تم تحديد (${count}) مدرسة للإضافة للخطة`);
        },

        // Validate school selections
        validateSchoolSelections: function () {
            const errors = [];

            if (this.state.selectedSchools.length === 0) {
                errors.push('يرجى تحديد مدرسة واحدة على الأقل');
                return { isValid: false, errors };
            }

            // Validate each selected school
            this.state.selectedSchools.forEach((school, index) => {
                if (!school.visitType) {
                    errors.push(`يرجى تحديد نوع الزيارة للمدرسة: ${school.name}`);
                }

                if (!school.dateRange) {
                    errors.push(`يرجى تحديد تاريخ الزيارة للمدرسة: ${school.name}`);
                }
            });

            return {
                isValid: errors.length === 0,
                errors
            };
        },

        // Handle plan type change
        handlePlanTypeChange: function (value) {
            // This is already handled by dynamic-plan-handler.js
            // But we can add additional logic here if needed
            console.log('Plan type changed to:', value);
        },

        // Submit the plan
        submitPlan: function () {
            // Validate form
            if (!this.validatePlanForm()) {
                return;
            }

            // Validate school selections
            const schoolValidation = this.validateSchoolSelections();
            if (!schoolValidation.isValid) {
                alert(schoolValidation.errors.join('\n'));
                return;
            }

            // Prepare data
            const planData = {
                title: $('#planTitle').val(),
                planTypeId: $('#ddlPlanType').val(),
                semesterId: $('#ddlSemester').val(),
                dateRange: $('#parentDate').val(),
                schools: this.state.selectedSchools.map(school => ({
                    schoolId: school.id,
                    visitTypeId: school.visitType,
                    visitDateRange: school.dateRange
                }))
            };

            // Show loading
            if (typeof formField !== 'undefined' && formField.coverSpin) {
                formField.coverSpin(true);
            }

            // Submit to API
            this.submitToAPI(planData);
        },

        // Submit to API
        submitToAPI: function (planData) {
            const self = this;

            jqClient().Post('/Plan/CreateEvaluationPlan', planData)
                .done(function (result) {
                    if (result && result.success) {
                        self.onSubmitSuccess(result);
                    } else {
                        self.onSubmitError(result.message || 'حدث خطأ أثناء حفظ الخطة');
                    }
                })
                .fail(function (jqXHR, textStatus, err) {
                    console.error('Submit failed', textStatus, err);
                    self.onSubmitError('فشل الاتصال بالخادم');
                })
                .always(function () {
                    if (typeof formField !== 'undefined' && formField.coverSpin) {
                        formField.coverSpin(false);
                    }
                });
        },

        // Handle submit success
        onSubmitSuccess: function (result) {
            // Close modal
            $('#confirmation-modal').modal('hide');

            // Show success message
            alert('تم حفظ الخطة بنجاح');

            // Reset form
            this.resetForm();

            // Redirect or reload
            window.location.href = result.redirectUrl || '/Plan/Index';
        },

        // Handle submit error
        onSubmitError: function (message) {
            alert(message);
        },

        // Reset form
        resetForm: function () {
            $('#planForm')[0].reset();
            $('#ddlPlanType').val(null).trigger('change');
            $('#ddlSemester').val(null).trigger('change');
            this.state.selectedSchools = [];
            $('.selectRow').prop('checked', false);
            $('#selectAll').prop('checked', false);
            this.updateConfirmationModal();
        },

        // Restore previous selections (for edit mode)
        restoreSelections: function () {
            if (this.state.selectedSchools.length > 0) {
                this.state.selectedSchools.forEach(school => {
                    const checkbox = $(`.selectRow[data-id="${school.id}"]`);
                    checkbox.prop('checked', true);

                    const row = checkbox.closest('tr');
                    row.find('.visitTypeSelect').val(school.visitType);
                    row.find('.childDate').val(school.dateRange);
                });

                this.updateSelectAllState();
            }
        },

        // Load plan for editing
        loadPlan: function (planId) {
            const self = this;

            if (typeof formField !== 'undefined' && formField.coverSpin) {
                formField.coverSpin(true);
            }

            jqClient().Get(`/Plan/GetPlan/${planId}`)
                .done(function (result) {
                    if (result && result.success) {
                        self.populatePlanData(result.data);
                    }
                })
                .fail(function (jqXHR, textStatus, err) {
                    console.error('Load plan failed', textStatus, err);
                })
                .always(function () {
                    if (typeof formField !== 'undefined' && formField.coverSpin) {
                        formField.coverSpin(false);
                    }
                });
        },

        // Populate plan data for editing
        populatePlanData: function (data) {
            $('#planTitle').val(data.title);
            $('#ddlPlanType').val(data.planTypeId).trigger('change');

            if (data.semesterId) {
                setTimeout(() => {
                    $('#ddlSemester').val(data.semesterId).trigger('change');
                }, 500);
            }

            $('#parentDate').val(data.dateRange);

            // Store selected schools
            this.state.selectedSchools = data.schools || [];

            // Restore selections after schools are loaded
            setTimeout(() => {
                this.restoreSelections();
            }, 1000);
        }
    };

    // Export to global scope
    global.SchoolPlanForm = SchoolPlanForm;

    // Auto-initialize on document ready
    $(document).ready(function () {
        SchoolPlanForm.init();

        // Check if we're in edit mode
        const urlParams = new URLSearchParams(window.location.search);
        const planId = urlParams.get('planId');
        if (planId) {
            SchoolPlanForm.loadPlan(planId);
        }
    });

})(window);