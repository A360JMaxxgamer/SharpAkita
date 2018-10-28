using SharpAkita.Api.Store;
using System;

namespace SharpAkita.Api.Query
{
    public class BaseQuery<TState>
    {
        private readonly Store<TState> store;

        public BaseQuery(Store<TState> store)
        {
            this.store = store;
        }

        /// <summary>
        /// Returns an <see cref="IObservable{T}"/> for the <paramref name="map"/>
        /// function.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public IObservable<TResult> Select<TResult>(Func<TState, TResult> map)
        {
            return store.Select(map);
        }
    }
}
