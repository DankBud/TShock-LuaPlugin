using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace LuaPLugin
{
	public class XMLConfig : IConfig
	{
		private List<Member> _members = new List<Member>();
		private readonly string _file;

		public XMLConfig(string file)
		{
			_file = file;
		}

		public bool Read()
		{
			try
			{
				var xmlSerializer = new XmlSerializer(typeof (List<Member>));
				using (var sr = new StreamReader(_file))
				{
					var deserialize = xmlSerializer.Deserialize(sr);
					_members = (List<Member>) deserialize;
				}
				return true;
			}
			catch (FileNotFoundException e)
			{
				//File was just created, it's ok :)
				return false;
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(e.Message);
				Console.ResetColor();
				return false;
			}
		}

		public bool Write()
		{
			try
			{
				var xmlSerializer = new XmlSerializer(typeof(List<Member>));
				using (var sw = new StreamWriter(_file, false))
					xmlSerializer.Serialize(sw, _members);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public void AddMember(string key, object o)
		{
			if (_members.Find(k => k.Key == key) == null)
				_members.Add(new Member() {Item = o, Key = key});
			else
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Warning: Attempted to duplicate Member to config: {0}, Ignored adding member", key);
				Console.ResetColor();
			}
		}

		public object ReadMember(string key)
		{
			object o;
			return _members.Find(k => k.Key == key);
		}

		public void EditMember(string key, object o)
		{
			Member m;
			if ((m = _members.Find(k => k.Key == key)) != null)
				m.Item = o;
		}
	}
}
