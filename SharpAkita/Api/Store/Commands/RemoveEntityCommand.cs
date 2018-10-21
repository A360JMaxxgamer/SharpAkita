namespace SharpAkita.Api.Store.Commands
{
    /// <summary>
    /// Removes an entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RemoveEntityCommand<TEntity> : IStoreCommand
    {
        private readonly string id;
        private readonly TEntity entity;
        private readonly EntityStore<TEntity> store;

        public RemoveEntityCommand(string id, EntityStore<TEntity> store)
        {
            this.id = id;
            this.store = store;
            entity = store.GetById(id);
        }

        public void Do()
        {
            store.Entites.Remove(id);
        }

        public void Undo()
        {
            store.Entites.Add(id, entity);
        }
    }
}
