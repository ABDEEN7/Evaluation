window.planUtility = window.planUtility || {};
const planUtility = window.planUtility;

(function (ns) {

    // ================== CONSTANTS ==================
    const {
        RENDER_TYPE,
        ACTION_TYPE,
        ReadOnly_ACTION_TYPES,
        PLAN_FIELD_TYPE,
        SCHOOL_FIELD_TYPE,
        VALIDATION_RULES,
        RATING_CLASSES,
        PLAN_TYPE_BACKEND
    } = window.PlanConstants || {};

    // ================== STATE MANAGEMENT ==================
    ns.allSchools = [];
    ns.filteredSchools = [];
    ns.selectedSchools = [];
    ns.visitTypes = [];
    ns.planTypes = [];
    ns.semesters = [];
    ns.holidays = [];
    ns.currentPage = 1;
    ns.pageSize = 10;
    ns.currentFilters = {};
    ns.childPicker = null;
    ns.parentPickerInstance = null;

    // ================== HELPER FUNCTIONS ==================

    const formatDateISO = (date) => {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    };

    const isHoliday = (date) => {
        const dateStr = formatDateISO(date);
        return ns.holidays.some(h => h.date === dateStr);
    };

    const isReadOnly = (renderType, actionType) => {
        return renderType === RENDER_TYPE.PREVIEW ||
            ReadOnly_ACTION_TYPES.includes(actionType);
    };

    // ================== PLAN FORM FIELD GENERATORS ==================

    const generateTitleField = (field, readonly) => {
        const inputElement = $('<input>')
            .attr('type', 'text')
            .attr('id', 'planTitle')
            .attr('name', 'PlanTitle')
            .addClass('form-control')
            .attr('placeholder', 'أدخل عنوان الخطة')
            .val(field?.value || '');

        if (readonly) {
            inputElement.prop('readonly', true).prop('disabled', true);
        }

        if (VALIDATION_RULES.TITLE.required) {
            inputElement.attr('required', true);
        }

        return inputElement;
    };

    const generatePlanTypeField = (field, readonly) => {
        const selectElement = $('<select>')
            .attr('id', 'ddlPlanType')
            .attr('name', 'PlanTypeId')
            .addClass('form-control')
            .append($('<option>').val('').text('اختر نوع الخطة'));

        if (readonly) {
            selectElement.prop('disabled', true);
        }

        if (VALIDATION_RULES.PLAN_TYPE.required) {
            selectElement.attr('required', true);
        }

        // Populate with plan types
        ns.planTypes.forEach(type => {
            const option = $('<option>')
                .val(type.id)
                .text(type.name)
                .attr('data-backendname', type.backendName);
            //.data('backendName', type.backendName);

            if (field?.value === type.id) {
                option.prop('selected', true);
            }

            selectElement.append(option);
        });

        return selectElement;
    };

    const generateSemesterField = (field, readonly) => {
        const container = $('<div>')
            .attr('id', 'semesterContainer')
            .addClass('col-md-4')
            .css('display', field?.visible !== false ? 'block' : 'none');

        const selectElement = $('<select>')
            .attr('id', 'ddlSemester')
            .attr('name', 'SemesterId')
            .addClass('form-control')
            .append($('<option>').val('').text('اختر الفصل الدراسي'));

        if (readonly) {
            selectElement.prop('disabled', true);
        }

        // Populate with semesters
        ns.semesters.forEach(semester => {
            const option = $('<option>')
                .val(semester.id)
                .text(semester.name)
                .data('startDate', semester.startDate)
                .data('endDate', semester.endDate);

            if (field?.value === semester.id) {
                option.prop('selected', true);
            }

            selectElement.append(option);
        });

        const label = $('<label>')
            .addClass('form-label')
            .attr('for', 'ddlSemester')
            .html('الفصل الدراسي <span class="text-danger">*</span>');

        const formGroup = $('<div>').addClass('mb-4');
        formGroup.append(label, selectElement);

        if (!readonly) {
            formGroup.append($('<div>').addClass('invalid-feedback').text('يرجى اختيار الفصل الدراسي'));
        }

        container.append(formGroup);
        return container;
    };

    const generateDateRangeField = (field, readonly) => {
        const inputElement = $('<input>')
            .attr('type', 'text')
            .attr('id', 'parentDate')
            .attr('name', 'dateRange')
            .addClass('form-control')
            .attr('placeholder', 'اختر تاريخ بداية ونهاية الخطة')
            .val(field?.value || '');

        if (readonly) {
            inputElement.prop('readonly', true).prop('disabled', true);
        } else {
            inputElement.attr('required', true);
        }

        const inputGroup = $('<div>').addClass('input-group datetime');
        inputGroup.append(
            inputElement,
            $('<span>').addClass('input-group-text').html('<i class="la la-calendar"></i>')
        );

        return inputGroup;
    };

    // ================== SCHOOL TABLE FIELD GENERATORS ==================

    const generateSelectCheckbox = (school, readonly, isComparison = false) => {
        const label = $('<label>').addClass('custom-checkbox');

        const checkbox = $('<input>')
            .attr('type', 'checkbox')
            .addClass('selectRow')
            .attr('data-id', school.id);

        if (readonly) {
            checkbox.prop('disabled', true);
        }

        // Check if this school is in selected schools
        if (ns.selectedSchools.some(s => s.id === school.id)) {
            checkbox.prop('checked', true);
        }

        const checkmark = $('<span>').addClass('checkmark');

        label.append(checkbox, checkmark);
        return label;
    };

    const generateSchoolNameCell = (school) => {
        const ratingClass = RATING_CLASSES[school.rating] || 'bg-light';

        const container = $('<div>').addClass('d-flex align-items-center justify-content-between');

        const infoDiv = $('<div>');
        infoDiv.append($('<h6>').text(school.name || '-'));

        const levelBadge = $('<div>').addClass('square-bullet');
        const levelText = (school.schoolLevel && school.schoolLevel.length > 0)
            ? school.schoolLevel.map(l => l.name).join(', ')
            : '-';
        levelBadge.append($('<div>').text(levelText));
        infoDiv.append(levelBadge);

        const ratingBadge = $('<span>')
            .addClass(`badge ${ratingClass}`)
            .text(school.rating || '');

        container.append(infoDiv, ratingBadge);
        return container;
    };

    const generateVisitDateField = (school, readonly, renderType) => {
        const inputElement = $('<input>')
            .attr('type', 'text')
            .addClass('form-control form-control-sm childDate')
            .attr('placeholder', 'اختر تاريخ بداية ونهاية الزيارة')
            .attr('data-school-id', school.id)
            .val(school.visitDate || '')
            .prop('readonly', true); // Always readonly for flatpickr

        if (readonly) {
            inputElement.prop('disabled', true);
        }

        return inputElement;
    };

    const generateVisitTypeField = (school, readonly) => {
        const selectElement = $('<select>')
            .addClass('form-select visitTypeSelect')
            .attr('data-school-id', school.id)
            .append($('<option>').val('').text('اختر نوع الزيارة'));

        ns.visitTypes.forEach(type => {
            const option = $('<option>')
                .val(type.id)
                .text(type.name);

            if (school.visitType === type.name || school.visitTypeId === type.id) {
                option.prop('selected', true);
            }

            selectElement.append(option);
        });

        if (readonly) {
            selectElement.prop('disabled', true);
        }

        return selectElement;
    };

    const generateActionsCell = (school, readonly) => {
        const container = $('<p>').addClass('m-0');

        const link = $('<a>')
            .attr('href', '#')
            .addClass('text-dark')
            .attr('type', 'button')
            .attr('data-bs-toggle', 'modal')
            .attr('data-bs-target', '#schoolDetailsModal')
            .attr('data-id', school.id)
            .html('<i class="la la-eye"></i>');

        container.append(link);
        return container;
    };

    // ================== TABLE ROW GENERATOR ==================

    const generateSchoolRow = (school, renderType, actionType) => {
        const readonly = isReadOnly(renderType, actionType);
        const row = $('<tr>');

        // Checkbox cell
        const checkboxCell = $('<td>');
        checkboxCell.append(generateSelectCheckbox(school, readonly));
        row.append(checkboxCell);

        // School name cell
        const nameCell = $('<td>');
        nameCell.append(generateSchoolNameCell(school));
        row.append(nameCell);

        // Visit date cell
        const visitDateCell = $('<td>');
        visitDateCell.append(generateVisitDateField(school, readonly, renderType));
        row.append(visitDateCell);

        // Last evaluation date cell
        const lastEvalCell = $('<td>');
        lastEvalCell.text(school.lastEvaluationDate || '-');
        row.append(lastEvalCell);

        // Visit type cell
        const visitTypeCell = $('<td>');
        visitTypeCell.append(generateVisitTypeField(school, readonly));
        row.append(visitTypeCell);

        // Academic year cell
        const academicYearCell = $('<td>');
        academicYearCell.text(school.academicYear || '-');
        row.append(academicYearCell);

        // Actions cell
        const actionsCell = $('<td>');
        actionsCell.append(generateActionsCell(school, readonly));
        row.append(actionsCell);

        return row;
    };

    // ================== COMPARISON VIEW GENERATOR ==================

    const generateComparisonView = (oldPlan, newPlan) => {
        const container = $('<div>').addClass('comparison-container row');

        // Old Plan Column
        const oldColumn = $('<div>').addClass('col-md-6 border-end');
        oldColumn.append($('<h4>').addClass('text-muted mb-3').text('الخطة القديمة'));
        oldColumn.append(renderPlanForm(oldPlan, RENDER_TYPE.PREVIEW, ACTION_TYPE.VIEW));
        oldColumn.append(renderSchoolTable(oldPlan.schools || [], RENDER_TYPE.PREVIEW, ACTION_TYPE.VIEW));

        // New Plan Column
        const newColumn = $('<div>').addClass('col-md-6');
        newColumn.append($('<h4>').addClass('text-primary mb-3').text('الخطة الجديدة'));
        newColumn.append(renderPlanForm(newPlan, RENDER_TYPE.PREVIEW, ACTION_TYPE.VIEW));
        newColumn.append(renderSchoolTable(newPlan.schools || [], RENDER_TYPE.PREVIEW, ACTION_TYPE.VIEW));

        container.append(oldColumn, newColumn);
        return container;
    };

    // ================== RENDER FUNCTIONS ==================

    const renderPlanForm = (planData, renderType, actionType) => {
        const readonly = isReadOnly(renderType, actionType);
        const form = $('<form>').addClass('row').attr('id', 'planForm');

        // Title Field
        const titleCol = $('<div>').addClass('col-md-4');
        const titleGroup = $('<div>').addClass('mb-4');
        const titleLabel = $('<label>')
            .addClass('form-label')
            .attr('for', 'planTitle')
            .html('عنوان <span class="text-danger">*</span>');

        const titleField = generateTitleField({ value: planData?.title }, readonly);
        titleGroup.append(titleLabel, titleField);

        if (!readonly) {
            titleGroup.append($('<div>').addClass('invalid-feedback').text('يرجى إدخال عنوان الخطة'));
        }

        titleCol.append(titleGroup);
        form.append(titleCol);

        // Plan Type Field
        const planTypeCol = $('<div>').addClass('col-md-4');
        const planTypeGroup = $('<div>').addClass('mb-4');
        const planTypeLabel = $('<label>')
            .addClass('form-label')
            .attr('for', 'ddlPlanType')
            .html('نوع الخطة <span class="text-danger">*</span>');

        const planTypeField = generatePlanTypeField({ value: planData?.planTypeId }, readonly);
        planTypeGroup.append(planTypeLabel, planTypeField);

        if (!readonly) {
            planTypeGroup.append($('<div>').addClass('invalid-feedback').text('يرجى اختيار نوع الخطة'));
        }

        planTypeCol.append(planTypeGroup);
        form.append(planTypeCol);

        // Semester Field (conditionally visible)
        const semesterField = generateSemesterField({
            value: planData?.semesterId,
            visible: planData?.showSemester
        }, readonly);
        form.append(semesterField);

        // Date Range Field
        const dateRangeCol = $('<div>').addClass('col-md-4');
        const dateRangeGroup = $('<div>').addClass('mb-4');
        const dateRangeLabel = $('<label>')
            .addClass('form-label')
            .attr('for', 'parentDate')
            .html('الفترة الزمنية <span class="text-danger">*</span>');

        const dateRangeField = generateDateRangeField({ value: planData?.dateRange }, readonly);
        dateRangeGroup.append(dateRangeLabel, dateRangeField);

        if (!readonly) {
            dateRangeGroup.append($('<div>').addClass('invalid-feedback').text('يرجى اختيار الفترة الزمنية'));
        }

        dateRangeCol.append(dateRangeGroup);
        form.append(dateRangeCol);

        return form;
    };

    const renderSchoolTable = (schools, renderType, actionType) => {
        const readonly = isReadOnly(renderType, actionType);
        const tbody = $('<tbody>');

        if (!schools || schools.length === 0) {
            const emptyRow = $('<tr>');
            emptyRow.append(
                $('<td>')
                    .attr('colspan', '7')
                    .addClass('text-center text-muted')
                    .text('لا توجد بيانات')
            );
            tbody.append(emptyRow);
        } else {
            schools.forEach(school => {
                const row = generateSchoolRow(school, renderType, actionType);
                tbody.append(row);
            });
        }

        return tbody;
    };

    const renderPagination = (totalRecords) => {
        const totalPages = Math.ceil(totalRecords / ns.pageSize);
        const pagination = $('<ul>').addClass('pagination pagination-sm mb-0');

        if (totalPages <= 1) return pagination;

        // Previous button
        if (ns.currentPage > 1) {
            const prevItem = $('<li>').addClass('page-item');
            const prevLink = $('<a>')
                .addClass('page-link')
                .attr('href', '#')
                .attr('data-page', ns.currentPage - 1)
                .text('السابق');
            prevItem.append(prevLink);
            pagination.append(prevItem);
        }

        // Page numbers
        for (let i = 1; i <= totalPages; i++) {
            const pageItem = $('<li>').addClass('page-item');
            if (i === ns.currentPage) {
                pageItem.addClass('active');
            }

            const pageLink = $('<a>')
                .addClass('page-link')
                .attr('href', '#')
                .attr('data-page', i)
                .text(i);

            pageItem.append(pageLink);
            pagination.append(pageItem);
        }

        // Next button
        if (ns.currentPage < totalPages) {
            const nextItem = $('<li>').addClass('page-item');
            const nextLink = $('<a>')
                .addClass('page-link')
                .attr('href', '#')
                .attr('data-page', ns.currentPage + 1)
                .text('التالي');
            nextItem.append(nextLink);
            pagination.append(nextItem);
        }

        return pagination;
    };

    // ================== FLATPICKR INITIALIZATION ==================

    const initParentPicker = (mode, minDate, maxDate) => {
        if (ns.parentPickerInstance) {
            ns.parentPickerInstance.destroy();
            ns.parentPickerInstance = null;
        }

        const config = {
            locale: "en",
            allowInput: true,
            onDayCreate: function (dObj, dStr, fp, dayElem) {
                if (isHoliday(dayElem.dateObj)) {
                    dayElem.classList.add('blocked');
                }
            }
        };

        // Handle disabled mode
        if (mode === 'disabled') {
            $('#parentDate').prop('disabled', true);
            return;
        }

        // Enable the input
        $('#parentDate').prop('disabled', false);

        // Configure based on mode
        if (mode === 'month') {
            // Month picker mode using monthSelectPlugin
            config.plugins = [
                new monthSelectPlugin({
                    shorthand: true,
                    dateFormat: "m.y",
                    altFormat: "F Y",
                    theme: "light" // or "dark" based on your theme
                })
            ];

            config.onChange = function (selectedDates, dateStr, instance) {
                if (selectedDates.length > 0) {
                    const selectedDate = selectedDates[0];
                    const year = selectedDate.getFullYear();
                    const month = selectedDate.getMonth();

                    // Get first and last day of selected month
                    const firstDay = new Date(year, month, 1);
                    const lastDay = new Date(year, month + 1, 0);

                    // Format the date range display
                    const rangeStr = `${formatDateISO(firstDay)} to ${formatDateISO(lastDay)}`;
                    $('#parentDate').val(rangeStr);

                    // Store the actual dates for form submission
                    $('#parentDate').data('startDate', formatDateISO(firstDay));
                    $('#parentDate').data('endDate', formatDateISO(lastDay));

                    // Initialize child pickers with month range
                    initChildPicker(firstDay, lastDay);
                } else {
                    destroyChildPicker();
                }
            };

        } else if (mode === 'custom') {
            // Custom date range picker mode
            config.mode = "range";
            config.dateFormat = "Y-m-d";

            if (minDate) config.minDate = minDate;
            if (maxDate) config.maxDate = maxDate;

            config.onClose = function (selectedDates, dateStr, instance) {
                if (selectedDates.length === 2) {
                    const [min, max] = selectedDates;
                    initChildPicker(min, max);
                } else if (selectedDates.length === 0) {
                    destroyChildPicker();
                }
            };
        }

        ns.parentPickerInstance = flatpickr("#parentDate", config);
    };

    const initChildPicker = (minDate, maxDate) => {
        if (ns.childPicker) {
            ns.childPicker.destroy();
            ns.childPicker = null;
        }

        ns.childPicker = flatpickr(".childDate", {
            mode: "range",
            locale: "en",
            dateFormat: "Y-m-d",
            allowInput: true,
            minDate: minDate,
            maxDate: maxDate,
            onReady: function (selectedDates, dateStr, instance) {
                const monthsContainer = instance.calendarContainer.querySelector('.flatpickr-months');
                const prev = instance.calendarContainer.querySelector('.flatpickr-prev-month');
                const next = instance.calendarContainer.querySelector('.flatpickr-next-month');

                const arrowStack = document.createElement('div');
                arrowStack.className = 'fp-arrow-stack';
                arrowStack.appendChild(prev);
                arrowStack.appendChild(next);

                monthsContainer.insertBefore(arrowStack, monthsContainer.firstChild);

                const monthYear = monthsContainer.querySelector('.flatpickr-current-month');
                monthsContainer.appendChild(monthYear);

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

        if (Array.isArray(ns.childPicker)) {
            ns.childPicker = ns.childPicker[0];
        }
    };

    const destroyChildPicker = () => {
        if (ns.childPicker) {
            ns.childPicker.destroy();
            ns.childPicker = null;
        }
    };

    // Helper function to get month range from selected date
    const getMonthRange = (date) => {
        const year = date.getFullYear();
        const month = date.getMonth();
        return {
            start: new Date(year, month, 1),
            end: new Date(year, month + 1, 0)
        };
    };

    // ================== EXPORTS ==================

    ns.generateTitleField = generateTitleField;
    ns.generatePlanTypeField = generatePlanTypeField;
    ns.generateSemesterField = generateSemesterField;
    ns.generateDateRangeField = generateDateRangeField;
    ns.generateSelectCheckbox = generateSelectCheckbox;
    ns.generateSchoolNameCell = generateSchoolNameCell;
    ns.generateVisitDateField = generateVisitDateField;
    ns.generateVisitTypeField = generateVisitTypeField;
    ns.generateActionsCell = generateActionsCell;
    ns.generateSchoolRow = generateSchoolRow;
    ns.generateComparisonView = generateComparisonView;
    ns.renderPlanForm = renderPlanForm;
    ns.renderSchoolTable = renderSchoolTable;
    ns.renderPagination = renderPagination;
    ns.initParentPicker = initParentPicker;
    ns.initChildPicker = initChildPicker;
    ns.destroyChildPicker = destroyChildPicker;
    ns.isReadOnly = isReadOnly;
    ns.formatDateISO = formatDateISO;
    ns.getMonthRange = getMonthRange;

})(planUtility);