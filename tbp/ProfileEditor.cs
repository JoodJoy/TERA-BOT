
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProfileEditor));
      this.f1p = new Panel();
      this.f1 = new Label();
      this.f2p = new Panel();
      this.f2 = new Label();
      this.f3p = new Panel();
      this.f3 = new Label();
      this.f4p = new Panel();
      this.f4 = new Label();
      this.f5p = new Panel();
      this.f5 = new Label();
      this.f6p = new Panel();
      this.f6 = new Label();
      this.k1p = new Panel();
      this.k1 = new Label();
      this.k2p = new Panel();
      this.k2 = new Label();
      this.k3p = new Panel();
      this.k3 = new Label();
      this.k4p = new Panel();
      this.k4 = new Label();
      this.k5p = new Panel();
      this.k5 = new Label();
      this.k6p = new Panel();
      this.k6 = new Label();
      this.LClickp = new Panel();
      this.LClick = new Label();
      this.RClickp = new Panel();
      this.RClick = new Label();
      this.panel1 = new Panel();
      this.profileListBox = new ListBox();
      this.doneButton = new Button();
      this.removeButton = new Button();
      this.profileNameL = new Label();
      this.insertKeyReleaseButton = new Button();
      this.insertKeyHoldButton = new Button();
      this.costBox = new NumericUpDown();
      this.costL = new Label();
      this.insertPressButton = new Button();
      this.label1 = new Label();
      this.cooldownBox = new NumericUpDown();
      this.button1 = new Button();
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
      this.costBox.BeginInit();
      this.cooldownBox.BeginInit();
      this.SuspendLayout();
      this.f1p.BackColor = SystemColors.Control;
      this.f1p.BorderStyle = BorderStyle.Fixed3D;
      this.f1p.Controls.Add((Control) this.f1);
      this.f1p.ForeColor = Color.Black;
      this.f1p.Location = new Point(4, 3);
      this.f1p.Name = "f1p";
      this.f1p.Size = new Size(30, 30);
      this.f1p.TabIndex = 0;
      this.f1.AutoSize = true;
      this.f1.BackColor = SystemColors.Control;
      this.f1.Cursor = Cursors.Hand;
      this.f1.Location = new Point(4, 8);
      this.f1.Name = "f1";
      this.f1.Size = new Size(19, 13);
      this.f1.TabIndex = 0;
      this.f1.Text = "F1";
      this.f1.Click += new EventHandler(this.f1L_Click);
      this.f2p.BackColor = SystemColors.Control;
      this.f2p.BorderStyle = BorderStyle.Fixed3D;
      this.f2p.Controls.Add((Control) this.f2);
      this.f2p.ForeColor = Color.Black;
      this.f2p.Location = new Point(40, 3);
      this.f2p.Name = "f2p";
      this.f2p.Size = new Size(30, 30);
      this.f2p.TabIndex = 1;
      this.f2.AutoSize = true;
      this.f2.Cursor = Cursors.Hand;
      this.f2.Location = new Point(4, 8);
      this.f2.Name = "f2";
      this.f2.Size = new Size(19, 13);
      this.f2.TabIndex = 0;
      this.f2.Text = "F2";
      this.f2.Click += new EventHandler(this.f2L_Click);
      this.f3p.BackColor = SystemColors.Control;
      this.f3p.BorderStyle = BorderStyle.Fixed3D;
      this.f3p.Controls.Add((Control) this.f3);
      this.f3p.ForeColor = Color.Black;
      this.f3p.Location = new Point(76, 3);
      this.f3p.Name = "f3p";
      this.f3p.Size = new Size(30, 30);
      this.f3p.TabIndex = 1;
      this.f3.AutoSize = true;
      this.f3.Cursor = Cursors.Hand;
      this.f3.Location = new Point(4, 8);
      this.f3.Name = "f3";
      this.f3.Size = new Size(19, 13);
      this.f3.TabIndex = 0;
      this.f3.Text = "F3";
      this.f3.Click += new EventHandler(this.f3L_Click);
      this.f4p.BackColor = SystemColors.Control;
      this.f4p.BorderStyle = BorderStyle.Fixed3D;
      this.f4p.Controls.Add((Control) this.f4);
      this.f4p.ForeColor = Color.Black;
      this.f4p.Location = new Point(112, 3);
      this.f4p.Name = "f4p";
      this.f4p.Size = new Size(30, 30);
      this.f4p.TabIndex = 1;
      this.f4.AutoSize = true;
      this.f4.Cursor = Cursors.Hand;
      this.f4.Location = new Point(4, 8);
      this.f4.Name = "f4";
      this.f4.Size = new Size(19, 13);
      this.f4.TabIndex = 0;
      this.f4.Text = "F4";
      this.f4.Click += new EventHandler(this.f4L_Click);
      this.f5p.BackColor = SystemColors.Control;
      this.f5p.BorderStyle = BorderStyle.Fixed3D;
      this.f5p.Controls.Add((Control) this.f5);
      this.f5p.ForeColor = Color.Black;
      this.f5p.Location = new Point(148, 3);
      this.f5p.Name = "f5p";
      this.f5p.Size = new Size(30, 30);
      this.f5p.TabIndex = 1;
      this.f5.AutoSize = true;
      this.f5.Cursor = Cursors.Hand;
      this.f5.Location = new Point(4, 8);
      this.f5.Name = "f5";
      this.f5.Size = new Size(19, 13);
      this.f5.TabIndex = 0;
      this.f5.Text = "F5";
      this.f5.Click += new EventHandler(this.f5L_Click);
      this.f6p.BackColor = SystemColors.Control;
      this.f6p.BorderStyle = BorderStyle.Fixed3D;
      this.f6p.Controls.Add((Control) this.f6);
      this.f6p.ForeColor = Color.Black;
      this.f6p.Location = new Point(184, 3);
      this.f6p.Name = "f6p";
      this.f6p.Size = new Size(30, 30);
      this.f6p.TabIndex = 1;
      this.f6.AutoSize = true;
      this.f6.Cursor = Cursors.Hand;
      this.f6.Location = new Point(4, 8);
      this.f6.Name = "f6";
      this.f6.Size = new Size(19, 13);
      this.f6.TabIndex = 0;
      this.f6.Text = "F6";
      this.f6.Click += new EventHandler(this.f6L_Click);
      this.k1p.BackColor = SystemColors.Control;
      this.k1p.BorderStyle = BorderStyle.Fixed3D;
      this.k1p.Controls.Add((Control) this.k1);
      this.k1p.ForeColor = Color.Black;
      this.k1p.Location = new Point(4, 39);
      this.k1p.Name = "k1p";
      this.k1p.Size = new Size(30, 30);
      this.k1p.TabIndex = 1;
      this.k1.AutoSize = true;
      this.k1.Cursor = Cursors.Hand;
      this.k1.Location = new Point(7, 8);
      this.k1.Name = "k1";
      this.k1.Size = new Size(13, 13);
      this.k1.TabIndex = 0;
      this.k1.Text = "1";
      this.k1.Click += new EventHandler(this.k1L_Click);
      this.k2p.BackColor = SystemColors.Control;
      this.k2p.BorderStyle = BorderStyle.Fixed3D;
      this.k2p.Controls.Add((Control) this.k2);
      this.k2p.ForeColor = Color.Black;
      this.k2p.Location = new Point(40, 39);
      this.k2p.Name = "k2p";
      this.k2p.Size = new Size(30, 30);
      this.k2p.TabIndex = 2;
      this.k2.AutoSize = true;
      this.k2.Cursor = Cursors.Hand;
      this.k2.Location = new Point(7, 8);
      this.k2.Name = "k2";
      this.k2.Size = new Size(13, 13);
      this.k2.TabIndex = 0;
      this.k2.Text = "2";
      this.k2.Click += new EventHandler(this.k2L_Click);
      this.k3p.BackColor = SystemColors.Control;
      this.k3p.BorderStyle = BorderStyle.Fixed3D;
      this.k3p.Controls.Add((Control) this.k3);
      this.k3p.ForeColor = Color.Black;
      this.k3p.Location = new Point(76, 39);
      this.k3p.Name = "k3p";
      this.k3p.Size = new Size(30, 30);
      this.k3p.TabIndex = 2;
      this.k3.AutoSize = true;
      this.k3.Cursor = Cursors.Hand;
      this.k3.Location = new Point(7, 8);
      this.k3.Name = "k3";
      this.k3.Size = new Size(13, 13);
      this.k3.TabIndex = 0;
      this.k3.Text = "3";
      this.k3.Click += new EventHandler(this.k3L_Click);
      this.k4p.BackColor = SystemColors.Control;
      this.k4p.BorderStyle = BorderStyle.Fixed3D;
      this.k4p.Controls.Add((Control) this.k4);
      this.k4p.ForeColor = Color.Black;
      this.k4p.Location = new Point(112, 39);
      this.k4p.Name = "k4p";
      this.k4p.Size = new Size(30, 30);
      this.k4p.TabIndex = 2;
      this.k4.AutoSize = true;
      this.k4.Cursor = Cursors.Hand;
      this.k4.Location = new Point(7, 8);
      this.k4.Name = "k4";
      this.k4.Size = new Size(13, 13);
      this.k4.TabIndex = 0;
      this.k4.Text = "4";
      this.k4.Click += new EventHandler(this.k4L_Click);
      this.k5p.BackColor = SystemColors.Control;
      this.k5p.BorderStyle = BorderStyle.Fixed3D;
      this.k5p.Controls.Add((Control) this.k5);
      this.k5p.ForeColor = Color.Black;
      this.k5p.Location = new Point(148, 39);
      this.k5p.Name = "k5p";
      this.k5p.Size = new Size(30, 30);
      this.k5p.TabIndex = 2;
      this.k5.AutoSize = true;
      this.k5.Cursor = Cursors.Hand;
      this.k5.Location = new Point(7, 8);
      this.k5.Name = "k5";
      this.k5.Size = new Size(13, 13);
      this.k5.TabIndex = 0;
      this.k5.Text = "5";
      this.k5.Click += new EventHandler(this.k5L_Click);
      this.k6p.BackColor = SystemColors.Control;
      this.k6p.BorderStyle = BorderStyle.Fixed3D;
      this.k6p.Controls.Add((Control) this.k6);
      this.k6p.ForeColor = Color.Black;
      this.k6p.Location = new Point(184, 39);
      this.k6p.Name = "k6p";
      this.k6p.Size = new Size(30, 30);
      this.k6p.TabIndex = 2;
      this.k6.AutoSize = true;
      this.k6.Cursor = Cursors.Hand;
      this.k6.Location = new Point(7, 8);
      this.k6.Name = "k6";
      this.k6.Size = new Size(13, 13);
      this.k6.TabIndex = 0;
      this.k6.Text = "6";
      this.k6.Click += new EventHandler(this.k6L_Click);
      this.LClickp.BackColor = SystemColors.Control;
      this.LClickp.BorderStyle = BorderStyle.Fixed3D;
      this.LClickp.Controls.Add((Control) this.LClick);
      this.LClickp.ForeColor = Color.Black;
      this.LClickp.Location = new Point(220, 39);
      this.LClickp.Name = "LClickp";
      this.LClickp.Size = new Size(50, 30);
      this.LClickp.TabIndex = 3;
      this.LClick.AutoSize = true;
      this.LClick.Cursor = Cursors.Hand;
      this.LClick.Location = new Point(5, 8);
      this.LClick.Name = "LClick";
      this.LClick.Size = new Size(39, 13);
      this.LClick.TabIndex = 0;
      this.LClick.Text = "L-Click";
      this.LClick.Click += new EventHandler(this.LClickL_Click);
      this.RClickp.BackColor = SystemColors.Control;
      this.RClickp.BorderStyle = BorderStyle.Fixed3D;
      this.RClickp.Controls.Add((Control) this.RClick);
      this.RClickp.ForeColor = Color.Black;
      this.RClickp.Location = new Point(220, 3);
      this.RClickp.Name = "RClickp";
      this.RClickp.Size = new Size(50, 30);
      this.RClickp.TabIndex = 4;
      this.RClick.AutoSize = true;
      this.RClick.Cursor = Cursors.Hand;
      this.RClick.Location = new Point(5, 8);
      this.RClick.Name = "RClick";
      this.RClick.Size = new Size(41, 13);
      this.RClick.TabIndex = 0;
      this.RClick.Text = "R-Click";
      this.RClick.Click += new EventHandler(this.RClickL_Click);
      this.panel1.BackColor = Color.Black;
      this.panel1.BorderStyle = BorderStyle.Fixed3D;
      this.panel1.Controls.Add((Control) this.RClickp);
      this.panel1.Controls.Add((Control) this.f3p);
      this.panel1.Controls.Add((Control) this.f1p);
      this.panel1.Controls.Add((Control) this.k4p);
      this.panel1.Controls.Add((Control) this.f6p);
      this.panel1.Controls.Add((Control) this.k2p);
      this.panel1.Controls.Add((Control) this.k1p);
      this.panel1.Controls.Add((Control) this.f2p);
      this.panel1.Controls.Add((Control) this.LClickp);
      this.panel1.Controls.Add((Control) this.f4p);
      this.panel1.Controls.Add((Control) this.k5p);
      this.panel1.Controls.Add((Control) this.k3p);
      this.panel1.Controls.Add((Control) this.k6p);
      this.panel1.Controls.Add((Control) this.f5p);
      this.panel1.Location = new Point(12, 198);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(280, 78);
      this.panel1.TabIndex = 5;
      this.profileListBox.BackColor = Color.Black;
      this.profileListBox.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 129);
      this.profileListBox.ForeColor = Color.White;
      this.profileListBox.FormattingEnabled = true;
      this.profileListBox.ItemHeight = 15;
      this.profileListBox.Items.AddRange(new object[1]
      {
        (object) " "
      });
      this.profileListBox.Location = new Point(12, 16);
      this.profileListBox.Name = "profileListBox";
      this.profileListBox.Size = new Size(239, 169);
      this.profileListBox.TabIndex = 6;
      this.doneButton.Location = new Point(308, 204);
      this.doneButton.Name = "doneButton";
      this.doneButton.Size = new Size(105, 30);
      this.doneButton.TabIndex = 7;
      this.doneButton.Text = "Save";
      this.doneButton.UseVisualStyleBackColor = true;
      this.doneButton.Click += new EventHandler(this.doneButton_Click);
      this.removeButton.Location = new Point(352, 148);
      this.removeButton.Name = "removeButton";
      this.removeButton.Size = new Size(69, 23);
      this.removeButton.TabIndex = 8;
      this.removeButton.Text = "Remove";
      this.removeButton.UseVisualStyleBackColor = true;
      this.removeButton.Click += new EventHandler(this.removeButton_Click);
      this.profileNameL.AutoSize = true;
      this.profileNameL.Cursor = Cursors.Hand;
      this.profileNameL.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Underline, GraphicsUnit.Point, (byte) 129);
      this.profileNameL.Location = new Point(319, 16);
      this.profileNameL.Name = "profileNameL";
      this.profileNameL.Size = new Size(48, 15);
      this.profileNameL.TabIndex = 13;
      this.profileNameL.Text = "profile1";
      this.profileNameL.TextAlign = ContentAlignment.MiddleCenter;
      this.profileNameL.Click += new EventHandler(this.profileNameL_Click);
      this.insertKeyReleaseButton.Location = new Point(352, 110);
      this.insertKeyReleaseButton.Name = "insertKeyReleaseButton";
      this.insertKeyReleaseButton.Size = new Size(69, 23);
      this.insertKeyReleaseButton.TabIndex = 11;
      this.insertKeyReleaseButton.Text = "Release";
      this.insertKeyReleaseButton.UseVisualStyleBackColor = true;
      this.insertKeyReleaseButton.Click += new EventHandler(this.insertKeyReleaseButton_Click);
      this.insertKeyHoldButton.Location = new Point(268, 110);
      this.insertKeyHoldButton.Name = "insertKeyHoldButton";
      this.insertKeyHoldButton.Size = new Size(69, 23);
      this.insertKeyHoldButton.TabIndex = 12;
      this.insertKeyHoldButton.Text = "Hold";
      this.insertKeyHoldButton.UseVisualStyleBackColor = true;
      this.insertKeyHoldButton.Click += new EventHandler(this.insertKeyHoldButton_Click);
      this.costBox.BackColor = Color.Black;
      this.costBox.ForeColor = Color.White;
      this.costBox.Location = new Point(268, 68);
      NumericUpDown numericUpDown1 = this.costBox;
      int[] bits1 = new int[4];
      bits1[0] = 5000;
      Decimal num1 = new Decimal(bits1);
//      numericUpDown1.Maximum = num1;
      this.costBox.Name = "costBox";
      this.costBox.Size = new Size(69, 20);
      this.costBox.TabIndex = 13;
      this.costBox.TabStop = false;
      this.costL.AutoSize = true;
      this.costL.Location = new Point(265, 52);
      this.costL.Name = "costL";
      this.costL.Size = new Size(31, 13);
      this.costL.TabIndex = 14;
      this.costL.Text = "Cost:";
      this.insertPressButton.Location = new Point(268, 148);
      this.insertPressButton.Name = "insertPressButton";
      this.insertPressButton.Size = new Size(69, 23);
      this.insertPressButton.TabIndex = 15;
      this.insertPressButton.TabStop = false;
      this.insertPressButton.Text = "Press";
      this.insertPressButton.UseVisualStyleBackColor = true;
      this.insertPressButton.Click += new EventHandler(this.insertPressButton_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(349, 51);
      this.label1.Name = "label1";
      this.label1.Size = new Size(57, 13);
      this.label1.TabIndex = 16;
      this.label1.Text = "Cooldown:";
      this.cooldownBox.BackColor = Color.Black;
      this.cooldownBox.ForeColor = Color.White;
      this.cooldownBox.Location = new Point(352, 68);
      NumericUpDown numericUpDown2 = this.cooldownBox;
      int[] bits2 = new int[4];
      bits2[0] = 5000;
      Decimal num2 = new Decimal(bits2);
//      numericUpDown2.Maximum = num2;
      this.cooldownBox.Name = "cooldownBox";
      this.cooldownBox.Size = new Size(69, 20);
      this.cooldownBox.TabIndex = 17;
      this.cooldownBox.TabStop = false;
      this.button1.Location = new Point(308, 240);
      this.button1.Name = "button1";
      this.button1.Size = new Size(105, 30);
      this.button1.TabIndex = 18;
      this.button1.Text = "Cancel";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
//      this.AutoScaleDimensions = new SizeF(6f, 13f);
//      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(433, 287);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.cooldownBox);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.profileNameL);
      this.Controls.Add((Control) this.insertPressButton);
      this.Controls.Add((Control) this.insertKeyReleaseButton);
      this.Controls.Add((Control) this.removeButton);
      this.Controls.Add((Control) this.insertKeyHoldButton);
      this.Controls.Add((Control) this.doneButton);
      this.Controls.Add((Control) this.costL);
      this.Controls.Add((Control) this.profileListBox);
      this.Controls.Add((Control) this.costBox);
      this.Controls.Add((Control) this.panel1);
//      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
//      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ProfileEditor";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Profile Editor";
      this.TopMost = true;
      this.KeyDown += new KeyEventHandler(this.ProfileEditor_KeyDown);
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
      this.costBox.EndInit();
      this.cooldownBox.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
