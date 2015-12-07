using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using PJA_Skills_032.Pages;
using PJA_Skills_032.Presentation;

namespace PJA_Skills_032
{
    public sealed partial class Shell : UserControl
    {
        public Shell()
        {
            this.InitializeComponent();

            var vm = new ShellViewModel();
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "My Profile", PageType = typeof(MyProfilePage) });
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Home", PageType = typeof(HomePage) });
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Offers", PageType = typeof(Offers) });
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Search", PageType = typeof(SearchPage) });
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Settings", PageType = typeof(SettingsPage) });
            //
            // select the first menu item
            vm.SelectedMenuItem = vm.MenuItems.First();

            this.ViewModel = vm;
        }

        public ShellViewModel ViewModel { get; private set; }

        public Frame RootFrame
        {
            get
            {
                return this.Frame;
            }
        }
    }
}
