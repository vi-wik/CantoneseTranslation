using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using System.Text.RegularExpressions;

namespace CantoneseTranslation.Business
{
    public class ChineseConverterHelper
    {
        public static bool HasTraditionalChar(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }

            string regexPattern = @"[\u2160-\u337f]";

            return Regex.IsMatch(content, regexPattern);
        }

        public static string TraditionalToSimplified(string content)
        {
            if(string.IsNullOrEmpty(content))
            {
                return content;
            }

            if(!HasTraditionalChar(content))
            {
                return content;
            }

            return ChineseConverter.Convert(content, ChineseConversionDirection.TraditionalToSimplified);
        }
    }
}
