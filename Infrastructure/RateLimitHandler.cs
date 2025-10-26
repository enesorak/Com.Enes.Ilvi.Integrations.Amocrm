namespace Infrastructure;
public class RateLimitHandler : DelegatingHandler
{
    private static DateTime _lastRequestTime = DateTime.MinValue;
    private static readonly TimeSpan _rateLimit = TimeSpan.FromSeconds(7); // 7 seconds rate limit

    public RateLimitHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Ensure the delay between requests
        var timeSinceLastRequest = DateTime.Now - _lastRequestTime;
        if (timeSinceLastRequest < _rateLimit)
        {
            var delay = _rateLimit - timeSinceLastRequest;
            await Task.Delay(delay, cancellationToken);  // Wait if necessary
        }

        // Make the request
        var response = await base.SendAsync(request, cancellationToken);

        // Update the last request time
        _lastRequestTime = DateTime.Now;

        return response;
    }
}