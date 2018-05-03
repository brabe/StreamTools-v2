namespace StreamTools_v2
{
    partial class home_FRM
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.gameSetup_TabPage = new System.Windows.Forms.TabPage();
            this.musicPlaylist_GB = new System.Windows.Forms.GroupBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.musicRefresh_BTN = new System.Windows.Forms.Button();
            this.musicNext_BTN = new System.Windows.Forms.Button();
            this.musicPrev_BTN = new System.Windows.Forms.Button();
            this.musicPlay_BTN = new System.Windows.Forms.Button();
            this.musicPlaylist_LB = new System.Windows.Forms.ListBox();
            this.musicArtist_LBL = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.musicTrackName_LBL = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.rightTeam_GB = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.leftTeam_GB = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.duringGame_TabPage = new System.Windows.Forms.TabPage();
            this.updateOBS = new System.Windows.Forms.Button();
            this.connectOBS_Button = new System.Windows.Forms.Button();
            this.connectionNotification_Label = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.gameSetup_TabPage.SuspendLayout();
            this.musicPlaylist_GB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.rightTeam_GB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.leftTeam_GB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.gameSetup_TabPage);
            this.tabControl1.Controls.Add(this.duringGame_TabPage);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(752, 359);
            this.tabControl1.TabIndex = 0;
            // 
            // gameSetup_TabPage
            // 
            this.gameSetup_TabPage.Controls.Add(this.musicPlaylist_GB);
            this.gameSetup_TabPage.Controls.Add(this.rightTeam_GB);
            this.gameSetup_TabPage.Controls.Add(this.leftTeam_GB);
            this.gameSetup_TabPage.Location = new System.Drawing.Point(4, 22);
            this.gameSetup_TabPage.Name = "gameSetup_TabPage";
            this.gameSetup_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.gameSetup_TabPage.Size = new System.Drawing.Size(744, 333);
            this.gameSetup_TabPage.TabIndex = 0;
            this.gameSetup_TabPage.Text = "Game Setup";
            this.gameSetup_TabPage.UseVisualStyleBackColor = true;
            // 
            // musicPlaylist_GB
            // 
            this.musicPlaylist_GB.Controls.Add(this.pictureBox3);
            this.musicPlaylist_GB.Controls.Add(this.musicRefresh_BTN);
            this.musicPlaylist_GB.Controls.Add(this.musicNext_BTN);
            this.musicPlaylist_GB.Controls.Add(this.musicPrev_BTN);
            this.musicPlaylist_GB.Controls.Add(this.musicPlay_BTN);
            this.musicPlaylist_GB.Controls.Add(this.musicPlaylist_LB);
            this.musicPlaylist_GB.Controls.Add(this.musicArtist_LBL);
            this.musicPlaylist_GB.Controls.Add(this.label9);
            this.musicPlaylist_GB.Controls.Add(this.musicTrackName_LBL);
            this.musicPlaylist_GB.Controls.Add(this.label10);
            this.musicPlaylist_GB.Location = new System.Drawing.Point(353, 6);
            this.musicPlaylist_GB.Name = "musicPlaylist_GB";
            this.musicPlaylist_GB.Size = new System.Drawing.Size(388, 324);
            this.musicPlaylist_GB.TabIndex = 1;
            this.musicPlaylist_GB.TabStop = false;
            this.musicPlaylist_GB.Text = "Music Playlist";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(9, 112);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(143, 143);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 6;
            this.pictureBox3.TabStop = false;
            // 
            // musicRefresh_BTN
            // 
            this.musicRefresh_BTN.Location = new System.Drawing.Point(192, 285);
            this.musicRefresh_BTN.Name = "musicRefresh_BTN";
            this.musicRefresh_BTN.Size = new System.Drawing.Size(190, 23);
            this.musicRefresh_BTN.TabIndex = 6;
            this.musicRefresh_BTN.Text = "Refresh Playlist";
            this.musicRefresh_BTN.UseVisualStyleBackColor = true;
            this.musicRefresh_BTN.Click += new System.EventHandler(this.musicRefresh_BTN_Click);
            // 
            // musicNext_BTN
            // 
            this.musicNext_BTN.Enabled = false;
            this.musicNext_BTN.Location = new System.Drawing.Point(82, 261);
            this.musicNext_BTN.Name = "musicNext_BTN";
            this.musicNext_BTN.Size = new System.Drawing.Size(70, 23);
            this.musicNext_BTN.TabIndex = 6;
            this.musicNext_BTN.Text = "Next";
            this.musicNext_BTN.UseVisualStyleBackColor = true;
            this.musicNext_BTN.Visible = false;
            this.musicNext_BTN.Click += new System.EventHandler(this.musicNext_BTN_Click);
            // 
            // musicPrev_BTN
            // 
            this.musicPrev_BTN.Enabled = false;
            this.musicPrev_BTN.Location = new System.Drawing.Point(9, 261);
            this.musicPrev_BTN.Name = "musicPrev_BTN";
            this.musicPrev_BTN.Size = new System.Drawing.Size(70, 23);
            this.musicPrev_BTN.TabIndex = 6;
            this.musicPrev_BTN.Text = "Previous";
            this.musicPrev_BTN.UseVisualStyleBackColor = true;
            this.musicPrev_BTN.Visible = false;
            this.musicPrev_BTN.Click += new System.EventHandler(this.musicPrev_BTN_Click);
            // 
            // musicPlay_BTN
            // 
            this.musicPlay_BTN.Enabled = false;
            this.musicPlay_BTN.Location = new System.Drawing.Point(9, 285);
            this.musicPlay_BTN.Name = "musicPlay_BTN";
            this.musicPlay_BTN.Size = new System.Drawing.Size(143, 23);
            this.musicPlay_BTN.TabIndex = 6;
            this.musicPlay_BTN.Text = "Play";
            this.musicPlay_BTN.UseVisualStyleBackColor = true;
            this.musicPlay_BTN.Visible = false;
            this.musicPlay_BTN.Click += new System.EventHandler(this.musicPlay_BTN_Click);
            // 
            // musicPlaylist_LB
            // 
            this.musicPlaylist_LB.FormattingEnabled = true;
            this.musicPlaylist_LB.Location = new System.Drawing.Point(192, 35);
            this.musicPlaylist_LB.Name = "musicPlaylist_LB";
            this.musicPlaylist_LB.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.musicPlaylist_LB.Size = new System.Drawing.Size(190, 238);
            this.musicPlaylist_LB.TabIndex = 0;
            // 
            // musicArtist_LBL
            // 
            this.musicArtist_LBL.AutoSize = true;
            this.musicArtist_LBL.Location = new System.Drawing.Point(11, 96);
            this.musicArtist_LBL.Name = "musicArtist_LBL";
            this.musicArtist_LBL.Size = new System.Drawing.Size(10, 13);
            this.musicArtist_LBL.TabIndex = 5;
            this.musicArtist_LBL.Text = "-";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(189, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Available Music:";
            // 
            // musicTrackName_LBL
            // 
            this.musicTrackName_LBL.AutoSize = true;
            this.musicTrackName_LBL.Location = new System.Drawing.Point(11, 83);
            this.musicTrackName_LBL.Name = "musicTrackName_LBL";
            this.musicTrackName_LBL.Size = new System.Drawing.Size(10, 13);
            this.musicTrackName_LBL.TabIndex = 4;
            this.musicTrackName_LBL.Text = "-";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Currently Playing:";
            // 
            // rightTeam_GB
            // 
            this.rightTeam_GB.Controls.Add(this.pictureBox2);
            this.rightTeam_GB.Controls.Add(this.label5);
            this.rightTeam_GB.Controls.Add(this.label6);
            this.rightTeam_GB.Controls.Add(this.comboBox3);
            this.rightTeam_GB.Controls.Add(this.label7);
            this.rightTeam_GB.Controls.Add(this.comboBox4);
            this.rightTeam_GB.Controls.Add(this.label8);
            this.rightTeam_GB.Location = new System.Drawing.Point(6, 171);
            this.rightTeam_GB.Name = "rightTeam_GB";
            this.rightTeam_GB.Size = new System.Drawing.Size(340, 159);
            this.rightTeam_GB.TabIndex = 0;
            this.rightTeam_GB.TabStop = false;
            this.rightTeam_GB.Text = "Right Team";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(6, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(125, 125);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(142, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "label3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(142, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "label2";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(140, 123);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(186, 21);
            this.comboBox3.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(137, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Color";
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(140, 35);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(186, 21);
            this.comboBox4.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(137, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "School";
            // 
            // leftTeam_GB
            // 
            this.leftTeam_GB.Controls.Add(this.pictureBox1);
            this.leftTeam_GB.Controls.Add(this.label3);
            this.leftTeam_GB.Controls.Add(this.label2);
            this.leftTeam_GB.Controls.Add(this.comboBox2);
            this.leftTeam_GB.Controls.Add(this.label4);
            this.leftTeam_GB.Controls.Add(this.comboBox1);
            this.leftTeam_GB.Controls.Add(this.label1);
            this.leftTeam_GB.Location = new System.Drawing.Point(6, 6);
            this.leftTeam_GB.Name = "leftTeam_GB";
            this.leftTeam_GB.Size = new System.Drawing.Size(340, 159);
            this.leftTeam_GB.TabIndex = 0;
            this.leftTeam_GB.TabStop = false;
            this.leftTeam_GB.Text = "Left Team";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(125, 125);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(142, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "label2";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(140, 123);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(186, 21);
            this.comboBox2.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(137, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Color";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(140, 35);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(186, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "School";
            // 
            // duringGame_TabPage
            // 
            this.duringGame_TabPage.Location = new System.Drawing.Point(4, 22);
            this.duringGame_TabPage.Name = "duringGame_TabPage";
            this.duringGame_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.duringGame_TabPage.Size = new System.Drawing.Size(744, 333);
            this.duringGame_TabPage.TabIndex = 1;
            this.duringGame_TabPage.Text = "During Game";
            this.duringGame_TabPage.UseVisualStyleBackColor = true;
            // 
            // updateOBS
            // 
            this.updateOBS.Location = new System.Drawing.Point(360, 377);
            this.updateOBS.Name = "updateOBS";
            this.updateOBS.Size = new System.Drawing.Size(400, 23);
            this.updateOBS.TabIndex = 1;
            this.updateOBS.Text = "Update OBS";
            this.updateOBS.UseVisualStyleBackColor = true;
            // 
            // connectOBS_Button
            // 
            this.connectOBS_Button.Location = new System.Drawing.Point(12, 377);
            this.connectOBS_Button.Name = "connectOBS_Button";
            this.connectOBS_Button.Size = new System.Drawing.Size(127, 23);
            this.connectOBS_Button.TabIndex = 1;
            this.connectOBS_Button.Text = "Connect OBS";
            this.connectOBS_Button.UseVisualStyleBackColor = true;
            this.connectOBS_Button.Click += new System.EventHandler(this.connectOBS);
            // 
            // connectionNotification_Label
            // 
            this.connectionNotification_Label.AutoSize = true;
            this.connectionNotification_Label.ForeColor = System.Drawing.Color.Red;
            this.connectionNotification_Label.Location = new System.Drawing.Point(145, 382);
            this.connectionNotification_Label.Name = "connectionNotification_Label";
            this.connectionNotification_Label.Size = new System.Drawing.Size(73, 13);
            this.connectionNotification_Label.TabIndex = 2;
            this.connectionNotification_Label.Text = "Disconnected";
            // 
            // home_FRM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 409);
            this.Controls.Add(this.connectionNotification_Label);
            this.Controls.Add(this.connectOBS_Button);
            this.Controls.Add(this.updateOBS);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "home_FRM";
            this.ShowIcon = false;
            this.Text = "Home - Stream Tools";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.home_FRM_FormClosing);
            this.Load += new System.EventHandler(this.home_FRM_Load);
            this.tabControl1.ResumeLayout(false);
            this.gameSetup_TabPage.ResumeLayout(false);
            this.musicPlaylist_GB.ResumeLayout(false);
            this.musicPlaylist_GB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.rightTeam_GB.ResumeLayout(false);
            this.rightTeam_GB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.leftTeam_GB.ResumeLayout(false);
            this.leftTeam_GB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabPage gameSetup_TabPage;
        private System.Windows.Forms.GroupBox leftTeam_GB;
        private System.Windows.Forms.GroupBox rightTeam_GB;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button updateOBS;
        private System.Windows.Forms.Button connectOBS_Button;
        private System.Windows.Forms.Label connectionNotification_Label;
        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage duringGame_TabPage;
        private System.Windows.Forms.GroupBox musicPlaylist_GB;
        private System.Windows.Forms.ListBox musicPlaylist_LB;
        private System.Windows.Forms.Button musicRefresh_BTN;
        private System.Windows.Forms.Button musicPlay_BTN;
        private System.Windows.Forms.Label musicArtist_LBL;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label musicTrackName_LBL;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button musicNext_BTN;
        private System.Windows.Forms.Button musicPrev_BTN;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}

