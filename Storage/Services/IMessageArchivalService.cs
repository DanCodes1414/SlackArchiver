namespace SlackMessageArchiver.Storage.Services;

public interface IMessageArchivalService
{
    Task ArchiveMessageAsync(string jsonMessages, DateOnly messageDate);
    Task HuddleJoinedAsync(string jsonHuddle);
}