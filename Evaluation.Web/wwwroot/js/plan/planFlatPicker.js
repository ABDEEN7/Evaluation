let childPicker = null;

// Initialize parent picker first
const parentPicker = flatpickr("#parentDate", {
    mode: "range",
    locale: "ar",
    dateFormat: "Y-m-d",
    allowInput: true,
    onClose: function (selectedDates, dateStr, instance) {
        if (selectedDates.length === 2) {
            const [minDate, maxDate] = selectedDates;

            // Destroy existing child picker if it exists
            if (childPicker) {
                childPicker.destroy();
            }

            // Initialize child picker with the date limits
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
                childPicker = childPicker[0]; // Get first instance
            }
        } else if (selectedDates.length === 0) {
            // Parent cleared, destroy child picker
            if (childPicker) {
                childPicker.destroy();
                childPicker = null;
            }
        }
    }
});