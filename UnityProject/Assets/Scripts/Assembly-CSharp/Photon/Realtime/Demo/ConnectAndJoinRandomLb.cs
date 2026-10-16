using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Realtime.Demo
{
	public class ConnectAndJoinRandomLb : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
	{
		[SerializeField]
		private AppSettings appSettings;

		private LoadBalancingClient lbc;

		private ConnectionHandler ch;

		public Text StateUiText;

		public void Start()
		{
		}

		public void Update()
		{
		}

		public void OnConnected()
		{
		}

		public void OnConnectedToMaster()
		{
		}

		public void OnDisconnected(DisconnectCause cause)
		{
		}

		public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
		}

		public void OnCustomAuthenticationFailed(string debugMessage)
		{
		}

		public void OnRegionListReceived(RegionHandler regionHandler)
		{
		}

		public void OnRoomListUpdate(List<RoomInfo> roomList)
		{
		}

		public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
		{
		}

		public void OnJoinedLobby()
		{
		}

		public void OnLeftLobby()
		{
		}

		public void OnFriendListUpdate(List<FriendInfo> friendList)
		{
		}

		public void OnCreatedRoom()
		{
		}

		public void OnCreateRoomFailed(short returnCode, string message)
		{
		}

		public void OnJoinedRoom()
		{
		}

		public void OnJoinRoomFailed(short returnCode, string message)
		{
		}

		public void OnJoinRandomFailed(short returnCode, string message)
		{
		}

		public void OnLeftRoom()
		{
		}

		private void OnRegionPingCompleted(RegionHandler regionHandler)
		{
		}
	}
}
