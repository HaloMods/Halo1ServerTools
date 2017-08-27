using System;
using System.Data.OleDb; // For Microsoft Access connectivity
using System.Data; // For the DataSet class

namespace HaloServerTools
{
	/// <summary>
	/// Summary description for DB.
	/// </summary>
	public class DB
	{
		public static OleDbConnection Conn;
        
		private static string m_dbPath;
		public static void OpenDatabase(string dbPath)
		{
			m_dbPath = dbPath;
		}
		public static OleDbDataReader GetData(string sql)
		{
			// Open the database connection
			string connString = "Provider=Microsoft.Jet.OLEDB.4.0;"
				+ "Data Source=" + m_dbPath ; // Instantiate the connection
			Conn=new OleDbConnection(connString) ;  
			Conn.Open() ; // Open the connection.

			OleDbCommand cmd = new OleDbCommand(sql, Conn);
			OleDbDataReader dr = cmd.ExecuteReader();
			System.Threading.Thread.Sleep(100);
			return dr;
		}
		public static int RunCommand(string sql)
		{
			// Open the database connection
			string connString = "Provider=Microsoft.Jet.OLEDB.4.0;"
				+ "Data Source=" + m_dbPath ;
			Conn=new OleDbConnection(connString) ;  
			Conn.Open() ; // Open the connection.

			// Create the command object
			OleDbCommand cmd = new OleDbCommand(sql, Conn);
			
			// Execute the SQL command
			return cmd.ExecuteNonQuery();
		}
	}
}
