using System;
using System.Collections;
using System.IO;

namespace HaloServerTools
{
	/// <summary>
	/// Summary description for Form Parser.
	/// </summary>
	public class FormParser
	{
		public bool SocketOpen;

		public class FormItem
		{
			public string Name;
			public string Filename;
			public int DataStartIndex;			
			public int DataLength;
			public bool IsFile
			{
				get
				{
					if (Filename.Length > 0) return true;
					return false;
				}
			}
			public FormItem()
			{
				Name = "";
				Filename = "";
				DataStartIndex = 0;
				DataLength = 0;
			}
			public string GetString(string sourceData)
			{
				return sourceData.Substring(DataStartIndex, DataLength);
			}
			public byte[] GetBytes(string sourceData)
			{
				return System.Text.Encoding.Default.GetBytes(
					sourceData.Substring(DataStartIndex, DataLength));
			}
			public void SaveToFile(string sourceData, string path)
			{
				if (Filename != null)
				{
					System.IO.FileInfo fi = new FileInfo(Filename);
					BinaryWriter bw = new BinaryWriter(new FileStream(path + fi.Name, FileMode.Create));
					
					// Write the file to disk in chunks of 512k
					int chunkSize = 1024 * 512;
					byte[] chunk = new byte[chunkSize];
					
					for (int x=0; x<DataLength; x+=chunkSize)
					{
						if ((x + chunkSize) < DataLength)
						{
							bw.Write(System.Text.Encoding.Default.GetBytes(
								sourceData.Substring(DataStartIndex + x, chunkSize)));
						}
						else
						{
							bw.Write(System.Text.Encoding.Default.GetBytes(
								sourceData.Substring(DataStartIndex + x, DataLength - x)));
							break;
						}
						System.GC.Collect();
					}
					bw.Close();
				}
			}
		}

		public FormParser()
		{
		}

		// Implements a type-safe searchable collection of FormItem objects
		public class FormItemCollection : System.Collections.DictionaryBase
		{
			public FormItem this[string formItemName]
			{
				get { return ((FormItem)(Dictionary[formItemName])); }
				set { Dictionary[formItemName] = value; }
			}
			public void Add(string formItemName, FormItem formItem)
			{
				Dictionary.Add(formItemName, formItem);
			}
			public void Remove(string formItemName)
			{
				Dictionary.Remove(formItemName);
			}
			public bool Contains(string formItemName)
			{
				return Dictionary.Contains(formItemName);
			}
		}

		// Parse form data and return an array of FormItems objects
		public FormItemCollection Decode(string postData, string boundary)
		{
			string s = "";
			StringReader postStream = new StringReader(ref postData);

			boundary = "--" + boundary;
			
			FormItemCollection formItems = new FormItemCollection();

			s = postStream.ReadLine();
			
			while (postStream.Position < postStream.Length)
			{
				if (s == (boundary + "--")) break;	// End of the form data
				if (s == boundary)
				{

					// Here's the beginning of a form value
					FormItem f = new FormItem();
					s = postStream.ReadLine();
				
					string[] itemData = s.Split(';');
					f.Name = itemData[1].Replace(" name=","");
					f.Name = f.Name.Replace("\"","");
										
					// See if we have any additional form data
					if (itemData.Length == 3)
					{
						f.Filename = (itemData[2].Replace(" filename=","")).Replace("\"","");
						
						// Skip the next two lines;
						s = postStream.ReadLine();
						s = postStream.ReadLine();
					}
					else
					{
						// Skip one line
						s = postStream.ReadLine();
					}
				
					// Parse the data of this form item
					f.DataStartIndex = postStream.Position;
					do
					{
						s = postStream.ReadLine();
					} while ((postStream.Position < postStream.Length)
						&& (!(s.StartsWith(boundary))));
				
					// Find the Data index and length based on current stream position
					f.DataLength = ((postStream.Position - f.DataStartIndex) - s.Length)-4; // Disregard the terminating 0x0d0a2d2d
					formItems.Add(f.Name, f); // Add to the return array
				}
			}
			return formItems;
		}

		public class StringReader
		{
			private string s;
			public int Position;
			public int Length;
			public StringReader(ref string str)
			{
				Position = 0;
				s = str;
				Length = s.Length;
			}
			public string ReadLine()
			{
				int lineEnd = s.IndexOf("\r\n",Position)+2;
				string r = s.Substring(Position, lineEnd-Position);
				Position = lineEnd;
				return r.Replace("\r\n","");
			}
		}
	}
}
