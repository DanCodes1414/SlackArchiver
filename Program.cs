using System.Text.Json;using System.Text.Json.Serialization;
using Amazon.S3;
using Amazon.SecretsManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Refit;
using SlackMessageArchiver.Extensions;
using SlackMessageArchiver.Models;
using SlackMessageArchiver.Settings;
using SlackMessageArchiver.Slack;
using SlackMessageArchiver.Slack.Clients;
using SlackMessageArchiver.Slack.Handlers;
using SlackMessageArchiver.Slack.Services;
using SlackMessageArchiver.Storage.Services;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonSecretsManager>();
builder.Services.AddAWSService<IAmazonS3>();

using (var secretsManager = builder.Services.BuildServiceProvider().GetRequiredService<IAmazonSecretsManager>())
{
    builder.Configuration.AddAwsSecretsManager(secretsManager);
}

builder.Services.Configure<SlackSettings>(builder.Configuration.GetSection(SlackSettings.SettingName));
builder.Services.Configure<AmazonS3Settings>(builder.Configuration.GetSection(AmazonS3Settings.SettingName));

builder.Services.AddTransient<AuthenticationDelegatingHandler>();
var slackRefitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(SlackJsonSerializerContext.Default.Options)
};
builder.Services.AddRefitClient<ISlackMessageSearchClient>(slackRefitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://slack.com/api/search.messages"))
    .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

builder.Services.AddTransient<ISlackMessageSearchService, SlackMessageSearchService>();
builder.Services.AddTransient<IMessageArchivalService, MessageArchivalService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, SlackHuddleJsonSerializerContext.Default);
});

var app = builder.Build();

var slackArchiverApi = app.MapGroup("/SlackMessages/Archive");
// TODO: Delete this endpoint after exporting from April to July
slackArchiverApi.MapPost("/Month/{month:int}", async (
    [FromRoute] int month,
    ISlackMessageSearchService slackSearchService,
    IMessageArchivalService messageArchivalService) =>
{
    var days = DateTime.DaysInMonth(2025, month);
    for (var i = 1; i <= days; i++)
    {
        var date = new DateTime(2025, month, i);
        var slackMessages = await slackSearchService.SearchUserMessagesOnDateAsync(date);
        var jsonMessages = JsonSerializer.Serialize(slackMessages.Messages, SlackJsonSerializerContext.Default.Options);
        await messageArchivalService.ArchiveMessageAsync(jsonMessages, slackMessages.Date);
    }
});
slackArchiverApi.MapPost("/Yesterday", async (
    ISlackMessageSearchService slackSearchService,
    IMessageArchivalService messageArchivalService) =>
{
    var yesterdayDate = DateTime.UtcNow.AddDays(-1);
    var slackMessages = await slackSearchService.SearchUserMessagesOnDateAsync(yesterdayDate);
    var jsonMessages = JsonSerializer.Serialize(slackMessages.Messages, SlackJsonSerializerContext.Default.Options);
    await messageArchivalService.ArchiveMessageAsync(jsonMessages, slackMessages.Date);
});
slackArchiverApi.MapPost("/Huddle", async (
    [FromBody] SlackHuddleWebhookMessage message,
    IOptions<SlackSettings> slackSettings,
    IMessageArchivalService messageArchivalService) =>
{
    if (message.Type == "url_verification")
    {
        await messageArchivalService.ArchiveMessageAsync(message.Token, DateOnly.MaxValue);
        return Results.Text(message.Challenge, contentType: "application/text", statusCode: 200);
    }

    if (message.Token != slackSettings.Value.WebhookToken)
    {
        return Results.Unauthorized();
    }

    if (message.EventPayload?.User?.Profile?.DisplayName != slackSettings.Value.User)
    {
        return Results.Accepted();
    }

    // Hiding the token when serializing the event payload
    message.Token = null;

    var jsonHuddle = JsonSerializer.Serialize(message, SlackHuddleJsonSerializerContext.Default.Options);
    await messageArchivalService.HuddleJoinedAsync(jsonHuddle);
    return Results.Ok();
});

app.Run();

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(SlackHuddleWebhookMessage))]
[JsonSerializable(typeof(SlackEventPayload))]
[JsonSerializable(typeof(SlackUser))]
[JsonSerializable(typeof(SlackUserProfile))]
internal partial class SlackHuddleJsonSerializerContext : JsonSerializerContext;