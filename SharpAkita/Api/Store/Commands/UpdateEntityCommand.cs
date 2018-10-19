using System.Collections.Generic;
using System.Linq;

namespace SharpAkita.Api.Store.Commands
{
    /// <summary>
    /// Updates an Entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class UpdateEntityCommand<TEntity> : IStoreCommand
    {
        private readonly string id;
        private readonly EntityStore<TEntity> store;

        private readonly Dictionary<string, object> newValueDictionary;
        private readonly Dictionary<string, object> oldValueDictionary;

        public UpdateEntityCommand(string id, Dictionary<string, object> propertyValueDictionary, EntityStore<TEntity> store)
        {
            this.id = id;
            this.store = store;
            newValueDictionary = new Dictionary<string, object>(propertyValueDictionary);
            oldValueDictionary = new Dictionary<string, object>();
            var entityToUpdate = store.Entites.First(e => e.Key == id).Value;
            var propertyInfos = entityToUpdate.GetType().GetProperties().Where(p => propertyValueDictionary.ContainsKey(p.Name));
            foreach (var prop in propertyInfos)
            {
                oldValueDictionary.Add(prop.Name, prop.GetValue(entityToUpdate));
            }
        }

        public void Do()
        {
            var entityToUpdate = store.Entites.First(e => e.Key == id).Value;
            var propertyInfos = entityToUpdate.GetType().GetProperties().Where(p => newValueDictionary.ContainsKey(p.Name));
            foreach (var prop in propertyInfos)
            {
                prop.SetValue(entityToUpdate, newValueDictionary[prop.Name]);
            }
            store.InvokeEntityChanged(id);
        }

        public void Undo()
        {
            var entityToUpdate = store.Entites.First(e => e.Key == id).Value;
            var propertyInfos = entityToUpdate.GetType().GetProperties().Where(p => oldValueDictionary.ContainsKey(p.Name));
            foreach (var prop in propertyInfos)
            {
                prop.SetValue(entityToUpdate, oldValueDictionary[prop.Name]);
            }
            store.InvokeEntityChanged(id);
        }
    }
}
