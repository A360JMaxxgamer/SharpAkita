using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SharpAkita.Api.Store
{
    /// <summary>
    /// A store which holds a <typeparamref name="TState"/>.
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class Store<TState>
    {
        private readonly BehaviorSubject<TState> store;

        private TState currentStoreState;

        public Store()
        {
            store = new BehaviorSubject<TState>(currentStoreState);
        }

        /// <summary>
        /// Sets a new state for the store.
        /// </summary>
        /// <param name="newState"></param>
        public void SetState(TState newState)
        {
            currentStoreState = newState;
            DispatchState(currentStoreState);
        }

        /// <summary>
        /// Sets a new state based on the old one. It updates all properties which are set
        /// by <paramref name="update"/>.
        /// </summary>
        /// <param name="update"></param>
        public void UpdateState(Action<TState> update)
        {
            var updatedState = currentStoreState.Copy();
            update(updatedState);
            SetState(updatedState);
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
            return store.AsObservable()
                .Select(map)
                .DistinctUntilChanged();
        }

        /// <summary>
        /// Notifies observer that the store state changed.
        /// </summary>
        /// <param name="state"></param>
        private void DispatchState(TState state)
        {
            store.OnNext(state);
        }
    }
}
