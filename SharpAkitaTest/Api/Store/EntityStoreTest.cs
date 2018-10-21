using System.Collections.Generic;
using System.Linq;
using SharpAkita.Api.Store;
using Xunit;

namespace SharpAkitaTest.api
{
    public class EntityStoreTest
    {
        [Fact]
        public void AddTest()
        {
            var entityForCheck = new StoreTestEntity();
            var store = new EntityStore<StoreTestEntity>();
            store.EntityAdded += (o, e) => entityForCheck.StringValue = store.GetById(e).StringValue;

            store.Add("1", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });

            Assert.Equal(1, store.Count);
            Assert.Equal("Hello", entityForCheck.StringValue);
        }

        [Fact]
        public void AddUndoAddTest()
        {
            var store = new EntityStore<StoreTestEntity>();

            store.Add("1", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });
            store.Add("2", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });
            store.Add("3", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });
            store.Undo();
            store.Undo();
            store.Add("4", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });

            Assert.Equal(2, store.Count);
        }

        [Fact]
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

            Assert.Equal(1, store.Count);
            Assert.Equal(4, entityForCheck.IntValue);
            Assert.Equal("Hello world", entityForCheck.StringValue);
        }

        [Fact]
        public void UndoUpdateTest()
        {
            var entity = new StoreTestEntity { IntValue = 1, StringValue = "Hello" };
            var store = new EntityStore<StoreTestEntity>();
            store.Add("1", entity);
            var valueDic = new Dictionary<string, object>
            {
                { nameof(StoreTestEntity.IntValue), 4 },
                { nameof(StoreTestEntity.StringValue), "Hello world" }
            };
            store.UpdateByProperties("1", valueDic);

            store.Undo();

            Assert.Equal("Hello", entity.StringValue);
        }

        [Fact]
        public void UndoAddTest()
        {
            var store = new EntityStore<StoreTestEntity>();
            store.Add("1", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });

            store.Undo();

            Assert.Equal(0, store.Count);
        }

        [Fact]
        public void RedoAddTest()
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

            Assert.Equal(1, store.Count);
            Assert.Equal("Hello", entityForCheck.StringValue);
        }

        [Fact]
        public void RemoveTest()
        {
            var store = new EntityStore<StoreTestEntity>();
            store.Add("1", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });
            store.Add("2", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });

            store.Remove("1");

            Assert.Equal(1, store.Count);
        }

        [Fact]
        public void RemoveUndoTest()
        {
            var store = new EntityStore<StoreTestEntity>();
            store.Add("1", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });
            store.Add("2", new StoreTestEntity { IntValue = 1, StringValue = "Hello" });
            store.Remove("1");

            store.Undo();

            Assert.Equal(2, store.Count);
        }

        [Fact]
        public void GetByIdTest()
        {
            var test = new StoreTestEntity();
            var test2 = new StoreTestEntity();
            var store = new EntityStore<StoreTestEntity>();
            store.Add("1", test);
            store.Add("2", test2);

            var result = store.GetById("2");
            
            Assert.Equal(test2, result);
        }

        [Fact]
        public void SelectWithConditionTest()
        {
            var test1 = new StoreTestEntity { IntValue = 1};
            var test2 = new StoreTestEntity { IntValue = 1};
            var test3 = new StoreTestEntity { IntValue = 2 };
            var store = new EntityStore<StoreTestEntity>();
            store.Add("1", test1);
            store.Add("2", test2);
            store.Add("3", test3);

            var select = store.Select(e => e.IntValue == 1);

            Assert.Equal(2, select.Count);
            Assert.True(select.All(e => e.Value.IntValue == 1));
        }

        [Fact]
        public void SelectTest()
        {
            var test1 = new StoreTestEntity { IntValue = 1 };
            var test2 = new StoreTestEntity { IntValue = 1 };
            var test3 = new StoreTestEntity { IntValue = 2 };
            var store = new EntityStore<StoreTestEntity>();
            store.Add("1", test1);
            store.Add("2", test2);
            store.Add("3", test3);

            var select = store.Select();

            Assert.Equal(3, select.Count);
        }
    }

    public class StoreTestEntity
    {
        public int IntValue { get; set; }   

        public string StringValue { get; set; }
    }
}
