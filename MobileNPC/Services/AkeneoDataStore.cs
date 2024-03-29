﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akeneo;
using Akeneo.Client;
using Akeneo.Model;
using Akeneo.Search;
using Microsoft.AppCenter.Crashes;
using MobileNPC.Models;
using ServiceStack;
using SearchQueryBuilder = MobileNPC.Filtering.SearchQueryBuilder;
namespace MobileNPC.Services
{
    public class AkeneoDataStore : IDataStore<Item>
    {
        private readonly IAkeneoClient _client;
        private readonly IEnumerable<string> _categories;
        private readonly string _akeneoFamily;
        private readonly SearchQueryBuilder _searchQueryBuilder;
        public AkeneoDataStore()
        {
            string optionsJson = string.Empty;
            try
            {
                var options = new AkeneoOptions
                {
                    ApiEndpoint = new Uri(App.AkeneoConfig.AkeneoUrl),
                    ClientId = App.AkeneoConfig.ClientId,
                    ClientSecret = App.AkeneoConfig.ClientSecret,
                    UserName = App.AkeneoConfig.Username,
                    Password = App.AkeneoConfig.Password
                };
                optionsJson = options.ToJson();
                _client = new AkeneoClient(options);
                _searchQueryBuilder = new SearchQueryBuilder();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                var properties = new Dictionary<string, string> {
                    { "options", optionsJson},
                    { "ApiEndpoint", App.AkeneoConfig.AkeneoUrl }
                };
                Crashes.TrackError(ex, properties);
                throw ex;
            }
            
            _akeneoFamily = App.AkeneoConfig.Configuration.Family;
        }

        public Task<bool> AddItemAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        async public Task<Item> GetItemAsync(string id)
        {
            var product = await _client.GetAsync<Product>(id);
            return ToItem(product);
        }

        async public Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            var family = Akeneo.Search.Family.In(_akeneoFamily);
            var queryString = _searchQueryBuilder.GetQueryString(new List<Criteria> { family });
            queryString = $"{queryString}&page={1}&limit={100}&with_count={true.ToString().ToLower()}";
            var products = await _client.FilterAsync<Product>(queryString);
            return products.GetItems().Select(ToItem);
        }

        public Task<bool> UpdateItemAsync(Item item)
        {
            throw new NotImplementedException();
        }

        static Item ToItem(Product product)
        {
            List<Akeneo.Model.ProductValue> brandName, functionalName, manufacturer, image;
            if (product == null) return null;
            var item = new Item();
            item.Id = product.Identifier;
            item.BrandName = product.Values.TryGetValue(App.AkeneoConfig.Configuration.Attributes.BrandName, out brandName) ? brandName.FirstOrDefault()?.Data?.ToString() : "N/A";
            item.Text = item.BrandName;
            item.Description = product.Identifier;
            item.FunctionalName = product.Values.TryGetValue(App.AkeneoConfig.Configuration.Attributes.FunctionalName, out functionalName) ? functionalName.FirstOrDefault()?.Data?.ToString() : "N/A";
            item.Manufacturer = product.Values.TryGetValue(App.AkeneoConfig.Configuration.Attributes.Manufacturer, out manufacturer) ? manufacturer.FirstOrDefault()?.Data?.ToString() : "N/A";
            item.GTIN = product.Identifier;
            item.Image = "https://i.ibb.co/42zVPjq/unavailable-image.jpg";
            var hasImage = product.Values.TryGetValue(App.AkeneoConfig.Configuration.Attributes.Image, out image);
            if(hasImage)
            {
                var url = image.FirstOrDefault()?.Data?.ToString();
                if(url != null)
                {
                    var path = $"media/cache/preview/{url}";
                    var builder = new UriBuilder(App.AkeneoConfig.AkeneoUrl);
                    builder.Path = path;
                    item.Image = $"{builder.Uri}";
                }
            }
            return item;
        }
    }
}
