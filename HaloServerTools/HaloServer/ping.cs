using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace HaloServerTools
{

	/// <summary>
	///		The Main Ping Class
	/// </summary>
	public class Ping
	{
		//Declare some Constant Variables
	    const int SOCKET_ERROR = -1;        
	    const int ICMP_ECHO = 8;
		
		/// <summary>
		///		This method takes the "hostname" of the server
		///		and then it ping's it and returns the response time
		/// </summary>
		public static int PingHost(string host)
		{
			//Declare the IPHostEntry 
	        IPHostEntry serverHE, fromHE;
	        int nBytes = 0;
	        int dwStart = 0, dwStop = 0;
	        
			//Initilize a Socket of the Type ICMP
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);

	        // Get the server endpoint
			try
			{
				serverHE = Dns.GetHostByName(host);	
			}
	        catch(Exception)
			{
				Trace.WriteLine("Host not found"); // fail
				return -1;
			}

	        // Convert the server IP_EndPoint to an EndPoint
	        IPEndPoint ipepServer = new IPEndPoint(serverHE.AddressList[0], 0);
	        EndPoint epServer = (ipepServer);	

			// Set the receiving endpoint to the client machine
	        fromHE = Dns.GetHostByName(Dns.GetHostName());
	        IPEndPoint ipEndPointFrom = new IPEndPoint(fromHE.AddressList[0], 0);        
	        EndPoint EndPointFrom = (ipEndPointFrom);

	        int PacketSize = 0;
			IcmpPacket packet = new IcmpPacket();
	        // Construct the packet to send
	        packet.Type = ICMP_ECHO; //8
	        packet.SubCode = 0;
	        packet.CheckSum = UInt16.Parse("0");
	        packet.Identifier   = UInt16.Parse("45"); 
	        packet.SequenceNumber  = UInt16.Parse("0"); 
			int PingData = 32; // sizeof(IcmpPacket) - 8;
	        packet.Data = new Byte[PingData];
			//Initilize the Packet.Data
	        for (int i = 0; i < PingData; i++)
			{
				packet.Data[i] = (byte)'#';
			}
	             
			//Variable to hold the total Packet size
	        PacketSize = PingData + 8;
	        Byte [] icmp_pkt_buffer = new Byte[ PacketSize ]; 
	        Int32 Index = 0;
			//Call a Methos Serialize which counts
			//The total number of Bytes in the Packet
	        Index = Serialize(  
	                      packet, 
	                      icmp_pkt_buffer, 
	                      PacketSize, 
	                      PingData );
			//Error in Packet Size
	        if( Index == -1 )
			{
				Trace.WriteLine("Error in Making Packet");
				return -1;
			}
          
	            // now get this critter into a UInt16 array
	         
	            //Get the Half size of the Packet
	            Double double_length = Convert.ToDouble(Index);
	            Double dtemp = Math.Ceiling( double_length / 2);
				int cksum_buffer_length = Convert.ToInt32(dtemp);
	            //Create a Byte Array
	            UInt16 [] cksum_buffer = new UInt16[cksum_buffer_length];
				//Code to initilize the Uint16 array 
	            int icmp_header_buffer_index = 0;
	            for( int i = 0; i < cksum_buffer_length; i++ ) {
					cksum_buffer[i] = 
	                   BitConverter.ToUInt16(icmp_pkt_buffer,icmp_header_buffer_index);
	                icmp_header_buffer_index += 2;
	            }
				//Call a method which will return a checksum             
	            UInt16 u_cksum = checksum(cksum_buffer, cksum_buffer_length);
				//Save the checksum to the Packet
	            packet.CheckSum  = u_cksum; 
	            
	            // Now that we have the checksum, serialize the packet again
	            Byte [] sendbuf = new Byte[ PacketSize ]; 
	            //again check the packet size
	            Index = Serialize(  
	                        packet, 
	                        sendbuf, 
	                        PacketSize, 
	                        PingData );
				//if there is a error report it
	            if( Index == -1 )
				{
					Trace.WriteLine("Error in Making Packet");
					return -1;
				}
	                

	            dwStart = System.Environment.TickCount; // Start timing
				//send the Pack over the socket
	            if ((nBytes = socket.SendTo(sendbuf, PacketSize, 0, epServer)) == SOCKET_ERROR) 
				{		
	                Trace.WriteLine("Socket Error cannot Send Packet");
					return -1; // Should we return yet?
	            }
				// Initialize the buffers. The receive buffer is the size of the
	            // ICMP header plus the IP header (20 bytes)
	            Byte [] ReceiveBuffer = new Byte[256]; 
	            nBytes = 0;
				//Receive the bytes
	            bool recd =false ;
				int timeout=0 ;
 
				//loop for checking the time of the server responding 
				while(!recd)
				{
					nBytes = socket.ReceiveFrom(ReceiveBuffer, 256, 0, ref EndPointFrom);
					if (nBytes == SOCKET_ERROR) 
					{
						Trace.WriteLine("Host not Responding") ;
						recd=true ;
						break;
					}
					else if(nBytes>0)
					{
						dwStop = System.Environment.TickCount - dwStart; // stop timing
						// Trace.WriteLine("Reply from "+epServer.ToString()+" in "+dwStop+"MS :Bytes Received"+nBytes);
						recd=true;
						break;
					}
					timeout=System.Environment.TickCount - dwStart;
					if(timeout>1000)
					{
						Trace.WriteLine("Time Out") ;
						recd=true;
					}
					
				}
	        
			//close the socket
	        socket.Close();     
			
			return dwStop;
		}
		/// <summary>
		///  This method get the Packet and calculates the total size 
		///  of the Pack by converting it to byte array
		/// </summary>
		public static Int32 Serialize(  IcmpPacket packet, Byte [] Buffer, Int32 PacketSize, Int32 PingData )
	    {
	        Int32 cbReturn = 0;
	        // serialize the struct into the array
	        int Index=0;

	        Byte [] b_type = new Byte[1];
	        b_type[0] = (packet.Type);

	        Byte [] b_code = new Byte[1];
	        b_code[0] = (packet.SubCode);

	        Byte [] b_cksum = BitConverter.GetBytes(packet.CheckSum);
	        Byte [] b_id = BitConverter.GetBytes(packet.Identifier);
	        Byte [] b_seq = BitConverter.GetBytes(packet.SequenceNumber);
	        
	        // Console.WriteLine("Serialize type ");
	        Array.Copy( b_type, 0, Buffer, Index, b_type.Length );
	        Index += b_type.Length;
	        
	        // Console.WriteLine("Serialize code ");
	        Array.Copy( b_code, 0, Buffer, Index, b_code.Length );
	        Index += b_code.Length;

	        // Console.WriteLine("Serialize cksum ");
	        Array.Copy( b_cksum, 0, Buffer, Index, b_cksum.Length );
	        Index += b_cksum.Length;

	        // Console.WriteLine("Serialize id ");
	        Array.Copy( b_id, 0, Buffer, Index, b_id.Length );
	        Index += b_id.Length;

	        Array.Copy( b_seq, 0, Buffer, Index, b_seq.Length );
	        Index += b_seq.Length;

	        // copy the data	        
	        Array.Copy( packet.Data, 0, Buffer, Index, PingData );
	        Index += PingData;
	        if( Index != PacketSize/* sizeof(IcmpPacket)  */) {
	            cbReturn = -1;
	            return cbReturn;
	        }

	        cbReturn = Index;
	        return cbReturn;
	    }
		/// <summary>
		///		This Method has the algorithm to make a checksum 
		/// </summary>
		public static UInt16 checksum( UInt16[] buffer, int size )
		{
		    Int32 cksum = 0;
		    int counter;

		    counter = 0;

	        while ( size > 0 ) {

	            UInt16 val = buffer[counter];

	            cksum += Convert.ToInt32( buffer[counter] );
		        counter += 1;
		        size -= 1;
		    }

	        cksum = (cksum >> 16) + (cksum & 0xffff);
		    cksum += (cksum >> 16);
		    return (UInt16)(~cksum);
		}
	} // class ping
	/// <summary>
	///		Class that holds the Pack information
	/// </summary>
	public class IcmpPacket 
	{ 
	    public Byte  Type;    // type of message
	    public Byte  SubCode;    // type of sub code
	    public UInt16 CheckSum;   // ones complement checksum of struct
	    public UInt16 Identifier;      // identifier
	    public UInt16 SequenceNumber;     // sequence number  
	    public Byte [] Data;

	} // class IcmpPacket
}