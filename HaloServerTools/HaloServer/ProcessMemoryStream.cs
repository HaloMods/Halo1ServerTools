using System;
using System.IO;

namespace HaloServerTools
{
	/// <summary>
	/// Encapsulates the manipulation of a process's memory into a Stream object.
	/// </summary>
	public class ProcessMemoryStream : Stream
	{
		private long m_currentPosition = 0;
    private long m_streamLength = 0;
    private int m_processID;
    private IntPtr m_handle;

    public ProcessMemoryStream(int processID)
		{
      m_processID = processID;
      m_handle = (IntPtr)Api.OpenProcess(0x001F0FFF, false, m_processID);
		}

    public override void Close()
    {
      Api.CloseHandle((long)m_handle);
      base.Close ();
    }

    public void Dispose()
    {
      this.Close();
    }

	  public override long Seek(long offset, SeekOrigin origin)
	  {
      if (origin == SeekOrigin.Begin) m_currentPosition = offset;
      if (origin == SeekOrigin.Current) m_currentPosition += offset;
      if (origin == SeekOrigin.End) m_currentPosition = m_streamLength - offset;
      return m_currentPosition;
	  }

	  public override void SetLength(long value)
	  {
	    m_streamLength = value;
	  }

	  public override int Read(byte[] buffer, int offset, int count)
	  {
      IntPtr zero;
      Api.ReadProcessMemory(m_handle, (IntPtr)m_currentPosition, buffer, (uint)count, out zero);
      m_currentPosition += count;
      return count;
	  }

	  public override void Write(byte[] buffer, int offset, int count)
	  {
      IntPtr zero;
      Api.WriteProcessMemory(m_handle, (IntPtr)m_currentPosition, buffer, (uint)count, out zero);
      m_currentPosition += count;
	  }

    public override long Length
    {
      get { return 0; }
    }

    public override long Position
    {
      get { return m_currentPosition; }
      set { m_currentPosition = value; }
    }

	  public override bool CanRead
	  {
	    get { return true; }
	  }

	  public override bool CanSeek
	  {
	    get { return true; }
	  }

	  public override bool CanWrite
	  {
	    get { return true; }
	  }

    public override void Flush()
    {
    }
  }
}
