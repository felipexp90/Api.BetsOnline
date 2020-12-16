using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Utils
{
    public class GenericUtil
    {
        private readonly HttpClient _httpClient;

        public GenericUtil(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public GenericUtil()
        {
        }

        public static DateTime GetDateZone(DateTime incoming)
        {
            try
            {
                incoming = TimeZoneInfo.ConvertTime(incoming, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
                incoming = incoming.AddTicks(-(incoming.Ticks % TimeSpan.TicksPerSecond));
                return incoming;
            }
            catch (Exception) { return incoming; }
        }

        public async Task<string> GetAsync(string url)
        {
            using (HttpClient client = _httpClient)
            {
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage response = await client.GetAsync(url);
                return await new StreamReader(await response.Content.ReadAsStreamAsync()).ReadToEndAsync();
            }
        }

        public async Task<string> GetAsync(string url, Dictionary<string, string> headers)
        {
            using (HttpClient client = _httpClient)
            {
                client.DefaultRequestHeaders.Clear();
                if (headers != null)
                    foreach (KeyValuePair<string, string> item in headers)
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                HttpResponseMessage response = await client.GetAsync(url);
                return await new StreamReader(await response.Content.ReadAsStreamAsync()).ReadToEndAsync();
            }
        }

        public async Task<string> PostAsync(string url, dynamic param, Dictionary<string, string> headers)
        {
            HttpClient client = _httpClient;
            client.DefaultRequestHeaders.Clear();
            if (headers != null)
                foreach (KeyValuePair<string, string> entry in headers)
                    client.DefaultRequestHeaders.Add(entry.Key, entry.Value);
            HttpResponseMessage response = await client.PostAsync(url, PrepareContent(param));
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PutAsync(string url, dynamic param)
        {
            var client = _httpClient;
            client.DefaultRequestHeaders.Clear();
            HttpResponseMessage response = await client.PutAsync(url, PrepareContent(param));
            return await response.Content.ReadAsStringAsync();
        }

        private ByteArrayContent PrepareContent(dynamic param)
        {
            var content = JsonConvert.SerializeObject(param);
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent =
                new ByteArrayContent(buffer) { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } };
            return byteContent;
        }

        public Pagination ValidatePagination(Pagination param)
        {
            if (param.PageSize < 0 || param.PageSize > 100) param.PageSize = 10;
            if (param.Page <= 0) param.Page = 1;
            return param;
        }

        public static object GetElementsResultService(dynamic result)
        {
            object objResult = result;
            return result != null ? objResult.GetType().GetProperties()[0].GetValue(objResult) : new object();
        }

        public static int RandomNumber(int min, int max)
        {
            Random _random = new Random();
            return _random.Next(min, max);
        }
    }
}