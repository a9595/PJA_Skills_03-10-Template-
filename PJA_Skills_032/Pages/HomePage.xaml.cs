using PJA_Skills_032.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Parse;
using PJA_Skills_032.ParseObjects;
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
        public ObservableCollection<TestUser> SearchSuggestionList = new ObservableCollection<TestUser>();
        public ObservableCollection<TestUser> SearchResults = new ObservableCollection<TestUser>();


        #region page events

        public HomePage()
        {
            this.InitializeComponent();
            ViewModel = new HomeViewModel();

            AutoSuggestBoxSearch.ItemsSource = SearchResults;

        }

        private async void HomePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.AddDownloadedUsers(); // set downloaded users to viewModel

            // search
            List<TestUser> usersList = await ParseHelper.GetAllUsersList();
            SearchSuggestionList = new ObservableCollection<TestUser>(usersList);
        }
        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var selectedArticle = e.AddedItems;
            var selectedUser = GridViewUsers.SelectedItem;

            if (selectedUser == null) return;
            Frame.Navigate(typeof(UserPage), selectedUser);
        }

        private void AutoSuggestBoxSearch_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            List<TestUser> searchSuggestions =
                new List<TestUser>(SearchSuggestionList
                .Where(item =>
                {
                    return item.Name != null && item.Name.ToLower()
                        .Contains(args.QueryText.ToLower());
                }));

            SearchResults.Clear();
            foreach (TestUser user in searchSuggestions)
            {
                SearchResults.Add(user);
            }
        }


        private void AutoSuggestBoxSearch_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            TestUser selectedUser = args.SelectedItem as TestUser;
            if (selectedUser != null)
            {
                Frame.Navigate(typeof(UserPage), selectedUser);
            }
        }
        #endregion

        #region methods




        private async Task CreateBook()
        {

            // now we create a book object
            //var book = new ParseObject("Book");
            var billGatesQuery = ParseObject.GetQuery("TestUser");
            var billGatesUser = await billGatesQuery.GetAsync("AX1ADmYAMy"); // get bill gates object (TestUser)

            //ParseObject skillEnglish = await ParseObject.GetQuery(ParseHelper.OBJECT_SKILL).GetAsync("NampQ2zYFo");
            //ParseObject skillHTA = await ParseObject.GetQuery(ParseHelper.OBJECT_SKILL).GetAsync("mP0HQcXTIj");
            ParseObject skillWriting = await ParseObject.GetQuery(ParseHelper.OBJECT_SKILL).GetAsync("Y5Y7vXHbdz");

            // now let’s associate the authors with the book
            // remember, we created a "authors" relation on Book
            var billGatesSkillsWantToLearn = billGatesUser.GetRelation<ParseObject>("SkillsWantToLearn2");
            //relation.Add(skillEnglish);
            //relation.Add(skillHTA);
            billGatesSkillsWantToLearn.Add(skillWriting);
            //relation.Add(authorThree;)

            // now save the book object
            await billGatesUser.SaveAsync();
        }

        private static async Task CreatePost()
        {
            //// Create the post
            //var myPost = new ParseObject("Post")
            //    {
            //        { "title", "I'm Hungry" },
            //        { "content", "Where should we go for lunch?" }
            //    };

            // Create the comment


            var myComment = new ParseObject("Comment")
            {
                {"content2", "Let's do Sushirrito.2"}
            };

            // Add a relation between the Post and Comment
            myComment["parent"] = ParseObject.CreateWithoutData("Post", "2dNK5eAIRG");

            // This will save both myPost and myComment
            await myComment.SaveAsync();

        }


        private async Task GetAnia()
        {
            // Get Ania Object:
            var query = ParseObject.GetQuery(ParseHelper.OBJECT_TEST_USER);

            ParseObject aniaParseObject = await query.GetAsync("W6kqOxRH85");
            TestUser aniaUserObject = new TestUser(aniaParseObject);



            //// Create a skill:
            //var aniaSkillEnglish = new ParseObject(ParseHelper.OBJECT_SKILL)
            //{
            //    { ParseHelper.OBJECT_SKILL_NAME, "English" }
            //};
            //var aniaSkillHTA = new ParseObject(ParseHelper.OBJECT_SKILL)
            //{
            //    { ParseHelper.OBJECT_SKILL_NAME, "HTA" }
            //};

            // Get skills:
            //ParseObject skillHTA = await ParseObject.GetQuery(ParseHelper.OBJECT_SKILL).WhereEqualTo(ParseHelper.OBJECT_ID, "mP0HQcXTIj").FirstAsync();
            //ParseObject skillEnglish = await ParseObject.GetQuery(ParseHelper.OBJECT_SKILL).WhereEqualTo(ParseHelper.OBJECT_ID, "NampQ2zYFo").FirstAsync();
            ParseObject skillEnglish = await ParseObject.GetQuery(ParseHelper.OBJECT_SKILL).GetAsync("NampQ2zYFo");
            ParseObject skillHTA = await ParseObject.GetQuery(ParseHelper.OBJECT_SKILL).GetAsync("mP0HQcXTIj");

            // Get SkillWantToLearn realation:
            ParseRelation<ParseObject> relation = aniaParseObject.GetRelation<ParseObject>("SkillsWantToLearn2");
            IEnumerable<ParseObject> aniaSkills = await relation.Query.FindAsync();

            //// Save skills to parse:
            //await skillEnglish.SaveAsync();
            //await skillHTA.SaveAsync();

            // Add skills to Ania:
            relation.Add(skillEnglish);
            relation.Add(skillHTA);

            // Save Ania
            //await aniaParseObject.FetchAsync();
            await aniaParseObject.SaveAsync();

        }



        #endregion

        private void AutoSuggestBoxSearch_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (SearchSuggestionList != null)
            {
                List<TestUser> searchSuggestions =
                    new List<TestUser>(SearchSuggestionList
                        .Where(item =>
                        {
                            return item.Name != null && item.Name.ToLower()
                                .Contains(AutoSuggestBoxSearch.Text.ToLower());
                        }));

                SearchResults.Clear();
                foreach (TestUser user in searchSuggestions)
                {
                    SearchResults.Add(user);
                }
            }
        }
    }
}
