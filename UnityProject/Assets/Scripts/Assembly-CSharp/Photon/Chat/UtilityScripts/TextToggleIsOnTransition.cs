using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Photon.Chat.UtilityScripts
{
	[RequireComponent(typeof(Text))]
	public class TextToggleIsOnTransition : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		public Toggle toggle;

		private Text _text;

		public Color NormalOnColor;

		public Color NormalOffColor;

		public Color HoverOnColor;

		public Color HoverOffColor;

		private bool isHover;

		public void OnEnable()
		{
		}

		public void OnDisable()
		{
		}

		public void OnValueChanged(bool isOn)
		{
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		public void OnPointerExit(PointerEventData eventData)
		{
		}
	}
}
