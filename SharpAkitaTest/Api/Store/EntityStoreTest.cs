using SharpAkita.Api.Store;
using Xunit;

namespace SharpAkitaTest.Api.Store
{
    public class EntityStoreTest
    {
        [Fact]
        public void GetEntitiesTest()
        {
            var entityStore = new EntityStore<EntityState<int>, int>(CreateInitialState<int>);

            entityStore.Add("1", 1);
            entityStore.Add("2", 2);

            var entities = entityStore.GetEntities();
            Assert.Equal(2, entities.Count);
            Assert.Equal(1, entities["1"]);
            Assert.Equal(2, entities["2"]);
        }

        [Fact]
        public void AddTest()
        {
            var entityStore = new EntityStore<EntityState<int>, int>(CreateInitialState<int>);

            entityStore.Add("1", 1);

            var entities = entityStore.GetEntities(); 
            Assert.Equal(1, entities.Count);
            Assert.Equal(1, entities["1"]);
        }

        [Fact]
        public void CreateOrReplaceTest()
        {
            var entityStore = new EntityStore<EntityState<int>, int>(CreateInitialState<int>);
            entityStore.Add("1", 1);

            entityStore.CreateOrReplace("2", 2);
            entityStore.CreateOrReplace("1", 5);

            var entities = entityStore.GetEntities();
            Assert.Equal(2, entities.Count);
            Assert.Equal(5, entities["1"]);
            Assert.Equal(2, entities["2"]);
        }

        [Fact]
        public void RemoveTest()
        {
            var entityStore = new EntityStore<EntityState<int>, int>(CreateInitialState<int>);
            entityStore.Add("1", 1);

            entityStore.Remove("1");

            var entities = entityStore.GetEntities();
            Assert.Equal(0, entities.Count);
        }

        [Fact]
        public void ActivePrimitiveTypeTest()
        {
            var entityStore = new EntityStore<EntityState<int>, int>(CreateInitialState<int>);
            entityStore.CreateOrReplace("2", 2);
            entityStore.CreateOrReplace("1", 5);

            entityStore.SetActive("2");
            entityStore.UpdateActive(i => 6);

            var entities = entityStore.GetEntities();
            Assert.Equal(6, entities["2"]);
        }

        [Fact]
        public void ActiveComplexObjectTypeTest()
        {
            var entityStore = new EntityStore<EntityState<Test>, Test>(CreateInitialState<Test>);
            entityStore.CreateOrReplace("1", new Test
            {
                Number = 1,
                Text = "Bla"
            });
            entityStore.CreateOrReplace("2", new Test
            {
                Number = 1,
                Text = "Blub"
            });

            entityStore.SetActive("2");
            entityStore.UpdateActive(t => 
            {
                t.Text = "It works";
                return t;
            });

            var entities = entityStore.GetEntities();
            Assert.Equal("It works", entities["2"].Text);
            Assert.Equal("Bla", entities["1"].Text);
        }

        private EntityState<TEntity> CreateInitialState<TEntity>()
        {
            return new EntityState<TEntity>();
        }
    }

    internal class Test
    {
        public string Text { get; set; }
        public int Number { get; set; }
    }
}
