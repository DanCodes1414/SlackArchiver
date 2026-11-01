using System.Text.Json.Serialization;

namespace SlackMessageArchiver.Slack.Models;

public class MatchResponse
{
    [JsonPropertyName("matches")]
    public List<Message> Messages { get; set; }

    [JsonPropertyName("paging")]
    public Pagination Pagination { get; set; }
}