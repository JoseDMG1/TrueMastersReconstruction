using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Attack Data")]
public class AttackData : ScriptableObject
{
	[Header("Basic Info")]
	public ElementType elementType;

	public PowerLevel powerLevel;

	[Header("Custom Actions")]
	public bool jumpable;

	[Header("Game Objects")]
	public GameObject explotion;

	public int GetDamage()
	{
		return 0;
	}

	public int GetDefense()
	{
		return 0;
	}
}
