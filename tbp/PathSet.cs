

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace tbp
{
  public class PathSet : Form
  {
    private Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config\\config.json"));
    private IContainer components;
    private Label standardPathL;
    private Label standardPathNameL;
    private Button standardPathSetButton;
    private Button resPathSetButton;
    private Label resPathNameL;
    private Label resPathL;
    private Button vendorPathSetButton;
    private Label vendorPathNameL;
    private Label vendorPathL;
    private Button button1;

    public PathSet()
    {
      this.InitializeComponent();
      this.getNames();
    }

    private void getNames()
    {
      this.standardPathNameL.Text = this.config.sPathName;
      this.resPathNameL.Text = this.config.rPathName;
      this.vendorPathNameL.Text = this.config.vPathName;
      if (this.standardPathNameL.Text == "")
        this.standardPathNameL.Text = "None, please -->";
      if (this.resPathNameL.Text == "")
        this.resPathNameL.Text = "None, please -->";
      if (!(this.vendorPathNameL.Text == ""))
        return;
      this.vendorPathNameL.Text = "None, please -->";
    }

    private void standardPathSetButton_Click(object sender, EventArgs e)
    {
      this.openSelector("std");
    }

    private void resPathSetButton_Click(object sender, EventArgs e)
    {
      this.openSelector("res");
    }

    private void vendorPathSetButton_Click(object sender, EventArgs e)
    {
      this.openSelector("vnd");
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
      this.getNames();
      this.BringToFront();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PathSet));
      this.standardPathL = new Label();
      this.standardPathNameL = new Label();
      this.standardPathSetButton = new Button();
      this.resPathSetButton = new Button();
      this.resPathNameL = new Label();
      this.resPathL = new Label();
      this.vendorPathSetButton = new Button();
      this.vendorPathNameL = new Label();
      this.vendorPathL = new Label();
      this.button1 = new Button();
      this.SuspendLayout();
      this.standardPathL.AutoSize = true;
      this.standardPathL.Location = new Point(12, 17);
      this.standardPathL.Name = "standardPathL";
      this.standardPathL.Size = new Size(53, 13);
      this.standardPathL.TabIndex = 0;
      this.standardPathL.Text = "Standard:";
      this.standardPathNameL.AutoSize = true;
      this.standardPathNameL.Location = new Point(71, 17);
      this.standardPathNameL.Name = "standardPathNameL";
      this.standardPathNameL.Size = new Size(31, 13);
      this.standardPathNameL.TabIndex = 1;
      this.standardPathNameL.Text = "none";
      this.standardPathSetButton.Location = new Point(181, 12);
      this.standardPathSetButton.Name = "standardPathSetButton";
      this.standardPathSetButton.Size = new Size(47, 23);
      this.standardPathSetButton.TabIndex = 2;
      this.standardPathSetButton.Text = "Select";
      this.standardPathSetButton.UseVisualStyleBackColor = true;
      this.standardPathSetButton.Click += new EventHandler(this.standardPathSetButton_Click);
      this.resPathSetButton.Location = new Point(181, 46);
      this.resPathSetButton.Name = "resPathSetButton";
      this.resPathSetButton.Size = new Size(47, 23);
      this.resPathSetButton.TabIndex = 5;
      this.resPathSetButton.Text = "Select";
      this.resPathSetButton.UseVisualStyleBackColor = true;
      this.resPathSetButton.Click += new EventHandler(this.resPathSetButton_Click);
      this.resPathNameL.AutoSize = true;
      this.resPathNameL.Location = new Point(71, 51);
      this.resPathNameL.Name = "resPathNameL";
      this.resPathNameL.Size = new Size(31, 13);
      this.resPathNameL.TabIndex = 4;
      this.resPathNameL.Text = "none";
      this.resPathL.AutoSize = true;
      this.resPathL.Location = new Point(12, 51);
      this.resPathL.Name = "resPathL";
      this.resPathL.Size = new Size(54, 13);
      this.resPathL.TabIndex = 3;
      this.resPathL.Text = "Res Path:";
      this.vendorPathSetButton.Location = new Point(181, 80);
      this.vendorPathSetButton.Name = "vendorPathSetButton";
      this.vendorPathSetButton.Size = new Size(47, 23);
      this.vendorPathSetButton.TabIndex = 8;
      this.vendorPathSetButton.Text = "Select";
      this.vendorPathSetButton.UseVisualStyleBackColor = true;
      this.vendorPathSetButton.Click += new EventHandler(this.vendorPathSetButton_Click);
      this.vendorPathNameL.AutoSize = true;
      this.vendorPathNameL.Location = new Point(87, 85);
      this.vendorPathNameL.Name = "vendorPathNameL";
      this.vendorPathNameL.Size = new Size(31, 13);
      this.vendorPathNameL.TabIndex = 7;
      this.vendorPathNameL.Text = "none";
      this.vendorPathL.AutoSize = true;
      this.vendorPathL.Location = new Point(12, 85);
      this.vendorPathL.Name = "vendorPathL";
      this.vendorPathL.Size = new Size(69, 13);
      this.vendorPathL.TabIndex = 6;
      this.vendorPathL.Text = "Vendor Path:";
      this.button1.Location = new Point(12, 114);
      this.button1.Name = "button1";
      this.button1.Size = new Size(47, 23);
      this.button1.TabIndex = 9;
      this.button1.Text = "Done";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
 //     this.AutoScaleDimensions = new SizeF(6f, 13f);
//      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(240, 149);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.vendorPathSetButton);
      this.Controls.Add((Control) this.vendorPathNameL);
      this.Controls.Add((Control) this.vendorPathL);
      this.Controls.Add((Control) this.resPathSetButton);
      this.Controls.Add((Control) this.resPathNameL);
      this.Controls.Add((Control) this.resPathL);
      this.Controls.Add((Control) this.standardPathSetButton);
      this.Controls.Add((Control) this.standardPathNameL);
      this.Controls.Add((Control) this.standardPathL);
 //     this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
 //     this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PathSet";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Paths";
      this.TopMost = true;
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
