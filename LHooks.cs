using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hooks;
using TShockAPI;
using Terraria;

namespace LuaPlugin
{
	public class LHooks
	{
		private readonly List<PrioritisedEvent> _initialize = new List<PrioritisedEvent>();
		private readonly List<PrioritisedEvent> _postInitialize = new List<PrioritisedEvent>();
		private readonly List<PrioritisedEvent> _update = new List<PrioritisedEvent>();
		private readonly List<PrioritisedEvent> _postUpdate = new List<PrioritisedEvent>();
		private readonly List<PrioritisedEvent> _getData = new List<PrioritisedEvent>();
		private readonly List<PrioritisedEvent> _greetPlayer = new List<PrioritisedEvent>();
		private readonly List<PrioritisedEvent> _chat = new List<PrioritisedEvent>();
		private readonly List<PrioritisedEvent> _command = new List<PrioritisedEvent>();
		private readonly List<PrioritisedEvent> _join = new List<PrioritisedEvent>();
		private readonly List<PrioritisedEvent> _leave = new List<PrioritisedEvent>();

		public Delegate RegisterHook(HookTypes hookType, EventHandler del, int priority = 0)
		{
			switch (hookType)
			{
				//Done
				case HookTypes.Initialize:
					_initialize.Add(new PrioritisedEvent(del, priority));
					_initialize.Sort(new PrioritisedEventComparer());
					break;

				//Done
				case HookTypes.PostInitialize:
					_postInitialize.Add(new PrioritisedEvent(del, priority));
					_postInitialize.Sort(new PrioritisedEventComparer());
					break;

				//Done
				case HookTypes.PostUpdate:
					_postUpdate.Add(new PrioritisedEvent(del, priority));
					_postUpdate.Sort(new PrioritisedEventComparer());
					break;

				//Done
				case HookTypes.Update:
					_update.Add(new PrioritisedEvent(del, priority));
					_update.Sort(new PrioritisedEventComparer());
					break;

				//Done
				case HookTypes.GetData:
					_getData.Add(new PrioritisedEvent(del, priority));
					_getData.Sort(new PrioritisedEventComparer());
					break;

					//Added
				case HookTypes.GreetPlayer:
					_greetPlayer.Add(new PrioritisedEvent(del, priority));
					_greetPlayer.Sort(new PrioritisedEventComparer());
					break;

				//Done
				case HookTypes.Chat:
					_chat.Add(new PrioritisedEvent(del, priority));
					_chat.Sort(new PrioritisedEventComparer());
					break;

				//Done
				case HookTypes.Command:
					_command.Add(new PrioritisedEvent(del, priority));
					_command.Sort(new PrioritisedEventComparer());
					break;

					//Added
				case HookTypes.Join:
					_join.Add(new PrioritisedEvent(del, priority));
					_join.Sort(new PrioritisedEventComparer());
					break;

				//Done
				case HookTypes.Leave:
					_leave.Add(new PrioritisedEvent(del, priority));
					_leave.Sort(new PrioritisedEventComparer());
					break;
			}
			return del;
		}

		public void OnInit()
		{
			foreach (var prioritisedEvent in _initialize)
			{
				try
				{
					prioritisedEvent.Event.Invoke(this, new EventArgs());
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
					Log.Error(ex.Message);
				}
			}
		}

		public void OnPostInit()
		{
			foreach (var prioritisedEvent in _postInitialize)
			{
				try
				{
					prioritisedEvent.Event.Invoke(this, new EventArgs());
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
					Log.Error(ex.Message);
				}
			}
		}

		public void OnUpdate()
		{
			foreach (var prioritisedEvent in _update)
			{
				try
				{
					prioritisedEvent.Event.Invoke(this, new EventArgs());
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
					Log.Error(ex.Message);
				}
			}
		}

		public void OnPostUpdate()
		{
			foreach (var prioritisedEvent in _postUpdate)
			{
				try
				{
					prioritisedEvent.Event.Invoke(this, new EventArgs());
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
					Log.Error(ex.Message);
				}
			}
		}

		public void OnJoin(int who, HandledEventArgs e)
		{
			foreach (var prioritisedEvent in _join)
			{
				try
				{
					prioritisedEvent.Event.DynamicInvoke(who, e);
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
					Log.Error(ex.Message);
				}
			}
		}

		public void OnGreetPlayer(int who, HandledEventArgs e)
		{
			foreach (var prioritisedEvent in _greetPlayer)
			{
				try
				{
					prioritisedEvent.Event.DynamicInvoke(who, e);
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
					Log.Error(ex.Message);
				}
			}
		}

		public void OnChat(messageBuffer msg, int who, string text, HandledEventArgs e)
		{
			foreach (var prioritisedEvent in _chat)
			{
				try
				{
					prioritisedEvent.Event.DynamicInvoke(msg, who, text, e);
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
					Log.Error(ex.Message);
				}
			}
		}

		public void OnCommand(string cmd, HandledEventArgs e)
		{
			foreach (var prioritisedEvent in _command)
			{
				try
				{
					prioritisedEvent.Event.DynamicInvoke(cmd, e);
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
					Log.Error(ex.Message);
				}
			}
		}

		public void OnLeave(int who)
		{
			foreach (var prioritisedEvent in _leave)
			{
				try
				{
					prioritisedEvent.Event.DynamicInvoke(who);
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
					Log.Error(ex.Message);
				}
			}
		}

		public void OnGetData(GetDataEventArgs args)
		{
			foreach (var prioritisedEvent in _getData)
			{
				try
				{
					prioritisedEvent.Event.DynamicInvoke(args);
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
					Log.Error(ex.Message);
				}
			}
		}
	}

	public enum HookTypes
	{
		//GameHooks
		Initialize,
		PostInitialize,
		PostUpdate,
		Update,
		//NetHooks
		GetData,
		GreetPlayer,
		//ServerHooks
		Chat,
		Command,
		Join,
		Leave
	}

	public class PrioritisedEvent
	{
		public EventHandler Event;
		public int Priority;

		public PrioritisedEvent(EventHandler del, int priority)
		{
			Event = del;
			Priority = priority;
		}
	}

	public class PrioritisedEventComparer : IComparer<PrioritisedEvent>
	{
		public int Compare(PrioritisedEvent x, PrioritisedEvent y)
		{
			return x.Priority.CompareTo(y);
		}
	}
}
