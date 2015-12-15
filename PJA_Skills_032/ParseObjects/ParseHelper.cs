using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace PJA_Skills_032.ParseObjects
{
    class ParseHelper
    {
        public static readonly string OBJECT_TEST_USER = "TestUser";


        private async Task GetGameScore()
        {
            ParseQuery<ParseObject> query = ParseObject.GetQuery("GameScore");
            ParseObject gameScore = await query.GetAsync("6VzYvXc73i");

            int score = gameScore.Get<int>("score");
            string playerName = gameScore.Get<string>("playerName");
            //bool cheatMode = gameScore.Get<bool>("cheatMode");
        }
    }
}
