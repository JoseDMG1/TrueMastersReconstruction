using UnityEngine;

public struct PlayerPosition
{
	public PlayerType playerType;

	public Vector3 position;

	public PlayerPosition(PlayerType type, Vector3 pos)
	{
		playerType = PlayerType.Host;
		position = default;
	}

	public Vector3 GetPosition()
	{
		return default;
	}
}
