namespace SharpAkita.Api.Store.Commands
{
    /// <summary>
    ///  Adds an entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class AddEntityCommand<TEntity> : IStoreCommand
    {
        private readonly string id;
        private readonly TEntity entity;
        private readonly EntityStore<TEntity> store;

        public AddEntityCommand(string id, TEntity entity, EntityStore<TEntity> store)
        {
            this.id = id;
            this.entity = entity;
            this.store = store;
        }

        public void Do()
        {
            store.Entites.Add(id, entity);
            store.InvokeEntityAdded(id);
        }

        public void Undo()
        {
            store.Entites.Remove(id);
        }
    }
}
