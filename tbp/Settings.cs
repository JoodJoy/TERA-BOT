
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace tbp
{
  public class Settings : Form
  {
    private Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config\\config.json"));
    private List<string> gatherWhiteList = new List<string>();
    private List<string> gatherBlackList = new List<string>();
    private List<string> pickupList = new List<string>();
    private List<string> mobExcludeList = new List<string>();
    private IContainer components;
    private Label roamTimeL;
    private NumericUpDown roamTimeBox;
    private Label roamDistanceL;
    private NumericUpDown roamDistanceBox;
    private Button saveButton;
    private ListBox gatherWhiteListBox;
    private ListBox gatherBlackListBox;
    private ListBox filterPickupListBox;
    private Button moveLeftButton;
    private Button moveRightButton;
    private TextBox filterPickupTextBox;
    private Button addPickupButton;
    private ComboBox classTypeBox;
    private GroupBox groupBox1;
    private CheckBox RetreatCheckBox;
    private Label label4;
    private GroupBox groupBox2;
    private Button removePickupButton;
    private CheckBox HealCheckBox;
    private NumericUpDown attRangeBox;
    private Label label5;
    private GroupBox groupBox3;
    private CheckBox useMountCheckBox;
    private NumericUpDown KiteDistanceBox;
    private CheckBox KiteCheckBox;
    private NumericUpDown RestMaxBox;
    private NumericUpDown RestMinBox;
    private CheckBox RestCheckBox;
    private NumericUpDown HealMinBox;
    private NumericUpDown RetreatMinBox;
    private Button removeMobButton;
    private Button addMobButton;
    private TextBox filterMobTextBox;
    private ListBox filterMobsListBox;
    private Button cancelButton;
    private GroupBox groupBox4;
    private NumericUpDown inventStartRowBox;
    private NumericUpDown inventTotalRowsBox;
    private Label label8;
    private Label label7;
    private NumericUpDown inventStartColumnBox;
    private Label spaceCountL;
    private CheckBox RestSkillCheckBox;
    private GroupBox groupBox5;
    private RadioButton euRb;
    private RadioButton usRb;
    private CheckBox filterPickupsCheckbox;
    private GroupBox pickupGb;
    private RadioButton pickupExcludeRb;
    private RadioButton pickupIncludeRb;
    private GroupBox gatherGb;
    private CheckBox dontGatherCheckbox;
    private Label label2;
    private Label label1;
    private GroupBox groupBox6;
    private RadioButton mobExcludeRb;
    private RadioButton mobIncludeRb;
    private CheckBox filterMobsCheckbox;
    private Label label6;
    private GroupBox groupBox7;
    private Label label3;
    private Label label9;
    private GroupBox groupBox8;
    private ComboBox kiteModeBox;

    public Settings()
    {
      this.InitializeComponent();
      this.loadFiles();
      this.fillBoxes();
      this.managePickup();
      this.manageMobs();
    }

    private void loadClient()
    {
      if (this.config.client == "US")
      {
        this.usRb.Checked = true;
      }
      else
      {
        if (!(this.config.client == "EU"))
          return;
        this.euRb.Checked = true;
      }
    }

    private void setClient()
    {
      if (this.usRb.Checked)
      {
        this.config.client = "US";
      }
      else
      {
        if (!this.euRb.Checked)
          return;
        this.config.client = "EU";
      }
    }

    private void loadFilterModes()
    {
      if (this.config.pickupFilterMode == "include")
      {
        this.pickupIncludeRb.Checked = true;
        this.pickupExcludeRb.Checked = false;
      }
      else if (this.config.pickupFilterMode == "exclude")
      {
        this.pickupExcludeRb.Checked = true;
        this.pickupIncludeRb.Checked = false;
      }
      if (this.config.mobFilterMode == "include")
      {
        this.mobIncludeRb.Checked = true;
        this.mobExcludeRb.Checked = false;
      }
      else
      {
        if (!(this.config.mobFilterMode == "exclude"))
          return;
        this.mobExcludeRb.Checked = true;
        this.mobIncludeRb.Checked = false;
      }
    }

    private void setFilterModes()
    {
      this.config.pickupFilterMode = !this.pickupIncludeRb.Checked ? "exclude" : "include";
      if (this.mobIncludeRb.Checked)
        this.config.mobFilterMode = "include";
      else
        this.config.mobFilterMode = "exclude";
    }

    private void manageClassType()
    {
      if (this.classTypeBox.Text == "ranged")
      {
        this.attRangeBox.Value = new Decimal(300);
        this.KiteDistanceBox.Value = new Decimal(250);
        this.KiteCheckBox.Checked = true;
      }
      else
      {
        if (!(this.classTypeBox.Text == "melee"))
          return;
        this.attRangeBox.Value = new Decimal(100);
        this.KiteCheckBox.Checked = false;
      }
    }

    private void managePickup()
    {
      if (!this.filterPickupsCheckbox.Checked)
      {
        this.filterPickupListBox.Enabled = true;
        this.filterPickupTextBox.Enabled = true;
        this.addPickupButton.Enabled = true;
        this.removePickupButton.Enabled = true;
        this.pickupIncludeRb.Enabled = true;
        this.pickupExcludeRb.Enabled = true;
      }
      else
      {
        this.filterPickupListBox.Enabled = false;
        this.filterPickupTextBox.Enabled = false;
        this.addPickupButton.Enabled = false;
        this.removePickupButton.Enabled = false;
        this.pickupIncludeRb.Enabled = false;
        this.pickupExcludeRb.Enabled = false;
      }
      if (this.pickupIncludeRb.Checked)
      {
        this.filterPickupListBox.ForeColor = Color.Lime;
      }
      else
      {
        if (!this.pickupExcludeRb.Checked)
          return;
        this.filterPickupListBox.ForeColor = Color.Red;
      }
    }

    private void manageMobs()
    {
      if (!this.filterMobsCheckbox.Checked)
      {
        this.filterMobsListBox.Enabled = true;
        this.filterMobTextBox.Enabled = true;
        this.addMobButton.Enabled = true;
        this.removeMobButton.Enabled = true;
        this.mobIncludeRb.Enabled = true;
        this.mobExcludeRb.Enabled = true;
      }
      else
      {
        this.filterMobsListBox.Enabled = false;
        this.filterMobTextBox.Enabled = false;
        this.addMobButton.Enabled = false;
        this.removeMobButton.Enabled = false;
        this.mobIncludeRb.Enabled = false;
        this.mobExcludeRb.Enabled = false;
      }
      if (this.mobIncludeRb.Checked)
      {
        this.filterMobsListBox.ForeColor = Color.Lime;
      }
      else
      {
        if (!this.mobExcludeRb.Checked)
          return;
        this.filterMobsListBox.ForeColor = Color.Red;
      }
    }

    private void manageGather()
    {
      if (!this.dontGatherCheckbox.Checked)
      {
        this.gatherWhiteListBox.Enabled = true;
        this.gatherBlackListBox.Enabled = true;
        this.moveLeftButton.Enabled = true;
        this.moveRightButton.Enabled = true;
      }
      else
      {
        this.gatherWhiteListBox.Enabled = false;
        this.gatherBlackListBox.Enabled = false;
        this.moveLeftButton.Enabled = false;
        this.moveRightButton.Enabled = false;
      }
    }

    private void loadKiteMode()
    {
      if (this.config.kiteMode == 1)
        this.kiteModeBox.Text = "V";
      else if (this.config.kiteMode == 2)
      {
        this.kiteModeBox.Text = "V + <";
      }
      else
      {
        if (this.config.kiteMode != 3)
          return;
        this.kiteModeBox.Text = "V + >";
      }
    }

    private void setKiteMode()
    {
      if (this.kiteModeBox.Text == "V")
        this.config.kiteMode = 1;
      else if (this.kiteModeBox.Text == "V + <")
      {
        this.config.kiteMode = 2;
      }
      else
      {
        if (!(this.kiteModeBox.Text == "V + >"))
          return;
        this.config.kiteMode = 3;
      }
    }

    private void loadFiles()
    {
      if (JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\gatherWhiteList.json")) != null)
        this.gatherWhiteList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\gatherWhiteList.json"));
      if (JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\gatherBlackList.json")) != null)
        this.gatherBlackList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\gatherBlackList.json"));
      if (JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\pickupList.json")) != null)
        this.pickupList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\pickupList.json"));
      if (JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\mobExcludeList.json")) != null)
        this.mobExcludeList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\mobExcludeList.json"));
      this.filterPickupsCheckbox.Checked = this.config.dontFilterPickups;
      this.filterMobsCheckbox.Checked = this.config.dontFilterMobs;
      this.dontGatherCheckbox.Checked = this.config.dontGather;
      this.roamTimeBox.Value = (Decimal) this.config.roamDuration;
      this.roamDistanceBox.Value = (Decimal) ((int) this.config.roamDist);
      this.KiteDistanceBox.Value = (Decimal) ((int) this.config.kiteDistance);
      this.attRangeBox.Value = (Decimal) ((int) this.config.attackDist);
      this.classTypeBox.Text = this.config.classType;
      this.useMountCheckBox.Checked = this.config.useMount;
      this.HealCheckBox.Checked = this.config.useHealSkill;
      this.HealMinBox.Value = (Decimal) this.config.HealMin;
      this.RetreatCheckBox.Checked = this.config.useRetreatSkill;
      this.RetreatMinBox.Value = (Decimal) ((int) this.config.RetreatDist);
      this.KiteCheckBox.Checked = this.config.useAutoKite;
      this.KiteDistanceBox.Enabled = this.config.useAutoKite;
      this.RestMaxBox.Value = (Decimal) this.config.RestMax;
      this.RestMinBox.Value = (Decimal) this.config.RestMin;
      this.RestCheckBox.Checked = this.config.useAutoRest;
      this.RestSkillCheckBox.Checked = this.config.useRestSkill;
      this.RestMinBox.Enabled = this.config.useAutoRest;
      this.RestMaxBox.Enabled = this.config.useAutoRest;
      this.HealMinBox.Enabled = this.config.useHealSkill;
      this.RetreatMinBox.Enabled = this.config.useRetreatSkill;
      this.inventStartRowBox.Value = (Decimal) this.config.inventStartRow;
      this.inventStartColumnBox.Value = (Decimal) this.config.inventStartColumn;
      this.inventTotalRowsBox.Value = (Decimal) this.config.inventTotalRows;
      this.spaceCountL.Text = "(" + (object) (this.inventTotalRowsBox.Value * new Decimal(8)) + " spaces)";
      this.loadClient();
      this.loadFilterModes();
      this.loadKiteMode();
    }

    private void fillBoxes()
    {
      if (this.gatherWhiteList.Count > 0)
      {
        this.gatherWhiteListBox.Items.Clear();
        for (int index = 0; index < this.gatherWhiteList.Count; ++index)
          this.gatherWhiteListBox.Items.Insert(index, (object) this.gatherWhiteList[index]);
      }
      if (this.gatherBlackList != null)
      {
        this.gatherBlackListBox.Items.Clear();
        for (int index = 0; index < this.gatherBlackList.Count; ++index)
          this.gatherBlackListBox.Items.Insert(index, (object) this.gatherBlackList[index]);
      }
      if (this.pickupList.Count > 0)
      {
        this.filterPickupListBox.Items.Clear();
        for (int index = 0; index < this.pickupList.Count; ++index)
          this.filterPickupListBox.Items.Insert(index, (object) this.pickupList[index]);
      }
      if (this.mobExcludeList == null)
        return;
      this.filterMobsListBox.Items.Clear();
      for (int index = 0; index < this.mobExcludeList.Count; ++index)
        this.filterMobsListBox.Items.Insert(index, (object) this.mobExcludeList[index]);
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
      this.config.roamDuration = (int) this.roamTimeBox.Value;
      this.config.roamDist = (float) this.roamDistanceBox.Value;
      this.config.attackDist = (float) this.attRangeBox.Value;
      this.config.kiteDistance = (float) this.KiteDistanceBox.Value;
      this.config.classType = this.classTypeBox.Text;
      this.config.useMount = this.useMountCheckBox.Checked;
      this.config.useRetreatSkill = this.RetreatCheckBox.Checked;
      this.config.useHealSkill = this.HealCheckBox.Checked;
      this.config.HealMin = (int) this.HealMinBox.Value;
      this.config.useAutoKite = this.KiteCheckBox.Checked;
      this.config.useAutoRest = this.RestCheckBox.Checked;
      this.config.RestMin = (int) this.RestMinBox.Value;
      this.config.RestMax = (int) this.RestMaxBox.Value;
      this.config.useRestSkill = this.RestSkillCheckBox.Checked;
      this.config.RetreatDist = (float) this.RetreatMinBox.Value;
      this.config.inventStartRow = (int) this.inventStartRowBox.Value;
      this.config.inventStartColumn = (int) this.inventStartColumnBox.Value;
      this.config.inventTotalRows = (int) this.inventTotalRowsBox.Value;
      this.config.dontFilterPickups = this.filterPickupsCheckbox.Checked;
      this.config.dontFilterMobs = this.filterMobsCheckbox.Checked;
      this.config.dontGather = this.dontGatherCheckbox.Checked;
      this.setClient();
      this.setFilterModes();
      this.setKiteMode();
      File.WriteAllText("config\\config.json", JsonConvert.SerializeObject((object) this.config));
      File.WriteAllText("config\\gatherWhiteList.json", JsonConvert.SerializeObject((object) this.gatherWhiteList));
      File.WriteAllText("config\\gatherBlackList.json", JsonConvert.SerializeObject((object) this.gatherBlackList));
      File.WriteAllText("config\\pickupList.json", JsonConvert.SerializeObject((object) this.pickupList));
      File.WriteAllText("config\\mobExcludeList.json", JsonConvert.SerializeObject((object) this.mobExcludeList));
      ((MainUI) this.Owner).loadFiles();
      this.Close();
    }

    private void moveLeftButton_Click(object sender, EventArgs e)
    {
      if (this.gatherBlackListBox.SelectedIndex == -1)
        return;
      this.gatherWhiteList.Add(this.gatherBlackList[this.gatherBlackListBox.SelectedIndex]);
      this.gatherBlackList.Remove(this.gatherBlackList[this.gatherBlackListBox.SelectedIndex]);
      this.fillBoxes();
    }

    private void moveRightButton_Click(object sender, EventArgs e)
    {
      if (this.gatherWhiteListBox.SelectedIndex == -1)
        return;
      this.gatherBlackList.Add(this.gatherWhiteList[this.gatherWhiteListBox.SelectedIndex]);
      this.gatherWhiteList.Remove(this.gatherWhiteList[this.gatherWhiteListBox.SelectedIndex]);
      this.fillBoxes();
    }

    private void gatherWhiteListBox_Click(object sender, EventArgs e)
    {
      if (this.gatherBlackListBox.SelectedIndex == -1)
        return;
      this.gatherBlackListBox.SetSelected(this.gatherBlackListBox.SelectedIndex, false);
    }

    private void gatherBlackListBox_Click(object sender, EventArgs e)
    {
      if (this.gatherWhiteListBox.SelectedIndex == -1)
        return;
      this.gatherWhiteListBox.SetSelected(this.gatherWhiteListBox.SelectedIndex, false);
    }

    private void addPickupButton_Click(object sender, EventArgs e)
    {
      this.pickupList.Add(this.filterPickupTextBox.Text);
      this.fillBoxes();
      this.managePickup();
    }

    private void removePickupButton_Click(object sender, EventArgs e)
    {
      if (this.filterPickupListBox.SelectedIndex == -1)
        return;
      this.pickupList.RemoveAt(this.filterPickupListBox.SelectedIndex);
      this.fillBoxes();
    }

    private void excludeMobAddButton_Click(object sender, EventArgs e)
    {
      this.mobExcludeList.Add(this.filterMobTextBox.Text);
      this.fillBoxes();
      this.manageMobs();
    }

    private void excludeMobRemoveButton_Click(object sender, EventArgs e)
    {
      this.mobExcludeList.RemoveAt(this.filterMobsListBox.SelectedIndex);
      this.fillBoxes();
    }

    private void useAutoKite_CheckedChanged(object sender, EventArgs e)
    {
      if (this.KiteCheckBox.Checked)
        this.KiteDistanceBox.Enabled = true;
      else
        this.KiteDistanceBox.Enabled = false;
    }

    private void RetreatCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.RetreatCheckBox.Checked)
        this.RetreatMinBox.Enabled = true;
      else
        this.RetreatMinBox.Enabled = false;
    }

    private void HealCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.HealCheckBox.Checked)
        this.HealMinBox.Enabled = true;
      else
        this.HealMinBox.Enabled = false;
    }

    private void RestCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.RestCheckBox.Checked)
      {
        this.RestMinBox.Enabled = true;
        this.RestMaxBox.Enabled = true;
      }
      else
      {
        this.RestMinBox.Enabled = false;
        this.RestMaxBox.Enabled = false;
      }
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void inventTotalRowsBox_ValueChanged(object sender, EventArgs e)
    {
      this.spaceCountL.Text = "(" + (object) (this.inventTotalRowsBox.Value * new Decimal(8)) + " slots)";
    }

    private void usRb_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void filterPickupsCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      this.managePickup();
    }

    private void classTypeBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.manageClassType();
    }

    private void pickupIncludeRb_CheckedChanged(object sender, EventArgs e)
    {
      this.managePickup();
    }

    private void mobIncludeRb_CheckedChanged(object sender, EventArgs e)
    {
      this.manageMobs();
    }

    private void filterMobsCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      this.manageMobs();
    }

    private void dontGatherCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      this.manageGather();
    }

    private void kiteModeBox_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.roamTimeL = new System.Windows.Forms.Label();
            this.roamTimeBox = new System.Windows.Forms.NumericUpDown();
            this.roamDistanceL = new System.Windows.Forms.Label();
            this.roamDistanceBox = new System.Windows.Forms.NumericUpDown();
            this.saveButton = new System.Windows.Forms.Button();
            this.gatherWhiteListBox = new System.Windows.Forms.ListBox();
            this.gatherBlackListBox = new System.Windows.Forms.ListBox();
            this.filterPickupListBox = new System.Windows.Forms.ListBox();
            this.moveLeftButton = new System.Windows.Forms.Button();
            this.moveRightButton = new System.Windows.Forms.Button();
            this.filterPickupTextBox = new System.Windows.Forms.TextBox();
            this.addPickupButton = new System.Windows.Forms.Button();
            this.classTypeBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.kiteModeBox = new System.Windows.Forms.ComboBox();
            this.RestSkillCheckBox = new System.Windows.Forms.CheckBox();
            this.RestMaxBox = new System.Windows.Forms.NumericUpDown();
            this.RestMinBox = new System.Windows.Forms.NumericUpDown();
            this.RestCheckBox = new System.Windows.Forms.CheckBox();
            this.HealMinBox = new System.Windows.Forms.NumericUpDown();
            this.RetreatMinBox = new System.Windows.Forms.NumericUpDown();
            this.KiteDistanceBox = new System.Windows.Forms.NumericUpDown();
            this.KiteCheckBox = new System.Windows.Forms.CheckBox();
            this.HealCheckBox = new System.Windows.Forms.CheckBox();
            this.attRangeBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.RetreatCheckBox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.removePickupButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.useMountCheckBox = new System.Windows.Forms.CheckBox();
            this.removeMobButton = new System.Windows.Forms.Button();
            this.addMobButton = new System.Windows.Forms.Button();
            this.filterMobTextBox = new System.Windows.Forms.TextBox();
            this.filterMobsListBox = new System.Windows.Forms.ListBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.spaceCountL = new System.Windows.Forms.Label();
            this.inventStartColumnBox = new System.Windows.Forms.NumericUpDown();
            this.inventStartRowBox = new System.Windows.Forms.NumericUpDown();
            this.inventTotalRowsBox = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.euRb = new System.Windows.Forms.RadioButton();
            this.usRb = new System.Windows.Forms.RadioButton();
            this.filterPickupsCheckbox = new System.Windows.Forms.CheckBox();
            this.pickupGb = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.pickupExcludeRb = new System.Windows.Forms.RadioButton();
            this.pickupIncludeRb = new System.Windows.Forms.RadioButton();
            this.gatherGb = new System.Windows.Forms.GroupBox();
            this.dontGatherCheckbox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.mobIncludeRb = new System.Windows.Forms.RadioButton();
            this.mobExcludeRb = new System.Windows.Forms.RadioButton();
            this.filterMobsCheckbox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.roamTimeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roamDistanceBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RestMaxBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RestMinBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HealMinBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RetreatMinBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KiteDistanceBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attRangeBox)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inventStartColumnBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventStartRowBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventTotalRowsBox)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.pickupGb.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.gatherGb.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // roamTimeL
            // 
            this.roamTimeL.AutoSize = true;
            this.roamTimeL.Location = new System.Drawing.Point(9, 24);
            this.roamTimeL.Name = "roamTimeL";
            this.roamTimeL.Size = new System.Drawing.Size(228, 13);
            this.roamTimeL.TabIndex = 0;
            this.roamTimeL.Text = "Roam time:                           Mins, 0 = unlimited";
            // 
            // roamTimeBox
            // 
            this.roamTimeBox.BackColor = System.Drawing.Color.White;
            this.roamTimeBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.roamTimeBox.Location = new System.Drawing.Point(96, 22);
            this.roamTimeBox.Name = "roamTimeBox";
            this.roamTimeBox.Size = new System.Drawing.Size(45, 20);
            this.roamTimeBox.TabIndex = 1;
            this.roamTimeBox.TabStop = false;
            // 
            // roamDistanceL
            // 
            this.roamDistanceL.AutoSize = true;
            this.roamDistanceL.Location = new System.Drawing.Point(9, 57);
            this.roamDistanceL.Name = "roamDistanceL";
            this.roamDistanceL.Size = new System.Drawing.Size(222, 13);
            this.roamDistanceL.TabIndex = 2;
            this.roamDistanceL.Text = "Roam distance:                            (500 - 5000)";
            // 
            // roamDistanceBox
            // 
            this.roamDistanceBox.BackColor = System.Drawing.Color.White;
            this.roamDistanceBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.roamDistanceBox.Location = new System.Drawing.Point(96, 55);
            this.roamDistanceBox.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.roamDistanceBox.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.roamDistanceBox.Name = "roamDistanceBox";
            this.roamDistanceBox.Size = new System.Drawing.Size(70, 20);
            this.roamDistanceBox.TabIndex = 3;
            this.roamDistanceBox.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(786, 418);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 21);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // gatherWhiteListBox
            // 
            this.gatherWhiteListBox.BackColor = System.Drawing.Color.White;
            this.gatherWhiteListBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gatherWhiteListBox.FormattingEnabled = true;
            this.gatherWhiteListBox.Items.AddRange(new object[] {
            " "});
            this.gatherWhiteListBox.Location = new System.Drawing.Point(6, 35);
            this.gatherWhiteListBox.Name = "gatherWhiteListBox";
            this.gatherWhiteListBox.ScrollAlwaysVisible = true;
            this.gatherWhiteListBox.Size = new System.Drawing.Size(130, 147);
            this.gatherWhiteListBox.TabIndex = 5;
            this.gatherWhiteListBox.TabStop = false;
            this.gatherWhiteListBox.Click += new System.EventHandler(this.gatherWhiteListBox_Click);
            // 
            // gatherBlackListBox
            // 
            this.gatherBlackListBox.BackColor = System.Drawing.Color.White;
            this.gatherBlackListBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gatherBlackListBox.FormattingEnabled = true;
            this.gatherBlackListBox.Items.AddRange(new object[] {
            " "});
            this.gatherBlackListBox.Location = new System.Drawing.Point(142, 35);
            this.gatherBlackListBox.Name = "gatherBlackListBox";
            this.gatherBlackListBox.ScrollAlwaysVisible = true;
            this.gatherBlackListBox.Size = new System.Drawing.Size(130, 147);
            this.gatherBlackListBox.TabIndex = 6;
            this.gatherBlackListBox.TabStop = false;
            this.gatherBlackListBox.Click += new System.EventHandler(this.gatherBlackListBox_Click);
            // 
            // filterPickupListBox
            // 
            this.filterPickupListBox.BackColor = System.Drawing.Color.White;
            this.filterPickupListBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.filterPickupListBox.FormattingEnabled = true;
            this.filterPickupListBox.Items.AddRange(new object[] {
            " "});
            this.filterPickupListBox.Location = new System.Drawing.Point(9, 58);
            this.filterPickupListBox.Name = "filterPickupListBox";
            this.filterPickupListBox.ScrollAlwaysVisible = true;
            this.filterPickupListBox.Size = new System.Drawing.Size(275, 82);
            this.filterPickupListBox.TabIndex = 7;
            this.filterPickupListBox.TabStop = false;
            // 
            // moveLeftButton
            // 
            this.moveLeftButton.Location = new System.Drawing.Point(109, 189);
            this.moveLeftButton.Name = "moveLeftButton";
            this.moveLeftButton.Size = new System.Drawing.Size(27, 21);
            this.moveLeftButton.TabIndex = 11;
            this.moveLeftButton.TabStop = false;
            this.moveLeftButton.Text = "<-";
            this.moveLeftButton.UseVisualStyleBackColor = true;
            this.moveLeftButton.Click += new System.EventHandler(this.moveLeftButton_Click);
            // 
            // moveRightButton
            // 
            this.moveRightButton.Location = new System.Drawing.Point(142, 188);
            this.moveRightButton.Name = "moveRightButton";
            this.moveRightButton.Size = new System.Drawing.Size(27, 21);
            this.moveRightButton.TabIndex = 12;
            this.moveRightButton.TabStop = false;
            this.moveRightButton.Text = "->";
            this.moveRightButton.UseVisualStyleBackColor = true;
            this.moveRightButton.Click += new System.EventHandler(this.moveRightButton_Click);
            // 
            // filterPickupTextBox
            // 
            this.filterPickupTextBox.BackColor = System.Drawing.Color.White;
            this.filterPickupTextBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.filterPickupTextBox.Location = new System.Drawing.Point(9, 32);
            this.filterPickupTextBox.Name = "filterPickupTextBox";
            this.filterPickupTextBox.Size = new System.Drawing.Size(148, 20);
            this.filterPickupTextBox.TabIndex = 13;
            this.filterPickupTextBox.TabStop = false;
            // 
            // addPickupButton
            // 
            this.addPickupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addPickupButton.Location = new System.Drawing.Point(163, 30);
            this.addPickupButton.Name = "addPickupButton";
            this.addPickupButton.Size = new System.Drawing.Size(50, 22);
            this.addPickupButton.TabIndex = 14;
            this.addPickupButton.TabStop = false;
            this.addPickupButton.Text = "Add";
            this.addPickupButton.UseVisualStyleBackColor = true;
            this.addPickupButton.Click += new System.EventHandler(this.addPickupButton_Click);
            // 
            // classTypeBox
            // 
            this.classTypeBox.BackColor = System.Drawing.Color.White;
            this.classTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classTypeBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.classTypeBox.FormattingEnabled = true;
            this.classTypeBox.Items.AddRange(new object[] {
            "melee",
            "ranged"});
            this.classTypeBox.Location = new System.Drawing.Point(79, 19);
            this.classTypeBox.Name = "classTypeBox";
            this.classTypeBox.Size = new System.Drawing.Size(96, 21);
            this.classTypeBox.Sorted = true;
            this.classTypeBox.TabIndex = 15;
            this.classTypeBox.TabStop = false;
            this.classTypeBox.SelectedIndexChanged += new System.EventHandler(this.classTypeBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.kiteModeBox);
            this.groupBox1.Controls.Add(this.RestSkillCheckBox);
            this.groupBox1.Controls.Add(this.RestMaxBox);
            this.groupBox1.Controls.Add(this.RestMinBox);
            this.groupBox1.Controls.Add(this.RestCheckBox);
            this.groupBox1.Controls.Add(this.HealMinBox);
            this.groupBox1.Controls.Add(this.RetreatMinBox);
            this.groupBox1.Controls.Add(this.KiteDistanceBox);
            this.groupBox1.Controls.Add(this.KiteCheckBox);
            this.groupBox1.Controls.Add(this.HealCheckBox);
            this.groupBox1.Controls.Add(this.attRangeBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.RetreatCheckBox);
            this.groupBox1.Controls.Add(this.classTypeBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(271, 219);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Combat";
            // 
            // kiteModeBox
            // 
            this.kiteModeBox.BackColor = System.Drawing.Color.White;
            this.kiteModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kiteModeBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.kiteModeBox.FormattingEnabled = true;
            this.kiteModeBox.Items.AddRange(new object[] {
            "V",
            "V + <",
            "V + >"});
            this.kiteModeBox.Location = new System.Drawing.Point(199, 89);
            this.kiteModeBox.Name = "kiteModeBox";
            this.kiteModeBox.Size = new System.Drawing.Size(58, 21);
            this.kiteModeBox.Sorted = true;
            this.kiteModeBox.TabIndex = 28;
            this.kiteModeBox.TabStop = false;
            this.kiteModeBox.SelectedIndexChanged += new System.EventHandler(this.kiteModeBox_SelectedIndexChanged);
            // 
            // RestSkillCheckBox
            // 
            this.RestSkillCheckBox.AutoSize = true;
            this.RestSkillCheckBox.Location = new System.Drawing.Point(13, 194);
            this.RestSkillCheckBox.Name = "RestSkillCheckBox";
            this.RestSkillCheckBox.Size = new System.Drawing.Size(183, 17);
            this.RestSkillCheckBox.TabIndex = 27;
            this.RestSkillCheckBox.Text = "Use skill while resting (hotkey \'=\' )";
            this.RestSkillCheckBox.UseVisualStyleBackColor = true;
            // 
            // RestMaxBox
            // 
            this.RestMaxBox.BackColor = System.Drawing.Color.White;
            this.RestMaxBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RestMaxBox.Location = new System.Drawing.Point(175, 166);
            this.RestMaxBox.Name = "RestMaxBox";
            this.RestMaxBox.Size = new System.Drawing.Size(59, 20);
            this.RestMaxBox.TabIndex = 26;
            this.RestMaxBox.TabStop = false;
            // 
            // RestMinBox
            // 
            this.RestMinBox.BackColor = System.Drawing.Color.White;
            this.RestMinBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RestMinBox.Location = new System.Drawing.Point(78, 166);
            this.RestMinBox.Name = "RestMinBox";
            this.RestMinBox.Size = new System.Drawing.Size(59, 20);
            this.RestMinBox.TabIndex = 25;
            this.RestMinBox.TabStop = false;
            // 
            // RestCheckBox
            // 
            this.RestCheckBox.AutoSize = true;
            this.RestCheckBox.Location = new System.Drawing.Point(13, 167);
            this.RestCheckBox.Name = "RestCheckBox";
            this.RestCheckBox.Size = new System.Drawing.Size(245, 17);
            this.RestCheckBox.TabIndex = 24;
            this.RestCheckBox.Text = "Rest if <                        until                         (%)";
            this.RestCheckBox.UseVisualStyleBackColor = true;
            this.RestCheckBox.CheckedChanged += new System.EventHandler(this.RestCheckBox_CheckedChanged);
            // 
            // HealMinBox
            // 
            this.HealMinBox.BackColor = System.Drawing.Color.White;
            this.HealMinBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.HealMinBox.Location = new System.Drawing.Point(134, 136);
            this.HealMinBox.Name = "HealMinBox";
            this.HealMinBox.Size = new System.Drawing.Size(59, 20);
            this.HealMinBox.TabIndex = 23;
            this.HealMinBox.TabStop = false;
            // 
            // RetreatMinBox
            // 
            this.RetreatMinBox.BackColor = System.Drawing.Color.White;
            this.RetreatMinBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RetreatMinBox.Location = new System.Drawing.Point(134, 113);
            this.RetreatMinBox.Name = "RetreatMinBox";
            this.RetreatMinBox.Size = new System.Drawing.Size(59, 20);
            this.RetreatMinBox.TabIndex = 22;
            this.RetreatMinBox.TabStop = false;
            // 
            // KiteDistanceBox
            // 
            this.KiteDistanceBox.BackColor = System.Drawing.Color.White;
            this.KiteDistanceBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.KiteDistanceBox.Location = new System.Drawing.Point(134, 90);
            this.KiteDistanceBox.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.KiteDistanceBox.Name = "KiteDistanceBox";
            this.KiteDistanceBox.Size = new System.Drawing.Size(59, 20);
            this.KiteDistanceBox.TabIndex = 1;
            this.KiteDistanceBox.TabStop = false;
            this.KiteDistanceBox.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // KiteCheckBox
            // 
            this.KiteCheckBox.AutoSize = true;
            this.KiteCheckBox.Location = new System.Drawing.Point(13, 93);
            this.KiteCheckBox.Name = "KiteCheckBox";
            this.KiteCheckBox.Size = new System.Drawing.Size(109, 17);
            this.KiteCheckBox.TabIndex = 21;
            this.KiteCheckBox.Text = "Auto-kite if within:";
            this.KiteCheckBox.UseVisualStyleBackColor = true;
            this.KiteCheckBox.CheckedChanged += new System.EventHandler(this.useAutoKite_CheckedChanged);
            // 
            // HealCheckBox
            // 
            this.HealCheckBox.AutoSize = true;
            this.HealCheckBox.Location = new System.Drawing.Point(13, 139);
            this.HealCheckBox.Name = "HealCheckBox";
            this.HealCheckBox.Size = new System.Drawing.Size(244, 17);
            this.HealCheckBox.TabIndex = 20;
            this.HealCheckBox.Text = "Use skill if hp  <                               (hotkey \'9\')";
            this.HealCheckBox.UseVisualStyleBackColor = true;
            this.HealCheckBox.CheckedChanged += new System.EventHandler(this.HealCheckBox_CheckedChanged);
            // 
            // attRangeBox
            // 
            this.attRangeBox.BackColor = System.Drawing.Color.White;
            this.attRangeBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.attRangeBox.Location = new System.Drawing.Point(84, 49);
            this.attRangeBox.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.attRangeBox.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.attRangeBox.Name = "attRangeBox";
            this.attRangeBox.Size = new System.Drawing.Size(54, 20);
            this.attRangeBox.TabIndex = 4;
            this.attRangeBox.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Class type:";
            // 
            // RetreatCheckBox
            // 
            this.RetreatCheckBox.AutoSize = true;
            this.RetreatCheckBox.Location = new System.Drawing.Point(13, 116);
            this.RetreatCheckBox.Name = "RetreatCheckBox";
            this.RetreatCheckBox.Size = new System.Drawing.Size(244, 17);
            this.RetreatCheckBox.TabIndex = 17;
            this.RetreatCheckBox.Text = "Use skill if within:                             (hotkey \'8\')";
            this.RetreatCheckBox.UseVisualStyleBackColor = true;
            this.RetreatCheckBox.CheckedChanged += new System.EventHandler(this.RetreatCheckBox_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(181, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Attack Range:                      (90-300)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.roamTimeBox);
            this.groupBox2.Controls.Add(this.roamDistanceBox);
            this.groupBox2.Controls.Add(this.roamDistanceL);
            this.groupBox2.Controls.Add(this.roamTimeL);
            this.groupBox2.Location = new System.Drawing.Point(12, 289);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(279, 85);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Roam";
            // 
            // removePickupButton
            // 
            this.removePickupButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removePickupButton.Location = new System.Drawing.Point(219, 30);
            this.removePickupButton.Name = "removePickupButton";
            this.removePickupButton.Size = new System.Drawing.Size(65, 22);
            this.removePickupButton.TabIndex = 18;
            this.removePickupButton.TabStop = false;
            this.removePickupButton.Text = "Remove";
            this.removePickupButton.UseVisualStyleBackColor = true;
            this.removePickupButton.Click += new System.EventHandler(this.removePickupButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.useMountCheckBox);
            this.groupBox3.Location = new System.Drawing.Point(12, 237);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(271, 46);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Misc";
            // 
            // useMountCheckBox
            // 
            this.useMountCheckBox.AutoSize = true;
            this.useMountCheckBox.Location = new System.Drawing.Point(13, 19);
            this.useMountCheckBox.Name = "useMountCheckBox";
            this.useMountCheckBox.Size = new System.Drawing.Size(191, 17);
            this.useMountCheckBox.TabIndex = 0;
            this.useMountCheckBox.Text = "Use mount in path mode (hotkey 0)";
            this.useMountCheckBox.UseVisualStyleBackColor = true;
            // 
            // removeMobButton
            // 
            this.removeMobButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeMobButton.Location = new System.Drawing.Point(219, 30);
            this.removeMobButton.Name = "removeMobButton";
            this.removeMobButton.Size = new System.Drawing.Size(65, 22);
            this.removeMobButton.TabIndex = 24;
            this.removeMobButton.TabStop = false;
            this.removeMobButton.Text = "Remove";
            this.removeMobButton.UseVisualStyleBackColor = true;
            this.removeMobButton.Click += new System.EventHandler(this.excludeMobRemoveButton_Click);
            // 
            // addMobButton
            // 
            this.addMobButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addMobButton.Location = new System.Drawing.Point(163, 30);
            this.addMobButton.Name = "addMobButton";
            this.addMobButton.Size = new System.Drawing.Size(50, 22);
            this.addMobButton.TabIndex = 23;
            this.addMobButton.TabStop = false;
            this.addMobButton.Text = "Add";
            this.addMobButton.UseVisualStyleBackColor = true;
            this.addMobButton.Click += new System.EventHandler(this.excludeMobAddButton_Click);
            // 
            // filterMobTextBox
            // 
            this.filterMobTextBox.BackColor = System.Drawing.Color.White;
            this.filterMobTextBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.filterMobTextBox.Location = new System.Drawing.Point(9, 32);
            this.filterMobTextBox.Name = "filterMobTextBox";
            this.filterMobTextBox.Size = new System.Drawing.Size(148, 20);
            this.filterMobTextBox.TabIndex = 22;
            this.filterMobTextBox.TabStop = false;
            // 
            // filterMobsListBox
            // 
            this.filterMobsListBox.BackColor = System.Drawing.Color.White;
            this.filterMobsListBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.filterMobsListBox.FormattingEnabled = true;
            this.filterMobsListBox.Items.AddRange(new object[] {
            " "});
            this.filterMobsListBox.Location = new System.Drawing.Point(9, 58);
            this.filterMobsListBox.Name = "filterMobsListBox";
            this.filterMobsListBox.ScrollAlwaysVisible = true;
            this.filterMobsListBox.Size = new System.Drawing.Size(275, 82);
            this.filterMobsListBox.TabIndex = 20;
            this.filterMobsListBox.TabStop = false;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(785, 444);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 21);
            this.cancelButton.TabIndex = 25;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.spaceCountL);
            this.groupBox4.Controls.Add(this.inventStartColumnBox);
            this.groupBox4.Controls.Add(this.inventStartRowBox);
            this.groupBox4.Controls.Add(this.inventTotalRowsBox);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Location = new System.Drawing.Point(12, 380);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(279, 93);
            this.groupBox4.TabIndex = 26;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Vendoring";
            // 
            // spaceCountL
            // 
            this.spaceCountL.AutoSize = true;
            this.spaceCountL.Location = new System.Drawing.Point(174, 31);
            this.spaceCountL.Name = "spaceCountL";
            this.spaceCountL.Size = new System.Drawing.Size(16, 13);
            this.spaceCountL.TabIndex = 5;
            this.spaceCountL.Text = "---";
            // 
            // inventStartColumnBox
            // 
            this.inventStartColumnBox.BackColor = System.Drawing.Color.White;
            this.inventStartColumnBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.inventStartColumnBox.Location = new System.Drawing.Point(223, 58);
            this.inventStartColumnBox.Name = "inventStartColumnBox";
            this.inventStartColumnBox.Size = new System.Drawing.Size(41, 20);
            this.inventStartColumnBox.TabIndex = 4;
            this.inventStartColumnBox.TabStop = false;
            // 
            // inventStartRowBox
            // 
            this.inventStartRowBox.BackColor = System.Drawing.Color.White;
            this.inventStartRowBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.inventStartRowBox.Location = new System.Drawing.Point(125, 58);
            this.inventStartRowBox.Name = "inventStartRowBox";
            this.inventStartRowBox.Size = new System.Drawing.Size(41, 20);
            this.inventStartRowBox.TabIndex = 3;
            this.inventStartRowBox.TabStop = false;
            // 
            // inventTotalRowsBox
            // 
            this.inventTotalRowsBox.BackColor = System.Drawing.Color.White;
            this.inventTotalRowsBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.inventTotalRowsBox.Location = new System.Drawing.Point(124, 28);
            this.inventTotalRowsBox.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.inventTotalRowsBox.Name = "inventTotalRowsBox";
            this.inventTotalRowsBox.Size = new System.Drawing.Size(41, 20);
            this.inventTotalRowsBox.TabIndex = 2;
            this.inventTotalRowsBox.TabStop = false;
            this.inventTotalRowsBox.ValueChanged += new System.EventHandler(this.inventTotalRowsBox_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(204, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Start selling from row:                    column:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Inventory total rows: ";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.euRb);
            this.groupBox5.Controls.Add(this.usRb);
            this.groupBox5.Location = new System.Drawing.Point(297, 426);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(124, 47);
            this.groupBox5.TabIndex = 27;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Game Client";
            // 
            // euRb
            // 
            this.euRb.AutoSize = true;
            this.euRb.Location = new System.Drawing.Point(77, 20);
            this.euRb.Name = "euRb";
            this.euRb.Size = new System.Drawing.Size(40, 17);
            this.euRb.TabIndex = 1;
            this.euRb.Text = "EU";
            this.euRb.UseVisualStyleBackColor = true;
            // 
            // usRb
            // 
            this.usRb.AutoSize = true;
            this.usRb.Checked = true;
            this.usRb.Location = new System.Drawing.Point(15, 20);
            this.usRb.Name = "usRb";
            this.usRb.Size = new System.Drawing.Size(40, 17);
            this.usRb.TabIndex = 0;
            this.usRb.TabStop = true;
            this.usRb.Text = "US";
            this.usRb.UseVisualStyleBackColor = true;
            this.usRb.CheckedChanged += new System.EventHandler(this.usRb_CheckedChanged);
            // 
            // filterPickupsCheckbox
            // 
            this.filterPickupsCheckbox.AutoSize = true;
            this.filterPickupsCheckbox.Location = new System.Drawing.Point(165, 20);
            this.filterPickupsCheckbox.Name = "filterPickupsCheckbox";
            this.filterPickupsCheckbox.Size = new System.Drawing.Size(88, 17);
            this.filterPickupsCheckbox.TabIndex = 28;
            this.filterPickupsCheckbox.Text = "No Item Filter";
            this.filterPickupsCheckbox.UseVisualStyleBackColor = true;
            this.filterPickupsCheckbox.CheckedChanged += new System.EventHandler(this.filterPickupsCheckbox_CheckedChanged);
            // 
            // pickupGb
            // 
            this.pickupGb.Controls.Add(this.label9);
            this.pickupGb.Controls.Add(this.groupBox8);
            this.pickupGb.Controls.Add(this.filterPickupListBox);
            this.pickupGb.Controls.Add(this.filterPickupTextBox);
            this.pickupGb.Controls.Add(this.addPickupButton);
            this.pickupGb.Controls.Add(this.removePickupButton);
            this.pickupGb.Location = new System.Drawing.Point(289, 219);
            this.pickupGb.Name = "pickupGb";
            this.pickupGb.Size = new System.Drawing.Size(293, 201);
            this.pickupGb.TabIndex = 29;
            this.pickupGb.TabStop = false;
            this.pickupGb.Text = "Item Filter";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(145, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "Enter the item of your choice:";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.pickupExcludeRb);
            this.groupBox8.Controls.Add(this.pickupIncludeRb);
            this.groupBox8.Controls.Add(this.filterPickupsCheckbox);
            this.groupBox8.Location = new System.Drawing.Point(9, 146);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(275, 45);
            this.groupBox8.TabIndex = 31;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Item List Options";
            // 
            // pickupExcludeRb
            // 
            this.pickupExcludeRb.AutoSize = true;
            this.pickupExcludeRb.Location = new System.Drawing.Point(84, 19);
            this.pickupExcludeRb.Name = "pickupExcludeRb";
            this.pickupExcludeRb.Size = new System.Drawing.Size(75, 17);
            this.pickupExcludeRb.TabIndex = 30;
            this.pickupExcludeRb.Text = "Exclussive";
            this.pickupExcludeRb.UseVisualStyleBackColor = true;
            // 
            // pickupIncludeRb
            // 
            this.pickupIncludeRb.AutoSize = true;
            this.pickupIncludeRb.Checked = true;
            this.pickupIncludeRb.Location = new System.Drawing.Point(6, 19);
            this.pickupIncludeRb.Name = "pickupIncludeRb";
            this.pickupIncludeRb.Size = new System.Drawing.Size(72, 17);
            this.pickupIncludeRb.TabIndex = 29;
            this.pickupIncludeRb.TabStop = true;
            this.pickupIncludeRb.Text = "Inclussive";
            this.pickupIncludeRb.UseVisualStyleBackColor = true;
            this.pickupIncludeRb.CheckedChanged += new System.EventHandler(this.pickupIncludeRb_CheckedChanged);
            // 
            // gatherGb
            // 
            this.gatherGb.Controls.Add(this.dontGatherCheckbox);
            this.gatherGb.Controls.Add(this.label2);
            this.gatherGb.Controls.Add(this.label1);
            this.gatherGb.Controls.Add(this.gatherWhiteListBox);
            this.gatherGb.Controls.Add(this.gatherBlackListBox);
            this.gatherGb.Controls.Add(this.moveLeftButton);
            this.gatherGb.Controls.Add(this.moveRightButton);
            this.gatherGb.Location = new System.Drawing.Point(588, 12);
            this.gatherGb.Name = "gatherGb";
            this.gatherGb.Size = new System.Drawing.Size(280, 241);
            this.gatherGb.TabIndex = 30;
            this.gatherGb.TabStop = false;
            this.gatherGb.Text = "Gather Filter";
            // 
            // dontGatherCheckbox
            // 
            this.dontGatherCheckbox.AutoSize = true;
            this.dontGatherCheckbox.Location = new System.Drawing.Point(77, 214);
            this.dontGatherCheckbox.Name = "dontGatherCheckbox";
            this.dontGatherCheckbox.Size = new System.Drawing.Size(130, 17);
            this.dontGatherCheckbox.TabIndex = 15;
            this.dontGatherCheckbox.Text = "Don\'t gather anything.";
            this.dontGatherCheckbox.UseVisualStyleBackColor = true;
            this.dontGatherCheckbox.CheckedChanged += new System.EventHandler(this.dontGatherCheckbox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Don\'t:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Do:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.groupBox7);
            this.groupBox6.Controls.Add(this.filterMobsListBox);
            this.groupBox6.Controls.Add(this.filterMobTextBox);
            this.groupBox6.Controls.Add(this.addMobButton);
            this.groupBox6.Controls.Add(this.removeMobButton);
            this.groupBox6.Location = new System.Drawing.Point(289, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(293, 201);
            this.groupBox6.TabIndex = 31;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Mob Filter";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 13);
            this.label6.TabIndex = 36;
            this.label6.Text = "Enter the mob of your choice: ";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.mobIncludeRb);
            this.groupBox7.Controls.Add(this.mobExcludeRb);
            this.groupBox7.Controls.Add(this.filterMobsCheckbox);
            this.groupBox7.Location = new System.Drawing.Point(9, 146);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(275, 45);
            this.groupBox7.TabIndex = 35;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Mob List Options";
            // 
            // mobIncludeRb
            // 
            this.mobIncludeRb.AutoSize = true;
            this.mobIncludeRb.Location = new System.Drawing.Point(6, 19);
            this.mobIncludeRb.Name = "mobIncludeRb";
            this.mobIncludeRb.Size = new System.Drawing.Size(72, 17);
            this.mobIncludeRb.TabIndex = 32;
            this.mobIncludeRb.Text = "Inclussive";
            this.mobIncludeRb.UseVisualStyleBackColor = true;
            this.mobIncludeRb.CheckedChanged += new System.EventHandler(this.mobIncludeRb_CheckedChanged);
            // 
            // mobExcludeRb
            // 
            this.mobExcludeRb.AutoSize = true;
            this.mobExcludeRb.Checked = true;
            this.mobExcludeRb.Location = new System.Drawing.Point(84, 19);
            this.mobExcludeRb.Name = "mobExcludeRb";
            this.mobExcludeRb.Size = new System.Drawing.Size(75, 17);
            this.mobExcludeRb.TabIndex = 33;
            this.mobExcludeRb.TabStop = true;
            this.mobExcludeRb.Text = "Exclussive";
            this.mobExcludeRb.UseVisualStyleBackColor = true;
            // 
            // filterMobsCheckbox
            // 
            this.filterMobsCheckbox.AutoSize = true;
            this.filterMobsCheckbox.Checked = true;
            this.filterMobsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.filterMobsCheckbox.Location = new System.Drawing.Point(165, 19);
            this.filterMobsCheckbox.Name = "filterMobsCheckbox";
            this.filterMobsCheckbox.Size = new System.Drawing.Size(89, 17);
            this.filterMobsCheckbox.TabIndex = 31;
            this.filterMobsCheckbox.Text = "No Mob Filter";
            this.filterMobsCheckbox.UseVisualStyleBackColor = true;
            this.filterMobsCheckbox.CheckedChanged += new System.EventHandler(this.filterMobsCheckbox_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(640, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 13);
            this.label3.TabIndex = 34;
            this.label3.Text = "Notice: Input should be Case Sensitive!";
            // 
            // Settings
            // 
            this.ClientSize = new System.Drawing.Size(878, 483);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.gatherGb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pickupGb);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.roamTimeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roamDistanceBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RestMaxBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RestMinBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HealMinBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RetreatMinBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KiteDistanceBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attRangeBox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inventStartColumnBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventStartRowBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventTotalRowsBox)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.pickupGb.ResumeLayout(false);
            this.pickupGb.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.gatherGb.ResumeLayout(false);
            this.gatherGb.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
  }
}
