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
using System.Data.SQLite;
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

        private void elgato_SetTimeClock()
        {
            deck.ClearKeys();

            var bit0 = KeyBitmap.FromFile(@"CustomDeck\time0.png");
            deck.SetKeyBitmap(0, bit0);

            var bit1 = KeyBitmap.FromFile(@"CustomDeck\time0.png");
            deck.SetKeyBitmap(1, bit1);

            var bit2 = KeyBitmap.FromFile(@"CustomDeck\timeColon.png");
            deck.SetKeyBitmap(2, bit2);

            var bit3 = KeyBitmap.FromFile(@"CustomDeck\time0.png");
            deck.SetKeyBitmap(3, bit3);

            var bit4 = KeyBitmap.FromFile(@"CustomDeck\time0.png");
            deck.SetKeyBitmap(4, bit4);

            var bit5 = KeyBitmap.FromFile(@"CustomDeck\timeIncrease.png");
            deck.SetKeyBitmap(5, bit5);

            var bit6 = KeyBitmap.FromFile(@"CustomDeck\timeIncrease.png");
            deck.SetKeyBitmap(6, bit6);

            var bit7 = KeyBitmap.FromFile(@"CustomDeck\goBack.png");
            deck.SetKeyBitmap(7, bit7);

            var bit8 = KeyBitmap.FromFile(@"CustomDeck\timeIncrease.png");
            deck.SetKeyBitmap(8, bit8);

            var bit9 = KeyBitmap.FromFile(@"CustomDeck\timeIncrease.png");
            deck.SetKeyBitmap(9, bit9);

            var bit10 = KeyBitmap.FromFile(@"CustomDeck\timeDecrease.png");
            deck.SetKeyBitmap(10, bit10);

            var bit11 = KeyBitmap.FromFile(@"CustomDeck\timeDecrease.png");
            deck.SetKeyBitmap(11, bit11);

            var bit12 = KeyBitmap.FromFile(@"CustomDeck\timeStart.png");
            deck.SetKeyBitmap(12, bit12);

            var bit13 = KeyBitmap.FromFile(@"CustomDeck\timeDecrease.png");
            deck.SetKeyBitmap(13, bit13);

            var bit14 = KeyBitmap.FromFile(@"CustomDeck\timeDecrease.png");
            deck.SetKeyBitmap(14, bit14);

            updateElgato_TimeClock();

            deckScreen = "timeClock";
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
                        this.InvokeEx(home_FRM => home_FRM.tabControl1.SelectedIndex = 0);
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
                        this.InvokeEx(home_FRM => home_FRM.tabControl1.SelectedIndex = 1);
                        return;
                    }

                    switch (e.Key)
                    {
                        case 0:
                            this.InvokeEx(home_FRM => home_FRM.right_ScoreDecrease3.PerformClick());
                            break;

                        case 1:
                            this.InvokeEx(home_FRM => home_FRM.right_ScoreIncrease3.PerformClick());
                            break;

                        case 2:
                            break;

                        case 3:
                            this.InvokeEx(home_FRM => home_FRM.left_ScoreDecrease3.PerformClick());
                            break;

                        case 4:
                            this.InvokeEx(home_FRM => home_FRM.left_ScoreIncrease3.PerformClick());
                            break;

                        case 5:
                            this.InvokeEx(home_FRM => home_FRM.right_ScoreDecrease2.PerformClick());
                            break;

                        case 6:
                            this.InvokeEx(home_FRM => home_FRM.right_ScoreIncrease2.PerformClick());
                            break;

                        // GO BACK
                        case 7:
                            elgato_SetHomeScreen();
                            screenLeft = "game";
                            break;

                        case 8:
                            this.InvokeEx(home_FRM => home_FRM.left_ScoreDecrease2.PerformClick());
                            break;

                        case 9:
                            this.InvokeEx(home_FRM => home_FRM.left_ScoreIncrease2.PerformClick());
                            break;

                        case 10:
                            this.InvokeEx(home_FRM => home_FRM.right_ScoreDecrease1.PerformClick());
                            break;

                        case 11:
                            this.InvokeEx(home_FRM => home_FRM.right_ScoreIncrease1.PerformClick());
                            break;

                        case 12:
                            leavingScreen = true;
                            screenLeft = "game";
                            elgato_SetTimeClock();
                            break;

                        case 13:
                            this.InvokeEx(home_FRM => home_FRM.left_ScoreDecrease1.PerformClick());
                            break;

                        case 14:
                            this.InvokeEx(home_FRM => home_FRM.left_ScoreIncrease1.PerformClick());
                            break;

                        default:
                            break;
                    }
                }

                if (deckScreen == "timeClock")
                {
                    if (leavingScreen == true)
                    {
                        leavingScreen = false;
                        this.InvokeEx(home_FRM => home_FRM.tabControl1.SelectedTab = home_FRM.duringGame_TabPage);
                        updateElgato_TimeClock();
                        return;
                    }

                    if (scoreTimeMin > 0)
                    {
                        switch (e.Key)
                        {
                            case 0:
                                break;

                            case 1:
                                break;

                            case 2:
                                break;

                            case 3:
                                break;

                            case 4:
                                break;

                            case 5:
                                if (scoreTimeMin > 0)
                                {
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                }
                                break;

                            case 6:
                                if (scoreTimeMin > 0)
                                {
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                }
                                break;

                            // GO BACK
                            case 7:
                                elgato_SetGameHome();
                                screenLeft = "home";
                                break;

                            case 8:
                                if (scoreTimeMin > 0)
                                {
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteIncrease1.PerformClick());
                                }
                                break;

                            case 9:
                                if (scoreTimeMin > 0)
                                {
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteIncrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteIncrease1.PerformClick());
                                }
                                break;

                            case 10:
                                if (scoreTimeMin > 0)
                                {
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                }
                                break;

                            case 11:
                                if (scoreTimeMin > 0)
                                {
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                }
                                break;

                            case 12:
                                this.InvokeEx(home_FRM => home_FRM.timeClock_LBL_Click(null, null));
                                break;

                            case 13:
                                if (scoreTimeMin > 0)
                                {
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteDecrease1.PerformClick());
                                }
                                break;

                            case 14:
                                if (scoreTimeMin > 0)
                                {
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteDecrease1.PerformClick());
                                    this.InvokeEx(home_FRM => home_FRM.time_MinuteDecrease1.PerformClick());
                                }
                                break;

                            default:
                                break;
                        }
                    }

                    if (scoreTimeMin == 0)
                    {
                        switch (e.Key)
                        {
                            case 0:
                                break;

                            case 1:
                                break;

                            case 2:
                                break;

                            case 3:
                                break;

                            case 4:
                                break;

                            case 5:
                                break;

                            case 6:
                                break;

                            // GO BACK
                            case 7:
                                elgato_SetGameHome();
                                screenLeft = "home";
                                break;

                            case 8:
                                this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                break;

                            case 9:
                                this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondIncrease1.PerformClick());
                                break;

                            case 10:
                                break;

                            case 11:
                                break;

                            case 12:
                                this.InvokeEx(home_FRM => home_FRM.timeClock_LBL_Click(null, null));
                                break;

                            case 13:
                                this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                break;

                            case 14:
                                this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                this.InvokeEx(home_FRM => home_FRM.time_SecondDecrease1.PerformClick());
                                break;

                            default:
                                break;
                        }
                    }

                }
            }

            if (e.Key == 0 && e.IsDown)
            {
                this.InvokeEx(home_FRM => home_FRM.musicPlay_BTN.PerformClick());
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

        #region Scorebug
        int leftTimeout;
        int leftScore;

        int rightTimeout;
        int rightScore;

        int scoreTimeMin;
        int scoreTimeSec;
        int scoreTimeMil;

        int scoreQuarter;

        private void timeClock_Timer_Tick(object sender, EventArgs e)
        {
            if (deckScreen == "timeClock")
            {
                KeyBitmap bitStop = KeyBitmap.FromFile(@"CustomDeck\timeStop.png");
                deck.SetKeyBitmap(12, bitStop);
            }

            scoreTimeMil = scoreTimeMil - 1;

            if (scoreTimeMil < 0)
            {
                scoreTimeSec = scoreTimeSec - 1;
                scoreTimeMil = 9;
            }

            if (scoreTimeSec < 0)
            {
                scoreTimeMin = scoreTimeMin - 1;
                scoreTimeSec = 59;
            }

            if (scoreTimeMin < 0)
            {
                scoreTimeMin = 0;
                scoreTimeSec = 0;
                scoreTimeMil = 0;
            }

            
            Console.WriteLine(scoreTimeMin.ToString() + ":" + scoreTimeSec.ToString() + ":" + scoreTimeMil.ToString());
            updateElgato_TimeClock();
            
        }

        KeyBitmap timeIncrease = KeyBitmap.FromFile(@"CustomDeck\timeIncrease.png");
        KeyBitmap timeDecrease = KeyBitmap.FromFile(@"CustomDeck\timeDecrease.png");
        KeyBitmap time0 = KeyBitmap.FromFile(@"CustomDeck\time0.png");
        KeyBitmap time1 = KeyBitmap.FromFile(@"CustomDeck\time1.png");
        KeyBitmap time2 = KeyBitmap.FromFile(@"CustomDeck\time2.png");
        KeyBitmap time3 = KeyBitmap.FromFile(@"CustomDeck\time3.png");
        KeyBitmap time4 = KeyBitmap.FromFile(@"CustomDeck\time4.png");
        KeyBitmap time5 = KeyBitmap.FromFile(@"CustomDeck\time5.png");
        KeyBitmap time6 = KeyBitmap.FromFile(@"CustomDeck\time6.png");
        KeyBitmap time7 = KeyBitmap.FromFile(@"CustomDeck\time7.png");
        KeyBitmap time8 = KeyBitmap.FromFile(@"CustomDeck\time8.png");
        KeyBitmap time9 = KeyBitmap.FromFile(@"CustomDeck\time9.png");
        KeyBitmap period = KeyBitmap.FromFile(@"CustomDeck\timePeriod.png");
        KeyBitmap colon = KeyBitmap.FromFile(@"CustomDeck\timeColon.png");

        private void updateElgato_TimeClock()
        {
            if (deckScreen == "timeClock")
            {
                if (scoreTimeMin > 99)
                {
                    scoreTimeMin = 99;
                }

                if (scoreTimeMin == 0)
                {
                    char[] secChar = scoreTimeSec.ToString("00").ToCharArray();
                    char[] milChar = scoreTimeMil.ToString("0").ToCharArray();

                    //SET FIRST CHAR SECONDS
                    switch (secChar[0])
                    {
                        case '0':
                            deck.SetKeyBitmap(4, time0);
                            break;
                        case '1':
                            deck.SetKeyBitmap(4, time1);
                            break;
                        case '2':
                            deck.SetKeyBitmap(4, time2);
                            break;
                        case '3':
                            deck.SetKeyBitmap(4, time3);
                            break;
                        case '4':
                            deck.SetKeyBitmap(4, time4);
                            break;
                        case '5':
                            deck.SetKeyBitmap(4, time5);
                            break;
                        case '6':
                            deck.SetKeyBitmap(4, time6);
                            break;
                        case '7':
                            deck.SetKeyBitmap(4, time7);
                            break;
                        case '8':
                            deck.SetKeyBitmap(4, time8);
                            break;
                        case '9':
                            deck.SetKeyBitmap(4, time9);
                            break;
                    }

                    //SET SECOND CHAR SECONDS
                    switch (secChar[1])
                    {
                        case '0':
                            deck.SetKeyBitmap(3, time0);
                            break;
                        case '1':
                            deck.SetKeyBitmap(3, time1);
                            break;
                        case '2':
                            deck.SetKeyBitmap(3, time2);
                            break;
                        case '3':
                            deck.SetKeyBitmap(3, time3);
                            break;
                        case '4':
                            deck.SetKeyBitmap(3, time4);
                            break;
                        case '5':
                            deck.SetKeyBitmap(3, time5);
                            break;
                        case '6':
                            deck.SetKeyBitmap(3, time6);
                            break;
                        case '7':
                            deck.SetKeyBitmap(3, time7);
                            break;
                        case '8':
                            deck.SetKeyBitmap(3, time8);
                            break;
                        case '9':
                            deck.SetKeyBitmap(3, time9);
                            break;
                    }

                    //SET PERIOD
                    deck.SetKeyBitmap(2, period);

                    //SET FIRST CHAR MIL
                    switch (milChar[0])
                    {
                        case '0':
                            deck.SetKeyBitmap(1, time0);
                            break;
                        case '1':
                            deck.SetKeyBitmap(1, time1);
                            break;
                        case '2':
                            deck.SetKeyBitmap(1, time2);
                            break;
                        case '3':
                            deck.SetKeyBitmap(1, time3);
                            break;
                        case '4':
                            deck.SetKeyBitmap(1, time4);
                            break;
                        case '5':
                            deck.SetKeyBitmap(1, time5);
                            break;
                        case '6':
                            deck.SetKeyBitmap(1, time6);
                            break;
                        case '7':
                            deck.SetKeyBitmap(1, time7);
                            break;
                        case '8':
                            deck.SetKeyBitmap(1, time8);
                            break;
                        case '9':
                            deck.SetKeyBitmap(1, time9);
                            break;
                    }

                    //SET SECOND CHAR MIL
                    deck.ClearKey(0);
                    deck.ClearKey(5);
                    deck.ClearKey(6);
                    deck.ClearKey(10);
                    deck.ClearKey(11);

                    //SET +1
                    int[] plusKeys = { 8, 9 };

                    foreach (int key in plusKeys)
                    {
                        deck.SetKeyBitmap(key, timeIncrease);
                    }

                    //SET -1
                    int[] minusKeys = { 13, 14 };

                    foreach (int key in minusKeys)
                    {
                        deck.SetKeyBitmap(key, timeDecrease);
                    }
                }

                if (scoreTimeMin > 0)
                {
                    char[] minChar = scoreTimeMin.ToString("00").ToCharArray();
                    char[] secChar = scoreTimeSec.ToString("00").ToCharArray();

                    //SET FIRST CHAR SECONDS
                    switch (minChar[0])
                    {
                        case '0':
                            deck.SetKeyBitmap(4, time0);
                            break;
                        case '1':
                            deck.SetKeyBitmap(4, time1);
                            break;
                        case '2':
                            deck.SetKeyBitmap(4, time2);
                            break;
                        case '3':
                            deck.SetKeyBitmap(4, time3);
                            break;
                        case '4':
                            deck.SetKeyBitmap(4, time4);
                            break;
                        case '5':
                            deck.SetKeyBitmap(4, time5);
                            break;
                        case '6':
                            deck.SetKeyBitmap(4, time6);
                            break;
                        case '7':
                            deck.SetKeyBitmap(4, time7);
                            break;
                        case '8':
                            deck.SetKeyBitmap(4, time8);
                            break;
                        case '9':
                            deck.SetKeyBitmap(4, time9);
                            break;
                    }

                    //SET SECOND CHAR SECONDS
                    switch (minChar[1])
                    {
                        case '0':
                            deck.SetKeyBitmap(3, time0);
                            break;
                        case '1':
                            deck.SetKeyBitmap(3, time1);
                            break;
                        case '2':
                            deck.SetKeyBitmap(3, time2);
                            break;
                        case '3':
                            deck.SetKeyBitmap(3, time3);
                            break;
                        case '4':
                            deck.SetKeyBitmap(3, time4);
                            break;
                        case '5':
                            deck.SetKeyBitmap(3, time5);
                            break;
                        case '6':
                            deck.SetKeyBitmap(3, time6);
                            break;
                        case '7':
                            deck.SetKeyBitmap(3, time7);
                            break;
                        case '8':
                            deck.SetKeyBitmap(3, time8);
                            break;
                        case '9':
                            deck.SetKeyBitmap(3, time9);
                            break;
                    }

                    //SET PERIOD
                    deck.SetKeyBitmap(2, colon);

                    //SET FIRST CHAR MIL
                    switch (secChar[0])
                    {
                        case '0':
                            deck.SetKeyBitmap(1, time0);
                            break;
                        case '1':
                            deck.SetKeyBitmap(1, time1);
                            break;
                        case '2':
                            deck.SetKeyBitmap(1, time2);
                            break;
                        case '3':
                            deck.SetKeyBitmap(1, time3);
                            break;
                        case '4':
                            deck.SetKeyBitmap(1, time4);
                            break;
                        case '5':
                            deck.SetKeyBitmap(1, time5);
                            break;
                        case '6':
                            deck.SetKeyBitmap(1, time6);
                            break;
                        case '7':
                            deck.SetKeyBitmap(1, time7);
                            break;
                        case '8':
                            deck.SetKeyBitmap(1, time8);
                            break;
                        case '9':
                            deck.SetKeyBitmap(1, time9);
                            break;
                    }

                    //SET SECOND CHAR MIL
                    switch (secChar[1])
                    {
                        case '0':
                            deck.SetKeyBitmap(0, time0);
                            break;
                        case '1':
                            deck.SetKeyBitmap(0, time1);
                            break;
                        case '2':
                            deck.SetKeyBitmap(0, time2);
                            break;
                        case '3':
                            deck.SetKeyBitmap(0, time3);
                            break;
                        case '4':
                            deck.SetKeyBitmap(0, time4);
                            break;
                        case '5':
                            deck.SetKeyBitmap(0, time5);
                            break;
                        case '6':
                            deck.SetKeyBitmap(0, time6);
                            break;
                        case '7':
                            deck.SetKeyBitmap(0, time7);
                            break;
                        case '8':
                            deck.SetKeyBitmap(0, time8);
                            break;
                        case '9':
                            deck.SetKeyBitmap(0, time9);
                            break;
                    }

                    //SET +1
                    int[] plusKeys = { 5, 6, 8, 9 };

                    foreach (int key in plusKeys)
                    {
                        deck.SetKeyBitmap(key, timeIncrease);
                    }

                    //SET -1
                    int[] minusKeys = { 10, 11, 13, 14 };

                    foreach (int key in minusKeys)
                    {
                        deck.SetKeyBitmap(key, timeDecrease);
                    }
                }

                if (scoreTimeSec == 0 & scoreTimeMin == 0 & scoreTimeMil == 0)
                {              
                    timeClock_Timer.Stop();
                    timeClockRunning = false;

                    KeyBitmap bitStart = KeyBitmap.FromFile(@"CustomDeck\timeStart.png");
                    deck.SetKeyBitmap(12, bitStart);

                }
            }

            update_timeClockLabel();
        }

        private void update_timeClockLabel()
        {
            if (scoreTimeMin == 0)
            {
                this.InvokeEx(home_FRM => home_FRM.timeClock_LBL.Text = scoreTimeSec.ToString() + "." + scoreTimeMil.ToString());

            }

            if (scoreTimeMin != 0)
            {
                this.InvokeEx(home_FRM => home_FRM.timeClock_LBL.Text = scoreTimeMin.ToString() + ":" + scoreTimeSec.ToString("00"));
            }
        }

        private void startScoreboard()
        {
            string sqlQueryLeft = "" +
                "SELECT team, name, score, timeouts " +
                "FROM currentGame ";

            DataTable dataTable = new DataTable();

            SQLiteDataAdapter sql_DA = new SQLiteDataAdapter(sqlQueryLeft, Global.con);
            sql_DA.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                if (row["team"].ToString() == "left")
                {
                    leftTimeout = Convert.ToInt32(row["timeouts"].ToString());
                    leftScore = Convert.ToInt32(row["score"].ToString());
                }

                if (row["team"].ToString() == "right")
                {
                    rightTimeout = Convert.ToInt32(row["timeouts"].ToString());
                    rightScore = Convert.ToInt32(row["score"].ToString());
                }
            }

            leftTimeouts_LBL.Text = leftTimeout.ToString();
            leftScore_LBL.Text = leftScore.ToString();

            rightTimeouts_LBL.Text = rightTimeout.ToString();
            rightScore_LBL.Text = rightScore.ToString();

        }

        #region Left Side Scoring / Timeouts

        private void left_TimeoutIncrease_Click(object sender, EventArgs e)
        {
            leftTimeout = leftTimeout + 1;
            leftTimeouts_LBL.Text = leftTimeout.ToString();
        }

        private void left_TimeoutDecrease_Click(object sender, EventArgs e)
        {
            leftTimeout = leftTimeout - 1;

            if (leftTimeout < 0)
            {
                leftTimeout = 0;
            }

            leftTimeouts_LBL.Text = leftTimeout.ToString();
        }

        private void left_ScoreIncrease3_Click(object sender, EventArgs e)
        {
            leftScore = leftScore + 3;
            leftScore_LBL.Text = leftScore.ToString();
        }

        private void left_ScoreDecrease3_Click(object sender, EventArgs e)
        {
            leftScore = leftScore - 3;

            if (leftScore < 0)
            {
                leftScore = 0;
            }

            leftScore_LBL.Text = leftScore.ToString();
        }

        private void left_ScoreIncrease2_Click(object sender, EventArgs e)
        {
            leftScore = leftScore + 2;
            leftScore_LBL.Text = leftScore.ToString();
        }

        private void left_ScoreDecrease2_Click(object sender, EventArgs e)
        {
            leftScore = leftScore - 2;

            if (leftScore < 0)
            {
                leftScore = 0;
            }

            leftScore_LBL.Text = leftScore.ToString();
        }

        private void left_ScoreIncrease1_Click(object sender, EventArgs e)
        {
            leftScore = leftScore + 1;
            leftScore_LBL.Text = leftScore.ToString();
        }

        private void left_ScoreDecrease1_Click(object sender, EventArgs e)
        {
            leftScore = leftScore - 1;

            if (leftScore < 0)
            {
                leftScore = 0;
            }

            leftScore_LBL.Text = leftScore.ToString();
        }

        #endregion

        #region Right Side Scoring / Timeouts
        private void right_TimeoutIncrease_Click(object sender, EventArgs e)
        {
            rightTimeout = rightTimeout + 1;
            rightTimeouts_LBL.Text = rightTimeout.ToString();
        }

        private void right_TimeoutDecrease_Click(object sender, EventArgs e)
        {
            rightTimeout = rightTimeout - 1;

            if (rightTimeout < 0)
            {
                rightTimeout = 0;
            }

            rightTimeouts_LBL.Text = rightTimeout.ToString();
        }

        private void right_ScoreIncrease3_Click(object sender, EventArgs e)
        {
            rightScore = rightScore + 3;

            rightScore_LBL.Text = rightScore.ToString();
        }

        private void right_ScoreDecrease3_Click(object sender, EventArgs e)
        {
            rightScore = rightScore - 3;

            if (rightScore < 0)
            {
                rightScore = 0;
            }

            rightScore_LBL.Text = rightScore.ToString();
        }

        private void right_ScoreIncrease2_Click(object sender, EventArgs e)
        {
            rightScore = rightScore + 2;

            rightScore_LBL.Text = rightScore.ToString();
        }

        private void right_ScoreDecrease2_Click(object sender, EventArgs e)
        {
            rightScore = rightScore - 2;

            if (rightScore < 0)
            {
                rightScore = 0;
            }

            rightScore_LBL.Text = rightScore.ToString();
        }

        private void right_ScoreIncrease1_Click(object sender, EventArgs e)
        {
            rightScore = rightScore + 1;

            rightScore_LBL.Text = rightScore.ToString();
        }

        private void right_ScoreDecrease1_Click(object sender, EventArgs e)
        {
            rightScore = rightScore - 1;

            if (rightScore < 0)
            {
                rightScore = 0;
            }

            rightScore_LBL.Text = rightScore.ToString();
        }
        #endregion

        private void time_MinuteIncrease1_Click(object sender, EventArgs e)
        {
            timeClock_Timer.Stop();
            scoreTimeMin = scoreTimeMin + 1;
            updateElgato_TimeClock();
            if (timeClockRunning == true)
            {
                timeClock_Timer.Start();
            }
        }

        private void time_MinuteDecrease1_Click(object sender, EventArgs e)
        {
            if (scoreTimeMin == 0)
            {
                return;
            }

            timeClock_Timer.Stop();
            scoreTimeMin = scoreTimeMin - 1;
            updateElgato_TimeClock();
            if (timeClockRunning == true)
            {
                timeClock_Timer.Start();
            }
        }

        private void time_SecondIncrease1_Click(object sender, EventArgs e)
        {
            timeClock_Timer.Stop();
            scoreTimeSec = scoreTimeSec + 1;
            if (scoreTimeSec > 59)
            {
                scoreTimeMin = scoreTimeMin + 1;
                scoreTimeSec = scoreTimeSec - 60;
            }


            updateElgato_TimeClock();
            if (timeClockRunning == true)
            {
                timeClock_Timer.Start();
            }
        }

        private void time_SecondDecrease1_Click(object sender, EventArgs e)
        {
            timeClock_Timer.Stop();
            scoreTimeSec = scoreTimeSec - 1;

            if (scoreTimeSec < 0)
            {
                scoreTimeSec = 59;
                scoreTimeMin = scoreTimeMin - 1;
                if (scoreTimeMin < 0)
                {
                    scoreTimeMin = 0;
                    scoreTimeSec = 0;
                }
            }

            updateElgato_TimeClock();
            if (timeClockRunning == true)
            {
                timeClock_Timer.Start();
            }
        }

        private void quarter_Increase1_Click(object sender, EventArgs e)
        {

        }

        private void quarter_decrease1_Click(object sender, EventArgs e)
        {

        }
        #endregion

        bool timeClockRunning = false;
        private void timeClock_LBL_Click(object sender, EventArgs e)
        {
            if (timeClockRunning == false)
            {
                timeClock_Timer.Start();
                timeClockRunning = true;
                return;
            }

            timeClockRunning = false;
            timeClock_Timer.Stop();

            var bit12 = KeyBitmap.FromFile(@"CustomDeck\timeStart.png");
            deck.SetKeyBitmap(12, bit12);
        }

        private void updateExports()
        {

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
