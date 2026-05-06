const $fileInput = $("#fileInput"),
    $secondMobileField = $("#UserProfileSecondMobile"),
    $previewImage = $("#previewImage"),
    $langToggler = $("#change-language-toggler"),
    input = document.querySelector("#UserProfileSecondMobile"),
    errorMsg = document.querySelector("#error-msg");

let userId = null, userType = null, secondMobile = null, iti = null, allowedExtensions = [], allowPictureSize = 5;
let intervals_UserProfile = [];
$fileInput.on('change', handleFileUpload);

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
            handleUserPermissions();
            
        }
    };
   
    await jqClient(options).Get("/User/UserDetails");
}

function initIntlTelInput() {
    iti = intlTelInput(input, {
        separateDialCode: true,
        excludeCountries: ["qa", "il"],
        initialCountry: "us",
        placeholderNumberType: "MOBILE",
    });
}

function handleFileUpload(event) {
    const file = event.target.files[0];
    if (!file) return alert("Please select a file.");

    const fileName = $(this).val().toLowerCase();
    const isValidExtension = allowedExtensions.split(',').some(ext => fileName.endsWith(ext));    

    if (!isValidExtension) {
        notificationUtil.error("Invalid file type! Please upload a valid image file.");
        return;
    }
    const maxFileSizeInBytes = allowPictureSize * 1024 * 1024; // allowPictureSize is 5 by default (5MB)
    const isSizeValid = file.size <= maxFileSizeInBytes; // Check if file size is within the limit
    if (!isSizeValid) {
        notificationUtil.error(`File size exceeds the limit of ${allowPictureSize}MB.`);
        return;
    }

    const reader = new FileReader();
    reader.onload = e => $previewImage[0].src = e.target.result;
    reader.readAsDataURL(file);

    const formData = new FormData();
    formData.append("profilePhoto", file);

    //jqClient().PostFormData("/User/UpdateProfilePhoto", formData)
    //    .done(response => response ? notificationUtil.success("Profile updated!") : notificationUtil.error(response.error))
    //    .fail(() => notificationUtil.error("Update profile photo failed!"));
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
    if (userType.toLowerCase() === "student") {
        if ($previewImage.length) {
            $previewImage[0].src = data.profilePhoto || '/assets/img/registeration.png';
            $previewImage.on('click', () => $fileInput[0]?.click());
        } else {
            console.warn("Preview image element not found");
        }
        if(typeof intlTelInput != "undefined") {
            initIntlTelInput();
            iti.setNumber(data.secondMobile.replace("00", "+"));
            if (!data.isVerifiedSecondMobile) $("#updateVerifyMobileNumberBtn").show();
        }
    }
}

function handleUserPermissions() {
    if (userType.toLowerCase() !== "student") {
        $("#fileInput, #second-mobile-div, #updateProfileBtn, #updateVerifyMobileNumberBtn").remove();
    }else{
        $("#OccupationDivId").remove();
    }
}

const errorMap = ["Invalid number", "Invalid country code", "Too short", "Too long", "Invalid number"];
const showError = msg => { input.classList.add("error"); errorMsg.innerHTML = msg; errorMsg.classList.remove("hide"); };
const hideError = () => { input.classList.remove("error"); errorMsg.innerHTML = ''; errorMsg.classList.add("hide"); };
const getNumberPhone = () => iti.getNumber().replace("+", "00");



// Manage Language
const pathParts = window.location.pathname.split("/");
let currentLang = pathParts[1];
if (!["ar", "en"].includes(currentLang)) currentLang = "en";

$langToggler.prop("checked", currentLang === "ar").change(() => {
    const newLang = $langToggler.is(":checked") ? "ar" : "en";
    sharedUtility().SetCookie("lang", newLang, 30);
    pathParts[1] = newLang;
    window.location.href = window.location.origin + pathParts.join("/");
});
