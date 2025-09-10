using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class RequiredIfAttributeTest
    {
        [TestMethod()]
        public void IsValidTest()
        {
            var model = new RequiredIf.Preferences() { 
                FavoriteColor = "blue", 
                FavoriteBlueShade = "some" 
            };
            Assert.IsTrue(model.IsValid("FavoriteBlueShade"));
        }

        [TestMethod()]
        public void IsValidTestComplex()
        {
            var model = new RequiredIf.PictureSettings() { 
                Prefs = new RequiredIf.Preferences() { FavoriteColor = "blue" }, 
                BlueShade2Use = "blue",
                GreenShade2Use = "",
                RedShade2Use = ""
            };
            Assert.IsTrue(model.IsValid("BlueShade2Use"));
            Assert.IsTrue(model.IsValid("RedShade2Use"));
            Assert.IsTrue(model.IsValid("GreenShade2Use"));

            model.Prefs.FavoriteColor = "red";
            model.BlueShade2Use = "";
            model.RedShade2Use = "some";
            model.GreenShade2Use = "";
            Assert.IsTrue(model.IsValid("BlueShade2Use"));
            Assert.IsTrue(model.IsValid("RedShade2Use"));
            Assert.IsTrue(model.IsValid("GreenShade2Use"));

            model.Prefs.FavoriteColor = "green";
            model.BlueShade2Use = "";
            model.RedShade2Use = "";
            model.GreenShade2Use = "some";
            Assert.IsTrue(model.IsValid("BlueShade2Use"));
            Assert.IsTrue(model.IsValid("RedShade2Use"));
            Assert.IsTrue(model.IsValid("GreenShade2Use"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new RequiredIf.Preferences() {
                FavoriteColor = "blue",
                FavoriteBlueShade = ""
            };
            Assert.IsFalse(model.IsValid("FavoriteBlueShade"));
        }

        [TestMethod()]
        public void IsNotValidTestComplex()
        {
            var model = new RequiredIf.PictureSettings() {
                Prefs = new RequiredIf.Preferences() { 
                    FavoriteColor = "blue" 
                },
                BlueShade2Use = "",
                GreenShade2Use = "",
                RedShade2Use = ""
            };
            Assert.IsFalse(model.IsValid("BlueShade2Use"));
            Assert.IsTrue(model.IsValid("RedShade2Use"));
            Assert.IsTrue(model.IsValid("GreenShade2Use"));

            model.Prefs.FavoriteColor = "red";
            Assert.IsFalse(model.IsValid("RedShade2Use"));
            Assert.IsTrue(model.IsValid("BlueShade2Use"));
            Assert.IsTrue(model.IsValid("GreenShade2Use"));

            model.Prefs.FavoriteColor = "green";
            Assert.IsFalse(model.IsValid("GreenShade2Use"));
            Assert.IsTrue(model.IsValid("BlueShade2Use"));
            Assert.IsTrue(model.IsValid("RedShade2Use"));
        }

        [TestMethod()]
        public void IsNotValidWithNullTest()
        {
            var model = new RequiredIf.Preferences() { 
                FavoriteColor = "blue", 
                FavoriteBlueShade = null 
            };
            Assert.IsFalse(model.IsValid("FavoriteBlueShade"));
        }

        [TestMethod()]
        public void IsNotValidWithNullTestComplex()
        {
            var model = new RequiredIf.PictureSettings() {
                Prefs = new RequiredIf.Preferences {
                    FavoriteColor = "blue"
                },
                BlueShade2Use = null,
                RedShade2Use = null,
                GreenShade2Use = null
            };
            Assert.IsFalse(model.IsValid("BlueShade2Use"));
            Assert.IsTrue(model.IsValid("RedShade2Use"));
            Assert.IsTrue(model.IsValid("GreenShade2Use"));

            model.Prefs.FavoriteColor = "red";
            Assert.IsFalse(model.IsValid("RedShade2Use"));
            Assert.IsTrue(model.IsValid("BlueShade2Use"));
            Assert.IsTrue(model.IsValid("GreenShade2Use"));

            model.Prefs.FavoriteColor = "green";
            Assert.IsFalse(model.IsValid("GreenShade2Use"));
            Assert.IsTrue(model.IsValid("BlueShade2Use"));
            Assert.IsTrue(model.IsValid("RedShade2Use"));
        }

        [TestMethod()]
        public void IsNotRequiredTest()
        {
            var model = new RequiredIf.Preferences() { 
                FavoriteColor = "yellow" 
            };
            Assert.IsTrue(model.IsValid("FavoriteBlueShade"));
        }

        [TestMethod()]
        public void IsNotRequiredTestComplex()
        {
            var model = new RequiredIf.PictureSettings() {
                Prefs = new RequiredIf.Preferences {
                    FavoriteColor = "yellow"
                }
            };
            Assert.IsTrue(model.IsValid("BlueShade2Use"));
            Assert.IsTrue(model.IsValid("RedShade2Use"));
            Assert.IsTrue(model.IsValid("GreenShade2Use"));
        }

        [TestMethod()]
        public void IsNotRequiredWithNullTest()
        {
            var model = new RequiredIf.Preferences() { 
                FavoriteColor = null
            };
            Assert.IsTrue(model.IsValid("FavoriteBlueShade"));
        }

        [TestMethod()]
        public void IsNotRequiredWithNullTestComplex()
        {
            var model = new RequiredIf.PictureSettings() {
                Prefs = new RequiredIf.Preferences {
                    FavoriteColor = null
                }
            };
            Assert.IsTrue(model.IsValid("BlueShade2Use"));
            Assert.IsTrue(model.IsValid("RedShade2Use"));
            Assert.IsTrue(model.IsValid("GreenShade2Use"));
        }
    }
}
