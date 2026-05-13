
const InitNavbarSection = () => {

    const hasToken = sharedUtility().ExistingToken();

    if (hasToken) {

        let lang = sharedUtility().GetCookie('lang') || 'ar';
        tokenData = sharedUtility().ParseJwt(GetLocalStorageValue(LocalStorageKeys.Token));

        const fullName = lang === 'ar'
            ? tokenData[Enums.UserProfileClaim.FullNameAr]
            : tokenData[Enums.UserProfileClaim.FullNameEn];

        const firstName = getFirstName(fullName, lang === 'ar');

        $('#navbarDropdown').html(`
            <i class="fa-solid fa-circle-user me-1"></i>
            <span>${firstName}</span>
        `);

        let userType = tokenData[Enums.UserProfileClaim.UserType];

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

    $("#logoutBtn").off('click').on('click', function (e) {
        e.preventDefault();
        sharedUtility().RedirectUnauthorized();
    });
};
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


