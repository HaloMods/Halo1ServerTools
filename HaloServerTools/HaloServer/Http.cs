/*using System;
using System.Data;
using Indy.Sockets.IndyCustomHTTPServer;
using Indy.Sockets.IndyHTTPServer;
using System.Collections;
using System.IO;

namespace HaloServerTools
{
	/// <summary>
	/// Implements a basic Http server via the indy sockets protocol for the purpose of
	/// receiving external commands from the Game Control Panel.
	/// </summary>
	public class HttpServer
	{
		private HTTPServer FHTTP;
		private string m_rootFolder;
		
		public bool Active
		{
			get { return FHTTP.Active; }
		}
		public int Port
		{
			get { return FHTTP.Bindings[0].Port; }
			set { FHTTP.Bindings[0].Port = value; }
		}
		public string Ip
		{
			get { return FHTTP.Bindings[0].IP;  }
			set { FHTTP.Bindings[0].IP = value; }
		}
		private void FHTTP_OnCommandGet(
			Indy.Sockets.IndyContext.Context AContext,
			HTTPRequestInfo ARequestInfo,
			HTTPResponseInfo AResponseInfo ) 
		{
			// Is this a command or a request for a physical resource
			string docPath = ARequestInfo.Document.ToLower();
			if (docPath.EndsWith(".cmd"))
			{
				// Call the appropriate function
				string s = docPath.Remove(0,1);
				s = s.Replace(".cmd","");
				switch (s)
				{
					case "saystuff":
						((HaloServer)ServerManager.ServerList[0])._serverExtensions._logInterpreter.script.ServerSay("This is a test");
						break;
					default:
					// If module not found
					AResponseInfo.ResponseNo = 404;
					AResponseInfo.ContentText = "<h1>404</h1><br><b>Specified module is invalid.</b>";
					break;
				}
			}
			else
			{
				// If the file exists, send it, else return 404
				if (File.Exists(m_rootFolder + docPath.Replace("/","\\")))
				{
					string mimeType;
					
					switch (docPath.Substring(docPath.Length-4,4))
					{
						case ".htm":
							goto case "html";
						case "html":
							mimeType = "text/html";
							break;
						case ".xml":
							mimeType = "text/xml";
							break;
						case ".gif":
							mimeType = "image/gif";
							break;
						case ".jpg":
							goto case "jpeg";
						case "jpeg":
							mimeType = "image/jpeg";
							break;
						default:
							mimeType = "application/octet-stream";
							break;
					}

					AResponseInfo.ContentType = mimeType;
					AResponseInfo.ServeFile(AContext, m_rootFolder + docPath);
				}
				else
				{
					AResponseInfo.ResponseNo = 404;
					AResponseInfo.ContentText = "<h1>404 - File not found</h1>";					
				}
			}
			System.GC.Collect();
		}

		public HttpServer(string rootFolder) 
		{
			FHTTP = new HTTPServer();
			
			// Add the network bindings
			//FHTTP.Bindings.Add();
			//FHTTP.Bindings[0].IP = "192.168.0.55";
			//FHTTP.Bindings[0].Port = 1337;
			//FHTTP.Bindings[0].UpdateBindingLocal();
			FHTTP.DefaultPort = 1337;
			m_rootFolder = rootFolder;
		}

		public void StartListening()
		{
			FHTTP.OnCommandGet += new TIdHTTPGetEvent(FHTTP_OnCommandGet);
			FHTTP.Active = true;
		}
		public void StopListening()
		{
			FHTTP.Active = false;
		}

		~HttpServer() 
		{
			FHTTP.Active = false;
		}
	}
}

*/