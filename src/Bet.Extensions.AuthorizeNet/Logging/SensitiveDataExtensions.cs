using System.Text.RegularExpressions;

namespace Bet.Extensions.AuthorizeNet.Logging
{
    public static class SensitiveDataExtensions
    {
        private static readonly SensitiveTag[] SensitiveTags = new SensitiveTag[]
        {
            new SensitiveTag("cardCode", string.Empty, "XXX", false),
            new SensitiveTag("cardNumber", "(\\p{N}+)(\\p{N}{4})", "XXXX-$2", false),
            new SensitiveTag("expirationDate", string.Empty, "XXX", false),
            new SensitiveTag("accountNumber", "(\\p{N}+)(\\p{N}{4})", "XXXX-$2", false),
            new SensitiveTag("nameOnAccount", string.Empty, "XXX", false),
            new SensitiveTag("transactionKey", string.Empty, "XXX", false)
        };

        private static readonly string[] CardPatterns = new string[]
        {
            "4\\p{N}{3}([\\ \\-]?)\\p{N}{4}\\1\\p{N}{4}\\1\\p{N}{4}",
            "4\\p{N}{3}([\\ \\-]?)(?:\\p{N}{4}\\1){2}\\p{N}(?:\\p{N}{3})?",
            "5[1-5]\\p{N}{2}([\\ \\-]?)\\p{N}{4}\\1\\p{N}{4}\\1\\p{N}{4}",
            "6(?:011|22(?:1(?=[\\ \\-]?(?:2[6-9]|[3-9]))|[2-8]|9(?=[\\ \\-]?(?:[01]|2[0-5])))|4[4-9]\\p{N}|5\\p{N}\\p{N})([\\ \\-]?)\\p{N}{4}\\1\\p{N}{4}\\1\\p{N}{4}",
            "35(?:2[89]|[3-8]\\p{N})([\\ \\-]?)\\p{N}{4}\\1\\p{N}{4}\\1\\p{N}{4}",
            "3[47]\\p{N}\\p{N}([\\ \\-]?)\\p{N}{6}\\1\\p{N}{5}"
        };

        public static string MaskCreditCards(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            for (var i = 0; i < CardPatterns.Length; i++)
            {
                value = Regex.Replace(value, CardPatterns[i], "XXXX");
            }

            return value;
        }

        public static string MaskSensitiveXmlString(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var noOfSensitiveTags = SensitiveTags.Length;
            var tagPatterns = new string[noOfSensitiveTags];
            var tagReplacements = new string[noOfSensitiveTags];

            for (var i = 0; i < noOfSensitiveTags; i++)
            {
                var tagName = SensitiveTags[i].TagName;
                var pattern = SensitiveTags[i].Pattern;
                var replacement = SensitiveTags[i].Replacement;

                if (!string.IsNullOrEmpty(pattern))
                {
                    tagPatterns[i] = "<" + tagName + ">" + pattern + "</" + tagName + ">";
                }
                else
                {
                    tagPatterns[i] = "<" + tagName + ">" + ".+" + "</" + tagName + ">";
                }

                tagReplacements[i] = "<" + tagName + ">" + replacement + "</" + tagName + ">";
            }

            for (var i = 0; i < tagPatterns.Length; i++)
            {
                value = Regex.Replace(value, tagPatterns[i], tagReplacements[i]);
            }

            return value;
        }

        public static string MaskSensitiveString(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var noOfSensitiveTags = SensitiveTags.Length;

            for (var i = 0; i < noOfSensitiveTags; i++)
            {
                var propName = SensitiveTags[i].TagName;
                var pattern = SensitiveTags[i].Pattern;
                var replacement = SensitiveTags[i].Replacement;

                if (!string.IsNullOrEmpty(pattern))
                {
                    pattern = $"\"{propName}\": \"({pattern}),\"";
                }
                else
                {
                    pattern = $"\"{propName}\": \"(.+)\"";
                }

                value = Regex.Replace(value, pattern, replacement + ",");
            }

            return value;
        }
    }
}
