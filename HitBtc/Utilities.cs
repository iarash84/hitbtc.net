using System;
using System.Collections.Generic;
using System.Linq;
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
            return input.First().ToString().ToLower() + input.Substring(1);
        }

        public static T ConverFromJason<T>(ApiResponse response) where T : class, new()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception)
            {
                return new T();
            }
        }

        public static List<T> ConverFromJasons<T>(ApiResponse response) where T : class, new()
        {
            try
            {
                return JsonConvert.DeserializeObject<List<T>>(response.Content);
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        public static Dictionary<string, Ticker> ConverFromJasonArray(ApiResponse response)
        {
            Dictionary<string, Ticker> result = new Dictionary<string, Ticker>();
            try
            {
                int length = response.Content.Trim().Length;
                var tickers = response.Content.Trim()
                    .Remove(length - 1)
                    .Remove(0, 1)
                    .Split(new[] { "}," }, StringSplitOptions.None).Select(x => x[x.Length - 1] != '}' ? x + "}" : x).ToList();

                foreach (var ticker in tickers)
                {
                    int index = ticker.IndexOf(':');
                    string name = ticker.Remove(index, ticker.Length - index);
                    string data = ticker.Remove(0, index + 1);
                    result.Add(name, JsonConvert.DeserializeObject<Ticker>(data));
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return result;
        }
    }
}
