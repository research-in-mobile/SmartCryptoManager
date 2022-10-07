using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Xamarin.Essentials;

using SmartCryptoManager.Exceptions;
using System.IO;
using System.Reflection;

namespace SmartCryptoManager.Services
{
    public class RequestService : IRequestService
    {
        readonly JsonSerializerSettings serializerSettings;

        public RequestService()
        {
            serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };

            serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token = "", bool IsMocked = false)
        {
            if(IsMocked)
            {
                return await GetJson<TResult>(uri);
            }

            var httpClient = CreateHttpClient(token);
            var response = await httpClient.GetAsync(uri);

            await HandleResponse(response);

            var serialized = await response.Content.ReadAsStringAsync();
            var result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings));

            return result;
        }

        public Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", bool IsMocked = false) => PostAsync<TResult, TResult>(uri, data, token, IsMocked);

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "", bool IsMocked = false)
        {
            var httpClient = CreateHttpClient(token);
            var serialized = await Task.Run(() => JsonConvert.SerializeObject(data, serializerSettings));
            var response = await httpClient.PostAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            var responseData = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, serializerSettings));
        }

        public Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", bool IsMocked = false) => PutAsync<TResult, TResult>(uri, data, token, IsMocked);

        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data, string token = "", bool IsMocked = false)
        {
            var httpClient = CreateHttpClient(token);
            var serialized = await Task.Run(() => JsonConvert.SerializeObject(data, serializerSettings));
            var response = await httpClient.PutAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            var responseData = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, serializerSettings));
        }

        private HttpClient CreateHttpClient(string token = "")
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                throw new ConnectivityException();
            }

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                if (IsEmail(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Email", token);
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return httpClient;
        }

        private bool IsEmail(string email) => new EmailAddressAttribute().IsValid(email);

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception(content);
                }

                throw new HttpRequestException(content);
            }
        }

        private async Task<TResult> GetJson<TResult>(string uri)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(RequestService)).Assembly;
            Stream stream = assembly.GetManifestResourceStream(MockedFileFromUri.GetFileLocation(uri));
            TResult result;

            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<TResult>(json);
            }

            await Task.Delay(0);

            return result;
        }

    }
}