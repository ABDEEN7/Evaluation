const apiUrl = 'api/EvalForm/Submit';
const useAjaxPromis = true;

function textOf(selectorOrElment) {
    if (!selectorOrElment) return '';
    const el = (typeof selectorOrElement === 'string') ? document.querySelector(selectorOrElement) : selectorOrElement;
    return el ? (el.textContent || el.value || '').trim() : '';
}