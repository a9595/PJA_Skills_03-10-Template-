using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Parse;
using PJA_Skills_032.Model;
using PJA_Skills_032.ParseObjects;
using PJA_Skills_032.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PJA_Skills_032.Pages
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditMyProfilePage : Page
    {
        public class SearchSuggestion
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public SearchSuggestion(string name, int id)
            {
                Id = id;
                Name = name;
            }

            //public override string ToString()
            //{
            //    return Name;

            //}
        }

        public ObservableCollection<SearchSuggestion> FirstSearchSuggestions = new ObservableCollection<SearchSuggestion>();
        public ObservableCollection<SearchSuggestion> ResultsSearchSuggestions = new ObservableCollection<SearchSuggestion>();

        public MyProfileViewModel ViewModel { get; set; }
        public EditMyProfilePage()
        {
            this.InitializeComponent();

            PopulateSearchSuggestions();

            AutoSuggestBox.ItemsSource = ResultsSearchSuggestions;
        }

        public void PopulateSearchSuggestions()
        {
            FirstSearchSuggestions.Add(
                new SearchSuggestion("PRM", 1)
                );

            FirstSearchSuggestions.Add(
                new SearchSuggestion("PRI", 2)
                );

            FirstSearchSuggestions.Add(
                new SearchSuggestion("BSI", 3)
                );
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //MyProfileViewModel myProfileViewModel = e.Parameter as MyProfileViewModel;
            ViewModel = new MyProfileViewModel();

            base.OnNavigatedTo(e);
        }

        private async void EditMyProfilePage_OnLoading(FrameworkElement sender, object args)
        {
            await ViewModel.CurrentUser.GetSkills();
        }

        private void TextBoxAddWantToLearn_OnTextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ButtonAddWantToLearn_OnClick(object sender, RoutedEventArgs e)
        {
            //string addedSkillName = TextBoxAddWantToLearn.Text;
            //if (string.IsNullOrWhiteSpace(addedSkillName))
            //{
            //    Skill addedSkill = new Skill(addedSkillName);
            //    ViewModel.CurrentUser.SkillsWantToLearn.Add(addedSkill);


            //    ParseObject addedSkillParseObject = new ParseObject(ParseHelper.OBJECT_SKILL);

            //    // Add skill to user
            //    addedSkillParseObject.Add(ParseHelper.OBJECT_SKILL_NAME, addedSkillName);

            //    var skillUsersRelation = addedSkillParseObject.GetRelation<ParseObject>(ParseHelper.OBJECT_SKILL_USERS);
            //    skillUsersRelation.Add(ViewModel.CurrentUser.BackingObject); // add current user to relation
            //    addedSkillParseObject.SaveAsync();

            //}
        }

        private void GridViewLearn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        private void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            IEnumerable<SearchSuggestion> searchSuggestions =
                FirstSearchSuggestions.Where(item => (item.Name).ToLower().Contains(args.QueryText.ToLower()));

            ResultsSearchSuggestions.Clear();
            foreach (SearchSuggestion suggestion in searchSuggestions)
            {
                ResultsSearchSuggestions.Add(suggestion);
            }
        }

        private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SearchSuggestion selectedSearchSuggestion = args.SelectedItem as SearchSuggestion;
            if (selectedSearchSuggestion != null)
                txtInfo.Text = selectedSearchSuggestion.Id + "  " + selectedSearchSuggestion.Name;
        }
    }
}
