namespace Evaluation.SharedHelper.Consts;

public static class LanguageStatic
{
    public static string SelectLang(string lang, string nameAr, string nameEn)
    {
        bool isArabic = string.Equals(lang, "ar", StringComparison.OrdinalIgnoreCase);

        return isArabic ? nameAr : nameEn;
    }
}
