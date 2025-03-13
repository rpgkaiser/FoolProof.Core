using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public partial class PredicateAttributeTest
    {
        [TestMethod()]
        public void IsElementaryValid()
        {
            var model = new Predicate.Model() { 
                YearsOfStudy = 6,
                ElementarySchool = true,
                HighSchool = true,
                University = true
            };
            Assert.IsTrue(model.IsValid(nameof(model.ElementarySchool)));
        }

        [TestMethod()]
        public void IsElementaryNotValid()
        {
            var model = new Predicate.Model() {
                YearsOfStudy = 3,
                ElementarySchool = true,
                HighSchool = true,
                University = true
            };
            Assert.IsFalse(model.IsValid(nameof(model.ElementarySchool)));
        }

        [TestMethod()]
        public void IsHihValid()
        {
            var model = new Predicate.Model() {
                YearsOfStudy = 10,
                ElementarySchool = true,
                HighSchool = true,
                University = true
            };
            Assert.IsTrue(model.IsValid(nameof(model.HighSchool)));
        }

        [TestMethod()]
        public void IsHighNotValid()
        {
            var model = new Predicate.Model() {
                YearsOfStudy = 5,
                ElementarySchool = true,
                HighSchool = true,
                University = true
            };
            Assert.IsFalse(model.IsValid(nameof(model.HighSchool)));
        }

        [TestMethod()]
        public void IsUniversityValid()
        {
            var model = new Predicate.Model() {
                YearsOfStudy = 14,
                ElementarySchool = true,
                HighSchool = true,
                University = true
            };
            Assert.IsTrue(model.IsValid(nameof(model.University)));
        }

        [TestMethod()]
        public void IsUniversityNotValid()
        {
            var model = new Predicate.Model() {
                YearsOfStudy = 10,
                ElementarySchool = true,
                HighSchool = true,
                University = true
            };
            Assert.IsFalse(model.IsValid(nameof(model.University)));
        }

        [TestMethod()]
        public void IsModelValid()
        {
            var model = new Predicate.Model() {
                FirstName = "Joe",
                LastName = "Doe",
                Email = "joe.doe@server.com",
                Age = 40,
                YearsOfStudy = 16,
                ElementarySchool = true,
                HighSchool = true,
                University = true
            };
            Assert.IsTrue(model.IsModelValid());
        }

        [TestMethod()]
        public void IsModelNotValid()
        {
            var model = new Predicate.Model() {
                FirstName = "",
                LastName = "",
                Email = "joe.doe.com",
                Age = 40,
                YearsOfStudy = 3,
                ElementarySchool = true,
                HighSchool = true,
                University = true
            };
            Assert.IsFalse(model.IsModelValid());
        }

        [TestMethod()]
        public void IsPhoneNumberValid()
        {
            var model = new Predicate.Model() {
                Country = "US",
                PhoneNumber = "+13053054436"
            };
            Assert.IsTrue(model.IsValid(nameof(model.PhoneNumber)));

            model.Country = "ES";
            model.PhoneNumber = "+34643771134";
            Assert.IsTrue(model.IsValid(nameof(model.PhoneNumber)));

            model.Country = "CU";
            model.PhoneNumber = "+5330522451";
            Assert.IsTrue(model.IsValid(nameof(model.PhoneNumber)));
        }

        [TestMethod()]
        public void IsPhoneNumberNotValid()
        {
            var model = new Predicate.Model() {
                Country = "US",
                PhoneNumber = "+230530544"
            };
            Assert.IsFalse(model.IsValid(nameof(model.PhoneNumber)));

            model.Country = "ES";
            model.PhoneNumber = "+354643771";
            Assert.IsFalse(model.IsValid(nameof(model.PhoneNumber)));

            model.Country = "CU";
            model.PhoneNumber = "+8330522";
            Assert.IsFalse(model.IsValid(nameof(model.PhoneNumber)));
        }
    }
}
