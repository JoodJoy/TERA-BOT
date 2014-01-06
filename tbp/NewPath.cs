

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace tbp
{
  public class NewPath : Form
  {
    private Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config\\config.json"));
    private string type;
    private IContainer components;
    private TextBox nameTextBox;
    private Button createButton;
    private Label pathNameL;
    private Button cancelButton;

    public NewPath(string pathType)
    {
      this.type = pathType;
      this.InitializeComponent();
    }

    private void createButton_Click(object sender, EventArgs e)
    {
      if (this.nameTextBox.Text.StartsWith(" "))
      {
        int num1 = (int) MessageBox.Show("Name cannot start with space");
      }
      else if (this.nameTextBox.Text.Length < 1)
      {
        int num2 = (int) MessageBox.Show("Name cannot be blank");
      }
      else
      {
        try
        {
          if (this.type == "std")
          {
            this.config.sPathName = this.nameTextBox.Text;
            FileStream fileStream1 = File.Create("paths\\standard paths\\" + this.nameTextBox.Text + ".json");
            FileStream fileStream2 = File.Create("paths\\standard paths\\deadzones\\" + this.nameTextBox.Text + " DeadZones.json");
            FileStream fileStream3 = File.Create("paths\\standard paths\\safezones\\" + this.nameTextBox.Text + " SafeZones.json");
            fileStream1.Close();
            fileStream1.Dispose();
            fileStream2.Close();
            fileStream2.Dispose();
            fileStream3.Close();
            fileStream3.Dispose();
          }
          else if (this.type == "res")
          {
            this.config.rPathName = this.nameTextBox.Text;
            FileStream fileStream = File.Create("paths\\res paths\\" + this.nameTextBox.Text + ".json");
            fileStream.Close();
            fileStream.Dispose();
          }
          else if (this.type == "vnd")
          {
            this.config.vPathName = this.nameTextBox.Text;
            FileStream fileStream = File.Create("paths\\vendor paths\\" + this.nameTextBox.Text + ".json");
            fileStream.Close();
            fileStream.Dispose();
          }
          File.WriteAllText("config\\config.json", JsonConvert.SerializeObject((object) this.config));
          this.Close();
        }
        catch (IOException ex)
        {
          Thread.Sleep(TimeSpan.FromSeconds(1.0));
        }
      }
    }

    private void cancelButton_Click(object sender, EventArgs e)
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
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.createButton = new System.Windows.Forms.Button();
            this.pathNameL = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nameTextBox
            // 
            this.nameTextBox.BackColor = System.Drawing.Color.White;
            this.nameTextBox.ForeColor = System.Drawing.Color.Black;
            this.nameTextBox.Location = new System.Drawing.Point(15, 25);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(179, 20);
            this.nameTextBox.TabIndex = 3;
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(200, 25);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(53, 20);
            this.createButton.TabIndex = 4;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // pathNameL
            // 
            this.pathNameL.AutoSize = true;
            this.pathNameL.Location = new System.Drawing.Point(12, 9);
            this.pathNameL.Name = "pathNameL";
            this.pathNameL.Size = new System.Drawing.Size(113, 13);
            this.pathNameL.TabIndex = 5;
            this.pathNameL.Text = "Please name the path:";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(259, 25);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(53, 20);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // NewPath
            // 
            this.ClientSize = new System.Drawing.Size(323, 57);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.pathNameL);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.nameTextBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewPath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Path";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

    }
  }
}
