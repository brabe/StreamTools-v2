using OBSWebsocketDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamTools_v2.OBS
{
    class Functions
    {
        public static void listOBS_Scenes(OBSWebsocket _obs)
        {
            if (_obs.IsConnected)
            {
                try
                {
                    Console.WriteLine(_obs.ListScenes().ToString());
                }
                catch (ErrorResponseException ex)
                {
                    MessageBox.Show("Failed to list scenes." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        public static void toggleStudioMode(OBSWebsocket _obs)
        {
            if (_obs.IsConnected)
            {
                try
                {
                    if(!_obs.StudioModeEnabled())
                    {
                        _obs.ToggleStudioMode();
                    }
                }
                catch (ErrorResponseException ex)
                {
                    MessageBox.Show("Failed to toggle studio mode." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
    }
}
