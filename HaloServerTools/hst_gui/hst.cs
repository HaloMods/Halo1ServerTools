using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace HaloServerTools
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class hst_gui : System.Windows.Forms.Form
	{
		private TD.SandDock.SandDockManager sandDockManager1;
		private TD.SandDock.DockContainer leftSandDock;
		private TD.SandDock.DockContainer rightSandDock;
		private TD.SandDock.DockContainer bottomSandDock;
		private TD.SandDock.DockContainer topSandDock;
		private TD.SandDock.DockControl dockControl1;
		private TD.SandDock.DockControl dockControl3;
		private TD.SandDock.DockControl dockControl4;
		private TD.SandDock.DockControl dockControl5;
		private TD.SandBar.SandBarManager sandBarManager1;
		private TD.SandBar.ToolBarContainer leftSandBarDock;
		private TD.SandBar.ToolBarContainer rightSandBarDock;
		private TD.SandBar.ToolBarContainer bottomSandBarDock;
		private TD.SandBar.ToolBarContainer topSandBarDock;
		private TD.SandBar.MenuBar menuBar1;
		private TD.SandBar.MenuBarItem menuBarItem1;
		private TD.SandBar.MenuBarItem menuBarItem3;
		private TD.SandBar.MenuBarItem menuBarItem5;
		private TD.SandBar.MenuButtonItem mnu;
		private TD.SandBar.MenuButtonItem mnuExit;
		private TD.SandBar.MenuBarItem mnuSettings;
		private TD.SandBar.MenuButtonItem mnuRunSetupWizard;
		private TD.SandBar.MenuButtonItem menuButtonItem1;
		private TD.SandBar.MenuButtonItem menuButtonItem2;
		private TD.SandBar.MenuButtonItem menuButtonItem3;
		private TD.SandBar.MenuButtonItem menuButtonItem4;
		private TD.SandBar.MenuButtonItem mnuControlServer;
		private System.Windows.Forms.TextBox txtServerLogfile;
		private System.Windows.Forms.TextBox txtHSTLog;
		private TD.SandBar.MenuButtonItem mnuManageAdmins;
		private TD.Eyefinder.NavigationBar navigationBar1;
		private TD.Eyefinder.NavigationPane navigationPane1;
		private TD.Eyefinder.NavigationPane navigationPane2;
		private TD.Eyefinder.NavigationPane navigationPane3;
		private TD.Eyefinder.NavigationPane navigationPane4;
		private TD.Eyefinder.NavigationPane navigationPane5;
		private GlacialComponents.Controls.GlacialList glacialList1;
		private System.Windows.Forms.CheckBox checkBox1;
		private GlacialComponents.Controls.GlacialList glacialList2;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.Label label4;
		private TD.SandDock.DocumentContainer documentContainer1;
		private TD.SandDock.DockControl dockControl2;
		private TD.SandDock.DockControl dockControl6;
		private TD.SandDock.DockControl dockControl7;
		private TD.SandDock.DockControl dockControl8;
		private TD.SandDock.DockControl dockControl9;
		private TD.SandDock.DockControl dockControl10;
		private System.Windows.Forms.Label lblPlayers;
		private MyXPButton.MyXPButton btnControlServer;
		private System.Windows.Forms.Label lblGameType;
		private System.Windows.Forms.Label lblMapName;
		private TD.SandBar.MenuButtonItem menuButtonItem5;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public hst_gui()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

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
			this.sandDockManager1 = new TD.SandDock.SandDockManager();
			this.leftSandDock = new TD.SandDock.DockContainer();
			this.dockControl4 = new TD.SandDock.DockControl();
			this.lblPlayers = new System.Windows.Forms.Label();
			this.btnControlServer = new MyXPButton.MyXPButton();
			this.lblGameType = new System.Windows.Forms.Label();
			this.lblMapName = new System.Windows.Forms.Label();
			this.dockControl1 = new TD.SandDock.DockControl();
			this.navigationBar1 = new TD.Eyefinder.NavigationBar();
			this.navigationPane1 = new TD.Eyefinder.NavigationPane();
			this.navigationPane2 = new TD.Eyefinder.NavigationPane();
			this.navigationPane3 = new TD.Eyefinder.NavigationPane();
			this.navigationPane4 = new TD.Eyefinder.NavigationPane();
			this.label4 = new System.Windows.Forms.Label();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.navigationPane5 = new TD.Eyefinder.NavigationPane();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.rightSandDock = new TD.SandDock.DockContainer();
			this.bottomSandDock = new TD.SandDock.DockContainer();
			this.dockControl3 = new TD.SandDock.DockControl();
			this.txtServerLogfile = new System.Windows.Forms.TextBox();
			this.dockControl5 = new TD.SandDock.DockControl();
			this.txtHSTLog = new System.Windows.Forms.TextBox();
			this.dockControl8 = new TD.SandDock.DockControl();
			this.dockControl9 = new TD.SandDock.DockControl();
			this.topSandDock = new TD.SandDock.DockContainer();
			this.sandBarManager1 = new TD.SandBar.SandBarManager();
			this.bottomSandBarDock = new TD.SandBar.ToolBarContainer();
			this.leftSandBarDock = new TD.SandBar.ToolBarContainer();
			this.rightSandBarDock = new TD.SandBar.ToolBarContainer();
			this.topSandBarDock = new TD.SandBar.ToolBarContainer();
			this.menuBar1 = new TD.SandBar.MenuBar();
			this.menuBarItem1 = new TD.SandBar.MenuBarItem();
			this.mnu = new TD.SandBar.MenuButtonItem();
			this.mnuExit = new TD.SandBar.MenuButtonItem();
			this.mnuSettings = new TD.SandBar.MenuBarItem();
			this.mnuRunSetupWizard = new TD.SandBar.MenuButtonItem();
			this.mnuManageAdmins = new TD.SandBar.MenuButtonItem();
			this.menuBarItem3 = new TD.SandBar.MenuBarItem();
			this.menuButtonItem2 = new TD.SandBar.MenuButtonItem();
			this.menuButtonItem3 = new TD.SandBar.MenuButtonItem();
			this.menuButtonItem4 = new TD.SandBar.MenuButtonItem();
			this.mnuControlServer = new TD.SandBar.MenuButtonItem();
			this.menuBarItem5 = new TD.SandBar.MenuBarItem();
			this.menuButtonItem1 = new TD.SandBar.MenuButtonItem();
			this.documentContainer1 = new TD.SandDock.DocumentContainer();
			this.dockControl2 = new TD.SandDock.DockControl();
			this.dockControl6 = new TD.SandDock.DockControl();
			this.dockControl7 = new TD.SandDock.DockControl();
			this.dockControl10 = new TD.SandDock.DockControl();
			this.menuButtonItem5 = new TD.SandBar.MenuButtonItem();
			this.leftSandDock.SuspendLayout();
			this.dockControl4.SuspendLayout();
			this.dockControl1.SuspendLayout();
			this.navigationBar1.SuspendLayout();
			this.navigationPane4.SuspendLayout();
			this.navigationPane5.SuspendLayout();
			this.bottomSandDock.SuspendLayout();
			this.dockControl3.SuspendLayout();
			this.dockControl5.SuspendLayout();
			this.topSandBarDock.SuspendLayout();
			this.documentContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// sandDockManager1
			// 
			this.sandDockManager1.DockingManager = TD.SandDock.DockingManager.Whidbey;
			this.sandDockManager1.OwnerForm = this;
			this.sandDockManager1.Renderer = new TD.SandDock.Rendering.Office2003Renderer();
			// 
			// leftSandDock
			// 
			this.leftSandDock.AllowDrop = false;
			this.leftSandDock.Controls.Add(this.dockControl4);
			this.leftSandDock.Controls.Add(this.dockControl1);
			this.leftSandDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandDock.Guid = new System.Guid("958057af-a8f5-4422-b97d-01d3b4451a1b");
			this.leftSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400, System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
																																											 new TD.SandDock.ControlLayoutSystem(268, 286, new TD.SandDock.DockControl[] {
																																																															 this.dockControl4,
																																																															 this.dockControl1}, this.dockControl1)});
			this.leftSandDock.Location = new System.Drawing.Point(0, 24);
			this.leftSandDock.Manager = this.sandDockManager1;
			this.leftSandDock.Name = "leftSandDock";
			this.leftSandDock.Size = new System.Drawing.Size(272, 286);
			this.leftSandDock.TabIndex = 0;
			// 
			// dockControl4
			// 
			this.dockControl4.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
			this.dockControl4.Controls.Add(this.lblPlayers);
			this.dockControl4.Controls.Add(this.btnControlServer);
			this.dockControl4.Controls.Add(this.lblGameType);
			this.dockControl4.Controls.Add(this.lblMapName);
			this.dockControl4.Guid = new System.Guid("c9f7b7eb-bf25-4a41-bf7f-0036e5cbd2b6");
			this.dockControl4.Location = new System.Drawing.Point(0, 25);
			this.dockControl4.Name = "dockControl4";
			this.dockControl4.Size = new System.Drawing.Size(268, 238);
			this.dockControl4.TabIndex = 1;
			this.dockControl4.Text = "Server Overview";
			// 
			// lblPlayers
			// 
			this.lblPlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblPlayers.Location = new System.Drawing.Point(96, 61);
			this.lblPlayers.Name = "lblPlayers";
			this.lblPlayers.Size = new System.Drawing.Size(160, 16);
			this.lblPlayers.TabIndex = 17;
			this.lblPlayers.Text = "[ 2 of 16 players ]";
			this.lblPlayers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnControlServer
			// 
			this.btnControlServer.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.btnControlServer.BtnShape = MyXPButton.emunType.BtnShape.Rectangle;
			this.btnControlServer.BtnStyle = MyXPButton.emunType.XPStyle.Default;
			this.btnControlServer.Location = new System.Drawing.Point(8, 56);
			this.btnControlServer.Name = "btnControlServer";
			this.btnControlServer.TabIndex = 16;
			this.btnControlServer.Text = "Start";
			this.btnControlServer.Click += new System.EventHandler(this.myXPButton2_Click);
			// 
			// lblGameType
			// 
			this.lblGameType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblGameType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lblGameType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblGameType.ForeColor = System.Drawing.Color.SteelBlue;
			this.lblGameType.Location = new System.Drawing.Point(8, 32);
			this.lblGameType.Name = "lblGameType";
			this.lblGameType.Size = new System.Drawing.Size(251, 16);
			this.lblGameType.TabIndex = 15;
			this.lblGameType.Text = "CTF";
			this.lblGameType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblMapName
			// 
			this.lblMapName.BackColor = System.Drawing.Color.SteelBlue;
			this.lblMapName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblMapName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblMapName.ForeColor = System.Drawing.Color.White;
			this.lblMapName.Location = new System.Drawing.Point(8, 8);
			this.lblMapName.Name = "lblMapName";
			this.lblMapName.Size = new System.Drawing.Size(251, 26);
			this.lblMapName.TabIndex = 14;
			this.lblMapName.Text = "Bloodgulch";
			this.lblMapName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// dockControl1
			// 
			this.dockControl1.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
			this.dockControl1.Controls.Add(this.navigationBar1);
			this.dockControl1.Guid = new System.Guid("c396f39d-efae-4321-bf8e-28dc7443190f");
			this.dockControl1.Location = new System.Drawing.Point(0, 25);
			this.dockControl1.Name = "dockControl1";
			this.dockControl1.Size = new System.Drawing.Size(268, 238);
			this.dockControl1.TabIndex = 0;
			this.dockControl1.Text = "Controls";
			// 
			// navigationBar1
			// 
			this.navigationBar1.AddRemoveButtonsText = "&Add or Remove Buttons";
			this.navigationBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.navigationBar1.Controls.Add(this.navigationPane1);
			this.navigationBar1.Controls.Add(this.navigationPane2);
			this.navigationBar1.Controls.Add(this.navigationPane3);
			this.navigationBar1.Controls.Add(this.navigationPane4);
			this.navigationBar1.Controls.Add(this.navigationPane5);
			this.navigationBar1.Dock = System.Windows.Forms.DockStyle.None;
			this.navigationBar1.DrawActionsButton = false;
			this.navigationBar1.FewerButtonsText = "Show &Fewer Buttons";
			this.navigationBar1.HeaderFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
			this.navigationBar1.Location = new System.Drawing.Point(0, 0);
			this.navigationBar1.MoreButtonsText = "Show &More Buttons";
			this.navigationBar1.Name = "navigationBar1";
			this.navigationBar1.PaneFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.navigationBar1.PaneOptionsText = "Na&vigation Pane Options...";
			this.navigationBar1.SelectedPane = this.navigationPane4;
			this.navigationBar1.ShowPanes = 3;
			this.navigationBar1.Size = new System.Drawing.Size(271, 236);
			this.navigationBar1.TabIndex = 20;
			this.navigationBar1.Text = "Kick Idle Players";
			// 
			// navigationPane1
			// 
			this.navigationPane1.LargeImage = null;
			this.navigationPane1.Location = new System.Drawing.Point(1, 26);
			this.navigationPane1.Name = "navigationPane1";
			this.navigationPane1.Size = new System.Drawing.Size(269, 107);
			this.navigationPane1.SmallImage = null;
			this.navigationPane1.TabIndex = 0;
			this.navigationPane1.Text = "Stats Logging";
			// 
			// navigationPane2
			// 
			this.navigationPane2.LargeImage = null;
			this.navigationPane2.Location = new System.Drawing.Point(1, 26);
			this.navigationPane2.Name = "navigationPane2";
			this.navigationPane2.Size = new System.Drawing.Size(269, 107);
			this.navigationPane2.SmallImage = null;
			this.navigationPane2.TabIndex = 1;
			this.navigationPane2.Text = "IRC Bot";
			// 
			// navigationPane3
			// 
			this.navigationPane3.LargeImage = null;
			this.navigationPane3.Location = new System.Drawing.Point(1, 26);
			this.navigationPane3.Name = "navigationPane3";
			this.navigationPane3.Size = new System.Drawing.Size(269, 107);
			this.navigationPane3.SmallImage = null;
			this.navigationPane3.TabIndex = 2;
			this.navigationPane3.Text = "Post Game Announcements";
			// 
			// navigationPane4
			// 
			this.navigationPane4.Controls.Add(this.label4);
			this.navigationPane4.Controls.Add(this.checkBox4);
			this.navigationPane4.Controls.Add(this.checkBox3);
			this.navigationPane4.Controls.Add(this.checkBox2);
			this.navigationPane4.LargeImage = null;
			this.navigationPane4.Location = new System.Drawing.Point(1, 26);
			this.navigationPane4.Name = "navigationPane4";
			this.navigationPane4.Size = new System.Drawing.Size(269, 107);
			this.navigationPane4.SmallImage = null;
			this.navigationPane4.TabIndex = 3;
			this.navigationPane4.Text = "In-Game Announcements";
			// 
			// label4
			// 
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label4.Location = new System.Drawing.Point(152, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 72);
			this.label4.TabIndex = 4;
			this.label4.Text = "Check the in-game announcements that you would like to be triggered.";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// checkBox4
			// 
			this.checkBox4.Location = new System.Drawing.Point(8, 56);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(136, 24);
			this.checkBox4.TabIndex = 3;
			this.checkBox4.Text = "HST Server Message";
			// 
			// checkBox3
			// 
			this.checkBox3.Location = new System.Drawing.Point(8, 32);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(136, 24);
			this.checkBox3.TabIndex = 2;
			this.checkBox3.Text = "Killtaculars";
			// 
			// checkBox2
			// 
			this.checkBox2.Location = new System.Drawing.Point(8, 8);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(136, 24);
			this.checkBox2.TabIndex = 1;
			this.checkBox2.Text = "Killing Sprees/Riots";
			// 
			// navigationPane5
			// 
			this.navigationPane5.Controls.Add(this.checkBox1);
			this.navigationPane5.LargeImage = null;
			this.navigationPane5.Location = new System.Drawing.Point(1, 26);
			this.navigationPane5.Name = "navigationPane5";
			this.navigationPane5.Size = new System.Drawing.Size(269, 107);
			this.navigationPane5.SmallImage = null;
			this.navigationPane5.TabIndex = 4;
			this.navigationPane5.Text = "In-Game Commands";
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(8, 8);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "checkBox1";
			// 
			// rightSandDock
			// 
			this.rightSandDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandDock.Guid = new System.Guid("184719bf-aeff-4ce8-ba9b-4fe6a40e3911");
			this.rightSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.rightSandDock.Location = new System.Drawing.Point(648, 24);
			this.rightSandDock.Manager = this.sandDockManager1;
			this.rightSandDock.Name = "rightSandDock";
			this.rightSandDock.Size = new System.Drawing.Size(0, 286);
			this.rightSandDock.TabIndex = 1;
			// 
			// bottomSandDock
			// 
			this.bottomSandDock.Controls.Add(this.dockControl3);
			this.bottomSandDock.Controls.Add(this.dockControl5);
			this.bottomSandDock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomSandDock.Guid = new System.Guid("75e4c8a7-c6a5-40d5-b145-3752fa434f9b");
			this.bottomSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400, System.Windows.Forms.Orientation.Vertical, new TD.SandDock.LayoutSystemBase[] {
																																											 new TD.SandDock.ControlLayoutSystem(648, 148, new TD.SandDock.DockControl[] {
																																																															 this.dockControl3,
																																																															 this.dockControl5}, this.dockControl3)});
			this.bottomSandDock.Location = new System.Drawing.Point(0, 310);
			this.bottomSandDock.Manager = this.sandDockManager1;
			this.bottomSandDock.Name = "bottomSandDock";
			this.bottomSandDock.Size = new System.Drawing.Size(648, 152);
			this.bottomSandDock.TabIndex = 2;
			// 
			// dockControl3
			// 
			this.dockControl3.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
			this.dockControl3.Closable = false;
			this.dockControl3.Controls.Add(this.txtServerLogfile);
			this.dockControl3.Guid = new System.Guid("b49c0a80-0d3c-4e12-8a0d-6b385cafb4aa");
			this.dockControl3.Location = new System.Drawing.Point(0, 29);
			this.dockControl3.Name = "dockControl3";
			this.dockControl3.Size = new System.Drawing.Size(648, 100);
			this.dockControl3.TabIndex = 1;
			this.dockControl3.Text = "Server Logfile";
			// 
			// txtServerLogfile
			// 
			this.txtServerLogfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtServerLogfile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtServerLogfile.Location = new System.Drawing.Point(7, 7);
			this.txtServerLogfile.Multiline = true;
			this.txtServerLogfile.Name = "txtServerLogfile";
			this.txtServerLogfile.Size = new System.Drawing.Size(633, 84);
			this.txtServerLogfile.TabIndex = 0;
			this.txtServerLogfile.Text = "";
			// 
			// dockControl5
			// 
			this.dockControl5.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
			this.dockControl5.Closable = false;
			this.dockControl5.Controls.Add(this.txtHSTLog);
			this.dockControl5.Guid = new System.Guid("aa22d0d5-d252-4ce2-afa0-a16d31cf33b1");
			this.dockControl5.Location = new System.Drawing.Point(0, 29);
			this.dockControl5.Name = "dockControl5";
			this.dockControl5.Size = new System.Drawing.Size(648, 100);
			this.dockControl5.TabIndex = 2;
			this.dockControl5.Text = "HST Log";
			// 
			// txtHSTLog
			// 
			this.txtHSTLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtHSTLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtHSTLog.Location = new System.Drawing.Point(8, 8);
			this.txtHSTLog.Multiline = true;
			this.txtHSTLog.Name = "txtHSTLog";
			this.txtHSTLog.Size = new System.Drawing.Size(633, 84);
			this.txtHSTLog.TabIndex = 1;
			this.txtHSTLog.Text = "";
			// 
			// dockControl8
			// 
			this.dockControl8.Guid = new System.Guid("f9cb6326-fbae-48ce-9fd5-4f937e4ad61c");
			this.dockControl8.Location = new System.Drawing.Point(0, 29);
			this.dockControl8.Name = "dockControl8";
			this.dockControl8.Size = new System.Drawing.Size(648, 100);
			this.dockControl8.TabIndex = 3;
			this.dockControl8.Text = "dockControl8";
			// 
			// dockControl9
			// 
			this.dockControl9.Guid = new System.Guid("8d550ff6-332b-48f9-846f-18a9c7ce63b6");
			this.dockControl9.Location = new System.Drawing.Point(0, 29);
			this.dockControl9.Name = "dockControl9";
			this.dockControl9.Size = new System.Drawing.Size(648, 100);
			this.dockControl9.TabIndex = 4;
			this.dockControl9.Text = "dockControl9";
			// 
			// topSandDock
			// 
			this.topSandDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandDock.Guid = new System.Guid("8b6d68e9-b487-42ef-8ae5-e0a5efbc5e0c");
			this.topSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.topSandDock.Location = new System.Drawing.Point(0, 24);
			this.topSandDock.Manager = this.sandDockManager1;
			this.topSandDock.Name = "topSandDock";
			this.topSandDock.Size = new System.Drawing.Size(648, 0);
			this.topSandDock.TabIndex = 3;
			// 
			// sandBarManager1
			// 
			this.sandBarManager1.BottomContainer = this.bottomSandBarDock;
			this.sandBarManager1.LeftContainer = this.leftSandBarDock;
			this.sandBarManager1.OwnerForm = this;
			this.sandBarManager1.RightContainer = this.rightSandBarDock;
			this.sandBarManager1.TopContainer = this.topSandBarDock;
			// 
			// bottomSandBarDock
			// 
			this.bottomSandBarDock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomSandBarDock.Location = new System.Drawing.Point(0, 462);
			this.bottomSandBarDock.Manager = this.sandBarManager1;
			this.bottomSandBarDock.Name = "bottomSandBarDock";
			this.bottomSandBarDock.Size = new System.Drawing.Size(648, 0);
			this.bottomSandBarDock.TabIndex = 7;
			// 
			// leftSandBarDock
			// 
			this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandBarDock.Location = new System.Drawing.Point(0, 24);
			this.leftSandBarDock.Manager = this.sandBarManager1;
			this.leftSandBarDock.Name = "leftSandBarDock";
			this.leftSandBarDock.Size = new System.Drawing.Size(0, 438);
			this.leftSandBarDock.TabIndex = 5;
			// 
			// rightSandBarDock
			// 
			this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandBarDock.Location = new System.Drawing.Point(648, 24);
			this.rightSandBarDock.Manager = this.sandBarManager1;
			this.rightSandBarDock.Name = "rightSandBarDock";
			this.rightSandBarDock.Size = new System.Drawing.Size(0, 438);
			this.rightSandBarDock.TabIndex = 6;
			// 
			// topSandBarDock
			// 
			this.topSandBarDock.Controls.Add(this.menuBar1);
			this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
			this.topSandBarDock.Manager = this.sandBarManager1;
			this.topSandBarDock.Name = "topSandBarDock";
			this.topSandBarDock.Size = new System.Drawing.Size(648, 24);
			this.topSandBarDock.TabIndex = 8;
			// 
			// menuBar1
			// 
			this.menuBar1.Buttons.AddRange(new TD.SandBar.ToolbarItemBase[] {
																				this.menuBarItem1,
																				this.mnuSettings,
																				this.menuBarItem3,
																				this.menuBarItem5});
			this.menuBar1.Guid = new System.Guid("1f627ce2-2d89-4634-b966-84630420f21b");
			this.menuBar1.ImageList = null;
			this.menuBar1.Location = new System.Drawing.Point(2, 0);
			this.menuBar1.Movable = false;
			this.menuBar1.Name = "menuBar1";
			this.menuBar1.Size = new System.Drawing.Size(648, 24);
			this.menuBar1.TabIndex = 1;
			this.menuBar1.Tearable = false;
			// 
			// menuBarItem1
			// 
			this.menuBarItem1.Icon = null;
			this.menuBarItem1.MenuItems.AddRange(new TD.SandBar.MenuButtonItem[] {
																					 this.mnu,
																					 this.menuButtonItem5,
																					 this.mnuExit});
			this.menuBarItem1.Text = "&File";
			// 
			// mnu
			// 
			this.mnu.Icon = null;
			this.mnu.Shortcut = System.Windows.Forms.Shortcut.None;
			this.mnu.Text = "&Save Layout";
			// 
			// mnuExit
			// 
			this.mnuExit.Icon = null;
			this.mnuExit.Shortcut = System.Windows.Forms.Shortcut.None;
			this.mnuExit.Text = "E&xit";
			// 
			// mnuSettings
			// 
			this.mnuSettings.Icon = null;
			this.mnuSettings.MenuItems.AddRange(new TD.SandBar.MenuButtonItem[] {
																					this.mnuRunSetupWizard,
																					this.mnuManageAdmins});
			this.mnuSettings.Text = "&Settings";
			// 
			// mnuRunSetupWizard
			// 
			this.mnuRunSetupWizard.Icon = null;
			this.mnuRunSetupWizard.Shortcut = System.Windows.Forms.Shortcut.None;
			this.mnuRunSetupWizard.Text = "&Run Setup Wizard";
			// 
			// mnuManageAdmins
			// 
			this.mnuManageAdmins.Icon = null;
			this.mnuManageAdmins.Shortcut = System.Windows.Forms.Shortcut.None;
			this.mnuManageAdmins.Text = "Manage Players and Admins";
			// 
			// menuBarItem3
			// 
			this.menuBarItem3.Icon = null;
			this.menuBarItem3.MenuItems.AddRange(new TD.SandBar.MenuButtonItem[] {
																					 this.menuButtonItem2,
																					 this.menuButtonItem3,
																					 this.menuButtonItem4,
																					 this.mnuControlServer});
			this.menuBarItem3.Text = "&Controls";
			// 
			// menuButtonItem2
			// 
			this.menuButtonItem2.BeginGroup = true;
			this.menuButtonItem2.Checked = true;
			this.menuButtonItem2.Icon = null;
			this.menuButtonItem2.Shortcut = System.Windows.Forms.Shortcut.None;
			this.menuButtonItem2.Text = "&Admin Shortcuts";
			// 
			// menuButtonItem3
			// 
			this.menuButtonItem3.Checked = true;
			this.menuButtonItem3.Icon = null;
			this.menuButtonItem3.Shortcut = System.Windows.Forms.Shortcut.None;
			this.menuButtonItem3.Text = "&Player Voting";
			// 
			// menuButtonItem4
			// 
			this.menuButtonItem4.Icon = null;
			this.menuButtonItem4.Shortcut = System.Windows.Forms.Shortcut.None;
			this.menuButtonItem4.Text = "&IRC Bot";
			// 
			// mnuControlServer
			// 
			this.mnuControlServer.BeginGroup = true;
			this.mnuControlServer.Icon = null;
			this.mnuControlServer.Shortcut = System.Windows.Forms.Shortcut.None;
			this.mnuControlServer.Text = "Stop Server";
			// 
			// menuBarItem5
			// 
			this.menuBarItem5.Icon = null;
			this.menuBarItem5.MenuItems.AddRange(new TD.SandBar.MenuButtonItem[] {
																					 this.menuButtonItem1});
			this.menuBarItem5.Text = "&Help";
			// 
			// menuButtonItem1
			// 
			this.menuButtonItem1.Icon = null;
			this.menuButtonItem1.Shortcut = System.Windows.Forms.Shortcut.None;
			this.menuButtonItem1.Text = "&About";
			// 
			// documentContainer1
			// 
			this.documentContainer1.Controls.Add(this.dockControl2);
			this.documentContainer1.Controls.Add(this.dockControl6);
			this.documentContainer1.Controls.Add(this.dockControl7);
			this.documentContainer1.Controls.Add(this.dockControl10);
			this.documentContainer1.Guid = new System.Guid("beefbfc5-afd3-4129-a225-65fce615c4d6");
			this.documentContainer1.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400, System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
																																												   new TD.SandDock.DocumentLayoutSystem(374, 284, new TD.SandDock.DockControl[] {
																																																																	this.dockControl2,
																																																																	this.dockControl6,
																																																																	this.dockControl7,
																																																																	this.dockControl10}, this.dockControl10)});
			this.documentContainer1.Location = new System.Drawing.Point(272, 24);
			this.documentContainer1.Manager = null;
			this.documentContainer1.Name = "documentContainer1";
			this.documentContainer1.Size = new System.Drawing.Size(376, 286);
			this.documentContainer1.TabIndex = 9;
			// 
			// dockControl2
			// 
			this.dockControl2.Guid = new System.Guid("84b8e4b3-c142-444b-a5fb-c06a15f978be");
			this.dockControl2.Location = new System.Drawing.Point(3, 23);
			this.dockControl2.Name = "dockControl2";
			this.dockControl2.Size = new System.Drawing.Size(370, 260);
			this.dockControl2.TabIndex = 0;
			this.dockControl2.Text = "Game Status";
			// 
			// dockControl6
			// 
			this.dockControl6.Guid = new System.Guid("b0d4cccb-8fe9-47fa-83b1-93ae6b936393");
			this.dockControl6.Location = new System.Drawing.Point(3, 23);
			this.dockControl6.Name = "dockControl6";
			this.dockControl6.Size = new System.Drawing.Size(370, 260);
			this.dockControl6.TabIndex = 1;
			this.dockControl6.Text = "Chat";
			// 
			// dockControl7
			// 
			this.dockControl7.Guid = new System.Guid("7dbef1df-c4c9-4ee4-b142-8a33a40c92a4");
			this.dockControl7.Location = new System.Drawing.Point(3, 23);
			this.dockControl7.Name = "dockControl7";
			this.dockControl7.Size = new System.Drawing.Size(370, 260);
			this.dockControl7.TabIndex = 2;
			this.dockControl7.Text = "RCON";
			// 
			// dockControl10
			// 
			this.dockControl10.Guid = new System.Guid("1f6b617d-dce8-4522-8eea-c654121504a7");
			this.dockControl10.Location = new System.Drawing.Point(3, 23);
			this.dockControl10.Name = "dockControl10";
			this.dockControl10.Size = new System.Drawing.Size(370, 260);
			this.dockControl10.TabIndex = 3;
			this.dockControl10.Text = "HST Commands";
			// 
			// menuButtonItem5
			// 
			this.menuButtonItem5.Icon = null;
			this.menuButtonItem5.Shortcut = System.Windows.Forms.Shortcut.None;
			this.menuButtonItem5.Text = "Run Wizard";
			// 
			// hst_gui
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(648, 462);
			this.Controls.Add(this.documentContainer1);
			this.Controls.Add(this.leftSandDock);
			this.Controls.Add(this.rightSandDock);
			this.Controls.Add(this.bottomSandDock);
			this.Controls.Add(this.topSandDock);
			this.Controls.Add(this.leftSandBarDock);
			this.Controls.Add(this.rightSandBarDock);
			this.Controls.Add(this.bottomSandBarDock);
			this.Controls.Add(this.topSandBarDock);
			this.Name = "hst_gui";
			this.Text = "Halo Server Tools LiTE v0.3 - Public Beta 1";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.hst_gui_Close);
			this.Load += new System.EventHandler(this.hst_gui_Load);
			this.leftSandDock.ResumeLayout(false);
			this.dockControl4.ResumeLayout(false);
			this.dockControl1.ResumeLayout(false);
			this.navigationBar1.ResumeLayout(false);
			this.navigationPane4.ResumeLayout(false);
			this.navigationPane5.ResumeLayout(false);
			this.bottomSandDock.ResumeLayout(false);
			this.dockControl3.ResumeLayout(false);
			this.dockControl5.ResumeLayout(false);
			this.topSandBarDock.ResumeLayout(false);
			this.documentContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new hst_gui());
		}

		private void myXPButton2_Click(object sender, System.EventArgs e)
		{
			txtHSTLog.Text = "2004-08-20 23:25:26	Starting HALOCE server process\r\n" +
				"2004-08-20 23:25:29	Binding Server Extensions\r\n" +
				"2004-08-20 23:25:31	Logfile monitor thread started\r\n" +
				"2004-08-20 23:25:31	Initializing IRC bot\r\n" +
				"2004-08-20 23:25:32	Server Extensions active - listening for triggers.\r\n" +
				"2004-08-20 23:25:45	IRC Bot connected to irc.shadowfire.org, joined #hst_test\r\n";
			HaloServer hs = (HaloServer)ServerManager.ServerList[0];
			hs.Start();
		}

		// Once per two seconds, update any controls that need it
		//
		private void _thGUI()
		{
			while (true)
			{
				UpdateMapInfo();
				Thread.Sleep(2000);
			}
		}

		private void UpdateMapInfo()
		{
			HaloServer hs = (HaloServer)ServerManager.ServerList[0];
			if (!hs.Active())
			{
				lblMapName.Text = "";
				lblPlayers.Text = "";
			}
			else
			{
				lblMapName.Text = hs._serverExtensions._logInterpreter.gameStatus.CurrentMap;
				lblGameType.Text = hs._serverExtensions._logInterpreter.gameStatus.CurrentGameMode;
			}
		}

		private HST h;
		private Thread thGUI;
		private void hst_gui_Load(object sender, System.EventArgs e)
		{
			lblMapName.Text = "Server not running.";
			lblPlayers.Text = "n/a";
			h = new HST();
			
			// Start the GUI update thread
			thGUI = new Thread(new ThreadStart(_thGUI));
			thGUI.Start();
		}
		private void hst_gui_Close(object sender, System.ComponentModel.CancelEventArgs e)
		{
			thGUI.Abort();
			Application.Exit();
		}
	}
}
