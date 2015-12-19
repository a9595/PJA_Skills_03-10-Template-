using System;
using Parse;
using PJA_Skills_032.ParseObjects;

namespace PJA_Skills_032.Model
{
    public class Skill
    {
        private readonly ParseObject _backingObject;

        public string Name
        {
            get
            {
                return _backingObject != null
                    && _backingObject.ContainsKey(ParseHelper.OBJECT_SKILL_NAME)
                    ? _backingObject.Get<string>(ParseHelper.OBJECT_SKILL_NAME)
                    : null;
            }
            private set
            {
                if (_backingObject != null
                    && _backingObject.ContainsKey(ParseHelper.OBJECT_SKILL_NAME))
                    _backingObject[ParseHelper.OBJECT_SKILL_NAME] = value;
            }
        }
        //public string Name { get; set; }


        public Skill(ParseObject backingParseObject)
        {
            if (backingParseObject == null) throw new ArgumentNullException(nameof(backingParseObject));
            this._backingObject = backingParseObject;
        }

        public Skill(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}