using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CodingExercise.Tests.Api
{
    public static class ClientHelper
    {
        public static async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(jsonString);

            return result;
        }

        public static StringContent SerializeObject(object value)
        {
            try
            {
                var objectAsJsonString = JsonConvert.SerializeObject(value);
                var result = new StringContent(objectAsJsonString, Encoding.UTF8, "application/json");
                return result;
            }
            catch (Exception ex)
            {
                var x = ex;
                return new StringContent("");
            }
        }
    }
}
