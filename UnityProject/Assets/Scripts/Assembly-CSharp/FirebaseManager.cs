using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass27_0
	{
		public Task<AuthResult> LoginTask;

		internal bool _003CLogin_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass27_1
	{
		public bool logged;

		internal void _003CLogin_003Eb__1(bool status)
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass28_0
	{
		public Task<AuthResult> RegisterTask;

		internal bool _003CRegister_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass28_1
	{
		public Task EmailTask;

		internal bool _003CRegister_003Eb__1()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass29_0
	{
		public Task LoginTask;

		internal bool _003CResetPassword_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass30_0
	{
		public Task<DataSnapshot> DBTask;

		internal bool _003CCheckUsernameDatabase_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass31_0
	{
		public Task DBTaskUsernames;

		internal bool _003CUpdateUsernameDatabase_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass31_1
	{
		public Task DBTask;

		internal bool _003CUpdateUsernameDatabase_003Eb__1()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCheckUsernameDatabase_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string _username;

		private _003C_003Ec__DisplayClass30_0 _003C_003E8__1;

		public FirebaseManager _003C_003E4__this;

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
		public _003CCheckUsernameDatabase_003Ed__30(int _003C_003E1__state)
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
	private sealed class _003CLogin_003Ed__27 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string _email;

		public string _password;

		private _003C_003Ec__DisplayClass27_0 _003C_003E8__1;

		public FirebaseManager _003C_003E4__this;

		private _003C_003Ec__DisplayClass27_1 _003C_003E8__2;

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
		public _003CLogin_003Ed__27(int _003C_003E1__state)
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
	private sealed class _003CRegister_003Ed__28 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FirebaseManager _003C_003E4__this;

		public string _email;

		public string _password;

		private _003C_003Ec__DisplayClass28_0 _003C_003E8__1;

		private _003C_003Ec__DisplayClass28_1 _003C_003E8__2;

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
		public _003CRegister_003Ed__28(int _003C_003E1__state)
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
	private sealed class _003CResetPassword_003Ed__29 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string _email;

		private _003C_003Ec__DisplayClass29_0 _003C_003E8__1;

		public FirebaseManager _003C_003E4__this;

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
		public _003CResetPassword_003Ed__29(int _003C_003E1__state)
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
	private sealed class _003CUpdateUsernameDatabase_003Ed__31 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string _username;

		private _003C_003Ec__DisplayClass31_0 _003C_003E8__1;

		public FirebaseManager _003C_003E4__this;

		private _003C_003Ec__DisplayClass31_1 _003C_003E8__2;

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
		public _003CUpdateUsernameDatabase_003Ed__31(int _003C_003E1__state)
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

	public static FirebaseManager i;

	[Header("Login")]
	public TMP_InputField emailLoginField;

	public TMP_InputField passwordLoginField;

	public TMP_Text warningLoginText;

	public TMP_Text confirmLoginText;

	[Header("Register")]
	public TMP_InputField usernameRegisterField;

	public TMP_InputField emailRegisterField;

	public TMP_InputField passwordRegisterField;

	public TMP_InputField passwordRegisterVerifyField;

	public TMP_Text warningRegisterText;

	[Header("ResetPassword")]
	public TMP_InputField emailResetPassField;

	public TMP_Text warningResetPassText;

	[Header("UserData")]
	public TMP_InputField usernameField;

	public TMP_InputField xpField;

	public TMP_InputField killsField;

	public TMP_InputField deathsField;

	public GameObject scoreElement;

	public Transform scoreboardContent;

	private void Awake()
	{
	}

	public void ClearLoginFeilds()
	{
	}

	public void ClearRegisterFeilds()
	{
	}

	public void LoginButton()
	{
	}

	public void GoToFriendsScene()
	{
	}

	public void RegisterButton()
	{
	}

	public void ResetPasswordButton()
	{
	}

	public void SignOutButton()
	{
	}

	public void SaveDataButton()
	{
	}

	[IteratorStateMachine(typeof(_003CLogin_003Ed__27))]
	public IEnumerator Login(string _email, string _password)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CRegister_003Ed__28))]
	private IEnumerator Register(string _email, string _password)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CResetPassword_003Ed__29))]
	private IEnumerator ResetPassword(string _email)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CCheckUsernameDatabase_003Ed__30))]
	private IEnumerator CheckUsernameDatabase(string _username)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CUpdateUsernameDatabase_003Ed__31))]
	private IEnumerator UpdateUsernameDatabase(string _username)
	{
		return null;
	}

	public void FormatString()
	{
	}

	public void GoToPatreon()
	{
	}

	public void QuestionLoad()
	{
	}
}
