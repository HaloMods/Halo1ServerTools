using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

[Flags]
public enum PaintOptions 
{
  CheckVisible = 0x01,
  NonClient = 0x02,
  Client = 0x04,
  EraseBackground = 0x08,
  Children = 0x10,
  Owned = 0x20
}

[SuppressUnmanagedCodeSecurity]
public sealed class ControlPainter 
{
  [DllImport("USER32.DLL")]
  private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, int lParam);

  private const int WM_PRINT = 0x317;

  public static void PaintControl(Graphics graphics, Control control) 
  {
    PaintControl(graphics, control, PaintOptions.Client);
  }

  public static void PaintControl(Graphics graphics, Control control, PaintOptions paintOptions) 
  {
    IntPtr hDC = graphics.GetHdc();

    //paint control onto graphics using provided options
    try 
    {
      SendMessage(control.Handle, WM_PRINT, hDC, (int) paintOptions);
    } 
    finally 
    {
      graphics.ReleaseHdc(hDC);
    }
  }
}