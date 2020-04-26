using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharp.Template.IRepositories;
using CSharp.Template.Repositories.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CSharp.Template.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 上下文对象
        /// </summary>
        private TemplateContext _context;

        public BaseRepository(TemplateContext context)
        {
            _context = context;
        }

        public Lazy<IQueryable<TEntity>> Entities => new Lazy<IQueryable<TEntity>>(_context.Set<TEntity>().AsQueryable());

        public Task Add(TEntity entity)
        {
            return Task.Run(() => { _context.Attach(entity).State = EntityState.Added; });

            //return RunTask(() => { _context.Attach(entity).State = EntityState.Added; });
        }

        public Task Add(IEnumerable<TEntity> entities)
        {
            return Task.Run(() =>
            {
                foreach (var entity in entities)
                {
                    _context.Attach(entity).State = EntityState.Added;
                }
            });
        }

        public Task Delete(TEntity entity)
        {
            return Task.Run(() => { _context.Entry(entity).State = EntityState.Deleted; });
        }


        public Task Deletes(IEnumerable<TEntity> entities)
        {
            return Task.Run(() =>
            {
                foreach (var entity in entities)
                {
                    _context.Entry(entity).State = EntityState.Deleted;
                }
            });
        }

        public Task Deletes(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.Run(() =>
            {
                var deleteList = _context.Set<TEntity>().Where(predicate).ToList();
                _context.Set<TEntity>().RemoveRange(deleteList);
            });
        }

        public Task Update(TEntity entity, List<string> fields = null)
        {
            return Task.Run(() =>
            {
                var model = _context.Entry(entity);
                if (fields?.Any() ?? false)
                {
                    foreach (var field in fields)
                    {
                        model.Property(field).IsModified = true;
                    }
                }
                else
                {
                    _context.Entry(entity).State = EntityState.Modified;
                }
            });
        }

        public Task Update(IEnumerable<TEntity> entities, List<string> fields = null)
        {
            throw new NotImplementedException();
        }

        public Task Update(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> updateExpression)
        {
            throw new NotImplementedException();
        }

        private Task RunTask(Action method)
        {
            return Task.Run(() =>
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
                try
                {
                    method.Invoke();
                }
                finally
                {
                    _context.ChangeTracker.AutoDetectChangesEnabled = false;
                }
            });
        }

        public Task<TEntity> GetByKey(object key)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            //_context.Set<TEntity>().FirstOrDefaultAsync()
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy = null, Expression<Func<TEntity, object>> 
        orderByDescending = null)
        {
            
            var query = Entities.Value;
            query=query.Where(predicate);
            if (orderBy != null)
                query = query.OrderBy(orderBy);
            if (orderByDescending != null)
                query = query.OrderByDescending(orderByDescending);

            return query.ToListAsync();
        }

        public Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, int? skip = null, int? take = null,
            Expression<Func<TEntity, object>> orderBy = null,
            Expression<Func<TEntity, object>> orderByDescending = null)
        {
            var query = Entities.Value;
            query=query.Where(predicate);
            if (orderBy != null)
                query = query.OrderBy(orderBy);
            if (orderByDescending != null)
                query = query.OrderByDescending(orderByDescending);

            if (skip.HasValue)
                query = query.Skip(skip.Value);
            if (take.HasValue)
                query = query.Take(take.Value);

            return query.ToListAsync();
        }

        public Task<List<TEntity>> GetAll()
        {
            return _context.Set<TEntity>().ToListAsync();
        }

        public Task<int> SaveChanges()
        {
            return _context.SaveChangesAsync();
        }
    }
}