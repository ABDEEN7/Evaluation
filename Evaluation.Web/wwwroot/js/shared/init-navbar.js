
const InitNavbarSection = () => {

    if (sharedUtility().ExistingToken()) {

        let lang = sharedUtility().GetCookie('lang');
        $(".dropdownsection1").show();
        $(".dropdownsection2").hide();
        tokenData = sharedUtility().ParseJwt(GetLocalStorageValue(LocalStorageKeys.Token));
        $('#navbarDropdown').html(getFirstName(lang == 'ar' ? tokenData[Enums.UserProfileClaim.FullNameAr] : tokenData[Enums.UserProfileClaim.FullNameEn], lang == 'ar'));

        let userType = sharedUtility().ParseJwt(GetLocalStorageValue(LocalStorageKeys.Token))[Enums.UserProfileClaim.UserType];

        if (userType !== Enums.UserType.Student) {
            $('.student-parts-only').remove();
        } 
        if (userType !== Enums.UserType.Ministry) {
            $('.ministry-parts-only').remove();
        }

        $("#profileDropdown").show();
        $(".login-or-register").hide();


    } else {
       
        $("#profileDropdown").hide();
        $(".login-or-register").show();
    }

    $("#logoutBtn").click(function () {
      
        sharedUtility().RedirectUnauthorized();
    });
}
const InitializeTooltip = () => {
    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
}
const getFirstName = (fullName, isArabic) => {
    if (!fullName) return '';

    const parts = fullName.trim().split(/\s+/); 

    if (isArabic) {
        const compoundStarts = ['عبد', 'نور', 'فخر', 'شمس', 'سيف'];

        if (parts.length >= 3 && compoundStarts.includes(parts[0])) {
            return parts[0] + ' ' + parts[1]; 
        }
    }

    return parts[0]; 
}


