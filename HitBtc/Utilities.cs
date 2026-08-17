using System;
using System.Collections.Generic;
using Hitbtc.HitBtcModel;
using Newtonsoft.Json;

namespace Hitbtc
{
    internal static class Utilities
    {
        public static string FirstCharToLower(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("input parameter cannot be empty");
            return char.ToLowerInvariant(input[0]) + input.Substring(1);
        }

        public static T ConvertFromJson<T>(ApiResponse response) where T : class
        {
            EnsureResponseContent(response);
            var result = JsonConvert.DeserializeObject<T>(response.Content);
            if (result == null)
                throw new JsonSerializationException("The API response JSON contained no value.");
            return result;
        }

        public static List<T> ConvertListFromJson<T>(ApiResponse response) where T : class
        {
            EnsureResponseContent(response);
            var result = JsonConvert.DeserializeObject<List<T>>(response.Content);
            if (result == null)
                throw new JsonSerializationException("The API response JSON contained no collection.");
            return result;
        }

        public static Dictionary<string, Ticker> ConvertTickerDictionaryFromJson(ApiResponse response)
        {
            EnsureResponseContent(response);
            var result = JsonConvert.DeserializeObject<Dictionary<string, Ticker>>(response.Content);
            if (result == null)
                throw new JsonSerializationException("The API response JSON contained no ticker dictionary.");
            return result;
        }

        private static void EnsureResponseContent(ApiResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));
            if (string.IsNullOrWhiteSpace(response.Content))
                throw new JsonSerializationException("The API response body was empty.");
        }
    }
}
