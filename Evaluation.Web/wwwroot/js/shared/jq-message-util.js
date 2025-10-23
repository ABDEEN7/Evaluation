const notificationUtil = ((customOptions = {}) => {
    // ---------------------------------------------
    // 🧩 Base Toastr Config (merge with overrides)
    // ---------------------------------------------
    const toastrDefaults = {
        closeButton: true,
        progressBar: true,
        positionClass: "toast-bottom-full-width",
        timeOut: 5000,
        extendedTimeOut: 1000,
        showDuration: 300,
        hideDuration: 1000,
        showMethod: "fadeIn",
        hideMethod: "fadeOut",
        preventDuplicates: false,
        newestOnTop: false
    };

    toastr.options = { ...toastrDefaults, ...customOptions };

    // ---------------------------------------------
    // 📣 Simple notification wrappers
    // ---------------------------------------------
    const notify = (type, msg) => {
        if (!msg) return;
        const fn = toastr[type];
        if (typeof fn === "function") fn(msg);
        else console.warn(`Unknown toastr type: ${type}`);
    };

    const success = (msg) => notify("success", msg);
    const error = (msg) => notify("error", msg);
    const warning = (msg) => notify("warning", msg);
    const message = (msg) => notify("info", msg);

    // ---------------------------------------------
    // 💬 Popup (SweetAlert)
    // ---------------------------------------------
    const popup = ({
        title = "Notice",
        body = "",
        icon = "info",
        okText = "OK",
        cancelText = "",
        showCloseButton = false,
        showCancelButton = false
    } = {}) => {
        return Swal.fire({
            title: `<strong>${title}</strong>`,
            icon,
            html: body,
            showCloseButton,
            showCancelButton,
            focusConfirm: false,
            confirmButtonText: `<i class="fa fa-check"></i> ${okText}`,
            cancelButtonText: cancelText ? `<i class="fa fa-times"></i> ${cancelText}` : undefined
        });
    };

    // ---------------------------------------------
    // ❓ Confirmation (Promise-based)
    // ---------------------------------------------
    const confirmation = ({
        title = "Are you sure?",
        body = "",
        okText = "Confirm",
        cancelText = "Cancel",
        width = "400px",
        showCancelButton = true
    } = {}) => {
        return Swal.fire({
            title,
            html: body,
            width,
            icon: "question",
            showCancelButton,
            confirmButtonText: okText,
            cancelButtonText: cancelText,
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
                confirmButton: "btn btn-primary mx-1",
                cancelButton: "btn btn-secondary"
            }
        }).then(result => result.isConfirmed);
    };

    // ---------------------------------------------
    // 🚨 Server error handler (optional)
    // ---------------------------------------------
    const serverError = (errors) => {
        if (!errors) return;
        if (Array.isArray(errors)) {
            errors.forEach(e => error(e?.message || e));
        } else if (typeof errors === "object" && errors.message) {
            error(errors.message);
        } else {
            error(JSON.stringify(errors));
        }
    };

    // ---------------------------------------------
    // 🧱 Public API
    // ---------------------------------------------
    return {
        success,
        error,
        warning,
        message,
        popup,
        confirmation,
        serverError
    };
})();
