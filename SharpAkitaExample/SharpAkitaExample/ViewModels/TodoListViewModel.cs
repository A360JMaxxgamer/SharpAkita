using Prism.Mvvm;
using SharpAkitaExample.Akita;
using SharpAkitaExample.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SharpAkitaExample.ViewModels
{
    public class TodoListViewModel : BindableBase
    {
        private readonly List<Todo> _allTodos;
        private DateTime _endDate;
        private DateTime _startDate;
        private ObservableCollection<Todo> _todos;

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value);
                RecalculateTodos();
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                SetProperty(ref _startDate, value);
                RecalculateTodos();
            }
        }

        public ObservableCollection<Todo> Todos
        {
            get => _todos;
            set
            {
                SetProperty(ref _todos, value);
            }
        }

        public TodoListViewModel(TodoQuery query)
        {
            _allTodos = new List<Todo>();
            Todos = new ObservableCollection<Todo>();
            query.Select(s => s).Subscribe(state => OnNewState(state));
        }

        private void OnNewState(SharpAkita.Api.Store.EntityState<Todo> state)
        {
            _allTodos.Clear();
            foreach (var entity in state.Entities.Select(e => e.Value))
            {
                _allTodos.Add(entity);
            }
            RecalculateTodos();
        }

        private void RecalculateTodos()
        {
            Todos.Clear();

            foreach (var todo in _allTodos.Where(todo => todo.Date >= StartDate && todo.Date <= EndDate))
            {
                Todos.Add(todo);
            }
        }
    }
}