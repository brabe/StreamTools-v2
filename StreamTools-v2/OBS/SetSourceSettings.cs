using Newtonsoft.Json.Linq;
using OBSWebsocketDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamTools_v2.OBS
{
    partial class Functions
    {
        public static void setSourceSettings(OBSWebsocket obs, string sourceName, string file)
        {
            JObject sourceSettings = new JObject();
            sourceSettings.Add("file", file);

            JObject requestObject = new JObject();
            requestObject.Add("sourceName", sourceName);
            requestObject.Add("sourceSettings", sourceSettings);

            obs.SendRequest("SetSourceSettings", requestObject);
        }
    }
}
