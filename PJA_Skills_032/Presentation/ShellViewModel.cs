using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace PJA_Skills_032.Presentation
{
    public class ShellViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();
        private MenuItem selectedMenuItem;
        private bool isSplitViewPaneOpen;

        public ShellViewModel()
        {
            this.ToggleSplitViewPaneCommand = new Command(() => this.IsSplitViewPaneOpen = !this.IsSplitViewPaneOpen);
        }

        public ICommand ToggleSplitViewPaneCommand { get; private set; }

        public bool IsSplitViewPaneOpen
        {
            get { return this.isSplitViewPaneOpen; }
            set { Set(ref this.isSplitViewPaneOpen, value); }
        }

        public MenuItem SelectedMenuItem
        {
            get { return this.selectedMenuItem; }
            set
            {
                if (Set(ref this.selectedMenuItem, value)) {
                    OnPropertyChanged("SelectedPageType");

                    if (selectedMenuItem != null && selectedMenuItem.Title.Equals("Logout"))
                    {
                        //dissable all other buttons from hamburger
                        foreach (MenuItem menuItem in menuItems)
                        {
                            menuItem.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        //enable all btns
                        foreach (MenuItem menuItem in menuItems)
                        {
                            menuItem.Visibility = Visibility.Visible;
                        }
                    }

                    // auto-close split view pane
                    this.IsSplitViewPaneOpen = false;
                }
            }
        }

        public Type SelectedPageType
        {
            get
            {
                return this.selectedMenuItem?.PageType;
            }
            set
            {
                // select associated menu item
                this.SelectedMenuItem = this.menuItems.FirstOrDefault(m => m.PageType == value);
            }
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get { return this.menuItems; }
        }
    }
}
