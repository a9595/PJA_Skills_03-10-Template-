using PJA_Skills_032.Model;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PJA_Skills_032.ViewModel;

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        //private ObservableCollection<GroupInfoList> source;
        public HomeViewModel ViewModel { get; set; }
        public HomePage()
        {
            this.InitializeComponent();
            ViewModel = new HomeViewModel();



        }

        private async void HomePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.AddDownloadedUsers();
            ObservableCollection<TestUser> observableCollection = ViewModel.Users;
            foreach (var testUser in observableCollection)
            {
                var TestUser = testUser;
                string name = TestUser.Name;
            }
        }
    }
}
