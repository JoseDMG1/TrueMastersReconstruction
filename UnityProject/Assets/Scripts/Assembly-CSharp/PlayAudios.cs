using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayAudios : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CPlay_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PlayAudios _003C_003E4__this;

		public AudioSource audio;

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
		public _003CPlay_003Ed__7(int _003C_003E1__state)
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

	public AudioSource[] audios;

	public float[] startTimes;

	public int listNumber;

	public int actualAudio;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void PlayNext()
	{
	}

	[IteratorStateMachine(typeof(_003CPlay_003Ed__7))]
	private IEnumerator Play(AudioSource audio)
	{
		return null;
	}
}
