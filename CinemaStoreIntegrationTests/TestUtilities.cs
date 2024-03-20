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
        internal static async Task<HttpResponseMessage> Get(HttpClient client, string route)
        {
            if (!String.IsNullOrEmpty(Setup.token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Setup.token);
            }

            return await client.GetAsync(new Uri(route));
        }

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

        internal static async Task<HttpResponseMessage> Put(HttpClient client, string route, Object obj)
        {
            using var textContent = new ByteArrayContent(Encoding.UTF8.GetBytes(
               JsonConvert.SerializeObject(obj)));
            textContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            if (!String.IsNullOrEmpty(Setup.token))
            {
                client.DefaultRequestHeaders.Authorization =
                   new AuthenticationHeaderValue("Bearer", Setup.token);
            }
            
            return await client.PutAsync(new Uri(route), textContent);
        }

        internal static async Task<HttpResponseMessage> Delete(HttpClient client, string route)
        {
         
            if (!String.IsNullOrEmpty(Setup.token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Setup.token);
            }
         
            return await client.DeleteAsync(new Uri(route));
        }
    }
}