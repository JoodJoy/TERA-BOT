
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace tbp
{
  public class ProfileEditor : Form
  {
    private List<Control> labelList = new List<Control>();
    private List<Command> commandList = new List<Command>();
    private string currentProfile;
    private IContainer components;
    private Panel f1p;
    private Label f1;
    private Panel f2p;
    private Label f2;
    private Panel f3p;
    private Label f3;
    private Panel f4p;
    private Label f4;
    private Panel f5p;
    private Label f5;
    private Panel f6p;
    private Label f6;
    private Panel k1p;
    private Label k1;
    private Panel k2p;
    private Label k2;
    private Panel k3p;
    private Label k3;
    private Panel k4p;
    private Label k4;
    private Panel k5p;
    private Label k5;
    private Panel k6p;
    private Label k6;
    private Panel LClickp;
    private Label LClick;
    private Panel RClickp;
    private Label RClick;
    private Panel panel1;
    private ListBox profileListBox;
    private Button doneButton;
    private Button removeButton;
    public Label profileNameL;
    private Button insertKeyReleaseButton;
    private Button insertKeyHoldButton;
    private NumericUpDown costBox;
    private Label costL;
    private Button insertPressButton;
    private NumericUpDown cooldownBox;
    private Label label1;
    private Button button1;

    public ProfileEditor(string prof)
    {
      this.InitializeComponent();
      this.currentProfile = prof;
      this.loadList();
      this.labelList = this.getLabels((Control) this);
      this.profileNameL.Text = this.currentProfile;
    }

    private void loadList()
    {
      Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
      try
      {
        if (JsonConvert.DeserializeObject<List<Command>>(File.ReadAllText("profiles\\" + this.currentProfile + ".json")) == null)
          return;
        try
        {
          this.commandList = JsonConvert.DeserializeObject<List<Command>>(File.ReadAllText("profiles\\" + this.currentProfile + ".json"));
          this.refreshList();
        }
        catch (IOException ex)
        {
          Thread.Sleep(TimeSpan.FromMilliseconds(500.0));
        }
      }
      catch (IOException ex)
      {
        Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
      }
    }

    private void enableLabels()
    {
      for (int index = 0; index < this.labelList.Count; ++index)
      {
        if (!this.labelList[index].Enabled)
          this.labelList[index].Enabled = true;
      }
    }

    private void f1L_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.f1.Enabled = false;
    }

    private void f2L_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.f2.Enabled = false;
    }

    private void f3L_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.f3.Enabled = false;
    }

    private void f4L_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.f4.Enabled = false;
    }

    private void f5L_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.f5.Enabled = false;
    }

    private void f6L_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.f6.Enabled = false;
    }

    private void k1L_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.k1.Enabled = false;
    }

    private void k2L_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.k2.Enabled = false;
    }

    private void k3L_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.k3.Enabled = false;
    }

    private void k4L_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.k4.Enabled = false;
    }

    private void k5L_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.k5.Enabled = false;
    }

    private void k6L_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.k6.Enabled = false;
    }

    private void LClickL_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.LClick.Enabled = false;
    }

    private void RClickL_Click(object sender, EventArgs e)
    {
      this.enableLabels();
      this.RClick.Enabled = false;
    }

    private void removeButton_Click(object sender, EventArgs e)
    {
      this.onDelete();
    }

    private void doneButton_Click(object sender, EventArgs e)
    {
      File.WriteAllText("profiles\\" + this.profileNameL.Text + ".json", JsonConvert.SerializeObject((object) this.commandList));
      this.Close();
    }

    private void profileNameL_Click(object sender, EventArgs e)
    {
      if (Application.OpenForms["FileNameSave"] is FileNameSave)
        return;
      FileNameSave fileNameSave = new FileNameSave(this.currentProfile, "profile", "");
      fileNameSave.FormClosed += new FormClosedEventHandler(this.fileNameSave_FormClosed);
      fileNameSave.Show((IWin32Window) this);
    }

    private void fileNameSave_FormClosed(object sender, FormClosedEventArgs e)
    {
    }

    private void insertKeyHoldButton_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.labelList.Count; ++index)
      {
        if (!this.labelList[index].Enabled)
        {
          Command command = new Command();
          command.cost = (int) this.costBox.Value;
          command.slot = this.labelList[index].Text + " (hold)";
          command.cd = (int) this.cooldownBox.Value;
          command.cdTick = command.cd;
          this.commandList.Add(command);
        }
      }
      this.refreshList();
    }

    private void insertKeyReleaseButton_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.labelList.Count; ++index)
      {
        if (!this.labelList[index].Enabled)
        {
          Command command = new Command();
          command.slot = this.labelList[index].Text + " (release)";
          command.cd = (int) this.cooldownBox.Value;
          command.cdTick = command.cd;
          this.commandList.Add(command);
        }
      }
      this.refreshList();
    }

    private void insertPressButton_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.labelList.Count; ++index)
      {
        if (!this.labelList[index].Enabled)
        {
          Command command = new Command();
          command.cost = (int) this.costBox.Value;
          command.slot = this.labelList[index].Text + " (press)";
          command.cd = (int) this.cooldownBox.Value;
          command.cdTick = command.cd;
          this.commandList.Add(command);
        }
      }
      this.refreshList();
    }

    private void onDelete()
    {
      if (this.profileListBox.SelectedIndex == -1)
        return;
      this.commandList.RemoveAt(this.profileListBox.SelectedIndex);
      this.refreshList();
    }

    private void refreshList()
    {
      this.profileListBox.Items.Clear();
      for (int index = 0; index < this.commandList.Count; ++index)
        this.profileListBox.Items.Insert(index, (object) ("slot : " + (object) this.commandList[index].slot + " cost : " + (Int32) (object) this.commandList[index].cost + " cd : " + (Int32) (object) this.commandList[index].cd));
    }

    private List<Control> getLabels(Control container)
    {
      List<Control> list = new List<Control>();
      foreach (Control container1 in (ArrangedElementCollection) container.Controls)
      {
        list.AddRange((IEnumerable<Control>) this.getLabels(container1));
        if (container1 is Label)
          list.Add(container1);
      }
      return list;
    }

    private void ProfileEditor_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Delete)
        return;
      this.onDelete();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.f1p = new System.Windows.Forms.Panel();
            this.f1 = new System.Windows.Forms.Label();
            this.f2p = new System.Windows.Forms.Panel();
            this.f2 = new System.Windows.Forms.Label();
            this.f3p = new System.Windows.Forms.Panel();
            this.f3 = new System.Windows.Forms.Label();
            this.f4p = new System.Windows.Forms.Panel();
            this.f4 = new System.Windows.Forms.Label();
            this.f5p = new System.Windows.Forms.Panel();
            this.f5 = new System.Windows.Forms.Label();
            this.f6p = new System.Windows.Forms.Panel();
            this.f6 = new System.Windows.Forms.Label();
            this.k1p = new System.Windows.Forms.Panel();
            this.k1 = new System.Windows.Forms.Label();
            this.k2p = new System.Windows.Forms.Panel();
            this.k2 = new System.Windows.Forms.Label();
            this.k3p = new System.Windows.Forms.Panel();
            this.k3 = new System.Windows.Forms.Label();
            this.k4p = new System.Windows.Forms.Panel();
            this.k4 = new System.Windows.Forms.Label();
            this.k5p = new System.Windows.Forms.Panel();
            this.k5 = new System.Windows.Forms.Label();
            this.k6p = new System.Windows.Forms.Panel();
            this.k6 = new System.Windows.Forms.Label();
            this.LClickp = new System.Windows.Forms.Panel();
            this.LClick = new System.Windows.Forms.Label();
            this.RClickp = new System.Windows.Forms.Panel();
            this.RClick = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.profileListBox = new System.Windows.Forms.ListBox();
            this.doneButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.profileNameL = new System.Windows.Forms.Label();
            this.insertKeyReleaseButton = new System.Windows.Forms.Button();
            this.insertKeyHoldButton = new System.Windows.Forms.Button();
            this.costBox = new System.Windows.Forms.NumericUpDown();
            this.costL = new System.Windows.Forms.Label();
            this.insertPressButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cooldownBox = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.f1p.SuspendLayout();
            this.f2p.SuspendLayout();
            this.f3p.SuspendLayout();
            this.f4p.SuspendLayout();
            this.f5p.SuspendLayout();
            this.f6p.SuspendLayout();
            this.k1p.SuspendLayout();
            this.k2p.SuspendLayout();
            this.k3p.SuspendLayout();
            this.k4p.SuspendLayout();
            this.k5p.SuspendLayout();
            this.k6p.SuspendLayout();
            this.LClickp.SuspendLayout();
            this.RClickp.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.costBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cooldownBox)).BeginInit();
            this.SuspendLayout();
            // 
            // f1p
            // 
            this.f1p.BackColor = System.Drawing.SystemColors.Control;
            this.f1p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.f1p.Controls.Add(this.f1);
            this.f1p.ForeColor = System.Drawing.Color.Black;
            this.f1p.Location = new System.Drawing.Point(4, 3);
            this.f1p.Name = "f1p";
            this.f1p.Size = new System.Drawing.Size(30, 30);
            this.f1p.TabIndex = 0;
            // 
            // f1
            // 
            this.f1.AutoSize = true;
            this.f1.BackColor = System.Drawing.SystemColors.Control;
            this.f1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.f1.Location = new System.Drawing.Point(4, 8);
            this.f1.Name = "f1";
            this.f1.Size = new System.Drawing.Size(19, 13);
            this.f1.TabIndex = 0;
            this.f1.Text = "F1";
            this.f1.Click += new System.EventHandler(this.f1L_Click);
            // 
            // f2p
            // 
            this.f2p.BackColor = System.Drawing.SystemColors.Control;
            this.f2p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.f2p.Controls.Add(this.f2);
            this.f2p.ForeColor = System.Drawing.Color.Black;
            this.f2p.Location = new System.Drawing.Point(40, 3);
            this.f2p.Name = "f2p";
            this.f2p.Size = new System.Drawing.Size(30, 30);
            this.f2p.TabIndex = 1;
            // 
            // f2
            // 
            this.f2.AutoSize = true;
            this.f2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.f2.Location = new System.Drawing.Point(4, 8);
            this.f2.Name = "f2";
            this.f2.Size = new System.Drawing.Size(19, 13);
            this.f2.TabIndex = 0;
            this.f2.Text = "F2";
            this.f2.Click += new System.EventHandler(this.f2L_Click);
            // 
            // f3p
            // 
            this.f3p.BackColor = System.Drawing.SystemColors.Control;
            this.f3p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.f3p.Controls.Add(this.f3);
            this.f3p.ForeColor = System.Drawing.Color.Black;
            this.f3p.Location = new System.Drawing.Point(76, 3);
            this.f3p.Name = "f3p";
            this.f3p.Size = new System.Drawing.Size(30, 30);
            this.f3p.TabIndex = 1;
            // 
            // f3
            // 
            this.f3.AutoSize = true;
            this.f3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.f3.Location = new System.Drawing.Point(4, 8);
            this.f3.Name = "f3";
            this.f3.Size = new System.Drawing.Size(19, 13);
            this.f3.TabIndex = 0;
            this.f3.Text = "F3";
            this.f3.Click += new System.EventHandler(this.f3L_Click);
            // 
            // f4p
            // 
            this.f4p.BackColor = System.Drawing.SystemColors.Control;
            this.f4p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.f4p.Controls.Add(this.f4);
            this.f4p.ForeColor = System.Drawing.Color.Black;
            this.f4p.Location = new System.Drawing.Point(112, 3);
            this.f4p.Name = "f4p";
            this.f4p.Size = new System.Drawing.Size(30, 30);
            this.f4p.TabIndex = 1;
            // 
            // f4
            // 
            this.f4.AutoSize = true;
            this.f4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.f4.Location = new System.Drawing.Point(4, 8);
            this.f4.Name = "f4";
            this.f4.Size = new System.Drawing.Size(19, 13);
            this.f4.TabIndex = 0;
            this.f4.Text = "F4";
            this.f4.Click += new System.EventHandler(this.f4L_Click);
            // 
            // f5p
            // 
            this.f5p.BackColor = System.Drawing.SystemColors.Control;
            this.f5p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.f5p.Controls.Add(this.f5);
            this.f5p.ForeColor = System.Drawing.Color.Black;
            this.f5p.Location = new System.Drawing.Point(148, 3);
            this.f5p.Name = "f5p";
            this.f5p.Size = new System.Drawing.Size(30, 30);
            this.f5p.TabIndex = 1;
            // 
            // f5
            // 
            this.f5.AutoSize = true;
            this.f5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.f5.Location = new System.Drawing.Point(4, 8);
            this.f5.Name = "f5";
            this.f5.Size = new System.Drawing.Size(19, 13);
            this.f5.TabIndex = 0;
            this.f5.Text = "F5";
            this.f5.Click += new System.EventHandler(this.f5L_Click);
            // 
            // f6p
            // 
            this.f6p.BackColor = System.Drawing.SystemColors.Control;
            this.f6p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.f6p.Controls.Add(this.f6);
            this.f6p.ForeColor = System.Drawing.Color.Black;
            this.f6p.Location = new System.Drawing.Point(184, 3);
            this.f6p.Name = "f6p";
            this.f6p.Size = new System.Drawing.Size(30, 30);
            this.f6p.TabIndex = 1;
            // 
            // f6
            // 
            this.f6.AutoSize = true;
            this.f6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.f6.Location = new System.Drawing.Point(4, 8);
            this.f6.Name = "f6";
            this.f6.Size = new System.Drawing.Size(19, 13);
            this.f6.TabIndex = 0;
            this.f6.Text = "F6";
            this.f6.Click += new System.EventHandler(this.f6L_Click);
            // 
            // k1p
            // 
            this.k1p.BackColor = System.Drawing.SystemColors.Control;
            this.k1p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.k1p.Controls.Add(this.k1);
            this.k1p.ForeColor = System.Drawing.Color.Black;
            this.k1p.Location = new System.Drawing.Point(4, 39);
            this.k1p.Name = "k1p";
            this.k1p.Size = new System.Drawing.Size(30, 30);
            this.k1p.TabIndex = 1;
            // 
            // k1
            // 
            this.k1.AutoSize = true;
            this.k1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.k1.Location = new System.Drawing.Point(7, 8);
            this.k1.Name = "k1";
            this.k1.Size = new System.Drawing.Size(13, 13);
            this.k1.TabIndex = 0;
            this.k1.Text = "1";
            this.k1.Click += new System.EventHandler(this.k1L_Click);
            // 
            // k2p
            // 
            this.k2p.BackColor = System.Drawing.SystemColors.Control;
            this.k2p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.k2p.Controls.Add(this.k2);
            this.k2p.ForeColor = System.Drawing.Color.Black;
            this.k2p.Location = new System.Drawing.Point(40, 39);
            this.k2p.Name = "k2p";
            this.k2p.Size = new System.Drawing.Size(30, 30);
            this.k2p.TabIndex = 2;
            // 
            // k2
            // 
            this.k2.AutoSize = true;
            this.k2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.k2.Location = new System.Drawing.Point(7, 8);
            this.k2.Name = "k2";
            this.k2.Size = new System.Drawing.Size(13, 13);
            this.k2.TabIndex = 0;
            this.k2.Text = "2";
            this.k2.Click += new System.EventHandler(this.k2L_Click);
            // 
            // k3p
            // 
            this.k3p.BackColor = System.Drawing.SystemColors.Control;
            this.k3p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.k3p.Controls.Add(this.k3);
            this.k3p.ForeColor = System.Drawing.Color.Black;
            this.k3p.Location = new System.Drawing.Point(76, 39);
            this.k3p.Name = "k3p";
            this.k3p.Size = new System.Drawing.Size(30, 30);
            this.k3p.TabIndex = 2;
            // 
            // k3
            // 
            this.k3.AutoSize = true;
            this.k3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.k3.Location = new System.Drawing.Point(7, 8);
            this.k3.Name = "k3";
            this.k3.Size = new System.Drawing.Size(13, 13);
            this.k3.TabIndex = 0;
            this.k3.Text = "3";
            this.k3.Click += new System.EventHandler(this.k3L_Click);
            // 
            // k4p
            // 
            this.k4p.BackColor = System.Drawing.SystemColors.Control;
            this.k4p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.k4p.Controls.Add(this.k4);
            this.k4p.ForeColor = System.Drawing.Color.Black;
            this.k4p.Location = new System.Drawing.Point(112, 39);
            this.k4p.Name = "k4p";
            this.k4p.Size = new System.Drawing.Size(30, 30);
            this.k4p.TabIndex = 2;
            // 
            // k4
            // 
            this.k4.AutoSize = true;
            this.k4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.k4.Location = new System.Drawing.Point(7, 8);
            this.k4.Name = "k4";
            this.k4.Size = new System.Drawing.Size(13, 13);
            this.k4.TabIndex = 0;
            this.k4.Text = "4";
            this.k4.Click += new System.EventHandler(this.k4L_Click);
            // 
            // k5p
            // 
            this.k5p.BackColor = System.Drawing.SystemColors.Control;
            this.k5p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.k5p.Controls.Add(this.k5);
            this.k5p.ForeColor = System.Drawing.Color.Black;
            this.k5p.Location = new System.Drawing.Point(148, 39);
            this.k5p.Name = "k5p";
            this.k5p.Size = new System.Drawing.Size(30, 30);
            this.k5p.TabIndex = 2;
            // 
            // k5
            // 
            this.k5.AutoSize = true;
            this.k5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.k5.Location = new System.Drawing.Point(7, 8);
            this.k5.Name = "k5";
            this.k5.Size = new System.Drawing.Size(13, 13);
            this.k5.TabIndex = 0;
            this.k5.Text = "5";
            this.k5.Click += new System.EventHandler(this.k5L_Click);
            // 
            // k6p
            // 
            this.k6p.BackColor = System.Drawing.SystemColors.Control;
            this.k6p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.k6p.Controls.Add(this.k6);
            this.k6p.ForeColor = System.Drawing.Color.Black;
            this.k6p.Location = new System.Drawing.Point(184, 39);
            this.k6p.Name = "k6p";
            this.k6p.Size = new System.Drawing.Size(30, 30);
            this.k6p.TabIndex = 2;
            // 
            // k6
            // 
            this.k6.AutoSize = true;
            this.k6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.k6.Location = new System.Drawing.Point(7, 8);
            this.k6.Name = "k6";
            this.k6.Size = new System.Drawing.Size(13, 13);
            this.k6.TabIndex = 0;
            this.k6.Text = "6";
            this.k6.Click += new System.EventHandler(this.k6L_Click);
            // 
            // LClickp
            // 
            this.LClickp.BackColor = System.Drawing.SystemColors.Control;
            this.LClickp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LClickp.Controls.Add(this.LClick);
            this.LClickp.ForeColor = System.Drawing.Color.Black;
            this.LClickp.Location = new System.Drawing.Point(220, 39);
            this.LClickp.Name = "LClickp";
            this.LClickp.Size = new System.Drawing.Size(50, 30);
            this.LClickp.TabIndex = 3;
            // 
            // LClick
            // 
            this.LClick.AutoSize = true;
            this.LClick.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LClick.Location = new System.Drawing.Point(5, 8);
            this.LClick.Name = "LClick";
            this.LClick.Size = new System.Drawing.Size(39, 13);
            this.LClick.TabIndex = 0;
            this.LClick.Text = "L-Click";
            this.LClick.Click += new System.EventHandler(this.LClickL_Click);
            // 
            // RClickp
            // 
            this.RClickp.BackColor = System.Drawing.SystemColors.Control;
            this.RClickp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RClickp.Controls.Add(this.RClick);
            this.RClickp.ForeColor = System.Drawing.Color.Black;
            this.RClickp.Location = new System.Drawing.Point(220, 3);
            this.RClickp.Name = "RClickp";
            this.RClickp.Size = new System.Drawing.Size(50, 30);
            this.RClickp.TabIndex = 4;
            // 
            // RClick
            // 
            this.RClick.AutoSize = true;
            this.RClick.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RClick.Location = new System.Drawing.Point(5, 8);
            this.RClick.Name = "RClick";
            this.RClick.Size = new System.Drawing.Size(41, 13);
            this.RClick.TabIndex = 0;
            this.RClick.Text = "R-Click";
            this.RClick.Click += new System.EventHandler(this.RClickL_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.RClickp);
            this.panel1.Controls.Add(this.f3p);
            this.panel1.Controls.Add(this.f1p);
            this.panel1.Controls.Add(this.k4p);
            this.panel1.Controls.Add(this.f6p);
            this.panel1.Controls.Add(this.k2p);
            this.panel1.Controls.Add(this.k1p);
            this.panel1.Controls.Add(this.f2p);
            this.panel1.Controls.Add(this.LClickp);
            this.panel1.Controls.Add(this.f4p);
            this.panel1.Controls.Add(this.k5p);
            this.panel1.Controls.Add(this.k3p);
            this.panel1.Controls.Add(this.k6p);
            this.panel1.Controls.Add(this.f5p);
            this.panel1.Location = new System.Drawing.Point(12, 198);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 78);
            this.panel1.TabIndex = 5;
            // 
            // profileListBox
            // 
            this.profileListBox.BackColor = System.Drawing.Color.White;
            this.profileListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.profileListBox.ForeColor = System.Drawing.Color.Black;
            this.profileListBox.FormattingEnabled = true;
            this.profileListBox.ItemHeight = 15;
            this.profileListBox.Items.AddRange(new object[] {
            " "});
            this.profileListBox.Location = new System.Drawing.Point(12, 16);
            this.profileListBox.Name = "profileListBox";
            this.profileListBox.Size = new System.Drawing.Size(239, 169);
            this.profileListBox.TabIndex = 6;
            // 
            // doneButton
            // 
            this.doneButton.Location = new System.Drawing.Point(308, 204);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(105, 30);
            this.doneButton.TabIndex = 7;
            this.doneButton.Text = "Save";
            this.doneButton.UseVisualStyleBackColor = true;
            this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(352, 148);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(69, 23);
            this.removeButton.TabIndex = 8;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // profileNameL
            // 
            this.profileNameL.AutoSize = true;
            this.profileNameL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.profileNameL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.profileNameL.Location = new System.Drawing.Point(319, 16);
            this.profileNameL.Name = "profileNameL";
            this.profileNameL.Size = new System.Drawing.Size(48, 15);
            this.profileNameL.TabIndex = 13;
            this.profileNameL.Text = "profile1";
            this.profileNameL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.profileNameL.Click += new System.EventHandler(this.profileNameL_Click);
            // 
            // insertKeyReleaseButton
            // 
            this.insertKeyReleaseButton.Location = new System.Drawing.Point(352, 110);
            this.insertKeyReleaseButton.Name = "insertKeyReleaseButton";
            this.insertKeyReleaseButton.Size = new System.Drawing.Size(69, 23);
            this.insertKeyReleaseButton.TabIndex = 11;
            this.insertKeyReleaseButton.Text = "Release";
            this.insertKeyReleaseButton.UseVisualStyleBackColor = true;
            this.insertKeyReleaseButton.Click += new System.EventHandler(this.insertKeyReleaseButton_Click);
            // 
            // insertKeyHoldButton
            // 
            this.insertKeyHoldButton.Location = new System.Drawing.Point(268, 110);
            this.insertKeyHoldButton.Name = "insertKeyHoldButton";
            this.insertKeyHoldButton.Size = new System.Drawing.Size(69, 23);
            this.insertKeyHoldButton.TabIndex = 12;
            this.insertKeyHoldButton.Text = "Hold";
            this.insertKeyHoldButton.UseVisualStyleBackColor = true;
            this.insertKeyHoldButton.Click += new System.EventHandler(this.insertKeyHoldButton_Click);
            // 
            // costBox
            // 
            this.costBox.BackColor = System.Drawing.Color.White;
            this.costBox.ForeColor = System.Drawing.Color.Black;
            this.costBox.Location = new System.Drawing.Point(268, 68);
            this.costBox.Name = "costBox";
            this.costBox.Size = new System.Drawing.Size(69, 20);
            this.costBox.TabIndex = 13;
            this.costBox.TabStop = false;
            // 
            // costL
            // 
            this.costL.AutoSize = true;
            this.costL.Location = new System.Drawing.Point(265, 52);
            this.costL.Name = "costL";
            this.costL.Size = new System.Drawing.Size(31, 13);
            this.costL.TabIndex = 14;
            this.costL.Text = "Cost:";
            // 
            // insertPressButton
            // 
            this.insertPressButton.Location = new System.Drawing.Point(268, 148);
            this.insertPressButton.Name = "insertPressButton";
            this.insertPressButton.Size = new System.Drawing.Size(69, 23);
            this.insertPressButton.TabIndex = 15;
            this.insertPressButton.TabStop = false;
            this.insertPressButton.Text = "Press";
            this.insertPressButton.UseVisualStyleBackColor = true;
            this.insertPressButton.Click += new System.EventHandler(this.insertPressButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(349, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Cooldown:";
            // 
            // cooldownBox
            // 
            this.cooldownBox.BackColor = System.Drawing.Color.White;
            this.cooldownBox.ForeColor = System.Drawing.Color.Black;
            this.cooldownBox.Location = new System.Drawing.Point(352, 68);
            this.cooldownBox.Name = "cooldownBox";
            this.cooldownBox.Size = new System.Drawing.Size(69, 20);
            this.cooldownBox.TabIndex = 17;
            this.cooldownBox.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(308, 240);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 30);
            this.button1.TabIndex = 18;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ProfileEditor
            // 
            this.ClientSize = new System.Drawing.Size(433, 287);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cooldownBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.profileNameL);
            this.Controls.Add(this.insertPressButton);
            this.Controls.Add(this.insertKeyReleaseButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.insertKeyHoldButton);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.costL);
            this.Controls.Add(this.profileListBox);
            this.Controls.Add(this.costBox);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Profile Editor";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProfileEditor_KeyDown);
            this.f1p.ResumeLayout(false);
            this.f1p.PerformLayout();
            this.f2p.ResumeLayout(false);
            this.f2p.PerformLayout();
            this.f3p.ResumeLayout(false);
            this.f3p.PerformLayout();
            this.f4p.ResumeLayout(false);
            this.f4p.PerformLayout();
            this.f5p.ResumeLayout(false);
            this.f5p.PerformLayout();
            this.f6p.ResumeLayout(false);
            this.f6p.PerformLayout();
            this.k1p.ResumeLayout(false);
            this.k1p.PerformLayout();
            this.k2p.ResumeLayout(false);
            this.k2p.PerformLayout();
            this.k3p.ResumeLayout(false);
            this.k3p.PerformLayout();
            this.k4p.ResumeLayout(false);
            this.k4p.PerformLayout();
            this.k5p.ResumeLayout(false);
            this.k5p.PerformLayout();
            this.k6p.ResumeLayout(false);
            this.k6p.PerformLayout();
            this.LClickp.ResumeLayout(false);
            this.LClickp.PerformLayout();
            this.RClickp.ResumeLayout(false);
            this.RClickp.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.costBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cooldownBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
  }
}
