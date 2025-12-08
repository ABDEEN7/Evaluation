// Initialize single date pickers (class: datepicker-single)
$('.datepicker-single').each(function () {
    flatpickr(this, {
        dateFormat: "Y-m-d",
        locale: "ar",
        allowInput: true,
    });
})
// Initialize date pickers with any date (class: datepicker-any)
$(".datepicker-any").each(function () {
    flatpickr(this, {
        dateFormat: "Y-m-d",
        locale: "ar",
        allowInput: true
    });
});

$(".year-picker").each(function () {
    flatpickr(this, {
        dateFormat: "Y",      // returned value
        altInput: true,
        altFormat: "Y",       // visible value
    });
});