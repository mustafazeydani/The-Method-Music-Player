using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Text;
using System.IO;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        // Resources
        string filepath = "data.txt";
        Icon PlayIcon = new Icon(@"..\..\Resources\Playbutton_White.ico");
        Icon PauseIcon = new Icon(@"..\..\Resources\Pausebutton_WhiteBlack1.ico");
        Icon FavStar = new Icon(@"..\..\Resources\FavStar1.ico");
        Icon FavStarFilled = new Icon(@"..\..\Resources\FavStarFilled1.ico");
        Icon TrashCan = new Icon(@"..\..\Resources\Trashcan.ico");
        string Default = (@"..\..\Resources\Default.png");
        string RedPlay = (@"..\..\Resources\Playbutton_RedWhite1.png");
        string RedPlayHover = (@"..\..\Resources\Playbutton_RedWhiteHover.png");
        string WhiteStop = (@"..\..\Resources\Stopbutton_WhiteBlack1.png");
        string WhiteStopHover = (@"..\..\Resources\Stopbutton_WhiteBlackHover.png");
        string WhitePause = (@"..\..\Resources\Pausebutton_WhiteBlack1.png");
        string WhitePauseHover = (@"..\..\Resources\Pausebutton_WhiteBlackHover.png");
        string PrevSong = (@"..\..\Resources\previous-song.png");
        string PrevSongHover = (@"..\..\Resources\previous-song-hover.png");
        string NextSong = (@"..\..\Resources\next-song.png");
        string NextSongHover = (@"..\..\Resources\next-song-hover.png");
        string Robot = (@"..\..\Resources\Robot.jpg");

        public readonly PrivateFontCollection BadSignal = new PrivateFontCollection();
        public bool isMinimized = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Import Custom Font
            BadSignal.AddFontFile(@"..\..\Fonts\BadSignal.otf");
            foreach (Control A in this.Controls)
                title1_TheMethod.Font = new Font(BadSignal.Families[0], 48, FontStyle.Regular);

            //Import Only .mp3 and Allow Multiple Select
            Songs_openFileDialog.DefaultExt = "mp3";
            Songs_openFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3";
            Songs_openFileDialog.Multiselect = true;

            // Read record on startup
            readRecord(filepath);
        }

        // Add Song Picture Function
        public void AddSongPic(PictureBox x, int y)
        {
            var file = TagLib.File.Create(path[y]);
            var mStream = new MemoryStream();
            var firstPicture = file.Tag.Pictures.FirstOrDefault();
            if (firstPicture != null)
            {
                byte[] pData = firstPicture.Data.Data;
                mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                var bm = new Bitmap(mStream, false);
                mStream.Dispose();
                x.Image = bm;
            }
            else
                x.Image = Image.FromFile(Default);
        }

        // Add Song and Artist Names Function
        public void SongAndArtist(Label SongTitle, Label Artist, int x)
        {
            var file = TagLib.File.Create(path[x]);
            if (file.Tag.Title == null)
                SongTitle.Text = "Unknown";
            else
                SongTitle.Text = file.Tag.Title;

            if (file.Tag.FirstPerformer == null)
                Artist.Text = "Unknown";
            else
                Artist.Text = file.Tag.FirstPerformer;
        }

        // Add Song, Artist and Genre Names Function
        public void SongArtistAndGenre(Label SongTitle, Label Artist, Label Genre, int x)
        {
            var file = TagLib.File.Create(path[x]);
            if (file.Tag.Title == null)
                SongTitle.Text = "Unknown";
            else
                SongTitle.Text = file.Tag.Title;

            if (file.Tag.FirstPerformer == null)
                Artist.Text = "Unknown";
            else
                Artist.Text = file.Tag.FirstPerformer;

            if (file.Tag.FirstGenre == null)
                Genre.Text = "Unknown";
            else
                Genre.Text = file.Tag.FirstGenre;
        }

        //Close Form Button
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Minimize Form
        private void PicBox_Minimize_Click(object sender, EventArgs e)
        {
            LeftPanel.Visible = false; // Bugs after minimizing
            this.WindowState = FormWindowState.Minimized;
            isMinimized = true;
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (isMinimized == true)
            {
                LeftPanel.Visible = true;
                isMinimized = false;
            }
        }

        //Add Record to CSV File 
        public static void addRecord(int id, string path, string title, string artist, string genre, string duration, string filepath)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(filepath, true))
                {
                    file.WriteLine(id + "," + path + "," + title + "," + artist + "," + genre + "," + duration);
                    file.Close();
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Error!", ex);
            }
        }

        // Read Record from CSV File
        public void readRecord(string filepath)
        {
            StreamReader reader = null;
            if (File.Exists(filepath))
            {
                reader = new StreamReader(File.OpenRead(filepath));
                while (!reader.EndOfStream) // Read SongsGrid data and from the file and add it to the SongsGrid
                {
                    var line = reader.ReadLine();
                    if (line.Contains("Favourites")) break;
                    var values = line.Split(',');
                    path.Add(values[1]);
                    SongsGrid.Rows.Add(Convert.ToInt32(values[0]), values[1], values[2], values[3], values[4], values[5]);
                    if (values[6] == "1") // If star is filled
                        FavGrid1.Rows.Add(FavStarFilled, PlayIcon);
                    else
                        FavGrid1.Rows.Add(FavStar, PlayIcon);
                    DeleteGrid.Rows.Add(TrashCan);
                }
                while (!reader.EndOfStream) // Read Favourites data from the file and add it to the FavouritesGrid
                {
                    var line = reader.ReadLine();
                    string [] values = line.Split(',');
                    FavouritesGrid.Rows.Add(Convert.ToInt32(values[0]), values[1], values[2], values[3], values[4], values[5]);
                    FavGrid2.Rows.Add(FavStarFilled, PlayIcon);
                }
                //Hide No Songs Labels
                NoSongs2.Visible = false;
                NoSongs1_1.Visible = false;

                // Stop Player
                Player.Ctlcontrols.stop();

                FavGrid1.ClearSelection(); // Clear Selection on FavGrid1
                DeleteGrid.ClearSelection(); // Clear Selection on DeleteGrid

                //Add Song Cover, Song Name and Artist to Bottom Panel
                AddSongPic(BottomPanelPicBox, SongsGrid.SelectedRows[0].Index);
                SongAndArtist(SongTitle2, Artist2, SongsGrid.SelectedRows[0].Index);

                // Add Song Cover, Song Name, Artist and Genre to Now Playing
                SongArtistAndGenre(SongTitle1_2, Artist1_2, Genre1_2, SongsGrid.SelectedRows[0].Index);
                AddSongPic(SongCoverPicBox, SongsGrid.SelectedRows[0].Index);

                reader.Dispose();
            } 
        }

        // Add Record to CSV file after closing the application 
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
                using (StreamWriter file = new StreamWriter(filepath, true))
                {
                    string x;
                    for(int i=0; i<SongsGrid.RowCount; i++)
                    {
                        if (FavGrid1[0, i].Value == FavStarFilled) x = "1";
                        else x = "0";

                        file.WriteLine(SongsGrid[0, i].Value + "," +
                                       SongsGrid[1, i].Value + "," +
                                       SongsGrid[2, i].Value + "," +
                                       SongsGrid[3, i].Value + "," +
                                       SongsGrid[4, i].Value + "," +
                                       SongsGrid[5, i].Value + "," +
                                       x
                                      );
                    }
                    file.Close();
                }
            }
            if(File.Exists(filepath) && FavouritesGrid.RowCount > 0)
            {
                using (StreamWriter file = new StreamWriter(filepath, true))
                {
                    file.WriteLine("Favourites");
                    for (int i = 0; i < FavouritesGrid.RowCount; i++)
                    {
                        file.WriteLine(FavouritesGrid[0, i].Value + "," +
                                       FavouritesGrid[1, i].Value + "," +
                                       FavouritesGrid[2, i].Value + "," +
                                       FavouritesGrid[3, i].Value + "," +
                                       FavouritesGrid[4, i].Value + "," +
                                       FavouritesGrid[5, i].Value
                                      );
                    }
                    file.Close();
                }
            }
        }

        // <-------------------   Songs Page   ------------------->

        // List to import songs
        List<string> path = new List<string>();

        //Import Songs Button
        private void ImportSongsButton_Click(object sender, EventArgs e)
        {
            //Import Songs
            if (Songs_openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int z;

                //Hide No Songs Labels
                NoSongs2.Visible = false;
                NoSongs1_1.Visible = false;

                if (path.Count == 0) // First Import
                    path.AddRange(Songs_openFileDialog.FileNames);
                else // Other Imports
                {
                    for (int i = 0; i < Songs_openFileDialog.FileNames.Length; i++)
                    {
                        z = 0;
                        for (int j = 0; j < path.Count; j++)
                        {
                            if (path[j] == Songs_openFileDialog.FileNames[i])
                            {
                                z = 1;
                                break;
                            }
                        } 
                        if(z == 0)
                            path.Add(Songs_openFileDialog.FileNames[i]);  
                    }
                }
                
                while (SongsGrid.Rows.Count > 0)
                {
                    SongsGrid.Rows.Remove(SongsGrid.Rows[0]);
                    FavGrid1.Rows.Remove(FavGrid1.Rows[0]);
                    DeleteGrid.Rows.Remove(DeleteGrid.Rows[0]);
                }

                if(File.Exists(filepath))
                    File.Delete(filepath);

                for (int x = 0; x < path.Count; x++)
                {
                    var file = TagLib.File.Create(path[x]);
                    string title = file.Tag.Title;
                    string artist = file.Tag.FirstPerformer;
                    string genre = file.Tag.FirstGenre;
                    double secs = file.Properties.Duration.TotalSeconds;
                    TimeSpan conv = TimeSpan.FromSeconds(secs);
                    string duration = string.Format("{0:D2}:{1:D2}", conv.Minutes, conv.Seconds);
                    SongsGrid.Rows.Add(x, path[x], title, artist, genre, duration);
                    FavGrid1.Rows.Add(FavStar, PlayIcon);
                    DeleteGrid.Rows.Add(TrashCan);
                    // Add Record to CSV File
                    addRecord(x, path[x], title, artist, genre, duration, filepath);
                }

                FavGrid1.ClearSelection(); // Clear Selection on FavGrid1
                DeleteGrid.ClearSelection(); // Clear Selection on DeleteGrid
                
                // Stop Player
                Player.Ctlcontrols.stop();

                //Add Song Cover, Song Name and Artist to Bottom Panel
                AddSongPic(BottomPanelPicBox, SongsGrid.SelectedRows[0].Index);
                SongAndArtist(SongTitle2, Artist2, SongsGrid.SelectedRows[0].Index);

                // Add Song Cover, Song Name, Artist and Genre to Now Playing
                SongArtistAndGenre(SongTitle1_2, Artist1_2, Genre1_2, SongsGrid.SelectedRows[0].Index);
                AddSongPic(SongCoverPicBox, SongsGrid.SelectedRows[0].Index);
            }
        }

        // Player State Changed
        private void Player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            for (int i = 0; i < FavGrid1.RowCount; i++) // Reset all FavGrid1
                FavGrid1[1, i].Value = PlayIcon;
            for (int i = 0; i < FavGrid2.RowCount; i++) // Reset all FavGrid2
                FavGrid2[1, i].Value = PlayIcon;
            // Syncing SongsGrid And FavouritesGrid Together
            int w = 0;
            DataGridView x=null, y=null, z1=null, z2=null;
            if (Player.playState == WMPLib.WMPPlayState.wmppsPlaying && FavGrid1.RowCount > 0)
            {
                if(Title.Text == "Favourites" && FavGrid2.RowCount > 0)
                {
                    FavGrid2[1, FavouritesGrid.SelectedRows[0].Index].Value = PauseIcon;
                    w = SongsGrid.RowCount;
                    x = FavouritesGrid;
                    y = SongsGrid;
                    z1 = FavGrid1;
                    z2 = FavGrid2;
                }
                else if (FavGrid1.RowCount > 0)
                {
                    FavGrid1[1, SongsGrid.SelectedRows[0].Index].Value = PauseIcon;
                    w = FavouritesGrid.RowCount;
                    x = SongsGrid;
                    y = FavouritesGrid;
                    z1 = FavGrid2;
                    z2 = FavGrid1;
                }
                PlayPic.Image = Image.FromFile(WhitePause);
            }
            else
            {
                if (Title.Text == "Favourites" && FavGrid2.RowCount > 0)
                {
                    FavGrid2[1, FavouritesGrid.SelectedRows[0].Index].Value = PlayIcon;
                    w = SongsGrid.RowCount;
                    x = FavouritesGrid;
                    y = SongsGrid;
                    z1 = FavGrid1;
                    z2 = FavGrid2;
                }
                else if(FavGrid1.RowCount > 0)
                {
                    FavGrid1[1, SongsGrid.SelectedRows[0].Index].Value = PlayIcon;
                    w = FavouritesGrid.RowCount;
                    x = SongsGrid;
                    y = FavouritesGrid;
                    z1 = FavGrid2;
                    z2 = FavGrid1;
                }
                PlayPic.Image = Image.FromFile(RedPlay);
            }

            for (int i = 0; i < w; i++)
            {
                if (x[0, x.SelectedRows[0].Index].Value.ToString() == y[0, i].Value.ToString())
                {
                    z1[1, i].Value = z2[1, x.SelectedRows[0].Index].Value;
                    y.Rows[i].Selected = true;
                }
            }
        }

        //Songs DataGridView Functionality
        private void SongsGrid_SelectionChanged(object sender, EventArgs e)
        {
            if(SongsGrid.Rows.Count > 0 || (Title.Text == "Favourites" && FavouritesGrid.Rows.Count == 0))
            {
                Player.URL = Convert.ToString(SongsGrid[1, SongsGrid.SelectedRows[0].Index].Value);
                Player.Ctlcontrols.play();
                // Add Song Cover, Song Name and Artist to BottomPanel
                AddSongPic(BottomPanelPicBox, SongsGrid.SelectedRows[0].Index);
                SongAndArtist(SongTitle2, Artist2, SongsGrid.SelectedRows[0].Index);
                // Add Song Cover, Song Name, Artist and Genre to Now Playing Page
                AddSongPic(SongCoverPicBox, SongsGrid.SelectedRows[0].Index);
                SongArtistAndGenre(SongTitle1_2, Artist1_2, Genre1_2, SongsGrid.SelectedRows[0].Index);
            }
        }

        //FavGrid1 DataGridView Functionality
        private void FavGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (FavGrid1[0, e.RowIndex].Selected == true) // Pressing the stars
            {
                //Toggle Fav Button On and Off
                if (FavGrid1.Rows[e.RowIndex].Cells[0].Value == FavStar)
                    FavGrid1.Rows[e.RowIndex].Cells[0].Value = FavStarFilled;
                else
                    FavGrid1.Rows[e.RowIndex].Cells[0].Value = FavStar;

                //Add Song to Favourites DataViewGrid
                if (FavGrid1.Rows[e.RowIndex].Cells[0].Value == FavStarFilled)
                {
                    FavouritesGrid.Rows.Add();
                    FavGrid2.Rows.Add(FavStarFilled, PlayIcon);
                    for (int i = 0; i < 6; i++)
                        FavouritesGrid[i, FavouritesGrid.RowCount - 1].Value = SongsGrid[i, e.RowIndex].Value;
                    if (FavGrid1[1, e.RowIndex].Value == PauseIcon)
                    {
                        FavouritesGrid.Rows[FavouritesGrid.RowCount - 1].Selected = true;
                        FavGrid2[1, FavouritesGrid.RowCount - 1].Value = PauseIcon;
                    }
                }
                else //Delete Song From Favourites DataGridView
                {
                    for (int x = 0; x < FavouritesGrid.RowCount; x++)
                    {
                        if (FavouritesGrid[0, x].Value.ToString() == SongsGrid[0, e.RowIndex].Value.ToString())
                        {
                            FavouritesGrid.Rows.RemoveAt(x);
                            FavGrid2.Rows.RemoveAt(x);
                        }
                    }
                }
            }
            else // Pressing the Play or Pause Buttons
            {
                SongsGrid.Rows[e.RowIndex].Selected = true;
                if(Player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                    Player.Ctlcontrols.pause();
                else
                    Player.Ctlcontrols.play();
            }
            FavGrid1.ClearSelection();
        }

        // DeleteGrid Functionality
        private void DeleteGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Syncing Deletion between SongsGrid and Favourites Grid
            if (FavouritesGrid.RowCount > 0)
            {
                for (int i = 0; i < FavouritesGrid.RowCount; i++)
                {
                    if (SongsGrid[0, e.RowIndex].Value.ToString() == FavouritesGrid[0, i].Value.ToString())
                    {
                        FavouritesGrid.Rows.RemoveAt(i);
                        FavGrid2.Rows.RemoveAt(i);
                    }
                }
            }

            // Deleting first song in list while it is playing
            if (e.RowIndex == SongsGrid.SelectedRows[0].Index && SongsGrid.SelectedRows[0].Index > 0)
                SongsGrid.Rows[SongsGrid.SelectedRows[0].Index - 1].Selected = true;
            else if (e.RowIndex == SongsGrid.SelectedRows[0].Index && SongsGrid.RowCount > 1)
                SongsGrid.Rows[SongsGrid.SelectedRows[0].Index + 1].Selected = true;

            // Deleting Songs from SongsGrid
            SongsGrid.Rows.RemoveAt(e.RowIndex);
            FavGrid1.Rows.RemoveAt(e.RowIndex);
            DeleteGrid.Rows.RemoveAt(e.RowIndex);
            path.RemoveAt(e.RowIndex);

            DeleteGrid.ClearSelection();
            FavGrid1.ClearSelection();

            // Reset all labels after clearing the list and deleting the record file
            if (SongsGrid.RowCount == 0)
            {
                if (File.Exists(filepath))
                    File.Delete(filepath);
                Player.Ctlcontrols.stop();
                NoSongs2.Visible = true;
                SongCoverPicBox.Image = Image.FromFile(Robot);
                BottomPanelPicBox.Image = null;
                SongTitle1_2.Text = "Song Title";
                SongTitle2.Text = "Song Title";
                Artist1_2.Text = "Artist";
                Artist2.Text = "Artist";
                Genre1_2.Text = "Genre";
            }      
        }

        //FavGrid1 DataGridView Row Hover Effect
        private void FavAndDelButtons_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCellStyle style1 = new DataGridViewCellStyle();
            style1.BackColor = Color.FromArgb(240, 84, 84);
            if (e.RowIndex > -1)
                FavGrid1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = style1;
        }
        private void FavAndDelButtons_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            style2.BackColor = Color.Black;
            if (e.RowIndex > -1)
                FavGrid1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = style2;
        }

        //DeleteGrid DataGridView Row Hover Effect
        private void DeleteGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCellStyle style1 = new DataGridViewCellStyle();
            style1.BackColor = Color.FromArgb(240, 84, 84);
            if (e.RowIndex > -1)
                DeleteGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = style1;
        }

        private void DeleteGrid_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            style2.BackColor = Color.Black;
            if (e.RowIndex > -1)
                DeleteGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = style2;
        }

        //Hover Effect on BottomPanel PlayButton
        private void PlayPic_MouseEnter(object sender, EventArgs e)
        {
            if (Player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                PlayPic.Image = Image.FromFile(WhitePauseHover);
            else
                PlayPic.Image = Image.FromFile(RedPlayHover);
        }
        private void PlayPic_MouseLeave(object sender, EventArgs e)
        {
            if (Player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                PlayPic.Image = Image.FromFile(WhitePause);
            else
                PlayPic.Image = Image.FromFile(RedPlay);
        }

        //Hover Effect on BottomPanel StopButton
        private void StopPic_MouseEnter(object sender, EventArgs e)
        {
            StopPic.Image = Image.FromFile(WhiteStopHover);
        }
        private void StopPic_MouseLeave(object sender, EventArgs e)
        {
            StopPic.Image = Image.FromFile(WhiteStop);
        }

        //Hover Effect on BottomPanel PrevButton
        private void PrevSongPic_MouseEnter(object sender, EventArgs e)
        {
            PrevSongPic.Image = Image.FromFile(PrevSongHover);
        }
        private void PrevSongPic_MouseLeave(object sender, EventArgs e)
        {
            PrevSongPic.Image = Image.FromFile(PrevSong);
        }

        //Hover Effect on BottomPanel NextButton
        private void NextSongPic_MouseEnter(object sender, EventArgs e)
        {
            NextSongPic.Image = Image.FromFile(NextSongHover);
        }
        private void NextSongPic_MouseLeave(object sender, EventArgs e)
        {
            NextSongPic.Image = Image.FromFile(NextSong);
        }

        //BottomPanel StopButton Functionality
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (Player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                Player.Ctlcontrols.stop();
        }

        //BottomPanel PlayButton Functionality
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (Player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                Player.Ctlcontrols.pause();
            else
                Player.Ctlcontrols.play();   
        }

        //BottomPanel PrevButton Functionality
        private void PrevSongPic_Click(object sender, EventArgs e)
        {
            if(Title.Text == "Favourites" && FavouritesGrid.RowCount > 0)
            {
                if (FavouritesGrid.SelectedRows[0].Index > 0)
                    FavouritesGrid.Rows[FavouritesGrid.SelectedRows[0].Index - 1].Selected = true;
                else
                {
                    Player.URL = Convert.ToString(FavouritesGrid[1, FavouritesGrid.SelectedRows[0].Index].Value);
                    Player.Ctlcontrols.play();
                }
            }
            else if (SongsGrid.RowCount > 0)
            {
                if (SongsGrid.SelectedRows[0].Index > 0)
                    SongsGrid.Rows[SongsGrid.SelectedRows[0].Index - 1].Selected = true;
                else
                {
                    Player.URL = Convert.ToString(SongsGrid[1, SongsGrid.SelectedRows[0].Index].Value);
                    Player.Ctlcontrols.play();
                }   
            }
        }

        //BottomPanel NextButton Functionality
        private void NextSongPic_Click(object sender, EventArgs e)
        {
            if (Title.Text == "Favourites" && FavouritesGrid.RowCount > 0 )
            {
                if (FavouritesGrid.SelectedRows[0].Index < FavouritesGrid.RowCount - 1)
                    FavouritesGrid.Rows[FavouritesGrid.SelectedRows[0].Index + 1].Selected = true;
                else
                {
                    Player.URL = Convert.ToString(FavouritesGrid[1, FavouritesGrid.SelectedRows[0].Index].Value);
                    Player.Ctlcontrols.play();
                }
            }
            else if (SongsGrid.RowCount > 0)
            {
                if (SongsGrid.SelectedRows[0].Index < SongsGrid.RowCount-1)
                    SongsGrid.Rows[SongsGrid.SelectedRows[0].Index + 1].Selected = true;
                else
                {
                    Player.URL = Convert.ToString(SongsGrid[1, SongsGrid.SelectedRows[0].Index].Value);
                    Player.Ctlcontrols.play();
                }
            }
        }

        //BottomPanel Song Timer Label
        private void timer_Tick(object sender, EventArgs e)
        {
            if (Player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                SongSlider.Maximum = (int)Player.Ctlcontrols.currentItem.duration;
                SongSlider.Value = (int)Player.Ctlcontrols.currentPosition;
                SongTimer.Text = Player.Ctlcontrols.currentPositionString;
            }
        }

        // Bottom Panel Song Slider
        private void SongSlider_Scroll(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ScrollEventArgs e)
        {
            Player.Ctlcontrols.currentPosition = SongSlider.Value;
        }

        //Bottom Panel SoundBar Functionality
        private void SoundSlider_Scroll(object sender, Utilities.BunifuSlider.BunifuVScrollBar.ScrollEventArgs e)
        {
            Player.settings.volume = SoundSlider.Value;
        }

        // <-------------------   Favourites Page   ------------------->

        // Favourites DataGridView Functionality
        private void FavouritesGrid_SelectionChanged(object sender, EventArgs e)
        {
            if(Title.Text == "Favourites" && FavouritesGrid.RowCount > 0)
            {
                Player.URL = Convert.ToString(FavouritesGrid[1, FavouritesGrid.SelectedRows[0].Index].Value);
                Player.Ctlcontrols.play();
                // Add Song Cover, Song Name and Artist to BottomPanel
                AddSongPic(BottomPanelPicBox, (int)FavouritesGrid[0, FavouritesGrid.SelectedRows[0].Index].Value);
                SongAndArtist(SongTitle2, Artist2, (int)FavouritesGrid[0, FavouritesGrid.SelectedRows[0].Index].Value);
                // Add Song Cover, Song Name, Artist and Genre to Now Playing Page
                AddSongPic(SongCoverPicBox, (int)FavouritesGrid[0, FavouritesGrid.SelectedRows[0].Index].Value);
                SongArtistAndGenre(SongTitle1_2, Artist1_2, Genre1_2, (int)FavouritesGrid[0, FavouritesGrid.SelectedRows[0].Index].Value);
            }
        }

        //FavGrid2 Functionality
        private void FavGrid2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (FavGrid2[0, e.RowIndex].Selected == true) // Pressing the stars
            {
                if (e.RowIndex == FavouritesGrid.SelectedRows[0].Index)
                {
                    if (FavouritesGrid.SelectedRows[0].Index > 0)
                        FavouritesGrid.Rows[FavouritesGrid.SelectedRows[0].Index - 1].Selected = true;
                }
                FavGrid1[0, (int)FavouritesGrid[0, e.RowIndex].Value].Value = FavStar; /************/
                FavouritesGrid.Rows.RemoveAt(e.RowIndex);
                FavGrid2.Rows.RemoveAt(e.RowIndex);
                if (FavouritesGrid.RowCount == 0)
                {
                    NoSongs2.Visible = true;
                }
            }
            else // Pressing the Play Button
            {
                FavouritesGrid.Rows[e.RowIndex].Selected = true;
                if (Player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                    Player.Ctlcontrols.pause();
                else
                    Player.Ctlcontrols.play();
            }
            FavGrid2.ClearSelection();
        }

        // FavGrid2 DataGridView Row Hover Effect
        private void FavGrid2_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCellStyle style1 = new DataGridViewCellStyle();
            style1.BackColor = Color.FromArgb(240, 84, 84);
            if (e.RowIndex > -1)
                FavGrid2.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = style1;
        }
        private void FavGrid2_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            style2.BackColor = Color.Black;
            if (e.RowIndex > -1)
                FavGrid2.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = style2;
        }

        // <-------------------   LeftPanel Page Navigation Buttons   ------------------->

        //Now Playing Button
        private void btn1_NowPlaying_Click(object sender, EventArgs e)
        {
            //Title
            if (Settings.isEnglish == true)
                Title.Text = "Now Playing";
            else
                Title.Text = "Şimdi Oynuyor";

            //Visibility Control
            NowPlayingPanel.Visible = true;
            SongListPanel.Visible = false;
        }

        // Songs Button
        private void btn2_Songs_Click(object sender, EventArgs e)
        {
            //Title
            if (Settings.isEnglish == true)
                Title.Text = "Songs";
            else
                Title.Text = "Şarkılar";

            //Visibiliy Control
            SongListPanel.Visible = true;
            ImportSongsButton.Visible = true;
            SongsGrid.Visible = true;
            FavGrid1.Visible = true;
            DeleteGrid.Visible = true;

            NowPlayingPanel.Visible = false;
            FavouritesGrid.Visible = false;
            FavGrid2.Visible = false;
            ScrollBarFavourites.Visible = false;

            // Scroll Bar Visibility
            if (SongsGrid.RowCount >= 9)
                ScrollBarSongs.Visible = true;
            else
                ScrollBarSongs.Visible = false;

            //No Songs Label Visibilty
            if (SongsGrid.RowCount > 0 && SongsGrid.Visible == true)
                NoSongs2.Visible = false;
            else
                NoSongs2.Visible = true;
        }

        // Favourites Button
        private void btn4_Favourites_Click(object sender, EventArgs e)
        {
            //Title
            if (Settings.isEnglish == true)
                Title.Text = "Favourites";
            else
                Title.Text = "Favoriler";

            //Visibility Control 
            FavouritesGrid.Visible = true;
            FavGrid2.Visible = true;
            SongListPanel.Visible = true;

            NowPlayingPanel.Visible = false;
            ImportSongsButton.Visible = false;
            SongsGrid.Visible = false;
            FavGrid1.Visible = false;
            DeleteGrid.Visible = false;
            ScrollBarSongs.Visible = false;

            // Scroll Bar Visibilty
            if (FavouritesGrid.RowCount >= 9)
                ScrollBarFavourites.Visible = true;
            else
                ScrollBarFavourites.Visible = false;
            
            //No Songs Label Visibility
            if (FavouritesGrid.RowCount > 0 && FavouritesGrid.Visible == true)
            NoSongs2.Visible = false;
            else
                NoSongs2.Visible = true;

            //Link WindowMediaPlayer To first song In Favourites
            if (FavouritesGrid.RowCount > 0 && Player.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                Player.URL = Convert.ToString(FavouritesGrid[1, FavouritesGrid.SelectedRows[0].Index].Value);
            }

            //Clear Selection on FavGrid2 Grid
            FavGrid2.ClearSelection();
        }

        // Settings and About Buttons
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            btn1_NowPlaying.PerformClick();
            btn1_NowPlaying.Focus();
            Program.settings.ShowDialog();
        }
        public void bunifuButton2_Click(object sender, EventArgs e)
        {
            btn1_NowPlaying.PerformClick();
            btn1_NowPlaying.Focus();
            Program.about.ShowDialog();
        }

        // Scroll Bar Settings for both Songs Grid And Favourites Grid

        //Syncing Scroll Bar with Songs DataGridView
        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                ScrollBarSongs.Value = e.NewValue;
        }
        private void ScrollBar_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
            if (e.Value < SongsGrid.RowCount-1)
            {
                SongsGrid.FirstDisplayedScrollingRowIndex = e.Value;
                FavGrid1.FirstDisplayedScrollingRowIndex = e.Value;
                DeleteGrid.FirstDisplayedScrollingRowIndex = e.Value;
            }
        }

        //Syncing Scroll Bar with Favourites DataGridView
        private void FavouritesGrid_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                ScrollBarFavourites.Value = e.NewValue;
        }
        private void ScrollBarFavourites_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
            if (e.Value < FavouritesGrid.RowCount)
            {
                FavouritesGrid.FirstDisplayedScrollingRowIndex = e.Value;
                FavGrid2.FirstDisplayedScrollingRowIndex = e.Value;
            }
        }

        private void SongsGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //Show ScrollBar
            if (SongsGrid.RowCount >= 9)
            {
                ScrollBarSongs.Visible = true;

                //Scroll Bar Settings
                ScrollBarSongs.Maximum = SongsGrid.RowCount;
                ScrollBarSongs.LargeChange = SongsGrid.DisplayedRowCount(true);
                ScrollBarSongs.SmallChange = 1;
            }
        }

        private void SongsGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //Hide ScrollBar
            if (SongsGrid.RowCount < 9)
                ScrollBarSongs.Visible = false;
            else
            {
                //Scroll Bar Settings
                ScrollBarSongs.Maximum = SongsGrid.RowCount;
                ScrollBarSongs.LargeChange = SongsGrid.DisplayedRowCount(true);
                ScrollBarSongs.SmallChange = 1;
            }
        }

        private void FavouritesGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //Show ScrollBar
            if (FavouritesGrid.RowCount >= 9)
            {
                ScrollBarSongs.Visible = true;

                //Scroll Bar Settings
                ScrollBarFavourites.Maximum = FavouritesGrid.RowCount;
                ScrollBarFavourites.LargeChange = FavouritesGrid.DisplayedRowCount(true);
                ScrollBarFavourites.SmallChange = 1;
            }
        }

        private void FavouritesGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //Hide ScrollBar
            if (FavouritesGrid.RowCount < 9)
                ScrollBarFavourites.Visible = false;
            else
            {
                //Scroll Bar Settings
                ScrollBarFavourites.Maximum = FavouritesGrid.RowCount;
                ScrollBarFavourites.LargeChange = FavouritesGrid.DisplayedRowCount(true);
                ScrollBarFavourites.SmallChange = 1;
            }
        }
    }
}