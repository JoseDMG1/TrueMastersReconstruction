using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PrisonController : MonoBehaviour
{
	private bool moveFinished;

	private bool trap;

	private int playerTrap;

	public List<GameObject> walls;

	public float delayRevert = 1f;

	private GameEvents registeredEvents;

	private void Awake()
	{
		var controller = GameController.i;
		if (controller.MainCharacter.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PrisonFree")
			|| controller.OpponentCharacter.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PrisonFree"))
		{
			GetComponent<DestroyOnCollision>().DeleteObj();
			return;
		}
		if (controller.online && !GetComponent<PhotonView>().IsMine) return;
		registeredEvents = GameEvents.i;
		registeredEvents.OnPlayerPrisonHit += DestroyPrisonHit;
		registeredEvents.OnPlayerPrisonTrap += PrisonTrap;
		registeredEvents.OnPlayerDamaged += DestroyPrison;
		Invoke(nameof(StartMoves), 1f);
		Invoke(nameof(FinishMove), delayRevert);
	}

	private void OnDestroy()
	{
		CancelInvoke();
		// Unregister from the exact event owner retained at subscription time.
		if (registeredEvents == null) return;
		registeredEvents.OnPlayerDamaged -= DestroyPrison;
		registeredEvents.OnPlayerPrisonHit -= DestroyPrisonHit;
		registeredEvents.OnPlayerPrisonTrap -= PrisonTrap;
		registeredEvents = null;
	}

	private void DestroyPrison(int playerLayer)
	{
		if (playerLayer != gameObject.layer && trap) DestroyAllPrison();
	}

	[PunRPC]
	public void DestroyPrisonOnline()
	{
		GetComponent<DestroyOnCollision>().DeleteObj();
	}

	private void DestroyPrisonHit(int playerLayer)
	{
		// Native 0x12780A8 is a tail branch, not a RET; no layer filter.
		DestroyAllPrison();
	}

	private void DestroyAllPrison()
	{
		GetComponent<DestroyOnCollision>().DeleteObj();
		if (GameController.i.online)
			Debug.LogWarning("Network prison deletion is unavailable.", this);
	}

	private void FinishMove()
	{
		if (!trap) RevertWalls();
		GetComponent<BoxCollider>().enabled = false;
	}

	private void PrisonTrap(int playerLayer)
	{
		playerTrap = playerLayer;
		trap = true;
	}

	public void StartMoves()
	{
		if (GetComponent<ConfigAttackLayer>().playerWeak)
		{
			Destroy(gameObject);
			return;
		}
		foreach (var wall in walls) wall.GetComponents<MoveObject>()[0].enabled = true;
		if (GameController.i.online)
			Debug.LogWarning("Network prison movement is unavailable.", this);
	}

	[PunRPC]
	public void StartMovesOnline()
	{
		foreach (var wall in walls) wall.GetComponents<MoveObject>()[0].enabled = true;
	}

	public void RevertWalls()
	{
		Debug.Log("Revert Walls");
		if (GameController.i.online)
			Debug.LogWarning("Network prison movement is unavailable.", this);
		GetComponent<BoxCollider>().enabled = false;
		foreach (var wall in walls)
		{
			var movements = wall.GetComponents<MoveObject>();
			movements[0].enabled = false;
			movements[1].enabled = true;
		}
		Destroy(gameObject, 2.5f);
	}

	[PunRPC]
	public void RevertWallsOnline()
	{
		GetComponent<BoxCollider>().enabled = false;
		foreach (var wall in walls)
		{
			var movements = wall.GetComponents<MoveObject>();
			movements[0].enabled = false;
			movements[1].enabled = true;
		}
		Destroy(gameObject, 1f);
	}

	[PunRPC]
	public void OnlineDestroy()
	{
		Destroy(gameObject);
	}
}
