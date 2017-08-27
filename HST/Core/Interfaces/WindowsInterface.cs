using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace Core.Interfaces
{
	/// <summary>
	/// WinAPI Functions and Window Class
	/// </summary>
	public class WindowsInterface
	{	
		#region API
		[DllImport("user32.dll",EntryPoint="SendMessage")] public static extern 
			int SendMessage(IntPtr _WindowHandler, int _WM_USER, Keys _data);

		[DllImport("user32.dll",EntryPoint="SendMessage")] public static extern 
			int SendMessage(IntPtr _WindowHandler, int _WM_USER, char key, IntPtr wParam);

		[DllImport("user32.dll",EntryPoint="SendMessage")] public static extern 
			int SendMessage(IntPtr _WindowHandler, int _cmd);

		[DllImport("user32.dll",EntryPoint="FindWindow")] public static extern 
			int FindWindow(string _ClassName, string _WindowName);

		[DllImport("user32.dll")] public static extern 
			bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")] public static extern 
			IntPtr GetForegroundWindow();

		[DllImport("kernel32.dll", SetLastError=true)] public static extern 
			int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll", SetLastError=true)] public static extern 
			int CloseHandle(long hObject);

		[DllImport("user32.dll",EntryPoint="VkKeyScan")] public static extern 
			Keys VkKeyScan(char ch);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hProcess">Target Process</param>
		/// <param name="lpBaseAddress">Address to start writing to</param>
		/// <param name="buffer">The bytes to write</param>
		/// <param name="size">Size of the buffer to write</param>
		/// <param name="lpNumberOfBytesWritten">Bytes actually written</param>
		/// <returns></returns>
		[DllImport("kernel32.dll")] public static extern 
			Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hProcess">Target Process</param>
		/// <param name="lpBaseAddress">Address to start reading from</param>
		/// <param name="buffer">Buffer to read the bytes into</param>
		/// <param name="size">Size of the buffer to read from memory</param>
		/// <param name="lpNumberOfBytesRead">Bytes actually read</param>
		/// <returns></returns>
		[DllImport("kernel32.dll")] public static extern 
			Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

		[DllImport("kernel32.dll", SetLastError=true)] public static extern 
			int VirtualProtectEx(int hProcess, ref int lpAddress, int dwSize, int flNewProtect, ref int lpflOldProtect);

		public enum WindowsMessage
		{
			Keydown = 0x100,
			Keyup = 0x101,
			Char = 0x102,
		};
		#endregion

		#region ConsoleInterface
		/// <summary>
		/// The handle to the console window
		/// </summary>
		private IntPtr _hWnd;
		private Thread thread;
		private System.Collections.ArrayList queue;

		public WindowsInterface(IntPtr hWnd)
		{
			_hWnd = hWnd;
			queue = new System.Collections.ArrayList();
			thread = new Thread(new ThreadStart(CommandListener));
			thread.Start();
		}

		private void CommandListener()
		{
			string s;
			while (true)
			{
				if (queue.Count > 0)
				{
					// There are items in the queue
					s = queue[0] as string;
					queue.Remove(queue[0]);
					SendCommand(s);
				}
				Wait(100); // Give a short pause between commands to improve stability
			}
		}

		public void QueueCommand (string text)
		{
			if (queue.Count < 25)
				queue.Add(text);
		}

		/// <summary>
		/// Send a command to the console window, appending an "ENTER" keypress.
		/// </summary>
		/// <param name="text">The command to be sent.</param>
		private void SendCommand(string text)
		{
			SendText(_hWnd, text);
			Wait(150);
			SendKeys("ENTER");
			Wait(150); 
		}

		/// <summary>
		/// Sends a string of text characters to the window
		/// </summary>
		/// <param name="text">A valid window handle.</param>
		/// <param name="text">The text string to be sent.</param>
		public void SendText(IntPtr hWnd, string text)
		{
			string s;
			Keys k;
			for (int x = 0; x < text.Length; x++)
			{
				s = text.Substring(x,1);
				// if this is extended ascii (> 126) we need to use the alt-keycode to type it
				int i = (int)(System.Text.Encoding.Default.GetBytes(s)[0]);
				if (i < 9999)
				{
					k = VkKeyScan(Convert.ToChar(s.ToLower()));
					SendMessage(hWnd, (int)WindowsMessage.Char, Convert.ToChar(s), IntPtr.Zero);
					System.Threading.Thread.Sleep(5); //10
					SendMessage(hWnd, (int)WindowsMessage.Keydown, Convert.ToChar(s), IntPtr.Zero);
					System.Threading.Thread.Sleep(5); //10
				}
				else
				{
					string s2 = Convert.ToString(i);
					k = Keys.Alt;
					SendMessage(_hWnd, (int)WindowsMessage.Keydown, k);
					for (int x2 = 0; x < s.Length; x++)
						SendMessage(hWnd, (int)WindowsMessage.Char, Convert.ToChar(s2.Substring(x2, 1)), IntPtr.Zero);
					k = Keys.Alt;
					SendMessage(_hWnd, (int)WindowsMessage.Char, k);
				}
			}
		}

		public void SendKeys (string param)
		{
			string[] s = param.Split("|".ToCharArray());
			string[] c;
			Keys[] k;

			for (int x = 0; x < s.Length; x++)
			{
				c = s[x].Split("+".ToCharArray(),2);
				k = new Keys[c.Length];				
				for (int y = 0; y < c.Length; y++)
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
						default: k[y] = VkKeyScan(Convert.ToChar(c[y].ToLower())); break;
					}
					SendMessage(_hWnd, (int)WindowsMessage.Keydown, k[y]);
				}
				// Release all of the keys in reverse order
				for (int y = c.Length; y > 0; y--) SendMessage(_hWnd, (int)WindowsMessage.Char, k[y-1]);
			}
		}

		private void Wait(int milliseconds)
		{
			Application.DoEvents();
			System.Threading.Thread.Sleep(milliseconds);
		}
		#endregion
	};
}