﻿using System.Collections.Generic;
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

        internal History()
        {
            history = new List<IStoreCommand>();
            lastCommandIndex = -1;
        }

        /// <summary>
        /// Adds <paramref name="command"/> to the history.
        /// </summary>
        /// <param name="command"></param>
        internal void Add(IStoreCommand command)
        {
            // remove all after current command
            var commandsToRemove = history.Where(c => history.IndexOf(c) > lastCommandIndex);
            foreach (var c in commandsToRemove)
            {
                history.Remove(c);
            }
            
            history.Add(command);
            lastCommandIndex = history.IndexOf(command);
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
    }
}
