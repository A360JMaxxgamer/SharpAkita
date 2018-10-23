using System;
using System.Reactive.Subjects;

namespace SharpAkita.Api.Store
{
    internal class Store<TState>
    {
        private BehaviorSubject<TState> store;

        private TState currentStoreState;

        //private ReplaySubject<Action> rootDispatcher = new ReplaySubject<Action>();

        public Store()
        {

        }

        public TState SetState(Func<TState, TState> newStateFunction)
        {
            var previousState = currentStoreState;
            currentStoreState = newStateFunction(currentStoreState);

            InitializeStore();
            // Todo handle transaction in process
            DispatchState(currentStoreState);
        }

        private void InitializeStore()
        {
            if (store == null)
            {
                store = new BehaviorSubject<TState>(currentStoreState);
                // rootDispatcher.OnNext() Todo evaluate
            }
        }

        private void DispatchState(TState state)
        {
            store.OnNext(state);
        }
    }
}
