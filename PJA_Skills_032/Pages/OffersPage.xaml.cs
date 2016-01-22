using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PJA_Skills_032.Model;
using PJA_Skills_032.ViewModel;

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Offers : Page
    {
        public OffersViewModel ViewModel { get; set; }

        public Offers()
        {
            this.InitializeComponent();
            ViewModel = new OffersViewModel();
        }


        private async void Offers_OnLoaded(object sender, RoutedEventArgs e)
        {
            // await download users of offers
            await ViewModel.AddDownloadedOffers();


        }

        private void AppBarButtonAddNewOffer_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OfferAddNewPage));
        }
    }
}
