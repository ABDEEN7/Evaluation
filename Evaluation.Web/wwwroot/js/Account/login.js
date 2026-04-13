let departmentRoutePath = sharedUtility().extractDepartmentName();

$(document).ready(function () {
    loadSelectedDepartment();
});

$('#Email').keypress(function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        checkUserAndRedirect();
    }
});

$('#ContinueBtnId').click(function (event) {
    event.preventDefault();
    checkUserAndRedirect();
});



$('#BackToEmailId').click(function (event) {
    event.preventDefault();
   
});

/*------------------------------------------------------
   STEP 1: Check user type by email
------------------------------------------------------*/
const checkUserAndRedirect = () => {
    const username = $('#Email').val()?.trim();
    if (!username) {
        notificationUtil.error(uiControlsSetup().GetUiControlText('lblEnterTheEmailOrQID'));
        return;
    }

    const options = {
        success: function (result) {
            if (!result || !result.ssoRedirectUrl) {
                notificationUtil.error(uiControlsSetup().GetUiControlText('lblUserNotFound'));
                return;
            }

            if (result.ssoRedirectUrl) {
                window.location.replace(result.ssoRedirectUrl);
            } else {
                notificationUtil.error('SSO URL غير صالح.');
            }
        },
        error: function (xhr) {
            if (xhr.status === 200 && xhr.responseText?.startsWith("http")) {
                window.location.replace(xhr.responseText);
                return;
            }

            notificationUtil.error(
                xhr.responseText || uiControlsSetup().GetUiControlText('lblUnexpectedError')
            );
        }
    };

    jqClient(options).Post(`/Account/CheckUserAuth?username=${username}`);
};

function loadSelectedDepartment() {
    jqClient().Get(`/Website/GetDepartment?routingPath=${departmentRoutePath}`)
        .done((result) => {

            const data = result.result.result;
            if (data != null) {
                if (data.depImage !== null && data.depImage !== undefined && data.depImage !== "") {
                    // valid string
                    $("#loginImg").attr("src", data.depImage);
                }
            }
        })
        .fail((jqXHR, textStatus, err) => {
            console.error('GetAll department failed', textStatus, err);
        });
}


