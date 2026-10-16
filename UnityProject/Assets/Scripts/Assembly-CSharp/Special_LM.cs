using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Special_LM : MonoBehaviour, IBattleTask
{
	[CompilerGenerated]
	private sealed class _003CDrainSpecialUI_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Special_LM _003C_003E4__this;

		public float duration;

		private float _003Celapsed_003E5__2;

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
		public _003CDrainSpecialUI_003Ed__18(int _003C_003E1__state)
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
	private sealed class _003CFinishAwakening_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float time;

		public PlayerController_LM playerController;

		public Special_LM _003C_003E4__this;

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
		public _003CFinishAwakening_003Ed__10(int _003C_003E1__state)
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

	private Image chargeImg;

	private Text letter;

	private Image outline;

	public TCPConnection tcpConnection;

	public LocalMultiplayerData localData;

	private bool charged;

	public void Init()
	{
	}

	private void OnDestroy()
	{
	}

	public void Reset()
	{
	}

	public void InitAwakening(PlayerController_LM playerController, float time)
	{
	}

	[IteratorStateMachine(typeof(_003CFinishAwakening_003Ed__10))]
	public IEnumerator FinishAwakening(PlayerController_LM playerController, float time)
	{
		return null;
	}

	public void SendDrainSpecial(PlayerController_LM playerController, float time)
	{
	}

	public void GetDrainSpecial(byte[] data)
	{
	}

	public void InitSpecial(PlayerController_LM playerController)
	{
	}

	public void ChargeSpecial(float val, int playerIndex)
	{
	}

	private void UpdateSpecialChargeUI()
	{
	}

	public void DrainSpecial(PlayerController_LM playerController, float time)
	{
	}

	public void ChangeSpecialButtons()
	{
	}

	[IteratorStateMachine(typeof(_003CDrainSpecialUI_003Ed__18))]
	public IEnumerator DrainSpecialUI(float duration)
	{
		return null;
	}

	public void SendSpecialCharges()
	{
	}

	public void GetSpecialCharges(byte[] data)
	{
	}
}
