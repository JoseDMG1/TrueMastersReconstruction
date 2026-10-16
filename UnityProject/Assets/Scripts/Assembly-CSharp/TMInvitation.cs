using System;

[Serializable]
public class TMInvitation
{
	public long invitationTime;

	public string receiverType;

	public string hostID;

	public string guestID;

	public string hostUsername;

	public string guestUsername;

	public bool hostReady;

	public bool guestReady;

	public string onlineCode;

	public bool decline;

	public bool inBattle;

	public int stageSelected;
}
