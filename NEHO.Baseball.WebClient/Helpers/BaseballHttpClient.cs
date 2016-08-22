using System;
using System.Net.Http;
using System.Net.Http.Headers;

using NEHO.Baseball.Constants;

namespace NEHO.Baseball.WebClient.Helpers
{
    public static class BaseballHttpClient
    {
        public static HttpClient GetClient()
        {
            var client = new HttpClient {BaseAddress = new Uri(BaseballConstants.NEHOBaseballAPI)};
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}