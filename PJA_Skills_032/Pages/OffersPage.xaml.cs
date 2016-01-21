using System;
using Windows.UI.Xaml.Controls;
using PJA_Skills_032.ViewModel;

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Offers : Page
    {
        public OffersViewModel ViewModel = new OffersViewModel();

        public Offers()
        {
            this.InitializeComponent();
        }
    }
}
