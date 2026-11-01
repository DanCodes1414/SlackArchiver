using System.Text.Json.Serialization;

namespace SlackMessageArchiver.Slack.Models;

[JsonSerializable(typeof(MessageSearchResponse))]
public class MessageSearchResponse : BaseResponse
{
    [JsonPropertyName("messages")]
    public MatchResponse Matches { get; set; }
}