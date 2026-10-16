using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;

public class VoiceChatManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CInitVoiceChat_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public VoiceChatManager _003C_003E4__this;

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
		public _003CInitVoiceChat_003Ed__9(int _003C_003E1__state)
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

	public GameObject alert;

	public GameObject micOn;

	public GameObject micOff;

	public GameObject loading;

	public PlayerVoice playerVoice;

	public bool playerVoiceCreated;

	public List<PlayerVoice> voices;

	private void Start()
	{
	}

	public void ShowIcon(string icon)
	{
	}

	[IteratorStateMachine(typeof(_003CInitVoiceChat_003Ed__9))]
	private IEnumerator InitVoiceChat()
	{
		return null;
	}

	public void CreatePlayerVoice()
	{
	}

	public void StartRecording()
	{
	}

	public void StopRecording()
	{
	}

	[PunRPC]
	public void ShowAlert()
	{
	}

	[PunRPC]
	public void RemoveAlert()
	{
	}

	public void ToggleVoiceChat()
	{
	}

	public void VoiceChatOn()
	{
	}

	public void VoiceChatOff()
	{
	}

	internal void PermissionCallbacks_PermissionGranted(string permissionName)
	{
	}

	public void AddToVoices(PlayerVoice voice)
	{
	}

	public void MuteAll()
	{
	}

	public void UnMuteAll()
	{
	}
}
