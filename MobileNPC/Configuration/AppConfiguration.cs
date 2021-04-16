namespace MobileNPC.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using Newtonsoft.Json;
    public class AppConfiguration
    {
        const string JsonBlobBaseUrl = "https://jsonblob.com";
        public ProductAttributes Attributes { get; set; }
        public string Family { get; set; }
        public List<Category> Categories { get; set; }

        public static AppConfiguration Create(string configurationUri)
        {
            var client = new HttpClient();
            var response = client.GetAsync(configurationUri).Result;
            response.EnsureSuccessStatusCode();
            var contentStream = response.Content.ReadAsStreamAsync().Result;
            using (var streamReader = new StreamReader(contentStream))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    var serializer = new JsonSerializer();
                    var appConfiguration = serializer.Deserialize<AppConfiguration>(jsonReader);
                    return appConfiguration;
                }
            }
        }
    }

    public class ProductAttributes
    {
        public string BrandName { get; set; }
        public string GTIN { get; set; }
        public string FunctionalName { get; set; }
        public string Manufacturer { get; set; }
        public string Image { get; set; }
    }

    public class Category
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
