using UnityEngine;

public class MenuController_LM : MonoBehaviour, IBattleTask
{
	public TCPConnection tcpConnection;

	public BattleTasks_LM battleTasks_LM;

	private bool playAgainClicked;

	public void Init()
	{
	}

	private void OnDestroy()
	{
	}

	public void Reset()
	{
	}

	private void OnPlayAgainClicked()
	{
	}

	private void SendPlayAgainMsg()
	{
	}

	private void GetPlayAgainMsg(byte[] data)
	{
	}
}
