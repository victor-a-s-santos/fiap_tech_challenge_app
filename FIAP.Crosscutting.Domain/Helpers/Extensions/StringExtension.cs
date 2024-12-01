using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FIAP.Crosscutting.Domain.Helpers.Extensions
{
    public static class StringExtension
    {
        public static string FormatToHiddenEmail(this string email)
        {
            string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
            string result = Regex.Replace(email, pattern, m => new string('*', m.Length));

            return result;
        }

        public static string FirstCharToUpper(this string input)
        {
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} não pode ser vazio", nameof(input)),
                _ => input.First().ToString().ToUpper() + input[1..],
            };
        }

        public static string ToLowerFormat(this string text)
        {
            if (!string.IsNullOrEmpty(text))
                return text.ToLower();

            return null;
        }

        public static string Capitalize(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.ToLower();

                TextInfo textInfo = new CultureInfo("pt-BR", false).TextInfo;

                string[] words = text.Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Length > 3 || i == 0)
                        words[i] = textInfo.ToTitleCase(words[i]);
                }

                return string.Join(" ", words);
            }

            return null;
        }

        public static string ToModelFormat(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string[] words = text.Split('_');

                for (int i = 0; i < words.Length; i++)
                    words[i] = words[i].FirstCharToUpper();

                return string.Join("", words);
            }

            return null;
        }

        public static string ToSlugName(this string text)
        {
            var normalText = text;

            var accentedCharacters = new Dictionary<char, char[]>
            {
                { 'a', new[] { 'à', 'á', 'ä', 'â', 'ã' } },
                { 'A', new[] { 'À', 'Á', 'Ä', 'Â', 'Ã' } },
                { 'c', new[] { 'ç' } },
                { 'C', new[] { 'Ç' } },
                { 'e', new[] { 'è', 'é', 'ë', 'ê' } },
                { 'E', new[] { 'È', 'É', 'Ë', 'Ê' } },
                { 'i', new[] { 'ì', 'í', 'ï', 'î' } },
                { 'I', new[] { 'Ì', 'Í', 'Ï', 'Î' } },
                { 'o', new[] { 'ò', 'ó', 'ö', 'ô', 'õ' } },
                { 'O', new[] { 'Ò', 'Ó', 'Ö', 'Ô', 'Õ' } },
                { 'u', new[] { 'ù', 'ú', 'ü', 'û' } },
                { 'U', new[] { 'Ù', 'Ú', 'Ü', 'Û' } }
            };

            normalText = accentedCharacters.Keys.Aggregate(normalText, (x, key)
                => accentedCharacters[key].Aggregate(x, (y, character) => y.Replace(character, key)));

            normalText = Regex.Replace(normalText, "[^0-9a-zA-Z._ ]+?", "");

            var sbReturn = new StringBuilder();
            var arrayText = normalText.Normalize(NormalizationForm.FormD).ToCharArray();

            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            normalText = sbReturn.ToString();
            normalText = normalText.Replace(" ", "-");

            return normalText.ToLower();
        }

        public static string GenerateToken(int size, bool onlyNumbers = false)
        {
            if (!onlyNumbers)
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var random = new Random();
                var result = new string(
                    Enumerable.Repeat(chars, size)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());

                string token = string.Concat(result,
                    Regex.Replace(DateTime.Now.ToString(), @"(\s)|(\/)|(\:)", ""));

                return token;
            }
            else
            {
                var chars = "0123456789";

                var random = new Random();
                var token = new string(
                    Enumerable.Repeat(chars, size)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());

                return token;
            }
        }

        public static bool IsJson(this string source)
        {
            if (source == null)
                return false;

            try
            {
                JsonDocument.Parse(source);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}
