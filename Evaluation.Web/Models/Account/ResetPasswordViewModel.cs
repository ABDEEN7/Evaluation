using System.ComponentModel.DataAnnotations;

namespace Evaluation.Web.Models.Account
{
    public class ResetPasswordViewModel
    {
        public string ResetPasswordLogId { get; set; } = null!;
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

    }
}
