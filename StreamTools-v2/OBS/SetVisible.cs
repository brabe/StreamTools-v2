using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OBSWebsocketDotNet;

namespace StreamTools_v2.OBS
{
    partial class OBS
    {
        public static void setVisible(OBSWebsocket obs, string sourceName, string sceneName, bool visible)
        {
            JObject requestObject = new JObject();
            requestObject.Add("scene-name", sceneName);
            requestObject.Add("item", sourceName);
            requestObject.Add("visible", visible);

            obs.SendRequest("SetSceneItemProperties", requestObject);
        }
    }
}
