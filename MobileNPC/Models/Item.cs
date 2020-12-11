using System;

namespace MobileNPC.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public string GTIN { get; set; }
        public string FunctionalName { get; set; }
        public string Manufacturer { get; set; }
        public Akeneo.Model.Product Product { get; set; }
    }
}