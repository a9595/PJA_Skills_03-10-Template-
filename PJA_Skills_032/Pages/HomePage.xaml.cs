using PJA_Skills_032.Model;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private ObservableCollection<GroupInfoList> source;
        public HomePage()
        {
            this.InitializeComponent();


            //ContactsCVS.Source = Contact.GetContactsGrouped(30);
            ContactsCVS.Source = source;

        }

        private async void HomePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            source = Contacts.GetContactsGrouped();
            var originalSource = Contact.GetContactsGrouped(2);
            int i = 0;
        }
    }
}
