using System;
using System.Globalization;

namespace SquadCalculator.Models;

public class Coffee
{
    public string DonationText { get; set; } = "";
    public Coffee()
    {
        CultureInfo culture = CultureInfo.InstalledUICulture;
        string language = culture.TwoLetterISOLanguageName;
        
        if (language == "ru")
        {
            DonationText =
                "Если вам нравится это приложение, вы можете нажать на картинку и поддержать автора, закинув ему на кофе. А если не нравится - можете закинуть ему на пиво, чтобы он спился.";
        }
        else
        {
            DonationText =
                "If you like this app and want to show appreciation to the author, you can do so by following this picture";
        }
    }
}