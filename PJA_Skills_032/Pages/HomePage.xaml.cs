using PJA_Skills_032.Model;
using System;
using Windows.UI.Xaml.Controls;

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();


            ContactsCVS.Source = Contact.GetContactsGrouped(30);

        }
    }
}
