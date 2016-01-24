using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using PJA_Skills_032.Model;

namespace PJA_Skills_032.ParseObjects
{
    class ParseHelper
    {

        #region filed

        //USER:
        public static readonly string OBJECT_TEST_USER = "User";
        public static readonly string OBJECT_TEST_USER_NAME = "Name";
        public static readonly string OBJECT_TEST_USER_FACULTY = "Faculty";
        public static readonly string OBJECT_TEST_USER_AVATAR = "Avatar";
        public static readonly string OBJECT_TEST_USER_FACEBOOK = "FacebookLink";
        public static readonly string OBJECT_TEST_USER_GOOGLE_PLUS = "GooglePlusLink";
        public static readonly string OBJECT_TEST_USER_SKYPE = "SkypeLink";


        //SKILL:
        public static readonly string OBJECT_SKILL = "Skill";
        public static readonly string OBJECT_SKILL_NAME = "Name";
        public static readonly string OBJECT_SKILL_USERS = "Users";
        public static readonly string OBJECT_SKILL_USERS_TEACH = "UsersTeach";
        public static readonly string OBJECT_SKILL_USERS_KORKING = "UsersKorking";

        //OFFER:
        public static readonly string OBJECT_OFFER = "Offer";
        public static readonly string OBJECT_OFFER_CONTENT = "Content";
        public static readonly string OBJECT_OFFER_USER = "User";


        // General:
        public static readonly string OBJECT_ID = "ObjectId";

        // Default dummy data
        public static readonly string DEFAULT_LOGIN = "tieorange";
        public static readonly string DEFAULT_PASSWORD = "admin";

        #endregion

        #region method

        /// <summary>
        /// get string from object (name from user)
        /// </summary>
        /// <param name="parseObjectKey"></param>
        /// <param name="backingObject"></param>
        /// <returns></returns>
        public static string GetParseObject(string parseObjectKey, ParseObject backingObject)
        {
            return backingObject != null
                    && backingObject.ContainsKey(parseObjectKey)
                    ? backingObject.Get<string>(parseObjectKey)
                    : null;
        }

        public static ParseObject GetParseObjectObject(string parseObjectKey, ParseObject backingObject)
        {
            return backingObject != null
                    && backingObject.ContainsKey(parseObjectKey)
                    ? backingObject.Get<ParseObject>(parseObjectKey)
                    : null;
        }
        public static void SetParseObject(string parseObjectKey, ParseObject backingObject, string value)
        {
            if (backingObject != null
                   && backingObject.ContainsKey(parseObjectKey))
                backingObject[parseObjectKey] = value;
        }

        public static void SetParseObjectObject(string parseObjectKey, ParseObject backingObject, ParseObject value)
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
            ParseRelation<ParseObject> relation = skill.GetRelation<ParseObject>(OBJECT_SKILL_USERS);
            relation.Add(userTieorange);
            relation.Add(userTieorange2);

            // now save the book object
            await skill.SaveAsync();
        }

        public static async Task AddSkillToUser(ParseUser user, ParseObject skill)
        {
            var skillUsersRelationship = skill.GetRelation<ParseObject>(OBJECT_SKILL_USERS);
            skillUsersRelationship.Add(user);
            await skill.SaveAsync();
        }
        public static async Task AddSkillTeachToUser(ParseUser user, ParseObject skill)
        {
            var skillUsersRelationship = skill.GetRelation<ParseObject>(OBJECT_SKILL_USERS_TEACH);
            skillUsersRelationship.Add(user);
            await skill.SaveAsync();
        }
        public static async Task AddSkillKorkingToUser(ParseUser user, ParseObject skill)
        {
            var skillUsersRelationship = skill.GetRelation<ParseObject>(OBJECT_SKILL_USERS_KORKING);
            skillUsersRelationship.Add(user);
            await skill.SaveAsync();
        }

        public static async Task AddSkillToUser(string username, string skillName)
        {
            ParseUser userParseObject = await GetUserByName(username);

            ParseObject skillParseObject = await GetSkillByName(skillName);

            await AddSkillToUser(userParseObject, skillParseObject);
        }

        public static async Task<IEnumerable<ParseUser>> GetAllUsers()
        {
            ParseQuery<ParseUser> query = from item in ParseUser.Query
                                          orderby item.CreatedAt
                                          select item;

            IEnumerable<ParseUser> allUsersList = await query.FindAsync();

            return allUsersList;
        }
        public static async Task<ParseObject> GetSkillByName(string skillName)
        {
            ParseObject skillParseObject = await (from skill in ParseObject.GetQuery(OBJECT_SKILL)
                                                  where skill.Get<string>(OBJECT_SKILL_NAME).Equals(skillName)
                                                  select skill).FirstAsync();

            return skillParseObject;
        }


        public static async Task<List<Skill>> GetAllSkillsList()
        {
            List<ParseObject> skillsParseObjectList = new List<ParseObject>(await ParseObject.GetQuery(OBJECT_SKILL).FindAsync());
            List<Skill> skillsList = new List<Skill>();
            foreach (ParseObject skillParseObject in skillsParseObjectList)
            {
                skillsList.Add(new Skill(skillParseObject));
            }

            return skillsList;
        }
        public static async Task<List<TestUser>> GetAllUsersList()
        {
            List<ParseUser> skillsParseObjectList = new List<ParseUser>(await GetAllUsers());
            List<TestUser> usersList = new List<TestUser>();
            foreach (ParseUser parseUser in skillsParseObjectList)
            {
                usersList.Add(
                    new TestUser(parseUser));
            }

            return usersList;
        }

        public static async Task<ParseUser> GetUserByName(string userName)
        {
            ParseUser userParseObject = await (from user in ParseUser.Query
                                               where user.Username.Equals(userName)
                                               select user).FirstAsync();
            return userParseObject;

        }
        public static async Task<ParseUser> GetUserById(string id)
        {
            return await ParseUser.Query.GetAsync(id);
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
            var relation = skill.GetRelation<ParseObject>(OBJECT_SKILL_USERS);

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
            var querySkillTable = ParseObject.GetQuery(OBJECT_SKILL);

            // now we will query the authors relation to see if the author object we have
            // is contained therein
            var queryTieorange = querySkillTable.WhereEqualTo("Users", userTieorange);

            ////linq
            //IEnumerable<Skill> queryTieorangeSkills = from item in await queryTieorange.FindAsync()
            //                                          where item.Get<ParseRelation<ParseUser>>("Users").Equals(userTieorange)
            //                                          select new Skill(item);

            //foreach (Skill skill in queryTieorangeSkills)
            //{
            //    string skillName = skill.Name;
            //}

            IEnumerable<ParseObject> skillsOfTieorange = await queryTieorange.FindAsync();

            List<Skill> skillsResultList = new List<Skill>();
            foreach (ParseObject skill in skillsOfTieorange)
            {
                string skillName = skill.Get<string>(OBJECT_SKILL);
                Debug.WriteLine("tieorange skill = ", skillName);
                skillsResultList.Add(new Skill(skill));
            }

            foreach (Skill skill in skillsResultList)
            {
                string name = skill.Name;
            }
        }
        #endregion

        public static async Task RemoveSkillFromUser(ParseObject skill)
        {
            var skillUsersRelationship = skill.GetRelation<ParseObject>(OBJECT_SKILL_USERS);
            skillUsersRelationship.Remove(ParseUser.CurrentUser);
            await skill.SaveAsync();
        }
        public static async Task RemoveSkillTeachFromUser(ParseObject skill)
        {
            var skillUsersRelationship = skill.GetRelation<ParseObject>(OBJECT_SKILL_USERS_TEACH);
            skillUsersRelationship.Remove(ParseUser.CurrentUser);
            await skill.SaveAsync();
        }
        public static async Task RemoveSkillKorkingFromUser(ParseObject skill)
        {
            var skillUsersRelationship = skill.GetRelation<ParseObject>(OBJECT_SKILL_USERS_KORKING);
            skillUsersRelationship.Remove(ParseUser.CurrentUser);
            await skill.SaveAsync();
        }


    }
}
