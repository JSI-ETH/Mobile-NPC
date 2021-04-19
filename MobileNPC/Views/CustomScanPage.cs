using System;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using MobileNPC.Services;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace MobileNPC.Views
{
	public class CustomScanPage : ContentPage
	{
		ZXingScannerView zxing;
		ZXingDefaultOverlay overlay;

		private readonly IDataStore<Models.Item> _dataStore = DependencyService.Get<IDataStore<Models.Item>>();

		public CustomScanPage() : base()
		{
			zxing = new ZXingScannerView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				AutomationId = "zxingScannerView"
			};
			zxing.OnScanResult += (result) =>
				Device.BeginInvokeOnMainThread(async () =>
				{

					// Stop analysis until we navigate away so we don't keep reading barcodes
					zxing.IsAnalyzing = false;

					// Show an alert
					await DisplayAlert("Scanned Barcode", result.Text, "OK");
					var selectedItem = await _dataStore.GetItemAsync(result.Text);

					if (selectedItem != null)
						await Navigation.PushAsync(new ItemDetailPage(new ViewModels.ItemDetailViewModel(selectedItem)));
					else
                    {
						await Navigation.PushAsync(new ItemDetailPage(new ViewModels.ItemDetailViewModel(new Models.Item
						{
							Text = "Test Item",
							BrandName = "Pfizer",
							GTIN = result.Text
						})));
					}
						
					
				});

			overlay = new ZXingDefaultOverlay
			{
				TopText = "Hold your phone up to the barcode",
				BottomText = "Scanning will happen automatically",
				ShowFlashButton = zxing.HasTorch,
				AutomationId = "zxingDefaultOverlay",
			};
			overlay.FlashButtonClicked += (sender, e) =>
			{
				zxing.IsTorchOn = !zxing.IsTorchOn;
			};
			var grid = new Grid
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			grid.Children.Add(zxing);
			grid.Children.Add(overlay);

			// The root page of your application
			Content = grid;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			zxing.IsScanning = true;
		}

		protected override void OnDisappearing()
		{
			zxing.IsScanning = false;

			base.OnDisappearing();
		}

		async Task ProcessScanAsync(string gtin)
		{
			try
			{
				var selectedItem = await _dataStore.GetItemAsync(gtin);
				if (selectedItem != null)
					await Navigation.PushAsync(new ItemDetailPage(new ViewModels.ItemDetailViewModel(selectedItem)));
				else
					await Navigation.PushAsync(new ItemDetailPage(new ViewModels.ItemDetailViewModel(new Models.Item
					{
						Text = "Test Item",
						BrandName = "Pfizer",
						GTIN = gtin
					})));
				//await DisplayAlert("Sorry!", $"The item with the specified GTIN `{result.Text}` could not be found. Please try again!", "OK");
			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
				await DisplayAlert("Sorry!", $"Could not connect to the NPC. Please try again!", "OK");
			}
		}
	}
}
