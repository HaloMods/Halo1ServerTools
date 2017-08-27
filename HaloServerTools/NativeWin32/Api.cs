using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

public class Api
{
	public const int MOUSEEVENTF_MOVE = 0x1;          // mouse move
		public const int MOUSEEVENTF_LEFTDOWN = 0x2;      // left button down
		public const int MOUSEEVENTF_LEFTUP = 0x4;        // left button up
		public const int MOUSEEVENTF_RIGHTDOWN = 0x8;     // right button down
		public const int MOUSEEVENTF_RIGHTUP = 0x10;      // right button up
		public const int MOUSEEVENTF_MIDDLEDOWN = 0x20;   // middle button down
		public const int MOUSEEVENTF_MIDDLEUP = 0x40;     // middle button up
		public const int MOUSEEVENTF_WHEEL = 0x800;       // wheel button rolled
		public const int MOUSEEVENTF_ABSOLUTE = 0x8000;   // absolute move
		public struct POINTAPI
		{
			public int x;
			public int y;
		}

		[DllImport("user32.dll", SetLastError=true)]
		public static extern bool RegisterHotKey(
			IntPtr hWnd, // handle to window    
			int id, // hot key identifier    
			KeyModifiers fsModifiers, // key-modifier options    
			Keys vk    // virtual-key code    
			); 
        
		[DllImport("user32.dll", SetLastError=true)]
		public static extern bool UnregisterHotKey( 
			IntPtr hWnd, // handle to window    
			int id      // hot key identifier    
			);

		[DllImport("kernel32.dll",EntryPoint="GetTickCount")]
		public static extern long GetTickCount();
	
		[DllImport("user32.dll",EntryPoint="SendMessage")]
		public static extern int SendMessage(IntPtr _WindowHandler, int _WM_USER, Keys _data);

		[DllImport("user32.dll",EntryPoint="SendMessage")]
		public static extern int SendMessage(IntPtr _WindowHandler, int _WM_USER, char key, IntPtr wParam);

		[DllImport("user32.dll",EntryPoint="SendMessage")]
		public static extern int SendMessage(IntPtr _WindowHandler, int _cmd);

		[DllImport("user32.dll",EntryPoint="FindWindow")]
		public static extern int FindWindow(string _ClassName, string _WindowName);

		[DllImport("user32.dll",EntryPoint="GetCursorPos")]
		public static extern long GetCursorPos(POINTAPI _lpPoint);

		[DllImport("user32.dll",EntryPoint="SetCursorPos")]
		public static extern long SetCursorPos(POINTAPI _lpPoint);

		[DllImport("user32.dll",EntryPoint="ClientToScreen")]
		public static extern long ClientToScreen(long _hwnd, POINTAPI _lpPoint);

		[DllImport("user32.dll",EntryPoint="mouse_event")]
		public static extern long mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

		[DllImport("user32.dll",EntryPoint="keybd_event")]
		public static extern long keybd_event(Keys bVk, byte bScan, long dwFlags, long dwExtraInfo);

		[DllImport("user32.dll",EntryPoint="VkKeyScan")]
		public static extern Keys VkKeyScan(char ch);

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		/*[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
			byte [] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesWritten);*/

  [DllImport("kernel32.dll")]
  public static extern Int32 WriteProcessMemory(
    IntPtr hProcess, 
    IntPtr lpBaseAddress,
    [In, Out] byte[] buffer, 
    UInt32 size, 
    out IntPtr lpNumberOfBytesWritten);

    /*[DllImport("kernel32.dll", SetLastError=true)]
    public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
      out byte[] lpBuffer, IntPtr nSize, IntPtr lpNumberOfBytesWritten);*/
  [DllImport("kernel32.dll")]
  public static extern Int32 ReadProcessMemory(
    IntPtr hProcess, 
    IntPtr lpBaseAddress,
    [In, Out] byte[] buffer, 
    UInt32 size, 
    out IntPtr lpNumberOfBytesRead
    );

		[DllImport("kernel32.dll", SetLastError=true)]
		public static extern int CloseHandle(long hObject);

		[DllImport("kernel32.dll", SetLastError=true)]
		public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll", SetLastError=true)]
		public static extern int VirtualProtectEx(int hProcess, ref int lpAddress,
			int dwSize, int flNewProtect, ref int lpflOldProtect);

		[Flags()]
			public enum KeyModifiers
		{  
			None = 0,
			Alt = 1,    
			Control = 2,    
			Shift = 4,    
			Windows = 8
		}
}
