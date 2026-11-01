namespace SlackMessageArchiver.Settings;

public class AmazonS3Settings
{
    public const string SettingName = "AmazonS3";
    public string Bucket { get; set; }
}