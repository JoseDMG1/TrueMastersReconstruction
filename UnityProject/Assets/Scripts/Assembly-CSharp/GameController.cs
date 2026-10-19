using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CConfigSpectator_003Ed__37 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameController _003C_003E4__this;

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
		public _003CConfigSpectator_003Ed__37(int _003C_003E1__state)
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
	private sealed class _003CCounterOnline_003Ed__46 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameController _003C_003E4__this;

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
		public _003CCounterOnline_003Ed__46(int _003C_003E1__state)
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
	private sealed class _003CStartCounterWhenTimeServerIs_003Ed__47 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public int timeStart;

		public GameController _003C_003E4__this;

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
		public _003CStartCounterWhenTimeServerIs_003Ed__47(int _003C_003E1__state)
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

	public static GameController i;

	public GameObject resumeButton;

	public Counter counter;

	public GameObject playerPrefab;

	public GameObject enemyPrefab;

	public GameObject hostPlayer;

	public GameObject guestPlayer;

	public GameObject MainCharacter;

	public GameObject OpponentCharacter;

	public bool multiplayer;

	public bool mobile;

	public bool online;

	private bool counterStarted;

	private bool specialUnlocked;

	public bool draw;

	public GameObject SpecialButton;

	public bool practiceMode;

	public GameObject WaitingMsg;

	public Text WaitingMsgText;

	public bool battleStarted;

	private bool pause;

	public bool opponentReadyToAttack;

	public PhotonView PV;

	public Animator animMainChar;

	private MobileInputs mobInputsMainChar;

	public bool notMove;

	public int lastAttackButton;

	public BattleUIManager uiManager;

	public bool gameStarted;

	private void Awake()
	{
		if (!Application.isMobilePlatform && SpecialButton) SpecialButton.SetActive(false);
		// Reconstructed offline branch, native VA 0x12911D0.
		if (SystemManager.i.mode == GameMode.LocalMultiplayer) return;
		if (SystemManager.i.mode == GameMode.Survival)
		{
			var names = CharactersManager.i.characters;
			PlayerPrefs.SetString("CharacterCPU", names[UnityEngine.Random.Range(0, names.Count - 1)]);
		}
		online = SystemManager.i.mode != GameMode.LocalOffline && PhotonNetwork.IsConnected;
		PV = GetComponent<PhotonView>();
		if (i != null && i != this) { Destroy(this); return; }
		i = this;
		if (online) throw new NotSupportedException("Online battle creation is not reconstructed.");
		CreateCharacters_LM();
	}

	public bool IsOutsideStage(Vector3 position, float maxDistance = 27f)
	{
		// 0x1291D48: all three axes participate, including height.
		return Vector3.Distance(Vector3.zero, position) > maxDistance;
	}

	public GameObject GetAttackOwner(int attackLayer)
	{
		return MainCharacter.layer == attackLayer ? MainCharacter : OpponentCharacter;
	}

	public GameObject GetEnemyByOwnerLayer(int attackLayer)
	{
		return MainCharacter.layer == attackLayer ? OpponentCharacter : MainCharacter;
	}

	public float GetDistancesBetweenPlayers()
	{
		return Vector3.Distance(MainCharacter.transform.position, OpponentCharacter.transform.position);
	}

	public void MainMenu()
	{
		// 0x1291F3C.
		LevelChanger.i.FadeToLevel(1);
	}

	public void LoadAds()
	{
		// Ad acquisition is outside the current offline reconstruction.
	}

	private void CreateCharacters_LM()
	{
		// 0x1291684, disconnected branch at 0x129182C. Resource names and
		// component type arguments resolved through exact native metadata slots.
		if (online) throw new NotSupportedException("Online character spawning is not reconstructed.");
		string main = CharactersManager.i.RemoveRandom(PlayerPrefs.GetString("Character"));
		string opponent = CharactersManager.i.RemoveRandom(PlayerPrefs.GetString("CharacterCPU"));
		var mainPrefab = Resources.Load<GameObject>("PhotonPrefabs/" + main);
		var opponentPrefab = Resources.Load<GameObject>("PhotonPrefabs/" + opponent);
		if (!mainPrefab || !opponentPrefab)
			throw new InvalidOperationException("Recovered fighter prefab unavailable: " + main + ", " + opponent);
		hostPlayer = Instantiate(mainPrefab, new Vector3(5, 0, 5), transform.rotation);
		guestPlayer = Instantiate(opponentPrefab, new Vector3(-5, 0, -5), transform.rotation);
		hostPlayer.AddComponent<HostPlayer>();
		guestPlayer.AddComponent<GuestPlayer>();
		if (SystemManager.i.mode == GameMode.LocalOffline)
		{
			var ai = guestPlayer.GetComponent<AI>();
			if (ai) ai.enabled = false;
			guestPlayer.AddComponent<LocalOfflinePlayerInput>();
		}
		var scene = BattleSceneSingleton.i;
		scene.MainCharacter = hostPlayer;
		scene.Opponent = guestPlayer;
		hostPlayer.layer = LayerMask.NameToLayer("Player");
		guestPlayer.layer = LayerMask.NameToLayer("Enemy");
		if (scene.aiToggle) scene.aiToggle.gameObject.SetActive(SystemManager.i.mode != GameMode.LocalOffline);
		if (SystemManager.i.mode == GameMode.SinglePlayer) scene.aiToggle.ChangeVisibility(true);
		MainCharacter = hostPlayer;
		OpponentCharacter = guestPlayer;
		CompleteMainChar();
		if (!online && SystemManager.i.mode != GameMode.LocalOffline) EnableSpecial();
		if (SystemManager.i.mode == GameMode.LocalOffline)
		{
			if (guestPlayer.GetComponent<Description>().moveset == 1)
				foreach (string parameter in new[] { "BasicAttack", "UpAttack", "DownAttack", "LeftAttack", "RightAttack" })
					guestPlayer.GetComponent<Animator>().SetInteger(parameter, 1);
			var splitScreen = gameObject.AddComponent<LocalOfflineSplitScreen>();
			splitScreen.Configure(MainCharacter, OpponentCharacter);
		}
		if (!Application.isMobilePlatform &&
			(SystemManager.i.mode == GameMode.SinglePlayer || SystemManager.i.mode == GameMode.Survival ||
			 SystemManager.i.mode == GameMode.Tournament || SystemManager.i.mode == GameMode.LocalOffline))
		{
			var desktopHud = gameObject.AddComponent<DesktopCombatHud>();
			desktopHud.Configure(MainCharacter, SystemManager.i.mode == GameMode.LocalOffline ? OpponentCharacter : null);
		}
		scene.counter.GetComponent<Animator>().SetTrigger("Start");
	}

	public void SetupCharacters(bool isHost = true)
	{
		// 0x1292354.
		MainCharacter = isHost ? hostPlayer : guestPlayer;
		OpponentCharacter = isHost ? guestPlayer : hostPlayer;
		CompleteMainChar();
	}

	[IteratorStateMachine(typeof(_003CConfigSpectator_003Ed__37))]
	private IEnumerator ConfigSpectator()
	{
		return null;
	}

	[PunRPC]
	public void OnlineRestart()
	{
	}

	public void Restart()
	{
		// 0x128BE04. Tournament draws replay arena; other tournament results
		// return to bracket. CPU/survival return to character selection.
		LevelChanger.i.FadeToLevel(SystemManager.i.mode == GameMode.Tournament ? (draw ? 5 : 6) : 4);
	}

	[PunRPC]
	public void PrepareBattle()
	{
	}

	[PunRPC]
	public void StartBattle()
	{
	}

	public void CreatePlayer(Vector3 position)
	{
	}

	public void EnableInputs()
	{
		// 0x1292888. Exact ownership is the scene's controlled fighter.
		gameStarted = true;
		BattleSceneSingleton.i.MainCharacter.GetComponent<MobileInputs>().enabled = true;
		if (SystemManager.i.mode == GameMode.LocalOffline && OpponentCharacter)
		{
			var secondPlayer = OpponentCharacter.GetComponent<LocalOfflinePlayerInput>();
			if (secondPlayer) secondPlayer.enabled = true;
		}
	}

	[PunRPC]
	public void ReadyToAttack()
	{
		opponentReadyToAttack = true;
	}

	[PunRPC]
	public void StartCounter()
	{
		if (online) throw new NotSupportedException("Server counter synchronization is not reconstructed.");
		BattleSceneSingleton.i.counter.GetComponent<Animator>().SetTrigger("Start");
		if (Notifications.i != null) Notifications.i.Remove();
	}

	[IteratorStateMachine(typeof(_003CCounterOnline_003Ed__46))]
	private IEnumerator CounterOnline()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CStartCounterWhenTimeServerIs_003Ed__47))]
	private IEnumerator StartCounterWhenTimeServerIs(int timeStart)
	{
		return null;
	}

	[PunRPC]
	public void StartCounterNow(int timeStart)
	{
	}

	private void CompleteMainChar()
	{
		// 0x12923C8, generic slots 0x28143D0/0x2816DC0/0x2814830.
		animMainChar = MainCharacter.GetComponent<Animator>();
		mobInputsMainChar = MainCharacter.GetComponent<MobileInputs>();
		if (MainCharacter.GetComponent<Description>().moveset == 1)
			foreach (string parameter in new[] { "BasicAttack", "UpAttack", "DownAttack", "LeftAttack", "RightAttack" })
				animMainChar.SetInteger(parameter, 1);
	}

	public bool IsPlayerWeak()
	{
		// 0x1292B70: preserve the original caller-layer comparison.
		return gameObject.layer == MainCharacter.layer && animMainChar.GetBool("Weak");
	}

	public void EnableSpecial()
	{
		// Offline branch 0x1292FC4-0x1293208. Metadata slots resolve names
		// and preference keys; no fallback ability is invented for other names.
		if (online) throw new NotSupportedException("Online ability visibility is not reconstructed.");
		if (!hostPlayer) throw new InvalidOperationException("Host fighter not available for ability selection.");
		string key;
		switch (hostPlayer.name)
		{
			case "Rina(Clone)": case "Amon(Clone)": key = "Rina"; break;
			case "Dante(Clone)": case "Agata(Clone)": key = "Dante"; break;
			case "Eliot(Clone)": case "Vinea(Clone)": key = "Eliot"; break;
			case "Sora(Clone)": case "Aura(Clone)": key = "Aura"; break;
			case "Diana(Clone)": case "Ren(Clone)": key = "Ren"; break;
			default: return;
		}
		specialUnlocked = PlayerPrefs.HasKey(key);
		if (SpecialButton) SpecialButton.SetActive(Application.isMobilePlatform && specialUnlocked);
	}

	public void DisableSpecial()
	{
		specialUnlocked = false;
		if (SpecialButton) SpecialButton.SetActive(false);
	}

	public bool IsMainChar(int layer)
	{
		return MainCharacter.layer == layer;
	}

	public void EnableMobileInputs()
	{
		hostPlayer = HostPlayer.hostPlayer.gameObject;
		guestPlayer = GuestPlayer.guestPlayer.gameObject;
		hostPlayer.GetComponent<MobileInputs>().enabled = true;
		guestPlayer.GetComponent<MobileInputs>().enabled = true;
	}

	public void EnableDesktopInputs()
	{
		hostPlayer = HostPlayer.hostPlayer.gameObject;
		guestPlayer = GuestPlayer.guestPlayer.gameObject;
		hostPlayer.GetComponent<DesktopInputs>().enabled = true;
		guestPlayer.GetComponent<DesktopInputs>().enabled = true;
	}

	public void Attacking(bool attack)
	{
		if (attack && !DesktopActionPressed(false)) return;
		if (MainCharacter && gameStarted) animMainChar.SetBool("Attack", attack);
	}

	public void AttackingButton(int attackbutton)
	{
		if (!DesktopActionPressed(false)) return;
		if (!MainCharacter) return;
		foreach (string parameter in new[] { "BasicAttack", "UpAttack", "DownAttack", "LeftAttack", "RightAttack" })
			animMainChar.SetInteger(parameter, attackbutton);
		lastAttackButton = attackbutton;
		animMainChar.SetBool("Attack", true);
	}

	public void Special()
	{
		if (LocalOfflinePlayerInput.IsLocalBattle)
		{
			LocalOfflinePlayerInput.TryActivateSpecial(MainCharacter);
			return;
		}
		// 0x1293668. The Stunned parameter gates entry and is set on activation.
		if (specialUnlocked && SpecialAtk.specialAtk && SpecialAtk.specialAtk.charged && MainCharacter &&
			!SpecialAtk.specialAtk.decharging && animMainChar && !animMainChar.GetBool("Stunned"))
		{
			animMainChar.SetTrigger("Special");
			animMainChar.SetBool("Stunned", true);
		}
	}

	public void Jumping(bool jump)
	{
		if (jump && !DesktopActionPressed(true)) return;
		if (MainCharacter && gameStarted) animMainChar.SetBool("Jump", jump);
	}

	private bool DesktopActionPressed(bool dodge)
	{
		var reader = BattleSceneSingleton.i ? BattleSceneSingleton.i.inputReader : null;
		if (!reader || !reader.keyboard || Application.isMobilePlatform) return true;
		// The configured local binding is the authority for desktop keyboard and gamepad input.
		var local = LocalControls.ReadPlayerOne();
		return dodge ? local.b : local.a;
	}

	public void UpDown(float vertical)
	{
		if (MainCharacter) mobInputsMainChar.z = vertical;
	}

	public void RightLeft(float horizontal)
	{
		if (MainCharacter) mobInputsMainChar.x = horizontal;
	}

	public void pauseGame()
	{
		// 0x1293940: offline pause changes simulation time; online only toggles UI.
		if (pause) { ResumeGame(); pause = false; return; }
		pause = true;
		if (SystemManager.i.mode != GameMode.Online) Time.timeScale = 0;
		resumeButton.SetActive(true);
	}

	public void ResumeGame()
	{
		// 0x12939F4 and native table at 0x67E0E0.
		int speed = PlayerPrefs.GetInt("GameSpeed");
		if (speed >= 0 && speed <= 2) Time.timeScale = new[] { 1.15f, 1.275f, 1.4f }[speed];
		resumeButton.SetActive(false);
	}

	public void quitGame()
	{
		Application.Quit();
	}
}
