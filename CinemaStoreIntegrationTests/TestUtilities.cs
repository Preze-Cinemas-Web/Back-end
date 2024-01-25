using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text;

namespace CinemaStoreIntegrationTests
{
    [ExcludeFromCodeCoverage]
    internal static class TestUtilities
    {
        internal static async Task<HttpResponseMessage> Post(HttpClient client, string route, Object obj)
        {
            using var textContent = new ByteArrayContent(Encoding.UTF8.GetBytes(
                JsonConvert.SerializeObject(obj)));
            textContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await client.PostAsync(new Uri(route), textContent);
        }
    }
}