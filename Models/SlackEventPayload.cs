using System.Text.Json.Serialization;

namespace SlackMessageArchiver.Models;

public class SlackEventPayload
{
    [JsonPropertyName("user")]
    public SlackUser User { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("event_ts")]
    public string EventTimestamp { get; set; }
}