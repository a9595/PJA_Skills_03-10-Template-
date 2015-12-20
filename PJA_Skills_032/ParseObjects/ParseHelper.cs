using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace PJA_Skills_032.ParseObjects
{
    class ParseHelper
    {

        #region filed

        //USER:
        public static readonly string OBJECT_TEST_USER = "TestUser";
        public static readonly string OBJECT_TEST_USER_NAME = "Name";
        public static readonly string OBJECT_TEST_USER_FACULTY = "Faculty";
        public static readonly string OBJECT_TEST_USER_SKILLS_WANT_TO_LEARN = "SkillsWantToLearn2";


        //SKILL:
        public static readonly string OBJECT_SKILL_NAME = "Name";
        public static readonly string OBJECT_SKILL = "Skill";

        // General:
        public static readonly string OBJECT_ID = "ObjectId";

        // Default dummy data
        public static readonly string DEFAULT_LOGIN = "tieorange";
        public static readonly string DEFAULT_PASSWORD = "admin";

        #endregion

        #region method
        public static string GetParseObject(string parseObjectKey, ParseObject backingObject)
        {
            return backingObject != null
                    && backingObject.ContainsKey(parseObjectKey)
                    ? backingObject.Get<string>(parseObjectKey)
                    : null;
        }
        public static void SetParseObject(string parseObjectKey, ParseObject backingObject, string value)
        {
            if (backingObject != null
                   && backingObject.ContainsKey(parseObjectKey))
                backingObject[parseObjectKey] = value;
        }

        public static async Task CreateDummyUsers()
        {
            // let’s say we have a few objects representing User objects
            ParseObject userTieorange = await (from user in ParseUser.Query
                                               where user.Username.Equals("tieorange")
                                               select user).FirstAsync();
            ParseObject userTieorange2 = await (from user in ParseUser.Query
                                                where user.Username.Equals("tieorange2")
                                                select user).FirstAsync();




            // now we create a Skill object
            string skillWritingId = "Y5Y7vXHbdz";
            ParseObject skill = await ParseObject.GetQuery(ParseHelper.OBJECT_SKILL).GetAsync(skillWritingId);

            // now let’s associate the authors with the book
            // remember, we created a "authors" relation on Book
            ParseRelation<ParseObject> relation = skill.GetRelation<ParseObject>("Users");
            relation.Add(userTieorange);
            relation.Add(userTieorange2);

            // now save the book object
            await skill.SaveAsync();
        }


        /// <summary>
        /// We have "writing skill". Gets Users who have this skill
        /// </summary>
        /// <returns></returns>
        public static async Task GetUsersWithSkill()
        {
            // suppose we have a skill object
            string skillWritingId = "Y5Y7vXHbdz";
            ParseObject skill = await ParseObject.GetQuery(ParseHelper.OBJECT_SKILL).GetAsync(skillWritingId);

            // create a relation based on the users key
            var relation = skill.GetRelation<ParseObject>("Users");

            // generate a query based on that relation
            var queryUsers = relation.Query;

            IEnumerable<ParseObject> usersList = await queryUsers.FindAsync();
            foreach (ParseObject user in usersList)
            {
                string userName = user.Get<string>("Name");
            }
        }

        /// <summary>
        /// We have Bill Gates. Gets his skills
        /// </summary>
        /// <returns></returns>
        public static async Task GetSkillsOfUser()
        {

            ParseObject userTieorange = await (from user in ParseUser.Query
                                               where user.Username.Equals("tieorange")
                                               select user).FirstAsync();
            ParseObject userTieorange2 = await (from user in ParseUser.Query
                                                where user.Username.Equals("tieorange2")
                                                select user).FirstAsync();

            // first we will create a query on the Skill object
            var querySkillTable = ParseObject.GetQuery("Skill");

            // now we will query the authors relation to see if the author object we have
            // is contained therein
            var queryTieorange = querySkillTable.WhereEqualTo("Users", userTieorange);
            var queryTieorange2 = querySkillTable.WhereEqualTo("Users", userTieorange2);

            IEnumerable<ParseObject> skillsOfTieorange = await queryTieorange.FindAsync();
            foreach (ParseObject skill in skillsOfTieorange)
            {
                string skillName = skill.Get<string>("Name");
                Debug.WriteLine("tieorange skill = ", skillName);
            }

            IEnumerable<ParseObject> skillsOfTieorange2 = await queryTieorange2.FindAsync();
            foreach (ParseObject skill in skillsOfTieorange2)
            {
                string skillName = skill.Get<string>("Name");
                Debug.WriteLine("tieorange2 skill = ", skillName);

            }

        }
        #endregion
    }
}
