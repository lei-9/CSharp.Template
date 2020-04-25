using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharp.Template.IRepositories;
using CSharp.Template.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace CSharp.Template.Repositories
{
    public class EfRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        protected readonly TemplateContext _context;

        private  bool Commited;

        public EfRepository(TemplateContext context)
        {
            Commited = false;
            _context = context;
        }


        public virtual void Commit()
        {
            this.Commited = true;
        }
        
        public virtual Task<int> Add(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Added;
            return _context.SaveChangesAsync();
        }

        public virtual Task<int> AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AttachRange(entities);
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Added;
            }

            return _context.SaveChangesAsync();
        }

        
    }
}