using System;
using System.Threading.Tasks;
using forte.models;
using forte.models.streaming;

namespace forte.services
{
    public interface IStreamingService
    {
        void UpdateAzureContextMode(bool useSingleContext);

        Task<VideoStreamModelEx> LinkVideoStreamResourcesAsync(SessionType? type, string requestRef, Guid sessionId, DateTime programStart, DateTime programEnd);

        Task<VideoStreamModelEx> StreamAsync(SessionType? type, string requestRef, Guid sessionId, DateTime programStart, DateTime programEnd);

        Task<VideoStreamModelEx> StreamPublishAsync(SessionType? type, string requestRef, Guid sessionId);

        Task StopStreamAsync(SessionType? type, string requestRef, Guid sessionId, bool deleteAsset);

        Task<VideoStreamModel> FetchSessionVideoStreamAsync(Guid sessionId);

        Task DeletePublicVideoAsync(string uid);

        Task<int> StopAllInconsistentRunningChannelsAsync();
    }
}
