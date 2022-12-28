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

        private string[] playerStatCodes = { "Field Goals", "Free Throws", "3 Pointers", "Fouls","Offensive Rebounds","Defensive Rebounds","Rebounds" };
        private string[] compTeamStatCodes = { "Team Field Goals", "Team Free Throws", "Team 3 Pointers", "Team Fouls", "Team Offensive Rebounds", "Team Defensive Rebounds", "Team Rebounds" };
        private string[] leaderStatCodes = { "Scoring Leaders", "Foul Trouble","Rebound Leaders"};


        private ITHSStat currentStat = null;
        private List<ITHSStat> gameStats = new List<ITHSStat>();

        readonly string  ucFileName = "c:\\test\\ucsettings.data";

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
            StatsListbox.Items.Add("Rebounds");
            StatsListbox.Items.Add("Team Field Goals");
            StatsListbox.Items.Add("Team Free Throws");
            StatsListbox.Items.Add("Team 3 Pointers");
            StatsListbox.Items.Add("Team Fouls");
            StatsListbox.Items.Add("Team Rebounds");
            StatsListbox.Items.Add("Scoring Leaders");
            StatsListbox.Items.Add("Foul Trouble");
            StatsListbox.Items.Add("Rebound Leaders");

            this.rosterMode = "Home";
            this.RosterButton.Text = this.rosterMode;
            this.RosterButton.Visible = this.selectedStatMode == "Player";

            this.Unique1Push.Click += UpdateUniqueCrawl;
            this.Unique2Push.Click += UpdateUniqueCrawl;
            this.Unique3Push.Click += UpdateUniqueCrawl;
            this.Unique4Push.Click += UpdateUniqueCrawl;
            this.Unique5Push.Click += UpdateUniqueCrawl;
            this.Unique6Push.Click += UpdateUniqueCrawl;
            this.Crawl1Push.Click += UpdateUniqueCrawl;
            this.Crawl2Push.Click += UpdateUniqueCrawl;
            this.Crawl3Push.Click += UpdateUniqueCrawl;
            this.Crawl4Push.Click += UpdateUniqueCrawl;
            this.Crawl5Push.Click += UpdateUniqueCrawl;
            this.Crawl6Push.Click += UpdateUniqueCrawl;

            loadUniqueCrawlValues();
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

                this.game.AllPlayers.ForEach(p => p.SetStatItemData(this.game.AllStatItems));

                }
            
        }
        private void FillOtherListbox(string statMode, string rosterMode="")
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
                case "Leaders":
                    var leaderList = new List<Object>();
                    leaderList.Add(new { Description = "Both Teams", Id = "1" });
                    //leaderList.Add(new { Description = "Home Team", Id = "1" });
                    //leaderList.Add(new { Description = "Visiting Team", Id = "2" });
                    OtherListbox.DataSource = leaderList;
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
                    this.FillOtherListbox(this.selectedStatMode, this.rosterMode);
                else
                    createStat(game, selectedStatMode, selectedStatCode, selectedId);
            }

            else if (this.compTeamStatCodes.Contains(box.Text))
            {
                this.selectedStatMode = "CompTeam";
                label2.Text = "";
                if (currentStatMode != this.selectedStatMode)
                    this.FillOtherListbox(this.selectedStatMode);
                else
                    createStat(game, selectedStatMode, selectedStatCode, selectedId);
            }
            else if (this.leaderStatCodes.Contains(box.Text))
            {
                this.selectedStatMode = "Leaders";
                label2.Text = "";
                if (currentStatMode != this.selectedStatMode)
                    this.FillOtherListbox(this.selectedStatMode);
                else
                    createStat(game, selectedStatMode, selectedStatCode, selectedId);
            }

            else
            {
                this.selectedStatMode = "None";
                OtherListbox.DataSource = null;
                OtherListbox.DisplayMember = "Description";
            }

            this.RosterButton.Visible = this.selectedStatMode == "Player";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.rosterMode= (this.rosterMode=="Home" ? "Visitor": "Home");
            RosterButton.Text = this.rosterMode;
            FillOtherListbox(selectedStatMode,rosterMode);
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
            {
                UpdateStat();
                return;
            }
                

            switch (this.selectedStatMode)
            {
                case "Player":
                    var player = this.game.AllPlayers.Single(p => p.PlayerId == this.selectedId);
                    currentStat = new IndividualStat(this.selectedStatCode, this.selectedId);
                    UpdateStat();
                    break;
                case "CompTeam":
                    currentStat = new CompTeamStat(this.selectedStatCode, this.selectedId);
                    UpdateStat();
                    break;
                case "Leaders":
                    currentStat = new LeadersTeamStat(this.selectedStatCode, this.selectedId);
                    UpdateStat();
                    break;
                default:
                    currentStat = null;
                    UpdateStat();
                    break;

                    //this.gameStats.Add(new IndividualStat(this.selectedStatMode, this.selectedId));
            }
        }
        private void UpdateStat()
        {
            if (currentStat == null)
            {
                StatInfoLabel.Text = "";
                StatInfoLabel2.Text = "";
                return;
            }
                

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
            string buttonName = ((Button)sender).Name;
            string textControlName = buttonName.Replace("Push", "Box");

            if (buttonName.ToLower().Contains("crawl"))
                filePrefix = "Crawl";
            else
                filePrefix = "UniqueStat";

            infoText = ((TextBox)(this.Controls.Find(textControlName,true)[0])).Text.Trim();

            OtherInfo info = new OtherInfo(infoText);

            string json = JsonSerializer.Serialize(info);
            json = "[" + json + "]";

            System.IO.File.WriteAllText($"c:\\test\\"+filePrefix+".json", json);
            MessageBox.Show("Update complete");
        }
        private void saveUniqueCrawlValues()
        {
            string uniqueValues = "";
            string crawlValues = "";

            for (int i = 1; i <= 6; i++)
            {
                string unique = ((TextBox)(this.Controls.Find("Unique"+i.ToString()+"Box", true)[0])).Text.Trim();
                string crawl = ((TextBox)(this.Controls.Find("Crawl" + i.ToString() + "Box", true)[0])).Text.Trim();
                unique = unique == "" ? "Empty" : unique;
                crawl = crawl == "" ? "Empty" : crawl;
                uniqueValues = uniqueValues + (i < 6 ? unique + "|" : unique);
                crawlValues = crawlValues + (i < 6 ? crawl + "|" : crawl);
            }
            File.WriteAllText(ucFileName, uniqueValues + "\r" + crawlValues);
        }

        private void loadUniqueCrawlValues()
        {
            
            if (File.Exists(ucFileName))
            {
                string fileInfo = System.IO.File.ReadAllText(ucFileName);
                string[] rows = fileInfo.Split('\r');
                string[] uniques = rows[0].Split('|');
                string[] crawls = rows[1].Split('|');

                Unique1Box.Text = uniques[0] == "Empty" ? "" : uniques[0];
                Unique2Box.Text = uniques[1] == "Empty" ? "" : uniques[1];
                Unique3Box.Text = uniques[2] == "Empty" ? "" : uniques[2];
                Unique4Box.Text = uniques[3] == "Empty" ? "" : uniques[3];
                Unique5Box.Text = uniques[4] == "Empty" ? "" : uniques[4];
                Unique6Box.Text = uniques[5] == "Empty" ? "" : uniques[5];

                Crawl1Box.Text = crawls[0] == "Empty" ? "" : crawls[0];
                Crawl2Box.Text = crawls[1] == "Empty" ? "" : crawls[1];
                Crawl3Box.Text = crawls[2] == "Empty" ? "" : crawls[2];
                Crawl4Box.Text = crawls[3] == "Empty" ? "" : crawls[3];
                Crawl5Box.Text = crawls[4] == "Empty" ? "" : crawls[4];
                Crawl6Box.Text = crawls[5] == "Empty" ? "" : crawls[5];

            }
        }

        private void THSStatsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveUniqueCrawlValues();
        }
    }
    public class Game
    {
        public List<Player> _allPlayers = null;
        public List<School> _allSchools = null;
        public List<StatItem> _allStatItems = null;

        public School HomeSchool { get; set; }
        public School VisitorSchool { get; set; }
        public List<Player> HomeRoster { get; set; }
        public List<Player> VisitorRoster { get; set; }
        public List<StatItem> PointStatItems { get; set; }
        public List<StatItem> ReboundStatItems { get; set; }

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
        public List<StatItem> AllStatItems
        {
            get
            {
                _allStatItems = new List<StatItem>();

                if (this.PointStatItems != null)
                    _allStatItems = _allStatItems.Concat(this.PointStatItems).ToList();

                if (this.ReboundStatItems != null)
                    _allStatItems = _allStatItems.Concat(this.ReboundStatItems).ToList();

                return _allStatItems;
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

        public void SetStatItemData(List<StatItem> items)
        {
            this.Fouls = items.Where(i => i.PlayerId == this.PlayerId && i.StatCode == "FOUL").Count();
            this.FreeThrowsMade = items.Where(i => i.PlayerId == this.PlayerId && i.StatCode == "FT").Count();
            this.FreeThrowsMissed = items.Where(i => i.PlayerId == this.PlayerId && i.StatCode == "FTM").Count();
            this.TwoPointsMade = items.Where(i => i.PlayerId == this.PlayerId && i.StatCode == "2P").Count();
            this.TwoPointsMissed = items.Where(i => i.PlayerId == this.PlayerId && i.StatCode == "2PM").Count();
            this.ThreePointsMade = items.Where(i => i.PlayerId == this.PlayerId && i.StatCode == "3P").Count();
            this.ThreePointsMissed = items.Where(i => i.PlayerId == this.PlayerId && i.StatCode == "3PM").Count();
            this.OffRebounds= items.Where(i => i.PlayerId == this.PlayerId && i.StatCode == "OREB").Count();
            this.DefRebounds = items.Where(i => i.PlayerId == this.PlayerId && i.StatCode == "DREB").Count();

            this.Points = this.FreeThrowsMade + 2 * this.TwoPointsMade + 3 * this.ThreePointsMade;
            this.Rebounds = this.OffRebounds + this.DefRebounds;

        }

    }

    public class StatItem
    {
        public string SchoolId { get; set; }
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string StatCode { get; set; }
        public string Period { get; set; }

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
                    case "Rebounds":
                        return "Def";
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
                    case "Rebounds":
                        return this.StatPlayer.DefRebounds.ToString();
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
                    case "Rebounds":
                        return "Off";
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
                    case "Rebounds":
                        return this.StatPlayer.OffRebounds.ToString();
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
                    case "Rebounds":
                        return "Total";
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
                    case "Rebounds":
                        return this.StatPlayer.Rebounds.ToString();
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
                    case "Team Rebounds":
                        return "Rebounds DEF/OFF/TOT";
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
                    case "Team Rebounds":
                        return this.StatGame.HomeRoster.Sum(p => p.DefRebounds).ToString();
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
                    case "Team Rebounds":
                        return this.StatGame.HomeRoster.Sum(p => p.Rebounds).ToString();
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
                    case "Team Rebounds":
                        return this.StatGame.VisitorRoster.Sum(p => p.DefRebounds).ToString();
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
                    case "Team Rebounds":
                        return this.StatGame.VisitorRoster.Sum(p => p.Rebounds).ToString();
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
                    case "Team Rebounds":
                        return this.StatGame.HomeRoster.Sum(p => p.OffRebounds).ToString();
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
                    case "Team Rebounds":
                        return this.StatGame.VisitorRoster.Sum(p => p.OffRebounds).ToString();
                    default:
                        return "";
                }

            }

        }
    }

    public class LeadersTeamStat : ITHSStat
    {
        public string StatCode { get; set; }
        public string Id { get; set; }

        private Game StatGame { get; set; }
        
        public string StatHeader
        {
            get
            {
                return this.StatCode;
            }
        }
        public string Jersey1 { get; set; }
        public string FirstName1 { get; set; }
        public string LastName1 { get; set; }
        public string Stat1 { get; set; }
        public string Logo1 { get; set; }

        public string Jersey2 { get; set; }
        public string FirstName2 { get; set; }
        public string LastName2 { get; set; }
        public string Stat2 { get; set; }
        public string Logo2 { get; set; }

        public string Jersey3 { get; set; }
        public string FirstName3 { get; set; }
        public string LastName3 { get; set; }
        public string Stat3 { get; set; }
        public string Logo3 { get; set; }

        public LeadersTeamStat(string statCode, string statId)
        {
            this.StatCode = statCode;
            this.Id = statId;

        }
        public void UpdateStat(Game game)
        {

            List<Player> leaders;

            switch (this.StatCode)
            {
                case "Scoring Leaders":
                    leaders = game.AllPlayers.OrderByDescending(p => p.Points).ToList();
                    break;
                case "Foul Trouble":
                    leaders = game.AllPlayers.OrderByDescending(p => p.Fouls).ToList();
                    break;
                case "Rebound Leaders":
                    leaders = game.AllPlayers.OrderByDescending(p => p.Rebounds).ToList();
                    break;
                default:
                    leaders = game.AllPlayers.OrderByDescending(p => p.Points).ToList();
                    break;
            }


            this.FirstName1 = leaders[0].FirstName;
            this.LastName1 = leaders[0].LastName;
            this.Jersey1 = leaders[0].Jersey;
            this.Stat1 = getStatValue(leaders[0]);
            this.Logo1 = leaders[0].SchoolId == game.HomeSchool.SchoolId ? game.HomeSchool.LogoFileName : game.VisitorSchool.LogoFileName;

            this.FirstName2 = leaders[1].FirstName;
            this.LastName2 = leaders[1].LastName;
            this.Jersey2 = leaders[1].Jersey;
            this.Stat2 = getStatValue(leaders[1]);
            this.Logo2 = leaders[1].SchoolId == game.HomeSchool.SchoolId ? game.HomeSchool.LogoFileName : game.VisitorSchool.LogoFileName;

            this.FirstName3 = leaders[2].FirstName;
            this.LastName3 = leaders[2].LastName;
            this.Jersey3 = leaders[2].Jersey;
            this.Stat3 = getStatValue(leaders[2]);
            this.Logo3 = leaders[2].SchoolId == game.HomeSchool.SchoolId ? game.HomeSchool.LogoFileName : game.VisitorSchool.LogoFileName;

        }

        private string getStatValue(Player player)
        {
            switch (this.StatCode)
            {
                case "Scoring Leaders":
                    return player.Points.ToString();
                case "Foul Trouble":
                    return player.Fouls.ToString();
                case "Rebound Leaders":
                    return player.Rebounds.ToString();
                default:
                    return 0.ToString();
            }
        }
        public string Description
        {
            get
            {
                //string ret = $"{this.HomeSchoolName} {this.StatLabel} {this.HomeStat1}, {this.HomeStat2}, {this.HomeStat3}";
                //ret += "|";
                //ret += $"{this.VisitorSchoolName} {this.StatLabel} {this.VisitorStat1}, {this.VisitorStat2}, {this.VisitorStat3} ";
                //return ret;
                string ret = $"{this.LastName1} ({this.Stat1}), {this.LastName2} ({this.Stat2}),{this.LastName3} ({this.Stat3})";
                return ret;
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
