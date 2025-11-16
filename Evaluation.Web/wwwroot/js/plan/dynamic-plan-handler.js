let childPicker = null;
let parentPickerInstance = null;
let holidays = [
    { date: '2025-01-01', name: 'New Year\'s Day' },
    { date: '2025-12-25', name: 'Christmas Day' }
];

const $ddlPlanType = $('#ddlPlanType');
const $parentDate = $('#parentDate');
const $semesterContainer = $('#semesterContainer');
const $ddlSemester = $('#ddlSemester');

// Get vacation days
function getVacationDays() {
    jqClient.Get('/AcademicYears/GetVcationDate').done((result) => {
        const data = (result && result.result) ? result.result : [];
        holidays = data.map(item => ({ date: item.date }));
    });
}

// Format date to ISO
function formatDateISO(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}

// Check if date is holiday
function isHoliday(date) {
    const dateStr = formatDateISO(date);
    return holidays.some(h => h.date === dateStr);
}

// Get plan types
const getPlanTypes = () => {
    jqClient().Get('/Plan/GetPlanType').done((result) => {
        const data = (result && result.result) ? result.result : [];
        $ddlPlanType.select2({
            placeholder: "اختر نوع الخطة",
            allowClear: true,
            width: '100%',
            dropdownCssClass: "manageselect2zindex",
            data: data.map(item => ({
                id: item.id,
                text: item.name,
                backendName: item.backendName
            }))
        });
        $ddlPlanType.val(null).trigger('change');
    }).fail((jqXHR, textStatus, err) => {
        console.error('GetPlanType failed', textStatus, err);
    });
};

// Get semesters
const getSemesters = () => {
    return jqClient().Get('/Plan/GetSemesters').then((result) => {
        const data = (result && result.result) ? result.result : [];
        return data;
    }).catch((jqXHR, textStatus, err) => {
        console.error('GetSemesters failed', textStatus, err);
        return [];
    });
};

// Initialize semester dropdown
const initSemesterDropdown = async () => {
    const semesters = await getSemesters();

    $ddlSemester.select2({
        placeholder: "اختر الفصل الدراسي",
        allowClear: true,
        width: '100%',
        dropdownCssClass: "manageselect2zindex",
        data: semesters.map(item => ({
            id: item.id,
            text: item.name,
            startDate: item.startDate,
            endDate: item.endDate
        }))
    });

    $ddlSemester.val(null).trigger('change');
};

// Destroy parent picker
function destroyParentPicker() {
    if (parentPickerInstance) {
        parentPickerInstance.destroy();
        parentPickerInstance = null;
    }
}

// Destroy child picker
function destroyChildPicker() {
    if (childPicker) {
        childPicker.destroy();
        childPicker = null;
    }
}

// Initialize parent picker for custom mode
function initCustomParentPicker() {
    destroyParentPicker();

    $parentDate.prop('disabled', false);

    parentPickerInstance = flatpickr("#parentDate", {
        mode: "range",
        locale: "ar",
        dateFormat: "Y-m-d",
        onDayCreate: function (dObj, dStr, fp, dayElem) {
            if (isHoliday(dayElem.dateObj)) {
                dayElem.classList.add('blocked');
            }
        },
        allowInput: true,
        onClose: function (selectedDates, dateStr, instance) {
            if (selectedDates.length === 2) {
                const [minDate, maxDate] = selectedDates;
                initChildPicker(minDate, maxDate);
            } else if (selectedDates.length === 0) {
                destroyChildPicker();
            }
        }
    });
}

// Initialize parent picker for Year mode
function initYearParentPicker() {
    destroyParentPicker();

    const currentYear = new Date().getFullYear();
    const startDate = `${currentYear}-01-01`;
    const endDate = `${currentYear}-12-31`;

    $parentDate.val(`${startDate} to ${endDate}`);
    $parentDate.prop('disabled', true);

    // Auto-initialize child picker with year range
    const minDate = new Date(currentYear, 0, 1);
    const maxDate = new Date(currentYear, 11, 31);
    initChildPicker(minDate, maxDate);
}

// Initialize parent picker for Month mode
function initMonthParentPicker() {
    destroyParentPicker();

    const now = new Date();
    const currentYear = now.getFullYear();
    const currentMonth = now.getMonth();

    const startDate = new Date(currentYear, currentMonth, 1);
    const endDate = new Date(currentYear, currentMonth + 1, 0);

    const startStr = formatDateISO(startDate);
    const endStr = formatDateISO(endDate);

    $parentDate.val(`${startStr} to ${endStr}`);
    $parentDate.prop('disabled', true);

    // Auto-initialize child picker with month range
    initChildPicker(startDate, endDate);
}

// Initialize parent picker for Semester mode
function initSemesterParentPicker(startDate, endDate) {
    destroyParentPicker();

    const start = new Date(startDate);
    const end = new Date(endDate);

    const startStr = formatDateISO(start);
    const endStr = formatDateISO(end);

    $parentDate.val(`${startStr} to ${endStr}`);
    $parentDate.prop('disabled', true);

    // Auto-initialize child picker with semester range
    initChildPicker(start, end);
}

// Initialize child picker
function initChildPicker(minDate, maxDate) {
    destroyChildPicker();

    childPicker = flatpickr(".childDate", {
        mode: "range",
        locale: "ar",
        dateFormat: "Y-m-d",
        allowInput: true,
        minDate: minDate,
        maxDate: maxDate,
        onReady: function (selectedDates, dateStr, instance) {
            const monthsContainer = instance.calendarContainer.querySelector('.flatpickr-months');
            const prev = instance.calendarContainer.querySelector('.flatpickr-prev-month');
            const next = instance.calendarContainer.querySelector('.flatpickr-next-month');

            // Create arrow stack container
            const arrowStack = document.createElement('div');
            arrowStack.className = 'fp-arrow-stack';
            arrowStack.appendChild(prev);
            arrowStack.appendChild(next);

            // Insert arrow stack at the beginning
            monthsContainer.insertBefore(arrowStack, monthsContainer.firstChild);

            // Move month/year to the end
            const monthYear = monthsContainer.querySelector('.flatpickr-current-month');
            monthsContainer.appendChild(monthYear);

            // Add custom buttons only once
            if (!instance.calendarContainer.querySelector('.fp-btns')) {
                const btns = document.createElement('div');
                btns.className = 'fp-btns';

                // Cancel button
                const cancel = document.createElement('button');
                cancel.type = 'button';
                cancel.className = 'fp-cancel';
                cancel.textContent = 'إلغاء';
                cancel.addEventListener('click', e => {
                    e.preventDefault();
                    instance.clear();
                    instance.close();
                });

                // Apply button
                const apply = document.createElement('button');
                apply.type = 'button';
                apply.className = 'fp-apply';
                apply.textContent = 'تأكيد';
                apply.addEventListener('click', e => {
                    e.preventDefault();
                    instance.close();
                });

                btns.appendChild(cancel);
                btns.appendChild(apply);
                instance.calendarContainer.appendChild(btns);
            }
        }
    });

    // Handle array of instances if using class selector
    if (Array.isArray(childPicker)) {
        childPicker = childPicker[0];
    }
}

// Handle plan type change
$ddlPlanType.on('change', async function () {
    const selectedOption = $(this).select2('data')[0];

    if (!selectedOption) {
        // Clear everything
        $semesterContainer.hide();
        destroyParentPicker();
        destroyChildPicker();
        $parentDate.val('').prop('disabled', false);
        return;
    }

    const backendName = selectedOption.backendName;

    // Hide semester container by default
    $semesterContainer.hide();
    destroyChildPicker();

    switch (backendName) {
        case 'Year':
            initYearParentPicker();
            break;

        case 'Month':
            initMonthParentPicker();
            break;

        case 'Semester':
            // Show semester dropdown
            $semesterContainer.show();
            await initSemesterDropdown();
            $parentDate.val('').prop('disabled', true);
            break;

        default: // Custom
            initCustomParentPicker();
            break;
    }
});

// Handle semester selection
$ddlSemester.on('change', function () {
    const selectedOption = $(this).select2('data')[0];

    if (!selectedOption) {
        destroyChildPicker();
        $parentDate.val('');
        return;
    }

    const startDate = selectedOption.startDate;
    const endDate = selectedOption.endDate;

    if (startDate && endDate) {
        initSemesterParentPicker(startDate, endDate);
    }
});

// Initialize on page load
$(document).ready(function () {
    //getVacationDays();
    getPlanTypes();
});