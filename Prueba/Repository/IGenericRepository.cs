using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Prueba.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(int id);
        Task<TEntity> GetByCodigo(string codigo);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> expression);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);

    }
}
