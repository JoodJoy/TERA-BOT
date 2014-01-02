
using System;
using System.IO;
using System.Windows.Forms;

namespace tbp
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      if (File.Exists("csSecure.exe"))
        File.Delete("csSecure.exe");
      Application.Run((Form)new MainUI()); // added.
        /*
        if (new Login().ShowDialog() == DialogResult.OK)
        Application.Run((Form) new MainUI());
      else
        Application.Exit();
       */  
    }
  }
}
