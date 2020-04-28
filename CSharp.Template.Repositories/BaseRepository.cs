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
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
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

        public Task Insert(TEntity entity,bool isSave =true)
        {
            _context.Attach(entity).State = EntityState.Added;

            return isSave ? this.SaveChanges() : Task.FromResult(0);
        }

        public Task Insert(IEnumerable<TEntity> entities,bool isSave =true)
        {
            foreach (var entity in entities)
            {
                _context.Attach(entity).State = EntityState.Added;
            }
            
            return isSave ? this.SaveChanges() : Task.FromResult(0);
        }

        public Task Delete(TEntity entity,bool isSave =true)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            
            return isSave ? this.SaveChanges() : Task.FromResult(0);
        }


        public Task Deletes(IEnumerable<TEntity> entities,bool isSave =true)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
            
            return isSave ? this.SaveChanges() : Task.FromResult(0);
        }

        public Task Deletes(Expression<Func<TEntity, bool>> predicate,bool isSave =true)
        {
            var deleteList = _context.Set<TEntity>().Where(predicate).ToList();
            _context.Set<TEntity>().RemoveRange(deleteList);
            
            return isSave ? this.SaveChanges() : Task.FromResult(0);
        }

        public Task Update(TEntity entity, List<string> fields = null,bool isSave =true)
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
            
            return isSave ? this.SaveChanges() : Task.FromResult(0);
        }

        public Task Update(IEnumerable<TEntity> entities, List<string> fields = null,bool isSave =true)
        {
            if (fields?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    var model=_context.Entry(entity);
                    foreach (var field in fields)
                    {
                        model.Property(field).IsModified = true;
                    }
                }
            }
            else
            {
                _context.UpdateRange(entities);
            }

            return isSave ? this.SaveChanges() : Task.FromResult(0);
        }

        public Task Update(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> updateExpression,bool isSave =true)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByKey(object key)
        {
            //return _context.Set<TEntity>().FindAsync(key);
            throw new NotImplementedException();
        }

        public Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
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