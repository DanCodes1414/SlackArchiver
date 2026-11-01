using Microsoft.Extensions.Options;
using SlackMessageArchiver.Extensions;
using SlackMessageArchiver.Models;
using SlackMessageArchiver.Settings;
using SlackMessageArchiver.Slack.Clients;
using SlackMessageArchiver.Slack.Models;

namespace SlackMessageArchiver.Slack.Services;

public class SlackMessageSearchService(
    ISlackMessageSearchClient searchClient,
    IOptions<SlackSettings> slackSettings)
    : ISlackMessageSearchService
{
    public async Task<SlackMessages> SearchUserMessagesOnDateAsync(DateTime date)
    {
        var searchQuery = $"from:@{slackSettings.Value.User} on:{date:yyyy-MM-dd}";
        var query = new MessageSearchRequest { SearchQuery = searchQuery };

        IEnumerable<Message> messages = [];
        var pages = 1;
        while (query.Page <= pages)
        {
            var response = await searchClient.SearchUserMessagesAsync(query);
            if (response.IsSuccess)
            {
                messages = [..messages, ..response.Matches.Messages];
            }

            pages = response.Matches.Pagination.Pages;
            query.Page++;
        }

        return new SlackMessages
        {
            Date = date.ToDateOnly(),
            Messages = messages
        };
    }
}