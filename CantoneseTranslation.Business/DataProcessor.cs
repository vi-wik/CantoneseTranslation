using CantoneseLearning.Business;
using CantoneseTranslation.Business.Model;
using CantoneseTranslation.DataAccess;
using CantoneseTranslation.Model;

namespace CantoneseTranslation.Business
{
    public class DataProcessor
    {
        static DataProcessor()
        {
            DbUtitlity.DataFilePath = DataFileManager.DataFilePath;
        }

        public static async Task<TranslationResult> Translate(TranslateType translateType, string content)
        {
            return await TranslateHelper.Translate(translateType, content);
        }

        public static async Task<IEnumerable<V_Mandarin2Cantonese>> GetVMandarin2Cantoneses()
        {
            return await DbObjectsFetcher.GetVMandarin2Cantoneses();
        }

        public static async Task<IEnumerable<V_CantoneseExample>> GetVCantoneseExamples()
        {
            return await DbObjectsFetcher.GetVCantoneseExamples();
        }

        public static async Task<IEnumerable<CantoneseSynonym>> GetCantoneseSynonyms()
        {
            return await DbObjectsFetcher.GetCantoneseSynonyms();
        }

        public static async Task<IEnumerable<CantoneseSentencePattern>> GetCantoneseSentencePatterns()
        {
            return await DbObjectsFetcher.GetCantoneseSentencePatterns();
        }
    }
}
