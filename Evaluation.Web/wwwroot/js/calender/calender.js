/*****************************
 * State
 *****************************/
const loadedMonths = new Set();   // faster lookups than array
let calendarResults = [];

/*****************************
 * Date Utilities
 *****************************/

const today = new Date().toISOString().split('T')[0];

const formatYYYYMM = (date) =>
    `${date.getFullYear()}${String(date.getMonth() + 1).padStart(2, '0')}`;

function getSurroundingMonths(dateStr) {
    const baseDate = new Date(dateStr);

    const prev = new Date(baseDate);
    prev.setMonth(prev.getMonth() - 1);

    const next = new Date(baseDate);
    next.setMonth(next.getMonth() + 1);

    return [
        formatYYYYMM(prev),
        formatYYYYMM(baseDate),
        formatYYYYMM(next)
    ];
}

function getMidVisibleDate(fetchInfo) {
    const date = new Date(fetchInfo.start);
    date.setDate(date.getDate() + 15);
    return date.toISOString().split('T')[0];
}

/*****************************
 * Helpers
 *****************************/
function buildUrl(baseUrl, months) {
    const params = new URLSearchParams();
    months.forEach(m => params.append("monthes", m));
    return `${baseUrl}?${params.toString()}`;
}

function getMonthsToLoad(months) {
    return months.filter(month => {
        if (loadedMonths.has(month)) return false;
        loadedMonths.add(month);
        return true;
    });
}

/*****************************
 * API
 *****************************/
async function fetchEvaluationRequests(months) {
    if (!months.length) return [];

    const url = buildUrl(
        "/EvaluationRequest/GetEvaluationRequests",
        months
    );

    return await jqClient().Get(url);
}

async function GetEvaluationRequests(fetchInfo) {
    try {
        const midDate = getMidVisibleDate(fetchInfo);
        const surroundingMonths = getSurroundingMonths(midDate);
        const monthsToLoad = getMonthsToLoad(surroundingMonths);

        if (monthsToLoad.length) {
            const result = await fetchEvaluationRequests(monthsToLoad);
            calendarResults = [...calendarResults, ...result];
        }

        return calendarResults;
    } catch (error) {
        console.error("Failed to load evaluation requests:", error);
        return [];
    }
}

/*****************************
 * Calendar
 *****************************/
document.addEventListener("DOMContentLoaded", () => {
    const calendarEl = document.getElementById("calendar");

    const calendar = new FullCalendar.Calendar(calendarEl, {
        headerToolbar: {
            left: "prev,next",
            center: "title",
            right: "dayGridMonth,timeGridWeek,timeGridDay"
        },
        initialDate: today,
        navLinks: true,
        businessHours: true,
        editable: true,
        selectable: true,

        events: async (fetchInfo, successCallback, failureCallback) => {
            try {
                const events = await GetEvaluationRequests(fetchInfo);
                successCallback(events);
            } catch (err) {
                failureCallback(err);
            }
        },

        eventDrop: onEventDrop,

        loading(isLoading) {
            if (isLoading) {
                console.log("Loading calendar events...");
            }
        }
    });

    calendar.render();
});

/*****************************
 * Event Handlers
 *****************************/
function onEventDrop(info) {
    Swal.fire({
        title: "هل أنت متأكد؟",
        text: "هل تريد تعديل تاريخ الطلب؟",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "نعم، تعديل",
        cancelButtonText: "إلغاء"
    }).then(result => {
        if (result.isConfirmed) {
            let eventResult = calendarResults.find(item => item.id === info.event.id);
            if (validateEventDates(info, eventResult)) {
                updateEventDates(info, eventResult);
            }
        } else {
            info.revert();
        }
    });
}

function validateEventDates(info, eventResult) {
    if (eventResult.source == 'ServiceRequest') {

        let parent = calendarResults.find(item => item.id === eventResult.parentId);
        //date.setDate(date.getDate() + 1);

        let parentStartDate = new Date(parent.start);
        let parentEndDate = new Date(parent.end);
        parentEndDate = parentEndDate.setDate(parentEndDate.getDate() - 1);

        if (!(parentStartDate <= info.event.start && parentEndDate >= info.event.end))
        {
            Swal.fire({
                icon: "error",
                title: "خطأ",
                text: "يجب ان تكون الخدمة ضمن فترة الطلب"
            });

            info.revert();
            return false;
        }
    }
    return true;
}
function updateEventDates(info, eventResult) {
    const payload = {
        id: info.event.id,
        Title: info.event.title,
        Start: info.event.start,
        End: info.event.end
    };

    let url = '';

    if (eventResult.source == 'ServiceRequest')
    {
        url = "/EvaluationRequest/UpdateEvaluationServiceRequest";
    }
    else
    {
        url = "/EvaluationRequest/UpdateEvaluationRequest";
    }

    jqClient().Post(url, payload)
        .done((res) => {
            Swal.fire({
                icon: "success",
                title: "تم التعديل",
                text: "تم تعديل تاريخ الطلب"
            });
        }).fail((err) => {
            Swal.fire({
                icon: "error",
                title: "خطأ",
                text: "حدث خطأ أثناء تعديل الطلب"
            });
            info.revert();
        });
}
