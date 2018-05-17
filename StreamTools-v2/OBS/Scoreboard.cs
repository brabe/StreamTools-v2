using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OBSWebsocketDotNet;

namespace StreamTools_v2.OBS
{
    class Scoreboard
    {
        public static void updateBackgroundGraphics(OBSWebsocket obs)
        {
            Functions.setSourceSettings(obs, "TimeClockBG", Global.obsMainExport + @"\ScoreboardBaseGraphics\Backgrounds\TimeClock.png");
        }

        public static void updateTimeClock(string time, OBSWebsocket obs)
        {
            JObject requestObject = new JObject();
            requestObject.Add("source", "TimeClockText");
            requestObject.Add("text", time);

            obs.SendRequest("SetTextGDIPlusProperties", requestObject);
        }

        public static void updateQuarter(string quarter, OBSWebsocket obs)
        {
            JObject requestObject = new JObject();
            requestObject.Add("source", "QuarterText");
            requestObject.Add("text", quarter);

            obs.SendRequest("SetTextGDIPlusProperties", requestObject);
        }

        public static void updateLeftTeam(string leftTeam, OBSWebsocket obs)
        {
            JObject fileObject = new JObject();
            fileObject.Add("file", leftTeam);

            JObject requestObject = new JObject();
            requestObject.Add("sourceName", "LeftTeamGraphic");
            requestObject.Add("sourceSettings", fileObject);

            obs.SendRequest("SetSourceSettings", requestObject);
        }

        public static void updateLeftTimeout(int total, OBSWebsocket obs)
        {
            JObject fileObject = new JObject();
            switch (total)
            {
                case 0:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout5.png");
                    break;
                case 1:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout1.png");
                    break;
                case 2:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout2.png");
                    break;
                case 3:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout3.png");
                    break;
                case 4:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout4.png");
                    break;
                case 5:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout5.png");
                    break;
                default:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout5.png");
                    break;
            }

            JObject requestObject = new JObject();
            requestObject.Add("sourceName", "LeftTimeoutGraphic");
            requestObject.Add("sourceSettings", fileObject);

            obs.SendRequest("SetSourceSettings", requestObject);

            if (total == 0)
            {
                OBS.setVisible(obs, "LeftTimeoutGraphic", "SETUP-SCOREBUG", false);
            }

            if (total > 0)
            {
                OBS.setVisible(obs, "LeftTimeoutGraphic", "SETUP-SCOREBUG", true);
            }
        }

        public static void updateLeftScore(string score, OBSWebsocket obs)
        {
            JObject requestObject = new JObject();
            requestObject.Add("source", "LeftScore");
            requestObject.Add("text", score);

            obs.SendRequest("SetTextGDIPlusProperties", requestObject);
        }

        public static void updateRightTeam(string rightTeam, OBSWebsocket obs)
        {
            JObject fileObject = new JObject();
            fileObject.Add("file", rightTeam);

            JObject requestObject = new JObject();
            requestObject.Add("sourceName", "RightTeamGraphic");
            requestObject.Add("sourceSettings", fileObject);

            obs.SendRequest("SetSourceSettings", requestObject);
        }

        public static void updateRightTimeout(int total, OBSWebsocket obs)
        {
            JObject fileObject = new JObject();
            switch (total)
            {
                case 0:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout5.png");
                    break;
                case 1:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout1.png");
                    break;
                case 2:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout2.png");
                    break;
                case 3:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout3.png");
                    break;
                case 4:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout4.png");
                    break;
                case 5:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout5.png");
                    break;
                default:
                    fileObject.Add("file", Global.obsMainExport + @"\ScoreboardBaseGraphics\TimeoutGraphics\Timeout5.png");
                    break;
            }

            JObject requestObject = new JObject();
            requestObject.Add("sourceName", "RightTimeoutGraphic");
            requestObject.Add("sourceSettings", fileObject);

            obs.SendRequest("SetSourceSettings", requestObject);

            if (total == 0)
            {
                OBS.setVisible(obs, "RightTimeoutGraphic", "SETUP-SCOREBUG", false);
            }

            if (total > 0)
            {
                OBS.setVisible(obs, "RightTimeoutGraphic", "SETUP-SCOREBUG", true);
            }
        }

        public static void updateRightScore (string score, OBSWebsocket obs)
        {

        }
    }
}
