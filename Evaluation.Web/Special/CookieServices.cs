namespace Evaluation.Web.Special
{
    public class CookieServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCookie(string key, string value, int? expireMinutes = 60)
        {
            var options = new CookieOptions
            {
                Expires = expireMinutes.HasValue || expireMinutes != null ? DateTime.Now.AddMinutes(expireMinutes.Value) : DateTime.Now.AddMonths(1),
                IsEssential = true, // Make the cookie essential
                //SameSite = SameSiteMode.None,
                //Secure = true, // Requires HTTPS
            };
            if (!string.IsNullOrEmpty(key))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
            }
        }

        public string GetCookie(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }
    }
}
