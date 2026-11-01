using System.Text.Json.Serialization;

namespace SlackMessageArchiver.Slack.Models;

public class BaseResponse
{
    [JsonPropertyName("ok")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("error")]
    public string Error { get; set; }
}