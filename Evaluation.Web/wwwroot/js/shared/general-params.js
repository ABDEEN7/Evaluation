let uiControls = {
    applabel: [],
}

//-----------------------------------------------------------------------
let pageActiveRequests = 0;
const systemSetting = {
    values: [],
}
let dropdowns = []
let ModuelName = null;

const LocalStorageKeys = {
    Token: 'token',
    Type: 'type',
    ExpirationTime: 'expirationTime',
    TempMessage: 'tempMessage',
    ErrorTempMessage: 'errortempMessage',
    TempSessionExpirationTime: 'tempSessionExpirationTime',
    RedirectUrl: 'redirectUrl',

    /*UserProfileDetailsInfo: 'userProfileDetailsInfo',*/
};

let userProfileDetailsInfo = null;

const GetLocalStorageValue = (key) => {
    let result = localStorage.getItem(key);
    return result;
}

const SetLocalStorageValue = (key, value) => {
    localStorage.setItem(key, value);
}

const RemoveLocalStorageValue = (key) => {
    localStorage.removeItem(key);
}


let departmentName = ModuelName;

const ConstantUrls = {
    LoginURL: '/Account/Login',
    AccessDenied: '/AccessDenied',
    LogoutUrl: '/Account/Logout',
    LoginMinistry: '/Account/LoginMinistry',
    RefreshTokenURL: '/Account/RefreshToken',
    GetMSAuthorizationURL: '/Account/GetMSAuthorizationURL',
    GetUserAuthType: '/Account/GetUserAuthType/{username}',
    Home: '/Home/Index',
    UserDetails: '/User/UserDetails',
    MobileResetPasswordURL: '/Account/MobileResetPassword',

};


let Enums = {
    UserProfileClaim: {
        UserId: 'UserId',
        Email: 'Email',
        CreationDate: 'CreationDate',
        FullNameEn: 'FullNameEn',
        FullNameAr: 'FullNameAr',
        LastLogin: 'LastLogin',
        PreferredLang: 'PreferredLang',
        UserType: 'UserType',
        Mobile: 'Mobile',
    },
    UserType: {
        Ministry: 'Ministry',
        Student: 'Student',
    },

    AllocationFrequency: {
        Yearly: 'Yearly',
        Once: 'Once',
        Monthly: 'Monthly',
        Randomly: 'Randomly',
    }

};
//-----------------------------------



