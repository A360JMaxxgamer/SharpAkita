using System.Collections.Generic;
using System.Linq;

namespace SharpAkita.Api.Store.Commands
{
    /// <summary>
    /// Keeps track of all triggered commands. It can do and undo all commands. 
    /// It points on the last acted command.
    /// </summary>
    internal class History
    {
        private readonly List<IStoreCommand> history;
        private int lastCommandIndex;

        /// <summary>
        /// Amount of actions which are saved. Default is 5.
        /// </summary>
        internal int HistorySize { get; set; }

        internal History()
        {
            history = new List<IStoreCommand>();
            lastCommandIndex = -1;
            HistorySize = 5;
        }

        /// <summary>
        /// Adds <paramref name="command"/> to the history.
        /// </summary>
        /// <param name="command"></param>
        internal void Add(IStoreCommand command)
        {
            // remove all after current command
            var commandsToRemove = history.Where(c => history.IndexOf(c) >= lastCommandIndex).ToList();
            foreach (var c in commandsToRemove)
            {
                history.Remove(c);
            }
            
            history.Add(command);
            lastCommandIndex = history.IndexOf(command);
            RemoveToManyActions();
        }
        
        /// <summary>
        /// Triggers the current command. And moves the index.
        /// </summary>
        internal void Do()
        {
            if (lastCommandIndex < history.Count)
            {
                history[lastCommandIndex].Do();
                lastCommandIndex++;
            }
        }

        /// <summary>
        /// Reverts/Undoes the last command.
        /// </summary>
        internal void Undo()
        {
            if (lastCommandIndex >= 0)
            {
                history[lastCommandIndex - 1].Undo();
                lastCommandIndex--;
            }
        }

        /// <summary>
        /// Removes the first action in the history as long as
        /// the actions count is greater than <see cref="HistorySize"/>.
        /// </summary>
        private void RemoveToManyActions()
        {
            while(history.Count > HistorySize)
            {
                history.RemoveAt(0);
            }
        }
    }
}
