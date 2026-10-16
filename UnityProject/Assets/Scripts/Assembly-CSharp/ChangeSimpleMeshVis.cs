using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChangeSimpleMeshVis : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CChangeVisibility_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ChangeSimpleMeshVis _003C_003E4__this;

		private float _003CtimeElapsed_003E5__2;

		private Color _003Ccolor_003E5__3;

		private float _003CoriginalOpacity_003E5__4;

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
		public _003CChangeVisibility_003Ed__6(int _003C_003E1__state)
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

	public MeshRenderer mesh;

	private Material mat;

	[Range(0f, 1f)]
	public float visibility;

	public float delay;

	public float duration;

	private void Start()
	{
	}

	[IteratorStateMachine(typeof(_003CChangeVisibility_003Ed__6))]
	public IEnumerator ChangeVisibility()
	{
		return null;
	}
}
