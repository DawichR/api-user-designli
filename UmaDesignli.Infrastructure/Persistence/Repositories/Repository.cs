using UmaDesignli.Application.Interfaces.Repositories;
using System.Collections.Concurrent;

namespace UmaDesignli.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Generic repository implementation (Memory storage)
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ConcurrentDictionary<int, TEntity> _entities;
        protected int _nextId;
        protected readonly object _lock = new();

        public Repository()
        {
            _entities = new ConcurrentDictionary<int, TEntity>();
            _nextId = 1;
        }
         
        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<TEntity>>(_entities.Values.ToList());
        }

        public virtual Task<TEntity?> GetByIdAsync(int id)
        {
            _entities.TryGetValue(id, out var entity);
            return Task.FromResult(entity);
        }

        public virtual Task<TEntity> AddAsync(TEntity entity)
        {
            lock (_lock)
            {
                SetEntityId(entity, _nextId);
                _entities.TryAdd(_nextId, entity);
                _nextId++;
            }
            return Task.FromResult(entity);
        }

        public virtual Task<TEntity?> UpdateAsync(int id, TEntity entity)
        {
            if (!_entities.ContainsKey(id))
                return Task.FromResult<TEntity?>(null);

            SetEntityId(entity, id);
            _entities[id] = entity;
            return Task.FromResult(entity)!;
        }

        public virtual Task<bool> DeleteAsync(int id)
        {
            return Task.FromResult(_entities.TryRemove(id, out _));
        }

        public virtual Task<int> CountAsync()
        {
            return Task.FromResult(_entities.Count);
        }

        public virtual Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            lock (_lock)
            {
                foreach (var entity in entities)
                {
                    SetEntityId(entity, _nextId);
                    _entities.TryAdd(_nextId, entity);
                    _nextId++;
                }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Sets the Id property of the entity using reflection
        /// </summary>
        protected virtual void SetEntityId(TEntity entity, int id)
        {
            var idProperty = typeof(TEntity).GetProperty("Id");
            if (idProperty != null && idProperty.CanWrite)
            {
                idProperty.SetValue(entity, id);
            }
        }

        /// <summary>
        /// Gets the Id property value of the entity using reflection
        /// </summary>
        protected virtual int GetEntityId(TEntity entity)
        {
            var idProperty = typeof(TEntity).GetProperty("Id");
            if (idProperty != null)
            {
                var value = idProperty.GetValue(entity);
                return value != null ? (int)value : 0;
            }
            return 0;
        }
    }

}
