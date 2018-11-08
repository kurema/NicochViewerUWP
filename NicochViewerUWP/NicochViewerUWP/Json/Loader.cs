using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization.Json;
using System.Net.Http;

namespace NicochViewerUWP.Json
{
    public class Loader
    {
        public static async Task<Stream> LoadHttp(string Path)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync(System.IO.Path.Combine(Path, "api.cgi"));
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return await result.Content.ReadAsStreamAsync();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch { return null; }
            }
        }

        public static async Task<NicochInfo> GetNicochInfo(string Path)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(NicochInfo));
            var stream = await LoadHttp(Path);
            if (stream == null) return null;
            var result = jsonSerializer.ReadObject(stream) as NicochInfo;
            return result;
        }
    }
}
