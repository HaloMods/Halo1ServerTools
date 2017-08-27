using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using GameStat;

namespace Hive.Node
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmNode : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmNode()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

    public delegate bool Action (params object[] args);

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      // 
      // frmNode
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(272, 190);
      this.Name = "frmNode";
      this.Text = "Node";
      this.Load += new System.EventHandler(this.frmNode_Load);
      this.Closed += new System.EventHandler(this.frmNode_Closed);

    }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmNode());
		}

    Node node;
    private void frmNode_Load(object sender, System.EventArgs e)
    {
      //this.node = new Node();
      DateTime start = DateTime.Now;
      bool output = true;
      for (int x=0; x<100; x++)
      {
        if (x<0) output = false;
        DoTheQuery( "70.84.125.231", 2302, output);
        DoTheQuery( "217.40.41.203", 2302, output);
        DoTheQuery( "67.19.96.12", 2302, output);
        DoTheQuery( "70.85.172.166", 2302, output);
        DoTheQuery( "68.47.143.109", 2302, output);
        DoTheQuery( "67.19.96.14", 2302, output);
      }
      string s = String.Format("{0} total seconds.", DateTime.Now.Subtract(start).TotalSeconds);
      //string s = String.Format("{0}: {1} - {2} / {3}", DateTime.Now, info.Map, info.NumPlayers, info.MaxPlayers);
      MessageBox.Show(s);
    }

    public class GameServerQueryer
    {
      public GameServerQueryer(string host, int port)
      {
        
      }
    }

    private void DoTheQuery(string host, int port, bool output)
    {
      try
      {
      ServerInfo info = ServerInfo.Query(GameType.HaloCombatEvolved, host, (ushort)port);
      if (output)
         Console.WriteLine(String.Format("{0}: {1} - {2} / {3}", DateTime.Now, info.Map, info.NumPlayers, info.MaxPlayers));
      }
      catch
      {
        Console.WriteLine("Couldn't query server: " + host);
      }
    }

    private void frmNode_Closed(object sender, System.EventArgs e)
    {
      //node.Dispose();
    }
  }
}
