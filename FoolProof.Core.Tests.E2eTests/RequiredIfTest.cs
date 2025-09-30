using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace FoolProof.Core.Tests.E2eTests
{
    [TestClass]
    public class RequiredIfTest : BasePageTest
    {
        protected override Regex PageTitleRegex() => new(@".+\s+[-]\s+RequiredIf");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), "requiredif");

        [TestInitialize]
        public override Task InitTest()
        {
            UseInputTypes = true;
            return base.InitTest();
        }

        [CustomTestMethod]
        public async Task ModelValidationPass()
        {
            await LoadPage();

            foreach (var testValues in GetValues2PassValidation())
            {
                await CallClientValidation(testValues, true);
                await ExpectValidationSucceed();

                await CallServerValidation(testValues, true);
                await ExpectValidationSucceed();
            }
        }

        [CustomTestMethod]
        public async Task ModelValidationFails()
        {
            await LoadPage();

            foreach (var testValues in GetValues2FailsValidation())
            {
                await CallClientValidation(testValues, true);
                await ExpectValidationFailed(testValues, "Model validation failed");
                
                await CallServerValidation(testValues, true);

                var validMsgs = testValues.AllValues()
                                .Where(tv => !string.IsNullOrEmpty(tv.ValidResultText))
                                .Select(tv => tv.ValidResultText!);
                await ExpectValidationFailed(testValues, [.. validMsgs]);
            }
        }

        protected IEnumerable<RequiredIfTestValues> GetValues2PassValidation()
        {
            var testValues = new RequiredIfTestValues(
                null,
                "blue",
                "#FF1122",
                null,
                true,
                1,
                512,
                DateOnly.Parse("05/05/2025"),
                blueShade: "#AA11EE",
                pixelsXinch: 150,
                scaleAlg: "Bicubic",
                sqrSize: 1024,
                compWith: RequiredIf.CompressionAlgorithm.Rar,
                startDate: DateOnly.Parse("10/10/2025")
            );
            yield return testValues;

            yield return testValues = testValues.Clone(tv => {
                tv.ColorModel.Value = "RGB";
                tv.BitDepth.Value = 8;
            });

            yield return testValues = testValues.Clone(tv => {
                tv.PixelsPerInch.Value = null;
            });

            yield return testValues = testValues.Clone(tv => {
                tv.BlueShade.Value = null;
                tv.FavColor.Value = "red";
                tv.RedShade.Value = "#DD11E2";
            });

            yield return testValues = testValues.Clone(tv => {
                tv.ColorModel.Value = "CYMK";
                tv.PixelsPerInch.Value = 150;
            });

            yield return testValues = testValues.Clone(tv => {
                tv.AutoScale.Value = false;
                tv.ScaleAlg.Value = null;
                tv.CropSize.Value = "1024x768";
            });

            yield return testValues = testValues.Clone(tv => {
                tv.AspectRatio.Value = 1.5M;
                tv.SqrSize.Value = null;
            });

            yield return testValues = testValues.Clone(tv => {
                tv.MaxFileSize.Value = 2048;
                tv.SqrSize.Value = null;
            });

            yield return testValues = testValues.Clone(tv => {
                tv.MaxFileSize.Value = 2048;
                tv.CompWith.Value = null;
            });

            yield return testValues = testValues.Clone(tv => {
                tv.InitDate.Value = DateOnly.Parse("02/02/2024");
                tv.StartDate.Value = null;
            });
        }

        protected IEnumerable<RequiredIfTestValues> GetValues2FailsValidation()
        {
            var testValues = new RequiredIfTestValues(
                null,
                "blue",
                "#FF1122",
                null,
                true,
                1,
                512,
                DateOnly.Parse("05/05/2025"),
                blueShade: "#AA11EE",
                pixelsXinch: 150,
                scaleAlg: "Bicubic",
                cropSize: "1024x768",
                sqrSize: 1024,
                compWith: RequiredIf.CompressionAlgorithm.Rar,
                startDate: DateOnly.Parse("10/10/2025")
            );

            yield return testValues = testValues.Clone(tv => {
                tv.FavColor.Value = null;
                tv.FavColor.ValidResultText = "FavoriteColor is required due to ColorModel being empty";
            });

            yield return testValues = testValues.Clone(tv => {
                tv.ColorModel.Value = "RGB";
                tv.FavColor.ValidResultText = "";
                tv.BitDepth.ValidResultText = @"BitDepth is required due to ColorModel being a match to ^\s*(r|R)(g|G)(b|B)\s*$";
            });

            yield return testValues = testValues.Clone(tv => {
                tv.BitDepth.Value = 8;
                tv.BitDepth.ValidResultText = "";
                tv.FavColor.Value = "blue";
                tv.FavShade.Value = null;
                tv.FavShade.ValidResultText = "FavoriteShade is required due to FavoriteColor not being empty";
            });

            yield return testValues = testValues.Clone(tv => {
                tv.FavShade.Value = "#FF1122";
                tv.FavShade.ValidResultText = "";
                tv.BlueShade.Value = null;
                tv.BlueShade.ValidResultText = "BlueShade2Use is required due to Prefs.FavoriteColor being equal to blue";
            });

            yield return testValues = testValues.Clone(tv => {
                tv.FavColor.Value = "red";
                tv.BlueShade.Value = null;
                tv.BlueShade.ValidResultText = "";
                tv.RedShade.Value = null;
                tv.RedShade.ValidResultText = "RedShade2Use is required due to Prefs.FavoriteColor being equal to red";
            });

            yield return testValues = testValues.Clone(tv => {
                tv.FavColor.Value = "green";
                tv.RedShade.ValidResultText = "";
                tv.ColorModel.Value = "CYMK";
                tv.PixelsPerInch.Value = null;
                tv.PixelsPerInch.ValidResultText = @"PixelsPerInch is required due to Prefs.ColorModel being not a match to ^\s*(r|R)(g|G)(b|B)\s*$";
            });

            yield return testValues = testValues.Clone(tv => {
                tv.PixelsPerInch.Value = 150;
                tv.PixelsPerInch.ValidResultText = "";
                tv.AutoScale.Value = true;
                tv.ScaleAlg.Value = null;
                tv.ScaleAlg.ValidResultText = "ScaleAlgorithm is required due to Prefs.AutoScale being equal to True";
            });

            yield return testValues = testValues.Clone(tv => {
                tv.AutoScale.Value = false;
                tv.ScaleAlg.Value = null;
                tv.ScaleAlg.ValidResultText = "";
                tv.CropSize.Value = null;
                tv.CropSize.ValidResultText = "CropSize is required due to Prefs.AutoScale being equal to False";
                tv.AspectRatio.Value = 1M;
                tv.SqrSize.Value = null;
                tv.SqrSize.ValidResultText = "SquareSize is required due to Prefs.AspectRatio being equal to 1";
            });

            yield return testValues = testValues.Clone(tv => {
                tv.CropSize.Value = "1024x768";
                tv.CropSize.ValidResultText = "";
                tv.AspectRatio.Value = 1M;
                tv.SqrSize.Value = null;
                tv.SqrSize.ValidResultText = "SquareSize is required due to Prefs.AspectRatio being equal to 1";
            });

            yield return testValues = testValues.Clone(tv => {
                tv.SqrSize.Value = 720;
                tv.SqrSize.ValidResultText = "";
                tv.MaxFileSize.Value = 512;
                tv.CompWith.Value = null;
                tv.CompWith.ValidResultText = "CompressWith is required due to Prefs.MaxFileSize being less than or equal to 1024";
            });

            yield return testValues = testValues.Clone(tv => {
                tv.CompWith.Value = RequiredIf.CompressionAlgorithm.Rar;
                tv.CompWith.ValidResultText = "";
                tv.InitDate.Value = DateOnly.Parse("05/05/2025");
                tv.StartDate.Value = null;
                tv.StartDate.ValidResultText = "StartProcessingDate is required due to Prefs.InitDate being greater than 01/01/2025";
            });
        }

        protected class RequiredIfTestValues : TestValues
        {
            protected internal IEnumerable<InputTestValue> _values;

            public RequiredIfTestValues(
                string? colorModel = default,
                string? favColor = default,
                string? favShade = default,
                int? bitDepth = default,
                bool? autoScale = null,
                decimal? aspectRatio = null,
                int? maxFileSize = null,
                DateOnly? initDate = null,
                string? redShade = null,
                string? blueShade = null,
                int? pixelsXinch = null,
                string? scaleAlg = null,
                string? cropSize = null,
                int? sqrSize = null,
                RequiredIf.CompressionAlgorithm? compWith = null,
                DateOnly? startDate = null
            )
            {
                _values = new InputTestValue[] {
                    new(

                        $"{nameof(RequiredIf.PictureSettings.Prefs)}_{nameof(RequiredIf.PictureSettings.Prefs.ColorModel)}",
                        colorModel,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        
                        $"{nameof(RequiredIf.PictureSettings.Prefs)}_{nameof(RequiredIf.PictureSettings.Prefs.FavoriteColor)}", 
                        favColor,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        $"{nameof(RequiredIf.PictureSettings.Prefs)}_{nameof(RequiredIf.PictureSettings.Prefs.FavoriteShade)}",
                        favShade,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        $"{nameof(RequiredIf.PictureSettings.Prefs)}_{nameof(RequiredIf.PictureSettings.Prefs.BitDepth)}",
                        bitDepth,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        $"{nameof(RequiredIf.PictureSettings.Prefs)}_{nameof(RequiredIf.PictureSettings.Prefs.AutoScale)}",
                        autoScale,
                        false,
                        resetAsEmpty: false
                    ),
                    new(
                        $"{nameof(RequiredIf.PictureSettings.Prefs)}_{nameof(RequiredIf.PictureSettings.Prefs.AspectRatio)}",
                        aspectRatio,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        $"{nameof(RequiredIf.PictureSettings.Prefs)}_{nameof(RequiredIf.PictureSettings.Prefs.MaxFileSize)}",
                        maxFileSize,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        $"{nameof(RequiredIf.PictureSettings.Prefs)}_{nameof(RequiredIf.PictureSettings.Prefs.InitDate)}",
                        initDate,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        nameof(RequiredIf.PictureSettings.RedShade2Use),
                        redShade,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        nameof(RequiredIf.PictureSettings.BlueShade2Use),
                        blueShade,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        nameof(RequiredIf.PictureSettings.PixelsPerInch),
                        pixelsXinch,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        nameof(RequiredIf.PictureSettings.ScaleAlgorithm),
                        scaleAlg,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        nameof(RequiredIf.PictureSettings.CropSize),
                        cropSize,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        nameof(RequiredIf.PictureSettings.SquareSize),
                        sqrSize,
                        false,
                        resetAsEmpty: true
                    ),
                    new(
                        nameof(RequiredIf.PictureSettings.CompressWith),
                        compWith,
                        true,
                        resetAsEmpty: false
                    ),
                    new(
                        nameof(RequiredIf.PictureSettings.StartProcessingDate),
                        startDate,
                        false,
                        resetAsEmpty: true
                    )
                };
            }

            protected RequiredIfTestValues(params InputTestValue[] values)
            {
                _values = values?.ToList() ?? new List<InputTestValue>();
            }

            public InputTestValue ColorModel => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.Preferences.ColorModel)));

            public InputTestValue FavColor => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.Preferences.FavoriteColor)));

            public InputTestValue FavShade => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.Preferences.FavoriteShade)));

            public InputTestValue BitDepth => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.Preferences.BitDepth)));

            public InputTestValue AutoScale => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.Preferences.AutoScale)));

            public InputTestValue AspectRatio => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.Preferences.AspectRatio)));

            public InputTestValue MaxFileSize => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.Preferences.MaxFileSize)));

            public InputTestValue InitDate => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.Preferences.InitDate)));

            public InputTestValue RedShade => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.PictureSettings.RedShade2Use)));

            public InputTestValue BlueShade => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.PictureSettings.BlueShade2Use)));

            public InputTestValue PixelsPerInch => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.PictureSettings.PixelsPerInch)));

            public InputTestValue ScaleAlg => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.PictureSettings.ScaleAlgorithm)));

            public InputTestValue CropSize => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.PictureSettings.CropSize)));

            public InputTestValue SqrSize => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.PictureSettings.SquareSize)));

            public InputTestValue CompWith => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.PictureSettings.CompressWith)));

            public InputTestValue StartDate => _values.First(v => v.InputId.EndsWith(nameof(RequiredIf.PictureSettings.StartProcessingDate)));

            public RequiredIfTestValues Clone(Action<RequiredIfTestValues>? modify = null)
            {
                var nVals = _values.Select(v => v.Clone());
                var result = new RequiredIfTestValues([.. nVals]);

                modify?.Invoke(result);

                return result;
            }

            public override IEnumerable<InputTestValue> AllValues() => _values;
        }
    }
}