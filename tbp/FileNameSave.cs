

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace tbp
{
  public class FileNameSave : Form
  {
    private Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config\\config.json"));
    private string selectedFilePath;
    private string directory;
    private string defaultName;
    private string pathType;
    private IContainer components;
    private TextBox textBox;
    private Button saveButton;

    public FileNameSave(string name, string type, string type2)
    {
      this.InitializeComponent();
      this.selectedFilePath = name;
      if (type == "path")
      {
        this.pathType = type2;
        if (this.pathType == "std")
          this.directory = "paths\\standard paths\\";
        else if (this.pathType == "res")
          this.directory = "paths\\res paths\\";
        else if (this.pathType == "vnd")
          this.directory = "paths\\vendor paths\\";
        this.defaultName = name;
      }
      else if (type == "profile")
      {
        this.directory = "profiles\\";
        this.defaultName = name;
      }
      this.textBox.Text = this.defaultName;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.saveAndExit();
    }

    private void saveAndExit()
    {
      if (this.textBox.Text.StartsWith(" "))
      {
        int num1 = (int) MessageBox.Show("Name cannot start with space");
      }
      else if (this.textBox.Text.Length < 1)
      {
        int num2 = (int) MessageBox.Show("Name cannot be blank");
      }
      else if (this.selectedFilePath == "")
      {
        if (!File.Exists(this.directory + this.textBox.Text + ".json"))
        {
          FileStream fileStream = File.Create(this.directory + this.textBox.Text + ".json");
          fileStream.Close();
          fileStream.Dispose();
          this.Close();
        }
        else
        {
          int num3 = (int) MessageBox.Show("A file with this name already exists");
        }
      }
      else if (!File.Exists(this.directory + this.textBox.Text + ".json"))
      {
        if (this.pathType == "std" && this.selectedFilePath == this.config.sPathName)
        {
          this.config.sPathName = this.textBox.Text;
          File.WriteAllText("config\\config.json", JsonConvert.SerializeObject((object) this.config));
        }
        else if (this.pathType == "res" && this.selectedFilePath == this.config.sPathName)
        {
          this.config.rPathName = this.textBox.Text;
          File.WriteAllText("config\\config.json", JsonConvert.SerializeObject((object) this.config));
        }
        else if (this.pathType == "vnd" && this.selectedFilePath == this.config.sPathName)
        {
          this.config.vPathName = this.textBox.Text;
          File.WriteAllText("config\\config.json", JsonConvert.SerializeObject((object) this.config));
        }
        if (this.pathType == "std")
        {
          File.Move("paths\\standard paths\\deadzones\\" + this.selectedFilePath + " DeadZones.json", "paths\\standard paths\\deadzones\\" + this.textBox.Text + " DeadZones.json");
          File.Move("paths\\standard paths\\safezones\\" + this.selectedFilePath + " SafeZones.json", "paths\\standard paths\\safezones\\" + this.textBox.Text + " SafeZones.json");
        }
        File.Move(this.directory + this.selectedFilePath + ".json", this.directory + this.textBox.Text + ".json");
        this.setLabel();
        this.Close();
      }
      else
      {
        int num4 = (int) MessageBox.Show("A file with this name already exists");
      }
    }

    private void setLabel()
    {
      if (!(this.Owner.Name == "ProfileEditor"))
        return;
      ((ProfileEditor) this.Owner).profileNameL.Text = this.textBox.Text;
    }

    private void FileNameSave_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.saveAndExit();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FileNameSave));
      this.textBox = new TextBox();
      this.saveButton = new Button();
      this.SuspendLayout();
      this.textBox.BackColor = Color.Black;
      this.textBox.ForeColor = Color.White;
      this.textBox.Location = new Point(12, 12);
      this.textBox.MaxLength = 64;
      this.textBox.Name = "textBox";
      this.textBox.Size = new Size(258, 20);
      this.textBox.TabIndex = 0;
      this.textBox.TabStop = false;
      this.saveButton.Location = new Point(277, 10);
      this.saveButton.Name = "saveButton";
      this.saveButton.Size = new Size(40, 23);
      this.saveButton.TabIndex = 1;
      this.saveButton.Text = "Save";
      this.saveButton.UseVisualStyleBackColor = true;
      this.saveButton.Click += new EventHandler(this.button1_Click);
//      this.AutoScaleDimensions = new SizeF(6f, 13f);
//      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(328, 45);
      this.Controls.Add((Control) this.saveButton);
      this.Controls.Add((Control) this.textBox);
//      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
//      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FileNameSave";
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Enter a new file name";
      this.TopMost = true;
      this.KeyDown += new KeyEventHandler(this.FileNameSave_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
