using SharpAkita.Api.Store;
using System;
using Xunit;

namespace SharpAkitaTest.Api.Store
{
    public class StoreTest
    {
        [Fact]
        public void SetStateTest()
        {
            var store = new Store<StoreTestState>();
            StoreTestState testResult = null;
            store.Select(state => state).Subscribe(s => testResult = s);

            store.SetState(new StoreTestState { Test1 = 1, Test2 = "Toast" });

            Assert.NotNull(testResult);
            Assert.Equal(1, testResult.Test1);
            Assert.Equal("Toast", testResult.Test2);
        }

        [Fact]
        public void UpdateStateTest()
        {
            var store = new Store<StoreTestState>();
            StoreTestState testResult = null;
            store.Select(state => state).Subscribe(s => testResult = s);
            store.SetState(new StoreTestState { Test1 = 1, Test2 = "Toast" });

            store.UpdateState(state => 
            {
                state.Test1 = 5;
            });

            Assert.NotNull(testResult);
            Assert.Equal(5, testResult.Test1);
            Assert.Equal("Toast", testResult.Test2);
        }
    }

    internal class StoreTestState
    {
        public int Test1 { get; set; }
        public string Test2 { get; set; }
    }
}
