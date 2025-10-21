using System.ComponentModel.DataAnnotations;

namespace Evaluation.Web.Models.Account
{
    public class LoginViewModel
    {
        public string? RedirectUrl { get; set; }
        public string Email { get; set; } = null!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

    }
}
