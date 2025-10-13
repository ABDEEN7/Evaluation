using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Evaluation.SharedHelper.Helper;
using Evaluation.DAL.Models.Base;
using Evaluation.DAL.Models.Audit;


namespace Evaluation.DAL.Repositories;

public class Repository<T>(DbContext context, UserInfo userInfo) : RepositoryBase<T>(context, userInfo) where T : class, IEntity<Guid>
{
    #region Get Methods
    public IQueryable<T> GetAllActiveNonDeleted(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? pageNumber = 0, int? pageSize = 0, params Expression<Func<T, object>>?[]? includeProperties)
    {
        IQueryable<T> query = _dbSet.IgnoreQueryFilters().Where(x => x.IsDeleted==false && x.IsActive==true);
        if (includeProperties != null)
            foreach (var item in includeProperties)
                if (item != null)
                    query = query.Include(item);
        if (filter != null)
            query = query.Where(filter);
        query = orderBy != null ? orderBy(query) : query;
        if (pageNumber > 0 && pageSize > 0)
            query = query.Skip(pageNumber!.Value * pageSize!.Value).Take(pageSize!.Value);
        return query;
    }
    #endregion

    #region CRUD
    public T Insert(T entity, bool generateGUID = true)
    {
        var today = DateTime.Now;
        if (generateGUID)
            entity.Id = Guid.NewGuid();
        entity.CreateDate = today;
        entity.IsDeleted = false;
        if (userInfo.UserId.HasValue)
            entity.CreateById = userInfo.UserId.Value;
        _dbSet.Add(entity);
        return entity;
    }

    public async Task<T> InsertAsync(T entity, bool generateGUID = true)
    {
        var today = DateTime.Now;
        if (generateGUID)
            entity.Id = Guid.NewGuid();
        entity.CreateDate = today;
        entity.IsDeleted = false;
        if (userInfo.UserId.HasValue)
            entity.CreateById = userInfo.UserId.Value;
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public T Update(T entity)
    {
        entity.UpdateDate = DateTime.Now;
        if (userInfo.UserId != Guid.Empty)
            entity.UpdateById = userInfo.UserId;
        _dbSet.Update(entity);
        return entity;
    }

    public bool Delete(T entity)
    {
        entity.IsDeleted = true;
        entity.DeleteDate = DateTime.Now;
        if (userInfo.UserId != Guid.Empty)
            entity.DeleteById = userInfo.UserId;
        context.Entry(entity).State = EntityState.Modified;
        return true;
    }
    #endregion

    #region Audit
    private List<AuditLog> GenerateAuditLogs(EntityEntry entityEntry)
    {
        var logs = new List<AuditLog>();
        foreach (var property in entityEntry.OriginalValues.Properties)
        {
            var original = entityEntry.OriginalValues[property];
            var current = entityEntry.CurrentValues[property];
            if (!Equals(original, current))
                logs.Add(new AuditLog
                {
                    Id = Guid.NewGuid(),
                    TableName = typeof(T).Name,
                    RefId = entityEntry.OriginalValues["Id"]!.ToString()!,
                    ColumnName = property.Name,
                    OldValue = original?.ToString(),
                    NewValue = current?.ToString(),
                    CreateDate = DateTime.Now,
                    CreateById = userInfo.UserId ?? Guid.Empty
                });
        }
        return logs;
    }
    #endregion
}
