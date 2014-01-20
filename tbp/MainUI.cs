
using MouseKeyboardActivityMonitor.Controls;
using Newtonsoft.Json;
using ReadWriteMemory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;

namespace tbp
{
  public class MainUI : Form
  {
  
    private IntPtr teraWindowHandle = MainUI.FindWindow((string) null, "TERA");
    private Point inventBase = new Point(820, 220);
    private Inventory invent = new Inventory();
    public int targetNode = -1;
    private int pathCombatTimerCaller = 1;
    private int zoomDist = 500;
    private int pathDir = 1;
    private int parseHits = -1;
    private float PI = 3.141593f;
    private string mainModule = "TERA.exe";
    private string version = ((object) Application.ProductVersion).ToString();
    public Config config = new Config();
    public Entity player = new Entity();
    public List<Entity> roamNodesList = new List<Entity>();
    public List<Entity> deadZonesList = new List<Entity>();
    public List<Entity> safeZonesList = new List<Entity>();
    public List<Entity> ol = new List<Entity>();
    public List<Entity> objList = new List<Entity>();
    public List<Entity> sNodeList = new List<Entity>();
    public List<Entity> rNodeList = new List<Entity>();
    public List<Entity> vNodeList = new List<Entity>();
    public List<Command> commandList = new List<Command>();
    public Entity camToPlayerAngle = new Entity();
    private List<Command> offCdCommands = new List<Command>();
    private Color CursorPixel = new Color();
    private Stopwatch runTime = new Stopwatch();
    private Stopwatch roamTime = new Stopwatch();
    private ProcessMemory Mem = new ProcessMemory("TERA");
    private StringList strings = new StringList();
    private cTimer attackDelay = new cTimer();
    private cTimer parseDelay = new cTimer();
    private cTimer mobKillDelay = new cTimer();
    private cTimer flipDelay = new cTimer();
    private cTimer zoomDelay = new cTimer();
    private cTimer seekDelay = new cTimer();
    private Entity target = new Entity();
    private Entity camera = new Entity();
    private Entity oldPlayer = new Entity();
    private Entity camToTargetAngle = new Entity();
    private Entity playerToTargetAngle = new Entity();
    private List<string> mobFilterList = new List<string>();
    private List<string> gatherWhiteList = new List<string>();
    private List<string> gatherBlackList = new List<string>();
    private List<string> pickupFilterList = new List<string>();
    private Radar radar = new Radar();
    public bool doRun;
    private bool devMode;
    private bool atVendor;
    private bool buttonFound;
    private bool usedRestSkill;
    private bool control;
    private bool inRange;
    private bool avoiding;
    private bool unsticking;
    private bool passItem;
    private bool combatStarted;
    private bool finishedSelling;
    private bool fullInvent;
    private Decimal startMoney;
    private Decimal currentMoney;
    private Decimal gainedMoney;
    public int editType;
    public int nearNode;
    public int nearRoamNode;
    private int inventItemCount;
    private int inventColumnIndex;
    private int inventRowIndex;
    private int commandIndex;
    private int maxHp;
    private int hpPercent;
    private int playerLevel;
    private int startPlayerLevel;
    private int gainedLevels;
    private int nearDeadZoneIndex;
    private int nearSafeZoneIndex;
    private int targetIndex;
    private int ySeekTick;
    private int sideStepTick;
    private int checkStuckTick;
    private int onStuckTick;
    private int NearObjListP;
    private int lockedOn;
    private int parseOffset;
    private int collectableId;
    private int combatState;
    private int rangerLootTimerTick;
    private int playerXp;
    private int oldXp;
    private float focus;
    private float distToNode;
    private float distToTarget;
    private float oldAttDist;
    private float camVerRot;
    private float rotDir;
    private string status;
    private string pathStatus;
    private string collectableName;
    public IntPtr windowHandle;
    private IContainer components;
    private Button modeToggleButton;
    private Button profileButton;
    private Button settingsButton;
    private Button radarToggleButton;
    private System.Windows.Forms.Timer oneSecTimer;
    private System.Windows.Forms.Timer mainTimer;
    private Button startButton;
    private Label profileL;
    private Label statusL;
    private Label runtimeL;
    private Label levelGainL;
    private Label pathL;
    private Label goldGainL;
    private MouseKeyEventProvider input;
    private Button pathButton;
    private System.Windows.Forms.Timer fileLoadTimer;
    private System.Windows.Forms.Timer ySeekTimer;
    private System.Windows.Forms.Timer unstickTimer;
    private System.Windows.Forms.Timer pickupTimer;
    private Label roamTimerL;
    private System.Windows.Forms.Timer retreatTimer;
    private System.Windows.Forms.Timer autoHealTimer;
    private Label loadLabel;
    private System.Windows.Forms.Timer rangerLootTimer;
    private System.Windows.Forms.Timer restTimer;
    private Label classL;
    private Label useMountL;
    private Label restL;
    private Label kiteL;
    private Label controlL;
    private System.Windows.Forms.Timer sideStepTimer;
    private System.Windows.Forms.Timer vendorTimer;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private System.Windows.Forms.Timer attackLoopTimer;
    private System.Windows.Forms.Timer findButton;
    private System.Windows.Forms.Timer oneMsTimer;
    private System.Windows.Forms.Timer pathCombatTimer;

    public MainUI()
    {
      this.InitializeComponent();
      this.radar.Owner = (Form) this;
      this.radar.Visible = false;
      this.checkDirectories();
      this.fileLoadTimer.Enabled = true;
    }

      // moving to lower right coner
    protected override void OnLoad(EventArgs e)
    {
        PlaceLowerRight();
        base.OnLoad(e);
    }

    private void PlaceLowerRight()
    {
        //Determine "rightmost" screen
        Screen rightmost = Screen.AllScreens[0];
        foreach (Screen screen in Screen.AllScreens)
        {
            if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                rightmost = screen;
        }

        this.Left = rightmost.WorkingArea.Right - this.Width;
        this.Top = rightmost.WorkingArea.Bottom - this.Height;
    }


    private void mainTimer_Tick(object sender, EventArgs e)
    {
      this.readMemory();
      this.checkDead();
      this.managePath();
      this.updateNodeIndexes();
      if (this.doRun)
        this.manageMode();
      this.updateLabels();
      if (!this.devMode)
        return;
      Console.Clear();
    // saving code //  Console.WriteLine("MODE: " + (object) this.config.mode + " kill delay:" + (string) (object) (bool) (this.mobKillDelay.delayed ? 1 : 0) + " tick: " + (string) (object) this.mobKillDelay.duration);

        // alt solution
      Console.WriteLine("MODE: " + (object)this.config.mode + " kill delay:" + (string)(object)(this.mobKillDelay.delayed ? 1 : 0) + " tick: " + (string)(object)this.mobKillDelay.duration);

        Console.WriteLine(string.Concat(new object[4]
      {
        (object) "Item Count: ",
        (object) this.inventItemCount,
        (object) " Full Invent: ",
       // saving code // (object) (bool) (this.fullInvent ? 1 : 0)

       // alt solution
       (object) (this.fullInvent ? 1 : 0) // pretty sure fullInvent is already a BOOL!

      }));
      Console.WriteLine("Target: " + this.target.name);
      Console.WriteLine("");
      foreach (Entity entity in this.objList)
        ;
      Console.WriteLine("ATT Delay tick: " + (object) this.attackDelay.duration);
      Console.WriteLine("OFF CD COMMANDS: " + (object) this.offCdCommands.Count);
      foreach (Command command in this.offCdCommands)
        Console.WriteLine(command.slot + (object) " " + (string) (object) command.cd + " " + (string) (object) command.cdTick);
      Console.WriteLine("");
      Console.WriteLine("ON CD COMMANDS: " + (object) this.commandList.Count);
      foreach (Command command in this.commandList)
        Console.WriteLine(command.slot + (object) " " + (string) (object) command.cd + " " + (string) (object) command.cdTick);
    }

    private void slowTimer_Tick(object sender, EventArgs e)
    {
      this.updateLabels();
      this.runManager();
      if (!this.doRun)
        return;
      this.status = "";
      this.clocks();
      this.manageZoom();
      this.checkForStuck();
      this.updateMoneyAndXp();
      this.updateCooldowns();
    }

    private void oneMsTimer_Tick(object sender, EventArgs e)
    {
      this.attackDelay.tick();
    }

    private void fileLoadTimer_Tick(object sender, EventArgs e)
    {
      this.loadFiles();
      this.loadInitialRadar();
    //  this.loadingPanel.Visible = false;
      this.settingsButton.Enabled = true;
      this.profileButton.Enabled = true;
      this.pathButton.Enabled = true;
      this.radarToggleButton.Enabled = true;
      this.modeToggleButton.Enabled = true;
     this.BringToFront(); //dont want forms to stick out like a sore thumb
      this.fileLoadTimer.Enabled = false;
    }

    private void checkPickup()
    {
      if (this.control && !this.fullInvent && (this.combatState == 0 && this.target.type != 1) && this.collectableId > 0)
      {
        this.pickupTimer.Enabled = true;
      }
      else
      {
        this.pickupTimer.Enabled = false;
        this.config.mode = 2;
      }
    }

    private void pickupTimer_Tick(object sender, EventArgs e)
    {
      this.pickup();
      this.pickupTimer.Enabled = false;
    }

    private void updateLabels()
    {
      if (this.config.mode == 1)
        this.pathStatus = "At (" + (object) (this.nearNode + 1) + "/" + (string) (object) this.sNodeList.Count + ") Going to " + (string) (object) (this.targetNode + 1);
      else if (this.config.mode == 3)
        this.pathStatus = "At [" + (object) (this.nearNode + 1) + "/" + (string) (object) this.sNodeList.Count + "]";
      this.runtimeL.Text = "Run Time: " + string.Format("{0:00}:{1:00}:{2:00}", (object) this.runTime.Elapsed.Hours, (object) this.runTime.Elapsed.Minutes, (object) this.runTime.Elapsed.Seconds);
      this.roamTimerL.Text = "Roam Time: " + string.Format("{0:00}:{1:00}", (object) this.roamTime.Elapsed.Minutes, (object) this.roamTime.Elapsed.Seconds);
      this.statusL.Text = "STATUS: " + this.status;
      if (!this.control && this.doRun)
        this.status = "Press F9 to enable bot control";
      else if (!this.doRun)
        this.status = "Please attach the bot";
      this.classL.Text = "Class type: " + this.config.classType;
      this.kiteL.Text = this.config.useAutoKite.ToString();
      if (this.config.useAutoKite)
        this.kiteL.ForeColor = Color.LimeGreen;
      else
        this.kiteL.ForeColor = Color.Red;
      this.restL.Text = this.config.useAutoRest.ToString();
      if (this.config.useAutoRest)
        this.restL.ForeColor = Color.LimeGreen;
      else
        this.restL.ForeColor = Color.Red;
      this.useMountL.Text = this.config.useMount.ToString();
      if (this.config.useMount)
        this.useMountL.ForeColor = Color.LimeGreen;
      else
        this.useMountL.ForeColor = Color.Red;
      this.controlL.Text = this.control.ToString();
      if (this.control)
        this.controlL.ForeColor = Color.LimeGreen;
      else
        this.controlL.ForeColor = Color.Red;
      this.levelGainL.Text = "Levels Gained: " + (object) this.gainedLevels;
      this.goldGainL.Text = "Gold Gained: " + string.Format("{0:0.00}", (object) this.gainedMoney);
      this.pathL.Text = "Path: " + this.pathStatus;
      this.profileL.Text = "Profile: " + this.config.profileName;
      if (this.config.mode == 1)
        this.modeToggleButton.Text = "Mode: PATH";
      else if (this.config.mode == 2)
      {
        this.modeToggleButton.Text = "Mode: ROAM";
      }
      else
      {
        if (this.config.mode != 3)
          return;
        this.modeToggleButton.Text = "Mode: EDIT";
      }
    }

    private void clocks()
    {
      if (!this.control)
        return;
      this.parseDelay.tick();
      this.mobKillDelay.tick();
      this.flipDelay.tick();
      this.zoomDelay.tick();
      this.seekDelay.tick();
      if (this.config.roamDuration > 0 && (this.roamTime.Elapsed.Minutes >= this.config.roamDuration && this.combatState == 0 && this.collectableId == 0))
      {
        this.roamTime.Stop();
        this.roamTime.Reset();
        if (this.sNodeList.Count > 0 && this.sNodeList[this.findNearestObjIndex(this.sNodeList.ToArray(), this.player)].type != 2)
          this.targetNode = this.findNearestObjIndex(this.sNodeList.ToArray(), this.player);
        else if (this.sNodeList.Count > 2)
        {
          try
          {
            this.targetNode = this.findNearestObjIndex(this.sNodeList.ToArray(), this.player) + 1;
          }
          catch
          {
            this.targetNode = -1;
          }
        }
        this.config.mode = 1;
      }
      if (this.roamTime.Elapsed.Seconds <= this.config.roamDuration * 60)
        return;
      this.roamTime.Stop();
      this.roamTime.Reset();
    }

    private void runManager()
    {
      if (!this.Mem.CheckProcess())
      {
        this.status = "Waiting for game process";
        this.doRun = false;
      }
      else
      {
        if (this.doRun)
          return;
        this.startButton.Enabled = true;
      }
    }

    private void manageMode()
    {
      if (this.config.mode == 1)
      {
        if (this.player.type != 1 && this.control && (this.config.useMount && !this.attackDelay.delayed))
        {
          InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_0);
          this.attackDelay.delay(100);
        }
        if (this.sNodeList.Count > 0)
        {
          this.updateAngles(this.sNodeList[this.targetNode]);
          this.moveToNode();
        }
        else
          this.camToPlayerAngle = this.getAngles(this.player, this.camera);
        if (this.combatState == 1)
        {
          this.pathCombatTimerCaller = this.config.mode;
          this.pathCombatTimer.Enabled = true;
        }
      }
      if (this.config.mode == 2)
      {
        if (this.player.type == 1 && this.control && !this.attackDelay.delayed)
        {
          InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_0); // 0 to mount
          this.attackDelay.delay(100);
        }
        if (this.collectableId > 0 && this.combatState == 0 && (this.target.type != 1 && this.control))
        {
            InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_F); // press f to loot. //was causing loot probs and was this : InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W); 
          this.status = "Looting...";
          this.checkPickup();
        }
        else
        {
          this.updateAngles(this.target);
          this.checkRoamRange();
          this.manageAvoidance();
          this.getTarget();
          this.onOrient(this.config.attackDist);
          this.getCommands();
        }
      }
      if (this.config.mode == 3)
      {
        this.status = "Press F10 again to save path";
        this.camToPlayerAngle = this.getAngles(this.player, this.camera);
        if (this.editType == 1 && this.sNodeList.Count > 0)
          this.nearNode = this.findNearestObjIndex(this.sNodeList.ToArray(), this.player);
        else if (this.editType == 2 && this.rNodeList.Count > 0)
          this.nearNode = this.findNearestObjIndex(this.rNodeList.ToArray(), this.player);
        else if (this.editType == 3 && this.vNodeList.Count > 0)
          this.nearNode = this.findNearestObjIndex(this.vNodeList.ToArray(), this.player);
      }
      if (this.config.mode == 4 && this.rNodeList.Count > 0)
      {
        if (this.player.type != 1 && this.control && (this.config.useMount && !this.attackDelay.delayed))
        {
          InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_0);
          this.attackDelay.delay(100);
        }
        this.updateAngles(this.rNodeList[this.targetNode]);
        this.moveToNode();
        if (this.combatState == 1)
        {
          this.pathCombatTimerCaller = this.config.mode;
          this.pathCombatTimer.Enabled = true;
        }
      }
      if (this.config.mode == 5 && this.vNodeList.Count > 0)
      {
        if (this.player.type != 1 && this.control && (this.config.useMount && !this.attackDelay.delayed))
        {
          InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_0);
          this.attackDelay.delay(100);
        }
        if (!this.atVendor)
        {
          this.updateAngles(this.vNodeList[this.targetNode]);
          this.moveToNode();
          if (this.combatState == 1)
          {
            this.pathCombatTimerCaller = this.config.mode;
            this.pathCombatTimer.Enabled = true;
          }
        }
        else if (!this.buttonFound && !this.findButton.Enabled)
        {
          if (this.collectableId > 0)
          {
            Thread.Sleep(TimeSpan.FromMilliseconds(1000.0));
            InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_F);
            Thread.Sleep(TimeSpan.FromMilliseconds(1000.0));
            this.setCursor(50, 200);
            Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
            this.findButton.Enabled = true;
            this.targetNode = -1;
          }
        }
        else if (this.buttonFound)
        {
          Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
          this.mouseClick(1);
          Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
          this.config.mode = 6;
        }
      }
      if (this.config.mode == 6 && this.control)
      {
        this.status = "Selling items...";
        if (!this.finishedSelling)
        {
          this.vendorTimer.Enabled = true;
        }
        else
        {
          this.vendorTimer.Enabled = false;
          this.atVendor = false;
          this.status = "Finished selling...";
        }
      }
      if (this.config.mode != 7)
        return;
      if (this.combatState == 1)
      {
        this.pathCombatTimerCaller = this.config.mode;
        this.pathCombatTimer.Enabled = true;
      }
      else
      {
        if (this.pickupTimer.Enabled)
          return;
        this.pickupTimer.Enabled = true;
      }
    }

    private void updateMoneyAndXp()
    {
      this.gainedMoney = (this.currentMoney - this.startMoney) / new Decimal(10000);
      this.gainedLevels = this.playerLevel - this.startPlayerLevel;
    }

    private void startButton_Click(object sender, EventArgs e)
    {
      if (this.config.profileName == null)
      {
        int num = (int) MessageBox.Show("You need to set a profile first");
      }
      else
      {
        this.doRun = true;
        this.startButton.Enabled = false;
        this.Mem.StartProcess();
        this.mainTimer.Enabled = true;
        this.input.Enabled = true;
        if (this.config.radarToggle == 1 && !this.radar.Visible)
          this.radar.Show((IWin32Window) this);
          //**************************************************************************************************
          //*** GOTTA FIND YOUR OWN! :D
          //**************************************************************************************************
        if (this.config.client == "US")
        {
          this.startMoney = (Decimal) this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
          this.startPlayerLevel = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
        }
        else
        {
          if (!(this.config.client == "EU"))
            return;
          this.startMoney = (Decimal)this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
          this.startPlayerLevel = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
        }
      }
    }

    private void modeToggleButton_Click(object sender, EventArgs e)
    {
      this.toggleMode();
    }

    private void radarToggleButton_Click(object sender, EventArgs e)
    {
      this.toggleRadar();
    }

    private void pathButton_Click(object sender, EventArgs e)
    {
      try
      {
        File.WriteAllText("config\\config.json", JsonConvert.SerializeObject((object) this.config));
      }
      catch (IOException ex)
      {
        Thread.Sleep(TimeSpan.FromSeconds(1.0));
      }
      if (Application.OpenForms["PathSet"] is PathSet)
        return;
      PathSet pathSet = new PathSet();
      pathSet.Owner = (Form) this;
      pathSet.FormClosed += new FormClosedEventHandler(this.pathSet_FormClosed);
      ((Control) pathSet).Show();
    }

    private void pathSet_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.loadFiles();
      this.BringToFront();
    }

    private void profileButton_Click(object sender, EventArgs e)
    {
      try
      {
        File.WriteAllText("config\\config.json", JsonConvert.SerializeObject((object) this.config));
      }
      catch (IOException ex)
      {
        Thread.Sleep(TimeSpan.FromSeconds(1.0));
      }
      if (Application.OpenForms["ProfileSelect"] is ProfileSelect)
        return;
      ProfileSelect profileSelect = new ProfileSelect();
      profileSelect.Owner = (Form) this;
      profileSelect.FormClosed += new FormClosedEventHandler(this.profileSelect_FormClosed);
      ((Control) profileSelect).Show();
    }

    private void profileSelect_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.BringToFront();
    }

    private void settingsButton_Click(object sender, EventArgs e)
    {
      this.config.attackDist = this.oldAttDist;
      try
      {
        File.WriteAllText("config\\config.json", JsonConvert.SerializeObject((object) this.config));
      }
      catch (IOException ex)
      {
        Thread.Sleep(TimeSpan.FromSeconds(1.0));
      }
      if (Application.OpenForms["Settings"] is Settings)
        return;
      Settings settings = new Settings();
      settings.Owner = (Form) this;
      settings.FormClosed += new FormClosedEventHandler(this.settings_FormClosed);
      ((Control) settings).Show();
    }

  private void settings_FormClosed(object sender, FormClosedEventArgs e)
   {
      this.BringToFront();
   }

    private int findNearestObjIndex(Entity[] objects, Entity pl)
    {
      float[] numArray = new float[objects.Length];
      for (int index = 0; index < objects.Length; ++index)
        numArray[index] = this.getDistance(objects[index], pl);
      float[] array = new float[numArray.Length];
      Array.Copy((Array) numArray, (Array) array, numArray.Length);
      Array.Sort<float>(array);
      for (int index = 0; index < numArray.Length; ++index)
      {
        if ((double) numArray[index] == (double) array[0])
          return index;
      }
      return -1;
    }

    private float radToDeg(float rad)
    {
      return rad * (180f / this.PI);
    }

    private float getDistance(Entity to, Entity from)
    {
      float num1 = from.x - to.x;
      float num2 = from.y - to.y;
      float num3 = from.z - to.z;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
    }

    private Entity getAngles(Entity to, Entity from)
    {
      Entity entity = new Entity();
      entity.dx = to.x - from.x;
      entity.dz = to.z - from.z;
      entity.dy = to.y - from.y;
      entity.yawX = (float) Math.Atan2((double) entity.dz, (double) entity.dx);
      entity.pitchY = (float) Math.Atan2((double) to.z - (double) from.z, (double) this.getDistance(to, from));
      if ((double) entity.yawX < 0.0)
        entity.yawX = 2f * this.PI + entity.yawX;
      entity.yawX = this.radToDeg(entity.yawX);
      entity.pitchY = this.radToDeg(entity.pitchY);
      return entity;
    }

    private void updateAngles(Entity targ)
    {
      this.playerToTargetAngle = this.getAngles(targ, this.player);
      this.camToPlayerAngle = this.getAngles(this.player, this.camera);
      this.camToTargetAngle = this.getAngles(targ, this.camera);
      this.distToTarget = this.getDistance(targ, this.player);
    }

    private void toggleOrient()
    {
      if (this.control)
      {
        this.runTime.Stop();
        this.control = false;
        this.inRange = false;
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
      }
      else
      {
        this.control = true;
        this.runTime.Start();
      }
    }

    private void onOrient(float attDist)
    {
      if (!this.control || this.unsticking || !(this.status != "Picking up"))
        return;
      this.rotDir = this.camToPlayerAngle.yawX - this.playerToTargetAngle.yawX;
      if (this.target.name != "node")
      {
        switch (this.target.type)
        {
          case 0:
            if ((double) this.distToTarget > (double) attDist)
            {
              this.inRange = false;
              if ((double) this.playerToTargetAngle.yawX - (double) this.camToTargetAngle.yawX < 5.0 && (double) this.playerToTargetAngle.yawX - (double) this.camToTargetAngle.yawX > -5.0)
              {
                InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_W);
                this.status = "Moving to " + this.target.name;
                break;
              }
              else
              {
                InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
                break;
              }
            }
            else if ((double) this.distToTarget <= (double) attDist)
            {
              if (InputSimulator.IsKeyDown(VirtualKeyCode.VK_W))
                InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
              this.status = "...";
              this.inRange = true;
              break;
            }
            else
              break;
          case 1:
            if ((double) this.distToTarget > 50.0 && this.combatState == 0)
            {
              this.inRange = false;
              if ((double) this.playerToTargetAngle.yawX - (double) this.camToTargetAngle.yawX < 5.0 && (double) this.playerToTargetAngle.yawX - (double) this.camToTargetAngle.yawX > -5.0)
              {
                InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_W);
                this.status = "Moving to " + this.target.name;
                break;
              }
              else
              {
                InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
                break;
              }
            }
            else if ((double) this.distToTarget <= 50.0 && this.combatState == 0)
            {
              if (InputSimulator.IsKeyDown(VirtualKeyCode.VK_W))
                InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
              if (this.collectableId > 0)
              {
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_F);
                this.status = "Gathering " + this.target.name;
              }
              this.inRange = true;
              break;
            }
            else
              break;
        }
      }
      else if (this.target.name == "node")
      {
        if ((double) this.distToTarget > 50.0)
        {
          this.inRange = false;
          if ((double) this.playerToTargetAngle.yawX - (double) this.camToTargetAngle.yawX < 5.0 && (double) this.playerToTargetAngle.yawX - (double) this.camToTargetAngle.yawX > -5.0)
          {
            InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_W);
            this.status = "Moving to path node";
          }
          else
            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
        }
        else if ((double) this.distToTarget <= 50.0)
        {
          if (InputSimulator.IsKeyDown(VirtualKeyCode.VK_W))
            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
          this.inRange = true;
        }
      }
      if ((double) this.rotDir > 2.0 && (double) Math.Abs(this.rotDir) <= 180.0)
        this.mouseMove(10, 0);
      else if ((double) this.rotDir > -2.0 && (double) Math.Abs(this.rotDir) > 180.0)
        this.mouseMove(-10, 0);
      else if ((double) this.rotDir < -2.0 && (double) Math.Abs(this.rotDir) <= 180.0)
        this.mouseMove(-10, 0);
      else if ((double) this.rotDir < 2.0 && (double) Math.Abs(this.rotDir) > 180.0)
        this.mouseMove(10, 0);
      else if (this.config.classType == "ranged")
      {
        if (this.lockedOn == 0 && (double) this.distToTarget < (double) attDist + 10.0 && (!this.seekDelay.delayed && this.target.type == 0))
        {
          this.ySeekTimer.Enabled = true;
          this.seekDelay.delay(1);
        }
        else if (this.lockedOn == 1)
          this.ySeekTick = 60;
        else if (this.target.type == 1)
          this.ySeekCloseRange();
      }
      else if (this.config.classType == "melee")
        this.ySeekCloseRange();
      if (this.combatState == 1 && this.config.useRetreatSkill && (double) this.distToTarget < (double) this.config.RetreatDist)
        this.retreatTimer.Enabled = true;
      if (this.combatState == 1 && this.config.useAutoKite && (double) this.distToTarget < (double) this.config.kiteDistance)
      {
        if (this.config.kiteMode == 1)
          InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_S);
        else if (this.config.kiteMode == 2)
        {
          InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_S);
          InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_A);
        }
        else if (this.config.kiteMode == 3)
        {
          InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_S);
          InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_D);
        }
      }
      else if (this.config.useAutoKite)
      {
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_S);
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
      }
   
      if (this.hpPercent < this.config.HealMin && this.combatState == 1 && this.config.useHealSkill)
        this.autoHealTimer.Enabled = true;
      if (this.hpPercent >= this.config.RestMin || this.combatState != 0 || (!this.config.useAutoRest || this.safeZonesList.Count <= 0))
        return;
      this.restTimer.Enabled = true;

    }

    private void restTimer_Tick(object sender, EventArgs e)
    {

   //stupid function
        //very buggy.
      if (this.combatState == 1)
        this.restTimer.Enabled = false;
      if (this.hpPercent < this.config.RestMax)
      {
        this.avoiding = true;
        this.target = this.safeZonesList[this.nearSafeZoneIndex];
        this.status = "Resting...";
        this.ySeekTimer.Enabled = false;
        if ((double) this.getDistance(this.safeZonesList[this.nearSafeZoneIndex], this.player) >= 100.0 || this.usedRestSkill)
          return;
        InputSimulator.SimulateKeyPress(VirtualKeyCode.OEM_PLUS);
        this.usedRestSkill = true;
      }
      else
      {
        this.avoiding = false;
        this.restTimer.Enabled = false;
        this.usedRestSkill = false;
      }
        
    }

    private void autoHealTimer_Tick(object sender, EventArgs e)
    {
      InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_8);
      this.autoHealTimer.Enabled = false;
    }

    private void retreatTimer_Tick(object sender, EventArgs e)
    {
      InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_9);
      this.retreatTimer.Enabled = false;
    }

    private void ySeekCloseRange()
    {
      if ((double) this.camVerRot < (double) this.camToTargetAngle.pitchY - 2.0)
      {
        if ((double) this.camToTargetAngle.pitchY < 0.0)
          this.mouseMove(0, -5);
        else
          this.mouseMove(0, 5);
      }
      else
      {
        if ((double) this.camVerRot <= (double) this.camToTargetAngle.pitchY + 2.0)
          return;
        if ((double) this.camToTargetAngle.pitchY < 0.0)
          this.mouseMove(0, 5);
        else
          this.mouseMove(0, -5);
      }
    }

    private void ySeekTimer_Tick(object sender, EventArgs e)
    {
      if (!this.control || this.mobKillDelay.delayed || this.attackDelay.delayed)
        return;
      if (this.ySeekTick < 30)
      {
        this.mouseMove(0, 14);
        ++this.ySeekTick;
      }
      else if (this.ySeekTick < 60)
      {
        this.mouseMove(0, -14);
        ++this.ySeekTick;
      }
      if (this.ySeekTick != 60)
        return;
      this.ySeekTimer.Enabled = false;
      this.ySeekTick = 0;
    }

    private void parseObjList(int runCount)
    {
      if (this.restTimer.Enabled)
        return;
      for (int index = 0; index < runCount; ++index)
      {
        this.focus = this.Mem.ReadFloat(this.NearObjListP + this.parseOffset);
        if ((double) this.focus < (double) this.player.z + 5000.0 && (double) this.focus > (double) this.player.z - 5000.0)
        {
          ++this.parseHits;
          this.ol.Add(new Entity());
          this.ol[this.parseHits].z = this.focus;
          this.parseOffset += 4;
          this.focus = this.Mem.ReadFloat(this.NearObjListP + this.parseOffset);
          if ((double) this.focus < (double) this.player.x + 5000.0 && (double) this.focus > (double) this.player.x - 5000.0)
          {
            this.ol[this.parseHits].x = this.focus;
            this.parseOffset += 4;
            this.focus = this.Mem.ReadFloat(this.NearObjListP + this.parseOffset);
            if ((double) this.focus < (double) this.player.y + 5000.0 && (double) this.focus > (double) this.player.y - 5000.0)
            {
              this.ol[this.parseHits].y = this.focus;
                // gotta find your own! :D
              if (this.config.client == "US")
                this.ol[this.parseHits].name = this.Mem.ReadStringUnicode(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, this.parseOffset + 0, 0), 0);
              else if (this.config.client == "EU")
                this.ol[this.parseHits].name = this.Mem.ReadStringUnicode(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, this.parseOffset + 0, 0), 0);
              this.parseOffset += 4;
            }
          }
        }
        else
          this.parseOffset += 4;
      }
      this.parseOffset = 0;
      this.parseHits = -1;
    }

    private void getTarget()
    {
      if (this.parseDelay.delayed || this.avoiding)
        return;
      this.parseObjList(800);
      this.objList.Clear();
      foreach (Entity to in this.ol)
      {
        try
        {
          if (to.name != null)
          {
            if (to.name.Contains("BONF")) // bonfie :3
              to.type = -1;
            if ((double) Math.Abs(to.y) - (double) Math.Abs(this.player.y) > 500.0)
              to.type = -1;
            for (int index = 0; index < this.gatherBlackList.Count; ++index)
            {
              if (to.name.Contains(this.gatherBlackList[index].Substring(0, 5)))
                to.type = -1;
            }
            if (this.config.dontGather)
            {
              for (int index = 0; index < this.gatherWhiteList.Count; ++index)
              {
                if (to.name.Contains(this.gatherWhiteList[index].Substring(0, 5)))
                  to.type = -1;
              }
            }
            else
            {
              for (int index = 0; index < this.gatherWhiteList.Count; ++index)
              {
                if (to.name.Contains(this.gatherWhiteList[index].Substring(0, 5)))
                  to.type = 1;
              }
            }
            if (!this.config.dontFilterMobs)
            {
              for (int index = 0; index < this.mobFilterList.Count; ++index)
              {
                if (this.mobFilterList[index].Length < 5)
                {
                  if (this.config.mobFilterMode == "exclude" && to.name.Contains(this.mobFilterList[index]))
                    to.type = -1;
                  else if (to.type != 1 && this.config.mobFilterMode == "include" && to.name.Contains(this.mobFilterList[index]))
                    to.hp = 1;
                  else if (to.hp != 1 && to.type != 1 && (this.config.mobFilterMode == "include" && !to.name.Contains(this.mobFilterList[index])))
                    to.type = -1;
                }
                else if (this.config.mobFilterMode == "exclude" && to.name.Contains(this.mobFilterList[index].Substring(0, 5)))
                  to.type = -1;
                else if (to.type != 1 && this.config.mobFilterMode == "include" && to.name.Contains(this.mobFilterList[index].Substring(0, 5)))
                  to.hp = 1;
                else if (to.hp != 1 && to.type != 1 && (this.config.mobFilterMode == "include" && !to.name.Contains(this.mobFilterList[index].Substring(0, 5))))
                  to.type = -1;
              }
            }
          }
          if ((double) this.getDistance(to, this.player) < 2000.0 && to.name.Length == 0)
            to.type = -1;
          if (this.roamNodesList.Count > 0 && (double) this.getDistance(to, this.roamNodesList[this.nearRoamNode]) > (double) this.config.roamDist)
            to.type = -1;
          if (this.deadZonesList.Count > 0)
          {
            if ((double) this.getDistance(to, this.deadZonesList[this.nearDeadZoneIndex]) < (double) this.config.avoidNodeDist)
              to.type = -1;
          }
        }
        catch (NullReferenceException ex)
        {
          break;
        }
      }
      foreach (Entity entity in this.ol)
      {
        try
        {
          if (entity.type == -1 && entity.hp == 1)
            this.objList.Add(entity);
          else if (entity.type != -1)
            this.objList.Add(entity);
        }
        catch (NullReferenceException ex)
        {
          break;
        }
      }
      this.targetIndex = this.findNearestObjIndex(this.objList.ToArray(), this.player);
      if (!this.avoiding && this.targetIndex != -1)
        this.target = this.objList[this.targetIndex];
      this.ol.Clear();
      this.focus = 0.0f;
    }

    private void getCommands()
    {
      if (!this.inRange || this.target.type != 0 || (!this.control || !(this.status != "picking up")) || (this.restTimer.Enabled || this.mobKillDelay.delayed))
        return;
      if (this.lockedOn == 1)
        this.mobKillDelay.duration = 0;
      if (this.combatStarted && this.playerXp != this.oldXp && this.combatState == 0)
      {
        this.combatStarted = false;
        this.commandIndex = 0;
        if ((double) this.config.attackDist > 10.0)
        {
          this.oldAttDist = this.config.attackDist;
          this.rangerLootTimer.Enabled = true;
        }
        this.mobKillDelay.delay(10);
      }
      if (!this.combatStarted)
        this.oldXp = this.playerXp;
      if (this.config.classType == "ranged" && this.combatState == 0 && this.lockedOn == 1)
        this.combatState = 1;
      else if (this.config.classType == "melee" && this.combatState == 0)
        this.combatState = 1;
      if (this.combatState != 1 || this.commandList.Count <= 0)
        return;
      this.combatStarted = true;
      this.status = "Fighting " + this.target.name;
      this.attackLoopTimer.Enabled = true;
    }

    private void updateCooldowns()
    {
      this.offCdCommands.Clear();
      foreach (Command command in this.commandList)
      {
        if (command.cdTick == command.cd)
        {
          if (!this.offCdCommands.Contains(command))
            this.offCdCommands.Add(command);
        }
        else if (command.cdTick <= 0)
          command.cdTick = command.cd;
        else if (command.cdTick != command.cd)
          --command.cdTick;
      }
    }

    private void attackLoopTimer_Tick(object sender, EventArgs e)
    {
      if (this.control && !this.mobKillDelay.delayed)
      {
        if (this.commandIndex < this.offCdCommands.Count && this.player.mp >= this.offCdCommands[this.commandIndex].cost)
        {
          if (this.offCdCommands[this.commandIndex].slot.Contains("hold"))
          {
            this.unsticking = true;
            if (this.offCdCommands[this.commandIndex + 1].slot.Contains("release"))
            {
              switch (this.offCdCommands[this.commandIndex].slot)
              {
                case "f1 (hold)":
                  InputSimulator.SimulateKeyDown(VirtualKeyCode.F1);
                  break;
                case "f2 (hold)":
                  InputSimulator.SimulateKeyDown(VirtualKeyCode.F2);
                  break;
                case "f3 (hold)":
                  InputSimulator.SimulateKeyDown(VirtualKeyCode.F3);
                  break;
                case "f4 (hold)":
                  InputSimulator.SimulateKeyDown(VirtualKeyCode.F4);
                  break;
                case "f5 (hold)":
                  InputSimulator.SimulateKeyDown(VirtualKeyCode.F5);
                  break;
                case "f6 (hold)":
                  InputSimulator.SimulateKeyDown(VirtualKeyCode.F6);
                  break;
                case "1 (hold)":
                  InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_1);
                  break;
                case "2 (hold)":
                  InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_2);
                  break;
                case "3 (hold)":
                  InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_3);
                  break;
                case "4 (hold)":
                  InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_4);
                  break;
                case "5 (hold)":
                  InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_5);
                  break;
                case "6 (hold)":
                  InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_6);
                  break;
                case "L-Click (hold)":
                  MainUI.mouse_event(2U, 0, 0, 0, 0);
                  break;
                case "R-Click (hold)":
                  MainUI.mouse_event(8U, 0, 0, 0, 0);
                  break;
              }
              Thread.Sleep(TimeSpan.FromMilliseconds(1000.0));
              switch (this.offCdCommands[this.commandIndex + 1].slot)
              {
                case "f1 (release)":
                  InputSimulator.SimulateKeyUp(VirtualKeyCode.F1);
                  break;
                case "f2 (release)":
                  InputSimulator.SimulateKeyUp(VirtualKeyCode.F2);
                  break;
                case "f3 (release)":
                  InputSimulator.SimulateKeyUp(VirtualKeyCode.F3);
                  break;
                case "f4 (release)":
                  InputSimulator.SimulateKeyUp(VirtualKeyCode.F4);
                  break;
                case "f5 (release)":
                  InputSimulator.SimulateKeyUp(VirtualKeyCode.F5);
                  break;
                case "f6 (release)":
                  InputSimulator.SimulateKeyUp(VirtualKeyCode.F6);
                  break;
                case "1 (release)":
                  InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_1);
                  break;
                case "2 (release)":
                  InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_2);
                  break;
                case "3 (release)":
                  InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_3);
                  break;
                case "4 (release)":
                  InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_4);
                  break;
                case "5 (release)":
                  InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_5);
                  break;
                case "6 (release)":
                  InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_6);
                  break;
                case "L-Click (release)":
                  MainUI.mouse_event(4U, 0, 0, 0, 0);
                  break;
                case "R-Click (release)":
                  MainUI.mouse_event(16U, 0, 0, 0, 0);
                  break;
              }
              if (this.offCdCommands[this.commandIndex].cd > 0)
                --this.offCdCommands[this.commandIndex].cdTick;
              if (this.offCdCommands[this.commandIndex + 1].cd > 0)
                --this.offCdCommands[this.commandIndex + 1].cdTick;
              ++this.commandIndex;
            }
          }
          else if (this.offCdCommands[this.commandIndex].slot.Contains("press"))
          {
            this.unsticking = true;
            switch (this.offCdCommands[this.commandIndex].slot)
            {
              case "f1 (press)":
                InputSimulator.SimulateKeyPress(VirtualKeyCode.F1);
                break;
              case "f2 (press)":
                InputSimulator.SimulateKeyPress(VirtualKeyCode.F2);
                break;
              case "f3 (press)":
                InputSimulator.SimulateKeyPress(VirtualKeyCode.F3);
                break;
              case "f4 (press)":
                InputSimulator.SimulateKeyPress(VirtualKeyCode.F4);
                break;
              case "f5 (press)":
                InputSimulator.SimulateKeyPress(VirtualKeyCode.F5);
                break;
              case "f6 (press)":
                InputSimulator.SimulateKeyPress(VirtualKeyCode.F6);
                break;
              case "1 (press)":
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_1);
                break;
              case "2 (press)":
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_2);
                break;
              case "3 (press)":
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_3);
                break;
              case "4 (press)":
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_4);
                break;
              case "5 (press)":
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_5);
                break;
              case "6 (press)":
                InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_6);
                break;
              case "L-Click (press)":
                this.mouseClick(1);
                break;
              case "R-Click (press)":
                this.mouseClick(2);
                break;
            }
            if (this.offCdCommands[this.commandIndex].cd > 0)
              --this.offCdCommands[this.commandIndex].cdTick;
          }
          if (this.commandIndex < this.offCdCommands.Count)
            ++this.commandIndex;
          this.unsticking = false;
        }
        else if (this.commandIndex < this.offCdCommands.Count)
          ++this.commandIndex;
        if (this.commandIndex == this.offCdCommands.Count)
          this.commandIndex = 0;
        if (this.combatState != 0)
          return;
        this.commandIndex = 0;
        this.attackLoopTimer.Enabled = false;
        this.releaseAll();
      }
      else
      {
        this.commandIndex = 0;
        this.attackLoopTimer.Enabled = false;
        this.releaseAll();
      }
    }

    private void releaseAll()
    {
      for (int index = 0; index < this.commandList.Count; ++index)
      {
        switch (this.commandList[index].slot)
        {
          case "f1 (release)":
            InputSimulator.SimulateKeyUp(VirtualKeyCode.F1);
            break;
          case "f2 (release)":
            InputSimulator.SimulateKeyUp(VirtualKeyCode.F2);
            break;
          case "f3 (release)":
            InputSimulator.SimulateKeyUp(VirtualKeyCode.F3);
            break;
          case "f4 (release)":
            InputSimulator.SimulateKeyUp(VirtualKeyCode.F4);
            break;
          case "f5 (release)":
            InputSimulator.SimulateKeyUp(VirtualKeyCode.F5);
            break;
          case "f6 (release)":
            InputSimulator.SimulateKeyUp(VirtualKeyCode.F6);
            break;
          case "1 (release)":
            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_1);
            break;
          case "2 (release)":
            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_2);
            break;
          case "3 (release)":
            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_3);
            break;
          case "4 (release)":
            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_4);
            break;
          case "5 (release)":
            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_5);
            break;
          case "6 (release)":
            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_6);
            break;
          case "L-Click (release)":
            MainUI.mouse_event(4U, 0, 0, 0, 0);
            break;
          case "R-Click (release)":
            MainUI.mouse_event(16U, 0, 0, 0, 0);
            break;
        }
      }
    }

    private void managePath()
    {
      if (this.config.mode == 1 && this.sNodeList.Count > 0)
      {
        this.nearRoamNode = this.findNearestObjIndex(this.roamNodesList.ToArray(), this.player);
        this.nearNode = this.findNearestObjIndex(this.sNodeList.ToArray(), this.player);
        if (this.nearNode != -1 && this.targetNode == -1)
          this.targetNode = this.nearNode;
        this.distToNode = this.getDistance(this.sNodeList[this.nearNode], this.player);
      }
      else if (this.config.mode == 4 && this.rNodeList.Count > 0)
      {
        this.nearNode = this.findNearestObjIndex(this.rNodeList.ToArray(), this.player);
        if (this.nearNode != -1 && this.targetNode == -1)
          this.targetNode = this.nearNode;
        this.distToNode = this.getDistance(this.rNodeList[this.nearNode], this.player);
      }
      else
      {
        if (this.config.mode != 5 || this.vNodeList.Count <= 0)
          return;
        this.nearNode = this.findNearestObjIndex(this.vNodeList.ToArray(), this.player);
        if (this.nearNode != -1 && this.targetNode == -1)
          this.targetNode = this.nearNode;
        this.distToNode = this.getDistance(this.vNodeList[this.nearNode], this.player);
      }
    }

    private void moveToNode()
    {
      if (this.config.mode == 1 && this.sNodeList.Count > 0)
      {
        if ((double) this.distToNode < 75.0 && this.nearNode == this.targetNode)
        {
          this.sNodeAction(this.sNodeList[this.nearNode]);
        }
        else
        {
          if (this.attackDelay.delayed)
            return;
          this.onOrient(10f);
        }
      }
      else if (this.config.mode == 4 && this.rNodeList.Count > 0)
      {
        if ((double) this.distToNode < 75.0 && this.nearNode == this.targetNode)
        {
          this.rNodeAction(this.rNodeList[this.nearNode]);
        }
        else
        {
          if (this.attackDelay.delayed)
            return;
          this.onOrient(10f);
        }
      }
      else
      {
        if (this.config.mode != 5 || this.vNodeList.Count <= 0)
          return;
        if ((double) this.distToNode < 75.0 && this.nearNode == this.targetNode)
        {
          this.vNodeAction(this.vNodeList[this.nearNode]);
        }
        else
        {
          if (this.attackDelay.delayed)
            return;
          this.onOrient(10f);
        }
      }
    }

    private void sNodeAction(Entity node)
    {
      if (!this.control)
        return;
      if (this.pathDir == 1)
      {
        if (this.sNodeList.Count == 1 && node.type == 2)
        {
          if (this.config.roamDuration > 0 && this.roamNodesList.Count > 0)
            this.roamTime.Start();
          this.config.mode = 2;
        }
        else if (this.targetNode < this.sNodeList.Count - 1)
        {
          ++this.targetNode;
          if (node.type == 2)
          {
            if (this.config.roamDuration > 0 && this.roamNodesList.Count > 0)
              this.roamTime.Start();
            this.config.mode = 2;
          }
          else
          {
            if (node.type != 3 || this.collectableId <= 0)
              return;
            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
            InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_F);
            this.attackDelay.delay(500);
          }
        }
        else
        {
          if (this.targetNode <= 0 || !this.control)
            return;
          InputSimulator.SimulateKeyPress(VirtualKeyCode.NUMPAD5);
          this.pathDir = 2;
        }
      }
      else
      {
        if (this.pathDir != 2)
          return;
        if (this.targetNode > 0)
        {
          --this.targetNode;
          if (node.type == 2)
          {
            if (this.config.roamDuration > 0 && this.roamNodesList.Count > 0)
              this.roamTime.Start();
            this.config.mode = 2;
          }
          else
          {
            if (node.type != 3 || this.collectableId <= 0)
              return;
            InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
            InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_F);
            this.attackDelay.delay(500);
          }
        }
        else
        {
          if (!this.control)
            return;
          InputSimulator.SimulateKeyPress(VirtualKeyCode.NUMPAD5);
          this.pathDir = 1;
        }
      }
    }

    private void rNodeAction(Entity node)
    {
        if (this.targetNode < this.rNodeList.Count) // was : if (this.targetNode < this.rNodeList.Count - 1) <-which was making me loose entities.
        ++this.targetNode;
      else if (this.sNodeList.Count > 0)
        this.config.mode = 1;
      else
        this.config.mode = 2;
    }

    private void vNodeAction(Entity node)
    {
      if (this.pathDir == 1)
      {
        if (node.type == 2)
        {
          InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
          this.atVendor = true;
        }
        if (this.targetNode >= this.vNodeList.Count - 1)
          return;
        ++this.targetNode;
      }
      else
      {
        if (this.pathDir != 2)
          return;
        if (this.targetNode > 0)
          --this.targetNode;
        else if (this.sNodeList.Count > 0)
          this.config.mode = 1;
        else
          this.config.mode = 2;
      }
    }

    private void addPathNode(int action)
    {
      if (this.config.mode != 3)
        return;
      Entity entity = new Entity();
      entity.x = this.player.x;
      entity.y = this.player.y;
      entity.z = this.player.z;
      entity.type = action;
      entity.name = "node";
      if (this.editType == 1)
      {
        this.sNodeList.Add(entity);
        if (action != 2)
          return;
        this.roamNodesList.Add(entity);
      }
      else if (this.editType == 2)
      {
        this.rNodeList.Add(entity);
      }
      else
      {
        if (this.editType != 3)
          return;
        this.vNodeList.Add(entity);
      }
    }

    private void addDeadZone()
    {
      if (this.deadZonesList.Count > 0 && (double) this.getDistance(this.deadZonesList[this.nearDeadZoneIndex], this.player) < (double) this.config.avoidNodeDist)
        this.deadZonesList.RemoveAt(this.nearDeadZoneIndex);
      else
        this.deadZonesList.Add(new Entity()
        {
          x = this.player.x,
          y = this.player.y,
          z = this.player.z,
          name = "node"
        });
    }

    private void addSafeNode()
    {
      if (this.safeZonesList.Count > 0 && (double) this.getDistance(this.safeZonesList[this.nearSafeZoneIndex], this.player) < 200.0)
        this.safeZonesList.RemoveAt(this.nearSafeZoneIndex);
      else
        this.safeZonesList.Add(new Entity()
        {
          x = this.player.x,
          y = this.player.y,
          z = this.player.z,
          name = "node"
        });
    }

    private void deletePathNode()
    {
      if (this.editType == 1)
      {
        if (this.sNodeList.Count <= 0 || this.nearNode <= -1)
          return;
        if (this.sNodeList[this.nearNode].type == 2)
          this.roamNodesList.RemoveAt(this.nearRoamNode);
        this.sNodeList.RemoveAt(this.nearNode);
      }
      else if (this.editType == 2)
      {
        if (this.rNodeList.Count <= 0 || this.nearNode <= -1)
          return;
        this.rNodeList.RemoveAt(this.nearNode);
      }
      else
      {
        if (this.editType != 3 || this.vNodeList.Count <= 0 || this.nearNode <= -1)
          return;
        this.vNodeList.RemoveAt(this.nearNode);
      }
    }

    private void checkRoamRange()
    {
      if (this.roamNodesList.Count > 0 && (double) this.getDistance(this.roamNodesList[this.nearRoamNode], this.player) > (double) this.config.roamDist)
      {
        this.avoiding = true;
        this.target = this.roamNodesList[this.nearRoamNode];
      }
      else
        this.avoiding = false;
    }

    private void updateNodeIndexes()
    {
      if (this.safeZonesList.Count > 0)
        this.nearSafeZoneIndex = this.findNearestObjIndex(this.safeZonesList.ToArray(), this.player);
      if (this.deadZonesList.Count <= 0)
        return;
      this.nearDeadZoneIndex = this.findNearestObjIndex(this.deadZonesList.ToArray(), this.player);
    }

    private void manageAvoidance()
    {
      if (this.deadZonesList.Count <= 0 || this.roamNodesList.Count <= 0)
        return;
      if ((double) this.getDistance(this.deadZonesList[this.nearDeadZoneIndex], this.player) < (double) this.config.avoidNodeDist && (double) this.getDistance(this.roamNodesList[this.nearRoamNode], this.player) < 100.0)
      {
        this.avoiding = true;
        this.target = this.roamNodesList[this.nearRoamNode];
      }
      else
        this.avoiding = false;
    }

    private void input_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.OemMinus && this.devMode)
        this.config.mode = 5;
      if (e.KeyCode == Keys.F7)
        this.toggleRadar();
      if (e.KeyCode == Keys.F8)
        this.toggleMode();
      if (e.KeyCode == Keys.F9)
        this.toggleOrient();
      if (e.KeyCode == Keys.F10)
      {
        if (this.config.mode == 1)
          this.beginEdit();
        else if (this.config.mode == 2)
          this.beginEdit();
        else if (this.config.mode == 3)
        {
          if (this.editType == 1)
          {
            File.WriteAllText("paths\\standard paths\\" + this.config.sPathName + ".json", JsonConvert.SerializeObject((object) this.sNodeList));
            File.WriteAllText("paths\\standard paths\\safezones\\" + this.config.sPathName + " SafeZones.json", JsonConvert.SerializeObject((object) this.safeZonesList));
            File.WriteAllText("paths\\standard paths\\deadzones\\" + this.config.sPathName + " DeadZones.json", JsonConvert.SerializeObject((object) this.deadZonesList));
          }
          else if (this.editType == 2)
            File.WriteAllText("paths\\res paths\\" + this.config.rPathName + ".json", JsonConvert.SerializeObject((object) this.rNodeList));
          else if (this.editType == 3)
            File.WriteAllText("paths\\vendor paths\\" + this.config.vPathName + ".json", JsonConvert.SerializeObject((object) this.vNodeList));
          this.config.mode = 1;
          this.editType = 0;
          this.nearNode = 0;
        }
      }
      if (e.KeyCode == Keys.Home)
        this.addSafeNode();
      if (e.KeyCode == Keys.F11 && Control.ModifierKeys != Keys.Control)
        this.addPathNode(1);
      if (e.KeyCode == Keys.F11 && Control.ModifierKeys == Keys.Control)
        this.addPathNode(3);
      if (e.KeyCode == Keys.F12 && Control.ModifierKeys != Keys.Control)
        this.addPathNode(2);
      if (e.KeyCode == Keys.End)
        this.addDeadZone();
      if (e.KeyCode != Keys.Delete)
        return;
      this.deletePathNode();
    }

    private void input_KeyPress(object sender, KeyPressEventArgs e)
    {
    }

    private void mouseMove(int x, int y)
    {
      MainUI.mouse_event(1U, x, y, 0, 0);
    }

    private void mouseClick(int type)
    {
      if (type == 1)
      {
        MainUI.mouse_event(2U, 0, 0, 0, 0);
        MainUI.mouse_event(4U, 0, 0, 0, 0);
      }
      else if (type == 2)
      {
        MainUI.mouse_event(8U, 0, 0, 0, 0);
        MainUI.mouse_event(16U, 0, 0, 0, 0);
      }
      else
      {
        if (type != 3)
          return;
        MainUI.mouse_event(32U, 0, 0, 0, 0);
        MainUI.mouse_event(64U, 0, 0, 0, 0);
      }
    }

    private void setCursor(int x, int y)
    {
      Point lpPoint = new Point();
      if (!MainUI.ClientToScreen(this.teraWindowHandle, ref lpPoint))
        return;
      MainUI.SetCursorPos(lpPoint.X + x, lpPoint.Y + y);
    }

    private void scrollWheel(int amount)
    {
      MainUI.mouse_event(2048U, 0, 0, amount, 0);
    }

    private void rangerLootTimer_Tick(object sender, EventArgs e)
    {
      ++this.rangerLootTimerTick;
      if (this.rangerLootTimerTick == 1)
      {
        this.config.attackDist = 10f;
      }
      else
      {
        if (this.rangerLootTimerTick != 2)
          return;
        this.rangerLootTimerTick = 0;
        this.rangerLootTimer.Enabled = false;
        this.config.attackDist = this.oldAttDist;
      }
    }

    private void unstickTimer_Tick(object sender, EventArgs e)
    {
      if (this.control && (double) this.getDistance(this.oldPlayer, this.player) < 200.0 && this.combatState == 0)
      {
        ++this.onStuckTick;
        this.status = "Unsticking...";
        this.unsticking = true;
        if (this.onStuckTick < 200)
        {
          this.mouseMove(10, 0);
          InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_W);
          InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_D);
        }
        else if (this.onStuckTick > 200 && this.onStuckTick < 400)
        {
          this.mouseMove(-10, 0);
          InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
          InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_A);
        }
        else
        {
          if (this.onStuckTick <= 400)
            return;
          this.onStuckTick = 0;
        }
      }
      else
      {
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
        this.unstickTimer.Enabled = false;
        this.unsticking = false;
      }
    }

    private void sideStepTimer_Tick(object sender, EventArgs e)
    {
      ++this.sideStepTick;
      if (this.sideStepTick == 1)
        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_A);
      else if (this.sideStepTick == 2)
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
      else if (this.sideStepTick == 3)
      {
        this.unsticking = true;
        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_W);
      }
      else if (this.sideStepTick == 4)
      {
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
        this.unsticking = false;
      }
      else if (this.sideStepTick == 5)
        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_D);
      else if (this.sideStepTick == 6)
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_D);
      else if (this.sideStepTick == 7)
      {
        this.unsticking = true;
        InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_S);
      }
      else
      {
        if (this.sideStepTick != 8)
          return;
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_S);
        this.sideStepTick = 0;
        this.sideStepTimer.Enabled = false;
        this.unsticking = false;
      }
    }

    private void vendorTimer_Tick(object sender, EventArgs e)
    {
      if (!this.control)
        return;
      if (this.inventColumnIndex < 7)
      {
        this.setCursor(this.invent.pointArray[this.inventColumnIndex, this.inventRowIndex].X, this.invent.pointArray[this.inventColumnIndex, this.inventRowIndex].Y);
        Thread.Sleep(TimeSpan.FromMilliseconds(50.0));
        this.mouseMove(2, 0);
        Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
        this.mouseClick(2);
        ++this.inventColumnIndex;
      }
      else
      {
        if (this.inventColumnIndex != 7)
          return;
        this.setCursor(this.invent.pointArray[this.inventColumnIndex, this.inventRowIndex].X, this.invent.pointArray[this.inventColumnIndex, this.inventRowIndex].Y);
        Thread.Sleep(TimeSpan.FromMilliseconds(50.0));
        this.mouseMove(2, 0);
        Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
        this.mouseClick(2);
        Thread.Sleep(TimeSpan.FromMilliseconds(300.0));
        this.setCursor(120, 490);
        this.mouseMove(1, 0);
        Thread.Sleep(TimeSpan.FromMilliseconds(500.0));
        this.mouseClick(1);
        Thread.Sleep(TimeSpan.FromMilliseconds(750.0));
        if (this.inventRowIndex < this.config.inventTotalRows - 1)
        {
          ++this.inventRowIndex;
        }
        else
        {
          this.inventColumnIndex = 0;
          this.inventRowIndex = 0;
          this.vendorTimer.Enabled = false;
          this.finishedSelling = true;
          Thread.Sleep(TimeSpan.FromMilliseconds(500.0));
          InputSimulator.SimulateKeyPress(VirtualKeyCode.MENU);
          this.pathDir = 2;
          this.targetNode = this.vNodeList.Count - 2;
          this.config.mode = 5;
        }
        this.inventColumnIndex = 0;
      }
    }

    private void findButton_Tick(object sender, EventArgs e)
    {
      this.mouseMove(0, 2);
      this.CursorPixel = MainUI.GetPixelColor(Cursor.Position.X, Cursor.Position.Y);
      if ((int) this.CursorPixel.R > 30 && (int) this.CursorPixel.R < 40 && ((int) this.CursorPixel.G > 70 && (int) this.CursorPixel.G < 80) && ((int) this.CursorPixel.B > 55 && (int) this.CursorPixel.B < 70))
      {
        this.findButton.Enabled = false;
        this.buttonFound = true;
      }
      if (Cursor.Position.Y <= 500)
        return;
      this.setCursor(Cursor.Position.X, 50);
    }

    private void beginEdit()
    {
      InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
      this.config.mode = 3;
      if (Application.OpenForms["EditPrompt"] is EditPrompt)
        return;
      EditPrompt editPrompt = new EditPrompt();
      editPrompt.Owner = (Form) this;
      editPrompt.FormClosed += new FormClosedEventHandler(this.editPrompt_FormClosed);
      InputSimulator.SimulateKeyPress(VirtualKeyCode.MENU);
      ((Control) editPrompt).Show();
    }

    private void editPrompt_FormClosed(object sender, EventArgs e)
    {
    }

    private void checkDead()
    {
      if (this.player.hp != 0)
        return;
      this.setCursor(520, 485);
      if (this.flipDelay.delayed)
        return;
      this.flipDelay.delay(1);
      this.mouseMove(1, 0);
      this.mouseClick(1);
      this.config.mode = 4;
      this.targetNode = 0;
      this.pathDir = 1;
    }

    private void toggleMode()
    {
      if (this.config.mode == 1)
      {
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
        this.config.mode = 2;
        this.targetNode = 0;
      }
      else if (this.config.mode == 2)
      {
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
        this.config.mode = 1;
        this.targetNode = 0;
      }
      else if (this.config.mode == 3)
      {
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
        this.config.mode = 1;
        this.targetNode = 0;
        File.WriteAllText("paths\\" + this.config.sPathName + ".json", JsonConvert.SerializeObject((object) this.sNodeList));
      }
      else
      {
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
        this.config.mode = 1;
        this.targetNode = 0;
      }
    }

    private void toggleRadar()
    {
      if (this.config.radarToggle == 1)
      {
        this.config.radarToggle = 0;
        if (this.radar.Visible)
          this.radar.Hide();
        this.radarToggleButton.Text = "Radar: OFF";
      }
      else
      {
        this.config.radarToggle = 1;
        if (!this.radar.Visible)
            this.radar.Show(); //this.radar.Show((IWin32Window) this);
        this.radarToggleButton.Text = "Radar: ON";
      }
      File.WriteAllText("config\\config.json", JsonConvert.SerializeObject((object) this.config));
    }

    private void loadInitialRadar()
    {
      if (this.config.radarToggle == 0)
      {
        if (this.radar.Visible)
          this.radar.Hide();
        this.radarToggleButton.Text = "Radar: OFF";
      }
      else
      {
        if (this.config.radarToggle != 1)
          return;
        if (!this.radar.Visible)
          this.radar.Show((IWin32Window) this);
        this.radarToggleButton.Text = "Radar: ON";
      }
    }

    private void pickup()
    {
      if (this.control && this.collectableId > 0)
      {
        this.unsticking = true;
        InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
        if (!this.collectableName.Contains("<") && this.collectableName.Contains("pick"))
          InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_F);
        else if (this.collectableName.Contains("<"))
        {
          if (!this.config.dontFilterPickups)
          {
            foreach (string str in this.pickupFilterList)
            {
              if (this.config.pickupFilterMode == "include" && this.collectableName.Contains(str))
              {
                if (!this.checkInventSpace())
                {
                  InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_F);
                  this.passItem = false;
                  break;
                }
              }
              else if (this.config.pickupFilterMode == "exclude" && !this.collectableName.Contains(str))
              {
                if (!this.checkInventSpace())
                {
                  InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_F);
                  this.passItem = false;
                  break;
                }
              }
              else if (!this.fullInvent)
                this.passItem = true;
            }
            if (this.passItem && !this.sideStepTimer.Enabled)
              this.sideStepTimer.Enabled = true;
          }
          else if (this.config.dontFilterPickups && !this.checkInventSpace())
            InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_F);
        }
        this.unsticking = false;
        if (this.fullInvent)
          return;
        this.pickupTimer.Enabled = false;
        this.config.mode = 2;
      }
      else
        this.config.mode = 2;
    }

    private bool checkInventSpace()
    {
      if (this.collectableId <= 0)
        return this.fullInvent;
      this.unsticking = true;
      InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I);
      Thread.Sleep(TimeSpan.FromMilliseconds(1000.0));
      this.status = "Checking inventory space...";
      InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_I);
      Thread.Sleep(TimeSpan.FromMilliseconds(500.0));
      if (this.inventItemCount == this.config.inventTotalRows * 8)
      {
        this.fullInvent = true;
        this.config.mode = 5;
        this.targetNode = -1;
        this.unsticking = false;
      }
      else
        this.fullInvent = false;
      return this.fullInvent;
    }

    private void manageZoom()
    {
      if ((double) this.getDistance(this.player, this.camera) >= (double) this.zoomDist || !this.control)
        return;
      this.scrollWheel(-200);
    }

    private void checkForStuck()
    {
      ++this.checkStuckTick;
      if (this.checkStuckTick == 1)
      {
        this.oldPlayer.x = this.player.x;
        this.oldPlayer.z = this.player.z;
        this.oldPlayer.y = this.player.y;
      }
      if (this.checkStuckTick != 3)
        return;
      if (this.control && (double) this.getDistance(this.oldPlayer, this.player) < 5.0 && (this.combatState == 0 && this.statusL.Text.Contains("Mo")))
      {
        this.unstickTimer.Enabled = true;
        InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
      }
      this.checkStuckTick = 0;
    }

    public static Color GetPixelColor(int x, int y)
    {
      IntPtr dc = MainUI.GetDC(IntPtr.Zero);
      uint pixel = MainUI.GetPixel(dc, x, y);
      MainUI.ReleaseDC(IntPtr.Zero, dc);
      return Color.FromArgb((int) pixel & (int) byte.MaxValue, ((int) pixel & 65280) >> 8, ((int) pixel & 16711680) >> 16);
    }
      
   
    private void readMemory() 
    {
        // mem = TERA.exe
      if (this.config.client == "US")
      {
          this.combatState = this.Mem.ReadInt(this.mainModule, 0);
          this.currentMoney = (Decimal)this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
          this.player.x = this.Mem.ReadFloat(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));

          this.player.y = this.Mem.ReadFloat(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
          this.player.z = this.Mem.ReadFloat(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
          this.player.type = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0, 0));
          this.camera.x = this.Mem.ReadFloat(this.mainModule, 0);
          this.camera.y = this.Mem.ReadFloat(this.mainModule, 0);
          this.camera.z = this.Mem.ReadFloat(this.mainModule, 0);
          this.camVerRot = this.radToDeg(this.Mem.ReadFloat(this.mainModule, 0));
          this.playerLevel = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
          this.playerXp = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
          this.player.hp = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
          this.maxHp = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
          if (this.player.hp > 0)
              this.hpPercent = 100 * this.player.hp / this.maxHp;
          this.player.mp = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
          this.NearObjListP = this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0);
          this.collectableId = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0));
          this.lockedOn = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0, 0));
          this.inventItemCount = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0));
          this.collectableName = this.Mem.ReadStringUnicode(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0, 0), 0);
          /*
       Gotta find your own offsets and pointers! :D
           */
      }
      else
      {
        if (!(this.config.client == "EU"))
          return;
        this.combatState = this.Mem.ReadInt(this.mainModule, 0);
        this.currentMoney = (Decimal)this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
        this.player.x = this.Mem.ReadFloat(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));

        this.player.y = this.Mem.ReadFloat(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
        this.player.z = this.Mem.ReadFloat(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
        this.player.type = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0, 0));
        this.camera.x = this.Mem.ReadFloat(this.mainModule, 0);
        this.camera.y = this.Mem.ReadFloat(this.mainModule, 0);
        this.camera.z = this.Mem.ReadFloat(this.mainModule, 0);
        this.camVerRot = this.radToDeg(this.Mem.ReadFloat(this.mainModule, 0));
        this.playerLevel = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
        this.playerXp = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
        this.player.hp = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
        this.maxHp = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
        if (this.player.hp > 0)
            this.hpPercent = 100 * this.player.hp / this.maxHp;
        this.player.mp = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0));
        this.NearObjListP = this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0);
        this.collectableId = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0));
        this.lockedOn = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0, 0));
        this.inventItemCount = this.Mem.ReadInt(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0));
        this.collectableName = this.Mem.ReadStringUnicode(this.Mem.Pointer(this.mainModule, 0, 0, 0, 0, 0, 0), 0);
          /*
       Gotta find your own offsets and pointers! :D
           */
      }
    }

    private void checkDirectories()
    {
      if (!Directory.Exists("paths\\standard paths\\"))
        Directory.CreateDirectory("paths\\standard paths\\");
      if (!Directory.Exists("paths\\standard paths\\deadzones"))
        Directory.CreateDirectory("paths\\standard paths\\deadzones");
      if (!Directory.Exists("paths\\standard paths\\safezones"))
        Directory.CreateDirectory("paths\\standard paths\\safezones");
      if (!Directory.Exists("paths\\res paths\\"))
        Directory.CreateDirectory("paths\\res paths\\");
      if (!Directory.Exists("paths\\vendor paths\\"))
        Directory.CreateDirectory("paths\\vendor paths\\");
      if (!Directory.Exists("profiles"))
        Directory.CreateDirectory("profiles");
      if (!Directory.Exists("config"))
        Directory.CreateDirectory("config");
      if (!File.Exists("config\\config.json"))
        File.WriteAllText("config\\config.json", JsonConvert.SerializeObject((object) new Config()
        {
          client = "US",
          version = this.version,
          classType = "ranged",
          mobFilterMode = "exclude",
          pickupFilterMode = "include",
          mode = 1,
          radarToggle = 0, // RADAR TOGGLE -- was originally 1 
          roamDuration = 0,
          roamDist = 2500f,
          attackDist = 300f,
          avoidNodeDist = 1000f,
          kiteDistance = 250f,
          kiteMode = 1,
          useAutoRest = true,
          RestMin = 50,
          RestMax = 90,
          RetreatDist = 100f,
          HealMin = 60,
          inventStartRow = 1,
          inventStartColumn = 1,
          inventTotalRows = 5,
          useMount = true,
          useHealSkill = false,
          useRetreatSkill = false,
          useAutoKite = true,
          dontFilterMobs = true,
          dontFilterPickups = true,
          dontGather = false
        }));
      if (!File.Exists("config\\gatherWhiteList.json"))
        File.WriteAllText("config\\gatherWhiteList.json", JsonConvert.SerializeObject((object) this.strings.nodeStrings));
      if (!File.Exists("config\\pickupList.json"))
        File.WriteAllText("config\\pickupList.json", JsonConvert.SerializeObject((object) this.strings.pickupStrings));
      if (!File.Exists("config\\gatherBlackList.json"))
      {
        FileStream fileStream = File.Create("config\\gatherBlackList.json");
        fileStream.Close();
        fileStream.Dispose();
      }
      if (File.Exists("config\\mobExcludeList.json"))
        return;
      FileStream fileStream1 = File.Create("config\\mobExcludeList.json");
      fileStream1.Close();
      fileStream1.Dispose();
    }

    public void loadFiles()
    {
      this.config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config\\config.json"));
      if (this.config.mode != 2)
        this.config.mode = 1;
      this.inventRowIndex = this.config.inventStartRow - 1;
      this.inventColumnIndex = this.config.inventStartColumn - 1;
      this.oldAttDist = this.config.attackDist;
      this.Text = "tbp v" + this.version + " " + this.config.client;
      if (File.Exists("paths\\standard paths\\" + this.config.sPathName + ".json") && JsonConvert.DeserializeObject<List<Entity>>(File.ReadAllText("paths\\standard paths\\" + this.config.sPathName + ".json")) != null)
      {
        this.sNodeList = JsonConvert.DeserializeObject<List<Entity>>(File.ReadAllText("paths\\standard paths\\" + this.config.sPathName + ".json"));
        foreach (Entity entity in this.sNodeList)
        {
          if (entity.type == 2)
            this.roamNodesList.Add(entity);
        }
      }
      if (File.Exists("paths\\standard paths\\deadzones\\" + this.config.sPathName + " DeadZones.json") && JsonConvert.DeserializeObject<List<Entity>>(File.ReadAllText("paths\\standard paths\\deadzones\\" + this.config.sPathName + " DeadZones.json")) != null)
        this.deadZonesList = JsonConvert.DeserializeObject<List<Entity>>(File.ReadAllText("paths\\standard paths\\deadzones\\" + this.config.sPathName + " DeadZones.json"));
      if (File.Exists("paths\\standard paths\\safezones\\" + this.config.sPathName + " SafeZones.json") && JsonConvert.DeserializeObject<List<Entity>>(File.ReadAllText("paths\\standard paths\\safezones\\" + this.config.sPathName + " SafeZones.json")) != null)
        this.safeZonesList = JsonConvert.DeserializeObject<List<Entity>>(File.ReadAllText("paths\\standard paths\\safezones\\" + this.config.sPathName + " SafeZones.json"));
      if (File.Exists("paths\\res paths\\" + this.config.rPathName + ".json") && JsonConvert.DeserializeObject<List<Entity>>(File.ReadAllText("paths\\res paths\\" + this.config.rPathName + ".json")) != null)
        this.rNodeList = JsonConvert.DeserializeObject<List<Entity>>(File.ReadAllText("paths\\res paths\\" + this.config.rPathName + ".json"));
      if (File.Exists("paths\\vendor paths\\" + this.config.vPathName + ".json") && JsonConvert.DeserializeObject<List<Entity>>(File.ReadAllText("paths\\vendor paths\\" + this.config.vPathName + ".json")) != null)
        this.vNodeList = JsonConvert.DeserializeObject<List<Entity>>(File.ReadAllText("paths\\vendor paths\\" + this.config.vPathName + ".json"));
      if (File.Exists("profiles\\" + this.config.profileName + ".json") && JsonConvert.DeserializeObject<List<Command>>(File.ReadAllText("profiles\\" + this.config.profileName + ".json")) != null)
        this.commandList = JsonConvert.DeserializeObject<List<Command>>(File.ReadAllText("profiles\\" + this.config.profileName + ".json"));
      if (File.Exists("config\\gatherWhiteList.json") && JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\gatherWhiteList.json")) != null)
        this.gatherWhiteList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\gatherWhiteList.json"));
      if (File.Exists("config\\mobExcludeList.json") && JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\mobExcludeList.json")) != null)
        this.mobFilterList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\mobExcludeList.json"));
      if (File.Exists("config\\pickupList.json") && JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\pickupList.json")) != null)
        this.pickupFilterList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\pickupList.json"));
      if (!File.Exists("config\\gatherBlackList.json") || JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\gatherBlackList.json")) == null)
        return;
      this.gatherBlackList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("config\\gatherBlackList.json"));
    }


// decided to make them all private static extern ___  cuz of wow ref.. sauce example

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    private void pathCombatTimer_Tick(object sender, EventArgs e)
    {
      if (this.combatState == 1)
        this.config.mode = 2;
      else
        this.config.mode = this.pathCombatTimerCaller;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this.loadLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.controlL = new System.Windows.Forms.Label();
            this.restL = new System.Windows.Forms.Label();
            this.kiteL = new System.Windows.Forms.Label();
            this.useMountL = new System.Windows.Forms.Label();
            this.classL = new System.Windows.Forms.Label();
            this.roamTimerL = new System.Windows.Forms.Label();
            this.goldGainL = new System.Windows.Forms.Label();
            this.runtimeL = new System.Windows.Forms.Label();
            this.levelGainL = new System.Windows.Forms.Label();
            this.pathL = new System.Windows.Forms.Label();
            this.profileL = new System.Windows.Forms.Label();
            this.statusL = new System.Windows.Forms.Label();
            this.modeToggleButton = new System.Windows.Forms.Button();
            this.profileButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.radarToggleButton = new System.Windows.Forms.Button();
            this.oneSecTimer = new System.Windows.Forms.Timer(this.components);
            this.mainTimer = new System.Windows.Forms.Timer(this.components);
            this.startButton = new System.Windows.Forms.Button();
            this.input = new MouseKeyboardActivityMonitor.Controls.MouseKeyEventProvider();
            this.pathButton = new System.Windows.Forms.Button();
            this.fileLoadTimer = new System.Windows.Forms.Timer(this.components);
            this.ySeekTimer = new System.Windows.Forms.Timer(this.components);
            this.unstickTimer = new System.Windows.Forms.Timer(this.components);
            this.pickupTimer = new System.Windows.Forms.Timer(this.components);
            this.retreatTimer = new System.Windows.Forms.Timer(this.components);
            this.autoHealTimer = new System.Windows.Forms.Timer(this.components);
            this.rangerLootTimer = new System.Windows.Forms.Timer(this.components);
            this.restTimer = new System.Windows.Forms.Timer(this.components);
            this.sideStepTimer = new System.Windows.Forms.Timer(this.components);
            this.vendorTimer = new System.Windows.Forms.Timer(this.components);
            this.attackLoopTimer = new System.Windows.Forms.Timer(this.components);
            this.findButton = new System.Windows.Forms.Timer(this.components);
            this.oneMsTimer = new System.Windows.Forms.Timer(this.components);
            this.pathCombatTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // loadLabel
            // 
            this.loadLabel.AutoSize = true;
            this.loadLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.loadLabel.Location = new System.Drawing.Point(12, 219);
            this.loadLabel.Name = "loadLabel";
            this.loadLabel.Size = new System.Drawing.Size(91, 13);
            this.loadLabel.TabIndex = 0;
            this.loadLabel.Text = "Welcome to tbp...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(98, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Bot:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(98, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Rest:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(98, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Mount:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(98, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Kite: ";
            // 
            // controlL
            // 
            this.controlL.AutoSize = true;
            this.controlL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.controlL.Location = new System.Drawing.Point(141, 160);
            this.controlL.Name = "controlL";
            this.controlL.Size = new System.Drawing.Size(16, 13);
            this.controlL.TabIndex = 16;
            this.controlL.Text = "---";
            // 
            // restL
            // 
            this.restL.AutoSize = true;
            this.restL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.restL.Location = new System.Drawing.Point(141, 134);
            this.restL.Name = "restL";
            this.restL.Size = new System.Drawing.Size(16, 13);
            this.restL.TabIndex = 15;
            this.restL.Text = "---";
            // 
            // kiteL
            // 
            this.kiteL.AutoSize = true;
            this.kiteL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.kiteL.Location = new System.Drawing.Point(141, 121);
            this.kiteL.Name = "kiteL";
            this.kiteL.Size = new System.Drawing.Size(16, 13);
            this.kiteL.TabIndex = 12;
            this.kiteL.Text = "---";
            // 
            // useMountL
            // 
            this.useMountL.AutoSize = true;
            this.useMountL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.useMountL.Location = new System.Drawing.Point(141, 147);
            this.useMountL.Name = "useMountL";
            this.useMountL.Size = new System.Drawing.Size(16, 13);
            this.useMountL.TabIndex = 11;
            this.useMountL.Text = "---";
            // 
            // classL
            // 
            this.classL.AutoSize = true;
            this.classL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.classL.Location = new System.Drawing.Point(98, 38);
            this.classL.Name = "classL";
            this.classL.Size = new System.Drawing.Size(38, 13);
            this.classL.TabIndex = 10;
            this.classL.Text = "Class: ";
            // 
            // roamTimerL
            // 
            this.roamTimerL.AutoSize = true;
            this.roamTimerL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.roamTimerL.Location = new System.Drawing.Point(98, 63);
            this.roamTimerL.Name = "roamTimerL";
            this.roamTimerL.Size = new System.Drawing.Size(64, 13);
            this.roamTimerL.TabIndex = 9;
            this.roamTimerL.Text = "Roam Time:";
            // 
            // goldGainL
            // 
            this.goldGainL.AutoSize = true;
            this.goldGainL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.goldGainL.Location = new System.Drawing.Point(98, 102);
            this.goldGainL.Name = "goldGainL";
            this.goldGainL.Size = new System.Drawing.Size(69, 13);
            this.goldGainL.TabIndex = 6;
            this.goldGainL.Text = "Gold Gained:";
            // 
            // runtimeL
            // 
            this.runtimeL.AutoSize = true;
            this.runtimeL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.runtimeL.Location = new System.Drawing.Point(98, 76);
            this.runtimeL.Name = "runtimeL";
            this.runtimeL.Size = new System.Drawing.Size(56, 13);
            this.runtimeL.TabIndex = 5;
            this.runtimeL.Text = "Run Time:";
            // 
            // levelGainL
            // 
            this.levelGainL.AutoSize = true;
            this.levelGainL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.levelGainL.Location = new System.Drawing.Point(98, 89);
            this.levelGainL.Name = "levelGainL";
            this.levelGainL.Size = new System.Drawing.Size(78, 13);
            this.levelGainL.TabIndex = 4;
            this.levelGainL.Text = "Levels Gained:";
            // 
            // pathL
            // 
            this.pathL.AutoSize = true;
            this.pathL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pathL.Location = new System.Drawing.Point(98, 25);
            this.pathL.Name = "pathL";
            this.pathL.Size = new System.Drawing.Size(32, 13);
            this.pathL.TabIndex = 3;
            this.pathL.Text = "Path:";
            // 
            // profileL
            // 
            this.profileL.AutoSize = true;
            this.profileL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.profileL.Location = new System.Drawing.Point(98, 12);
            this.profileL.Name = "profileL";
            this.profileL.Size = new System.Drawing.Size(39, 13);
            this.profileL.TabIndex = 2;
            this.profileL.Text = "Profile:";
            // 
            // statusL
            // 
            this.statusL.AutoSize = true;
            this.statusL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.statusL.Location = new System.Drawing.Point(98, 190);
            this.statusL.Name = "statusL";
            this.statusL.Size = new System.Drawing.Size(59, 13);
            this.statusL.TabIndex = 0;
            this.statusL.Text = "Bot Status:";
            // 
            // modeToggleButton
            // 
            this.modeToggleButton.Enabled = false;
            this.modeToggleButton.Location = new System.Drawing.Point(12, 193);
            this.modeToggleButton.Name = "modeToggleButton";
            this.modeToggleButton.Size = new System.Drawing.Size(80, 23);
            this.modeToggleButton.TabIndex = 2;
            this.modeToggleButton.TabStop = false;
            this.modeToggleButton.Text = "Mode: PATH";
            this.modeToggleButton.UseVisualStyleBackColor = true;
            this.modeToggleButton.Click += new System.EventHandler(this.modeToggleButton_Click);
            // 
            // profileButton
            // 
            this.profileButton.Enabled = false;
            this.profileButton.Location = new System.Drawing.Point(12, 41);
            this.profileButton.Name = "profileButton";
            this.profileButton.Size = new System.Drawing.Size(80, 23);
            this.profileButton.TabIndex = 3;
            this.profileButton.TabStop = false;
            this.profileButton.Text = "Profiles";
            this.profileButton.UseVisualStyleBackColor = true;
            this.profileButton.Click += new System.EventHandler(this.profileButton_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Enabled = false;
            this.settingsButton.Location = new System.Drawing.Point(12, 99);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(80, 23);
            this.settingsButton.TabIndex = 4;
            this.settingsButton.TabStop = false;
            this.settingsButton.Text = "Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // radarToggleButton
            // 
            this.radarToggleButton.Enabled = false;
            this.radarToggleButton.Location = new System.Drawing.Point(12, 164);
            this.radarToggleButton.Name = "radarToggleButton";
            this.radarToggleButton.Size = new System.Drawing.Size(80, 23);
            this.radarToggleButton.TabIndex = 5;
            this.radarToggleButton.TabStop = false;
            this.radarToggleButton.Text = "Radar: ";
            this.radarToggleButton.UseVisualStyleBackColor = true;
            this.radarToggleButton.Click += new System.EventHandler(this.radarToggleButton_Click);
            // 
            // oneSecTimer
            // 
            this.oneSecTimer.Enabled = true;
            this.oneSecTimer.Interval = 1000;
            this.oneSecTimer.Tick += new System.EventHandler(this.slowTimer_Tick);
            // 
            // mainTimer
            // 
            this.mainTimer.Interval = 10;
            this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(12, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 23);
            this.startButton.TabIndex = 8;
            this.startButton.TabStop = false;
            this.startButton.Text = "Hook";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // input
            // 
            this.input.Enabled = false;
            this.input.HookType = MouseKeyboardActivityMonitor.Controls.HookType.Global;
            this.input.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.input_KeyPress);
            this.input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.input_KeyDown);
            // 
            // pathButton
            // 
            this.pathButton.Enabled = false;
            this.pathButton.Location = new System.Drawing.Point(12, 70);
            this.pathButton.Name = "pathButton";
            this.pathButton.Size = new System.Drawing.Size(80, 23);
            this.pathButton.TabIndex = 9;
            this.pathButton.TabStop = false;
            this.pathButton.Text = "Paths";
            this.pathButton.UseVisualStyleBackColor = true;
            this.pathButton.Click += new System.EventHandler(this.pathButton_Click);
            // 
            // fileLoadTimer
            // 
            this.fileLoadTimer.Interval = 1000;
            this.fileLoadTimer.Tick += new System.EventHandler(this.fileLoadTimer_Tick);
            // 
            // ySeekTimer
            // 
            this.ySeekTimer.Interval = 10;
            this.ySeekTimer.Tick += new System.EventHandler(this.ySeekTimer_Tick);
            // 
            // unstickTimer
            // 
            this.unstickTimer.Interval = 10;
            this.unstickTimer.Tick += new System.EventHandler(this.unstickTimer_Tick);
            // 
            // pickupTimer
            // 
            this.pickupTimer.Enabled = true;
            this.pickupTimer.Interval = 1800;
            this.pickupTimer.Tick += new System.EventHandler(this.pickupTimer_Tick);
            // 
            // retreatTimer
            // 
            this.retreatTimer.Interval = 1000;
            this.retreatTimer.Tick += new System.EventHandler(this.retreatTimer_Tick);
            // 
            // autoHealTimer
            // 
            this.autoHealTimer.Interval = 1000;
            this.autoHealTimer.Tick += new System.EventHandler(this.autoHealTimer_Tick);
            // 
            // rangerLootTimer
            // 
            this.rangerLootTimer.Interval = 2500;
            this.rangerLootTimer.Tick += new System.EventHandler(this.rangerLootTimer_Tick);
            // 
            // restTimer
            // 
            this.restTimer.Interval = 8;
            this.restTimer.Tick += new System.EventHandler(this.restTimer_Tick);
            // 
            // sideStepTimer
            // 
            this.sideStepTimer.Interval = 550;
            this.sideStepTimer.Tick += new System.EventHandler(this.sideStepTimer_Tick);
            // 
            // vendorTimer
            // 
            this.vendorTimer.Interval = 800;
            this.vendorTimer.Tick += new System.EventHandler(this.vendorTimer_Tick);
            // 
            // attackLoopTimer
            // 
            this.attackLoopTimer.Interval = 250;
            this.attackLoopTimer.Tick += new System.EventHandler(this.attackLoopTimer_Tick);
            // 
            // findButton
            // 
            this.findButton.Interval = 30;
            this.findButton.Tick += new System.EventHandler(this.findButton_Tick);
            // 
            // oneMsTimer
            // 
            this.oneMsTimer.Enabled = true;
            this.oneMsTimer.Interval = 1;
            this.oneMsTimer.Tick += new System.EventHandler(this.oneMsTimer_Tick);
            // 
            // pathCombatTimer
            // 
            this.pathCombatTimer.Tick += new System.EventHandler(this.pathCombatTimer_Tick);
            // 
            // MainUI
            // 
            this.ClientSize = new System.Drawing.Size(354, 238);
            this.Controls.Add(this.loadLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pathButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.controlL);
            this.Controls.Add(this.radarToggleButton);
            this.Controls.Add(this.restL);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.kiteL);
            this.Controls.Add(this.profileButton);
            this.Controls.Add(this.useMountL);
            this.Controls.Add(this.modeToggleButton);
            this.Controls.Add(this.classL);
            this.Controls.Add(this.roamTimerL);
            this.Controls.Add(this.statusL);
            this.Controls.Add(this.goldGainL);
            this.Controls.Add(this.profileL);
            this.Controls.Add(this.runtimeL);
            this.Controls.Add(this.pathL);
            this.Controls.Add(this.levelGainL);
            this.Controls.Add(this.label4);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    [System.Flags]
    public enum MouseEventFlags
    {
      LEFTDOWN = 2,
      LEFTUP = 4,
      MIDDLEDOWN = 32,
      MIDDLEUP = 64,
      MOUSEEVENTF_WHEEL = 2048,
      MOVE = 1,
      ABSOLUTE = 32768,
      RIGHTDOWN = 8,
      RIGHTUP = 16,
    }
  }
}
