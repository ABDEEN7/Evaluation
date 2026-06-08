window.planUtility = window.planUtility || {};

(function (ns) {

    function t(key, fallback = '') {
        const text = uiControlsSetup()?.GetUiControlText(key);
        return text || key;
    }

    ns.fieldIdPrefixes = new Map();
    ns.initializePrefix = function (fieldId) {
        if (fieldId) { ns.fieldIdPrefixes.set(fieldId, fieldId); ns.fieldIdPrefix = fieldId; }
    };
    ns.getPrefixedId = function (id, fieldId) {
        const prefix = fieldId || ns.fieldIdPrefix;
        return (!prefix || !id) ? id : `${prefix}_${id}`;
    };

    const { VALIDATION_RULES, RATING_CLASSES, PLAN_TYPE_BACKEND } = window.PlanConstants || {};

    ns.visitTypes = [];
    ns.planTypes = [];
    ns.semesters = [];
    ns.holidays = [];
    ns.parentSchool = [];
    ns.schoolLevels = [];
    ns.genders = [];
    ns.departments = [];
    ns.currentPage = 1;
    ns.pageSize = 10;

    /* ==================== DATE HELPERS ==================== */
    const formatDateRange = (startDate, endDate) => {
        if (!startDate || !endDate) return '';
        const start = new Date(startDate), end = new Date(endDate);
        if (isNaN(start.getTime()) || isNaN(end.getTime())) return '';
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
            if (dateStr < formatDateISO(holiday.start) || dateStr > formatDateISO(holiday.end)) return false;
            return holiday.isCron && holiday.cron
                ? matchesCronExpression(dateObj, holiday.cron)
                : true;
        }) || false;
    };

    // ✅ FIX: was referencing undefined `currentDayOfWeek`
    const matchesCronExpression = (date, cronExpression) => {
        const parts = cronExpression.trim().split(/\s+/);
        if (parts.length < 5) return false;
        const dow = parts[4];
        const current = date.getDay();
        if (dow.includes(',')) return dow.split(',').map(Number).includes(current);
        if (dow.includes('-')) { const [s, e] = dow.split('-').map(Number); return current >= s && current <= e; }
        return parseInt(dow) === current;
    };

    /* ==================== FORM FIELD GENERATORS ==================== */
    const generateTitleField = (fieldId, field, readonly) => {
        const el = $('<input>').attr('type', 'text').attr('id', `${fieldId}_planTitle`)
            .attr('name', 'PlanTitle').addClass('form-control')
            .attr('placeholder', t('lblInsertPlan')).val(field?.value || '');
        if (readonly) el.prop('readonly', true).prop('disabled', true);
        if (VALIDATION_RULES.TITLE.required) el.attr('required', true);
        return el;
    };

    const generatePlanTypeField = (fieldId, field, readonly) => {
        const selectElement = $('<select>').attr('id', `${fieldId}_ddlPlanType`).attr('name', 'PlanTypeId')
            .addClass('form-control').append($('<option>').val('').text(t('lblChoosePlanType')));
        if (readonly) selectElement.prop('disabled', true);
        if (VALIDATION_RULES.PLAN_TYPE.required) selectElement.attr('required', true);
        ns.planTypes.forEach(type => {
            const option = $('<option>').val(type.id).text(type.name).attr('data-backendname', type.backendName);
            if (field?.value === type.id) option.prop('selected', true);
            selectElement.append(option);
        });
        return selectElement;
    };

    const generateSemesterField = (fieldId, field, readonly) => {
        const container = $('<div>').attr('id', `${fieldId}_semesterContainer`).addClass('col-md-4')
            // ✅ FIX: was `!== false` (showed by default). Now `=== true` (hidden by default)
            .css('display', field?.visible === true ? 'block' : 'none');
        const selectElement = $('<select>').attr('id', `${fieldId}_ddlSemester`).attr('name', 'SemesterId')
            .addClass('form-control').append($('<option>').val('').text(t('lblChooseSemester')));
        if (readonly) selectElement.prop('disabled', true);
        ns.semesters.forEach(s => {
            const opt = $('<option>').val(s.id).text(s.name).data('startDate', s.startDate).data('endDate', s.endDate);
            if (field?.value === s.id) opt.prop('selected', true);
            selectElement.append(opt);
        });
        const label = $('<label>').addClass('form-label').attr('for', `${fieldId}_ddlSemester`)
            .html(`${t('lblSemester')} <span class="text-danger">*</span>`);
        const group = $('<div>').addClass('mb-4').append(label, selectElement);
        if (!readonly) group.append($('<div>').addClass('invalid-feedback').text(t('lblPleaseChooseSemester')));
        container.append(group);
        return container;
    };

    const generateDateRangeField = (fieldId, field, readonly) => {
        const el = $('<input>').attr('type', 'text').attr('id', `${fieldId}_parentDate`)
            .attr('name', 'dateRange').addClass('form-control')
            .attr('placeholder', t('lblChooseDateRange')).val(field?.value || '');
        if (readonly) el.prop('readonly', true).prop('disabled', true);
        else el.attr('required', true);
        return $('<div>').addClass('input-group datetime').append(
            el,
            $('<span>').addClass('input-group-text').html('<i class="la la-calendar"></i>')
        );
    };

    /* ==================== TABLE CELL GENERATORS ==================== */
    const generateSelectCheckbox = (fieldId, school, readonly, isSelected = false) => {
        const label = $('<label>').addClass('custom-checkbox');
        const checkbox = $('<input>').attr('type', 'checkbox').addClass('selectRow')
            .attr('data-id', `${fieldId}_${school.id}_chk`)
            .attr('data-school-id', school.id)
            .attr('data-name', school.name);
        if (isSelected) checkbox.prop('checked', true);
        if (readonly) checkbox.prop('disabled', true);
        label.append(checkbox, $('<span>').addClass('checkmark'));
        return label;
    };

    const generateSchoolNameCell = (fieldId, school) => {
        const ratingClass = RATING_CLASSES[school.rating] || 'bg-light';
        const container = $('<div>').addClass('d-flex align-items-center justify-content-between');
        const infoDiv = $('<div>');

        infoDiv.append($('<h6>').addClass('mb-1').text(school.name || '-')).attr('data-name', `${fieldId}_${school.id}_name`);

        if (school.orgParent?.nameEn) {
            infoDiv.append($('<small>').addClass('text-muted d-block').text(school.orgParent.nameEn));
        }

        const levelText = (school.schoolLevel?.length > 0)
            ? school.schoolLevel.map(l => l.name).join(', ')
            : '-';
        infoDiv.append($('<div>').addClass('square-bullet mt-1').append($('<div>').text(levelText)));

        container.append(infoDiv, $('<span>').addClass(`badge ${ratingClass}`).text(school.rating || ''));
        return container;
    };

    const generateVisitDateField = (fieldId, school, readonly) => {
        let val = '';
        if (school.fromDate || school.toDate) val = formatDateRange(school.fromDate, school.toDate);
        else if (school.visitDate) val = school.visitDate;
        else if (school.startEvaluationDate && school.endEvaluationDate) val = formatDateRange(school.startEvaluationDate, school.endEvaluationDate);

        const el = $('<input>').attr('type', 'text')
            .addClass('form-control form-control-sm childDate')
            .attr('placeholder', t('lblChooseVisitDateRange'))
            .attr('data-school-id', school.id)
            .attr('data-field-id', `${fieldId}_${school.id}_ddlVisitDate`)
            .val(val || '').prop('readonly', true);
        if (readonly) el.prop('disabled', true);
        return el;
    };

    const generateVisitTypeField = (fieldId, school, readonly) => {
        const sel = $('<select>').addClass('form-select visitTypeSelect')
            .attr('data-school-id', school.id)
            .attr('data-field-id', `${fieldId}_${school.id}_visitType`)
            .append($('<option>').val('').text(t('lblChooseVisitType')));
        ns.visitTypes.forEach(type => {
            const opt = $('<option>').val(type.id).text(type.name);
            if (school.visitType === type.name || school.visitTypeId === type.id) opt.prop('selected', true);
            sel.append(opt);
        });
        if (readonly) sel.prop('disabled', true);
        return sel;
    };

    const generateActionsCell = (school) => {
        return $('<p>').addClass('m-0').append(
            $('<a>').attr('href', '#').addClass('text-dark').attr('type', 'button')
                .attr('data-bs-toggle', 'modal').attr('data-bs-target', '#schoolDetailsModal')
                .attr('data-id', school.id).html('<i class="la la-eye"></i>')
        );
    };

    /* ==================== ROW GENERATOR ==================== */
    const generateSchoolRow = (fieldId, school, isReadOnly, isSelected = false) => {
        const row = $('<tr>');
        row.append($('<td>').append(generateSelectCheckbox(fieldId, school, isReadOnly, isSelected)));
        row.append($('<td>').append(generateSchoolNameCell(fieldId, school)));
        row.append($('<td>').append(generateVisitDateField(fieldId, school, isReadOnly)));
        row.append($('<td>').text(school.lastEvaluationDate || '-'));
        row.append($('<td>').append(generateVisitTypeField(fieldId, school, isReadOnly)));
        row.append($('<td>').text(school.academicYear || '-'));
        row.append($('<td>').append(generateActionsCell(school)));
        return row;
    };

    /* ==================== RENDER FUNCTIONS ==================== */
    const renderPlanForm = (fieldId, planData, isReadOnly) => {
        const form = $('<form>').addClass('row').attr('id', `${fieldId}_planForm`);

        // Title
        const titleGroup = $('<div>').addClass('mb-4');
        titleGroup.append(
            $('<label>').addClass('form-label').attr('for', `${fieldId}_planTitle`)
                .html(`${t('lblTitle')} <span class="text-danger">*</span>`),
            generateTitleField(fieldId, { value: planData?.title }, isReadOnly)
        );
        if (!isReadOnly) titleGroup.append($('<div>').addClass('invalid-feedback').text(t('lblPleaseEnterPlanTitle')));
        form.append($('<div>').addClass('col-md-12').append(titleGroup));

        // Plan Type
        const ptGroup = $('<div>').addClass('mb-4');
        ptGroup.append(
            $('<label>').addClass('form-label').attr('for', `${fieldId}_ddlPlanType`)
                .html(`${t('lblPlanType')} <span class="text-danger">*</span>`),
            generatePlanTypeField(fieldId, { value: planData?.planTypeId }, isReadOnly)
        );
        if (!isReadOnly) ptGroup.append($('<div>').addClass('invalid-feedback').text(t('lblPleaseChoosePlanType')));
        form.append($('<div>').addClass('col-md-4').append(ptGroup));

        // Semester
        form.append(generateSemesterField(fieldId, { value: planData?.semesterId, visible: planData?.showSemester }, isReadOnly));

        // Date Range
        const drGroup = $('<div>').addClass('mb-4');
        drGroup.append(
            $('<label>').addClass('form-label').attr('for', `${fieldId}_parentDate`)
                .html(`${t('lblTimePeriod')} <span class="text-danger">*</span>`),
            generateDateRangeField(fieldId, { value: planData?.dateRange }, isReadOnly)
        );
        if (!isReadOnly) drGroup.append($('<div>').addClass('invalid-feedback').text(t('lblPleaseChooseTimePeriod')));
        form.append($('<div>').addClass('col-md-4').append(drGroup));

        return form;
    };

    const renderSchoolTable = (fieldId, schools, isReadOnly, selectedSchoolsMap = null) => {
        const tbody = $('<tbody>');
        if (!schools || schools.length === 0) {
            tbody.append($('<tr>').append($('<td>').attr('colspan', '7').addClass('text-center text-muted').text(t('lblNoData'))));
            return tbody;
        }
        schools.forEach(originalSchool => {
            const isSelected = selectedSchoolsMap && selectedSchoolsMap.has(originalSchool.id);
            let school = originalSchool;
            if (isSelected) {
                const mapData = selectedSchoolsMap.get(originalSchool.id);
                school = {
                    ...originalSchool,
                    visitDate: mapData.visitDate || originalSchool.visitDate || '',
                    visitTypeId: mapData.visitTypeId || originalSchool.visitTypeId || ''
                };
            }
            tbody.append(generateSchoolRow(fieldId, school, isReadOnly, isSelected));
        });
        return tbody;
    };

    const renderPagination = (totalRecords) => {
        const totalPages = Math.ceil(totalRecords / ns.pageSize);
        const pagination = $('<ul>').addClass('pagination pagination-sm mb-0');
        if (totalPages <= 1) return pagination;

        if (ns.currentPage > 1) {
            pagination.append($('<li>').addClass('page-item').append(
                $('<a>').addClass('page-link').attr('href', '#').attr('data-page', ns.currentPage - 1).text(t('lblPrevious'))
            ));
        }
        for (let i = 1; i <= totalPages; i++) {
            const li = $('<li>').addClass('page-item' + (i === ns.currentPage ? ' active' : ''));
            li.append($('<a>').addClass('page-link').attr('href', '#').attr('data-page', i).text(i));
            pagination.append(li);
        }
        if (ns.currentPage < totalPages) {
            pagination.append($('<li>').addClass('page-item').append(
                $('<a>').addClass('page-link').attr('href', '#').attr('data-page', ns.currentPage + 1).text(t('lblNext'))
            ));
        }
        return pagination;
    };

    /* ==================== FLATPICKR ==================== */
    const initParentPicker = (fieldId, mode, minDate, maxDate, existingValue = null) => {
        const selector = `#${fieldId}_parentDate`;
        const $input = $(selector);
        if (!$input.length) { console.error('[initParentPicker] not found:', selector); return; }
        if ($input.data('flatpickr')) $input.data('flatpickr').destroy();

        if (mode === 'disabled') { $input.prop('disabled', true); return; }
        $input.prop('disabled', false);

        const base = {
            locale: 'en', allowInput: true,
            onDayCreate: (dObj, dStr, fp, dayElem) => {
                if (isHoliday(dayElem.dateObj)) {
                    dayElem.classList.add('blocked', 'flatpickr-disabled');
                    dayElem.style.pointerEvents = 'none';
                }
            },
            onChange: (selectedDates, dateStr, instance) => {
                if (!selectedDates.length) return;
                const s = selectedDates[0], e = selectedDates[selectedDates.length - 1];
                if (isHoliday(s) || (selectedDates.length > 1 && isHoliday(e))) {
                    instance.clear(); alert('لا يمكن البدء أو الانتهاء في يوم عطلة');
                }
            }
        };

        if (mode === 'month') {
            base.plugins = [new monthSelectPlugin({ shorthand: false, dateFormat: 'm-y', altFormat: 'F Y', altInput: true, theme: 'light' })];
            if (existingValue?.includes(' to ')) base.defaultDate = new Date(existingValue.split(' to ')[0].trim());
            base.onChange = (selectedDates, dateStr, instance) => {
                if (!selectedDates.length) { destroyChildPicker(); return; }
                const d = selectedDates[0];
                const first = new Date(d.getFullYear(), d.getMonth(), 1);
                const last = new Date(d.getFullYear(), d.getMonth() + 1, 0);
                $input.val(`${formatDateISO(first)} to ${formatDateISO(last)}`)
                    .data('startDate', formatDateISO(first)).data('endDate', formatDateISO(last));
                initChildPicker(first, last);
            };
            $input.data('flatpickr', flatpickr(selector, base));
        } else if (mode === 'custom') {
            base.mode = 'range'; base.dateFormat = 'Y-m-d';
            if (minDate) base.minDate = minDate;
            if (maxDate) base.maxDate = maxDate;
            if (existingValue?.includes(' to ')) {
                const [s, e] = existingValue.split(' to ');
                base.defaultDate = [s.trim(), e.trim()];
            }
            base.onClose = (selectedDates) => {
                if (selectedDates.length === 2) initChildPicker(selectedDates[0], selectedDates[1]);
                else if (!selectedDates.length) destroyChildPicker();
            };
            $input.data('flatpickr', flatpickr(selector, base));
        }
    };

    const initChildPicker = (minDate, maxDate) => {
        $('.childDate').each(function () { if ($(this).data('flatpickr')) $(this).data('flatpickr').destroy(); });

        flatpickr('.childDate', {
            mode: 'range', locale: 'en', dateFormat: 'Y-m-d', allowInput: true,
            minDate, maxDate,
            onReady: function (selectedDates, dateStr, instance) {
                const mc = instance.calendarContainer.querySelector('.flatpickr-months');
                const prev = instance.calendarContainer.querySelector('.flatpickr-prev-month');
                const next = instance.calendarContainer.querySelector('.flatpickr-next-month');
                const stack = document.createElement('div');
                stack.className = 'fp-arrow-stack';
                stack.appendChild(prev); stack.appendChild(next);
                mc.insertBefore(stack, mc.firstChild);
                mc.appendChild(mc.querySelector('.flatpickr-current-month'));

                if (!instance.calendarContainer.querySelector('.fp-btns')) {
                    const btns = document.createElement('div');
                    btns.className = 'fp-btns';
                    const cancel = document.createElement('button');
                    cancel.type = 'button'; cancel.className = 'fp-cancel'; cancel.textContent = t('lblCancel');
                    cancel.onclick = e => { e.preventDefault(); instance.clear(); instance.close(); };
                    const apply = document.createElement('button');
                    apply.type = 'button'; apply.className = 'fp-apply'; apply.textContent = t('lblConfirm');
                    apply.onclick = e => { e.preventDefault(); instance.close(); };
                    btns.appendChild(cancel); btns.appendChild(apply);
                    instance.calendarContainer.appendChild(btns);
                }
            }
        });
    };

    const destroyChildPicker = () => {
        $('.childDate').each(function () { if ($(this).data('flatpickr')) $(this).data('flatpickr').destroy(); });
    };

    const getMonthRange = (date) => ({
        start: new Date(date.getFullYear(), date.getMonth(), 1),
        end: new Date(date.getFullYear(), date.getMonth() + 1, 0)
    });

    /* ==================== EXPORTS ==================== */
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