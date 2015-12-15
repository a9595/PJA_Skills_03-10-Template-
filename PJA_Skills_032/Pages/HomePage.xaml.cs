using PJA_Skills_032.Model;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Parse;
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

        private static async System.Threading.Tasks.Task CreatePost()
        {
            // Create the post
            var myPost = new ParseObject("Post")
                {
                    { "title", "I'm Hungry" },
                    { "content", "Where should we go for lunch?" }
                };

            // Create the comment
            var myComment = new ParseObject("Comment")
                {
                    { "content", "Let's do Sushirrito." }
                };

            // Add a relation between the Post and Comment
            myComment["parent"] = myPost;

            // This will save both myPost and myComment
            await myComment.SaveAsync();
        }

        private async void HomePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.AddDownloadedUsers(); // set downloaded users to viewModel

            await CreatePost();
        }
    }
}
