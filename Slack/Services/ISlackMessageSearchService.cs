using SlackMessageArchiver.Models;

namespace SlackMessageArchiver.Slack.Services;

public interface ISlackMessageSearchService
{
    Task<SlackMessages> SearchUserMessagesOnDateAsync(DateTime date);
}