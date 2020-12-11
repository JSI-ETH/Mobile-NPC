﻿using System;
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
        public void ZXingScannerPage_OnScanResult(ZXing.Result result)
        {
            IsScanning = !IsScanning;

            Device.BeginInvokeOnMainThread(async () =>
            {
                var gtin = result.Text;
                var selectedItem = await _dataStore.GetItemAsync(gtin);
                if(selectedItem != null)
                {
                    //MessagingCenter.Send(this, "ItemScanned", selectedItem);
                    await Navigation.PushAsync(new ItemDetailPage(new ViewModels.ItemDetailViewModel(selectedItem)));
                    //await Navigation.
                    //await Navigation.PushAsync(new ItemsPage(new ViewModels.ItemDetailViewModel(selectedItem)));
                }
                    
                else
                    await DisplayAlert("Sorry!",$"The item with the specified GTIN `{result.Text}` could not be found. Please try again!", "OK");
               
            });
            IsScanning = !IsScanning;
        }

        
        protected override bool OnBackButtonPressed()
        {
            IsScanning = true;
            return base.OnBackButtonPressed();
        }

    }

}