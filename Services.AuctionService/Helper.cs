using System;

namespace Services.AuctionService
{
    static class Helper
    {
        public static string GetIntPart(this string text, string stopAt = "-")
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            return charLocation > 0 ? text.Substring(0, charLocation) : text;
        }
    }
}