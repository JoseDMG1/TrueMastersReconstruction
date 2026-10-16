using UnityEngine;

public class HostMessageHandler : MonoBehaviour
{
	public IPConnection iPConnection;

	public LocalMultiplayerData multiplayerData;

	public void SendHostMessage(HostMessage netMessage)
	{
	}

	public string SerializeMessage<T>(HostMessageType type, T payload)
	{
		return null;
	}

	public T DeserializeMessage<T>(string json)
	{
		return default;
	}
}
