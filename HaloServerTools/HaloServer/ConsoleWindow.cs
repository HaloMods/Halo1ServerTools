using System;
using System.Windows.Forms;
using System.Threading;

namespace HaloServerTools
{
	/// <summary>
	/// A ConsoleWindow - provides methods for sending messages to it's message queue.
	/// </summary>
	public class ConsoleWindow
	{
		const int WM_KEYDOWN = 0x100;
		const int WM_KEYUP   = 0x101;
		const int WM_CHAR    = 0x102;

		private IntPtr _hWnd; // The handle to the console window
		private Thread th;
		private System.Collections.ArrayList q;

		public ConsoleWindow(IntPtr hWnd)
		{
			_hWnd = hWnd;
			q = new System.Collections.ArrayList();
			th = new Thread(new ThreadStart(CommandListener));
			th.Start();
		}

		private void CommandListener()
		{
			while (true)
			{
				if (q.Count > 0)
				{
					// There are items in the queue
					string s = (string)q[0];
					q.Remove(q[0]);
					SendCommand(s);
				}
				Wait(100); // Give a short pause between commands to improve stability
			}
		}

		public void QueueCommand (string text)
		{
			if (q.Count < 25)
			{
				q.Add(text);
			}
		}

		/// <summary>
		/// Send a command to the console window, appending an "ENTER" keypress.
		/// </summary>
		/// <param name="text">The command to be sent.</param>
		private void SendCommand (string text)
		{
			SendText (_hWnd, text);
			Wait(150);
			SendKeys ("ENTER");
			Wait(150); 
		}

		/// <summary>
		/// Sends a string of text characters to the window
		/// </summary>
		/// <param name="text">A valid window handle.</param>
		/// <param name="text">The text string to be sent.</param>
		public void SendText (IntPtr hWnd, string text)
		{
			string s;
			Keys k;
			for (int x=0;x<text.Length;x++)
			{
				s = text.Substring(x,1);
				// if this is extended ascii (> 126) we need to use the alt-keycode to type it
				byte[] b = System.Text.Encoding.Default.GetBytes(s);
				int i = (int)b[0];
				if (i < 9999)
				{
					k = Api.VkKeyScan(Convert.ToChar(s.ToLower()));
					Api.SendMessage(hWnd, 0x102, Convert.ToChar(s), new IntPtr(0));
					System.Threading.Thread.Sleep(5); //10
					Api.SendMessage(hWnd, 0x100, Convert.ToChar(s), new IntPtr(0));
					System.Threading.Thread.Sleep(5); //10
				}
				else
				{
					string s2 = Convert.ToString(i);
					k = Keys.Alt;
					Api.SendMessage(_hWnd, 0x100, k);
					for (int x2=0; x<s.Length; x++)
					{
						Api.SendMessage(hWnd, 0x102, Convert.ToChar(s2.Substring(x2, 1)), new IntPtr(0));
					}
					k = Keys.Alt;
					Api.SendMessage(_hWnd, 0x102, k);
				}
			}
		}

    public void ServerSay (string text)
    {
      QueueCommand("sv_say \"" + text + "\"");
    }

		public void SendKeys (string param)
		{
			string[] s = param.Split("|".ToCharArray());
			string[] c;
			Keys[] k;

			for (int x=0; x<s.Length; x++)
			{
				c = s[x].Split("+".ToCharArray(),2);
				k = new Keys[c.Length];				
				for (int y=0; y<c.Length; y++)
				{
					switch (c[y])
					{
						case "CTRL": k[y] = Keys.ControlKey; break;
						case "SHIFT": k[y] = Keys.ShiftKey; break;
						case "ALT": k[y] = Keys.Menu; break;
						case "ENTER": k[y] = Keys.Enter; break;
						case "F1": k[y] = Keys.F1; break;
						case "F2": k[y] = Keys.F2; break;
						case "F3": k[y] = Keys.F3; break;
						case "F4": k[y] = Keys.F4; break;
						default: k[y] = Api.VkKeyScan(Convert.ToChar(c[y].ToLower())); break;
					}
					Api.SendMessage(_hWnd, 0x100, k[y]);
				}
				// Release all of the keys in reverse order
				for (int y=c.Length; y>0; y--) Api.SendMessage(_hWnd, 0x102, k[y-1]);
			}
		}
		private void Wait(int milliseconds)
		{
			Application.DoEvents();
			System.Threading.Thread.Sleep(milliseconds);
		}
	}
}
