using UnityEngine;

public class SumonInOpponent : MonoBehaviour
{
	private GameEvents registeredEvents;

	private void Awake()
	{
		registeredEvents = GameEvents.i;
		registeredEvents.onAttackLayerSetUp += SumonInOpPos;
	}

	public void SumonInOpPos(EntityId AtkID)
	{
		if (!gameObject.GetEntityId().Equals(AtkID)) return;
		if (GameController.i.online && !GetComponent<Photon.Pun.PhotonView>().IsMine) return;
		var opponent = gameObject.layer == 9 ? GuestPlayer.guestPlayer.transform : HostPlayer.hostPlayer.transform;
		transform.position = opponent.position;
		registeredEvents.SummonedInOpp(AtkID);
		registeredEvents.onAttackLayerSetUp -= SumonInOpPos;
	}

	private void OnDestroy()
	{
		if (registeredEvents != null) registeredEvents.onAttackLayerSetUp -= SumonInOpPos;
		registeredEvents = null;
	}
}
