using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendsController : MonoBehaviour
{
	[Serializable]
	public class JSONArray<T>
	{
		public T[] Items;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass32_0
	{
		public Task<DataSnapshot> DBTask;

		internal bool _003CSearchFriendDatabase_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass33_0
	{
		public Task<DataSnapshot> DBTask;

		internal bool _003CCheckIfHaveFriend_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass48_0
	{
		public Task<DataSnapshot> DBTask;

		internal bool _003CSearchFriendsDatabase_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass52_0
	{
		public Task<DataSnapshot> DBTask;

		internal bool _003CCheckIfFriendHasOtherInvitation_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass55_0
	{
		public Task<DataSnapshot> DBTask;

		internal bool _003CGetOnlineCode_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCheckIfFriendHasOtherInvitation_003Ed__52 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendsController _003C_003E4__this;

		private _003C_003Ec__DisplayClass52_0 _003C_003E8__1;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CCheckIfFriendHasOtherInvitation_003Ed__52(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CCheckIfHaveFriend_003Ed__33 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string friendID;

		private _003C_003Ec__DisplayClass33_0 _003C_003E8__1;

		public FriendsController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CCheckIfHaveFriend_003Ed__33(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CFillRequestList_003Ed__41 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendsController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CFillRequestList_003Ed__41(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CFillTmFriendsList_003Ed__42 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendsController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CFillTmFriendsList_003Ed__42(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetOnlineCode_003Ed__55 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string userID;

		private _003C_003Ec__DisplayClass55_0 _003C_003E8__1;

		public FriendsController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CGetOnlineCode_003Ed__55(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CInitFriendScene_003Ed__47 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendsController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CInitFriendScene_003Ed__47(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003COrderFriendListCo_003Ed__45 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendsController _003C_003E4__this;

		private List<GameObject> _003CfriendList_003E5__2;

		private List<GameObject>.Enumerator _003C_003E7__wrap2;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003COrderFriendListCo_003Ed__45(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
		}

		private void _003C_003Em__Finally2()
		{
		}

		private void _003C_003Em__Finally3()
		{
		}

		private void _003C_003Em__Finally4()
		{
		}

		private void _003C_003Em__Finally5()
		{
		}

		private void _003C_003Em__Finally6()
		{
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CSearchFriendDatabase_003Ed__32 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendsController _003C_003E4__this;

		public string friendUsername;

		private _003C_003Ec__DisplayClass32_0 _003C_003E8__1;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CSearchFriendDatabase_003Ed__32(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CSearchFriendsDatabase_003Ed__48 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FriendsController _003C_003E4__this;

		private _003C_003Ec__DisplayClass48_0 _003C_003E8__1;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CSearchFriendsDatabase_003Ed__48(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	public static FriendsController i;

	public TMP_InputField inputField;

	public TMP_Text alert;

	public GameObject friendList_UI;

	public GameObject addFriend_UI;

	public GameObject friendRequests_UI;

	public GameObject loading_UI;

	public Image friendList_Btn;

	public Image addFriend_Btn;

	public Image friendRequests_Btn;

	public GameObject invite_Btn;

	public GameObject watchBattle_Btn;

	public ScrollRect scroll;

	[Header("Request")]
	public GameObject friendReqsPanel;

	public GameObject friendReqPrefab;

	[Header("FriendList")]
	public GameObject tmFriendPanel;

	public GameObject tmFriendPrefab;

	private List<TMFriend> completeFriendsList;

	private List<TMFriend> requestFriendsList;

	private List<TMFriend> tmFriendsList;

	public FriendTemplate friendSelected;

	public FriendReqTemplate friendReqSelected;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	public void LoginScreen()
	{
	}

	public void RefreshFriendsList()
	{
	}

	public void SearchFriend()
	{
	}

	public void FriendRequestSent()
	{
	}

	public void QuestionDeleteFriend()
	{
	}

	public void DeleteFriend()
	{
	}

	public void FriendDeleted()
	{
	}

	public void FailToDeleteFriend()
	{
	}

	[IteratorStateMachine(typeof(_003CSearchFriendDatabase_003Ed__32))]
	public IEnumerator SearchFriendDatabase(string friendUsername)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CCheckIfHaveFriend_003Ed__33))]
	public IEnumerator CheckIfHaveFriend(string friendID)
	{
		return null;
	}

	public void CleanUI()
	{
	}

	public void ShowAddFriendUI()
	{
	}

	public void ShowFriendListUI()
	{
	}

	public void ShowfriendRequestsUI()
	{
	}

	public void ShowLoadingUI()
	{
	}

	public void HideLoadingUI()
	{
	}

	public void AcceptFriendRequest()
	{
	}

	[IteratorStateMachine(typeof(_003CFillRequestList_003Ed__41))]
	public IEnumerator FillRequestList()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CFillTmFriendsList_003Ed__42))]
	public IEnumerator FillTmFriendsList()
	{
		return null;
	}

	public void OnFriendSelected()
	{
	}

	public void OrderFriendList()
	{
	}

	[IteratorStateMachine(typeof(_003COrderFriendListCo_003Ed__45))]
	public IEnumerator OrderFriendListCo()
	{
		return null;
	}

	public int SortFriendListPanel(GameObject a, GameObject b)
	{
		return 0;
	}

	[IteratorStateMachine(typeof(_003CInitFriendScene_003Ed__47))]
	public IEnumerator InitFriendScene()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CSearchFriendsDatabase_003Ed__48))]
	public IEnumerator SearchFriendsDatabase()
	{
		return null;
	}

	public List<TMFriend> SearchFriendsInJson(string json)
	{
		return null;
	}

	public void SendInvitation()
	{
	}

	[IteratorStateMachine(typeof(_003CCheckIfFriendHasOtherInvitation_003Ed__52))]
	public IEnumerator CheckIfFriendHasOtherInvitation()
	{
		return null;
	}

	public void GoToFriendConnection()
	{
	}

	public void WatchBattle()
	{
	}

	[IteratorStateMachine(typeof(_003CGetOnlineCode_003Ed__55))]
	private IEnumerator GetOnlineCode(string userID)
	{
		return null;
	}
}
