using Microsoft.EntityFrameworkCore;
using Prueba.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Prueba.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ZonapagosContext dbContext;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ZonapagosContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
        }

        public DbSet<TEntity> DbSet { get { return dbSet; } }


        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> expression)
        {
            return await dbSet.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByCodigo(string codigo)
        {
            return await dbSet.FindAsync(codigo);
        }

        public async Task<TEntity> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
