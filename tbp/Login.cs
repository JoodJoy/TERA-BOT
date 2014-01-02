// Type: sparlok_tera.Login
// Assembly: sparlok_tera, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 3937D873-7877-4034-B17E-CA518004B56C
// Assembly location: C:\Users\Z\Desktop\Tera Online\sparlok_tera-cleaned.exe

using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SharpUpdate;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace sparlok_tera
{
  public class Login : Form, ISharpUpdatable
  {
    private UserDetails user = new UserDetails();
    private string hashAndSaltSource;
    private string sTemp;
    private SharpUpdater update;
    private IContainer components;
    private TextBox userTextBox;
    private Label usernameL;
    private Label passwordL;
    private TextBox passTextBox;
    private Button button1;
    private LinkLabel siteLink;
    private CheckBox rememberCheckbox;

    public string appName
    {
      get
      {
        return "Sparlok Tera Bot";
      }
    }

    public string appId
    {
      get
      {
        return "sparlok_tera";
      }
    }

    public Assembly appAssembly
    {
      get
      {
        return Assembly.GetExecutingAssembly();
      }
    }

    public Icon appIcon
    {
      get
      {
        return this.Icon;
      }
    }

    public Uri updateXmlLocation
    {
      get
      {
        return new Uri("http://sparlok.net/updates/update.xml");
      }
    }

    public Form context
    {
      get
      {
        return (Form) this;
      }
    }

    public Login()
    {
      this.InitializeComponent();
      this.temp();
      this.update = new SharpUpdater((ISharpUpdatable) this);
      this.update.doUpdate();
      this.BringToFront();
      if (!File.Exists("user info.json"))
        File.Create("user info.json");
      else
        this.loadDetails();
    }

    private void temp()
    {
      for (int index = 1; index < 13; ++index)
      {
        switch (index - 1)
        {
          case 0:
            this.sTemp = "l";
            break;
          case 1:
            Login login1 = this;
            string str1 = login1.sTemp + "D";
            login1.sTemp = str1;
            break;
          case 2:
            Login login2 = this;
            string str2 = login2.sTemp + "E";
            login2.sTemp = str2;
            break;
          case 3:
            Login login3 = this;
            string str3 = login3.sTemp + "X";
            login3.sTemp = str3;
            break;
          case 4:
            Login login4 = this;
            string str4 = login4.sTemp + "+";
            login4.sTemp = str4;
            break;
          case 5:
            Login login5 = this;
            string str5 = login5.sTemp + "&";
            login5.sTemp = str5;
            break;
          case 6:
            Login login6 = this;
            string str6 = login6.sTemp + "~";
            login6.sTemp = str6;
            break;
          case 7:
            Login login7 = this;
            string str7 = login7.sTemp + "d";
            login7.sTemp = str7;
            break;
          case 8:
            Login login8 = this;
            string str8 = login8.sTemp + ")";
            login8.sTemp = str8;
            break;
          case 9:
            Login login9 = this;
            string str9 = login9.sTemp + "k";
            login9.sTemp = str9;
            break;
          case 10:
            Login login10 = this;
            string str10 = login10.sTemp + "D";
            login10.sTemp = str10;
            break;
          case 11:
            Login login11 = this;
            string str11 = login11.sTemp + "M";
            login11.sTemp = str11;
            break;
        }
      }
    }

    private void loadDetails()
    {
      if (JsonConvert.DeserializeObject<UserDetails>(File.ReadAllText("user info.json")) == null)
        return;
      this.user = JsonConvert.DeserializeObject<UserDetails>(File.ReadAllText("user info.json"));
      this.userTextBox.Text = this.user.name;
      this.passTextBox.Text = this.user.password;
      this.rememberCheckbox.Checked = this.user.remember;
    }

    private void saveDetails()
    {
      if (!this.rememberCheckbox.Checked)
        return;
      this.user.name = this.userTextBox.Text;
      this.user.password = this.passTextBox.Text;
      this.user.remember = this.rememberCheckbox.Checked;
      File.WriteAllText("user info.json", JsonConvert.SerializeObject((object) this.user));
    }

    public bool tryLogin(string username, string password)
    {
      MySqlConnection connection = new MySqlConnection("Server=sparlok.net; Database=sparlok_jmln1; Uid=sparlok_login; Pwd=" + this.sTemp + ";");
      MySqlCommand mySqlCommand1 = new MySqlCommand("SELECT password FROM joom_users WHERE username = @username;", connection);
      MySqlCommand mySqlCommand2 = new MySqlCommand("SELECT * FROM joom_users WHERE username = @username AND password = @password;", connection);
      mySqlCommand1.Parameters.Add(new MySqlParameter("username", (object) username));
      mySqlCommand2.Parameters.Add(new MySqlParameter("username", (object) username));
      connection.Open();
      this.hashAndSaltSource = mySqlCommand1.ExecuteScalar().ToString();
      string[] strArray = Regex.Split(this.hashAndSaltSource, ":");
      string md5Hash = this.CreateMD5Hash(this.passTextBox.Text + strArray[1]);
      mySqlCommand2.Parameters.Add(new MySqlParameter("password", (object) (md5Hash + ":" + strArray[1])));
      MySqlDataReader mySqlDataReader = mySqlCommand2.ExecuteReader();
      if (!mySqlDataReader.Read())
        return false;
      if (mySqlDataReader.IsDBNull(0))
      {
        mySqlCommand2.Connection.Close();
        mySqlDataReader.Dispose();
        mySqlCommand2.Dispose();
        return false;
      }
      else
      {
        mySqlCommand2.Connection.Close();
        mySqlDataReader.Dispose();
        mySqlCommand2.Dispose();
        return true;
      }
    }

    private void submit()
    {
      this.userTextBox.Enabled = false;
      this.passTextBox.Enabled = false;
      this.button1.Enabled = false;
      this.siteLink.Enabled = false;
      this.rememberCheckbox.Enabled = false;
      this.usernameL.Enabled = false;
      this.passwordL.Enabled = false;
      try
      {
        if (this.tryLogin(this.userTextBox.Text, this.passTextBox.Text))
        {
          this.saveDetails();
          this.DialogResult = DialogResult.OK;
          this.Close();
        }
        else
        {
          int num = (int) MessageBox.Show("Wrong user pass");
          this.userTextBox.Enabled = true;
          this.passTextBox.Enabled = true;
          this.button1.Enabled = true;
          this.siteLink.Enabled = true;
          this.rememberCheckbox.Enabled = true;
          this.usernameL.Enabled = true;
          this.passwordL.Enabled = true;
        }
      }
      catch (NullReferenceException ex)
      {
        int num = (int) MessageBox.Show("Wrong user ID");
        this.userTextBox.Enabled = true;
        this.passTextBox.Enabled = true;
        this.button1.Enabled = true;
        this.siteLink.Enabled = true;
        this.rememberCheckbox.Enabled = true;
        this.usernameL.Enabled = true;
        this.passwordL.Enabled = true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.submit();
    }

    private void Login_Load(object sender, EventArgs e)
    {
    }

    private void Login_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.submit();
    }

    private void siteLink_Click(object sender, EventArgs e)
    {
      try
      {
        Process.Start("http://www.sparlok.net/");
      }
      catch
      {
      }
    }

    public string CreateMD5Hash(string input)
    {
      byte[] hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input));
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < hash.Length; ++index)
        stringBuilder.Append(hash[index].ToString("x2"));
      return ((object) stringBuilder).ToString();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Login));
      this.userTextBox = new TextBox();
      this.usernameL = new Label();
      this.passwordL = new Label();
      this.passTextBox = new TextBox();
      this.button1 = new Button();
      this.siteLink = new LinkLabel();
      this.rememberCheckbox = new CheckBox();
      this.SuspendLayout();
      this.userTextBox.Location = new Point(90, 38);
      this.userTextBox.Name = "userTextBox";
      this.userTextBox.Size = new Size(100, 20);
      this.userTextBox.TabIndex = 1;
      this.usernameL.AutoSize = true;
      this.usernameL.Location = new Point(26, 41);
      this.usernameL.Name = "usernameL";
      this.usernameL.Size = new Size(58, 13);
      this.usernameL.TabIndex = 1;
      this.usernameL.Text = "Username:";
      this.passwordL.AutoSize = true;
      this.passwordL.Location = new Point(27, 72);
      this.passwordL.Name = "passwordL";
      this.passwordL.Size = new Size(56, 13);
      this.passwordL.TabIndex = 2;
      this.passwordL.Text = "Password:";
      this.passTextBox.Location = new Point(90, 69);
      this.passTextBox.Name = "passTextBox";
      this.passTextBox.Size = new Size(100, 20);
      this.passTextBox.TabIndex = 2;
      this.passTextBox.UseSystemPasswordChar = true;
      this.button1.Location = new Point(136, 107);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 3;
      this.button1.Text = "Login";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.siteLink.AutoSize = true;
      this.siteLink.LinkColor = Color.Black;
      this.siteLink.Location = new Point(5, 156);
      this.siteLink.Name = "siteLink";
      this.siteLink.Size = new Size(61, 13);
      this.siteLink.TabIndex = 4;
      this.siteLink.TabStop = true;
      this.siteLink.Text = "Sparlok.net";
      this.siteLink.VisitedLinkColor = Color.Black;
      this.siteLink.Click += new EventHandler(this.siteLink_Click);
      this.rememberCheckbox.AutoSize = true;
      this.rememberCheckbox.Location = new Point(40, 111);
      this.rememberCheckbox.Name = "rememberCheckbox";
      this.rememberCheckbox.Size = new Size(77, 17);
      this.rememberCheckbox.TabIndex = 5;
      this.rememberCheckbox.Text = "Remember";
      this.rememberCheckbox.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(235, 178);
      this.Controls.Add((Control) this.rememberCheckbox);
      this.Controls.Add((Control) this.siteLink);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.passTextBox);
      this.Controls.Add((Control) this.passwordL);
      this.Controls.Add((Control) this.usernameL);
      this.Controls.Add((Control) this.userTextBox);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Login";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "SPARLOK - Login";
      this.TopMost = true;
      this.Load += new EventHandler(this.Login_Load);
      this.KeyDown += new KeyEventHandler(this.Login_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
