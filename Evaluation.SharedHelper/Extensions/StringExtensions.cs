using Evaluation.SharedHelper.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Evaluation.SharedHelper.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidJson(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            input = input.Trim();

            // A valid JSON object starts with '{' and ends with '}'
            // A valid JSON array starts with '[' and ends with ']'
            if ((input.StartsWith("{") && input.EndsWith("}")) ||
                (input.StartsWith("[") && input.EndsWith("]")))
            {
                try
                {
                    // Try parsing the input into a JToken
                    JToken.Parse(input);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
                catch (Exception) // Other exceptions, unlikely
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static Dictionary<string, string>? JsonToDictionary(this string json)
        {
            var result = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(json) && json.IsValidJson())
            {
                result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.Trim());
            }
            return result;


        }



        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            // Simple and common regex for email validation
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email.Trim(), pattern);
        }

        public static bool IsQatariMobile(this string mobile, string qatariMobilePattern)
        {
            if (string.IsNullOrEmpty(mobile))
                return false;

            return Regex.IsMatch(mobile.Trim(), qatariMobilePattern);
        }

        public static bool IsQatariID(this string QID)
        {
            if (string.IsNullOrEmpty(QID))
                return false;


            return QID.Length == 11;//291 422 010 61
        }


        public static TSource? StringToObject<TSource>(string source)
        {
            return JsonConvert.DeserializeObject<TSource>(source);
        }
        public static string GetStringfromObj(object _obj)
        {
            try
            {
                string jsonObj = JsonConvert.SerializeObject(_obj);
                return jsonObj;
            }
            catch (Exception)
            {
                return "";
            }

        }
  

        public static async Task ParallelForEachAsync<T>(
            this IEnumerable<T> source,
            Func<T, Task> action,
            int maxDegreeOfParallelism = 15,
            CancellationToken cancellationToken = default)
        {
            using var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);
            var tasks = source.Select(async item =>
            {
                await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    await action(item).ConfigureAwait(false);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            // Throws with all inner exceptions aggregated.
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }


        public static void ValidateRequired(this string? input, string? errorMessage = null)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrEmpty(input))
                throw new BusinessException(string.IsNullOrEmpty(errorMessage) ? "Value cannot be empty." : errorMessage);
        }
        public static void ValidateRequired(this Guid input, string? errorMessage = null)
        {
            if (input == Guid.Empty)
                throw new BusinessException(string.IsNullOrEmpty(errorMessage) ? "Value cannot be empty." : errorMessage);
        }
        public static void ValidateRequired(this Guid? input, string? errorMessage = null)
        {
            if (input is null)
                input!.Value.ValidateRequired(errorMessage);
        }

        public static void ValidateMinLength(this string? input, int minLength, string? errorMessage = null)
        {
            if (minLength < 0)
            {
                throw new BusinessException("Minimum length cannot be negative.");
            }

            if (!string.IsNullOrEmpty(input) && input.Trim().Length < minLength)
            {
                throw new BusinessException(errorMessage ?? $"{input} must be at least {minLength} characters long.");
            }
        }

        public static void ValidateMaxLength(this string? input, int maxLength, string? errorMessage = null)
        {
            if (maxLength < 0)
            {
                throw new BusinessException("Maximum length cannot be negative.");
            }

            if (!string.IsNullOrEmpty(input) && input.Trim().Length > maxLength)
            {
                throw new BusinessException(errorMessage ?? $"{input} must not exceed {maxLength} characters.");
            }
        }

        public static void ValidateRegex(this string? input, string pattern, string? errorMessage = null)
        {
            if (string.IsNullOrWhiteSpace(pattern) || string.IsNullOrEmpty(pattern))
            {
                throw new BusinessException($"Regex pattern cannot be null or empty. {nameof(pattern)}");
            }

            if (!string.IsNullOrEmpty(input) && !Regex.IsMatch(input.Trim(), pattern))
            {
                throw new BusinessException(errorMessage ?? $"{input} does not match the required format.");
            }
        }


        public static void ValidatePositive<T>(this T number, string? errorMessage = null) where T : struct, IComparable<T>
        {
            if (number.CompareTo(default(T)) <= 0)
            {
                throw new BusinessException(errorMessage ?? "Value must be positive.");
            }
        }

        public static void ValidateNegative<T>(this T number, string? errorMessage = null) where T : struct, IComparable<T>
        {
            if (number.CompareTo(default(T)) >= 0)
            {
                throw new BusinessException(errorMessage ?? "Value must be negative.");
            }
        }


        public static DateTime ValidateDate(this string? input, string format, string? errorMessage = null)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrEmpty(input))
            {
                throw new BusinessException(errorMessage ?? "Date value cannot be empty.");
            }

            if (!DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                throw new BusinessException(errorMessage ?? $"Invalid date format. Expected format: {format}");
            }

            if (parsedDate == DateTime.MinValue)
            {
                throw new BusinessException(errorMessage ?? "Invalid date value.");
            }

            return parsedDate;
        }

        public static DateOnly ValidateDateOnly(this string? input, string format, string? errorMessage = null)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrEmpty(input))
            {
                throw new BusinessException(errorMessage ?? "Date value cannot be empty.");
            }

            // Define supported formats: default + fallback
            var formats = new[] { format, "yyyy-MM-dd" };

            if (!DateOnly.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                throw new BusinessException(errorMessage ?? $"Invalid date format. Expected format: {format} or yyyy-MM-dd");
            }
            
            if (parsedDate == DateOnly.MinValue)
            {
                throw new BusinessException(errorMessage ?? "Invalid date value.");
            }

            return parsedDate;
        }


        public static DateTime? ToDateTime(this string input, string format, string? errorMessage = null, bool throwOnError = false)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrEmpty(input))
            {
                string message = errorMessage ?? "Date string cannot be empty.";
                if (throwOnError) throw new BusinessException(message);
                return null;
            }

            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }

            string defaultErrorMessage = $"Invalid date format. Expected format: {format}";
            if (throwOnError)
            {
                throw new BusinessException(errorMessage ?? defaultErrorMessage);
            }

            return null;
        }

    }
}
