using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileNPC.Models;

namespace MobileNPC.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        String[] source = {
                "https://www.gs1uk.org/sites/default/files/2018-09/gs1_uk_healthcare_for_suppliers_eprocurement_widget_img_1.jpg",
                "https://www.gs1uk.org/sites/default/files/blocks/promo/2020-08/hospital-4904921_960_720%20%28002%29400x200_0.jpg"
        };
        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description.", Source=source  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description.", Source=source  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description.", Source=source  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description.", Source=source  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description.", Source=source  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description.", Source=source  }
            };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}