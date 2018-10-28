using System;
using System.Collections.Generic;

namespace SharpAkita.Api.Store
{
    public class EntityStore<TEntityState, TEntity> : Store<TEntityState> where TEntityState : EntityState<TEntity>
    {
        private string activeId;

        public EntityStore(Func<TEntityState> initialStateCreator) : base(initialStateCreator)
        {
        }

        public void Add(string identifier, TEntity entity)
        {
            var copiedState = currentStoreState.Copy();
            copiedState.Entities.Add(identifier, entity);
            SetState(copiedState);
        }

        public void CreateOrReplace(string identifier, TEntity entity)
        {
            if (currentStoreState.Entities.ContainsKey(identifier))
            {
                UpdateExistingEntity(identifier, entity);
            }
            else
            {
                Add(identifier, entity);
            }
        }

        public void Remove(string identifier)
        {
            var copiedState = currentStoreState.Copy();
            copiedState.Entities.Remove(identifier);
            SetState(copiedState);
        }

        public void SetActive(string identifier)
        {
            activeId = identifier;
        }

        public void UpdateActive(Func<TEntity, TEntity> update)
        {
            var copiedEntity = currentStoreState.Entities[activeId].Copy();
            var updated = update(copiedEntity);
            UpdateExistingEntity(activeId, updated);
        }

        public IDictionary<string, TEntity> GetEntities()
        {
            return currentStoreState.Entities;
        }

        private void UpdateExistingEntity(string identifier, TEntity entity)
        {
            var copiedState = currentStoreState.Copy();
            copiedState.Entities[identifier] = entity;
            SetState(copiedState);
        }
    }
}
