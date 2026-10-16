using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AvatarSwapper : MonoBehaviour
{
	public Animator animator;

	public List<GameObject> models;

	public List<Avatar> avatars;

	public List<AnimationClip> iddleAnimations;

	private int currentIndex;

	public CharacterEnum currentCharacter;

	private GameObject currentInstance;

	public PlayerController_LM playerController;

	public UnityEvent onPlayerSwapped;

	private void Awake()
	{
	}

	private void Update()
	{
	}

	public void ChangeCharacter(CharacterEnum characterEnum)
	{
	}
}
