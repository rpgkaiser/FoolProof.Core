namespace FoolProof.Core.Tests.Models
{
    public class RequiredIf
    {
        public class Preferences : ValidationModelBase
        {
            public string? FavoriteColor { get; set; }

            [RequiredIf("FavoriteColor", "blue")]
            public string? FavoriteBlueShade { get; set; }
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
        }
    }
}
