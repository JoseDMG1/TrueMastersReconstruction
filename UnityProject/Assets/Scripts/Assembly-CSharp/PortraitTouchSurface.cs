using UnityEngine;
using UnityEngine.EventSystems;

public sealed class PortraitTouchSurface : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	private PortraitCombatTouchControls owner;

	public void Initialize(PortraitCombatTouchControls controls) { owner = controls; }
	public void OnPointerDown(PointerEventData eventData) { if (owner) owner.BeginSurface(eventData.pointerId, eventData.position); }
	public void OnDrag(PointerEventData eventData) { if (owner) owner.MoveSurface(eventData.pointerId, eventData.position); }
	public void OnPointerUp(PointerEventData eventData) { if (owner) owner.EndSurface(eventData.pointerId); }
}
