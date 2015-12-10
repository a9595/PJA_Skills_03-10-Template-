using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PJA_Skills_032.Model;

namespace PJA_Skills_032.UnitTest
{
    [TestClass]
    public class ContactTests
    {
        string lastName = "Kovalchuk";
        string firstName = "Andrii";
        string position = "Programmer";
        string phoneNumber = "730328282";
        string biography = "like pinguins";
        private Contact sut;

        [TestInitialize]
        public void TestMethod1()
        {
            sut = new Contact(lastName, firstName, position, phoneNumber, biography);
        }

        [TestMethod]
        public void InitialsTes()
        {
            // Act
            var initials = sut.Initials;

            // Assert
            Assert.AreEqual("AK", initials);
        }

        [TestMethod]
        public void NameTest()
        {
            // Act
            string expectedName = firstName + " " + lastName;
            string actualName = sut.Name;
            // Assert
            Assert.AreEqual(expectedName, actualName);
        }


        //[TestMethod]
        //public void Debit_WithValidAmount_UpdatesBalance()
        //{
        //    // arrange
        //    double beginningBalance = 11.99;
        //    double debitAmount = 4.55;
        //    double expected = 7.44;
        //    BankAccount account = new BankAccount("Mr. Bryan Walton", beginningBalance);

        //    // act
        //    account.Debit(debitAmount);

        //    // assert
        //    double actual = account.Balance;
        //    Assert.AreEqual(expected, actual, 0.001, "Account not debited correctly");
        //}
    }
}
