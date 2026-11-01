namespace SlackMessageArchiver.Settings;

public class SlackSettings
{
    public const string SettingName = "Slack";
    public string User { get; set; }
    public string UserToken { get; set; }
    public string WebhookToken { get; set; }
}