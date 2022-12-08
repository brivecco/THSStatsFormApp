using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.IO;
using System.Text.Json;
using System.Reflection;

namespace THSStats
{
    public partial class THSStatsForm : Form
    {
        public Game game;

        public string selectedStatMode = "";
        public string selectedStatCode = "";
        public string selectedId = "";

        public string rosterMode = "";

        private string[] playerStatCodes = { "Field Goals", "Free Throws", "3 Pointers", "Fouls" };
        private string[] compTeamStatCodes = { "Team Field Goals", "Team Free Throws", "Team 3 Pointers", "Team Fouls" };

        private ITHSStat currentStat = null;
        private List<ITHSStat> gameStats = new List<ITHSStat>();

        public THSStatsForm()
        {
            InitializeComponent();

            this.Text = "THS Stat Manager v"+ Assembly.GetExecutingAssembly().GetName().Version.ToString();

            OtherListbox.DisplayMember = "Description";
            OtherListbox.ValueMember = "Id";

            // Set stat types
            StatsListbox.Items.Add("Field Goals");
            StatsListbox.Items.Add("Free Throws");
            StatsListbox.Items.Add("3 Pointers");
            StatsListbox.Items.Add("Fouls");
            StatsListbox.Items.Add("Team Field Goals");
            StatsListbox.Items.Add("Team Free Throws");
            StatsListbox.Items.Add("Team 3 Pointers");
            StatsListbox.Items.Add("Team Fouls");

            this.rosterMode = "Home";
            this.RosterButton.Text = this.rosterMode;
            this.RosterButton.Visible = this.selectedStatMode == "Player";

            this.Crawl1Push.Click += UpdateUniqueCrawl;
            this.Crawl2Push.Click += UpdateUniqueCrawl;
            this.Crawl3Push.Click += UpdateUniqueCrawl;
            this.UniqueStatPush.Click += UpdateUniqueCrawl;

            LoadGame();

        }

        private void LoadGame()
        {
            

                string sURL;
                sURL = "https://us-central1-thshooptest.cloudfunctions.net/getGame?foo=jocular";

                using (WebClient wc = new WebClient())
                {
                    string gameJSON = wc.DownloadString(sURL);

                    this.game= JsonSerializer.Deserialize<Game>(gameJSON);
                }
            
        }
        private void fillOtherListbox(string statMode, string rosterMode="")
        {

            switch (statMode)
            {
            
                case "Player":
                    List<Player> players;
                    if (rosterMode == "Home")
                        players = this.game.HomeRoster;
                    else
                        players = this.game.VisitorRoster;
                    var selPlayers = players.Select(p => new { Description = $"#{p.Jersey} {p.FullName}", Id = p.PlayerId }).ToList();
                    OtherListbox.DataSource = selPlayers;
                    break;
                case "IndTeam":
                    List<School> teams = new List<School>() { this.game.HomeSchool, this.game.VisitorSchool };
                    var selTeams = teams.Select(t => new { Description = t.Name, Id = t.SchoolId }).ToList();
                   OtherListbox.DataSource = selTeams;
                    break;
                case "CompTeam":
                    var compTeamItem = new { Description = "Both Teams", Id = "1" };
                    var list= new List<Object>();
                    list.Add(compTeamItem);
                    OtherListbox.DataSource = list;
                    break;
            }
        }

        private void StatsListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox box = (ListBox)sender;
            this.selectedStatCode = box.Text;
            string currentStatMode = this.selectedStatMode;

            if (this.playerStatCodes.Contains(box.Text))
            {
                this.selectedStatMode = "Player";
                label2.Text = "Roster";
                if (currentStatMode != this.selectedStatMode)
                    this.fillOtherListbox(this.selectedStatMode, this.rosterMode);
                else
                    createStat(game, selectedStatMode, selectedStatCode, selectedId);
            }

            else if (this.compTeamStatCodes.Contains(box.Text))
            {
                this.selectedStatMode = "CompTeam";
                label2.Text = "";
                if (currentStatMode != this.selectedStatMode)
                    this.fillOtherListbox(this.selectedStatMode);
                else
                    createStat(game, selectedStatMode, selectedStatCode, selectedId);
            }
            else
            {
                OtherListbox.DataSource = null;
                OtherListbox.Items.Clear();
            }

            this.RosterButton.Visible = this.selectedStatMode == "Player";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.rosterMode= (this.rosterMode=="Home" ? "Visitor": "Home");
            RosterButton.Text = this.rosterMode;
            fillOtherListbox(selectedStatMode,rosterMode);
        }

        private void OtherListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox box = (ListBox)sender;

            if (box.SelectedIndex == -1)
                this.selectedId = null;
            else
            {
                this.selectedId = box.SelectedValue.ToString();
                createStat(game, selectedStatMode, selectedStatCode, selectedId);
            }

            
        }
        private void createStat(Game game, string statMode, string statCode, string id)
        {
            if (game == null || String.IsNullOrEmpty(statMode) || string.IsNullOrEmpty(id))
                return;

            switch (this.selectedStatMode)
            {
                case "Player":
                    currentStat = new IndividualStat(this.selectedStatCode, this.selectedId);
                    UpdateStat();
                    break;
                case "CompTeam":
                    currentStat = new CompTeamStat(this.selectedStatCode, this.selectedId);
                    CompTeamStat compStat = (CompTeamStat)currentStat;
                    UpdateStat();
                    break;
                default:
                    break;

                    //this.gameStats.Add(new IndividualStat(this.selectedStatMode, this.selectedId));
            }
        }
        private void UpdateStat()
        {
            if (currentStat == null)
                return;

            LoadGame();
            currentStat.UpdateStat(game);

            if (currentStat.Description.Contains("|"))
            {
                string[] desc = currentStat.Description.Split('|');
                StatInfoLabel.Text = desc[0];
                StatInfoLabel2.Text = desc[1];
            }
            else
            {
                StatInfoLabel.Text = currentStat.Description;
                StatInfoLabel2.Text = "";
            }

            string json = JsonSerializer.Serialize(currentStat,currentStat.GetType());
            json = "[" + json + "]";

            System.IO.File.WriteAllText($"c:\\test\\{this.selectedStatMode.Replace(" ","")}.json", json);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            UpdateStat();
        }


        // Unique Stat and Crawls
        private void UpdateUniqueCrawl(object sender,EventArgs e)
        {
            //MessageBox.Show("push " + ((Button)sender).Name);
            string infoText;
            string filePrefix;

            switch (((Button)sender).Name) 
            {
                case "Crawl1Push":
                    infoText = Crawl1Box.Text.Trim();
                    filePrefix = "Crawl";
                    break;
                case "Crawl2Push":
                    infoText = Crawl2Box.Text.Trim();
                    filePrefix = "Crawl";
                    break;
                case "Crawl3Push":
                    infoText = Crawl3Box.Text.Trim();
                    filePrefix = "Crawl";
                    break;
                case "UniqueStatPush":
                    infoText = UniqueStatBox.Text.Trim();
                    filePrefix = "UniqueStat";
                    break;
                default:
                    return;
            }

            OtherInfo info = new OtherInfo(infoText);

            string json = JsonSerializer.Serialize(info);
            json = "[" + json + "]";

            System.IO.File.WriteAllText($"c:\\test\\"+filePrefix+".json", json);

            MessageBox.Show("Update complete");
        }
    }
    public class Game
    {
        public List<Player> _allPlayers = null;
        public List<School> _allSchools = null;
        public School HomeSchool { get; set; }
        public School VisitorSchool { get; set; }
        public List<Player> HomeRoster { get; set; }
        public List<Player> VisitorRoster { get; set; }
        public List<Player> AllPlayers
        {
            get
            {
                if (_allPlayers == null)
                    _allPlayers = this.HomeRoster.Concat(this.VisitorRoster).ToList();
                return _allPlayers;
                
            }
        }
        public List<School> AllSchools
        {
            get
            {
                if (_allSchools == null)
                    _allSchools = new List<School> { this.HomeSchool, this.VisitorSchool };
                return _allSchools;

            }
        }

    }
    public class School
    {
        public string SchoolId { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string TextColor { get; set; }
        public string BackgroundColor { get; set; }
        public string LogoFileName { get; set; }

    }
    public class Player
    {
        public string SchoolId { get; set; }
        public string PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Jersey { get; set; }
        public string Height { get; set; }
        public string Position { get; set; }
        public bool OnFloor { get; set; }

        public Int32 Points { get; set; }
        public Int32 Fouls { get; set; }
        public Int32 Assists { get; set; }
        public Int32 OffRebounds { get; set; }
        public Int32 DefRebounds { get; set; }
        public Int32 Rebounds { get; set; }
        public Int32 ThreePointsMade { get; set; }
        public Int32 ThreePointsMissed { get; set; }
        public Int32 TwoPointsMade { get; set; }
        public Int32 TwoPointsMissed { get; set; }
        public Int32 FreeThrowsMade { get; set; }
        public Int32 FreeThrowsMissed { get; set; }
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

    }

    interface ITHSStat
    {
        string StatCode { get; set; }
        string Id { get; set; }
        string Description { get; }

        void UpdateStat(Game game);
    }

    public class IndividualStat:ITHSStat
    {
        public string Description
        {
            get
            {
                return $"({this.FirstName} {this.LastName}) {this.StatHeader1} {this.Stat1}," +
                    $" {this.StatHeader2} {this.Stat2}, {this.StatHeader3} {this.Stat3},";
            }
        }
        public string StatCode { get; set; }
        public string Id { get; set; }

        private Player StatPlayer { get; set; }
        private School StatSchool { get; set; }
        private Game StatGame { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Jersey { get; set; }
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
        public string LogoFileName { get; set; }
       
        public IndividualStat(string statCode, string statId)
        {
            this.StatCode = statCode;
            this.Id = statId;

        }
        public void UpdateStat(Game game)
        {

            this.StatPlayer = game.AllPlayers.Single(p => p.PlayerId ==Id);
            this.StatSchool = game.AllSchools.Single(s => s.SchoolId == this.StatPlayer.SchoolId);

            this.FirstName = this.StatPlayer.FirstName;
            this.LastName = this.StatPlayer.LastName;
            this.Jersey = this.StatPlayer.Jersey;

            this.BackgroundColor = this.StatSchool.BackgroundColor;
            this.TextColor = this.StatSchool.TextColor;
            this.LogoFileName = this.StatSchool.LogoFileName;


           
        }
        public string StatHeader1
        {
            get
            {
                switch (this.StatCode)
                {
                    case "Field Goals":
                    case "Free Throws":
                    case "3 Pointers":
                        return "Pts";
                    case "Fouls":
                        return "Fouls";
                    default:
                        return "";
                }
                    
            }
        }
        public string Stat1
        {
            get
            {
                switch (this.StatCode)
                {
                    case "Field Goals":
                    case "Free Throws":
                    case "3 Pointers":
                        return this.StatPlayer.Points.ToString();
                    case "Fouls":
                        return this.StatPlayer.Fouls.ToString();
                    default:
                        return "";
                }

            }
        }
        public string StatHeader2
        {
            get
            {
                switch (this.StatCode)
                {
                    case "Field Goals":
                        return "FG";
                    case "Free Throws":
                        return "FT";
                    case "3 Pointers":
                        return "3 Pt";
                    default:
                        return "";
                }

            }
        }
        public string Stat2
        {
            get
            {
                Int32 made;
                Int32 total;

                switch (this.StatCode)
                {
                    case "Field Goals":
                        made = this.StatPlayer.TwoPointsMade + this.StatPlayer.ThreePointsMade;
                        total = made + this.StatPlayer.TwoPointsMissed + this.StatPlayer.ThreePointsMissed;
                        return made.ToString() + "/" + total.ToString();
                    case "Free Throws":
                        made = this.StatPlayer.FreeThrowsMade;
                        total = made + StatPlayer.FreeThrowsMissed;
                        return made.ToString() + "/" + total.ToString();
                    case "3 Pointers":
                        made = this.StatPlayer.ThreePointsMade;
                        total = made + StatPlayer.ThreePointsMissed;
                        return made.ToString() + "/" + total.ToString();
                    default:
                        return "";
                }

            }
        }

        public string StatHeader3
        {
            get
            {
                switch (this.StatCode)
                {
                    case "Field Goals":
                    case "Free Throws":
                    case "3 Pointers":
                        return "Pct";
                    default:
                        return "";
                }

            }
        }
        public string Stat3
        {
            get
            {
                Int32 made;
                Int32 total;
                string pct;

                switch (this.StatCode)
                {
                    case "Field Goals":
                        made = this.StatPlayer.TwoPointsMade + this.StatPlayer.ThreePointsMade;
                        total = made + this.StatPlayer.TwoPointsMissed + this.StatPlayer.ThreePointsMissed;
                        if (total == 0)
                            pct = "-";
                        else
                            pct=String.Format("{0:P0}", (Decimal)made / total);
                        return pct;
                    case "Free Throws":
                        made = this.StatPlayer.FreeThrowsMade;
                        total = made + this.StatPlayer.FreeThrowsMissed;
                        if (total == 0)
                            pct = "-";
                        else
                            pct = String.Format("{0:P0}", (Decimal)made / total);
                        return pct;
                    case "3 Pointers":
                        made = this.StatPlayer.ThreePointsMade;
                        total = made + this.StatPlayer.ThreePointsMissed;
                        if (total == 0)
                            pct = "-";
                        else
                            pct = String.Format("{0:P0}", (Decimal)made / total);
                        return pct;
                    default:
                        return "";
                }

            }
        }
    }
    public class CompTeamStat : ITHSStat
    {
        public string Description
        {
            get
            {
                string ret = $"{this.HomeSchoolName} {this.StatLabel} {this.HomeStat1}, {this.HomeStat2}, {this.HomeStat3}";
                ret += "|";
                ret += $"{this.VisitorSchoolName} {this.StatLabel} {this.VisitorStat1}, {this.VisitorStat2}, {this.VisitorStat3} ";
                return ret;
            }
        }
        public string StatCode { get; set; }
        public string Id { get; set; }

        private Game StatGame { get; set; }

        public string HomeSchoolName { get; set; }
        public string VisitorSchoolName { get; set; }
        public string HomeBackgroundColor { get; set; }
        public string HomeTextColor { get; set; }
        public string HomeLogoFileName { get; set; }
        public string VisitorBackgroundColor { get; set; }
        public string VisitorTextColor { get; set; }
        public string VisitorLogoFileName { get; set; }
        public int HomeFirstHalfFouls { get; set; }
        public int VisitorFirstHalfFouls { get; set; }

        public CompTeamStat(string statCode, string statId)
        {
            this.StatCode = statCode;
            this.Id = statId;

        }

        public void UpdateStat(Game game)
        {
            this.HomeSchoolName = game.HomeSchool.Name;
            this.HomeBackgroundColor = game.HomeSchool.BackgroundColor;
            this.HomeTextColor = game.HomeSchool.TextColor;
            this.HomeLogoFileName = game.HomeSchool.LogoFileName;

            this.VisitorSchoolName = game.VisitorSchool.Name;
            this.VisitorBackgroundColor = game.VisitorSchool.BackgroundColor;
            this.VisitorTextColor = game.VisitorSchool.TextColor;
            this.VisitorLogoFileName = game.VisitorSchool.LogoFileName;

            this.StatGame = game;

        }
        public string StatLabel
        {
            get
            {
                switch (this.StatCode)
                {
                    case "Team Field Goals":
                        return "Field Goals";
                    case "Team Free Throws":
                        return "Free Throws";
                    case "Team 3 Pointers":
                        return "3 Pointers";
                    case "Team Fouls":
                        return "Team Fouls";
                    default:
                        return "";
                }

            }
        }
        public string HomeStat1
        {
            get
            {
                Int32 made;
                Int32 total;
                switch (this.StatCode)
                {
                    case "Team Field Goals":
                        made = this.StatGame.HomeRoster.Sum(p => p.TwoPointsMade + p.ThreePointsMade);
                        total = made + this.StatGame.HomeRoster.Sum(p => p.TwoPointsMissed + p.ThreePointsMissed);
                        return made.ToString() + "/" + total.ToString();
                    case "Team Free Throws":
                        made = this.StatGame.HomeRoster.Sum(p => p.FreeThrowsMade);
                        total = made + this.StatGame.HomeRoster.Sum(p => p.FreeThrowsMissed);
                        return made.ToString() + "/" + total.ToString();
                    case "Team 3 Pointers":
                        made = this.StatGame.HomeRoster.Sum(p => p.ThreePointsMade);
                        total = made + this.StatGame.HomeRoster.Sum(p => p.ThreePointsMissed);
                        return made.ToString() + "/" + total.ToString();
                    default:
                        return "";
                }

            }
        }
        public string HomeStat2
        {
            get
            {
                Int32 made;
                Int32 total;
                string pct;

                switch (this.StatCode)
                {
                    case "Team Field Goals":
                        made = this.StatGame.HomeRoster.Sum(p => p.TwoPointsMade + p.ThreePointsMade);
                        total = made + this.StatGame.HomeRoster.Sum(p => p.TwoPointsMissed + p.ThreePointsMissed);
                        if (total == 0)
                            pct = "-";
                        else
                            pct= String.Format("{0:P0}", (Decimal) made / total);
                        return pct;
                    case "Team Free Throws":
                        made = this.StatGame.HomeRoster.Sum(p => p.FreeThrowsMade);
                        total = made + this.StatGame.HomeRoster.Sum(p => p.FreeThrowsMissed);
                        if (total == 0)
                            pct = "-";
                        else
                            pct=String.Format("{0:P0}", (Decimal)made / total);
                        return pct;
                    case "Team 3 Pointers":
                        made = this.StatGame.HomeRoster.Sum(p => p.ThreePointsMade);
                        total = made + this.StatGame.HomeRoster.Sum(p => p.ThreePointsMissed);
                        if (total == 0)
                            pct = "-";
                        else
                            pct = String.Format("{0:P0}", (Decimal)made / total);
                        return pct;

                    default:
                        return "";
                }

            }
        }

        public string VisitorStat1
        {
            get
            {
                Int32 made;
                Int32 total;
                switch (this.StatCode)
                {
                    case "Team Field Goals":
                        made = this.StatGame.VisitorRoster.Sum(p => p.TwoPointsMade + p.ThreePointsMade);
                        total = made + this.StatGame.VisitorRoster.Sum(p => p.TwoPointsMissed + p.ThreePointsMissed);
                        return made.ToString() + "/" + total.ToString();
                    case "Team Free Throws":
                        made = this.StatGame.VisitorRoster.Sum(p => p.FreeThrowsMade);
                        total = made + this.StatGame.VisitorRoster.Sum(p => p.FreeThrowsMissed);
                        return made.ToString() + "/" + total.ToString();
                    case "Team 3 Pointers":
                        made = this.StatGame.VisitorRoster.Sum(p => p.ThreePointsMade);
                        total = made + this.StatGame.VisitorRoster.Sum(p => p.ThreePointsMissed);
                        return made.ToString() + "/" + total.ToString();
                    default:
                        return "";
                }

            }
        }

        public string VisitorStat2
        {
            get
            {
                Int32 made;
                Int32 total;
                string pct;

                switch (this.StatCode)
                {
                    case "Team Field Goals":
                        made = this.StatGame.VisitorRoster.Sum(p => p.TwoPointsMade + p.ThreePointsMade);
                        total = made + this.StatGame.VisitorRoster.Sum(p => p.TwoPointsMissed + p.ThreePointsMissed);
                        if (total == 0)
                            pct = "-";
                        else
                            pct = String.Format("{0:P0}", (Decimal)made / total);
                        return pct;
                    case "Team Free Throws":
                        made = this.StatGame.VisitorRoster.Sum(p => p.FreeThrowsMade);
                        total = made + this.StatGame.VisitorRoster.Sum(p => p.FreeThrowsMissed);
                        if (total == 0)
                            pct = "-";
                        else
                            pct = String.Format("{0:P0}", (Decimal)made / total);
                        return pct;
                    case "Team 3 Pointers":
                        made = this.StatGame.VisitorRoster.Sum(p => p.ThreePointsMade);
                        total = made + this.StatGame.VisitorRoster.Sum(p => p.ThreePointsMissed);
                        if (total == 0)
                            pct = "-";
                        else
                            pct = String.Format("{0:P0}", (Decimal)made / total);
                        return pct;

                    default:
                        return "";
                }

            }
        }

        public string HomeStat3
        {
            get
            {
                Int32 made;
                switch (this.StatCode)
                {
                    case "Team Fouls":
                        made = this.StatGame.HomeRoster.Sum(p => p.Fouls);
                        made = made - HomeFirstHalfFouls;
                        if (made == 1)
                            return made + " Foul";
                        else
                            return made.ToString() + " Fouls";
                    default:
                        return "";
                }

            }

        }
        public string VisitorStat3
        {
            get
            {
                Int32 made;
                switch (this.StatCode)
                {
                    case "Team Fouls":
                        made = this.StatGame.VisitorRoster.Sum(p => p.Fouls);
                        made = made - VisitorFirstHalfFouls;
                        if (made == 1)
                            return made + " Foul";
                        else
                            return made.ToString() + " Fouls";
                    default:
                        return "";
                }

            }

        }
    }


    // Unique Stat and crawls
    public class OtherInfo
    {
        public string InfoText { get; set; }

        public OtherInfo(string infoText)
        {
            this.InfoText= infoText;
        }
    }
}
