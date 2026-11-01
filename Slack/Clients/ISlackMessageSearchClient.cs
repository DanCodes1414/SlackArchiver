using Refit;
using SlackMessageArchiver.Slack.Models;

namespace SlackMessageArchiver.Slack.Clients;

public interface ISlackMessageSearchClient
{
    [Get("")]
    Task<MessageSearchResponse> SearchUserMessagesAsync([Query] MessageSearchRequest messageSearchQuery);
}