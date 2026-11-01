using System.Text.Json.Serialization;

namespace SlackMessageArchiver.Models;

public class SlackHuddleWebhookMessage
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("challenge")]
    public string Challenge { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("event")]
    public SlackEventPayload EventPayload { get; set; }

    [JsonPropertyName("event_time")]
    public int EventTime { get; set; }
}