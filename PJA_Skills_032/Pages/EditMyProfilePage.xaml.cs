using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PJA_Skills_032.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditMyProfilePage : Page
    {
        public MyProfileViewModel ViewModel { get; set; }
        public EditMyProfilePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //MyProfileViewModel myProfileViewModel = e.Parameter as MyProfileViewModel;
            ViewModel = new MyProfileViewModel();

            base.OnNavigatedTo(e);
        }

        private async void EditMyProfilePage_OnLoading(FrameworkElement sender, object args)
        {
            // Bind viemodel to view 

            await ViewModel.CurrentUser.GetSkills();

            //this.DataContext = ViewModel;
            //GridViewLearn.ItemsSource = 
        }
    }
}
