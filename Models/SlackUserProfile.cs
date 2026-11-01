using System.Text.Json.Serialization;

namespace SlackMessageArchiver.Models;

public class SlackUserProfile
{
    [JsonPropertyName("huddle_state")]
    public string HuddleState { get; set; }
    
    [JsonPropertyName("huddle_state_call_id")]
    public string HuddleStateCallId { get; set; }
    
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; }
}