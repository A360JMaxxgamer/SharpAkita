namespace SharpAkita.Api.Store.Commands
{
    /// <summary>
    /// Interface for store commands.
    /// </summary>
    internal interface IStoreCommand
    {
        /// <summary>
        /// Trigger command.
        /// </summary>
        void Do();

        /// <summary>
        /// Rollback all actions for this command.
        /// </summary>
        void Undo();
    }
}
