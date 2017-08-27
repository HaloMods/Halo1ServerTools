using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Text;
using System.Threading;

namespace HaloServerTools
{
	/// <summary>
	/// Summary description for WebServer.
	/// </summary>
	public class WebServer
	{
		#region "Properties"
		
		private IPAddress m_ip;
		private int m_port;
		private Socket m_socListener;
		private Socket m_socWorker;
		private AsyncCallback pfnWorkerCallBack ;
		private MemoryStream m_receiveStream;
		
		#endregion

		#region "Classes"
		
		public class Request
		{
			// A value posted to a WebPage via a form object
			public class RequestForm
			{
			}
			// A value posted to a WebPage via the URL string
			public class RequestQueryString
			{
			}
		}

		public class CSocketPacket
		{
			public System.Net.Sockets.Socket thisSocket;
			public byte[] dataBuffer = new byte[1000000];
		}
		
		#endregion

		/// <summary>
		/// Implements a TCP/IP async sockt based Web Server
		/// </summary>
		/// <param name="ipAddress">The IP Address to bind to</param>
		/// <param name="port">The port to listen on</param>
		public WebServer(string ipAddress, int port)
		{
			m_ip = IPAddress.Parse(ipAddress);
			m_port = port;
			m_receiveStream = new MemoryStream();
		}

		#region "Private Methods"
		#endregion

		#region "Public Methods"
		#endregion

		#region "Delegates"
		public void OnClientConnect(IAsyncResult asyn)
		{
			try
			{
				m_socWorker = m_socListener.EndAccept(asyn);
				WaitForData(m_socWorker);
			}
			catch(ObjectDisposedException)
			{
				System.Diagnostics.Debugger.Log(0,"1","\n OnClientConnection: Socket has been closed\n");
			}
			catch(SocketException se)
			{
				Console.WriteLine(se.Message);
			} 
		}
		public  void OnDataReceived(IAsyncResult asyn)
		{
			try
			{
				CSocketPacket theSockId = (CSocketPacket)asyn.AsyncState ;
				int iRx = theSockId.thisSocket.EndReceive(asyn);
				m_receiveStream.Write(theSockId.dataBuffer, 0, iRx);
				Indy.Sockets.IndyHTTPServer.HTTPServer h = new Indy.Sockets.IndyHTTPServer.HTTPServer();
				

				
				// For Debugging
				//if (m_receiveStream.Length < 2048) Console.WriteLine(System.Text.Encoding.Default.GetString(theSockId.dataBuffer));
				if (iRx > 100)
				{
					BinaryWriter bw = new BinaryWriter(new FileStream("C:\\message.bin", FileMode.Create));
					bw.Write(theSockId.dataBuffer, 0, iRx);
					bw.Close();
				}
				// END
				WaitForData(m_socWorker);
			}
			catch (ObjectDisposedException )
			{
				Console.WriteLine("OnDataReceived: Socket has been closed");
			}
			catch(SocketException se)
			{
				Console.WriteLine(se.Message);
			}
		}
		#endregion

		// Create the listener socket
		public void StartListening()
		{
			try
			{
				m_socListener = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
				m_socListener.Bind(new IPEndPoint(m_ip, m_port));
				m_socListener.Listen(4);
				m_socListener.BeginAccept(new AsyncCallback(OnClientConnect), null);
			}
			catch(SocketException se)
			{
				Console.WriteLine(se.Message);
			}
		}

		private void WaitForData(System.Net.Sockets.Socket soc)
		{
			try
			{
				if (pfnWorkerCallBack == null) pfnWorkerCallBack = new AsyncCallback(OnDataReceived);
				CSocketPacket theSocPkt = new CSocketPacket();
				theSocPkt.thisSocket = m_socWorker;
				
				// Begin listening for data on the socket
				m_socWorker.BeginReceive(
					theSocPkt.dataBuffer,
					0,
					theSocPkt.dataBuffer.Length,
					SocketFlags.None,
					pfnWorkerCallBack,theSocPkt
					);
			}
			catch(SocketException se)
			{
				Console.WriteLine(se.Message);
			}

		}

		
		// Creates receiving and data parsing threads for a socket
		public class HttpSocketHandler
		{
			private Socket _socket;
			private MemoryStream _receiveStream;
			private FormParser fp = new FormParser();

			public HttpSocketHandler(ref Socket socket)
			{
				_socket = socket;
				_receiveStream = new MemoryStream();
				
				// Create the threads
				Thread receiveThread = new Thread(new ThreadStart(ReceiveData));
				receiveThread.Start();
				Thread processThread = new Thread(new ThreadStart(ProcessSocketData));
				processThread.Start();
				
				while ((receiveThread.IsAlive) && (processThread.IsAlive))
				{
				}
			}
			
			// Read data from the socket
			public void ReceiveData()
			{
				int recLength = 512;
				Byte[] bReceive = new Byte[recLength] ;

				fp.SocketOpen = true;
				while (_socket.Connected)
				{
					if (_socket.Poll(0, SelectMode.SelectRead))
					{
						int receiveLength = _socket.Receive(bReceive, 0, recLength, SocketFlags.None);
						_receiveStream.Write(bReceive, 0, receiveLength);
						System.GC.Collect();
					}
				}
				fp.SocketOpen = false;
			}

			// Process the received data on this socket
			public void ProcessSocketData()
			{
				StreamReader sr = new StreamReader(_receiveStream);
				sr.BaseStream.Seek(0, SeekOrigin.Begin);

				string sBuffer = "";

				// Get the HTTP version (ex: "HTTP/1.1") and determine the request type (GET/POST)
				sBuffer = sr.ReadLine();
				string sHttpVersion = sBuffer.Substring(sBuffer.IndexOf("HTTP"),8);
				bool isPost = (sBuffer.Substring(0,4) == "POST");

				// ** NOTE - This is custom tailored to FireFox - A parsing function will need to
				// be added to allow for other browsers
				sBuffer = sr.ReadLine(); // Host
				sBuffer = sr.ReadLine(); // User-Agent:
				sBuffer = sr.ReadLine(); // Accept:
				sBuffer = sr.ReadLine(); // Accept-Language:
				sBuffer = sr.ReadLine(); // Accept-Encoding:
				sBuffer = sr.ReadLine(); // Accept-Charset:
				sBuffer = sr.ReadLine(); // Keep-Alive:
				sBuffer = sr.ReadLine(); // Connection:

				if (isPost)
				{
					sBuffer = sr.ReadLine(); // Content-Type:
 
					// Split this line so that we can get the boundary info
					string[] c = sBuffer.Split(';');
					string boundary = c[1].Replace(" boundary=","");
					boundary = boundary.Replace("\r","");
			
					// Parse the form data
					fp = new FormParser();

					//ArrayList formItems = fp.Decode(sr, boundary);

					// Move back to the beginning of the stream
					_receiveStream.Seek(0, SeekOrigin.Begin);

					// ** Temporary test - save any uploaded files
					/*foreach (object o in formItems)
					{
						FormParser.FormItem fi = (FormParser.FormItem)o;
						if (fi.Filename != null)
						{
							byte [] bFileContent = new byte[fi.DataLength];
							_receiveStream.Seek(fi.DataStartIndex, SeekOrigin.Begin);
							_receiveStream.Read(bFileContent, 0, (int)fi.DataLength);
							BinaryWriter bw = new BinaryWriter(new FileStream("C:\\" + fi.Filename, FileMode.Create));
							bw.Write(bFileContent);
							bw.Close();
						}
					}*/
					sBuffer = sr.ReadLine(); // Content-Length:
					sBuffer = sr.ReadLine(); // ""
					System.GC.Collect(); // Force a memory cleanup in case of a large file.
				}
				else
				{
					sBuffer = sr.ReadLine(); // Content-Length:
					sBuffer = sr.ReadLine(); // ""
				}	
			}
		}
		

		/// <summary>
		/// This function takes FileName as Input and returns the mime type..
		/// </summary>
		/// <param name="sRequestedFile">To indentify the Mime Type</param>
		/// <returns>Mime Type</returns>
		public string GetMimeType(string sRequestedFile)
		{
			String sMimeType = "";
			String sFileExt = "";
			
			// Convert to lowercase
			sRequestedFile = sRequestedFile.ToLower();
			
			int iStartPos = sRequestedFile.IndexOf(".");

			sFileExt = sRequestedFile.Substring(iStartPos);
			
			switch (sFileExt)
			{
				case "htm":
					goto case "html";
				case "php":
					goto case "html";
				case "html":
					sMimeType = "text/html";
					break;
				case "jpg":
					sMimeType = "image/jpeg";
					break;
				case "gif":
					sMimeType = "image/gif";
					break;
				case "":
					break;
			}
			return sMimeType; 
		}


		/// <summary>
		/// This function send the Header Information to the client (Browser)
		/// </summary>
		/// <param name="sHttpVersion">HTTP Version</param>
		/// <param name="sMIMEHeader">Mime Type</param>
		/// <param name="iTotBytes">Total Bytes to be sent in the body</param>
		/// <param name="mySocket">Socket reference</param>
		/// <returns></returns>
		public void SendHeader(string sHttpVersion, string sMIMEHeader, int iTotBytes, string sStatusCode, ref Socket mySocket)
		{

			String sBuffer = "";
			
			// if Mime type is not provided set default to text/html
			if (sMIMEHeader.Length == 0 )
			{
				sMIMEHeader = "text/html";  // Default Mime Type is text/html
			}

			sBuffer = sBuffer + sHttpVersion + sStatusCode + "\r\n";
			sBuffer = sBuffer + "Server: cx1193719-b\r\n";
			sBuffer = sBuffer + "Content-Type: " + sMIMEHeader + "\r\n";
			sBuffer = sBuffer + "Accept-Ranges: bytes\r\n";
			sBuffer = sBuffer + "Content-Length: " + iTotBytes + "\r\n\r\n";
			
			Byte[] bSendData = System.Text.Encoding.ASCII.GetBytes(sBuffer); 

			SendToBrowser( bSendData, ref mySocket);

			Console.WriteLine("Total Bytes : " + iTotBytes.ToString());

		}
		
		
		/// <summary>
		/// Overloaded Function, takes string, convert to bytes and calls 
		/// overloaded sendToBrowserFunction.
		/// </summary>
		/// <param name="sData">The data to be sent to the browser(client)</param>
		/// <param name="mySocket">Socket reference</param>
		public void SendToBrowser(String sData, ref Socket mySocket)
		{
			SendToBrowser (System.Text.Encoding.ASCII.GetBytes(sData), ref mySocket);
		}

		/// <summary>
		/// Sends data to the browser (client)
		/// </summary>
		/// <param name="bSendData">Byte Array</param>
		/// <param name="mySocket">Socket reference</param>
		public void SendToBrowser(Byte[] bSendData, ref Socket mySocket)
		{
			int numBytes = 0;
			
			try
			{
				if (mySocket.Connected)
				{
					if (( numBytes = mySocket.Send(bSendData, bSendData.Length,0)) == -1)
						Console.WriteLine("Socket Error cannot Send Packet");
					else
					{
						Console.WriteLine("No. of bytes send {0}" , numBytes);
					}
				}
				else
					Console.WriteLine("Connection Dropped....");
			}
			catch (Exception  e)
			{
				Console.WriteLine("Error Occurred : {0} ", e );
							
			}
		}

		public void ReadBuffer(ref Socket mySocket)
		{
			int iStartPos = 0;
			String sRequest;
			String sDirName;
			String sRequestedFile;
			String sErrorMessage;
			String sLocalDir="";
			//String sMyWebServerRoot = "C:\\MyWebServerRoot\\";
			String sPhysicalFilePath = "";
			String sFormattedMessage = "";
			String sResponse = "";
			bool bPageExists = false;

			return;

			// Extract the Requested Type and Requested file/directory
			string sBuffer;
			string sHttpVersion;
			sRequest = sBuffer.Substring(0,iStartPos - 1);
										
			//Replace backslash with Forward Slash, if Any
			sRequest.Replace("\\","/");

			//If file name is not supplied add forward slash to indicate 
			//that it is a directory and then we will look for the 
			//default file name..
			if ((sRequest.IndexOf(".") <1) && (!sRequest.EndsWith("/")))
			{
				sRequest = sRequest + "/index"; 
			}

			//Extract the requested file name
			iStartPos = sRequest.LastIndexOf("/") + 1;
			sRequestedFile = sRequest.Substring(iStartPos);
					
			//Extract The directory Name
			sDirName = sRequest.Substring(sRequest.IndexOf("/"), sRequest.LastIndexOf("/")-3);
					
			// Identify the File Name
			if (sRequestedFile.Length == 0 ) sRequestedFile = "index.hst";

			// Ensure that  the requested file is either
			// (a) An actual file, or (b) a virtual page method.
			bPageExists = false;
					
			//Check for methods first

			if (!bPageExists)
			{
				sErrorMessage = "<H2>Error!! No Default File Name Specified</H2>";
				SendHeader(sHttpVersion,  "", sErrorMessage.Length, " 404 Not Found", ref mySocket);
				SendToBrowser ( sErrorMessage, ref mySocket);

				mySocket.Close();
			}
			else
			{
				/////////////////////////////////////////////////////////////////////
				// Get TheMime Type
				/////////////////////////////////////////////////////////////////////
					
				String sMimeType = GetMimeType(sRequestedFile);
	
				//Build the physical path
				sPhysicalFilePath = sLocalDir + sRequestedFile;
				Console.WriteLine("File Requested : " +  sPhysicalFilePath);
					
				//** Code needs to be added to make sure this is a valid page.
				//** Physical files will also be sent from this method
				if (System.IO.File.Exists(sPhysicalFilePath) == false)
				{
					sErrorMessage = "<H2>404 Error! File Does Not Exist...</H2>";
					SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", ref mySocket);
					SendToBrowser( sErrorMessage, ref mySocket);
		
					Console.WriteLine(sFormattedMessage);
				}
				else
				{
					int iTotBytes=0;
	
					sResponse ="";
	
					System.IO.FileStream fs = new System.IO.FileStream(sPhysicalFilePath, FileMode.Open, 	FileAccess.Read, FileShare.Read);
					// Create a reader that can read bytes from the FileStream.
								
					System.IO.BinaryReader reader = new System.IO.BinaryReader(fs);
					byte[] bytes = new byte[fs.Length];
					int read;
					while((read = reader.Read(bytes, 0, bytes.Length)) != 0) 
					{
						// Read from the file and write the data to the network
						sResponse = sResponse + System.Text.Encoding.ASCII.GetString(bytes,0,read);
	
						iTotBytes = iTotBytes + read;
	
					}
					reader.Close(); 
					fs.Close();
	
					SendHeader(sHttpVersion,  sMimeType, iTotBytes, " 200 OK", ref mySocket);
	
					SendToBrowser(bytes, ref mySocket);
							
					//mySocket.Send(bytes, bytes.Length,0);
	
				}
				//mySocket.Close();	
			}
		}
	}
}

