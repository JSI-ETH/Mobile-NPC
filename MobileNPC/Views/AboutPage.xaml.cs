using System;
using System.ComponentModel;
using MobileNPC.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace MobileNPC.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ZXingScannerPage
    {
        private readonly IDataStore<Models.Item> _dataStore = DependencyService.Get<IDataStore<Models.Item>>();
        public AboutPage()
        {
            InitializeComponent();
        }
        public async void ZXingScannerPage_OnScanResult(ZXing.Result result)
        {
            IsScanning = false;
            var gtin = result.Text;
            var selectedItem = await _dataStore.GetItemAsync(gtin);
            if (selectedItem != null)
                await Navigation.PushModalAsync(new NavigationPage(new ItemDetailPage(new ViewModels.ItemDetailViewModel(selectedItem))));
            else
                await DisplayAlert("Sorry!", $"The item with the specified GTIN `{result.Text}` could not be found. Please try again!", "OK");
            IsScanning = true;
        }
        
        protected override bool OnBackButtonPressed()
        {
            IsScanning = true;
            return base.OnBackButtonPressed();
        }

    }

}