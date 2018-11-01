using SharpAkitaExample.Views;
using Prism.Ioc;
using System.Windows;
using SharpAkita.Api.Store;
using SharpAkitaExample.Model;
using SharpAkitaExample.Akita;

namespace SharpAkitaExample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var todoStore = new EntityStore<EntityState<Todo>, Todo>(() => new EntityState<Todo>());
            containerRegistry.RegisterInstance(todoStore);
            containerRegistry.Register<TodoQuery>();
        }
    }
}
