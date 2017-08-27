using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Hive.Node.Core.ServerInterfaces;

namespace TestApp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : Form
	{
    private Button button1;
    private TextBox txtUsername;
    private TextBox txtPassword;
    private Label label1;
    private Label label2;
    private Button button2;
    private TextBox textBox1;
    private Label label3;
    private NumericUpDown numericUpDown1;
    private Label label4;
    private Panel panel1;
    private TextBox txtServerSay;
    private Button button3;
    private Label label5;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.Button button5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox txtPort;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.Button button6;
    private System.Windows.Forms.Button button7;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

      ts = new TeamspeakServerInterface("70.84.125.228");
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
      this.button1 = new System.Windows.Forms.Button();
      this.txtUsername = new System.Windows.Forms.TextBox();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.button2 = new System.Windows.Forms.Button();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
      this.label4 = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.button7 = new System.Windows.Forms.Button();
      this.button6 = new System.Windows.Forms.Button();
      this.button4 = new System.Windows.Forms.Button();
      this.label5 = new System.Windows.Forms.Label();
      this.button3 = new System.Windows.Forms.Button();
      this.txtServerSay = new System.Windows.Forms.TextBox();
      this.button5 = new System.Windows.Forms.Button();
      this.label6 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.txtPort = new System.Windows.Forms.TextBox();
      this.panel2 = new System.Windows.Forms.Panel();
      this.panel3 = new System.Windows.Forms.Panel();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(8, 24);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(208, 23);
      this.button1.TabIndex = 0;
      this.button1.Text = "Server Overview";
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // txtUsername
      // 
      this.txtUsername.Location = new System.Drawing.Point(8, 40);
      this.txtUsername.Name = "txtUsername";
      this.txtUsername.TabIndex = 1;
      this.txtUsername.Text = "";
      // 
      // txtPassword
      // 
      this.txtPassword.Location = new System.Drawing.Point(120, 40);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '*';
      this.txtPassword.TabIndex = 2;
      this.txtPassword.Text = "";
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(8, 24);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(100, 16);
      this.label1.TabIndex = 3;
      this.label1.Text = "Username";
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(120, 24);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(100, 16);
      this.label2.TabIndex = 4;
      this.label2.Text = "Password";
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(8, 64);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(104, 23);
      this.button2.TabIndex = 5;
      this.button2.Text = "Login";
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(8, 24);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(112, 20);
      this.textBox1.TabIndex = 6;
      this.textBox1.Text = "";
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(8, 8);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(96, 16);
      this.label3.TabIndex = 7;
      this.label3.Text = "PlayerID";
      // 
      // numericUpDown1
      // 
      this.numericUpDown1.Location = new System.Drawing.Point(120, 24);
      this.numericUpDown1.Name = "numericUpDown1";
      this.numericUpDown1.Size = new System.Drawing.Size(88, 20);
      this.numericUpDown1.TabIndex = 8;
      this.numericUpDown1.Value = new System.Decimal(new int[] {
                                                                 50,
                                                                 0,
                                                                 0,
                                                                 0});
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point(120, 8);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(96, 16);
      this.label4.TabIndex = 9;
      this.label4.Text = "How many times?";
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.button7);
      this.panel1.Controls.Add(this.button6);
      this.panel1.Controls.Add(this.button4);
      this.panel1.Controls.Add(this.label5);
      this.panel1.Controls.Add(this.button3);
      this.panel1.Controls.Add(this.txtServerSay);
      this.panel1.Controls.Add(this.textBox1);
      this.panel1.Controls.Add(this.label3);
      this.panel1.Controls.Add(this.numericUpDown1);
      this.panel1.Controls.Add(this.label4);
      this.panel1.Enabled = false;
      this.panel1.Location = new System.Drawing.Point(0, 96);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(224, 184);
      this.panel1.TabIndex = 10;
      // 
      // button7
      // 
      this.button7.Location = new System.Drawing.Point(8, 152);
      this.button7.Name = "button7";
      this.button7.Size = new System.Drawing.Size(208, 23);
      this.button7.TabIndex = 15;
      this.button7.Text = "Wreak Havoc";
      this.button7.Click += new System.EventHandler(this.button7_Click);
      // 
      // button6
      // 
      this.button6.Location = new System.Drawing.Point(120, 48);
      this.button6.Name = "button6";
      this.button6.Size = new System.Drawing.Size(96, 23);
      this.button6.TabIndex = 14;
      this.button6.Text = "Get Attention";
      this.button6.Click += new System.EventHandler(this.button6_Click);
      // 
      // button4
      // 
      this.button4.Location = new System.Drawing.Point(8, 48);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(96, 23);
      this.button4.TabIndex = 13;
      this.button4.Text = "Slap";
      this.button4.Click += new System.EventHandler(this.button4_Click);
      // 
      // label5
      // 
      this.label5.Location = new System.Drawing.Point(8, 80);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(128, 16);
      this.label5.TabIndex = 12;
      this.label5.Text = "Say Something Bitch";
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(8, 120);
      this.button3.Name = "button3";
      this.button3.TabIndex = 11;
      this.button3.Text = "Say";
      this.button3.Click += new System.EventHandler(this.button3_Click);
      // 
      // txtServerSay
      // 
      this.txtServerSay.Location = new System.Drawing.Point(8, 96);
      this.txtServerSay.Name = "txtServerSay";
      this.txtServerSay.Size = new System.Drawing.Size(200, 20);
      this.txtServerSay.TabIndex = 10;
      this.txtServerSay.Text = "";
      // 
      // button5
      // 
      this.button5.Location = new System.Drawing.Point(120, 56);
      this.button5.Name = "button5";
      this.button5.Size = new System.Drawing.Size(96, 23);
      this.button5.TabIndex = 11;
      this.button5.Text = "Get Player List";
      this.button5.Click += new System.EventHandler(this.button5_Click);
      // 
      // label6
      // 
      this.label6.BackColor = System.Drawing.Color.Black;
      this.label6.ForeColor = System.Drawing.Color.White;
      this.label6.Location = new System.Drawing.Point(0, 0);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(224, 16);
      this.label6.TabIndex = 12;
      this.label6.Text = "Anonymous Functions";
      // 
      // label7
      // 
      this.label7.BackColor = System.Drawing.Color.Black;
      this.label7.ForeColor = System.Drawing.Color.White;
      this.label7.Location = new System.Drawing.Point(0, 0);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(224, 16);
      this.label7.TabIndex = 13;
      this.label7.Text = "Authorization Required";
      // 
      // txtPort
      // 
      this.txtPort.Location = new System.Drawing.Point(8, 56);
      this.txtPort.Name = "txtPort";
      this.txtPort.Size = new System.Drawing.Size(104, 20);
      this.txtPort.TabIndex = 14;
      this.txtPort.Text = "1337";
      // 
      // panel2
      // 
      this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel2.Controls.Add(this.button5);
      this.panel2.Controls.Add(this.label6);
      this.panel2.Controls.Add(this.button1);
      this.panel2.Controls.Add(this.txtPort);
      this.panel2.Location = new System.Drawing.Point(8, 8);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(224, 88);
      this.panel2.TabIndex = 15;
      // 
      // panel3
      // 
      this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel3.Controls.Add(this.txtUsername);
      this.panel3.Controls.Add(this.txtPassword);
      this.panel3.Controls.Add(this.label1);
      this.panel3.Controls.Add(this.label2);
      this.panel3.Controls.Add(this.button2);
      this.panel3.Controls.Add(this.panel1);
      this.panel3.Controls.Add(this.label7);
      this.panel3.Location = new System.Drawing.Point(8, 104);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(224, 280);
      this.panel3.TabIndex = 16;
      // 
      // Form1
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(248, 398);
      this.Controls.Add(this.panel3);
      this.Controls.Add(this.panel2);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Form1";
      this.Text = "Fun Times with TS";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);

    }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

	  TeamspeakServerInterface ts;

    private void button1_Click(object sender, EventArgs e)
    {
      int[] servers = ts.GetServerPortList();
      int maxPlayers = 0;
      int activePlayers = 0;
      foreach (int port in servers)
      {
        ts.Query(port);
        maxPlayers += ts.ServerInfo.MaxPlayers;
        activePlayers += ts.ServerInfo.Players.Length;
      }
      MessageBox.Show(String.Format("There are currently {0} active players on the system.\r\n({1} total slots available on {2} servers)", activePlayers, maxPlayers, servers.Length));
    }

    private void button2_Click(object sender, EventArgs e)
    {
      try
      {
        ts.SuperAdminLogin(txtUsername.Text, txtPassword.Text);
        this.panel1.Enabled = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void button3_Click(object sender, EventArgs e)
    {
      ts.Query(Convert.ToInt32(this.txtPort.Text));
      ts.ServerSay(this.txtServerSay.Text);
    }

    private void button4_Click(object sender, System.EventArgs e)
    {
      ts.Query(Convert.ToInt32(this.txtPort.Text));
      ts.Slap(Convert.ToInt32(this.textBox1.Text), Convert.ToInt32(this.numericUpDown1.Text));
    }

    public delegate void ThreadedSlap(int playerNumber, int length);

    private void button5_Click(object sender, System.EventArgs e)
    {
      ts.Query(Convert.ToInt32(txtPort.Text));
      string s = String.Empty;
      foreach (TeamspeakPlayer pl in ts.ServerInfo.Players)
      {
        string statusText = String.Empty;
        if (pl.GetUserStatus(PlayerUserStatus.ServerAdmin)) statusText += "[SA]";
        s += String.Format("[{0}] {1} {2}\r\n", pl.PlayerID, pl.PlayerName, statusText);
      }
      MessageBox.Show("Players on server " + txtPort.Text + ":\r\n\r\n" + s,
        "Information",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);
    }

    private void button6_Click(object sender, System.EventArgs e)
    {
      ts.Query(Convert.ToInt32(this.txtPort.Text));
      ts.GetAttention(Convert.ToInt32(this.textBox1.Text), Convert.ToInt32(this.numericUpDown1.Text));
      //ThreadedSlap[] slappers = new ThreadedSlap[5];
      //slappers[0] = new ThreadedSlap(SlapperMethod); slappers[0].BeginInvoke(Convert.ToInt32(this.textBox1.Text), Convert.ToInt32(this.numericUpDown1.Text), null, null);
      //slappers[1] = new ThreadedSlap(SlapperMethod); slappers[1].BeginInvoke(Convert.ToInt32(this.textBox2.Text), Convert.ToInt32(this.numericUpDown1.Text), null, null);
      //slappers[2] = new ThreadedSlap(SlapperMethod); slappers[2].BeginInvoke(Convert.ToInt32(this.textBox3.Text), Convert.ToInt32(this.numericUpDown1.Text), null, null);
      //slappers[3] = new ThreadedSlap(SlapperMethod); slappers[3].BeginInvoke(Convert.ToInt32(this.textBox4.Text), Convert.ToInt32(this.numericUpDown1.Text), null, null);
      //slappers[4] = new ThreadedSlap(SlapperMethod); slappers[4].BeginInvoke(Convert.ToInt32(this.textBox5.Text), Convert.ToInt32(this.numericUpDown1.Text), null, null);
    }

    private void SlapperMethod(int playerNumber, int length)
    {
      Console.WriteLine(playerNumber);
      TeamspeakServerInterface tsi = new TeamspeakServerInterface("70.84.125.228");
      tsi.SuperAdminLogin(txtUsername.Text, txtPassword.Text);
      tsi.Query(Convert.ToInt32(this.txtPort.Text));
      tsi.Slap(playerNumber, length);
    }

    private void button7_Click(object sender, System.EventArgs e)
    {
      ts.Query(Convert.ToInt32(this.txtPort.Text));
      ts.WreakHavok(Convert.ToInt32(this.numericUpDown1.Text));
    }
	}
}

///////////////////////////////////////////////////////////////////////////////////////////////////////
//
// public void Hahaha(string match)
// {
//   Random r = new Random(DateTime.Now.Millisecond);
//   foreach (TeamspeakPlayer p in players)
//   {
//     if (p.PlayerName.StartsWith(match))
//     {
//       int channelIndex = r.Next(0, channels.Length-1);
//       int channelID = channels[channelIndex].ID;
//        string command = "mptc " + channelID.ToString() + " " +  p.PlayerID.ToString() + "\n";
//       SendData(command);
//        string response = ReceiveData(1024)[0];
//     }
//   }
// }
// 
// public void Hahaha2(string match)
// {
//   Random r = new Random(DateTime.Now.Millisecond);
//   foreach (TeamspeakPlayer p in players)
//   {
//     if (true) //(p.PlayerName.StartsWith(match))
//     {
//       SendData("sppriv " + p.PlayerID.ToString() + " privilege_serveradmin 0\n");
//       ReceiveData(512);
//       SendData("sppriv " + p.PlayerID.ToString() + " privilege_serveradmin 1\n");
//       ReceiveData(512);
//     }
//   }
// }
//
///////////////////////////////////////////////////////////////////////////////////////////////////////