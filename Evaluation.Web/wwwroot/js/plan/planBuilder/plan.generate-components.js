window.planUtility = window.planUtility || {};

(function (ns) {

    // =================== LOCALIZATION Helper===================
    function t(key, fallback = '') {
        const text = uiControlsSetup()?.GetUiControlText(key);
        return text || key;
    }

    // ================== PREFIX CONFIGURATION ==================
    ns.fieldIdPrefixes = new Map();

    ns.initializePrefix = function (fieldId) {
        if (fieldId) {
            ns.fieldIdPrefixes.set(fieldId, fieldId);
            ns.fieldIdPrefix = fieldId;
        }
    }

    ns.getPrefixedId = function (id, fieldId) {
        const prefix = fieldId || ns.fieldIdPrefix;
        if (!prefix || !id) return id;
        return `${prefix}_${id}`;
    };
    // ================== CONSTANTS ==================
    const {
        VALIDATION_RULES,
        RATING_CLASSES,
        PLAN_TYPE_BACKEND
    } = window.PlanConstants || {};

    // ================== STATE MANAGEMENT (Per Instance) ==================
    // ⚠️ تم نقل الـ state إلى main-handler ليكون منفصل لكل instance
    ns.visitTypes = [];
    ns.planTypes = [];
    ns.semesters = [];
    ns.holidays = [];
    ns.parentSchool = [];
    ns.fomrEvalMatrixValue = [];
    ns.currentPage = 1;
    ns.pageSize = 10;

    // ================== HELPER FUNCTIONS ==================

    const formatDateRange = (startDate, endDate) => {
        if (!startDate || !endDate) return '';
        const start = new Date(startDate);
        const end = new Date(endDate);
        if (isNaN(start.getTime()) || isNaN(end.getTime())) {
            return '';
        }
        return `${formatDateISO(start)} to ${formatDateISO(end)}`;
    };

    const formatDateISO = (date) => {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    };

    const isHoliday = (dateObj) => {
        const dateStr = formatDateISO(dateObj);

        return ns.holidays?.some(holiday => {
            // First check if date is within the holiday's valid period
            const holidayStartStr = formatDateISO(holiday.start);
            const holidayEndStr = formatDateISO(holiday.end);

            if (dateStr < holidayStartStr || dateStr > holidayEndStr) {
                return false; // Date is outside the holiday period
            }

            if (holiday.isCron && holiday.cron) {
                // For cron holidays, check if the date matches the cron pattern
                return matchesCronExpression(dateObj, holiday.cron, holiday.start, holiday.end);
            } else {
                // For regular range holidays, if date is within start-end, it's a holiday
                return true; // Already confirmed above that date is within range
            }
        }) || false;
    };
    const matchesCronExpression = (date, cronExpression, startDate, endDate) => {
        const cronParts = cronExpression.trim().split(/\s+/);
        if (cronParts.length >= 5) {
            const dayOfWeekCron = cronParts[4];
            if (dayOfWeekCron.includes(",")) {
                const days = dayOfWeekCron.split(",").map(d => parseInt(d));
                return days.includes(currentDayOfWeek);
            } else if (dayOfWeekCron.includes("-")) {
                // Range like "1-5" (Monday to Friday)
                const [start, end] = dayOfWeekCron.split("-").map(d => parseInt(d));
                return currentDayOfWeek >= start && currentDayOfWeek <= end;
            }
            else {
                //single day
                return parseInt(dayOfWeekCron) === currentDayOfWeek;
            }
            return false;
        }
    }
    // ================== PLAN FORM FIELD GENERATORS ==================
    // 1. In generateTitleField - FIX THE PLACEHOLDER
    const generateTitleField = (fieldId, field, readonly) => {
        const inputElement = $('<input>')
            .attr('type', 'text')
            .attr('id', `${fieldId}_planTitle`)
            .attr('name', 'PlanTitle')
            .addClass('form-control')
            .attr('placeholder', `${t('lblInsertPlan')}`)
            .val(field?.value || '');

        if (readonly) {
            inputElement.prop('readonly', true).prop('disabled', true);
        }

        if (VALIDATION_RULES.TITLE.required) {
            inputElement.attr('required', true);
        }

        return inputElement;
    };

    const generatePlanTypeField = (fieldId, field, readonly) => {
        const selectElement = $('<select>')
            .attr('id', `${fieldId}_ddlPlanType`)
            .attr('name', 'PlanTypeId')
            .addClass('form-control')
            .append($('<option>').val('').text(`${t('lblChoosePlanType')}`));

        if (readonly) {
            selectElement.prop('disabled', true);
        }

        if (VALIDATION_RULES.PLAN_TYPE.required) {
            selectElement.attr('required', true);
        }

        ns.planTypes.forEach(type => {
            const option = $('<option>')
                .val(type.id)
                .text(type.name)
                .attr('data-backendname', type.backendName);

            if (field?.value === type.id) {
                option.prop('selected', true);
            }

            selectElement.append(option);
        });

        return selectElement;
    };

    const generateSemesterField = (fieldId, field, readonly) => {
        const container = $('<div>')
            .attr('id', `${fieldId}_semesterContainer`)
            .addClass('col-md-4')
            .css('display', field?.visible !== false ? 'block' : 'none');

        const selectElement = $('<select>')
            .attr('id', `${fieldId}_ddlSemester`)
            .attr('name', 'SemesterId')
            .addClass('form-control')
            .append($('<option>').val('').text(`${t('lblChooseSemester')}`));

        if (readonly) {
            selectElement.prop('disabled', true);
        }

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
            .attr('for', `${fieldId}_ddlSemester`)
            .html(`${t('lblSemester')} <span class="text-danger">*</span>`);

        const formGroup = $('<div>').addClass('mb-4');
        formGroup.append(label, selectElement);


        if (!readonly) {
            formGroup.append($('<div>').addClass('invalid-feedback').text(`${t('lblPleaseChooseSemester')}`));
        }
        container.append(formGroup);
        return container;
    };

    const generateDateRangeField = (fieldId, field, readonly) => {
        const inputElement = $('<input>')
            .attr('type', 'text')
            .attr('id', `${fieldId}_parentDate`)
            .attr('name', 'dateRange')
            .addClass('form-control')
            .attr('placeholder', `${t('lblChooseDateRange')}`)
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

    const generateSelectCheckbox = (fieldId, school, readonly, isSelected = false) => {
        const label = $('<label>').addClass('custom-checkbox');

        const checkbox = $('<input>')
            .attr('type', 'checkbox')
            .addClass('selectRow')
            .attr('data-id', `${fieldId}_${school.id}_chk`)
            .attr('data-school-id', school.id)
            .attr('data-name', school.name);

        // Check the checkbox if the school is selected
        if (isSelected) {
            checkbox.prop('checked', true);
        }

        if (readonly) {
            checkbox.prop('disabled', true);
        }

        const checkmark = $('<span>').addClass('checkmark');
        label.append(checkbox, checkmark);
        return label;
    };

    const generateSchoolNameCell = (fieldId, school) => {
        const ratingClass = RATING_CLASSES[school.rating] || 'bg-light';
        const container = $('<div>').addClass('d-flex align-items-center justify-content-between');

        const infoDiv = $('<div>');
        const nameId = `${fieldId}_${school.id}_name`;

        // School name
        infoDiv
            .append($('<h6>').addClass('mb-1').text(school.name || '-'))
            .attr('data-name', nameId);

        // Org parent (small label)
        if (school.orgParent?.nameEn) {
            infoDiv.append(
                $('<small>')
                    .addClass('text-muted d-block')
                    .text(school.orgParent.nameEn)
            );
        }

        // School levels
        const levelBadge = $('<div>').addClass('square-bullet mt-1');
        const levelText = (school.schoolLevel && school.schoolLevel.length > 0)
            ? school.schoolLevel.map(l => l.name).join(', ')
            : '-';

        levelBadge.append($('<div>').text(levelText));
        infoDiv.append(levelBadge);

        // Rating badge
        const ratingBadge = $('<span>')
            .addClass(`badge ${ratingClass}`)
            .text(school.rating || '');

        container.append(infoDiv, ratingBadge);
        return container;
    };


    const generateVisitDateField = (fieldId, school, readonly) => {
        let visitDateValue = '';

        if (school.fromDate || school.toDate) {
            visitDateValue = formatDateRange(school.fromDate, school.toDate);
        } else if (school.visitDate) {
            visitDateValue = school.visitDate;
        } else if (school.startEvaluationDate && school.endEvaluationDate) {
            visitDateValue = formatDateRange(school.startEvaluationDate, school.endEvaluationDate);
        }

        const dateId = `${fieldId}_${school.id}_ddlVisitDate`;

        const inputElement = $('<input>')
            .attr('type', 'text')
            .addClass('form-control form-control-sm childDate')
            .attr('placeholder', `${t('lblChooseVisitDateRange')}`)
            .attr('data-school-id', school.id)
            .attr('data-field-id', dateId)
            .val(visitDateValue || '')
            .prop('readonly', true);

        if (readonly) {
            inputElement.prop('disabled', true);
        }

        return inputElement;
    };

    const generateVisitTypeField = (fieldId, school, readonly) => {
        const selectId = `${fieldId}_${school.id}_visitType`;

        const selectElement = $('<select>')
            .addClass('form-select visitTypeSelect')
            .attr('data-school-id', school.id)
            .attr('data-field-id', selectId)
            .append($('<option>').val('').text(`${t('lblChooseVisitType')}`));

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
            .attr('data-bs-target', '#SCHOOL')
            .attr('data-id', school.id)
            .html('<i class="la la-eye"></i>');

        container.append(link);
        return container;
    };

    // ================== TABLE ROW GENERATOR ==================

    const generateSchoolRow = (fieldId, school, isReadOnly, isSelected = false) => {
        const readonly = isReadOnly;
        const row = $('<tr>');

        // Checkbox cell
        const checkboxCell = $('<td>');
        checkboxCell.append(generateSelectCheckbox(fieldId, school, readonly, isSelected));
        row.append(checkboxCell);

        // School name cell
        const nameCell = $('<td>');
        nameCell.append(generateSchoolNameCell(fieldId, school));
        row.append(nameCell);

        // Visit date cell
        const visitDateCell = $('<td>');
        visitDateCell.append(generateVisitDateField(fieldId, school, readonly));
        row.append(visitDateCell);

        // Last evaluation date cell
        const lastEvalCell = $('<td>');
        lastEvalCell.text(school.lastEvaluationDate || '-');
        row.append(lastEvalCell);

        // Visit type cell
        const visitTypeCell = $('<td>');
        visitTypeCell.append(generateVisitTypeField(fieldId, school, readonly));
        row.append(visitTypeCell);

        // Academic year cell
        const academicYearCell = $('<td>');
        const academicYearText = school.academicYear || '-';
        const yearAcdemicYearText = school.yearAcdemicYear;

        academicYearCell.text(
            yearAcdemicYearText
                ? `${academicYearText} (${yearAcdemicYearText})`
                : academicYearText
        );

        row.append(academicYearCell);

        // Actions cell
        const actionsCell = $('<td>');
        actionsCell.append(generateActionsCell(school, readonly));
        row.append(actionsCell);

        return row;
    };

    // ================== RENDER FUNCTIONS ==================

    const renderPlanForm = (fieldId, planData, isReadOnly) => {
        const readonly = isReadOnly;
        const form = $('<form>').addClass('row').attr('id', `${fieldId}_planForm`);

        // Title Field
        const titleCol = $('<div>').addClass('col-md-12');
        const titleGroup = $('<div>').addClass('mb-4');
        const titleLabel = $('<label>')
            .addClass('form-label')
            .attr('for', `${fieldId}_planTitle`)
            .html(`${t('lblTitle')} <span class="text-danger">*</span>`);

        const titleField = generateTitleField(fieldId, { value: planData?.title }, readonly);
        titleGroup.append(titleLabel, titleField);

        if (!readonly) {
            titleGroup.append($('<div>').addClass('invalid-feedback').text(`${t('lblPleaseEnterPlanTitle')}`));
        }
        titleGroup.append(
            $('<div>').attr('id', `error_${fieldId}_planTitle`).addClass('error-message text-danger'));

        titleCol.append(titleGroup);
        form.append(titleCol);

        // Plan Type Field
        const planTypeCol = $('<div>').addClass('col-md-4');
        const planTypeGroup = $('<div>').addClass('mb-4');
        const planTypeLabel = $('<label>')
            .addClass('form-label')
            .attr('for', `${fieldId}_ddlPlanType`)
            .html(`${t('lblPlanType')} <span class="text-danger">*</span>`);

        const planTypeField = generatePlanTypeField(fieldId, { value: planData?.planTypeId }, readonly);
        planTypeGroup.append(planTypeLabel, planTypeField);

        if (!readonly) {
            planTypeGroup.append($('<div>').addClass('invalid-feedback').text(`${t('lblPleaseChoosePlanType')}`));
        }
        planTypeGroup.append(
            $('<div>').attr('id', `error_${fieldId}_ddlPlanType`).addClass('error-message text-danger'));
        planTypeCol.append(planTypeGroup);
        form.append(planTypeCol);

        // Semester Field
        const semesterField = generateSemesterField(fieldId, {
            value: planData?.semesterId,
            visible: planData?.showSemester
        }, readonly);
        form.append(semesterField);

        // Date Range Field
        const dateRangeCol = $('<div>').addClass('col-md-4');
        const dateRangeGroup = $('<div>').addClass('mb-4');
        const dateRangeLabel = $('<label>')
            .addClass('form-label')
            .attr('for', `${fieldId}_parentDate`)
            .html(`${t('lblTimePeriod')} <span class="text-danger">*</span>`);

        const dateRangeField = generateDateRangeField(fieldId, { value: planData?.dateRange }, readonly);
        dateRangeGroup.append(dateRangeLabel, dateRangeField);

        if (!readonly) {
            dateRangeGroup.append($('<div>').addClass('invalid-feedback').text(`${t('lblPleaseChooseTimePeriod')}`));
        }
        dateRangeGroup.append(
            $('<div>').attr('id', `error_${fieldId}_parentDate`).addClass('error-message text-danger')
        );

        dateRangeCol.append(dateRangeGroup);
        form.append(dateRangeCol);

        return form;
    };

    const renderSchoolTable = (fieldId, schools, isReadOnly, selectedSchoolsMap = null) => {
        const tbody = $('<tbody>');

        if (!schools || schools.length === 0) {
            const emptyRow = $('<tr>');
            emptyRow.append(
                $('<td>')
                    .attr('colspan', '7')
                    .addClass('text-center text-muted')
                    .text(`${t('lblNoData')}`)
            );
            tbody.append(emptyRow);
        } else {
            schools.forEach(school => {
                // Check if this school is in the selected schools map
                const isSelected = selectedSchoolsMap && selectedSchoolsMap.has(school.id);

                // If selected, merge the selection data into the school object
                if (isSelected) {
                    const selectedData = selectedSchoolsMap.get(school.id);
                    school = { ...school, ...selectedData };
                }

                const row = generateSchoolRow(fieldId, school, isReadOnly, isSelected);
                tbody.append(row);
            });
        }

        return tbody;
    };

    const renderPagination = (totalRecords) => {
        const totalPages = Math.ceil(totalRecords / ns.pageSize);
        const pagination = $('<ul>').addClass('pagination pagination-sm mb-0');

        if (totalPages <= 1) return pagination;

        if (ns.currentPage > 1) {
            const prevItem = $('<li>').addClass('page-item');
            const prevLink = $('<a>')
                .addClass('page-link')
                .attr('href', '#')
                .attr('data-page', ns.currentPage - 1)
                .text(`${t('lblPrevious')}`);
            prevItem.append(prevLink);
            pagination.append(prevItem);
        }

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

        if (ns.currentPage < totalPages) {
            const nextItem = $('<li>').addClass('page-item');
            const nextLink = $('<a>')
                .addClass('page-link')
                .attr('href', '#')
                .attr('data-page', ns.currentPage + 1)
                .text(`${t('lblNext')}`);
            nextItem.append(nextLink);
            pagination.append(nextItem);
        }

        return pagination;
    };

    // ================== FLATPICKR INITIALIZATION ==================

    const initParentPicker = (fieldId, mode, minDate, maxDate, existingValue = null) => {
        const targetSelector = `#${fieldId}_parentDate`;
        const $input = $(targetSelector);

        if (!$input.length) {
            console.error('[initParentPicker] Element not found:', targetSelector);
            return;
        }

        // Destroy previous instance for this specific fieldId
        if ($input.data('flatpickr')) {
                $input.data('flatpickr').destroy();
        }

        const config = {
            locale: "en",
            allowInput: true,
            onDayCreate: function (dObj, dStr, fp, dayElem) {
                if (isHoliday(dayElem.dateObj)) {
                    dayElem.classList.add('blocked');
                    // Make the day non-clickable
                    dayElem.classList.add('flatpickr-disabled');
                    // Remove the click event
                    dayElem.style.pointerEvents = 'none';
                }
            },
            onChange: function (selectedDates, dateStr, instance) {
                // Double-check on change (for manual input via allowInput)
                if (selectedDates.length > 0) {
                    const startDate = selectedDates[0];
                    const endDate = selectedDates[selectedDates.length - 1];

                    if (isHoliday(startDate) || (selectedDates.length > 1 && isHoliday(endDate))) {
                        instance.clear();
                        alert('لا يمكن البدء أو الانتهاء في يوم عطلة');
                    }
                }
            }
        };

        if (mode === 'disabled') {
            $input.prop('disabled', true);
            return;
        }

        $input.prop('disabled', false);

        if (mode === 'month') {
            config.plugins = [
                new monthSelectPlugin({
                    shorthand: false,
                    dateFormat: "m-y",
                    altFormat: "F Y",
                    altInput: true,
                    theme: "light"
                })
            ];

            if (existingValue && existingValue.includes(' to ')) {
                const parts = existingValue.split(' to ');
                if (parts.length === 2) {
                    const startDate = new Date(parts[0].trim());
                    config.defaultDate = startDate;
                }
            }

            config.onChange = function (selectedDates, dateStr, instance) {
                if (selectedDates.length > 0) {
                    const selectedDate = selectedDates[0];
                    const year = selectedDate.getFullYear();
                    const month = selectedDate.getMonth();

                    const firstDay = new Date(year, month, 1);
                    const lastDay = new Date(year, month + 1, 0);

                    const rangeStr = `${formatDateISO(firstDay)} to ${formatDateISO(lastDay)}`;
                    $input.val(rangeStr);

                    $input.data('startDate', formatDateISO(firstDay));
                    $input.data('endDate', formatDateISO(lastDay));

                    initChildPicker(firstDay, lastDay);
                } else {
                    destroyChildPicker();
                }
            }

            const fp = flatpickr(targetSelector, config);
            $input.data('flatpickr', fp);
        }
        else if (mode === 'custom') {
            config.mode = "range";
            config.dateFormat = "Y-m-d";

            if (minDate) config.minDate = minDate;
            if (maxDate) config.maxDate = maxDate;

            if (existingValue && existingValue.includes(' to ')) {
                const parts = existingValue.split(' to ');
                if (parts.length === 2) {
                    config.defaultDate = [parts[0].trim(), parts[1].trim()];
                }
            }

            config.onClose = function (selectedDates, dateStr, instance) {
                if (selectedDates.length === 2) {
                    const [min, max] = selectedDates;
                    initChildPicker(min, max);
                } else if (selectedDates.length === 0) {
                    destroyChildPicker();
                }
            };

            const fp = flatpickr(targetSelector, config);
            $input.data('flatpickr', fp);
        }
    };

    const initChildPicker = (minDate, maxDate) => {
        // Destroy all existing child pickers
        $('.childDate').each(function () {
            if ($(this).data('flatpickr')) {
                $(this).data('flatpickr').destroy();
            }
        });

        flatpickr(".childDate", {
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
                    cancel.textContent = `${t('lblCancel')}`;
                    cancel.onclick = (e) => {
                        e.preventDefault();
                        instance.clear();
                        instance.close();
                    };

                    const apply = document.createElement('button');
                    apply.type = 'button';
                    apply.className = 'fp-apply';
                    apply.textContent = `${t('lblConfirm')}`;
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
    };

    const destroyChildPicker = () => {
        $('.childDate').each(function () {
            if ($(this).data('flatpickr')) {
                $(this).data('flatpickr').destroy();
            }
        });
    };

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
    ns.renderPlanForm = renderPlanForm;
    ns.renderSchoolTable = renderSchoolTable;
    ns.renderPagination = renderPagination;
    ns.initParentPicker = initParentPicker;
    ns.initChildPicker = initChildPicker;
    ns.destroyChildPicker = destroyChildPicker;
    ns.formatDateISO = formatDateISO;
    ns.getMonthRange = getMonthRange;

})(planUtility);