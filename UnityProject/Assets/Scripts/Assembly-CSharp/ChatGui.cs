using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using UnityEngine;
using UnityEngine.UI;

public class ChatGui : MonoBehaviour, IChatClientListener
{
	public string[] ChannelsToJoinOnConnect;

	public string[] FriendsList;

	public int HistoryLengthToFetch;

	private string selectedChannelName;

	public ChatClient chatClient;

	protected internal ChatAppSettings chatAppSettings;

	public GameObject missingAppIdErrorPanel;

	public GameObject ConnectingLabel;

	public RectTransform ChatPanel;

	public GameObject UserIdFormPanel;

	public InputField InputFieldChat;

	public Text CurrentChannelText;

	public Toggle ChannelToggleToInstantiate;

	public GameObject FriendListUiItemtoInstantiate;

	private readonly Dictionary<string, Toggle> channelToggles;

	private readonly Dictionary<string, FriendItem> friendListItemLUT;

	public bool ShowState;

	public GameObject Title;

	public Text StateText;

	public Text UserIdText;

	private static string HelpText;

	public int TestLength;

	private byte[] testBytes;

	public string UserName { get; set; }

	public void Start()
	{
	}

	public void Connect()
	{
	}

	public void OnDestroy()
	{
	}

	public void OnApplicationQuit()
	{
	}

	public void Update()
	{
	}

	public void OnEnterSend()
	{
	}

	public void OnClickSend()
	{
	}

	private void SendChatMessage(string inputLine)
	{
	}

	public void PostHelpToCurrentChannel()
	{
	}

	public void DebugReturn(DebugLevel level, string message)
	{
	}

	public void OnConnected()
	{
	}

	public void OnDisconnected()
	{
	}

	public void OnChatStateChange(ChatState state)
	{
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
	}

	public void OnSubscribed(string channel, string[] users, Dictionary<object, object> properties)
	{
	}

	private void InstantiateChannelButton(string channelName)
	{
	}

	private void InstantiateFriendButton(string friendId)
	{
	}

	public void OnUnsubscribed(string[] channels)
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

	public void OnUserSubscribed(string channel, string user)
	{
	}

	public void OnUserUnsubscribed(string channel, string user)
	{
	}

	public void OnChannelPropertiesChanged(string channel, string userId, Dictionary<object, object> properties)
	{
	}

	public void OnUserPropertiesChanged(string channel, string targetUserId, string senderUserId, Dictionary<object, object> properties)
	{
	}

	public void OnErrorInfo(string channel, string error, object data)
	{
	}

	public void AddMessageToSelectedChannel(string msg)
	{
	}

	public void ShowChannel(string channelName)
	{
	}

	public void OpenDashboard()
	{
	}
}
