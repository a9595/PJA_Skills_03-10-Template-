﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignupPage : Page
    {
        // User fields:
        public ObservableCollection<Skill> UserLearnList = new ObservableCollection<Skill>();
        public ObservableCollection<Skill> UserTeachList = new ObservableCollection<Skill>();
        public ObservableCollection<Skill> UserKorkingList = new ObservableCollection<Skill>();


        public ObservableCollection<Skill> SearchSuggestionsList = new ObservableCollection<Skill>();
        public ObservableCollection<Skill> ResultsSearchSuggestions = new ObservableCollection<Skill>();
        private ParseUser _user = new ParseUser();
        private StorageFile _avatarFile;
        private byte[] _avatarBytesArray;
        private ParseFile _avatarParseFile;

        public SignupPage()
        {
            this.InitializeComponent();

            AutoSuggestBoxLearn.ItemsSource = ResultsSearchSuggestions;
            AutoSuggestBoxTeach.ItemsSource = ResultsSearchSuggestions;
            AutoSuggestBoxKorking.ItemsSource = ResultsSearchSuggestions;

            GridViewLearn.ItemsSource = UserLearnList;
            GridViewTeach.ItemsSource = UserTeachList;
            GridViewKorking.ItemsSource = UserKorkingList;
        }

        private async void SignupPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            List<Skill> allSkillsList = await ParseHelper.GetAllSkillsList();
            SearchSuggestionsList = new ObservableCollection<Skill>(allSkillsList);
        }

        private async void ButtonSignUp_OnClick(object sender, RoutedEventArgs e)
        {
            _user.Username = TxtEmail.Text;
            _user.Password = TxtPassword.Password;
            _user.Email = TxtEmail.Text;


            // other fields can be set just like with ParseObject
            _user[ParseHelper.OBJECT_TEST_USER_NAME] = TxtFullName.Text;

            // Social links:
            _user[ParseHelper.OBJECT_TEST_USER_FACEBOOK] = TxtFacebook.Text;
            _user[ParseHelper.OBJECT_TEST_USER_GOOGLE_PLUS] = TxtGooglePlus.Text;
            _user[ParseHelper.OBJECT_TEST_USER_SKYPE] = TxtSkype.Text;

            // Set faculty
            List<RadioButton> radioList = new List<RadioButton>
            {
                RadioFacultyArts,
                RadioFacultyInformatics,
                RadioFacultyInterior,
                RadioFacultyJapan,
                RadioFacultyManagement
            };
            RadioButton buttons = radioList
                           .FirstOrDefault(n => n.IsChecked != null && (bool)n.IsChecked);
            if (buttons != null)
                _user[ParseHelper.OBJECT_TEST_USER_FACULTY] = buttons.Content?.ToString();

            // Avatar
            if (_avatarBytesArray != null && _avatarFile != null)
            {
                // save to Parse
                _avatarParseFile = new ParseFile("avatar" + _avatarFile.FileType, _avatarBytesArray);
                await _avatarParseFile.SaveAsync();
                _user[ParseHelper.OBJECT_TEST_USER_AVATAR] = _avatarParseFile;
            }


            // Sign up
            try
            {
                await _user.SignUpAsync();
            }
            catch (Exception exception)
            {
                string ex = exception.ToString();
                var dialog = new MessageDialog("Please, fill all forms or try another email");
                await dialog.ShowAsync();
                return;

            }


            // Login
            try
            {
                await ParseUser.LogInAsync(TxtEmail.Text, TxtPassword.Password);
                // Login was successful.
            }
            catch (Exception)
            {
                var dialog = new MessageDialog("Please, try another email");
                await dialog.ShowAsync();
                return;
            }

            // Add skill to created user

            await AddSkillsToUser();

            Frame.Navigate(typeof(MyProfilePage));
        }

        public async Task AddSkillsToUser()
        {
            // Add learn skills
            foreach (Skill skill in UserLearnList)
            {
                await ParseHelper.AddSkillToUser(_user, skill.getBackingObject);
            }

            // Add teach skills
            foreach (Skill skill in UserTeachList)
            {
                await ParseHelper.AddSkillTeachToUser(_user, skill.getBackingObject);
            }

            // Add korking skills
            foreach (Skill skill in UserKorkingList)
            {
                await ParseHelper.AddSkillKorkingToUser(_user, skill.getBackingObject);
            }
        }


        // Learn
        private void AutoSuggestBoxLearn_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AutoSuggestionBoxTextChanged(AutoSuggestBoxLearn, UserLearnList);
        }
        private void AutoSuggestBoxLearn_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            AutoSuggestionChosen(args, UserLearnList, AutoSuggestBoxLearn);
        }


        // Teach
        private void AutoSuggestBoxTeach_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AutoSuggestionBoxTextChanged(AutoSuggestBoxTeach, UserTeachList);
        }
        private void AutoSuggestBoxTeach_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            AutoSuggestionChosen(args, UserTeachList, AutoSuggestBoxTeach);
        }

        // Korking
        private void AutoSuggestBoxKorking_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AutoSuggestionBoxTextChanged(AutoSuggestBoxKorking, UserKorkingList);
        }
        private void AutoSuggestBoxKorking_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            AutoSuggestionChosen(args, UserKorkingList, AutoSuggestBoxKorking);
        }

        private void AutoSuggestionBoxTextChanged(AutoSuggestBox autoSuggestBox, ObservableCollection<Skill> userSkillsList)
        {
            List<Skill> searchSuggestions =
                new List<Skill>(SearchSuggestionsList.
                    Where(item => (item.Name).ToLower().Contains
                        (autoSuggestBox.Text.ToLower())));

            ResultsSearchSuggestions.Clear();

            // Remove already added by user (to avoid dublicates)
            foreach (Skill skill in searchSuggestions.ToList())
            {
                if (skill.IsContainsInOtherList(userSkillsList))
                    searchSuggestions.Remove(skill);
            }

            // Add all matched skills
            foreach (Skill skill in searchSuggestions)
            {
                ResultsSearchSuggestions.Add(skill);
            }
        }

        private void AutoSuggestionChosen(AutoSuggestBoxSuggestionChosenEventArgs args, ObservableCollection<Skill> userLearnList, AutoSuggestBox autoSuggestBox)
        {
            Skill selectedSkill = args.SelectedItem as Skill;
            if (selectedSkill != null)
            {
                userLearnList.Add(selectedSkill);
            }

            autoSuggestBox.Text = "";
        }

        private async void BtnChangeAvatar_OnClick(object sender, RoutedEventArgs e)
        {
            // Set up the file picker.
            Windows.Storage.Pickers.FileOpenPicker openPicker =
                new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            openPicker.ViewMode =
                Windows.Storage.Pickers.PickerViewMode.Thumbnail;

            // Filter to include a sample subset of file types.
            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".jpg");

            // Open the file picker.
            _avatarFile = await openPicker.PickSingleFileAsync();

            // 'file' is null if user cancels the file picker.
            if (_avatarFile != null)
            {
                // Open a stream for the selected file.
                // The 'using' block ensures the stream is disposed
                // after the image is loaded.
                using (Windows.Storage.Streams.IRandomAccessStream fileStream =
                    await _avatarFile.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap.
                    Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage =
                        new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                    bitmapImage.SetSource(fileStream);
                    UserImage.Source = bitmapImage;

                    // stream to []byte
                    DataReader reader = new DataReader(fileStream.GetInputStreamAt(0));
                    _avatarBytesArray = new byte[fileStream.Size];
                    await reader.LoadAsync((uint)fileStream.Size);
                    reader.ReadBytes(_avatarBytesArray);

                }
            }
        }
    }
}
