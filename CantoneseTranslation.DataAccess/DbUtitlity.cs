using Microsoft.Data.Sqlite;
using System;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace CantoneseTranslation.DataAccess
{
    public class DbUtitlity
    {
        public static string DataFilePath { get; set; }

        public static SqliteConnection CreateDbConnection(string dataFilePath = null)
        {
            if (string.IsNullOrEmpty(dataFilePath))
            {
                if(string.IsNullOrEmpty(DataFilePath))
                {
                    throw new ArgumentNullException("dataFilePath can't be empty.");
                }

                dataFilePath = DataFilePath;
            }

            DbProviderFactory factory = SqliteFactory.Instance;

            SqliteConnection connection = factory.CreateConnection() as SqliteConnection;

            if (connection != null)
            {
                connection.ConnectionString = $"Data Source={dataFilePath};Mode=ReadWriteCreate;";
            }

            return connection;
        }

        public static object GetParameterValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return DBNull.Value;
            }

            return value;
        }

        public static string GetSafeValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            value = Regex.Replace(value, @";", string.Empty);
            value = Regex.Replace(value, @"'", string.Empty);
            value = Regex.Replace(value, @"&", string.Empty);
            value = Regex.Replace(value, @"%20", string.Empty);
            value = Regex.Replace(value, @"--", string.Empty);
            value = Regex.Replace(value, @"==", string.Empty);
            value = Regex.Replace(value, @"<", string.Empty);
            value = Regex.Replace(value, @">", string.Empty);
            value = Regex.Replace(value, @"%", string.Empty);

            return value;
        }
    }
}
