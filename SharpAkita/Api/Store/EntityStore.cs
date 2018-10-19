using SharpAkita.Api.Store.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SharpAkita.Api.Store
{
    /// <summary>
    /// A store to keep track of the state of an entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityStore<TEntity>
    {
        private readonly History history;
        internal readonly Dictionary<string, TEntity> Entites;

        /// <summary>
        /// Triggered when an entity changed. The payload is the id of the changed entity.
        /// </summary>
        public event EventHandler<string> EntityChanged;

        /// <summary>
        /// Triggered when an entity was added. The payload is the id of the added entity.
        /// </summary>
        public event EventHandler<string> EntityAdded;

        public EntityStore()
        {
            history = new History();
            Entites = new Dictionary<string, TEntity>();
        }

        /// <summary>
        /// Add an entity for the specific <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        public void Add(string id, TEntity entity)
        {
            var addCommand = new AddEntityCommand<TEntity>(id, entity, this);
            history.Add(addCommand);
            history.Do();
        }

        /// <summary>
        /// Updates the entity for specific <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="propertyValueDictionary">A dictionary which has the names of the properties as its
        /// keys and the values are the values for the property</param>
        public void UpdateByProperties(string id, Dictionary<string, object> propertyValueDictionary)
        {
            var updateCommand = new UpdateEntityCommand<TEntity>(id, propertyValueDictionary, this);
            history.Add(updateCommand);
            history.Do();
        }

        /// <summary>
        /// Returns the specific entity for <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetById(string id)
        {
            return Entites[id];
        }

        /// <summary>
        /// Returns all matchin entities for <paramref name="match"/> as an readonly dictionary.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public IReadOnlyDictionary<string, TEntity> Select(Func<TEntity, bool> match)
        {
            return new ReadOnlyDictionary<string, TEntity>(Entites.Where(e => match(e.Value)).ToDictionary(e => e.Key, e => e.Value));
        }

        /// <summary>
        /// Redo a reverted action.
        /// </summary>
        public void Redo()
        {
            history.Do();
        }

        /// <summary>
        /// Undo an action.
        /// </summary>
        public void Undo()
        {
            history.Undo();
        }

        /// <summary>
        /// Amount of entities.
        /// </summary>
        public int Count => Entites.Count;
         
        internal void InvokeEntityAdded(string id)
        {
            EntityAdded?.Invoke(this, id);
        }

        internal void InvokeEntityChanged(string id)
        {
            EntityChanged?.Invoke(this, id);
        }
    }
}
