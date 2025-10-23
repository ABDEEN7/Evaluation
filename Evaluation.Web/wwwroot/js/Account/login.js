$(document).ready(function () {

    // When user clicks the button
    $('#LoginBtnId').click(function (event) {
        event.preventDefault();

        // Optional: show spinner or disable button
        $(this).prop('disabled', true).text('Redirecting...');

        // Redirect directly to your configured SSO endpoint
        // For Azure AD OIDC: /signin-oidc
        // For Okta: /signin-okta
        // For Google: /signin-google
        // (depends on what you configured in Program.cs)
        window.location.replace('/signin-oidc');
    });

});
