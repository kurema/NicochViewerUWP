using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicochViewerUWP.Storages
{
    public static class History
    {
        private static ConfigStorage.ConfigStorageItem LastPlayedIdItem = new ConfigStorage.ConfigStorageItem("LastPlayedId");

        public static string LastPlayedId
        {
            get => LastPlayedIdItem.GetString();
            set => LastPlayedIdItem.SetString(value);
        }
    }
}
