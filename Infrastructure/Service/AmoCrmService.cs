using System.Net;
using System.Net.Http.Headers;
using Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Service;

public class AmoCrmService(HttpClient httpClient, IOptions<AmocrmOptions> credentials)
{
    public async Task<string> GetAsync(string uri, CancellationToken cancellationToken)
    {
        try
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", credentials.Value.Token);

            var result =
                await httpClient.GetAsync(uri, cancellationToken);
        
      
            switch (result.StatusCode)
            {
                case HttpStatusCode.TooManyRequests:
                    throw new Exception("TOO MANY REQUESTS, The allowed number of requests per second has been exceeded.");
                case HttpStatusCode.Forbidden:
                    throw new Exception("Account blocked for repeatedly exceeding the number of requests per second");
                case HttpStatusCode.Unauthorized:
                    throw new Exception("Unauthorized, Please check your token");
            }


            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Error while getting data from AmoCRM. Error code: " + result.StatusCode + "Message: " + result.ReasonPhrase + "");
            }
        
            return
                await result.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (Exception e)
        {
            
            throw new Exception("Error while getting data from AmoCRM: " + e.Message);
        
        
        }
        
       
    }
}