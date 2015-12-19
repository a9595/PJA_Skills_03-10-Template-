using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public List<Skill> SkillsWantToLearn { get; set; }

        public string Name
        {
            get
            {
                return BackingObject != null && BackingObject.ContainsKey("Name") ? BackingObject.Get<string>("Name") : null;
            }
            set { if (BackingObject != null) BackingObject["Name"] = value; }
            //get; set;
        }


        public string Faculty
        {
            get { return BackingObject != null && BackingObject.ContainsKey("Faculty") ? BackingObject.Get<string>("Faculty") : null; }
            set { if (BackingObject != null) BackingObject["Faculty"] = value; }
            //get; set;
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

            //TODO: mock
            this.SkillsWantToLearn = new List<Skill>();
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

        public async Task GetSkillsL()
        {
            var relation = _backingObject.GetRelation<ParseObject>(ParseHelper.OBJECT_TEST_USER_SKILLS_WANT_TO_LEARN);
            var querySkills = relation.Query;

            var queryLinq = from item in await querySkills.FindAsync()
                            select new Skill(item);
            // Get Skills want to learn 
            var skillsCollection = new List<Skill>(queryLinq);
            SkillsWantToLearn = skillsCollection;
        }

        #endregion


    }
}
