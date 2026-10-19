using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
{
	[SerializeField]
	private float handleRange = 1f;

	[SerializeField]
	private float deadZone;

	[SerializeField]
	private AxisOptions axisOptions;

	[SerializeField]
	private bool snapX;

	[SerializeField]
	private bool snapY;

	[SerializeField]
	protected RectTransform background;

	[SerializeField]
	private RectTransform handle;

	private RectTransform baseRect;

	private Canvas canvas;

	private Camera cam;

	private Vector2 input;

	public float Horizontal => snapX ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x;

	public float Vertical => snapY ? SnapFloat(input.y, AxisOptions.Vertical) : input.y;

	public RectTransform Background => background;

	public Vector2 Direction => new Vector2(Horizontal, Vertical);

	public float HandleRange
	{
		get
		{
			return handleRange;
		}
		set
		{
			handleRange = Mathf.Abs(value);
		}
	}

	public float DeadZone
	{
		get
		{
			return deadZone;
		}
		set
		{
			deadZone = Mathf.Abs(value);
		}
	}

	public AxisOptions AxisOptions
	{
		get
		{
			return axisOptions;
		}
		set
		{
			axisOptions = value;
		}
	}

	public bool SnapX
	{
		get
		{
			return snapX;
		}
		set
		{
			snapX = value;
		}
	}

	public bool SnapY
	{
		get
		{
			return snapY;
		}
		set
		{
			snapY = value;
		}
	}

	protected virtual void Start()
	{
		HandleRange = handleRange;
		DeadZone = deadZone;
		baseRect = GetComponent<RectTransform>();
		canvas = GetComponentInParent<Canvas>();
		if (!canvas) Debug.LogError("The Joystick is not placed inside a canvas");
		Vector2 center = new Vector2(.5f, .5f);
		background.pivot = center;
		handle.anchorMin = center;
		handle.anchorMax = center;
		handle.pivot = center;
		handle.anchoredPosition = Vector2.zero;
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		OnDrag(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		cam = null;
		if (canvas.renderMode == RenderMode.ScreenSpaceCamera) cam = canvas.worldCamera;
		Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
		Vector2 radius = background.sizeDelta / 2f;
		input = (eventData.position - position) / (radius * canvas.scaleFactor);
		FormatInput();
		HandleInput(input.magnitude, input.normalized, radius, cam);
		handle.anchoredPosition = input * radius * handleRange;
	}

	protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
	{
		if (magnitude > deadZone)
		{
			if (magnitude > 1f) input = normalised;
		}
		else input = Vector2.zero;
	}

	private void FormatInput()
	{
		if (axisOptions == AxisOptions.Horizontal) input.y = 0f;
		else if (axisOptions == AxisOptions.Vertical) input.x = 0f;
	}

	private float SnapFloat(float value, AxisOptions snapAxis)
	{
		if (value == 0f) return value;
		if (axisOptions == AxisOptions.Both)
		{
			float angle = Vector2.Angle(input, Vector2.up);
			if (snapAxis == AxisOptions.Horizontal)
			{
				if (angle < 22.5f || angle > 157.5f) return 0f;
			}
			else if (snapAxis == AxisOptions.Vertical && angle > 67.5f && angle < 112.5f) return 0f;
		}
		return value > 0f ? 1f : -1f;
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		input = Vector2.zero;
		handle.anchoredPosition = Vector2.zero;
	}

	protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
	{
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out Vector2 localPoint))
		{
			Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
			return localPoint - background.anchorMax * baseRect.sizeDelta + pivotOffset;
		}
		return Vector2.zero;
	}
}
