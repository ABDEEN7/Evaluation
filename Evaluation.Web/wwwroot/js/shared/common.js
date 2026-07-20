function localization(key, fallback = '') {
    const text = uiControlsSetup()?.GetUiControlText(key);
    return text || fallback || key;
}