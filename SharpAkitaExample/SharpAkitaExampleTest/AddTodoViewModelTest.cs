using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpAkita.Api.Store;
using SharpAkitaExample.Model;
using SharpAkitaExample.ViewModels;

namespace SharpAkitaExampleTest
{
    [TestClass]
    public class AddTodoViewModelTest
    {
        [TestMethod]
        public void CanAddTest()
        {
            var viewModel = new AddTodoViewModel(CreateStore())
            {
                Name = "Test",
                Description = "Test"
            };

            Assert.IsTrue(viewModel.AddCommand.CanExecute());
        }

        [TestMethod]
        public void CanNotAddTest()
        {
            var viewModel = new AddTodoViewModel(CreateStore())
            {
                Name = "Test"
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute());
        }

        [TestMethod]
        public void AddTest()
        {
            var viewModel = new AddTodoViewModel(CreateStore())
            {
                Name = "Test",
                Description = "Test"
            };

            viewModel.AddCommand.Execute();

            Assert.IsTrue(viewModel.AddCommand.CanExecute());
        }

        private EntityStore<EntityState<Todo>, Todo> CreateStore()
        {
            return new EntityStore<EntityState<Todo>, Todo>(() => new EntityState<Todo>());
        }
    }
}
