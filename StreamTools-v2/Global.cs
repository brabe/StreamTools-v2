using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace StreamTools_v2
{
    class Global
    {
        // SQLite Connection for Global Program Usage
        public static SQLiteConnection con = new SQLiteConnection("Data Source='Settings.db';Version=3;");

        // Application Working Directory
        public static string workingDir = Environment.CurrentDirectory;

        public static string obsMainExport = workingDir + @"\OBS_Exports";
        public static string obsTimerExport = obsMainExport + @"\Timer";
        public static string obsScoreExport = obsMainExport + @"\Score";
        public static string obsTeamExport = obsMainExport + @"\TeamInfo";
        public static string musicLocation = workingDir + @"\Music";

        // Application Structure
        public static void applicationSetup()
        {

            // CREATE ALL OF THE EXPORT DIRECTORIES IF THEY DO NOT ALREADY EXIST
            try
            {
                Directory.CreateDirectory(obsMainExport);
            }
            catch { }

            try
            {
                Directory.CreateDirectory(obsTimerExport);
            }
            catch { }

            try
            {
                Directory.CreateDirectory(obsScoreExport);
            }
            catch { }

            try
            {
                Directory.CreateDirectory(obsTeamExport);
            }
            catch { }

            try
            {
                Directory.CreateDirectory(musicLocation);
            }
            catch { }

            // CREATE SQLITE DATABASE
            try
            {
                // FOR DEVELOPMENT PURPOSES // DELETE DATABASE ON SETUP
                try
                {
                    File.Delete(workingDir + @"\Settings.db");
                    Console.WriteLine("Database Deleted!");
                }
                catch { }

                SQLiteConnection.CreateFile("Settings.db");
                Console.WriteLine("Database Created");
                createSQLTables();
                Console.WriteLine("Database Tables Created");
            }
            catch { }
        }

        public static void createSQLTables()
        {
            // CREATE SQLITE DATABASE TABLES
            try
            {
                // OPEN DATABASE CONNECTIONS
                Global.con.Open();

                // CREATE AVAILABLE SCHOOLS TABLE
                string schools_SQL =
                    @"CREATE TABLE `availableSchools` ( `id` INTEGER NOT NULL, `name` TEXT NOT NULL, `mascot` TEXT NOT NULL, `boysName` TEXT NOT NULL, `girlsName` TEXT NOT NULL, `city` TEXT NOT NULL, `color` TEXT NOT NULL, PRIMARY KEY(`id`) )";
                SQLiteCommand schools_CMD = new SQLiteCommand(schools_SQL, Global.con);
                schools_CMD.ExecuteNonQuery();

                // CREATE CURRENT GAME TABLE
                string game_SQL =
                    @"CREATE TABLE `currentGame` ( `team` TEXT NOT NULL, `name` TEXT NOT NULL, `score` TEXT NOT NULL, `timeouts` TEXT NOT NULL, `fouls` TEXT NOT NULL, PRIMARY KEY(`team`) )";
                SQLiteCommand game_CMD = new SQLiteCommand(game_SQL, Global.con);
                game_CMD.ExecuteNonQuery();

                // CREATE APPLICATION SETTINGS TABLE
                string obsSettings_SQL =
                    @"CREATE TABLE `obsSettings` ( `obs_IP` TEXT NOT NULL, `obs_PORT` TEXT NOT NULL, `obs_USERNAME` TEXT NOT NULL, `obs_PASSWORD` TEXT NOT NULL )";
                SQLiteCommand obsSettings_CMD = new SQLiteCommand(obsSettings_SQL, Global.con);
                obsSettings_CMD.ExecuteNonQuery();

                // CLOSE DATABASE CONNECTIONS
                Global.con.Close();
            }
            catch { }
        }
    }

    public class TextBoxConsole : TextWriter
    {
        TextBox output = null;

        public TextBoxConsole(TextBox _output)
        {
            output = _output;
            output.ScrollBars = ScrollBars.Both;
            output.WordWrap = true;
        }

        public override void Write(char value)
        {
            base.Write(value);
            output.AppendText(value.ToString());
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
