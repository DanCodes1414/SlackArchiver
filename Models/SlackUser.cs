using System.Text.Json.Serialization;

namespace SlackMessageArchiver.Models;

public class SlackUser
{
    [JsonPropertyName("tz")]
    public string Timezone { get; set; }
    
    [JsonPropertyName("profile")]
    public SlackUserProfile Profile { get; set; }
}