
using Evaluation.SharedHelper.Models.Admin;
using Newtonsoft.Json;

namespace Evaluation.Admin.Extensions
{
    public static class AppExtensions
    {

        public static TSource StringToObject<TSource>(this string source)
        {
            var result = JsonConvert.DeserializeObject<TSource>(source);
            if (result == null)
                throw new InvalidOperationException($"Failed to deserialize JSON into {typeof(TSource).Name}");

            return result;
        }
        public static bool IsPermissionAvailabe(this List<string> Permisions, string Key)
        {
            var isAvl = Permisions.FirstOrDefault(c => c.ToLower() == Key.ToLower());
            if (null == isAvl) return false;
            else return true;
        }

        public static string GetUiControlText(this IList<UiControlDTO> ControlsList, string BackendName, string lang)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(BackendName))
            {
                var control = ControlsList.FirstOrDefault(c => c.BackEndName == BackendName);
                if (control == null)
                {
                    result = $"Missing Text For : [{BackendName}]";
                }
                else
                {
                    if (lang == "ar")
                    {
                        if (!string.IsNullOrEmpty(control.ArValue))
                        {
                            result = control.ArValue;
                        }
                        else
                        {
                            result = $"Missing Arabic Text For : [{BackendName}]";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(control.EnValue))
                        {
                            result = control.EnValue;
                        }
                        else
                        {
                            result = $"Missing English Text For : [{BackendName}]";
                        }
                    }
                }
            }
            else
            {
                result = $"Missing Text For Unknown : [BackendName]";

            }
            return result;
        }


        public static string GetUiControlText(this IList<UiControlDTO> ControlsList, string BackendName)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(BackendName))
            {
                var control = ControlsList.FirstOrDefault(c => c.BackEndName == BackendName);
                if (control == null)
                {
                    result = $"Missing Label For : [{BackendName}]";
                }
                else
                {
                    if (!string.IsNullOrEmpty(control.txtValue))
                    {
                        result = control.txtValue;
                    }
                    else
                    {
                        result = $"Missing Text For : [{BackendName}]";
                    }
                }
            }
            else
            {
                result = $"Missing Text For Unknown : [BackendName]";

            }
            return result;
        }


    }
}
