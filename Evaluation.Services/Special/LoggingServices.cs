using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.Exception;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Special
{
    public class LoggingServices
    {
        private static readonly object fileLock = new();
        private readonly string logDirectory;
        private readonly string logFilePath;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public LoggingServices(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

            logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            // Use date-based file name to avoid infinite growth
            logFilePath = Path.Combine(logDirectory, $"log_{DateTime.UtcNow:yyyyMMdd}.txt");
        }

        #region === BASIC CONSOLE/DEBUG LOGGING ===

        public void WriteLine(params string[] messages)
        {
            if (messages == null || messages.Length == 0)
                return;

            string separator = new string('-', 25);
            Debug.WriteLine(separator);
            foreach (var msg in messages.Where(x => !string.IsNullOrWhiteSpace(x)))
                Debug.WriteLine($"{DateTime.Now:HH:mm:ss} | {msg}");
            Debug.WriteLine(separator);
        }

        #endregion

        #region === FILE LOGGING ===

        public void WriteLineToFile(bool append = true, params string[] messages)
        {
            if (messages == null || messages.Length == 0)
                return;

            var nonEmpty = messages.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (!nonEmpty.Any())
                return;

            StringBuilder sb = new();
            string separator = new string('-', 40);

            sb.AppendLine(separator);
            sb.AppendLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}]");
            foreach (var msg in nonEmpty)
                sb.AppendLine(msg);
            sb.AppendLine(separator);

            lock (fileLock)
            {
                using var writer = new StreamWriter(logFilePath, append, Encoding.UTF8);
                writer.WriteLine(sb.ToString());
            }
        }

        #endregion

        #region === AUDIT LOGGING ===

        public async Task AddAuditLog(string tableName, string refId, string columnName, string oldValue, string newValue)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var uow = scope.ServiceProvider.CreateScopedUow();

                var log = new AuditLog
                {
                    TableName = tableName,
                    RefId = refId,
                    ColumnName = columnName,
                    OldValue = oldValue,
                    NewValue = newValue,
                    CreateById = AppSettings.CreateById,
                    CreateDate = DateTime.UtcNow
                };

                uow.GetRepository<AuditLog>().Insert(log);
                await uow.CommitAsync();
            }
            catch (Exception ex)
            {
                WriteLineToFile(true, $"[AuditLogError] {ex.Message}");
            }
        }

        #endregion

        #region === EXCEPTION LOGGING ===

        public void SaveExceptionLog(Exception ex, string? json = null, string? context = null)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var uow = scope.ServiceProvider.CreateScopedUow();

                    var log = new ExceptionLog
                    {
                        ExceptionType = ex.GetType().FullName,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        Timestamp = DateTime.UtcNow,
                        JsonParameter = json,
                        CreateById = AppSettings.CreateById,
                        Context = context
                    };

                    uow.GetRepository<ExceptionLog>().Insert(log);
                    await uow.CommitAsync();
                }
                catch (Exception innerEx)
                {
                    // Fallback to file if DB logging fails
                    WriteLineToFile(true,
                        $"[ExceptionLogFallback] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
                        $"Type: {ex.GetType().Name}",
                        $"Message: {ex.Message}",
                        $"Inner: {innerEx.Message}");
                }
            });
        }

        #endregion
    }
}