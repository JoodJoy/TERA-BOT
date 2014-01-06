

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
            this.textBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.Color.White;
            this.textBox.ForeColor = System.Drawing.Color.Black;
            this.textBox.Location = new System.Drawing.Point(12, 12);
            this.textBox.MaxLength = 64;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(258, 20);
            this.textBox.TabIndex = 0;
            this.textBox.TabStop = false;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(277, 10);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(40, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // FileNameSave
            // 
            this.ClientSize = new System.Drawing.Size(328, 45);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.textBox);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileNameSave";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enter a new file name";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FileNameSave_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

    }
  }
}
