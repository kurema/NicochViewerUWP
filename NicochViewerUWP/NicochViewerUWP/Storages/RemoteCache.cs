using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicochViewerUWP.Storages
{
    public static class RemoteCache
    {
        public static Json.NicochInfo NicochInfo { get; set; }

        public static async Task<Json.NicochInfo> GetNicochInfoAsync() => NicochInfo = NicochInfo ?? await Json.Loader.GetNicochInfo(Storages.ConfigStorage.ServerUrl);
        public static async Task<Json.NicochInfo> UpdateNicochInfoAsync() => NicochInfo = await Json.Loader.GetNicochInfo(Storages.ConfigStorage.ServerUrl);
    }
}
