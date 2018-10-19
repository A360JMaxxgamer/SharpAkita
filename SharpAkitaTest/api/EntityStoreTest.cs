using System.Collections.Generic;
using System.Linq;
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

        [TestMethod]
        public void GetByIdTest()
        {
            var test = new StoreTestEntity();
            var test2 = new StoreTestEntity();
            var store = new EntityStore<StoreTestEntity>();
            store.Add("1", test);
            store.Add("2", test2);

            var result = store.GetById("2");
            
            Assert.AreEqual(test2, result);
        }

        [TestMethod]
        public void SelectTest()
        {
            var test1 = new StoreTestEntity { IntValue = 1};
            var test2 = new StoreTestEntity { IntValue = 1};
            var test3 = new StoreTestEntity { IntValue = 2 };
            var store = new EntityStore<StoreTestEntity>();
            store.Add("1", test1);
            store.Add("2", test2);
            store.Add("3", test3);

            var select = store.Select(e => e.IntValue == 1);

            Assert.AreEqual(2, select.Count);
            Assert.IsTrue(select.All(e => e.Value.IntValue == 1));
        }
    }

    public class StoreTestEntity
    {
        public int IntValue { get; set; }   

        public string StringValue { get; set; }
    }
}
