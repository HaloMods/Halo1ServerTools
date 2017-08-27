using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace hst_gui
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
		private TD.SandDock.DocumentContainer documentContainer1;
		private TD.SandDock.DockControl dockControl2;
		private TD.SandDock.DockControl dockControl6;
		private TD.SandDock.DockControl dockControl7;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private MyXPButton.MyXPButton myXPButton1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "11:30:45",
																													 "=EP_Mono=CO",
																													 "Saying some stuff yeah text goes here."}, -1);
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "11:31:27",
																													 "GuyInServer",
																													 "I said things just now."}, -1);
			this.sandDockManager1 = new TD.SandDock.SandDockManager();
			this.leftSandDock = new TD.SandDock.DockContainer();
			this.rightSandDock = new TD.SandDock.DockContainer();
			this.bottomSandDock = new TD.SandDock.DockContainer();
			this.topSandDock = new TD.SandDock.DockContainer();
			this.dockControl1 = new TD.SandDock.DockControl();
			this.dockControl3 = new TD.SandDock.DockControl();
			this.dockControl4 = new TD.SandDock.DockControl();
			this.dockControl5 = new TD.SandDock.DockControl();
			this.documentContainer1 = new TD.SandDock.DocumentContainer();
			this.dockControl2 = new TD.SandDock.DockControl();
			this.dockControl6 = new TD.SandDock.DockControl();
			this.dockControl7 = new TD.SandDock.DockControl();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.myXPButton1 = new MyXPButton.MyXPButton();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.leftSandDock.SuspendLayout();
			this.bottomSandDock.SuspendLayout();
			this.documentContainer1.SuspendLayout();
			this.dockControl2.SuspendLayout();
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
			this.leftSandDock.Controls.Add(this.dockControl4);
			this.leftSandDock.Controls.Add(this.dockControl1);
			this.leftSandDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandDock.Guid = new System.Guid("958057af-a8f5-4422-b97d-01d3b4451a1b");
			this.leftSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400, System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
																																											 new TD.SandDock.ControlLayoutSystem(268, 358, new TD.SandDock.DockControl[] {
																																																															 this.dockControl4,
																																																															 this.dockControl1}, this.dockControl4)});
			this.leftSandDock.Location = new System.Drawing.Point(0, 0);
			this.leftSandDock.Manager = this.sandDockManager1;
			this.leftSandDock.Name = "leftSandDock";
			this.leftSandDock.Size = new System.Drawing.Size(272, 358);
			this.leftSandDock.TabIndex = 0;
			// 
			// rightSandDock
			// 
			this.rightSandDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandDock.Guid = new System.Guid("184719bf-aeff-4ce8-ba9b-4fe6a40e3911");
			this.rightSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.rightSandDock.Location = new System.Drawing.Point(712, 0);
			this.rightSandDock.Manager = this.sandDockManager1;
			this.rightSandDock.Name = "rightSandDock";
			this.rightSandDock.Size = new System.Drawing.Size(0, 358);
			this.rightSandDock.TabIndex = 1;
			// 
			// bottomSandDock
			// 
			this.bottomSandDock.Controls.Add(this.dockControl3);
			this.bottomSandDock.Controls.Add(this.dockControl5);
			this.bottomSandDock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomSandDock.Guid = new System.Guid("75e4c8a7-c6a5-40d5-b145-3752fa434f9b");
			this.bottomSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400, System.Windows.Forms.Orientation.Vertical, new TD.SandDock.LayoutSystemBase[] {
																																											 new TD.SandDock.ControlLayoutSystem(712, 148, new TD.SandDock.DockControl[] {
																																																															 this.dockControl3,
																																																															 this.dockControl5}, this.dockControl3)});
			this.bottomSandDock.Location = new System.Drawing.Point(0, 358);
			this.bottomSandDock.Manager = this.sandDockManager1;
			this.bottomSandDock.Name = "bottomSandDock";
			this.bottomSandDock.Size = new System.Drawing.Size(712, 152);
			this.bottomSandDock.TabIndex = 2;
			// 
			// topSandDock
			// 
			this.topSandDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandDock.Guid = new System.Guid("8b6d68e9-b487-42ef-8ae5-e0a5efbc5e0c");
			this.topSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.topSandDock.Location = new System.Drawing.Point(0, 0);
			this.topSandDock.Manager = this.sandDockManager1;
			this.topSandDock.Name = "topSandDock";
			this.topSandDock.Size = new System.Drawing.Size(712, 0);
			this.topSandDock.TabIndex = 3;
			// 
			// dockControl1
			// 
			this.dockControl1.Guid = new System.Guid("c396f39d-efae-4321-bf8e-28dc7443190f");
			this.dockControl1.Location = new System.Drawing.Point(0, 25);
			this.dockControl1.Name = "dockControl1";
			this.dockControl1.Size = new System.Drawing.Size(268, 310);
			this.dockControl1.TabIndex = 0;
			this.dockControl1.Text = "dockControl1";
			// 
			// dockControl3
			// 
			this.dockControl3.Guid = new System.Guid("b49c0a80-0d3c-4e12-8a0d-6b385cafb4aa");
			this.dockControl3.Location = new System.Drawing.Point(0, 29);
			this.dockControl3.Name = "dockControl3";
			this.dockControl3.Size = new System.Drawing.Size(712, 100);
			this.dockControl3.TabIndex = 1;
			this.dockControl3.Text = "dockControl3";
			// 
			// dockControl4
			// 
			this.dockControl4.Guid = new System.Guid("c9f7b7eb-bf25-4a41-bf7f-0036e5cbd2b6");
			this.dockControl4.Location = new System.Drawing.Point(0, 25);
			this.dockControl4.Name = "dockControl4";
			this.dockControl4.Size = new System.Drawing.Size(268, 310);
			this.dockControl4.TabIndex = 1;
			this.dockControl4.Text = "dockControl4";
			// 
			// dockControl5
			// 
			this.dockControl5.Guid = new System.Guid("aa22d0d5-d252-4ce2-afa0-a16d31cf33b1");
			this.dockControl5.Location = new System.Drawing.Point(0, 29);
			this.dockControl5.Name = "dockControl5";
			this.dockControl5.Size = new System.Drawing.Size(712, 100);
			this.dockControl5.TabIndex = 2;
			this.dockControl5.Text = "dockControl5";
			// 
			// documentContainer1
			// 
			this.documentContainer1.Controls.Add(this.dockControl2);
			this.documentContainer1.Controls.Add(this.dockControl6);
			this.documentContainer1.Controls.Add(this.dockControl7);
			this.documentContainer1.Guid = new System.Guid("3bca33ff-7d04-4170-ba74-cef89d512f7e");
			this.documentContainer1.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400, System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
																																												   new TD.SandDock.DocumentLayoutSystem(438, 356, new TD.SandDock.DockControl[] {
																																																																	this.dockControl2,
																																																																	this.dockControl6,
																																																																	this.dockControl7}, this.dockControl2)});
			this.documentContainer1.Location = new System.Drawing.Point(272, 0);
			this.documentContainer1.Manager = null;
			this.documentContainer1.Name = "documentContainer1";
			this.documentContainer1.Size = new System.Drawing.Size(440, 358);
			this.documentContainer1.TabIndex = 4;
			// 
			// dockControl2
			// 
			this.dockControl2.Controls.Add(this.listView1);
			this.dockControl2.Controls.Add(this.myXPButton1);
			this.dockControl2.Controls.Add(this.label1);
			this.dockControl2.Controls.Add(this.textBox1);
			this.dockControl2.Guid = new System.Guid("b7826977-2623-4692-9243-4862b08ea3c1");
			this.dockControl2.Location = new System.Drawing.Point(3, 23);
			this.dockControl2.Name = "dockControl2";
			this.dockControl2.Size = new System.Drawing.Size(434, 332);
			this.dockControl2.TabIndex = 0;
			this.dockControl2.Text = "Chat";
			// 
			// dockControl6
			// 
			this.dockControl6.Guid = new System.Guid("7ce518b5-71d2-46d5-8f78-8722a08a0a9c");
			this.dockControl6.Location = new System.Drawing.Point(3, 23);
			this.dockControl6.Name = "dockControl6";
			this.dockControl6.Size = new System.Drawing.Size(388, 260);
			this.dockControl6.TabIndex = 1;
			this.dockControl6.Text = "RCON";
			// 
			// dockControl7
			// 
			this.dockControl7.Guid = new System.Guid("dae88ed6-41de-48e3-ae97-e80dbe5fc9fb");
			this.dockControl7.Location = new System.Drawing.Point(3, 23);
			this.dockControl7.Name = "dockControl7";
			this.dockControl7.Size = new System.Drawing.Size(388, 260);
			this.dockControl7.TabIndex = 2;
			this.dockControl7.Text = "HST Commands";
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox1.Location = new System.Drawing.Point(8, 304);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(339, 20);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(8, 280);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(370, 15);
			this.label1.TabIndex = 2;
			this.label1.Text = "Enter your text to say in game, and click Send.";
			// 
			// myXPButton1
			// 
			this.myXPButton1.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.myXPButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.myXPButton1.BtnShape = MyXPButton.emunType.BtnShape.Rectangle;
			this.myXPButton1.BtnStyle = MyXPButton.emunType.XPStyle.Default;
			this.myXPButton1.Location = new System.Drawing.Point(354, 303);
			this.myXPButton1.Name = "myXPButton1";
			this.myXPButton1.Size = new System.Drawing.Size(71, 23);
			this.myXPButton1.TabIndex = 3;
			this.myXPButton1.Text = "Send";
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader1,
																						this.columnHeader2,
																						this.columnHeader3});
			this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																					  listViewItem1,
																					  listViewItem2});
			this.listView1.Location = new System.Drawing.Point(8, 8);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(417, 264);
			this.listView1.TabIndex = 4;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Time";
			this.columnHeader1.Width = 55;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Player";
			this.columnHeader2.Width = 92;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Message";
			this.columnHeader3.Width = 263;
			// 
			// hst_gui
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(712, 510);
			this.Controls.Add(this.documentContainer1);
			this.Controls.Add(this.leftSandDock);
			this.Controls.Add(this.rightSandDock);
			this.Controls.Add(this.bottomSandDock);
			this.Controls.Add(this.topSandDock);
			this.Name = "hst_gui";
			this.Text = "Halo Server Tools LiTE v0.3 - Public Beta 1";
			this.leftSandDock.ResumeLayout(false);
			this.bottomSandDock.ResumeLayout(false);
			this.documentContainer1.ResumeLayout(false);
			this.dockControl2.ResumeLayout(false);
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
	}
}
