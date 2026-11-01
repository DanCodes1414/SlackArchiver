using System.Text.Json.Serialization;

namespace SlackMessageArchiver.Slack.Models;

public class Pagination
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("pages")]
    public int Pages { get; set; }  
}