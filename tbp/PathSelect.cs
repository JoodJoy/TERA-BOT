

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace tbp
{
  public class PathSelect : Form
  {
    private Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config\\config.json"));
    private List<string> paths = new List<string>();
    private List<string> fileNames = new List<string>();
    public string pathType;
    private string pathName;
    private IContainer components;
    private ListBox pathListBox;
    private Button useButton;
    private Button createButton;
    private Button deleteButton;
    private Label label1;
    private Button renameButton;

    public PathSelect(string pathRef)
    {
      this.InitializeComponent();
      this.pathType = pathRef;
      this.getNames();
      this.checkSelection();
      if (pathRef == "std")
      {
        this.Text = "Standard Paths";
        this.pathName = this.config.sPathName;
      }
      else if (pathRef == "res")
      {
        this.Text = "Res Paths";
        this.pathName = this.config.rPathName;
      }
      else
      {
        if (!(pathRef == "vnd"))
          return;
        this.Text = "Vendor Paths";
        this.pathName = this.config.vPathName;
      }
    }

    private void getNames()
    {
      this.pathListBox.Items.Clear();
      if (this.pathType == "std")
        this.paths = Enumerable.ToList<string>(Directory.EnumerateFiles("paths\\standard paths\\"));
      else if (this.pathType == "res")
        this.paths = Enumerable.ToList<string>(Directory.EnumerateFiles("paths\\res paths\\"));
      else if (this.pathType == "vnd")
        this.paths = Enumerable.ToList<string>(Directory.EnumerateFiles("paths\\vendor paths\\"));
      for (int index = 0; index < this.paths.Count; ++index)
      {
        this.pathListBox.Items.Insert(index, (object) Path.GetFileNameWithoutExtension(this.paths[index]));
        if ((string) this.pathListBox.Items[index] == this.pathName)
          this.pathListBox.SetSelected(index, true);
      }
    }

    private void pathListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.checkSelection();
    }

    private void checkSelection()
    {
      if (this.pathListBox.SelectedIndex != -1)
        this.renameButton.Enabled = true;
      else
        this.renameButton.Enabled = false;
      if (this.paths.Count == 1)
      {
        this.deleteButton.Enabled = false;
        if (this.pathListBox.SelectedIndex != -1)
          return;
        this.pathListBox.SetSelected(0, true);
      }
      else
        this.deleteButton.Enabled = true;
    }

    private void createButton_Click(object sender, EventArgs e)
    {
      this.createPath();
    }

    private void createPath()
    {
      if (Application.OpenForms["NewPath"] is NewPath)
        return;
      if (this.pathListBox.SelectedIndex != -1)
        this.pathListBox.SetSelected(this.pathListBox.SelectedIndex, false);
      NewPath newPath = new NewPath(this.pathType);
      newPath.FormClosed += new FormClosedEventHandler(this.newPath_FormClosed);
      ((Control) newPath).Show();
    }

    private void newPath_FormClosed(object sender, EventArgs e)
    {
      this.getNames();
      this.checkSelection();
    }

    private void deleteButton_Click(object sender, EventArgs e)
    {
      if (this.paths.Count > 1)
      {
        try
        {
          if (this.pathType == "std")
          {
            File.Delete("paths\\standard paths\\" + (string) this.pathListBox.SelectedItem + ".json");
            File.Delete("paths\\standard paths\\deadzones\\" + (string) this.pathListBox.SelectedItem + " DeadZones.json");
            File.Delete("paths\\standard paths\\safezones\\" + (string) this.pathListBox.SelectedItem + " SafeZones.json");
          }
          else if (this.pathType == "res")
            File.Delete("paths\\res paths\\" + (string) this.pathListBox.SelectedItem + ".json");
          else if (this.pathType == "vnd")
            File.Delete("paths\\vendor paths\\" + (string) this.pathListBox.SelectedItem + ".json");
        }
        catch (IOException ex)
        {
          Thread.Sleep(TimeSpan.FromSeconds(1.0));
        }
      }
      this.getNames();
      this.checkSelection();
    }

    private void useButton_Click(object sender, EventArgs e)
    {
      this.onUse();
    }

    private void onUse()
    {
      if (this.pathType == "std")
        this.config.sPathName = (string) this.pathListBox.SelectedItem;
      else if (this.pathType == "res")
        this.config.rPathName = (string) this.pathListBox.SelectedItem;
      else if (this.pathType == "vnd")
        this.config.vPathName = (string) this.pathListBox.SelectedItem;
      try
      {
        File.WriteAllText("config\\config.json", JsonConvert.SerializeObject((object) this.config));
      }
      catch (IOException ex)
      {
        Thread.Sleep(TimeSpan.FromSeconds(1.0));
      }
      ((MainUI) this.Owner).sNodeList.Clear();
      ((MainUI) this.Owner).rNodeList.Clear();
      ((MainUI) this.Owner).vNodeList.Clear();
      ((MainUI) this.Owner).roamNodesList.Clear();
      ((MainUI) this.Owner).deadZonesList.Clear();
      ((MainUI) this.Owner).safeZonesList.Clear();
      ((MainUI) this.Owner).targetNode = -1;
      ((MainUI) this.Owner).loadFiles();
      this.checkSelection();
      this.Close();
    }

    private void renameButton_Click(object sender, EventArgs e)
    {
      if (Application.OpenForms["FileNameSave"] is FileNameSave)
        return;
      try
      {
        FileNameSave fileNameSave = new FileNameSave((string) this.pathListBox.SelectedItem, "path", this.pathType);
        fileNameSave.FormClosed += new FormClosedEventHandler(this.fileNameSave_FormClosed);
        fileNameSave.Owner = (Form) this;
        ((Control) fileNameSave).Show();
      }
      catch (IOException ex)
      {
        Thread.Sleep(TimeSpan.FromSeconds(1.0));
      }
    }

    private void fileNameSave_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.getNames();
      this.checkSelection();
    }

    private void pathListBox_DoubleClick(object sender, EventArgs e)
    {
      this.onUse();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.pathListBox = new System.Windows.Forms.ListBox();
            this.useButton = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.renameButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pathListBox
            // 
            this.pathListBox.BackColor = System.Drawing.Color.White;
            this.pathListBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pathListBox.FormattingEnabled = true;
            this.pathListBox.Items.AddRange(new object[] {
            " "});
            this.pathListBox.Location = new System.Drawing.Point(15, 25);
            this.pathListBox.Name = "pathListBox";
            this.pathListBox.Size = new System.Drawing.Size(254, 108);
            this.pathListBox.Sorted = true;
            this.pathListBox.TabIndex = 0;
            this.pathListBox.TabStop = false;
            this.pathListBox.SelectedIndexChanged += new System.EventHandler(this.pathListBox_SelectedIndexChanged);
            this.pathListBox.DoubleClick += new System.EventHandler(this.pathListBox_DoubleClick);
            // 
            // useButton
            // 
            this.useButton.Location = new System.Drawing.Point(15, 139);
            this.useButton.Name = "useButton";
            this.useButton.Size = new System.Drawing.Size(59, 23);
            this.useButton.TabIndex = 0;
            this.useButton.TabStop = false;
            this.useButton.Text = "Set";
            this.useButton.UseVisualStyleBackColor = true;
            this.useButton.Click += new System.EventHandler(this.useButton_Click);
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(80, 139);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(59, 23);
            this.createButton.TabIndex = 1;
            this.createButton.TabStop = false;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(210, 139);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(59, 23);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.TabStop = false;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // renameButton
            // 
            this.renameButton.Location = new System.Drawing.Point(145, 139);
            this.renameButton.Name = "renameButton";
            this.renameButton.Size = new System.Drawing.Size(59, 23);
            this.renameButton.TabIndex = 3;
            this.renameButton.TabStop = false;
            this.renameButton.Text = "Rename";
            this.renameButton.UseVisualStyleBackColor = true;
            this.renameButton.Click += new System.EventHandler(this.renameButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Current paths:";
            // 
            // PathSelect
            // 
            this.ClientSize = new System.Drawing.Size(287, 177);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.renameButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.useButton);
            this.Controls.Add(this.pathListBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PathSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Paths";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

    }
  }
}
