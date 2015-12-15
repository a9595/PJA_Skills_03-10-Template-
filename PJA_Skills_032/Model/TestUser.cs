using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

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
                return _backingObject != null && _backingObject.ContainsKey("Name") ? _backingObject.Get<string>("Name") : null;
            }
            set { if (_backingObject != null) _backingObject["Name"] = value; }
            //get; set;
        }


        public string Faculty
        {
            get { return _backingObject != null && _backingObject.ContainsKey("Faculty") ? _backingObject.Get<string>("Faculty") : null; }
            set { if (_backingObject != null) _backingObject["Faculty"] = value; }
            //get; set;
        }

        #endregion


        #region contructor

        public TestUser(ParseObject backingParseObject)
        {
            this._backingObject = backingParseObject;

            //TODO: mock
            this.SkillsWantToLearn = new List<Skill>();
            this.SkillsWantToLearn.Add(new Skill("WKD"));
            this.SkillsWantToLearn.Add(new Skill("PRM"));
        }


        public TestUser(string name, string faculty)
        {
            this.Name = name;
            this.Faculty = faculty;
        }

        #endregion

    }
}
