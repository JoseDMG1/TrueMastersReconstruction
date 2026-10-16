using Photon.Pun;
using Photon.Realtime;

public class OnlineCustomProperties : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
	public void CharacterSelected()
	{
	}

	public bool AreCharactersSelected()
	{
		return false;
	}

	public void PlayerReadyToBattle()
	{
	}

	public bool ArePlayersReady()
	{
		return false;
	}

	public void CleanCustomProperties()
	{
	}

	public void SetCounter()
	{
	}

	public void SetStage(int stage)
	{
	}

	public void SetPlayerScene(int scene)
	{
	}

	public bool ArePlayersSameScene(int scene)
	{
		return false;
	}
}
