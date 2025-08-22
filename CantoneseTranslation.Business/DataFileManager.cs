using System.Reflection;
using viwik.CantoneseLearning.Data;
using viwik.CantoneseLearning.DataAccess;

namespace viwik.CantoneseTranslation.Business
{
    public class DataFileManager
    {
        private static string dbFolder => "data";
        private readonly static string dataFileName = "cantonese.db3";
        private readonly static string dbVersionFileName = "DbVersion.txt";

        internal static string DataFilePath
        {
            get
            {
                return Path.Combine(dbFolder, dataFileName);
            }
        }     

        static DataFileManager()
        {
            DbUtitlity.DataFilePath = DataFilePath;          
        }

        public async static void Init()
        {
            if (!Directory.Exists(dbFolder))
            {
                Directory.CreateDirectory(dbFolder);
            }

            string dbFilePath = DataFilePath;
            string dbVersionFilePath = Path.Combine(dbFolder, dbVersionFileName);

            bool hasDbFile = File.Exists(dbFilePath);
            bool hasDbVersionFile = File.Exists(dbVersionFilePath);

            bool dbCopied = false;
#if DEBUG
            await CopyDbFileAsync(dbFilePath);
            dbCopied = true;
#endif

            if (!hasDbFile && !dbCopied)
            {
                await CopyDbFileAsync(dbFilePath);
                dbCopied = true;
            }

            var dbVersion = GetDatabaseVersion();

            if (!hasDbVersionFile)
            {
                File.WriteAllText(dbVersionFilePath, dbVersion);
            }

            if (hasDbVersionFile)
            {
                string oldDbVersion = File.ReadAllText(dbVersionFilePath);

                if (dbVersion != oldDbVersion)
                {
                    if (!dbCopied)
                    {
                        await CopyDbFileAsync(dbFilePath);
                    }

                    File.WriteAllText(dbVersionFilePath, dbVersion);
                }
            }
        }

        private static Assembly GetDataAssembly()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(CantoneseDataHook));

            return assembly;
        }

        private static AssemblyName GetDataAssemblyName()
        {
            Assembly assembly = GetDataAssembly();

            var assemblyName = assembly.GetName();

            return assemblyName;
        }

        private static string GetDatabaseVersion()
        {
            string version = GetDataAssemblyName().Version.ToString();

            return version;
        }

        private static async Task CopyDbFileAsync(string targetFilePath)
        {
            Assembly assembly = GetDataAssembly();

            var assemblyName = assembly.GetName().Name;

            string resourceName = $"{assemblyName}.{dataFileName}";

            using (Stream fs = assembly.GetManifestResourceStream(resourceName))
            {
                if (fs != null)
                {
                    using (FileStream target = new FileStream(targetFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        StreamWriter writer = new StreamWriter(target);

                        fs.CopyTo(target);

                        writer.Flush();

                        fs.Close();
                    }                   
                }
            }
        }
    }
}
