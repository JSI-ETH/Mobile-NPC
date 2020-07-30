using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akeneo;
using Akeneo.Client;
using Akeneo.Model;
using Akeneo.Search;
using MobileNPC.Models;

namespace MobileNPC.Services
{
    public class AkeneoDataStore : IDataStore<Item>
    {
        private readonly IAkeneoClient _client;

        public AkeneoDataStore()
        {
            var options = new AkeneoOptions
            {
                ApiEndpoint = new Uri(App.AkeneoConfig.AkeneoUrl),
                ClientId = App.AkeneoConfig.ClientId,
                ClientSecret = App.AkeneoConfig.ClientSecret,
                UserName = App.AkeneoConfig.Username,
                Password = App.AkeneoConfig.Password
            };
            _client = new AkeneoClient(options);
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
            var products = await _client.SearchAsync<Product>(new List<Criteria>
            {
                // TODO: Remove this (limited to FamilyPlanning category)
                Akeneo.Search.Category.In("FP"),
            });
           return products.GetItems().Select(ToItem);
        }

        public Task<bool> UpdateItemAsync(Item item)
        {
            throw new NotImplementedException();
        }

        static Item ToItem(Product product)
        {
            return product == null ? null : new Item
            {
                Id = product.Identifier,
                Text = product.Values[Constants.ProductAttributes.BrandName]?.FirstOrDefault()?.Data?.ToString(),
                Description = product.Identifier,
                Product = product
            };
        }
    }
}
