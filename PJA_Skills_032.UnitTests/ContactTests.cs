using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PJA_Skills_032.Model;

namespace PJA_Skills_032.UnitTests
{
    [TestClass]
    public class ContactTests
    {
        private Contact sut;
        private Contact _surname;
        private Contact _name;
        private Contact _position;
        private Contact _number;
        private Contact _biography;

        //[SetUp]
        public void Init()
        {
            _biography = _number = _position = _name = _surname = sut = new Contact("Kovalchuk", "Andrii", "Programmer", "730328282", "Frog");
        }

        [TestMethod]
        public void ShouldInit()
        {
            //assert
            Assert.AreEqual("A K", sut.Initials);
            Assert.That(sut.Initials, Is.EqualTo("A K"));
        }


    }
}
