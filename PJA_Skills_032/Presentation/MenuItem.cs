using System;
using Windows.UI.Xaml;

namespace PJA_Skills_032.Presentation
{
    public class MenuItem : NotifyPropertyChanged
    {
        private string icon;
        private string title;
        private Type pageType;
        private Visibility visibility;


        public string Icon
        {
            get { return this.icon; }
            set { Set(ref this.icon, value); }
        }

        public string Title
        {
            get { return this.title; }
            set { Set(ref this.title, value); }
        }

        public Type PageType
        {
            get { return this.pageType; }
            set { Set(ref this.pageType, value); }
        }
        public Visibility Visibility
        {
            get { return this.visibility; }
            set { Set(ref this.visibility, value); }
        }
    }
}
