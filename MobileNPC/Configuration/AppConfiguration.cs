namespace MobileNPC.Configuration
{
    using System;
    using System.Collections.Generic;
    using ServiceStack;

    public class AppConfiguration
    {
        const string JsonBlobBaseUrl = "https://jsonblob.com";
        public ProductAttributes Attributes { get; set; }
        public string Family { get; set; }
        public List<Category> Categories { get; set; }

        public static AppConfiguration Create(string configurationUri)
        {
            var client = new JsonServiceClient();
            var appConfiguration = client.Get<AppConfiguration>(configurationUri);
            return appConfiguration;
        }
    }

    public class ProductAttributes
    {
        public string BrandName { get; set; }
        public string GTIN { get; set; }
        public string FunctionalName { get; set; }
        public string Manufacturer { get; set; }
    }

    public class Category
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
