using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

namespace NicochViewerUWP.Storages
{
    public static class ConfigStorage
    {
        public static ApplicationDataContainer LocalSettings => Windows.Storage.ApplicationData.Current.LocalSettings;

        private static ConfigStorageItem ServerUrlItem = new ConfigStorageItem("ServerUrl");
        public static string ServerUrl
        {
            get => ServerUrlItem.GetString();
            set => ServerUrlItem.SetString(value);
        }

        public class ConfigStorageItem
        {
            public string Key { get; private set; }

            public ConfigStorageItem(string key)
            {
                Key = key ?? throw new ArgumentNullException(nameof(key));
            }

            public string GetString() => LocalSettings.Values[Key]?.ToString();
            public void SetString(string value) => LocalSettings.Values[Key] = value;
        }
    }
}
