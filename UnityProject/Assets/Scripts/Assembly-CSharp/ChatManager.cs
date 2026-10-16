using ExitGames.Client.Photon;
using Photon.Chat;
using UnityEngine;

public class ChatManager : MonoBehaviour, IChatClientListener
{
	private ChatClient chatclient;

	[SerializeField]
	private string userID;

	public void DebugReturn(DebugLevel level, string message)
	{
	}

	public void OnChatStateChange(ChatState state)
	{
	}

	public void OnConnected()
	{
	}

	public void OnDisconnected()
	{
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
	}

	public void OnPrivateMessage(string sender, object message, string channelName)
	{
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
	}

	public void OnUnsubscribed(string[] channels)
	{
	}

	public void OnUserSubscribed(string channel, string user)
	{
	}

	public void OnUserUnsubscribed(string channel, string user)
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
