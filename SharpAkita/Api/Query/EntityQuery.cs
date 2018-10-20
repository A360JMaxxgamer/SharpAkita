using SharpAkita.Api.Store;
using SharpAkita.Data;
using System;

namespace SharpAkita.Api.Query
{
    public class EntityQuery<TEntity>
    {
        private readonly EntityStore<TEntity> store;
        private readonly ObservableDictionary<string, TEntity> entities;

        public EntityQuery(EntityStore<TEntity> store)
        {
            this.store = store;
            this.store.EntityAdded += StoreEntityAdded;
            this.store.EntityChanged += StoreEntityChanged;
            entities = new ObservableDictionary<string, TEntity>();
            foreach (var entity in store.Select())
            {
                entities.Add(entity.Key, entity.Value);
            }
        }

        private void StoreEntityChanged(object sender, string e)
        {
            entities[e] = store.GetById(e);
        }

        private void StoreEntityAdded(object sender, string e)
        {
            entities.Add(e, store.GetById(e));
        }
    }
}
