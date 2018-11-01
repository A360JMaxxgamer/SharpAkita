using Prism.Commands;
using Prism.Mvvm;
using SharpAkita.Api.Store;
using SharpAkitaExample.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SharpAkitaExample.ViewModels
{
    public class AddTodoViewModel : BindableBase
    {
        private string _name;
        private string _description;
        private DateTime _date;
        private readonly EntityStore<EntityState<Todo>, Todo> _store;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public DelegateCommand AddCommand { get; set; }

        public AddTodoViewModel(EntityStore<EntityState<Todo>, Todo> store)
        {
            _store = store;
            AddCommand = new DelegateCommand(Add, CanAdd);
        }

        private void Add()
        {
            var id = Guid.NewGuid();
            _store.Add(id.ToString(), new Todo
            {
                Name = Name,
                Description = Description,
                Date = Date
            });
        }

        private bool CanAdd()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            AddCommand.RaiseCanExecuteChanged();
        }
    }
}
