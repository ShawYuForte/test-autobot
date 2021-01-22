using Microsoft.WindowsAzure.Storage.Table;

namespace forte.models.classes
{
    public class LeaderBoardSignalRConnection : TableEntity
    {
        public string UserId { get; set; }

        public string ConnectionId { get; set; }

        public string SessionId { get; set; }
    }
}
