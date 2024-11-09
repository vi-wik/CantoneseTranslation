using CantoneseTranslation.Model;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantoneseTranslation.DataAccess
{
    public class DbObjectsFetcher
    {

        public static async Task<IEnumerable<V_Mandarin2Cantonese>> GetVMandarin2Cantoneses()
        {
            string sql = "select * from v_Mandarin2Cantonese";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<V_Mandarin2Cantonese>(sql));
            }
        }

        public static async Task<IEnumerable<V_CantoneseExample>> GetVCantoneseExamples()
        {
            string sql = "select * from v_CantoneseExamples";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<V_CantoneseExample>(sql));
            }
        }

        public static async Task<IEnumerable<CantoneseSynonym>> GetCantoneseSynonyms()
        {
            string sql = "select * from CantoneseSynonym";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<CantoneseSynonym>(sql));
            }
        }

        public static async Task<IEnumerable<CantoneseSentencePattern>> GetCantoneseSentencePatterns()
        {
            string sql = "select * from CantoneseSentencePattern";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<CantoneseSentencePattern>(sql));
            }
        }
    }
}
