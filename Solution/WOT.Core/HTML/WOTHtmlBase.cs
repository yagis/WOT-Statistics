using System;
using System.Drawing;

using System.IO;

namespace WOTStatistics.Core
{
    public class WOTHtmlBase
    {
     
     	public MessageQueue _message {get; private set;}

        public WOTHtmlBase(MessageQueue message)
		{

			_message = message;

		}

        public string GetDelta(double value, string sign, int decimalPlaces, bool reverse = false)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                value = 0;
            }

            if (!reverse)
            {
                return GetDeltaExtracted(value, sign, decimalPlaces, reverse, UserSettings.ColorPositive, UserSettings.ColorNegative);
            }
            else
            {
                return GetDeltaExtracted(value, sign, decimalPlaces, reverse, UserSettings.ColorNegative, UserSettings.ColorPositive);
            }
        }

        private string GetDeltaExtracted(double value, string sign, int decimalPlaces, bool reverse, int argb, int argb1)
        {
            if (value == 0)
                return @"<span style='white-space: nowrap;'><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNeutral)) + "'>" + WOTHelper.FormatNumberToString(value, decimalPlaces) + sign + "</font>" + MovementGif(value, reverse) + "</span>";
            else
                if (value > 0)
                    return @"<span style='white-space: nowrap;'><font color='" + ColorTranslator.ToHtml(Color.FromArgb(argb)) + "'>+" + WOTHelper.FormatNumberToString(value, decimalPlaces) + sign + "</font>" + MovementGif(value, reverse) + "</span>";
                else
                    return String.Format(@"<span style='white-space: nowrap;'><font color='" + ColorTranslator.ToHtml(Color.FromArgb(argb1)) + "'>-{0}{1}</font>{2}</span>", WOTHelper.FormatNumberToString(Math.Abs(value), decimalPlaces), sign, MovementGif(value, reverse));
        }

        protected internal string MovementGif(double value, bool reverse)
        {
            if (UserSettings.HTMLShowMovementPics == true)
            {
                if (!reverse)
                {
                    if (value > 0)
                        return "<font face='webdings' Color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>5</font>";
                    else
                        if (value < 0)
                            return "<font face='webdings' Color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNegative)) + "'>6</font>";
                        else
                            return "<font face='webdings' Color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNeutral)) + "'>=</font>";
                }
                else
                {
                    if (value > 0)
                        return "<font face='webdings' Color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNegative)) + "'>6</font>";
                    else
                        if (value < 0)
                            return "<font face='webdings' Color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>5</font>";
                        else
                            return "<font face='webdings' Color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNeutral)) + "'>=</font>";
                }
            }
            else
                return "";
        }


     
    

        protected internal string CountryFlag(int country)
        {
#if DEBUG
            return string.Empty;
#else
            if (country == 0)
                return "bg_ussr";
            else if (country == 1)
                return "bg_germany";
            else if (country == 2)
                return "bg_usa";
            else if (country == 3)
                return "bg_china";
            else if (country == 4)
                return "bg_french";
            else if (country == 5)
                return "bg_uk";
            else
                return "bg_japan";
#endif
        }

        protected internal string CountryFlagFill(int country)
        {
#if DEBUG
            return string.Empty;
#else

            if (country == 0)
                return "bg_ussrfill";
            else if (country == 1)
                return "bg_germanyfill";
            else if (country == 2)
                return "bg_usafill";
            else if (country == 3)
                return "bg_chinafill";
            else if (country == 4)
                return "bg_frenchfill";
            else if ((country == 5))
                return "bg_ukfill";
            else
                return "bg_japanfill";
#endif
        }

        //protected internal
        public static string GetRoman(int tier)
        {
            switch (tier)
            {
                case 1:
                    return "I";
                case 2:
                    return "II";
                case 3:
                    return "III";
                case 4:
                    return "IV";
                case 5:
                    return "V";
                case 6:
                    return "VI";
                case 7:
                    return "VII";
                case 8:
                    return "VIII";
                case 9:
                    return "IX";
                case 10:
                    return "X";
                default:
                    return "O";
            }
        }

        protected internal string TankImage(int countryID, int tankID, string tankDescription)
        {
            string tankName = string.Format("{0}_{1}.png", countryID, tankID);
#if DEBUG
            return string.Empty;
#else
            return String.Format(@"<Image src='{0}' alt='{2}'/>", WOTHelper.GetImagePath(tankName), tankName, tankDescription);
#endif

        }

        protected internal string TankImageToolTip(int countryID, int tankID, string tankDescription)
        {
            string tankName = string.Format("{0}_{1}.png", countryID, tankID);
            return String.Format(@"<Image src='{0}'/>", WOTHelper.GetImagePath(tankName), tankName, tankDescription);
        }

        protected internal string TankImageLarge(int countryID, int tankID, string tankDescription)
        {
            string tankName = string.Format("{0}_{1}_Large.png", countryID, tankID);
#if DEBUG
            return string.Empty;
#else
            return String.Format(@"<Image src='{0}' alt='{2}'/>", WOTHelper.GetImagePath(tankName), tankName, tankDescription);
#endif
        }

        protected internal string MasterBadgeImage(int masterLevel)
        {
            if (File.Exists(string.Format(@"{0}\Images\MasterBadge\{1}.png", WOTHelper.GetEXEPath(), masterLevel)))
#if DEBUG
                return string.Empty;
#else
                return String.Format(@"<Image src='{0}\Images\MasterBadge\{1}.png' alt='{2}'/>", WOTHelper.GetEXEPath(), masterLevel, MasterBadgeDescription(masterLevel));
#endif
            else
                return string.Empty;

        }

        protected internal string MasterBadgeDescription(int MasterLevel)
        {
            switch (MasterLevel)
            {
                case 4:
                    return "Mastery Badge: Ace Tanker";
                case 3:
                    return "Mastery Badge: I class";
                case 2:
                    return "Mastery Badge: II class";
                case 1:
                    return "Mastery Badge: III class";
                default:
                    return "";
            }
        }
    }
}
