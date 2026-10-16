using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass16_0
	{
		public Task<AuthResult> LoginTask;

		internal bool _003CLogin_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass17_0
	{
		public Task<AuthResult> RegisterTask;

		internal bool _003CRegister_003Eb__0()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass17_1
	{
		public Task ProfileTask;

		internal bool _003CRegister_003Eb__1()
		{
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CLogin_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AuthManager _003C_003E4__this;

		public string _email;

		public string _password;

		private _003C_003Ec__DisplayClass16_0 _003C_003E8__1;

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
		public _003CLogin_003Ed__16(int _003C_003E1__state)
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
	private sealed class _003CRegister_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string _username;

		public AuthManager _003C_003E4__this;

		public string _email;

		public string _password;

		private _003C_003Ec__DisplayClass17_0 _003C_003E8__1;

		private _003C_003Ec__DisplayClass17_1 _003C_003E8__2;

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
		public _003CRegister_003Ed__17(int _003C_003E1__state)
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

	[Header("Firebase")]
	public DependencyStatus dependencyStatus;

	public FirebaseAuth auth;

	public FirebaseUser User;

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

	private void Awake()
	{
	}

	private void InitializeFirebase()
	{
	}

	public void LoginButton()
	{
	}

	public void RegisterButton()
	{
	}

	[IteratorStateMachine(typeof(_003CLogin_003Ed__16))]
	private IEnumerator Login(string _email, string _password)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CRegister_003Ed__17))]
	private IEnumerator Register(string _email, string _password, string _username)
	{
		return null;
	}
}
