using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using SlackMessageArchiver.Settings;

namespace SlackMessageArchiver.Storage.Services;

public class MessageArchivalService(IAmazonS3 amazonS3Client, IOptions<AmazonS3Settings> amazonS3Settings) : IMessageArchivalService
{
    public async Task ArchiveMessageAsync(string jsonMessages, DateOnly messageDate)
    {
        var request = new PutObjectRequest
        {
            BucketName = amazonS3Settings.Value.Bucket,
            Key = GenerateFileName("Messages", messageDate),
            ContentBody = jsonMessages
        };
        await amazonS3Client.PutObjectAsync(request);
    }

    public async Task HuddleJoinedAsync(string jsonHuddle)
    {
        var request = new PutObjectRequest
        {
            BucketName = amazonS3Settings.Value.Bucket,
            Key = GenerateFileName("Huddle", DateTime.UtcNow),
            ContentBody = jsonHuddle
        };
        await amazonS3Client.PutObjectAsync(request);
    }

    private static string GenerateFileName(string messageType, DateOnly messageDate)
    {
        return $"{messageDate.Year:0000}/{messageDate:MMMM}/{messageType}_{messageDate.Year:0000}_{messageDate.Month:00}_{messageDate.Day:00}";
    }
    
    private static string GenerateFileName(string messageType, DateTime messageDate)
    {
        return $"{messageDate.Year:0000}/{messageDate:MMMM}/" +
               $"{messageType}_{messageDate.Year:0000}_{messageDate.Month:00}_{messageDate.Day:00}_" +
               $"{messageDate.Hour:00}_{messageDate.Second:00}_{messageDate.Minute:00}";
    }
}