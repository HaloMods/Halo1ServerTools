using System;
using System.Xml;

namespace HaloServerTools
{
	/// <summary>
	/// Summary description for SettingsManager.
	/// </summary>
	public class SettingsManager
	{
		
		private XmlDocument _xmlD;
		private XmlNode _rootNode;

		public class XmlNodeArrayList : System.Collections.ArrayList
		{
			// if i'm not too lazy, make this typesafe later
		}

		public SettingsManager(string filename)
		{
			_xmlD = new XmlDocument();
			_xmlD.Load(filename);
			_rootNode = _xmlD.SelectSingleNode("/*");
		}

		public XmlNodeArrayList GetGroup (string name)
		{
			return GetGroup (name, _rootNode);
		}
		public XmlNodeArrayList GetGroup (string name, XmlNode baseNode)
		{
			XmlNodeArrayList list = new XmlNodeArrayList();
			
			foreach (XmlNode n in baseNode)
			{
				if (n.Name == name) list.Add(n);
			}
			return list;
		}

		public string ReadProperty (string name, XmlNode baseNode)
		{
			if (baseNode.HasChildNodes == false) return null;
			
			foreach (XmlNode n in baseNode)
			{
				if (n.Name == name) return n.InnerText;
			}
			return null;
		}
	}
}
