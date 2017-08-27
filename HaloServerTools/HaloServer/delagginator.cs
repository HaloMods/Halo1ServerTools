using System;
using System.Diagnostics;

namespace HaloServerTools
{
	/// <summary>
	/// Summary description for Delagginator.
	/// </summary>
	public class Delagginator
	{
		private IntPtr offset1 = (IntPtr)0x613AD4;
		private int Value1 = 0;
		private IntPtr offset7 = (IntPtr)0x613AD0;
		private int Value7 = 0;
		private IntPtr offset2 = (IntPtr)0x59A410;
		private int Value2 = 180;
		private IntPtr offset3 = (IntPtr)0x59A414;
		private int Value3 = 30;
		private IntPtr offset4 = (IntPtr)0x59A418;
		private int Value4 = 90;
		private IntPtr offset5 = (IntPtr)0x59A41C;
		private int Value5 = 15;
		private IntPtr offset6 = (IntPtr)0x59A420;
		private int Value6 = 90;
		private int pId;

		private bool m_delagged = false;
		public bool IsDelagged {
			get { return m_delagged; }
		}

		public Delagginator(int id)
		{
			pId = id;
		}

		private void HackMem(int PID, IntPtr Address, int Value, int Bytes)
		{
			IntPtr zero = (IntPtr)0;
			//object addy = Address;
			//object val = Value;
			byte[] bValue = new byte[4];
			bValue = BitConverter.GetBytes(Convert.ToInt32(Value));

			System.Diagnostics.Debug.WriteLine("PID: " + PID.ToString());
			IntPtr pHandle = (IntPtr)Api.OpenProcess(0x001F0FFF, false, PID);
			System.Diagnostics.Debug.WriteLine("OP: " + pHandle.ToString());

			//int test2 = WinAPI.Kernal32.VirtualProtectEx(pHandle, ref addy, sizeof(int), 0x1F040, ref zero);
			//System.Diagnostics.Debug.WriteLine("VPEx: " + test2.ToString());

			zero = (IntPtr)0;
			//bool test = Api.WriteProcessMemory(pHandle, Address, bValue, (UIntPtr)Bytes, zero);
			//System.Diagnostics.Debug.WriteLine("WPM: " + test.ToString());
			Api.CloseHandle((long)pHandle);
		}

		public void Delaggit()
		{
			this.HackMem(pId, this.offset1, 0, 4);
			this.HackMem(pId, this.offset2, 0, 4);
			this.HackMem(pId, this.offset3, 0, 4);
			this.HackMem(pId, this.offset4, 0, 4);
			this.HackMem(pId, this.offset5, 0, 4);
			this.HackMem(pId, this.offset6, 0, 4);
			this.HackMem(pId, this.offset7, 0, 4);
			m_delagged = true;
		}
		public void Laggit()
		{
			this.HackMem(pId, this.offset1, this.Value1, 4);
			this.HackMem(pId, this.offset2, this.Value2, 4);
			this.HackMem(pId, this.offset3, this.Value3, 4);
			this.HackMem(pId, this.offset4, this.Value4, 4);
			this.HackMem(pId, this.offset5, this.Value5, 4);
			this.HackMem(pId, this.offset6, this.Value6, 4);
			this.HackMem(pId, this.offset7, this.Value7, 4);
			m_delagged = false;
		}
	}
}
