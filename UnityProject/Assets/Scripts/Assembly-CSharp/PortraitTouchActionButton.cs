using UnityEngine;
using UnityEngine.EventSystems;

public sealed class PortraitTouchActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	private PortraitCombatTouchControls owner;
	private int actionKind;
	public bool IsRightColumn { get; private set; }
	public Vector2 Direction { get; private set; }

	public void Initialize(PortraitCombatTouchControls controls, int kind, bool rightColumn, Vector2 direction)
	{
		owner = controls;
		actionKind = kind;
		IsRightColumn = rightColumn;
		Direction = direction;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (owner) owner.BeginButton(eventData.pointerId, actionKind, Direction);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (owner) owner.EndButton(eventData.pointerId);
	}
}
