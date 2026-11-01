using Refit;

namespace SlackMessageArchiver.Slack.Models;

public class MessageSearchRequest
{
    [AliasAs("query")]
    public string SearchQuery  { get; set; }

    [AliasAs("sort")]
    public string SortType { get; set; } = "timestamp";

    [AliasAs("sort_dir")]
    public string SortDirection { get; set; } = "desc";

    public int Page { get; set; } = 1;

    public int Count { get; set; } = 100;

    public int Pretty { get; set; } = 1;
}