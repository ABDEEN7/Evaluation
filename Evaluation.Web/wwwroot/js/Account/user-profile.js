const $fileInput = $("#fileInput"),
    $secondMobileField = $("#UserProfileSecondMobile"),
    $previewImage = $("#previewImage"),
    $langToggler = $("#change-language-toggler"),
    input = document.querySelector("#UserProfileSecondMobile"),
    errorMsg = document.querySelector("#error-msg");

let userId = null, userType = null, secondMobile = null, iti = null, allowedExtensions = [], allowPictureSize = 5;
let intervals_UserProfile = [];


async function LoadUserDetails() {
    $("#updateVerifyMobileNumberBtn").hide();
    if (!sharedUtility().ExistingToken()) return;

    const options = {
        success: function ({ data }) {
            if (!data) return sharedUtility().RedirectUnauthorized();

            userId = data.id;
            userType = data.type;
            secondMobile = data.secondMobile;

            updateProfileFields(data);
           
        }
    };
   
    await jqClient(options).Get("/UserInfo/UserDetails");
}

function initIntlTelInput() {
    iti = intlTelInput(input, {
        separateDialCode: true,
        excludeCountries: ["qa", "il"],
        initialCountry: "us",
        placeholderNumberType: "MOBILE",
    });
}

function updateProfileFields(data) {
    $("#EmailId").val(data.email);
    $("#FullNameArId").val(data.fullNameAr);
    $("#FullNameEnId").val(data.fullNameEn);
    $("#LastLoginDateID").val(data.lastLoginDate);
    $("#UserProfileMobile").val(data.mobile);
    $("#UserProfileNationality").val(data.nationalityCode);
    $("#QIDId").val(data.qid);
    $("#UserProfileOccupation").val(data.jobDescription);
    $("#PrefferedLang").val(data.prefferedLang);

    userProfileDetailsInfo = {
        name: data.fullNameEn,
        UserType: userType,
        QIDId:data.qid,
    };
   
}


const errorMap = ["Invalid number", "Invalid country code", "Too short", "Too long", "Invalid number"];
const showError = msg => { input.classList.add("error"); errorMsg.innerHTML = msg; errorMsg.classList.remove("hide"); };
const hideError = () => { input.classList.remove("error"); errorMsg.innerHTML = ''; errorMsg.classList.add("hide"); };
const getNumberPhone = () => iti.getNumber().replace("+", "00");



// Manage Language
const pathPart = window.location.pathname.split("/");
let lang = window.currentLang;
if (!["ar", "en"].includes(lang)) window.currentLang = "en";


