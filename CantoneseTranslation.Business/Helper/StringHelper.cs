namespace CantoneseTranslation.Business.Helper
{
    public class StringHelper
    {
        public static bool IsChineseChar(char ch)
        {
            if (ch >= 0x4E00 && ch <= 0x9FFF)
            {
                return true;
            }

            return false;
        }
    }
}
