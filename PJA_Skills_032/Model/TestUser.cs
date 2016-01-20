using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using PJA_Skills_032.ParseObjects;

namespace PJA_Skills_032.Model
{
    public class TestUser
    {
        #region property
        private readonly ParseObject _backingObject;

        public ObservableCollection<Skill> SkillsWantToLearn
        {
            //get
            //{
            //    var querySkillTable = ParseObject.GetQuery(ParseHelper.OBJECT_SKILL);
            //    List<Skill> resultSkillsList = new List<Skill>();
            //    if (this._backingObject != null)
            //    {
            //        var queryTieorange = querySkillTable.WhereEqualTo(ParseHelper.OBJECT_SKILL_USERS, this._backingObject);

            //        IEnumerable<ParseObject> skillsOfTieorange = queryTieorange.FindAsync().Result;

            //        foreach (ParseObject skill in skillsOfTieorange)
            //        {
            //            resultSkillsList.Add(new Skill(skill));
            //        }
            //    }
            //    return resultSkillsList;
            //}
            get; set;
        }

        public List<Skill> SkillsWantToTeach { get; set; }

        public string Name
        {
            get
            {
                return ParseHelper.GetParseObject(ParseHelper.OBJECT_TEST_USER_NAME, _backingObject);
            }
            set
            {
                ParseHelper.SetParseObject(ParseHelper.OBJECT_TEST_USER_NAME, _backingObject, value);
            }
        }

        public string Faculty
        {
            get
            {
                return ParseHelper.GetParseObject(ParseHelper.OBJECT_TEST_USER_FACULTY, _backingObject);
            }
            set
            {
                ParseHelper.SetParseObject(ParseHelper.OBJECT_TEST_USER_FACULTY, _backingObject, value);
            }
        }

        public ParseObject BackingObject
        {
            get
            {
                return _backingObject;
            }
        }

        #endregion


        #region contructor

        public TestUser(ParseObject backingParseObject)
        {
            this._backingObject = backingParseObject;
            this.SkillsWantToLearn = new ObservableCollection<Skill>();

            //TODO: mock
            //this.SkillsWantToLearn = new List<Skill>();
            //this.SkillsWantToLearn.Add(new Skill("WKD"));
            //this.SkillsWantToLearn.Add(new Skill("PRM"));
        }


        public TestUser(string name, string faculty)
        {
            this.Name = name;
            this.Faculty = faculty;
        }
        #endregion


        #region methods

        //public async Task GetSkillsL()
        //{
        //    var relation = _backingObject.GetRelation<ParseObject>(ParseHelper.OBJECT_TEST_USER_SKILLS_WANT_TO_LEARN);
        //    var querySkills = relation.Query;

        //    var queryLinq = from item in await querySkills.FindAsync()
        //                    select new Skill(item);
        //    // Get Skills want to learn 
        //    var skillsCollection = new List<Skill>(queryLinq);
        //    SkillsWantToLearn = skillsCollection;
        //}
        //public void GetSkillsLdummy()
        //{
        //    // skills
        //    Skill skillPRM = new Skill("PRM");
        //    Skill skillBSI = new Skill("BSI");
        //    Skill skillUKO = new Skill("UKO");

        //    SkillsWantToLearn.Add(skillPRM);
        //    SkillsWantToLearn.Add(skillBSI);
        //    SkillsWantToLearn.Add(skillUKO);
        //}

        public static async Task Login()
        {
            try
            {
                await ParseUser.LogInAsync(ParseHelper.DEFAULT_LOGIN, ParseHelper.DEFAULT_PASSWORD);
                // Login was successful.
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static async Task Login(string login, string password)
        {
            try
            {
                if (login != null && password != null)
                    await ParseUser.LogInAsync(login, password);
                // Login was successful.
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task GetSkills()
        {
            // first we will create a query on the Skill object
            ParseQuery<ParseObject> querySkillTable = ParseObject.GetQuery(ParseHelper.OBJECT_SKILL);

            // now we will query the authors relation to see if the author object we have
            // is contained therein
            if (this._backingObject != null)
            {
                ParseQuery<ParseObject> queryTieorange = querySkillTable.WhereEqualTo(ParseHelper.OBJECT_SKILL_USERS, this._backingObject);

                IEnumerable<ParseObject> skillsOfTieorange = await queryTieorange.FindAsync();

                foreach (ParseObject skill in skillsOfTieorange)
                {
                    this.SkillsWantToLearn.Add(new Skill(skill));
                }
            }
        }

        public ParseRelation<ParseObject> GetSkillsParseRelation()
        {
            ParseRelation<ParseObject> billGatesSkillsWantToLearn = _backingObject.GetRelation<ParseObject>("SkillsWantToLearn2");
            return billGatesSkillsWantToLearn;
        }


        #endregion


    }
}
