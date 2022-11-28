using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Settings : Form
    {
        public static bool isEnglish = true;
        public Settings()
        {
            InitializeComponent();
        }

        private void ExitPicture_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // English Button
        private void btn_English_Click(object sender, EventArgs e)
        {
            isEnglish = true;

            // <----- Edit Labels ----->

            // <---- Settings ---->
            Title.Text = "Settings";
            lbl_Language.Text = "Language";
            lbl_Style.Text = "Style";
            lbl_DragControl.Text = "Drag Control";
            if(toggle_Style.Checked == true)
                lbl_StyleStatus.Text = "Rounded";
            else
                lbl_StyleStatus.Text = "Box";

            if (toggle_DragControl.Checked == true)
                lbl_DragControlStatus.Text = "Enabled";
            else
                lbl_DragControlStatus.Text = "Disabled";

            // <---- About ---->
            Program.about.Title.Text = "About";
            Program.about.lbl_MusicPlayer.Text = "Music Player";
            Program.about.lbl_Version.Text = "Version 1.0";
            Program.about.lbl_Copyrights.Text = "© The Method. All rights reserved.";

            // <---- Main Form ---->

            // Left Panel
            Program.mainform.title2_MusicPlayer.Text = "Music Player";
            Program.mainform.title3_Library.Text = "Library";
            Program.mainform.btn1_NowPlaying.Text = "  Now Playing";
            Program.mainform.btn2_Songs.Text = "  Songs";
            Program.mainform.btn3_Favourites.Text = "  Favourites";
            Program.mainform.btn1_Settings.Text = "  Settings  ";
            Program.mainform.btn2_About.Text = "  About  ";

            // Now Playing Page
            Program.mainform.Title.Text = "Now Playing";
            Program.mainform.NoSongs1_1.Text = "No songs added!";
            Program.mainform.SongTitle1_1.Text = "Song Title";
            Program.mainform.Artist1_1.Text = "Artist";
            Program.mainform.Genre1_1.Text = "Genre";

            // Now Playing Page Grey Labels and Bottom Panel Song Name and Artist
            if (Program.mainform.SongsGrid == null || Program.mainform.SongsGrid.Rows.Count == 0)
            {
                // Grey Labels
                Program.mainform.SongTitle1_2.Text = "Song Name";
                Program.mainform.Artist1_2.Text = "Artist";
                Program.mainform.Genre1_2.Text = "Genre";

                // Bottom Panel
                Program.mainform.SongTitle2.Text = "Song Title";
                Program.mainform.Artist2.Text = "Artist";
            }

            // Songs Page
            Program.mainform.ImportSongsButton.Text = "Import Songs";

            Program.mainform.NoSongs2.Text = "No songs added"; // One Time

            Program.mainform.SongsGrid.Columns[2].HeaderText = "Song";
            Program.mainform.SongsGrid.Columns[3].HeaderText = "Artist";
            Program.mainform.SongsGrid.Columns[4].HeaderText = "Genre";
            Program.mainform.SongsGrid.Columns[5].HeaderText = "Duration";

            // Favourites Songs
            Program.mainform.FavouritesGrid.Columns[2].HeaderText = "Song";
            Program.mainform.FavouritesGrid.Columns[3].HeaderText = "Artist";
            Program.mainform.FavouritesGrid.Columns[4].HeaderText = "Genre";
            Program.mainform.FavouritesGrid.Columns[5].HeaderText = "Duration";
        }

        // Turkish Button
        private void btn_Turkish_Click(object sender, EventArgs e)
        {
            isEnglish = false;

            // <----- Edit Labels ----->

            // <---- Settings ---->
            Title.Text = "Ayarlar";
            lbl_Language.Text = "Dil";
            lbl_Style.Text = "Stil";
            lbl_DragControl.Text = "Sürükleme Kontrolü";
            if (toggle_Style.Checked == true)
                lbl_StyleStatus.Text = "Yuvarlak";
            else
                lbl_StyleStatus.Text = "Kutu";

            if (toggle_DragControl.Checked == true)
                lbl_DragControlStatus.Text = "Açık";
            else
                lbl_DragControlStatus.Text = "Kapalı";

            // <---- About ---->
            Program.about.Title.Text = "Hakkında";
            Program.about.lbl_MusicPlayer.Text = "Müzik Çalar";
            Program.about.lbl_Version.Text = "Versiyon 1.0";
            Program.about.lbl_Copyrights.Text = "© The Method. Tüm hakları saklıdır.";

            // <---- Main Form ---->

            // Left Panel
            Program.mainform.title2_MusicPlayer.Text = "Müzik Çalar";
            Program.mainform.title3_Library.Text = "Kütüphane";
            Program.mainform.btn1_NowPlaying.Text = "  Şimdi Oynuyor";
            Program.mainform.btn2_Songs.Text = "  Şarkılar";
            Program.mainform.btn3_Favourites.Text = "  Favoriler";
            Program.mainform.btn1_Settings.Text = "  Ayarlar  ";
            Program.mainform.btn2_About.Text = "  Hakkında  ";

            // Now Playing Page
            Program.mainform.Title.Text = "Şimdi Oynuyor";
            Program.mainform.NoSongs1_1.Text = "Eklenen şarkı yok!";
            Program.mainform.SongTitle1_1.Text = "Şarkı Adı";
            Program.mainform.Artist1_1.Text = "Sanatçı";
            Program.mainform.Genre1_1.Text = "Tür";

            // Now Playing Page Grey Labels and Bottom Panel Song Name and Artist
            if (Program.mainform.SongsGrid == null || Program.mainform.SongsGrid.Rows.Count == 0)
            {
                // Grey Labels
                Program.mainform.SongTitle1_2.Text = "Şarkı Adı";
                Program.mainform.Artist1_2.Text = "Sanatçı";
                Program.mainform.Genre1_2.Text = "Tür";

                // Bottom panel
                Program.mainform.SongTitle2.Text = "Şarkı Adı";
                Program.mainform.Artist2.Text = "Sanatçı";
            }

            // Songs Page
            Program.mainform.ImportSongsButton.Text = "Şarkı Ekle";

            Program.mainform.NoSongs2.Text = "Eklenen şarkı yok"; // One Time

            Program.mainform.SongsGrid.Columns[2].HeaderText = "Şarkı";
            Program.mainform.SongsGrid.Columns[3].HeaderText = "Sanatçı";
            Program.mainform.SongsGrid.Columns[4].HeaderText = "Tür";
            Program.mainform.SongsGrid.Columns[5].HeaderText = "Süre";

            // Favourites Songs
            Program.mainform.FavouritesGrid.Columns[2].HeaderText = "Şarkı";
            Program.mainform.FavouritesGrid.Columns[3].HeaderText = "Sanatçı";
            Program.mainform.FavouritesGrid.Columns[4].HeaderText = "Tür";
            Program.mainform.FavouritesGrid.Columns[5].HeaderText = "Süre";
        }

        //Style Toggle
        private void toggle_Style_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if(toggle_Style.Checked == true)
            {
                if(isEnglish == true)
                    lbl_StyleStatus.Text = "Rounded";
                else
                    lbl_StyleStatus.Text = "Yuvarlak";

                bunifuElipse1.ElipseRadius = 50;
                Program.mainform.bunifuElipse1.ElipseRadius = 30;
                Program.about.bunifuElipse1.ElipseRadius = 50;
            }
            else
            {
                if (isEnglish == true)
                    lbl_StyleStatus.Text = "Box";
                else
                    lbl_StyleStatus.Text = "Kutu";

                bunifuElipse1.ElipseRadius = 0;
                Program.mainform.bunifuElipse1.ElipseRadius = 0;
                Program.about.bunifuElipse1.ElipseRadius = 0;
            }
        }

        private void toggle_DragControl_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (toggle_DragControl.Checked == true)
            {
                if(isEnglish == true)
                    lbl_DragControlStatus.Text = "Enabled";
                else
                    lbl_DragControlStatus.Text = "Açık";

                Program.mainform.DragControl_LeftPanel.Vertical = true;
                Program.mainform.DragControl_LeftPanel.Horizontal = true;

                Program.mainform.DragControl_Library.Vertical = true;
                Program.mainform.DragControl_Library.Horizontal = true;

                Program.mainform.DragControl_MainForm.Vertical = true;
                Program.mainform.DragControl_MainForm.Horizontal = true;

                Program.mainform.DragControl_MusicPlayer.Vertical = true;
                Program.mainform.DragControl_MusicPlayer.Horizontal = true;

                Program.mainform.DragControl_NowPlayingPanel.Vertical = true;
                Program.mainform.DragControl_NowPlayingPanel.Horizontal = true;

                Program.mainform.DragControl_TheMethod.Vertical = true;
                Program.mainform.DragControl_TheMethod.Horizontal = true;

                Program.mainform.DragControl_SongCover.Vertical = true;
                Program.mainform.DragControl_SongCover.Horizontal = true;
            }
            else
            {
                if (isEnglish == true)
                    lbl_DragControlStatus.Text = "Disabled";
                else
                    lbl_DragControlStatus.Text = "Kapalı";
                    
                Program.mainform.DragControl_LeftPanel.Vertical = false;
                Program.mainform.DragControl_LeftPanel.Horizontal = false;

                Program.mainform.DragControl_Library.Vertical = false;
                Program.mainform.DragControl_Library.Horizontal = false;

                Program.mainform.DragControl_MainForm.Vertical = false;
                Program.mainform.DragControl_MainForm.Horizontal = false;

                Program.mainform.DragControl_MusicPlayer.Vertical = false;
                Program.mainform.DragControl_MusicPlayer.Horizontal = false;

                Program.mainform.DragControl_NowPlayingPanel.Vertical = false;
                Program.mainform.DragControl_NowPlayingPanel.Horizontal = false;

                Program.mainform.DragControl_TheMethod.Vertical = false;
                Program.mainform.DragControl_TheMethod.Horizontal = false;

                Program.mainform.DragControl_SongCover.Vertical = false;
                Program.mainform.DragControl_SongCover.Horizontal = false;
            }
        }
    }
}
