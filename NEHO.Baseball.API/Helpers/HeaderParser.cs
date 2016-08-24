using System.Linq;
using System.Net.Http.Headers;

using Newtonsoft.Json;

namespace NEHO.Baseball.API.Helpers
{
    public static class HeaderParser
    {
        public static PagingInfo FindAndParsePagingInfo(HttpResponseHeaders httpResponseHeaders)
        {
            if (httpResponseHeaders.Contains("X-Pagination"))
            {
                var xPagination = httpResponseHeaders.First(p => p.Key == "X-Pagination").Value;

                return JsonConvert.DeserializeObject<PagingInfo>(xPagination.First());
            }

            return null;
        }
    }
}