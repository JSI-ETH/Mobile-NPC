using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileNPC.Models;
using MobileNPC.ViewModels;

namespace MobileNPC.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Item
            {
                Text = "Item 1",
                Description = "This is an item description."
            };

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }

        void OnButtonClicked(object sender, EventArgs args)
        {
            viewModel.Item.Current += 1;

            if (viewModel.Item.Current >= viewModel.Item.Source.Length)
            {
                viewModel.Item.Current = 0;
            }

            itemImage.Source = "";
            itemImage.Source = ImageSource
                    .FromUri(new Uri(viewModel.Item.Source[viewModel.Item.Current]));
        }
    }
}