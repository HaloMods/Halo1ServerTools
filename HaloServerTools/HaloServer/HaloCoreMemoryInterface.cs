using System;
using System.IO;

namespace HaloServerTools.CoreMemory
{
	/// <summary>
	/// Summary description for HaloCoreMemoryInterface.
	/// </summary>
	public class HaloCoreMemoryInterface
	{
    public class DataArrayHeader
    {
      public char[] Name = new char[32];
      public short DataCount;
      public short SingleElementSize; // (player element: 0x200 \ 512)
      public short unknown1;
      public short unknown2;
      public int unknown3;
      public short unknown4;
      public short unknown5;
      public short unknown6;
      public short unknown7;
      public int ElementsOffset; // (don't worry about this)
      public void Read(ref BinaryReader br)
      {
        try
        {
          Name = br.ReadChars(32);
          DataCount = br.ReadInt16();
          SingleElementSize = br.ReadInt16(); // (player element: 0x200 \ 512)
          br.BaseStream.Seek(20, SeekOrigin.Current);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }
      public void Write(ref BinaryWriter bw)
      {
        bw.BaseStream.Seek(56, SeekOrigin.Current);
      }
    }

    public class PlayerData
    {
      public short DatumizedPlayerIndex;
      public char[] UnicodePlayerName = new char[24];
      public short Team;
      public short RespawnTime;
      public short MachineIndex;
      public short OvershieldTimeLeft;
      public short Kills;
      public short Assists;
      public short Deaths;
      public short TKs;
      public short Score;
      public short Ping;
      public float X;
      public float Y;
      public float Z;
      public float PredictedX;
      public float PredictedY;
      public float PredictedZ;

      public string PlayerName
      {
        get { return LogfileMonitor.DeUnicode(new string(UnicodePlayerName)).Trim('\0'); }
        set
        {
          string name = value;
          char[] letters = value.ToCharArray(); 
          char[] unicode = new char[letters.Length*2]; 
  
          for (int x=0; x<letters.Length; x+=2) 
          { 
            unicode[x] = letters[x]; 
            unicode[x+1] = '\0'; 
          } 
          UnicodePlayerName = unicode;
        }
      }
      
      public void Read(ref BinaryReader br)
      {
        long startPos = br.BaseStream.Position;
        br.BaseStream.Position = startPos + 0x0; DatumizedPlayerIndex = br.ReadInt16();
        br.BaseStream.Position = startPos + 0x4; UnicodePlayerName = br.ReadChars(24);
        br.BaseStream.Position = startPos + 0x20; Team = br.ReadInt16();
        br.BaseStream.Position = startPos + 0x2C; RespawnTime = br.ReadInt16();
        br.BaseStream.Position = startPos + 0x64; MachineIndex = br.ReadInt16();
        br.BaseStream.Position = startPos + 0x68; PowerupTimeLeft = br.ReadInt16();
        br.BaseStream.Position = startPos + 0x9C; Kills = br.ReadInt16();
        br.BaseStream.Position = startPos + 0xA4; Assists = br.ReadInt16();
        br.BaseStream.Position = startPos + 0xAE; Deaths = br.ReadInt16();
        br.BaseStream.Position = startPos + 0xC0; TKs = br.ReadInt16();
        br.BaseStream.Position = startPos + 0xC4; Score = br.ReadInt16();
        br.BaseStream.Position = startPos + 0xDC; Ping = br.ReadInt16();
        br.BaseStream.Position = startPos + 0xF8; X = br.ReadSingle();
        br.BaseStream.Position = startPos + 0xFC; Y = br.ReadSingle();
        br.BaseStream.Position = startPos + 0x100; Z = br.ReadSingle();
        br.BaseStream.Position = startPos + 0x170; PredictedX = br.ReadSingle();
        br.BaseStream.Position = startPos + 0x174; PredictedY = br.ReadSingle();
        br.BaseStream.Position = startPos + 0x178; PredictedZ = br.ReadSingle();
        br.BaseStream.Position = startPos + 512;
      }
      public void Write(ref BinaryWriter bw)
      {
        long startPos = bw.BaseStream.Position;
        bw.BaseStream.Position = startPos + 0x0; bw.Write(DatumizedPlayerIndex);
        bw.BaseStream.Position = startPos + 0x4; bw.Write(UnicodePlayerName);
        bw.BaseStream.Position = startPos + 0x20; bw.Write(Team);
        bw.BaseStream.Position = startPos + 0x2C; bw.Write(RespawnTime);
        bw.BaseStream.Position = startPos + 0x64; bw.Write(MachineIndex);
        bw.BaseStream.Position = startPos + 0x68; bw.Write(PowerupTimeLeft);
        bw.BaseStream.Position = startPos + 0x9C; bw.Write(Kills);
        bw.BaseStream.Position = startPos + 0xA4; bw.Write(Assists);
        bw.BaseStream.Position = startPos + 0xAE; bw.Write(Deaths);
        bw.BaseStream.Position = startPos + 0xC0; bw.Write(TKs);
        bw.BaseStream.Position = startPos + 0xC4; bw.Write(Score);
        bw.BaseStream.Position = startPos + 0xDC; bw.Write(Ping);
        bw.BaseStream.Position = startPos + 0xF8; bw.Write(X);
        bw.BaseStream.Position = startPos + 0xFC; bw.Write(Y);
        bw.BaseStream.Position = startPos + 0x100; bw.Write(Z);
        bw.BaseStream.Position = startPos + 0x170; bw.Write(PredictedX);
        bw.BaseStream.Position = startPos + 0x174; bw.Write(PredictedY);
        bw.BaseStream.Position = startPos + 0x178; bw.Write(PredictedZ);
        bw.BaseStream.Position = startPos + 512;
      }
    }

    public PlayerData[] playerData = new PlayerData[16];

    public void ReadPlayerData()
    {
      HaloServer hs = (HaloServer)HaloServerTools.ServerManager.ServerList[0];
      BinaryReader br = new BinaryReader(
        new ProcessMemoryStream(hs.proc.Id));
      br.BaseStream.Seek(0x4029CE90, SeekOrigin.Begin);
      DataArrayHeader playerHeader = new DataArrayHeader();
      playerHeader.Read(ref br);
      
      for (int x=0; x<playerHeader.DataCount; x++)
      {
        playerData[x] = new PlayerData();
        playerData[x].Read(ref br);
      }

      br.Close();
    }
    
    public void WritePlayerData()
    {
      HaloServer hs = (HaloServer)HaloServerTools.ServerManager.ServerList[0];
      BinaryWriter bw = new BinaryWriter(
        new ProcessMemoryStream(hs.proc.Id));
      bw.BaseStream.Seek(0x4029CE90, SeekOrigin.Begin);
      DataArrayHeader playerHeader = new DataArrayHeader();
      playerHeader.Write(ref bw);
      
      for (int x=0; x<16; x++)
      {
        playerData[0].Write(ref bw);
      }
      bw.Close();
    }

	}
}