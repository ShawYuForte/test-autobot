using System.Threading.Tasks;
using forte.models.classes;

namespace forte.services.events
{
    public interface ILeaderBoardSignalRConnectionService
    {
        Task<LeaderBoardSignalRConnection> GetUserConnectionAsync(string connectionId);

        Task SaveConnectionAsync(LeaderBoardSignalRConnection connection);

        Task RemoveUserConnectionAsync(LeaderBoardSignalRConnection connection);
    }
}
