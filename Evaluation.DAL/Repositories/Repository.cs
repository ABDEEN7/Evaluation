using System.Linq.Expressions;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;




namespace Evaluation.DAL.Repositories;

public class Repository<T>(DbContext context, UserInfo userInfo) : RepositoryBase<T>(context, userInfo) where T : class, IEntity
{
    #region Get Methods
    public IQueryable<T> GetAllActiveNonDeleted(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? pageNumber = 0, int? pageSize = 0, params Expression<Func<T, object>>?[]? includeProperties)
    {
        IQueryable<T> query = _dbSet.IgnoreQueryFilters().Where(x => x.IsDeleted == false && x.IsActive == true);
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

    public IQueryable<T> GetAllQueryFiltered(Expression<Func<T, bool>>? filter = null,
     Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
     int? pageNumber = 0, int? pageSize = 0, params Expression<Func<T, object>>?[]? includeProperties)
    {
        IQueryable<T> query = _dbSet;


        if (includeProperties != null)
            foreach (var item in includeProperties)
                if (item != null)
                    query = query.Include(item);

        if (filter != null)
            query = query.Where(filter);
        query = orderBy != null ? orderBy(query) : query;


        if (pageNumber != null && pageNumber > 0 && pageSize != null && pageSize > 0)
            return query.Skip(pageNumber.Value * pageSize.Value).Take(pageSize.Value);

        return query;
    }

    public IQueryable<T> GetAllNonDeleted(Expression<Func<T, bool>>? filter = null,
      Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
      int? pageNumber = 0, int? pageSize = 0, params Expression<Func<T, object>>?[]? includeProperties)
    {
        IQueryable<T> query = _dbSet.IgnoreQueryFilters().Where(x => x.IsDeleted == false);

        if (includeProperties != null)
            foreach (var item in includeProperties)
                if (item != null)
                    query = query.Include(item);

        if (filter != null)
            query = query.Where(filter);
        query = orderBy != null ? orderBy(query) : query;


        if (pageNumber != null && pageNumber > 0 && pageSize != null && pageSize > 0)
            return query.Skip(pageNumber.Value * pageSize.Value).Take(pageSize.Value);

        return query;
    }


    public async Task<T?> GetByIDNonDeleted(Guid id, params Expression<Func<T, object>>[] includeProperties)
    {
        var query = _dbSet.IgnoreQueryFilters().Where(x => x.IsDeleted == false);

        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        var entity = await query.FirstOrDefaultAsync(e => e.Id == id);
        return entity is { IsDeleted: false } ? entity : null;
    }

    public async Task<T?> GetByIDActiveNonDeleted(Guid id, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet;

        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        var entity = await query.FirstOrDefaultAsync(e => e.Id == id);
        return entity is { IsDeleted: false, IsActive: true } ? entity : null;
    }

    public async Task<T?> GetByIdIncludingDeletedAsync( Guid id, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet.IgnoreQueryFilters(); 

        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return await query.FirstOrDefaultAsync(e => e.Id == id);
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
    public async Task<IEnumerable<T>> InsertRange(IEnumerable<T> entities)
    {
        var today = DateTime.Now;
        var addedentities = entities.ToList();
        foreach (var entity in addedentities)
        {
            entity.Id = Guid.NewGuid();
            entity.CreateDate = today;
            entity.IsDeleted = false;
            if (userInfo.UserId.HasValue)
                entity.CreateById = userInfo.UserId.Value;

        }
        await _dbSet.AddRangeAsync(addedentities);
        return entities;
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
        else
			entity.CreateById =Guid.Parse( "C2536611-576B-4EB8-84F4-747F4ECE9A23");

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
    public IEnumerable<T> UpdateRange(IEnumerable<T> entities)
    {
        var today = DateTime.Now;

        foreach (var entity in entities)
        {
            if (entity is IAuditLogEntity)
            {
                var entry = context.Entry(entity);

                var auditLogs = GenerateAuditLogs(entry);

                // Add audit logs to the context
                if (auditLogs.Any()) context.Set<AuditLog>().AddRange(auditLogs);
            }

            entity.UpdateDate = today;
            entity.IsDeleted = false;
            if (userInfo.UserId != Guid.Empty) entity.UpdateById = userInfo.UserId;
        }

        _dbSet.UpdateRange(entities);
        return entities;
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

    public bool DeleteRange(IEnumerable<T> entities)
    {
        if (entities == null || !entities.Any()) return false;

        var today = DateTime.Now;
        var userId = userInfo?.UserId;

        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.DeleteDate = today;
            if (userId != Guid.Empty)
            {
                entity.DeleteById = userId;
            }
        }

        return true;
    }
    public async Task<T?> GetByIdAsync(Guid? id)
    {
        T? query = await _dbSet.Where(x => x.Id == id).FirstAsync();
        //if (query is null)
        //    throw new Exception();
        return query;
    }
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(filter);
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
