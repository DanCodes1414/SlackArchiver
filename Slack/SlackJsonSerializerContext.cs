using System.Text.Json.Serialization;
using SlackMessageArchiver.Models;
using SlackMessageArchiver.Slack.Models;

namespace SlackMessageArchiver.Slack;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(MessageSearchRequest))]
[JsonSerializable(typeof(MessageSearchResponse))]
[JsonSerializable(typeof(List<Message>))]
[JsonSerializable(typeof(Channel))]
[JsonSerializable(typeof(MatchResponse))]
[JsonSerializable(typeof(Pagination))]
[JsonSerializable(typeof(SlackMessages))]
internal partial class SlackJsonSerializerContext : JsonSerializerContext;