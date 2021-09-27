using Deviot.Hermes.Domain;
using Deviot.Hermes.Infra.SQLite.Configuration;
using Deviot.Hermes.Infra.SQLite.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Deviot.Hermes.Infra.SQLite
{
    public class RepositorySQLite : IRepositorySQLite
    {
        private const string GENERIC_ERROR = "Houve um erro na camada de infraestrutura";

        public DbContext DbContext { get; private set; }

        public RepositorySQLite(ApplicationDbContext db)
        {
            DbContext = db;
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : Entity
        {
            try
            {
                return DbContext.Set<TEntity>();
            }
            catch (Exception exception)
            {
                throw new Exception(GENERIC_ERROR, exception);
            }
        }

        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            try
            {
                await DbContext.Set<TEntity>().AddAsync(entity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw new Exception(GENERIC_ERROR, exception);
            }
        }

        public async Task EditAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            try
            {
                DbContext.Entry(entity).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw new Exception(GENERIC_ERROR, exception);
            }
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            try
            {
                DbContext.Entry(entity).State = EntityState.Deleted;
                await DbContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw new Exception(GENERIC_ERROR, exception);
            }
        }

        public void Dispose()
        {
            if (DbContext != null)
                DbContext.Dispose();
        }
    }
}
