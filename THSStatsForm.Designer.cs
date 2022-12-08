namespace THSStats
{
    partial class THSStatsForm
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
            this.StatsListbox = new System.Windows.Forms.ListBox();
            this.OtherListbox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RosterButton = new System.Windows.Forms.Button();
            this.StatInfoLabel = new System.Windows.Forms.Label();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.StatInfoLabel2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.UniqueStatBox = new System.Windows.Forms.TextBox();
            this.Crawl1Box = new System.Windows.Forms.TextBox();
            this.Crawl2Box = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Crawl3Box = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.UniqueStatPush = new System.Windows.Forms.Button();
            this.Crawl1Push = new System.Windows.Forms.Button();
            this.Crawl2Push = new System.Windows.Forms.Button();
            this.Crawl3Push = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatsListbox
            // 
            this.StatsListbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatsListbox.FormattingEnabled = true;
            this.StatsListbox.ItemHeight = 24;
            this.StatsListbox.Location = new System.Drawing.Point(6, 45);
            this.StatsListbox.Name = "StatsListbox";
            this.StatsListbox.Size = new System.Drawing.Size(260, 364);
            this.StatsListbox.TabIndex = 0;
            this.StatsListbox.SelectedIndexChanged += new System.EventHandler(this.StatsListbox_SelectedIndexChanged);
            // 
            // OtherListbox
            // 
            this.OtherListbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OtherListbox.FormattingEnabled = true;
            this.OtherListbox.ItemHeight = 24;
            this.OtherListbox.Location = new System.Drawing.Point(315, 45);
            this.OtherListbox.Name = "OtherListbox";
            this.OtherListbox.Size = new System.Drawing.Size(307, 364);
            this.OtherListbox.TabIndex = 1;
            this.OtherListbox.SelectedIndexChanged += new System.EventHandler(this.OtherListbox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Statistic";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(311, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 24);
            this.label2.TabIndex = 3;
            // 
            // RosterButton
            // 
            this.RosterButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RosterButton.Location = new System.Drawing.Point(547, 12);
            this.RosterButton.Name = "RosterButton";
            this.RosterButton.Size = new System.Drawing.Size(75, 30);
            this.RosterButton.TabIndex = 4;
            this.RosterButton.Text = "home";
            this.RosterButton.UseVisualStyleBackColor = true;
            this.RosterButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // StatInfoLabel
            // 
            this.StatInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatInfoLabel.ForeColor = System.Drawing.Color.Maroon;
            this.StatInfoLabel.Location = new System.Drawing.Point(2, 424);
            this.StatInfoLabel.Name = "StatInfoLabel";
            this.StatInfoLabel.Size = new System.Drawing.Size(616, 41);
            this.StatInfoLabel.TabIndex = 5;
            this.StatInfoLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // RefreshButton
            // 
            this.RefreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshButton.Location = new System.Drawing.Point(163, 12);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(103, 30);
            this.RefreshButton.TabIndex = 6;
            this.RefreshButton.Text = "Update Game";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // StatInfoLabel2
            // 
            this.StatInfoLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatInfoLabel2.ForeColor = System.Drawing.Color.Maroon;
            this.StatInfoLabel2.Location = new System.Drawing.Point(6, 456);
            this.StatInfoLabel2.Name = "StatInfoLabel2";
            this.StatInfoLabel2.Size = new System.Drawing.Size(616, 41);
            this.StatInfoLabel2.TabIndex = 7;
            this.StatInfoLabel2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(11, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(675, 548);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.tabPage1.Controls.Add(this.OtherListbox);
            this.tabPage1.Controls.Add(this.StatInfoLabel2);
            this.tabPage1.Controls.Add(this.StatInfoLabel);
            this.tabPage1.Controls.Add(this.StatsListbox);
            this.tabPage1.Controls.Add(this.RefreshButton);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.RosterButton);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(667, 522);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Statistics";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Crawl3Push);
            this.tabPage2.Controls.Add(this.Crawl2Push);
            this.tabPage2.Controls.Add(this.Crawl1Push);
            this.tabPage2.Controls.Add(this.UniqueStatPush);
            this.tabPage2.Controls.Add(this.Crawl3Box);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.Crawl2Box);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.Crawl1Box);
            this.tabPage2.Controls.Add(this.UniqueStatBox);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(667, 522);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Other Info";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Crawl #1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Unique Stat";
            // 
            // UniqueStatBox
            // 
            this.UniqueStatBox.Location = new System.Drawing.Point(75, 28);
            this.UniqueStatBox.Multiline = true;
            this.UniqueStatBox.Name = "UniqueStatBox";
            this.UniqueStatBox.Size = new System.Drawing.Size(510, 35);
            this.UniqueStatBox.TabIndex = 6;
            // 
            // Crawl1Box
            // 
            this.Crawl1Box.Location = new System.Drawing.Point(75, 69);
            this.Crawl1Box.Multiline = true;
            this.Crawl1Box.Name = "Crawl1Box";
            this.Crawl1Box.Size = new System.Drawing.Size(510, 35);
            this.Crawl1Box.TabIndex = 7;
            // 
            // Crawl2Box
            // 
            this.Crawl2Box.Location = new System.Drawing.Point(75, 110);
            this.Crawl2Box.Multiline = true;
            this.Crawl2Box.Name = "Crawl2Box";
            this.Crawl2Box.Size = new System.Drawing.Size(510, 35);
            this.Crawl2Box.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Crawl #2";
            // 
            // Crawl3Box
            // 
            this.Crawl3Box.Location = new System.Drawing.Point(75, 151);
            this.Crawl3Box.Multiline = true;
            this.Crawl3Box.Name = "Crawl3Box";
            this.Crawl3Box.Size = new System.Drawing.Size(510, 35);
            this.Crawl3Box.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Crawl #3";
            // 
            // UniqueStatPush
            // 
            this.UniqueStatPush.Location = new System.Drawing.Point(591, 34);
            this.UniqueStatPush.Name = "UniqueStatPush";
            this.UniqueStatPush.Size = new System.Drawing.Size(54, 23);
            this.UniqueStatPush.TabIndex = 12;
            this.UniqueStatPush.Text = "Push";
            this.UniqueStatPush.UseVisualStyleBackColor = true;
            // 
            // Crawl1Push
            // 
            this.Crawl1Push.Location = new System.Drawing.Point(591, 72);
            this.Crawl1Push.Name = "Crawl1Push";
            this.Crawl1Push.Size = new System.Drawing.Size(54, 23);
            this.Crawl1Push.TabIndex = 13;
            this.Crawl1Push.Text = "Push";
            this.Crawl1Push.UseVisualStyleBackColor = true;
            // 
            // Crawl2Push
            // 
            this.Crawl2Push.Location = new System.Drawing.Point(591, 113);
            this.Crawl2Push.Name = "Crawl2Push";
            this.Crawl2Push.Size = new System.Drawing.Size(54, 23);
            this.Crawl2Push.TabIndex = 14;
            this.Crawl2Push.Text = "Push";
            this.Crawl2Push.UseVisualStyleBackColor = true;
            // 
            // Crawl3Push
            // 
            this.Crawl3Push.Location = new System.Drawing.Point(591, 154);
            this.Crawl3Push.Name = "Crawl3Push";
            this.Crawl3Push.Size = new System.Drawing.Size(54, 23);
            this.Crawl3Push.TabIndex = 15;
            this.Crawl3Push.Text = "Push";
            this.Crawl3Push.UseVisualStyleBackColor = true;
            // 
            // THSStatsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 591);
            this.Controls.Add(this.tabControl1);
            this.Name = "THSStatsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "THS Stats Manager";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox StatsListbox;
        private System.Windows.Forms.ListBox OtherListbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button RosterButton;
        private System.Windows.Forms.Label StatInfoLabel;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Label StatInfoLabel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Crawl3Push;
        private System.Windows.Forms.Button Crawl2Push;
        private System.Windows.Forms.Button Crawl1Push;
        private System.Windows.Forms.Button UniqueStatPush;
        private System.Windows.Forms.TextBox Crawl3Box;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Crawl2Box;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Crawl1Box;
        private System.Windows.Forms.TextBox UniqueStatBox;
        private System.Windows.Forms.Label label6;
    }
}

