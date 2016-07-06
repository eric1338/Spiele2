using DemoScene.Utils;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene
{
	class InputManager
	{

		private Dictionary<Key, UserAction> _prolongedUserActionKeyMap = new Dictionary<Key, UserAction>();
		private Dictionary<Key, UserAction> _singleUserActionKeyMap = new Dictionary<Key, UserAction>();

		private Dictionary<UserAction, bool> ProlongedUserActions = new Dictionary<UserAction, bool>();
		private Queue<UserAction> SingleUserActions = new Queue<UserAction>();

		public InputManager()
		{

		}

		public void ProcessKeyUp(Key key)
		{
			if (_prolongedUserActionKeyMap.ContainsKey(key))
			{
				ProlongedUserActions[_prolongedUserActionKeyMap[key]] = false;
			}
		}

		public void ProcessKeyDown(Key key)
		{
			if (_prolongedUserActionKeyMap.ContainsKey(key))
			{
				ProlongedUserActions[_prolongedUserActionKeyMap[key]] = true;
			}

			if (_singleUserActionKeyMap.ContainsKey(key))
			{
				SingleUserActions.Enqueue(_singleUserActionKeyMap[key]);
			}
		}

		public bool IsUserActionActive(UserAction userAction)
		{
			return ProlongedUserActions.ContainsKey(userAction) && ProlongedUserActions[userAction];
		}

		public List<UserAction> GetSingleUserActionsAsList()
		{
			List<UserAction> singleUserActions = SingleUserActions.ToList();
			SingleUserActions.Clear();

			return singleUserActions;
		}

		public void AddProlongedUserActionMapping(Key key, UserAction userAction)
		{
			_prolongedUserActionKeyMap.Add(key, userAction);
		}

		public void AddSingleUserActionMapping(Key key, UserAction userAction)
		{
			_singleUserActionKeyMap.Add(key, userAction);
		}
	}
}
