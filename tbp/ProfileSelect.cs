

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
  public class ProfileSelect : Form
  {
    private Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config\\config.json"));
    private List<string> profiles = new List<string>();
    private List<string> fileNames = new List<string>();
    private IContainer components;
    private ListBox profileListBox;
    private Button editButton;
    private Button deleteButton;
    private Button createButton;
    private Button useButton;

    public ProfileSelect()
    {
      this.InitializeComponent();
      this.getNames();
      this.checkSelection();
    }

    private void getNames()
    {
      this.profileListBox.Items.Clear();
      this.profiles = Enumerable.ToList<string>(Directory.EnumerateFiles("profiles"));
      for (int index = 0; index < this.profiles.Count; ++index)
      {
        this.profileListBox.Items.Insert(index, (object) Path.GetFileNameWithoutExtension(this.profiles[index]));
        if ((string) this.profileListBox.Items[index] == this.config.profileName)
          this.profileListBox.SetSelected(index, true);
      }
    }

    private void checkSelection()
    {
      if (this.profileListBox.SelectedIndex != -1)
        this.editButton.Enabled = true;
      else
        this.editButton.Enabled = false;
      if (this.profiles.Count == 1)
      {
        this.deleteButton.Enabled = false;
        if (this.profileListBox.SelectedIndex != -1)
          return;
        this.profileListBox.SetSelected(0, true);
      }
      else
        this.deleteButton.Enabled = true;
    }

    private void createProfile()
    {
      if (Application.OpenForms["FileNameSave"] is FileNameSave)
        return;
      if (this.profileListBox.SelectedIndex != -1)
        this.profileListBox.SetSelected(this.profileListBox.SelectedIndex, false);
      FileNameSave fileNameSave = new FileNameSave("", "profile", "");
      fileNameSave.FormClosed += new FormClosedEventHandler(this.fileNameSave_FormClosed);
      ((Control) fileNameSave).Show();
    }

    private void fileNameSave_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.getNames();
      this.checkSelection();
    }

    private void onUse()
    {
      this.config.profileName = (string) this.profileListBox.SelectedItem;
      try
      {
        File.WriteAllText("config\\config.json", JsonConvert.SerializeObject((object) this.config));
      }
      catch (IOException ex)
      {
        Thread.Sleep(TimeSpan.FromSeconds(1.0));
      }
      ((MainUI) this.Owner).commandList.Clear();
      ((MainUI) this.Owner).loadFiles();
      this.checkSelection();
      this.Close();
    }

    private void editButton_Click(object sender, EventArgs e)
    {
      if (Application.OpenForms["ProfileEditor"] is ProfileEditor)
        return;
      try
      {
        ProfileEditor profileEditor = new ProfileEditor((string) this.profileListBox.SelectedItem);
        profileEditor.FormClosed += new FormClosedEventHandler(this.fileNameSave_FormClosed);
        ((Control) profileEditor).Show();
      }
      catch (IOException ex)
      {
        Thread.Sleep(TimeSpan.FromSeconds(1.0));
      }
    }

    private void useButton_Click_1(object sender, EventArgs e)
    {
      this.onUse();
    }

    private void profileListBox_DoubleClick(object sender, EventArgs e)
    {
      this.onUse();
    }

    private void profileListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.checkSelection();
    }

    private void createButton_Click_1(object sender, EventArgs e)
    {
      this.createProfile();
    }

    private void deleteButton_Click_1(object sender, EventArgs e)
    {
      if (this.profiles.Count > 1)
      {
        try
        {
          File.Delete("profiles\\" + (string) this.profileListBox.SelectedItem + ".json");
        }
        catch (IOException ex)
        {
          Thread.Sleep(TimeSpan.FromSeconds(1.0));
        }
      }
      this.getNames();
      this.checkSelection();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.profileListBox = new System.Windows.Forms.ListBox();
            this.editButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.useButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // profileListBox
            // 
            this.profileListBox.BackColor = System.Drawing.Color.White;
            this.profileListBox.ForeColor = System.Drawing.Color.Black;
            this.profileListBox.FormattingEnabled = true;
            this.profileListBox.Items.AddRange(new object[] {
            " "});
            this.profileListBox.Location = new System.Drawing.Point(12, 12);
            this.profileListBox.Name = "profileListBox";
            this.profileListBox.Size = new System.Drawing.Size(225, 186);
            this.profileListBox.Sorted = true;
            this.profileListBox.TabIndex = 1;
            this.profileListBox.TabStop = false;
            this.profileListBox.SelectedIndexChanged += new System.EventHandler(this.profileListBox_SelectedIndexChanged);
            this.profileListBox.DoubleClick += new System.EventHandler(this.profileListBox_DoubleClick);
            // 
            // editButton
            // 
            this.editButton.Location = new System.Drawing.Point(180, 204);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(57, 23);
            this.editButton.TabIndex = 7;
            this.editButton.TabStop = false;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(123, 204);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(51, 23);
            this.deleteButton.TabIndex = 6;
            this.deleteButton.TabStop = false;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click_1);
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(66, 204);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(51, 23);
            this.createButton.TabIndex = 5;
            this.createButton.TabStop = false;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click_1);
            // 
            // useButton
            // 
            this.useButton.Location = new System.Drawing.Point(12, 204);
            this.useButton.Name = "useButton";
            this.useButton.Size = new System.Drawing.Size(47, 23);
            this.useButton.TabIndex = 4;
            this.useButton.TabStop = false;
            this.useButton.Text = "Use";
            this.useButton.UseVisualStyleBackColor = true;
            this.useButton.Click += new System.EventHandler(this.useButton_Click_1);
            // 
            // ProfileSelect
            // 
            this.ClientSize = new System.Drawing.Size(249, 235);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.useButton);
            this.Controls.Add(this.profileListBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Profiles";
            this.TopMost = true;
            this.ResumeLayout(false);

    }
  }
}
