const isAr = document.documentElement.lang.toLowerCase().startsWith("ar");
const calendarLocale = isAr ? "ar" : "default";

$('.datepicker-single').each(function() {
    flatpickr(this, {
        dateFormat: "Y-m-d",
        locale: calendarLocale,
        allowInput: true
    });
});

$(".datepicker-any").each(function() {
    flatpickr(this, {
        dateFormat: "Y-m-d",
        locale: calendarLocale,
        allowInput: true
    });
});

$(".year-picker").each(function() {
    flatpickr(this, {
        dateFormat: "Y",
        altInput: true,
        altFormat: "Y",
        locale: calendarLocale,
        allowInput: true
    });
});

$("#parentDate").each(function() {
    flatpickr(this, {
        mode: "range",
        dateFormat: "Y-m-d",
        locale: calendarLocale,
        allowInput: true
    });
});