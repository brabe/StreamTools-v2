using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OBSWebsocketDotNet;

namespace StreamTools_v2.OBS
{
    class WebSocket
    {
        public static void onConnect(object sender, EventArgs e)
        {

        }

        public static void onDisconnect(object sender, EventArgs e)
        {

        }

        public static void onSceneChange(OBSWebsocket sender, string newSceneName)
        {

        }

        public static void onSceneColChange(object sender, EventArgs e)
        {

        }

        public static void onProfileChange(object sender, EventArgs e)
        {

        }

        public static void onTransitionChange(OBSWebsocket sender, string newTransitionNam)
        {

        }

        public static void onTransitionDurationChange(OBSWebsocket sender, int newDuration)
        {

        }

        public static void onStreamingStateChange(OBSWebsocket sender, OutputState newState)
        {

        }

        public static void onRecordingStateChange(OBSWebsocket sender, OutputState newState)
        {

        }

        public static void onStreamData(OBSWebsocket sender, StreamStatus data)
        {

        }
    }
}
