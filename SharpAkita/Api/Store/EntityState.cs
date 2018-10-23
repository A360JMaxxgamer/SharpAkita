using System;
using System.Collections.Generic;
using System.Text;

namespace SharpAkita.Api.Store
{
    public class EntityState<TEntity>
    {
        public Dictionary<string, TEntity> Entites { get; set; }
    }
}
