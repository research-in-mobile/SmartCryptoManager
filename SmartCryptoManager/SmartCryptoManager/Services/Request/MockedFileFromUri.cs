using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCryptoManager.Services
{
    public static class MockedFileFromUri
    {
        private const string _mockedFileLocation = "SmartCryptoManager.Services.Request.MockedResponses";

        public static Dictionary<string, string> UriFileDictionary = new Dictionary<string, string>()
        {
            {"//min-api.cryptocompare.com/data/top/mktcapfull", "topmktcapfull.json"} 
        };

        public static string GetFileName(string uri)
        {
            var uriNoParameter = uri.Split('?')[0];
            var key = uriNoParameter.Split(':')[1];

            var value = string.Empty;
            var HasValue = UriFileDictionary.TryGetValue(key, out value);

            return HasValue ? value : null;
        }

        public static string GetFileLocation(string uri)
        {
            return string.Join(".", _mockedFileLocation, GetFileName(uri));
        }
    }
}
