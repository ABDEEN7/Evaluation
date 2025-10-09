using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Evaluation.DAL.Exceptions
{
    public class DatabaseException : Exception
    {
        public string UserFriendlyMessage { get; }
        public string InnerExceptionMessage { get; }
        public string InnerExceptionStackTrace { get; }

        public DatabaseException(string message, Exception ex)
            : base(message, ex)
        {
            UserFriendlyMessage = message;
            InnerExceptionMessage = GetAllInnerExceptionMessagesAsJson(ex);
            InnerExceptionStackTrace = GetAllInnerExceptionStackTracesAsJson(ex);
        }

        private string GetAllInnerExceptionMessagesAsJson(Exception ex)
        {
            var messages = new Dictionary<string, string>();
            int level = 1;
            while (ex != null)
            {
                messages[$"Level {level}"] = ex.Message;
                ex = ex.InnerException;
                level++;
            }
            return JsonSerializer.Serialize(messages, new JsonSerializerOptions { WriteIndented = true });
        }

        private string GetAllInnerExceptionStackTracesAsJson(Exception ex)
        {
            var traces = new Dictionary<string, string>();
            int level = 1;
            while (ex != null)
            {
                traces[$"Level {level}"] = ex.StackTrace ?? "[No stack trace]";
                ex = ex.InnerException;
                level++;
            }
            return JsonSerializer.Serialize(traces, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
