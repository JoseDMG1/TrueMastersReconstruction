using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChangeMeshVisibility : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CChangeVisibility_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ChangeMeshVisibility _003C_003E4__this;

		public float opacity;

		public float duration;

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
		public _003CChangeVisibility_003Ed__5(int _003C_003E1__state)
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

	public SkinnedMeshRenderer mesh;

	public float duration;

	private Material mat;

	private void Start()
	{
	}

	public void VisibilityChange(float opacity)
	{
	}

	[IteratorStateMachine(typeof(_003CChangeVisibility_003Ed__5))]
	public IEnumerator ChangeVisibility(float opacity, float duration = 1f)
	{
		return null;
	}
}
