using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class RequiredIf
    {
        public enum CompressionAlgorithm
        {
            Rar = 0,
            SevenZ,
            Tar,
            Zip
        }

        public class Preferences : ValidationModelBase
        {
            public string? FavoriteColor { get; set; }

            [RequiredIf("FavoriteColor", "blue")]
            public string? FavoriteBlueShade { get; set; }

            public bool AutoScale { get; set; }

            public decimal? AspectRatio { get; set; }

            public int? MaxFileSize { get; set; }

            [DataType(DataType.Date)]
            public DateOnly? InitDate { get; set; }
        }

        public class PictureSettings : ValidationModelBase
        {
            public Preferences Prefs { get; set; } = new Preferences();

            [RequiredIf("Prefs.FavoriteColor", "blue")]
            public string? BlueShade2Use { get; set; }

            [RequiredIf("Prefs.FavoriteColor", "red")]
            public string? RedShade2Use { get; set; }

            [RequiredIf("Prefs.FavoriteColor", "green")]
            public string? GreenShade2Use { get; set; }

            [RequiredIf("Prefs.AutoScale", true)]
            public string? ScaleAlgorithm { get; set; }

            [RequiredIf("Prefs.AspectRatio", 1)]
            public int? SquareSize { get; set; }

            [RequiredIf("Prefs.MaxFileSize", Operator.LessThanOrEqualTo, 1024)]
            public CompressionAlgorithm? CompressWith { get; set; }

            [RequiredIf("Prefs.InitDate", Operator.GreaterThan, dependentValue: "01/01/2025")]
            [DataType(DataType.Date)]
            public DateOnly? StartProcessingDate { get; set; }
        }
    }
}
