
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace tbp
{
  public class Radar : Form
  {
    private List<Entity> objList = new List<Entity>(); //objects?
    private List<Entity> sNodeList = new List<Entity>(); //?
    private List<Entity> rNodeList = new List<Entity>(); //?
    private List<Entity> vNodeList = new List<Entity>();
    private List<Entity> deadZonesList = new List<Entity>();
    private List<Entity> safeZonesList = new List<Entity>();
    private List<Entity> roamNodeList = new List<Entity>();
    private Entity player = new Entity();
    private Pen bluePen = new Pen(Color.Aqua, 1f);
    private Pen yellowPen = new Pen(Color.Yellow, 1f);
    private Pen redPen = new Pen(Color.Red, 1f);
    private Pen whitePen = new Pen(Color.White, 1f);
    private Pen greenPen = new Pen(Color.LimeGreen, 1f);
    private SolidBrush whiteBrush = new SolidBrush(Color.White);
    private SolidBrush redBrush = new SolidBrush(Color.Red);
    private SolidBrush greenBrush = new SolidBrush(Color.LimeGreen);
    private SolidBrush yellowBrush = new SolidBrush(Color.Yellow);
    private FontFamily ff = new FontFamily("Arial");
    private Point[] triangle = new Point[3]
    {
      new Point(145, 150), // new Point(295, 300),
      new Point(155, 150), // new Point(305, 300),
      new Point(150, 135)  // new Point(300, 285)
    };
    private float factorX = (float) Math.Abs(26);
    private float factorY = -26f;
    public const int WM_NCLBUTTONDOWN = 161;
    public const int HT_CAPTION = 2;
    private Font font;
    private Graphics g;
    private Graphics pg;
    private float camRot;
    private float roamDist;
    private float avoidDist;
    private int editType;
    private int nearNode;
    private int nearRoamNode;
    private int mode;
    private IContainer components;
    private Panel canvas;
    private Timer refreshTimer;
    private CheckBox showMobs;
    private CheckBox showNodes;
    private CheckBox showRoamRadius;
    private CheckBox showDeadZones;
    private CheckBox showResources;
    private GroupBox groupBox1;
    private CheckBox showSafeZones;

    public Radar()
    {
      this.InitializeComponent();
      this.g = this.canvas.CreateGraphics();
      this.g.SmoothingMode = SmoothingMode.HighSpeed;
      this.pg = this.canvas.CreateGraphics();
      this.pg.SmoothingMode = SmoothingMode.HighSpeed;
      this.font = new Font(this.ff, 10f);
    }


    private void refreshTimer_Tick(object sender, EventArgs e)
    {
      if (!((MainUI) this.Owner).doRun)
       return;
 
      this.objList = ((MainUI) this.Owner).objList;
      this.sNodeList = ((MainUI) this.Owner).sNodeList;
      this.rNodeList = ((MainUI) this.Owner).rNodeList;
      this.vNodeList = ((MainUI) this.Owner).vNodeList;
      this.deadZonesList = ((MainUI) this.Owner).deadZonesList;
      this.safeZonesList = ((MainUI) this.Owner).safeZonesList;
      this.player = ((MainUI) this.Owner).player; //shows player
      this.camRot = ((MainUI) this.Owner).camToPlayerAngle.yawX;
      this.nearRoamNode = ((MainUI) this.Owner).nearRoamNode;
      this.editType = ((MainUI) this.Owner).editType;
      this.nearNode = ((MainUI) this.Owner).nearNode;
      this.mode = ((MainUI) this.Owner).config.mode;
      this.roamDist = ((MainUI) this.Owner).config.roamDist;
      this.avoidDist = ((MainUI) this.Owner).config.avoidNodeDist;
      this.roamNodeList = ((MainUI) this.Owner).roamNodesList;
      this.g.Clear(Color.Black); // BACKGROUND RADAR COLOR
      this.draw(); //display / view things in RADAR // must go last
    }

    private Point getPos(Entity obj)
    {
      return new Point()
      {
        X = (int) (((double) obj.x - (double) this.player.x) / (double) this.factorX + 150.0), //was adding 300
        Y = (int) (((double) obj.z - (double) this.player.z) / (double) this.factorY + 150.0) // same as above
      };
    }

    private void draw()
    {
      this.g.ResetTransform();
      this.g.TranslateTransform(150f, 150f); //was 300f,300f
      this.g.RotateTransform(this.camRot - 90f);
      this.g.TranslateTransform(-150f, -150f); //was 300f, 300f
      this.drawPaths();
      this.drawDeadZones();
      this.drawSafeZones();
      this.drawObjects();
      this.drawPlayer();
    }

    private void drawPlayer()
    {
      this.pg.DrawPolygon(this.whitePen, this.triangle); // bot pos
    }

    private void drawPaths()
    {
      if (!this.showNodes.Checked)
        return;
      if (this.sNodeList.Count > 0)
      {
        for (int index = 0; index < this.sNodeList.Count; ++index)
        {
          if (this.sNodeList[index].type == 1)
            this.g.DrawRectangle(this.whitePen, this.getPos(this.sNodeList[index]).X - 4, this.getPos(this.sNodeList[index]).Y - 4, 8, 8);
          else if (this.sNodeList[index].type == 2)
            this.g.DrawRectangle(this.redPen, this.getPos(this.sNodeList[index]).X - 4, this.getPos(this.sNodeList[index]).Y - 4, 8, 8);
          else if (this.sNodeList[index].type == 3)
            this.g.DrawRectangle(this.greenPen, this.getPos(this.sNodeList[index]).X - 4, this.getPos(this.sNodeList[index]).Y - 4, 8, 8);
          if (index < this.sNodeList.Count - 1)
            this.g.DrawLine(this.whitePen, new Point(this.getPos(this.sNodeList[index]).X, this.getPos(this.sNodeList[index]).Y), new Point(this.getPos(this.sNodeList[index + 1]).X, this.getPos(this.sNodeList[index + 1]).Y));
        }
        if (this.mode == 1 || this.editType == 1 && this.nearNode < this.sNodeList.Count)
          this.g.DrawRectangle(this.redPen, this.getPos(this.sNodeList[this.nearNode]).X - 6, this.getPos(this.sNodeList[this.nearNode]).Y - 6, 12, 12);
        if (this.showRoamRadius.Checked && this.roamNodeList.Count > 0 && this.nearRoamNode > -1)
          this.g.DrawEllipse(this.whitePen, (float) this.getPos(this.roamNodeList[this.nearRoamNode]).X - this.roamDist / this.factorX, (float) this.getPos(this.roamNodeList[this.nearRoamNode]).Y - this.roamDist / this.factorX, (float) ((double) this.roamDist / (double) this.factorX * 2.0), (float) ((double) this.roamDist / (double) this.factorX * 2.0));
      }
      if (this.rNodeList.Count > 0)
      {
        for (int index = 0; index < this.rNodeList.Count; ++index)
        {
          this.g.DrawRectangle(this.bluePen, this.getPos(this.rNodeList[index]).X - 4, this.getPos(this.rNodeList[index]).Y - 4, 8, 8);
          if (index < this.rNodeList.Count - 1)
            this.g.DrawLine(this.whitePen, new Point(this.getPos(this.rNodeList[index]).X, this.getPos(this.rNodeList[index]).Y), new Point(this.getPos(this.rNodeList[index + 1]).X, this.getPos(this.rNodeList[index + 1]).Y));
        }
        if (this.mode == 4 || this.editType == 2 && this.nearNode < this.rNodeList.Count)
          this.g.DrawRectangle(this.whitePen, this.getPos(this.rNodeList[this.nearNode]).X - 6, this.getPos(this.rNodeList[this.nearNode]).Y - 6, 12, 12);
      }
      if (this.vNodeList.Count <= 0)
        return;
      for (int index = 0; index < this.vNodeList.Count; ++index)
      {
        if (this.vNodeList[index].type == 1)
          this.g.DrawRectangle(this.yellowPen, this.getPos(this.vNodeList[index]).X - 4, this.getPos(this.vNodeList[index]).Y - 4, 8, 8);
        else if (this.vNodeList[index].type == 2)
          this.g.DrawRectangle(this.redPen, this.getPos(this.vNodeList[index]).X - 4, this.getPos(this.vNodeList[index]).Y - 4, 8, 8);
        if (index < this.vNodeList.Count - 1)
          this.g.DrawLine(this.whitePen, new Point(this.getPos(this.vNodeList[index]).X, this.getPos(this.vNodeList[index]).Y), new Point(this.getPos(this.vNodeList[index + 1]).X, this.getPos(this.vNodeList[index + 1]).Y));
      }
      if (this.mode != 5 && (this.editType != 3 || this.nearNode >= this.vNodeList.Count))
        return;
      this.g.DrawRectangle(this.redPen, this.getPos(this.vNodeList[this.nearNode]).X - 6, this.getPos(this.vNodeList[this.nearNode]).Y - 6, 12, 12);
    }

    private void drawDeadZones()
    {
      if (!this.showDeadZones.Checked)
        return;
      foreach (Entity entity in this.deadZonesList)
        this.g.DrawEllipse(this.redPen, (float) this.getPos(entity).X - (float) ((double) this.avoidDist / (double) this.factorX / 2.0), (float) this.getPos(entity).Y - (float) ((double) this.avoidDist / (double) this.factorX / 2.0), this.avoidDist / this.factorX, this.avoidDist / this.factorX);
    }

    private void drawSafeZones()
    {
      if (!this.showSafeZones.Checked)
        return;
      foreach (Entity entity in this.safeZonesList)
        this.g.DrawEllipse(this.greenPen, (float) this.getPos(entity).X - 150f / this.factorX, (float) this.getPos(entity).Y - 150f / this.factorX, 300f / this.factorX, 300f / this.factorX);
    }
      //^^above : 150, 150, 300, 300 was originally 250, 250, 500, 500



   // fixed but doesnt show some resources...
    private void drawObjects()
    {

        if (this.objList.Count <= 0 || this.mode != 2)
            return;
        foreach (Entity entity in this.objList)
        {
            if (entity.type != 0) // fixed it all :3 
            {
                if (this.showResources.Checked) //entity.type > 0 && 
                {
                    this.g.FillEllipse((Brush)this.greenBrush, this.getPos(entity).X - 3, this.getPos(entity).Y - 3, 6, 6);
                }
            }
            else
            {
                if (this.showMobs.Checked)
                {
                    this.g.FillEllipse((Brush)this.redBrush, this.getPos(entity).X - 3, this.getPos(entity).Y - 3, 6, 6);
                }
            }
        }
    }
       

        /* POOR CODING. 
    private void drawObjects()
    {
        
      if (this.objList.Count <= 0 || this.mode != 2)
        return;
          foreach (Entity entity in this.objList)
          {
            if (entity.type != 0 && (entity.hp != 1 || !this.showMobs.Checked)) // ify about the logic here.
            {
              if (entity.type == 1 && this.showResources.Checked)
                this.g.FillEllipse((Brush) this.greenBrush, this.getPos(entity).X - 3, this.getPos(entity).Y - 3, 6, 6);
            }
            else
              this.g.FillEllipse((Brush) this.redBrush, this.getPos(entity).X - 3, this.getPos(entity).Y - 3, 6, 6);
          }
        }
        
      */


    // decide to make them all extern ___ cuz wow sauce
    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();


      /* PANEL1 was movable frm
    private void panel1_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;
      Radar.ReleaseCapture();
      Radar.SendMessage(this.Handle, 161, 2, 0);
    }

      
    private void panel1_MouseHover(object sender, EventArgs e)
    {
    }

    private void panel1_MouseLeave(object sender, EventArgs e)
    {
    }
    */
    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }
      
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this.canvas = new System.Windows.Forms.Panel();
            this.showSafeZones = new System.Windows.Forms.CheckBox();
            this.showDeadZones = new System.Windows.Forms.CheckBox();
            this.showResources = new System.Windows.Forms.CheckBox();
            this.showRoamRadius = new System.Windows.Forms.CheckBox();
            this.showMobs = new System.Windows.Forms.CheckBox();
            this.showNodes = new System.Windows.Forms.CheckBox();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.SystemColors.Desktop;
            this.canvas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.canvas.Location = new System.Drawing.Point(12, 12);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(300, 300);
            this.canvas.TabIndex = 0;
            // 
            // showSafeZones
            // 
            this.showSafeZones.AutoSize = true;
            this.showSafeZones.Checked = true;
            this.showSafeZones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showSafeZones.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showSafeZones.Location = new System.Drawing.Point(29, 42);
            this.showSafeZones.Name = "showSafeZones";
            this.showSafeZones.Size = new System.Drawing.Size(78, 17);
            this.showSafeZones.TabIndex = 7;
            this.showSafeZones.TabStop = false;
            this.showSafeZones.Text = "Rest Areas";
            this.showSafeZones.UseVisualStyleBackColor = true;
            // 
            // showDeadZones
            // 
            this.showDeadZones.AutoSize = true;
            this.showDeadZones.Checked = true;
            this.showDeadZones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showDeadZones.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showDeadZones.Location = new System.Drawing.Point(113, 42);
            this.showDeadZones.Name = "showDeadZones";
            this.showDeadZones.Size = new System.Drawing.Size(85, 17);
            this.showDeadZones.TabIndex = 6;
            this.showDeadZones.TabStop = false;
            this.showDeadZones.Text = "Dead Zones";
            this.showDeadZones.UseVisualStyleBackColor = true;
            // 
            // showResources
            // 
            this.showResources.AutoSize = true;
            this.showResources.Checked = true;
            this.showResources.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showResources.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showResources.Location = new System.Drawing.Point(140, 19);
            this.showResources.Name = "showResources";
            this.showResources.Size = new System.Drawing.Size(77, 17);
            this.showResources.TabIndex = 5;
            this.showResources.TabStop = false;
            this.showResources.Text = "Resources";
            this.showResources.UseVisualStyleBackColor = true;
            // 
            // showRoamRadius
            // 
            this.showRoamRadius.AutoSize = true;
            this.showRoamRadius.Checked = true;
            this.showRoamRadius.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showRoamRadius.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showRoamRadius.Location = new System.Drawing.Point(204, 42);
            this.showRoamRadius.Name = "showRoamRadius";
            this.showRoamRadius.Size = new System.Drawing.Size(90, 17);
            this.showRoamRadius.TabIndex = 4;
            this.showRoamRadius.TabStop = false;
            this.showRoamRadius.Text = "Roam Radius";
            this.showRoamRadius.UseVisualStyleBackColor = true;
            // 
            // showMobs
            // 
            this.showMobs.AutoSize = true;
            this.showMobs.Checked = true;
            this.showMobs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMobs.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showMobs.Location = new System.Drawing.Point(65, 19);
            this.showMobs.Name = "showMobs";
            this.showMobs.Size = new System.Drawing.Size(69, 17);
            this.showMobs.TabIndex = 2;
            this.showMobs.TabStop = false;
            this.showMobs.Text = "Monsters";
            this.showMobs.UseVisualStyleBackColor = true;
            // 
            // showNodes
            // 
            this.showNodes.AutoSize = true;
            this.showNodes.Checked = true;
            this.showNodes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showNodes.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showNodes.Location = new System.Drawing.Point(6, 19);
            this.showNodes.Name = "showNodes";
            this.showNodes.Size = new System.Drawing.Size(53, 17);
            this.showNodes.TabIndex = 1;
            this.showNodes.TabStop = false;
            this.showNodes.Text = "Paths";
            this.showNodes.UseVisualStyleBackColor = true;
            // 
            // refreshTimer
            // 
            this.refreshTimer.Enabled = true;
            this.refreshTimer.Interval = 200;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.showNodes);
            this.groupBox1.Controls.Add(this.showMobs);
            this.groupBox1.Controls.Add(this.showSafeZones);
            this.groupBox1.Controls.Add(this.showRoamRadius);
            this.groupBox1.Controls.Add(this.showResources);
            this.groupBox1.Controls.Add(this.showDeadZones);
            this.groupBox1.Location = new System.Drawing.Point(12, 318);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 68);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filters";
            // 
            // Radar
            // 
            this.ClientSize = new System.Drawing.Size(325, 392);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.canvas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Radar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Radar";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

    }

 
  }
}
