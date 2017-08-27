using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading; 

public class IrcBot
{
	public string SERVER;
	private int PORT; 
	// User information defined in RFC 2812 (Internet Relay Chat: Client Protocol) is sent to irc server   
	private string USER;       
	private string NICK; 
	private string CHANNEL; 
	
	public StreamWriter writer;       

	// Constructor
	public IrcBot()
	{
		SERVER = "surreal.shadowfire.org";
		PORT = 6667; 
		USER = "USER hst_test_bot 8 * :I'm a Halo GameInfo relay bot!";       
		NICK = "hst_testbot"; 
		CHANNEL = "#hst_test"; 
	}

	public void Connect()
	{      
		NetworkStream stream;
		TcpClient irc;
		string inputLine;
		StreamReader reader;
		string nickname;              

		// Setup a 15-second ping timer
		System.Timers.Timer t = new System.Timers.Timer(15000);
		t.Elapsed += new System.Timers.ElapsedEventHandler(ping_irc);
		t.Start();
		
		try
		{
			Trace.WriteLine("IRC Bot initialized");
			irc = new TcpClient (SERVER, PORT);
			stream = irc.GetStream ();
			reader = new StreamReader (stream);
			writer = new StreamWriter (stream);                      

			// Start PingSender thread
			//PingSender ping = new PingSender ();
			//ping.Start ();                      

			writer.WriteLine (USER);
			writer.Flush ();
			writer.WriteLine ("NICK " + NICK);
			writer.Flush ();
			//writer.WriteLine ("NICKSERV IDENTIFY 1acbk5359ck161400");
			//writer.Flush ();
			writer.WriteLine ("JOIN " + CHANNEL); Trace.WriteLine("JOIN " + CHANNEL);
			writer.Flush (); 

			while (true)
			{                     
				while ( (inputLine = reader.ReadLine () ) != null )
				{
					//Trace.WriteLine(inputLine);
					if (inputLine.EndsWith ("JOIN :" + CHANNEL) )
					{
						// Parse nickname of person who joined the channel
						nickname = inputLine.Substring(1, inputLine.IndexOf ("!") - 1);
                                             
						// Welcome the nickname to channel by sending a notice
						//writer.WriteLine ("NOTICE " + nickname + " :Hi " + nickname +
						//	" and welcome to " + CHANNEL + " channel!");
						//writer.Flush ();
						// Sleep to prevent excess flood
						Thread.Sleep (2000);
					}
				}                              

				// Close all streams
				writer.Close ();
				reader.Close ();
				irc.Close ();
			}
		}
		catch (Exception e)
		{
			// Show the exception, sleep for a while and try to establish a new connection to irc server
			Trace.WriteLine (e.ToString () );
			Thread.Sleep(5000);
		}
	}

	public void Send(string command)
	{
		writer.WriteLine ("PRIVMSG " + CHANNEL + " " + command);
		writer.Flush ();
	}

	// Sends PING to irc server
	public void ping_irc(object sender, System.Timers.ElapsedEventArgs e)
	{
        string PING = "PING :";
		writer.WriteLine (PING + SERVER);
		writer.Flush ();
	}
}