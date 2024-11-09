namespace CantoneseTranslation.Business
{
    public class DataFileManager
    {
        private readonly static string dataFileName = "language.db3";

        internal static string DataFilePath
        {
            get
            {
                return dataFileName;
            }
        }

        static DataFileManager()
        {

        }
    }
}
