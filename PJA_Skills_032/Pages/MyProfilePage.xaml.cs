using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Parse;
using PJA_Skills_032.Model;

namespace PJA_Skills_032.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyProfilePage : Page
    {
        public MyProfilePage()
        {
            this.InitializeComponent();

            Contacts.GetAllContacts();
        }

        private async Task GetGameScore()
        {
            ParseQuery<ParseObject> query = ParseObject.GetQuery("GameScore");
            ParseObject gameScore = await query.GetAsync("6VzYvXc73i");

            int score = gameScore.Get<int>("score");
            string playerName = gameScore.Get<string>("playerName");
            //bool cheatMode = gameScore.Get<bool>("cheatMode");

            UserName.Text = playerName;
            UserFaculty.Text = score.ToString();
        }

        private async void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // await AddParseObject();
            //await GetGameScore();
        }

        private static async Task AddParseObject()
        {
            ParseObject gameScore = new ParseObject("GameScore");
            gameScore["score"] = 1337;

            gameScore["playerName"] = "Sean Plott";
            await gameScore.SaveAsync();
        }
    }
}
