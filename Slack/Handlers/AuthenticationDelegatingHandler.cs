using Microsoft.Extensions.Options;
using SlackMessageArchiver.Settings;

namespace SlackMessageArchiver.Slack.Handlers;

public class AuthenticationDelegatingHandler(IOptions<SlackSettings> options) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("Authorization", $"Bearer {options.Value.UserToken}");
        return base.SendAsync(request, cancellationToken);
    }
}