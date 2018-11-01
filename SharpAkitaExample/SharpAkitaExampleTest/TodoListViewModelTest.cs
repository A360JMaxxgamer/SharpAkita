using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpAkita.Api.Store;
using SharpAkitaExample.Akita;
using SharpAkitaExample.Model;
using SharpAkitaExample.ViewModels;
using System;

namespace SharpAkitaExampleTest
{
    [TestClass]
    public class TodoListViewModelTest
    {
        [TestMethod]
        public void CountTest()
        {
            var store = CreateStore();
            var query = new TodoQuery(store);
            var viewModel = new TodoListViewModel(query);

            store.Add("1", new Todo
            {
                Name = "1",
                Description = "1",
                Date = new DateTime()
            });

            Assert.AreEqual(1, viewModel.Todos.Count);
        }

        private EntityStore<EntityState<Todo>, Todo> CreateStore()
        {
            return new EntityStore<EntityState<Todo>, Todo>(() => new EntityState<Todo>());
        }
    }
}
