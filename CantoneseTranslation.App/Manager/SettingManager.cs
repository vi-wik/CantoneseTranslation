using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CantoneseTranslation.App.Manager
{
    internal class SettingManager
    {
        internal static readonly string ConfigFolder = "Config";
        internal static readonly string ReservedFileName = "Reserved.txt";

        internal static string ReservedFilePath
        {
            get { return Path.Combine(ConfigFolder, ReservedFileName); }
        }
    }
}
