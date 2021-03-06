﻿using System;
using System.Collections;
using System.Collections.Generic;
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
                return ParseHelper.GetParseObject(ParseHelper.OBJECT_SKILL_NAME, _backingObject);
            }

            set
            {
                ParseHelper.SetParseObject(ParseHelper.OBJECT_SKILL_NAME, _backingObject, value);
            }
        }

        public bool IsEqualToOtherSkill(Skill skill)
        {
            if (this._backingObject.ObjectId.Equals(skill.getBackingObject.ObjectId))
                return true;
            else return false;
        }

        public bool IsContainsInOtherList(IEnumerable<Skill> skillsList)
        {
            bool result = false;
            foreach (Skill skill in skillsList)
            {
                if (this.Name.Equals(skill.Name))
                {
                    result = true;
                }
            }
            return result;
        }

        public ParseObject getBackingObject => _backingObject;

        public Skill(ParseObject backingParseObject)
        {
            if (backingParseObject == null) throw new ArgumentNullException(nameof(backingParseObject));
            this._backingObject = backingParseObject;
        }

        public Skill(string skillName)
        {

        }

        /*
                public Skill(string name)
                {
                    if (name == null) throw new ArgumentNullException(nameof(name));
                    this.Name = name;
                }
        */

        public override string ToString()
        {
            return Name;
        }
    }
}