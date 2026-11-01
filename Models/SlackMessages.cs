using SlackMessageArchiver.Slack.Models;

namespace SlackMessageArchiver.Models;

public class SlackMessages
{
    public DateOnly Date { get; set; }

    public IEnumerable<Message> Messages { get; set; }
}