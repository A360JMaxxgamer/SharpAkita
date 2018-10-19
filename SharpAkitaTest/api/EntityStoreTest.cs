using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpAkita.Api.Store;

namespace SharpAkitaTest.api
{
    [TestClass]
    public class EntityStoreTest
    {
        [TestMethod]
        public void AddTest()
        {
            var entityForCheck = new StoreTestEntity();
            var store = new EntityStore<StoreTestEntity>();
            store.EntityAdded += (o, e) => entityForCheck.StringValue = store.GetById(e).StringValue;

            store.Add("1", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });

            Assert.AreEqual(1, store.Count);
            Assert.AreEqual("Hello", entityForCheck.StringValue);
        }

        [TestMethod]
        public void UpdateByPropertiesTest()
        {
            var entityForCheck = new StoreTestEntity();
            var store = new EntityStore<StoreTestEntity>();
            store.EntityChanged += (o, e) =>
            {
                entityForCheck.IntValue = store.GetById(e).IntValue;
                entityForCheck.StringValue = store.GetById(e).StringValue;
            };
            store.Add("1", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });
            var valueDic = new Dictionary<string, object>
            {
                { nameof(StoreTestEntity.IntValue), 4 },
                { nameof(StoreTestEntity.StringValue), "Hello world" }
            };

            store.UpdateByProperties("1", valueDic);

            Assert.AreEqual(1, store.Count);
            Assert.AreEqual(4, entityForCheck.IntValue);
            Assert.AreEqual("Hello world", entityForCheck.StringValue);
        }

        [TestMethod]
        public void UndoTest()
        {
            var store = new EntityStore<StoreTestEntity>();
            store.Add("1", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });

            store.Undo();

            Assert.AreEqual(0, store.Count);
        }

        [TestMethod]
        public void RedoTest()
        {
            var entityForCheck = new StoreTestEntity();
            var store = new EntityStore<StoreTestEntity>();
            store.EntityAdded += (o, e) =>
            {
                entityForCheck.IntValue = store.GetById(e).IntValue;
                entityForCheck.StringValue = store.GetById(e).StringValue;
            };
            store.Add("1", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });
            store.Undo();

            store.Redo();

            Assert.AreEqual(1, store.Count);
            Assert.AreEqual("Hello", entityForCheck.StringValue);
        }
    }

    public class StoreTestEntity
    {
        public int IntValue { get; set; }   

        public string StringValue { get; set; }
    }
}
