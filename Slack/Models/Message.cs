using System.Text.Json.Serialization;

namespace SlackMessageArchiver.Slack.Models;

public class Message
{
    [JsonPropertyName("iid")]
    public string Id { get; set; }

    [JsonPropertyName("channel")]
    public Channel Channel { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("ts")]
    public string Timestamp { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("permalink")]
    public string Permalink { get; set; }
}