

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace tbp
{
  public class EditPrompt : Form
  {
    private Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config\\config.json"));
    private string type;
    private IContainer components;
    private RadioButton standardRb;
    private RadioButton resRb;
    private RadioButton vendorRb;
    private Button Edit;
    private Label label1;
    private Label pathNameL;
    private GroupBox groupBox1;
    private Button pathChangeButton;

    public EditPrompt()
    {
      this.InitializeComponent();
      this.BringToFront();
      this.setName();
    }

    private void Edit_Click(object sender, EventArgs e)
    {
      if (this.standardRb.Checked)
        ((MainUI) this.Owner).editType = 1;
      else if (this.resRb.Checked)
        ((MainUI) this.Owner).editType = 2;
      else if (this.vendorRb.Checked)
        ((MainUI) this.Owner).editType = 3;
      this.Close();
    }

    private void setName()
    {
      if (this.standardRb.Checked)
      {
        this.pathNameL.Text = this.config.sPathName;
        this.type = "std";
      }
      else if (this.resRb.Checked)
      {
        this.pathNameL.Text = this.config.rPathName;
        this.type = "res";
      }
      else if (this.vendorRb.Checked)
      {
        this.pathNameL.Text = this.config.vPathName;
        this.type = "vnd";
      }
      if (this.pathNameL.Text.Length < 1)
      {
        this.pathNameL.Text = "None selected, please --->";
        this.Edit.Enabled = false;
      }
      else
        this.Edit.Enabled = true;
    }

    private void openSelector(string type)
    {
      if (Application.OpenForms["PathSelect"] is PathSelect)
        return;
      PathSelect pathSelect = new PathSelect(type);
      pathSelect.Owner = this.Owner;
      pathSelect.FormClosed += new FormClosedEventHandler(this.pathSelect_FormClosed);
      ((Control) pathSelect).Show();
    }

    private void pathSelect_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config\\config.json"));
      this.setName();
      this.BringToFront();
    }

    private void pathChangeButton_Click(object sender, EventArgs e)
    {
      this.openSelector(this.type);
    }

    private void standardRb_CheckedChanged(object sender, EventArgs e)
    {
      this.setName();
    }

    private void resRb_CheckedChanged(object sender, EventArgs e)
    {
      this.setName();
    }

    private void vendorRb_CheckedChanged(object sender, EventArgs e)
    {
      this.setName();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.standardRb = new System.Windows.Forms.RadioButton();
            this.resRb = new System.Windows.Forms.RadioButton();
            this.vendorRb = new System.Windows.Forms.RadioButton();
            this.Edit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pathNameL = new System.Windows.Forms.Label();
            this.pathChangeButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // standardRb
            // 
            this.standardRb.AutoSize = true;
            this.standardRb.Checked = true;
            this.standardRb.Location = new System.Drawing.Point(6, 19);
            this.standardRb.Name = "standardRb";
            this.standardRb.Size = new System.Drawing.Size(68, 17);
            this.standardRb.TabIndex = 0;
            this.standardRb.TabStop = true;
            this.standardRb.Text = "Standard";
            this.standardRb.UseVisualStyleBackColor = true;
            this.standardRb.CheckedChanged += new System.EventHandler(this.standardRb_CheckedChanged);
            // 
            // resRb
            // 
            this.resRb.AutoSize = true;
            this.resRb.Location = new System.Drawing.Point(80, 20);
            this.resRb.Name = "resRb";
            this.resRb.Size = new System.Drawing.Size(44, 17);
            this.resRb.TabIndex = 1;
            this.resRb.Text = "Res";
            this.resRb.UseVisualStyleBackColor = true;
            this.resRb.CheckedChanged += new System.EventHandler(this.resRb_CheckedChanged);
            // 
            // vendorRb
            // 
            this.vendorRb.AutoSize = true;
            this.vendorRb.Location = new System.Drawing.Point(156, 19);
            this.vendorRb.Name = "vendorRb";
            this.vendorRb.Size = new System.Drawing.Size(59, 17);
            this.vendorRb.TabIndex = 2;
            this.vendorRb.Text = "Vendor";
            this.vendorRb.UseVisualStyleBackColor = true;
            this.vendorRb.CheckedChanged += new System.EventHandler(this.vendorRb_CheckedChanged);
            // 
            // Edit
            // 
            this.Edit.Location = new System.Drawing.Point(225, 47);
            this.Edit.Name = "Edit";
            this.Edit.Size = new System.Drawing.Size(44, 23);
            this.Edit.TabIndex = 3;
            this.Edit.Text = "OK";
            this.Edit.UseVisualStyleBackColor = true;
            this.Edit.Click += new System.EventHandler(this.Edit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Path file:";
            // 
            // pathNameL
            // 
            this.pathNameL.AutoSize = true;
            this.pathNameL.Location = new System.Drawing.Point(63, 19);
            this.pathNameL.Name = "pathNameL";
            this.pathNameL.Size = new System.Drawing.Size(16, 13);
            this.pathNameL.TabIndex = 5;
            this.pathNameL.Text = "---";
            // 
            // pathChangeButton
            // 
            this.pathChangeButton.Location = new System.Drawing.Point(214, 14);
            this.pathChangeButton.Name = "pathChangeButton";
            this.pathChangeButton.Size = new System.Drawing.Size(55, 23);
            this.pathChangeButton.TabIndex = 6;
            this.pathChangeButton.Text = "Change";
            this.pathChangeButton.UseVisualStyleBackColor = true;
            this.pathChangeButton.Click += new System.EventHandler(this.pathChangeButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.standardRb);
            this.groupBox1.Controls.Add(this.resRb);
            this.groupBox1.Controls.Add(this.vendorRb);
            this.groupBox1.Location = new System.Drawing.Point(12, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 43);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // EditPrompt
            // 
            this.ClientSize = new System.Drawing.Size(281, 140);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pathChangeButton);
            this.Controls.Add(this.pathNameL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Edit);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditPrompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Mode - Select path type";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
  }
}
