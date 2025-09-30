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
                    ColorModel = "RGB",
                    FavoriteColor = "blue",
                    FavoriteShade = "some"
                }, 
                BlueShade2Use = "blue",
                RedShade2Use = "",
                ScaleAlgorithm = "",
                CropSize = ""
            };
            Assert.IsTrue(model.Prefs.IsValid(nameof(model.Prefs.FavoriteShade)));
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));
            
            model.Prefs.FavoriteColor = "red";
            model.BlueShade2Use = "";
            model.RedShade2Use = "some";
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));

            Assert.IsTrue(model.IsValid(nameof(model.PixelsPerInch)));
            Assert.IsTrue(model.IsValid(nameof(model.SquareSize)));
            Assert.IsTrue(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsFalse(model.IsValid(nameof(model.CropSize)));
            Assert.IsTrue(model.IsValid(nameof(model.CompressWith)));
            Assert.IsTrue(model.IsValid(nameof(model.StartProcessingDate)));

            model.Prefs.ColorModel = "CMYK";
            model.Prefs.AspectRatio = 1;
            model.Prefs.AutoScale = true;
            model.Prefs.MaxFileSize = 500;
            model.Prefs.InitDate = DateOnly.Parse("05/05/2025");

            model.PixelsPerInch = 150;
            model.SquareSize = 1024;
            model.ScaleAlgorithm = "Bicubic";
            model.CompressWith = RequiredIf.CompressionAlgorithm.Rar;
            model.StartProcessingDate = DateOnly.FromDateTime(DateTime.Now.Date);

            Assert.IsTrue(model.IsValid(nameof(model.PixelsPerInch)));
            Assert.IsTrue(model.IsValid(nameof(model.SquareSize)));
            Assert.IsTrue(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsTrue(model.IsValid(nameof(model.CropSize)));
            Assert.IsTrue(model.IsValid(nameof(model.CompressWith)));
            Assert.IsTrue(model.IsValid(nameof(model.StartProcessingDate)));

            model.Prefs.AutoScale = false;
            model.CropSize = "1024x768";
            Assert.IsTrue(model.IsValid(nameof(model.CropSize)));
        }

        [TestMethod()]
        public void NotValidTest()
        {
            var model = new RequiredIf.PictureSettings() {
                Prefs = new RequiredIf.Preferences() {
                    FavoriteColor = "blue",
                    FavoriteShade = ""
                },
                BlueShade2Use = "",
                RedShade2Use = "",
                ScaleAlgorithm = "",
                CropSize = ""
            };
            Assert.IsFalse(model.Prefs.IsValid(nameof(model.Prefs.FavoriteShade)));
            Assert.IsFalse(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));
            
            model.Prefs.FavoriteColor = "red";
            Assert.IsFalse(model.IsValid(nameof(model.RedShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));

            model.Prefs.ColorModel = "CMYK";
            model.Prefs.AspectRatio = 1;
            model.Prefs.AutoScale = true;
            model.Prefs.MaxFileSize = 500;
            model.Prefs.InitDate = DateOnly.Parse("05/05/2025");

            Assert.IsFalse(model.IsValid(nameof(model.PixelsPerInch)));
            Assert.IsFalse(model.IsValid(nameof(model.SquareSize)));
            Assert.IsFalse(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsTrue(model.IsValid(nameof(model.CropSize)));
            Assert.IsFalse(model.IsValid(nameof(model.CompressWith)));
            Assert.IsFalse(model.IsValid(nameof(model.StartProcessingDate)));

            model.Prefs.AutoScale = false;
            model.ScaleAlgorithm = "";
            Assert.IsTrue(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsFalse(model.IsValid(nameof(model.CropSize)));
        }

        [TestMethod()]
        public void NullValuesTest()
        {
            var model = new RequiredIf.PictureSettings() {
                Prefs = new RequiredIf.Preferences {
                    FavoriteColor = "blue",
                    FavoriteShade = null
                },
                BlueShade2Use = null,
                RedShade2Use = null,
                PixelsPerInch = null,
                ScaleAlgorithm = null,
                CropSize = null
            };
            Assert.IsFalse(model.Prefs.IsValid(nameof(model.Prefs.FavoriteShade)));
            Assert.IsFalse(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));
            
            model.Prefs.FavoriteColor = "red";
            Assert.IsFalse(model.IsValid(nameof(model.RedShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            
            model.Prefs.FavoriteColor = "green";
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));

            Assert.IsFalse(model.IsValid(nameof(model.PixelsPerInch)));

            model.Prefs.ColorModel = "CYMK";
            model.Prefs.AspectRatio = 1;
            model.Prefs.AutoScale = true;
            model.Prefs.MaxFileSize = 500;
            model.Prefs.InitDate = DateOnly.Parse("05/05/2025");
            model.PixelsPerInch = null;
            model.SquareSize = null;
            model.ScaleAlgorithm = null;
            model.CropSize = null;
            model.CompressWith = null;
            model.StartProcessingDate = null;

            Assert.IsFalse(model.IsValid(nameof(model.PixelsPerInch)));
            Assert.IsFalse(model.IsValid(nameof(model.SquareSize)));
            Assert.IsFalse(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsTrue(model.IsValid(nameof(model.CropSize)));
            Assert.IsFalse(model.IsValid(nameof(model.CompressWith)));
            Assert.IsFalse(model.IsValid(nameof(model.StartProcessingDate)));
        }

        [TestMethod()]
        public void NotRequiredTest()
        {
            var model = new RequiredIf.PictureSettings() {
                Prefs = new RequiredIf.Preferences {
                    FavoriteColor = "yellow",
                    FavoriteShade = "#EEAA11"
                }
            };

            Assert.IsTrue(model.Prefs.IsValid(nameof(model.Prefs.FavoriteShade)));
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));
            Assert.IsFalse(model.IsValid(nameof(model.PixelsPerInch)));

            model.Prefs.ColorModel = "RGB";
            model.Prefs.AspectRatio = 0.5M;
            model.Prefs.AutoScale = false;
            model.Prefs.MaxFileSize = 2024;
            model.Prefs.InitDate = DateOnly.Parse("01/01/2024");
            
            Assert.IsTrue(model.IsValid(nameof(model.PixelsPerInch)));
            Assert.IsTrue(model.IsValid(nameof(model.SquareSize)));
            Assert.IsTrue(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsFalse(model.IsValid(nameof(model.CropSize)));
            Assert.IsTrue(model.IsValid(nameof(model.CompressWith)));
            Assert.IsTrue(model.IsValid(nameof(model.StartProcessingDate)));
        }

        [TestMethod()]
        public void NullDepsTest()
        {
            var model = new RequiredIf.PictureSettings() {
                Prefs = new RequiredIf.Preferences {
                    FavoriteColor = null
                }
            };
            Assert.IsTrue(model.Prefs.IsValid(nameof(model.Prefs.FavoriteShade)));
            Assert.IsTrue(model.IsValid(nameof(model.BlueShade2Use)));
            Assert.IsTrue(model.IsValid(nameof(model.RedShade2Use)));

            Assert.IsFalse(model.IsValid(nameof(model.PixelsPerInch)));
            Assert.IsTrue(model.IsValid(nameof(model.SquareSize)));
            Assert.IsTrue(model.IsValid(nameof(model.ScaleAlgorithm)));
            Assert.IsFalse(model.IsValid(nameof(model.CropSize)));
            Assert.IsTrue(model.IsValid(nameof(model.CompressWith)));
            Assert.IsTrue(model.IsValid(nameof(model.StartProcessingDate)));
        }
    }
}
