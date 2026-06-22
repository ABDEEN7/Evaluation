using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.Services.Special;

public interface IEmailNotificationService
{
    Task<bool> SendByTemplateAsync(
       string templateSetting,
       List<string> emails);
}
