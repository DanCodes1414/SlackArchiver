using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using SlackMessageArchiver.Settings;

namespace SlackMessageArchiver.Extensions;

public static class AwsSecretsManagerExtension
{
    public static void AddAwsSecretsManager(this IConfigurationBuilder configurationBuilder, IAmazonSecretsManager secretsManager)
    {
        var configurationSource = new AwsSecretsManagerConfigurationSource(secretsManager);
        configurationBuilder.Add(configurationSource);
    }
}

public class AwsSecretsManagerConfigurationSource(IAmazonSecretsManager secretsManager) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new AwsSecretsManagerConfigurationProvider(secretsManager);
    }
}

public class AwsSecretsManagerConfigurationProvider(IAmazonSecretsManager secretsManager) : ConfigurationProvider
{
    public override void Load()
    {
        var slackSecrets = GetSlackSecrets();
        var amazonS3Secrets = GetAmazonS3Secrets();
        var secrets = slackSecrets.Concat(amazonS3Secrets).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Data = secrets;
    }

    private Dictionary<string, string> GetSlackSecrets()
    {
        var request = new GetSecretValueRequest { SecretId = SlackSettings.SettingName };
        var response = secretsManager.GetSecretValueAsync(request).Result;
        var settings = JsonSerializer.Deserialize<AppSettings>(response.SecretString, AwsJsonSerializerContext.Default.Options);
        return settings.Slack.ToDictionary(SlackSettings.SettingName);
    }
    
    private Dictionary<string, string> GetAmazonS3Secrets()
    {
        var request = new GetSecretValueRequest { SecretId = AmazonS3Settings.SettingName };
        var response = secretsManager.GetSecretValueAsync(request).Result;
        var settings = JsonSerializer.Deserialize<AppSettings>(response.SecretString, AwsJsonSerializerContext.Default.Options);
        return settings.AmazonS3.ToDictionary(AmazonS3Settings.SettingName);
    }
}

public static class SettingsDictionaryExtensions
{
    public static Dictionary<string, string> ToDictionary(this Dictionary<string, string> settings, string settingName)
    {
        var secretDictionary = new Dictionary<string, string>();
        foreach (var kvp in settings)
        {
            secretDictionary[$"{settingName}:{kvp.Key}"] = kvp.Value;
        }

        return secretDictionary;
    }
}

public class AppSettings
{
    public Dictionary<string, string> Slack { get; set; }
    public Dictionary<string, string> AmazonS3 { get; set; }
}

[JsonSerializable(typeof(AppSettings))]
internal partial class AwsJsonSerializerContext : JsonSerializerContext;