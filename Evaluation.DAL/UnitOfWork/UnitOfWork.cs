using Microsoft.EntityFrameworkCore;
using Evaluation.DAL.Context;
using Evaluation.DAL.Repositories;
using System.Data;
using Evaluation.SharedHelper.Helper;
using Evaluation.DAL.Exceptions;
using Evaluation.DAL.Entities.Generic;


namespace Evaluation.DAL.UnitOfWork
{
    public partial class UnitOfWork : IDisposable
    {
        private EvaluationDbContext _context { get; set; }
        private readonly UserInfo _userInfo;

        public UnitOfWork(EvaluationDbContext context, UserInfo userInfo)
        {
            _context = context;
            _userInfo = userInfo;
        }

        public Repository<T> GetRepository<T>() where T : class, IEntity
            => new Repository<T>(_context, _userInfo);

        public ViewRepository<T> GetViewRepository<T>() where T : class
            => new ViewRepository<T>(_context, _userInfo);

        public async Task CommitAsync()
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new DatabaseException("Error while saving data.", ex);
            }
        }

        public async Task<IEnumerable<IDictionary<string, object>>> ExecuteRawQueryAsync(string query, object parameters = null)
        {
            try
            {
                using var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    foreach (var property in parameters.GetType().GetProperties())
                    {
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = property.Name;
                        parameter.Value = property.GetValue(parameters) ?? DBNull.Value;
                        command.Parameters.Add(parameter);
                    }
                }

                await _context.Database.OpenConnectionAsync();
                using var result = await command.ExecuteReaderAsync();
                var columns = Enumerable.Range(0, result.FieldCount).Select(result.GetName).ToList();
                var rows = new List<IDictionary<string, object>>();

                while (await result.ReadAsync())
                {
                    var row = new Dictionary<string, object>();
                    foreach (var col in columns)
                        row[col] = result[col] is DBNull ? null : result[col];
                    rows.Add(row);
                }
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing raw SQL query: {ex.Message}", ex);
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

