using System;
using System.Collections.Generic;

namespace SharpAkita.Api.Store
{
    public class EntityStore<TEntityState, TEntity> : Store<TEntityState> where TEntityState : EntityState<TEntity>
    {
        private string activeId;

        public void Add(string identifier, TEntity entity)
        {
            var copiedState = currentStoreState.Copy();
            copiedState.Entites.Add(identifier, entity);
            SetState(copiedState);
        }

        public void CreateOrReplace(string identifier, TEntity entity)
        {
            if (currentStoreState.Entites.ContainsKey(identifier))
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
            copiedState.Entites.Remove(identifier);
            SetState(copiedState);
        }

        public void SetActive(string identifier)
        {
            activeId = identifier;
        }

        public void UpdateActive(Action<TEntity> update)
        {
            var copiedEntity = currentStoreState.Entites[activeId].Copy();
            update(copiedEntity);
            UpdateExistingEntity(activeId, copiedEntity);
        }

        public IDictionary<string, TEntity> GetEntities()
        {
            return currentStoreState.Entites;
        }

        private void UpdateExistingEntity(string identifier, TEntity entity)
        {
            var copiedState = currentStoreState.Copy();
            copiedState.Entites[identifier] = entity;
            SetState(copiedState);
        }
    }
}
