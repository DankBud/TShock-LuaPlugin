using System;
using System.ComponentModel;
using System.IO;
using Hooks;
using Terraria;

namespace LuaPlugin
{
	[APIVersion(1, 11)]
	public class LuaPlugin : TerrariaPlugin
	{
		public LuaPlugin(Main game)
			: base(game)
		{
			Order = -10;
		}

		public override string Name
		{
			get { return "Lua Loader"; }
		}

		public override Version Version
		{
			get { return new Version(0, 1); }
		}

		public override string Author
		{
			get { return "The Nyx Team"; }
		}

		public override string Description
		{
			get { return "Adds Lua support for Terraria API"; }
		}

		public override bool Enabled { get; set; }

		private LuaLoader _luaLoader;
		private readonly LGame _lGame = new LGame();
		private readonly LHooks _lHooks = new LHooks();

		public override void Initialize()
		{
			_luaLoader = new LuaLoader(Path.Combine(".", "lua"), _lGame, _lHooks);

			GameHooks.Initialize += new Action(OnInitialize);
			GameHooks.PostInitialize += OnPostInitialize;
			GameHooks.Update += new Action(OnUpdate);
			GameHooks.PostUpdate += new Action(OnPostUpdate);

			ServerHooks.Join += OnJoin;
			ServerHooks.Leave += OnLeave;
			ServerHooks.Chat += OnChat;
			ServerHooks.Command += OnCommand;

			NetHooks.GetData += new NetHooks.GetDataD(OnGetData);
			NetHooks.GreetPlayer += OnGreetPlayer;
		}

		void OnGetData(GetDataEventArgs e)
		{
			_lHooks.OnGetData(e);
		}

		void OnLeave(int who)
		{
			_lHooks.OnLeave(who);
		}

		void OnCommand(string cmd, HandledEventArgs e)
		{
			_lHooks.OnCommand(cmd, e);
		}

		void OnChat(messageBuffer msg, int who, string text, HandledEventArgs e)
		{
			_lHooks.OnChat(msg, who, text, e);
		}

		void OnGreetPlayer(int who, HandledEventArgs e)
		{
			_lHooks.OnGreetPlayer(who, e);
		}

		void OnJoin(int who, HandledEventArgs e)
		{
			_lHooks.OnJoin(who, e);
		}

		void OnPostUpdate()
		{
			_lHooks.OnPostUpdate();
		}

		void OnUpdate()
		{
			_lHooks.OnUpdate();
		}

		void OnInitialize()
		{
			_lHooks.OnInit();
		}

		private void OnPostInitialize()
		{
			_lHooks.OnPostInit();
		}
	}
}