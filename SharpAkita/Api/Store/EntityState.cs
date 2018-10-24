using System.Collections.Generic;

namespace SharpAkita.Api.Store
{
    public class EntityState<TEntity>
    {
        public EntityState()
        {
            Entities = new Dictionary<string, TEntity>();
        }           

        public Dictionary<string, TEntity> Entities { get; set; }
    }
}
