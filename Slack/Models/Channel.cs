using System.Text.Json.Serialization;

namespace SlackMessageArchiver.Slack.Models;

public class Channel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}