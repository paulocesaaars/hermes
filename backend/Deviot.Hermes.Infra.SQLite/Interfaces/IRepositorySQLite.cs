using Deviot.Hermes.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Deviot.Hermes.Infra.SQLite.Interfaces
{
    public interface IRepositorySQLite : IDisposable
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity : Entity;

        Task AddAsync<TEntity>(TEntity entity) where TEntity : Entity;

        Task EditAsync<TEntity>(TEntity entity) where TEntity : Entity;

        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity;
    }
}
