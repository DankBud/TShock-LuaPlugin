using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace LuaPLugin
{
	public class JsonConfig : IConfig
	{
		private Dictionary<string, object> _members = new Dictionary<string, object>();
		private readonly string _file;

		public JsonConfig(string file)
		{
			_file = file;
		}

		public bool Read()
		{
			try
			{
				using (var sr = new StreamReader(_file))
					_members = JsonConvert.DeserializeObject<Dictionary<string, object>>(sr.ReadToEnd());
			}
			catch (FileNotFoundException e)
			{
				return false;
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(e.Message);
				Console.ResetColor();
				return false;
			}
			return true;
		}

		public bool Write()
		{
			try
			{
				var str = JsonConvert.SerializeObject(_members, Formatting.Indented);
				using (var sw = new StreamWriter(_file, false))
					sw.Write(str);
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(e.Message);
				Console.ResetColor();
				return false;
			}
			return true;
		}

		public void AddMember(string key, object o)
		{
			object ob;
			if (!_members.TryGetValue(key, out ob))
				_members.Add(key, o);
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
			_members.TryGetValue(key, out o);
			return o;
		}

		public void EditMember(string key, object o)
		{
			object ob;
			if (_members.TryGetValue(key, out ob))
			{
				_members.Remove(key);
				_members.Add(key, o);
			}
			else
				_members.Add(key, o);
		}
	}
}
