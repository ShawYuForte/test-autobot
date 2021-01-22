using System.Threading.Tasks;
using forte.models.commands;

namespace forte.services.commands
{
    /// <summary>
    ///     Any class that implements this interface supports execution via async command. The async command must
    ///     still be compatible with the target, but that is completely handled and owned by the target
    /// </summary>
    public interface ICommandDelegate
    {
        /// <summary>
        ///     Execute a command
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="System.NotSupportedException">Command was sent in incorrectly and is not supported as such.</exception>
        Task ExecuteCommandAsync(Command command);
    }
}
