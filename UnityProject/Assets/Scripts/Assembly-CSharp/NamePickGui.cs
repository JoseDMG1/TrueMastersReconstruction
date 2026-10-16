using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChatGui))]
public class NamePickGui : MonoBehaviour
{
	private const string UserNamePlayerPref = "NamePickUserName";

	public ChatGui chatNewComponent;

	public InputField idInput;

	public void Start()
	{
	}

	public void EndEditOnEnter()
	{
	}

	public void StartChat()
	{
	}
}
