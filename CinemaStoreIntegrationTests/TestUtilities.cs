using CinemaStore;
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

            if (!String.IsNullOrEmpty(Setup.token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Setup.token);
            }

            return await client.PostAsync(new Uri(route), textContent);
        }

        internal static async Task<Y> Get<Y>(HttpClient client, string route)
        {
            
            if (!String.IsNullOrEmpty(Setup.token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Setup.token);
            }
           
            var result = await client.GetAsync(new Uri(route));
           
            Assert.True(result.IsSuccessStatusCode);
            
            return JsonConvert.DeserializeObject<Y>(
                await result.Content.ReadAsStringAsync());
        }



    }
}