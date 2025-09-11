using Microsoft.AspNetCore.Components.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class RequiredIfAttributeTest
    {
        [TestMethod()]
        public void IsValidTest()
        {
            var model = new RequiredIf.PictureSettings() { 
                Prefs = new RequiredIf.Preferences() { 
                    FavoriteColor = "blue",
                    FavoriteBlueShade = "some"
                }, 
                BlueShade2Use = "blue",
                GreenShade2Use = "",
                RedShade2Use = "",
                ScaleAlgorithm = ""
            };
            Assert.IsTrue(model.Prefs.IsValid(nameof(model.Prefs.FavoriteBlueShade)));
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.GreenShade2Use)));

            model.Prefs.FavoriteColor = "red";
            model.BlueShade2Use = "";
            model.RedShade2Use = "some";
            model.GreenShade2Use = "";
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.GreenShade2Use)));

            model.Prefs.FavoriteColor = "green";
            model.BlueShade2Use = "";
            model.RedShade2Use = "";
            model.GreenShade2Use = "some";
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.GreenShade2Use)));

            Assert.IsTrue(model.IsValid(nameof(model.SquareSize)));
            Assert.IsTrue(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsTrue(model.IsValid(nameof(model.CompressWith)));
            Assert.IsTrue(model.IsValid(nameof(model.StartProcessingDate)));

            model.Prefs.AspectRatio = 1;
            model.Prefs.AutoScale = true;
            model.Prefs.MaxFileSize = 500;
            model.Prefs.InitDate = DateOnly.Parse("05/05/2025");
            model.SquareSize = 1024;
            model.ScaleAlgorithm = "Bicubic";
            model.CompressWith = RequiredIf.CompressionAlgorithm.Rar;
            model.StartProcessingDate = DateOnly.FromDateTime(DateTime.Now.Date);

            Assert.IsTrue(model.IsValid(nameof(model.SquareSize)));
            Assert.IsTrue(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsTrue(model.IsValid(nameof(model.CompressWith)));
            Assert.IsTrue(model.IsValid(nameof(model.StartProcessingDate)));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new RequiredIf.PictureSettings() {
                Prefs = new RequiredIf.Preferences() { 
                    FavoriteColor = "blue",
                    FavoriteBlueShade = ""
                },
                BlueShade2Use = "",
                GreenShade2Use = "",
                RedShade2Use = ""
            };
            Assert.IsFalse(model.Prefs.IsValid(nameof(model.Prefs.FavoriteBlueShade)));
            Assert.IsFalse(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.GreenShade2Use)));

            model.Prefs.FavoriteColor = "red";
            Assert.IsFalse(model.IsValid(nameof(model.RedShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.GreenShade2Use)));

            model.Prefs.FavoriteColor = "green";
            Assert.IsFalse(model.IsValid(nameof(model.GreenShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));

            model.Prefs.AspectRatio = 1;
            model.Prefs.AutoScale = true;
            model.Prefs.MaxFileSize = 500;
            model.Prefs.InitDate = DateOnly.Parse("05/05/2025");

            Assert.IsFalse(model.IsValid(nameof(model.SquareSize)));
            Assert.IsFalse(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsFalse(model.IsValid(nameof(model.CompressWith)));
            Assert.IsFalse(model.IsValid(nameof(model.StartProcessingDate)));
        }

        [TestMethod()]
        public void NullValuesIsNotValidTest()
        {
            var model = new RequiredIf.PictureSettings() {
                Prefs = new RequiredIf.Preferences {
                    FavoriteColor = "blue",
                    FavoriteBlueShade = null
                },
                BlueShade2Use = null,
                RedShade2Use = null,
                GreenShade2Use = null
            };
            Assert.IsFalse(model.Prefs.IsValid(nameof(model.Prefs.FavoriteBlueShade)));
            Assert.IsFalse(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.GreenShade2Use)));

            model.Prefs.FavoriteColor = "red";
            Assert.IsFalse(model.IsValid(nameof(model.RedShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.GreenShade2Use)));

            model.Prefs.FavoriteColor = "green";
            Assert.IsFalse(model.IsValid(nameof(model.GreenShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));

            model.Prefs.AspectRatio = 1;
            model.Prefs.AutoScale = true;
            model.Prefs.MaxFileSize = 500;
            model.Prefs.InitDate = DateOnly.Parse("05/05/2025");
            model.SquareSize = null;
            model.ScaleAlgorithm = null;
            model.CompressWith = null;
            model.StartProcessingDate = null;

            Assert.IsFalse(model.IsValid(nameof(model.SquareSize)));
            Assert.IsFalse(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsFalse(model.IsValid(nameof(model.CompressWith)));
            Assert.IsFalse(model.IsValid(nameof(model.StartProcessingDate)));
        }

        [TestMethod()]
        public void IsNotRequiredTest()
        {
            var model = new RequiredIf.PictureSettings() {
                Prefs = new RequiredIf.Preferences {
                    FavoriteColor = "yellow",
                    FavoriteBlueShade = ""
                }
            };

            Assert.IsTrue(model.Prefs.IsValid(nameof(model.Prefs.FavoriteBlueShade)));
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.GreenShade2Use)));

            model.Prefs.AspectRatio = 0.5M;
            model.Prefs.AutoScale = false;
            model.Prefs.MaxFileSize = 2024;
            model.Prefs.InitDate = DateOnly.Parse("01/01/2024");

            Assert.IsTrue(model.IsValid(nameof(model.SquareSize)));
            Assert.IsTrue(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsTrue(model.IsValid(nameof(model.CompressWith)));
            Assert.IsTrue(model.IsValid(nameof(model.StartProcessingDate)));
        }

        [TestMethod()]
        public void NullDepsIsNotRequiredTest()
        {
            var model = new RequiredIf.PictureSettings() {
                Prefs = new RequiredIf.Preferences {
                    FavoriteColor = null
                }
            };
            Assert.IsTrue(model.Prefs.IsValid(nameof(model.Prefs.FavoriteBlueShade)));
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.GreenShade2Use)));

            Assert.IsTrue(model.IsValid(nameof(model.SquareSize)));
            Assert.IsTrue(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsTrue(model.IsValid(nameof(model.CompressWith)));
            Assert.IsTrue(model.IsValid(nameof(model.StartProcessingDate)));
        }
    }
}
