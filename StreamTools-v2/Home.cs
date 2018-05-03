using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OBSWebsocketDotNet;
using StreamTools_v2.OBS;
using System.Data.SqlClient;
using System.ComponentModel;
using System.IO;
using StreamDeckSharp;
using System.Drawing.Imaging;

namespace StreamTools_v2
{
    public partial class home_FRM : Form
    {
        protected OBSWebsocket _obs;

        public home_FRM()
        {
            InitializeComponent();

            // OBS WebSocket
            _obs = new OBSWebsocket();

            _obs.Connected += WebSocket.onConnect;
            _obs.Disconnected += WebSocket.onDisconnect;

            _obs.SceneChanged += WebSocket.onSceneChange;
            _obs.SceneCollectionChanged += WebSocket.onSceneColChange;
            _obs.ProfileChanged += WebSocket.onProfileChange;
            _obs.TransitionChanged += WebSocket.onTransitionChange;
            _obs.TransitionDurationChanged += WebSocket.onTransitionDurationChange;

            _obs.StreamingStateChanged += WebSocket.onStreamingStateChange;
            _obs.RecordingStateChanged += WebSocket.onRecordingStateChange;

            // Setup Environment
            Global.applicationSetup();

            // Refresh Music Playlist
            musicRefresh_BTN.PerformClick();
        }

        

        public void connectOBS(object sender, EventArgs e)
        {
            string ipAddress;
            string password;
            string port;

            string sqlQuery =
                "SELECT * " +
                "FROM connectionInformation " +
                "WHERE id='1'";

            //SqlDataAdapter sda = new SqlDataAdapter(sqlQuery, Global.con);
            DataTable connectDT = new DataTable();
            //sda.Fill(connectDT);

            DataRow connectDR = connectDT.Rows[0];

            ipAddress = connectDR["ipAddress"].ToString();
            password = connectDR["password"].ToString();
            port = connectDR["port"].ToString();

            if (!_obs.IsConnected)
            {
                try
                {
                    _obs.Connect("ws://" + ipAddress + ":" + port + "", password);

                    connectionNotification_Label.Text = "Connected (" + ipAddress + ")";
                    connectionNotification_Label.ForeColor = Color.Green;

                    connectOBS_Button.Text = "Disconnect OBS";
                }
                catch (AuthFailureException)
                {
                    connectionNotification_Label.Text = "Authentication Failed!!!";
                    connectionNotification_Label.ForeColor = Color.Red;
                    MessageBox.Show("Authentication failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (ErrorResponseException ex)
                {
                    connectionNotification_Label.Text = "Connection Failed!!!";
                    connectionNotification_Label.ForeColor = Color.Red;
                    MessageBox.Show("Connect failed : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else
            {
                _obs.Disconnect();

                connectionNotification_Label.Text = "Disconnected";
                connectionNotification_Label.ForeColor = Color.Red;

                connectOBS_Button.Text = "Connect OBS";
            }
        }

        #region Music Player
        List<string> musicPlaylist = new List<string>();
        bool playing = false;

        WMPLib.WindowsMediaPlayer musicPlayer = new WMPLib.WindowsMediaPlayer();

        private void musicPlay_BTN_Click(object sender, EventArgs e)
        {
            if (playing == false)
            {
                musicPlayer.controls.play();
                musicPlayer_ItemUpdate();
                playing = true;

                musicPlay_BTN.Text = "Pause";
                return;
            }

            if (playing == true)
            {
                musicPlayer.controls.pause();
                musicArtist_LBL.Text = "";
                musicTrackName_LBL.Text = "";

                playing = false;

                musicPlay_BTN.Text = "Play";
                return;
            }
        }

        private void musicPrev_BTN_Click(object sender, EventArgs e)
        {
            musicPlayer.controls.previous();
            musicPlayer_ItemUpdate();
        }

        private void musicNext_BTN_Click(object sender, EventArgs e)
        {
            musicPlayer.controls.next();
            musicPlayer_ItemUpdate();
        }

        private void musicRefresh_BTN_Click(object sender, EventArgs e)
        {
            if (playing == true) return;

            musicPlayer.controls.stop();

            WMPLib.IWMPPlaylist playlist = musicPlayer.currentPlaylist;

            try
            {
                foreach (string file in Directory.GetFiles(Global.musicLocation, "*.mp3", SearchOption.TopDirectoryOnly))
                {
                    playlist.appendItem(musicPlayer.newMedia(file));
                }

                musicPlayer.currentPlaylist = playlist;
                musicPlayer.settings.setMode("loop", true);
                musicPlayer.settings.setMode("shuffle", true);

                musicPlayer.controls.stop();
            }
            catch { }
            

            musicPlaylist.Clear();
            musicPlaylist_LB.DataSource = null;

            foreach (string file in Directory.GetFiles(Global.musicLocation, "*.mp3", SearchOption.TopDirectoryOnly))
            {
                string modified = file;

                modified = modified.Replace(".mp3", "");
                modified = modified.Replace(Global.musicLocation + @"\", "");

                musicPlaylist.Add(modified);
            }

            musicPlaylist_LB.DataSource = musicPlaylist;

            musicPlay_BTN.Enabled = true;
            musicPlay_BTN.Visible = true;

            musicNext_BTN.Enabled = true;
            musicNext_BTN.Visible = true;

            musicPrev_BTN.Enabled = true;
            musicPrev_BTN.Visible = true;
        }

        private void musicPlayer_ItemUpdate()
        {
            try
            {
                TagLib.File file = TagLib.File.Create(musicPlayer.currentMedia.sourceURL);

                musicTrackName_LBL.Text = file.Tag.Title;

                musicArtist_LBL.Text = file.Tag.FirstPerformer;

                var bin = (byte[])(file.Tag.Pictures[0].Data.Data);
                MemoryStream ms = new MemoryStream(bin);
                pictureBox3.Image = Image.FromStream(ms);

                // SAVE COVER TO DISC
                try
                {
                    File.Delete(Global.musicLocation + @"\export\cover.png");
                }
                catch { }
                FileStream fsArt = new FileStream(Global.musicLocation + @"\export\cover.png", FileMode.Create, FileAccess.Write);
                ms.WriteTo(fsArt);
                fsArt.Close();
                
                // SAVE TRACK NAME TO DISC
                try
                {
                    File.Delete(Global.musicLocation + @"\export\name.txt");
                }
                catch { }
                
                FileStream fsName = new FileStream(Global.musicLocation + @"\export\name.txt", FileMode.Create, FileAccess.Write);
                fsName.Write(new UTF8Encoding(true).GetBytes(musicTrackName_LBL.Text), 0, new UTF8Encoding(true).GetBytes(musicTrackName_LBL.Text).Length);
                fsName.Close();

                FileStream fsArtist = new FileStream(Global.musicLocation + @"\export\artist.txt", FileMode.Create, FileAccess.Write);
                fsArtist.Write(new UTF8Encoding(true).GetBytes(musicArtist_LBL.Text), 0, new UTF8Encoding(true).GetBytes(musicArtist_LBL.Text).Length);
                fsArtist.Close();
            }
            catch
            {

            }
            
        }
        #endregion

        #region Elgato StreamDeck
        IStreamDeck deck = StreamDeck.OpenDevice();
        string deckScreen = "home";
        bool leavingScreen = false;
        string screenLeft = "";

        private void elgatoStreamDeck(home_FRM form)
        {
            deck.KeyStateChanged += StreamDeck_KeyHandler;
            elgato_SetHomeScreen();
        }

        private void elgato_SetHomeScreen()
        {
            deck.ClearKeys();

            var bitmap6 = KeyBitmap.FromFile(@"CustomDeck\gameFolder.png");
            deck.SetKeyBitmap(6, bitmap6);

            var bitmap7 = KeyBitmap.FromFile(@"CustomDeck\obsFolder.png");
            deck.SetKeyBitmap(7, bitmap7);

            var bitmap8 = KeyBitmap.FromFile(@"CustomDeck\musicFolder.png");
            deck.SetKeyBitmap(8, bitmap8);

            deckScreen = "home";
        }

        private void elgato_SetGameHome()
        {
            deck.ClearKeys();

            var bit0 = KeyBitmap.FromFile(@"CustomDeck\right-3.png");
            deck.SetKeyBitmap(0, bit0);

            var bit1 = KeyBitmap.FromFile(@"CustomDeck\right3.png");
            deck.SetKeyBitmap(1, bit1);

            var bit2 = KeyBitmap.FromFile(@"CustomDeck\timeouts.png");
            deck.SetKeyBitmap(2, bit2);

            var bit3 = KeyBitmap.FromFile(@"CustomDeck\left-3.png");
            deck.SetKeyBitmap(3, bit3);

            var bit4 = KeyBitmap.FromFile(@"CustomDeck\left3.png");
            deck.SetKeyBitmap(4, bit4);

            var bit5 = KeyBitmap.FromFile(@"CustomDeck\right-2.png");
            deck.SetKeyBitmap(5, bit5);

            var bit6 = KeyBitmap.FromFile(@"CustomDeck\right2.png");
            deck.SetKeyBitmap(6, bit6);

            var bit7 = KeyBitmap.FromFile(@"CustomDeck\goBack.png");
            deck.SetKeyBitmap(7, bit7);

            var bit8 = KeyBitmap.FromFile(@"CustomDeck\left-2.png");
            deck.SetKeyBitmap(8, bit8);

            var bit9 = KeyBitmap.FromFile(@"CustomDeck\left2.png");
            deck.SetKeyBitmap(9, bit9);

            var bit10 = KeyBitmap.FromFile(@"CustomDeck\right-1.png");
            deck.SetKeyBitmap(10, bit10);

            var bit11 = KeyBitmap.FromFile(@"CustomDeck\right1.png");
            deck.SetKeyBitmap(11, bit11);

            var bit12 = KeyBitmap.FromFile(@"CustomDeck\timeClock.png");
            deck.SetKeyBitmap(12, bit12);

            var bit13 = KeyBitmap.FromFile(@"CustomDeck\left-1.png");
            deck.SetKeyBitmap(13, bit13);

            var bit14 = KeyBitmap.FromFile(@"CustomDeck\left1.png");
            deck.SetKeyBitmap(14, bit14);



            deckScreen = "game";
        }

        private void elgato_SetMusicHome()
        {
            deck.ClearKeys();

            var bit4 = KeyBitmap.FromFile(@"CustomDeck\goBack.png");
            deck.SetKeyBitmap(4, bit4);

            var bit6 = KeyBitmap.FromFile(@"CustomDeck\musicNext.png");
            deck.SetKeyBitmap(6, bit6);

            if (playing == true)
            {
                var bit7 = KeyBitmap.FromFile(@"CustomDeck\musicPause.png");
                deck.SetKeyBitmap(7, bit7);
            }
            else
            {
                var bit7 = KeyBitmap.FromFile(@"CustomDeck\musicPlay.png");
                deck.SetKeyBitmap(7, bit7);
            }

            var bit8 = KeyBitmap.FromFile(@"CustomDeck\musicPrev.png");
            deck.SetKeyBitmap(8, bit8);

            var bit12 = KeyBitmap.FromFile(@"CustomDeck\musicRefresh.png");
            deck.SetKeyBitmap(12, bit12);

            deckScreen = "music";
        }

        private void elgato_SetOBSHome()
        {

        }

        private void StreamDeck_KeyHandler(object sender, StreamDeckSharp.KeyEventArgs e)
        {
            var deck = sender as IStreamDeck;
            if (deck == null) return;

            if (e.IsDown)
            {
                if (deckScreen == "home")
                {
                    if (leavingScreen == true)
                    {
                        leavingScreen = false;
                        return;
                    }

                    switch (e.Key)
                    {
                        // GAME FOLDER
                        case 6:
                            elgato_SetGameHome();

                            leavingScreen = true;
                            screenLeft = "home";
                            break;
                        // OBS FOLDER
                        case 7:
                            elgato_SetOBSHome();

                            leavingScreen = true;
                            screenLeft = "home";
                            break;
                        // MUSIC FOLDER
                        case 8:
                            elgato_SetMusicHome();

                            leavingScreen = true;
                            screenLeft = "home";
                            break;
                        default:
                            elgato_SetHomeScreen();
                            break;
                    }
                }

                if (deckScreen == "music")
                {
                    if (leavingScreen == true)
                    {
                        leavingScreen = false;
                        return;
                    }

                    switch (e.Key)
                    {
                        // GO BACK
                        case 4:
                            if (screenLeft == "home")
                            {
                                elgato_SetHomeScreen();
                                screenLeft = "music";
                            }
                            break;
                        // NEXT
                        case 6:
                            if (musicPlaylist.Count == 0) return;

                            this.InvokeEx(home_FRM => home_FRM.musicNext_BTN.PerformClick());
                            break;
                        // PLAY / PAUSE
                        case 7:
                            if (musicPlaylist.Count == 0) return;

                            if (playing == false)
                            {
                                var bitmap0 = KeyBitmap.FromFile(@"CustomDeck\musicPause.png");
                                deck.SetKeyBitmap(7, bitmap0);
                            }

                            if (playing == true)
                            {
                                var bitmap0 = KeyBitmap.FromFile(@"CustomDeck\musicPlay.png");
                                deck.SetKeyBitmap(7, bitmap0);
                            }

                            this.InvokeEx(home_FRM => home_FRM.musicPlay_BTN.PerformClick());
                            break;
                        // PREV
                        case 8:
                            if (musicPlaylist.Count == 0) return;

                            this.InvokeEx(home_FRM => home_FRM.musicPrev_BTN.PerformClick());
                            break;
                        // REFRESH
                        case 12:
                            this.InvokeEx(home_FRM => home_FRM.musicRefresh_BTN.PerformClick());
                            break;
                        default:
                            break;
                    }
                }

                if (deckScreen == "obs")
                {
                    if (leavingScreen == true)
                    {
                        leavingScreen = false;
                        return;
                    }
                }

                if (deckScreen == "game")
                {
                    if (leavingScreen == true)
                    {
                        leavingScreen = false;
                        return;
                    }

                    switch (e.Key)
                    {
                        // GO BACK
                        case 7:
                            if (screenLeft == "home")
                            {
                                elgato_SetHomeScreen();
                                screenLeft = "game";
                            }
                            break;

                        default:
                            break;
                    }
                }
            }

            if (e.Key == 0 && e.IsDown)
            {
                this.InvokeEx(home_FRM => home_FRM.musicPlay_BTN.PerformClick());
            }

            if (e.Key == 1)
            {

            }

            if (e.Key == 2)
            {

            }

            if (e.Key == 5)
            {

            }
        }
        #endregion

        private void home_FRM_Load(object sender, EventArgs e)
        {
            elgatoStreamDeck(this);

            musicRefresh_BTN.PerformClick();
        }

        private void home_FRM_FormClosing(object sender, FormClosingEventArgs e)
        {
            deck.ShowLogo();
        }
    }

    public static class ISynchronizeInvokeExtensions
    {
        public static void InvokeEx<T>(this T @this, Action<T> action) where T : ISynchronizeInvoke
        {
            if (@this.InvokeRequired)
            {
                @this.Invoke(action, new object[] { @this });
            }
            else
            {
                action(@this);
            }
        }
    }
}
