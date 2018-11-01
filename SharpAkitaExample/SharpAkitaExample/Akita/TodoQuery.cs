using SharpAkita.Api.Query;
using SharpAkita.Api.Store;
using SharpAkitaExample.Model;

namespace SharpAkitaExample.Akita
{
    public class TodoQuery: BaseQuery<EntityState<Todo>>
    {
        public TodoQuery(EntityStore<EntityState<Todo>, Todo> store) : base(store)
        {
        }
    }
}
