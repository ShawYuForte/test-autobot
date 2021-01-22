using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using forte.models;
using forte.models.classes;

namespace forte.services
{
    public interface IClassSessionMessagesService
    {
        Task<IReadOnlyCollection<ClassSessionMessageModel>> GetSessionMessagesAsync(Guid sessionId);

        Task<ClassSessionMessageModel> UpsertClassSessionMessageAsync(
            UpsertClassSessionMessageModel classSessionMessage);

        Task<ClassSessionMessageModel> DeleteClassSessionMessageAsync(Guid messageId);

        Task<ClassSessionMessageLikeModel> AddClassSessionMessageLikeAsync(Guid messageId, string userId);

        Task RemoveClassSessionMessageLikeAsync(Guid messageLikeId);

        Task MarkMessagesAsReadAsync(ReadClassSessionMessageModel[] readMessages);

        Task<ResultPage<SessionMessageThreadModel>> GetSessionMessageThreadsPagedAsync(string userId, int skip, int take, string filter);

        Task<ResultPage<ClassSessionMessageModel>> GetUserThreadMessagesAsync(string userId, string chatWithUserId, int skip, int take);

        Task<ResultPage<ClassSessionMessageModel>> GetSessionThreadMessagesAsync(Guid sessionId, int skip, int take);
    }
}
