using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Android portrait combat controls built as a single-touch action surface.</summary>
public sealed class PortraitCombatTouchControls : MonoBehaviour
{
	private const float ReferenceWidth = 720f;
	private const float ReferenceHeight = 1600f;
	private const float GestureWindow = .22f;
	private const float DodgeDistanceDp = 48f;
	private const float MovementRadiusDp = 104f;
	private const float DodgeDuration = .42f;

	private InputReader inputReader;
	private GameObject canvasObject;
	private CanvasGroup canvasGroup;
	private int activePointerId = int.MinValue;
	private TouchKind activeKind;
	private Vector2 startPosition;
	private float startTime;
	private bool dodgeFinished;
	private Coroutine dodgeRoutine;
	private Coroutine attackRoutine;
	private bool attackWasStarted;
	private bool pointerReleased;
	private float unitsPerDp = 1f;

	private enum TouchKind { None, FreeSurface, BasicAttack, DirectionalAttack, Shield, Special, Dodge }

	public void Initialize(InputReader reader)
	{
		inputReader = reader;
		BuildInterface();
	}

	private void BuildInterface()
	{
		canvasObject = new GameObject("AndroidPortraitCombatControls", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(CanvasGroup));
		var canvas = canvasObject.GetComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = -1;
		var scaler = canvasObject.GetComponent<CanvasScaler>();
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		scaler.referenceResolution = new Vector2(ReferenceWidth, ReferenceHeight);
		scaler.matchWidthOrHeight = .5f;
		float uiScale = Mathf.Max(.1f, Mathf.Min(Screen.width / ReferenceWidth, Screen.height / ReferenceHeight));
		float pixelsPerDp = Screen.dpi > 0f ? Screen.dpi / 160f : uiScale;
		unitsPerDp = pixelsPerDp / uiScale;
		canvasGroup = canvasObject.GetComponent<CanvasGroup>();
		canvasGroup.interactable = true;

		var safeArea = CreateRect("SafeArea", canvasObject.transform);
		var safe = Screen.safeArea;
		safeArea.anchorMin = new Vector2(safe.xMin / Screen.width, safe.yMin / Screen.height);
		safeArea.anchorMax = new Vector2(safe.xMax / Screen.width, safe.yMax / Screen.height);
		safeArea.offsetMin = safeArea.offsetMax = Vector2.zero;

		var surface = CreateRect("FreeMovementSurface", safeArea);
		surface.anchorMin = Vector2.zero;
		surface.anchorMax = new Vector2(1f, .94f);
		surface.offsetMin = surface.offsetMax = Vector2.zero;
		var surfaceImage = surface.gameObject.AddComponent<Image>();
		surfaceImage.color = new Color(0f, 0f, 0f, 0f);
		surfaceImage.raycastTarget = true;
		var touchSurface = surface.gameObject.AddComponent<PortraitTouchSurface>();
		touchSurface.Initialize(this);

		var circle = CreateCircleSprite();
		CreateActionButton(safeArea, circle, "\u2190", TouchKind.DirectionalAttack, new Vector2(-99f * unitsPerDp, 32f * unitsPerDp), 58f * unitsPerDp);
		CreateActionButton(safeArea, circle, "\u2193", TouchKind.DirectionalAttack, new Vector2(-33f * unitsPerDp, 32f * unitsPerDp), 58f * unitsPerDp);
		CreateActionButton(safeArea, circle, "\u2191", TouchKind.DirectionalAttack, new Vector2(33f * unitsPerDp, 32f * unitsPerDp), 58f * unitsPerDp);
		CreateActionButton(safeArea, circle, "\u2192", TouchKind.DirectionalAttack, new Vector2(99f * unitsPerDp, 32f * unitsPerDp), 58f * unitsPerDp);

		CreateActionButton(safeArea, circle, "A", TouchKind.BasicAttack, new Vector2(0f, 32f * unitsPerDp), 64f * unitsPerDp);
		CreateActionButton(safeArea, circle, "B", TouchKind.Shield, new Vector2(0f, 116f * unitsPerDp), 64f * unitsPerDp);
		CreateActionButton(safeArea, circle, "S", TouchKind.Special, new Vector2(0f, 200f * unitsPerDp), 64f * unitsPerDp);
		SetActionButtonsAnchor(safeArea);
	}

	private static void SetActionButtonsAnchor(Transform safeArea)
	{
		for (int i = 0; i < safeArea.childCount; i++)
		{
			var action = safeArea.GetChild(i).GetComponent<PortraitTouchActionButton>();
			if (action && action.IsRightColumn)
			{
				var rect = (RectTransform)action.transform;
				rect.anchorMin = rect.anchorMax = new Vector2(.90f, 0f);
			}
		}
	}

	private void CreateActionButton(Transform parent, Sprite circle, string label, TouchKind kind, Vector2 position, float size)
	{
		bool rightColumn = kind == TouchKind.BasicAttack || kind == TouchKind.Shield || kind == TouchKind.Special;
		var button = CreateRect("TouchButton_" + label, parent);
		button.anchorMin = button.anchorMax = rightColumn ? new Vector2(.90f, 0f) : new Vector2(.42f, 0f);
		button.pivot = new Vector2(.5f, 0f);
		button.anchoredPosition = position;
		button.sizeDelta = new Vector2(size, size);
		var image = button.gameObject.AddComponent<Image>();
		image.sprite = circle;
		image.color = new Color(.04f, .08f, .13f, .72f);
		image.raycastTarget = true;
		var textObject = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
		textObject.transform.SetParent(button, false);
		var textRect = (RectTransform)textObject.transform;
		textRect.anchorMin = Vector2.zero;
		textRect.anchorMax = Vector2.one;
		textRect.offsetMin = textRect.offsetMax = Vector2.zero;
		var text = textObject.GetComponent<Text>();
		text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
		text.text = label;
		text.fontSize = label.Length == 1 ? 38 : 42;
		text.fontStyle = FontStyle.Bold;
		text.alignment = TextAnchor.MiddleCenter;
		text.color = Color.white;
		text.raycastTarget = false;
		var handler = button.gameObject.AddComponent<PortraitTouchActionButton>();
		Vector2 direction = label == "\u2190" ? Vector2.left : label == "\u2192" ? Vector2.right : label == "\u2191" ? Vector2.up : Vector2.down;
		handler.Initialize(this, (int)kind, rightColumn, direction);
	}

	private static RectTransform CreateRect(string name, Transform parent)
	{
		var item = new GameObject(name, typeof(RectTransform));
		item.transform.SetParent(parent, false);
		return (RectTransform)item.transform;
	}

	private static Sprite CreateCircleSprite()
	{
		const int size = 64;
		var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
		texture.name = "AndroidTouchButtonCircle";
		texture.hideFlags = HideFlags.HideAndDontSave;
		for (int y = 0; y < size; y++)
		for (int x = 0; x < size; x++)
		{
			float distance = Vector2.Distance(new Vector2(x, y), new Vector2((size - 1) * .5f, (size - 1) * .5f));
			float alpha = Mathf.Clamp01((size * .48f - distance) * 2f);
			texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
		}
		texture.Apply(false, true);
		var sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(.5f, .5f), size);
		sprite.name = "AndroidTouchButtonCircle";
		sprite.hideFlags = HideFlags.HideAndDontSave;
		return sprite;
	}

	private void Update()
	{
		var game = GameController.i;
		if (game && game.SpecialButton && game.SpecialButton.activeSelf) game.SpecialButton.SetActive(false);
		bool active = game && game.gameStarted && inputReader && !inputReader.blockInputs && Time.timeScale > 0f;
		canvasGroup.blocksRaycasts = active;
		if (!active && activePointerId != int.MinValue) EndAction(activePointerId, true);
	}

	public bool BeginSurface(int pointerId, Vector2 position)
	{
		if (!TryClaim(pointerId, TouchKind.FreeSurface)) return false;
		startPosition = position;
		startTime = Time.unscaledTime;
		dodgeFinished = false;
		SetAxes(Vector2.zero);
		return true;
	}

	public void MoveSurface(int pointerId, Vector2 position)
	{
		if (pointerId != activePointerId || activeKind != TouchKind.FreeSurface) return;
		Vector2 delta = position - startPosition;
		float elapsed = Time.unscaledTime - startTime;
		if (elapsed <= GestureWindow && delta.magnitude >= DpToPixels(DodgeDistanceDp))
		{
			StartDodge(delta.normalized);
			return;
		}
		if (elapsed > GestureWindow)
		{
			float radius = DpToPixels(MovementRadiusDp);
			SetAxes(Vector2.ClampMagnitude(delta / Mathf.Max(1f, radius), 1f));
		}
	}

	public void EndSurface(int pointerId)
	{
		EndAction(pointerId, false);
	}

	public bool BeginButton(int pointerId, int rawKind, Vector2 direction)
	{
		var kind = (TouchKind)rawKind;
		if (!TryClaim(pointerId, kind)) return false;
		var game = GameController.i;
		if (!game || !game.gameStarted) { EndAction(pointerId, true); return false; }

		switch (kind)
		{
			case TouchKind.BasicAttack:
				SetAxes(Vector2.zero);
				inputReader.SetAButton(true);
				attackWasStarted = true;
				game.Attacking(true);
				attackRoutine = StartCoroutine(KeepTapAttackAlive());
				break;
			case TouchKind.DirectionalAttack:
				var mobileInputs = GetMobileInputs(game);
				if (mobileInputs) mobileInputs.suppressTouchMovement = true;
				SetAxes(direction);
				inputReader.SetAButton(true);
				attackWasStarted = true;
				game.Attacking(true);
				attackRoutine = StartCoroutine(KeepTapAttackAlive());
				break;
			case TouchKind.Shield:
				inputReader.SetBButton(true);
				if (game.animMainChar) game.animMainChar.SetBool("Blocking", true);
				break;
			case TouchKind.Special:
				inputReader.SetSButton(true);
				game.Special();
				break;
		}
		return true;
	}

	public void EndButton(int pointerId)
	{
		EndAction(pointerId, false);
	}

	private bool TryClaim(int pointerId, TouchKind kind)
	{
		if (activePointerId != int.MinValue || !inputReader || inputReader.blockInputs || Time.timeScale <= 0f) return false;
		var game = GameController.i;
		if (!game || !game.gameStarted) return false;
		activePointerId = pointerId;
		activeKind = kind;
		pointerReleased = false;
		return true;
	}

	private void StartDodge(Vector2 direction)
	{
		var game = GameController.i;
		if (!game) return;
		activeKind = TouchKind.Dodge;
		SetAxes(direction);
		game.Jumping(true);
		dodgeRoutine = StartCoroutine(StopDodgeAfterDuration());
	}

	private IEnumerator StopDodgeAfterDuration()
	{
		yield return new WaitForSecondsRealtime(DodgeDuration);
		if (activeKind == TouchKind.Dodge)
		{
			SetAxes(Vector2.zero);
			if (GameController.i) GameController.i.Jumping(false);
			dodgeFinished = true;
		}
		dodgeRoutine = null;
	}

	private void EndAction(int pointerId, bool force)
	{
		if (activePointerId == int.MinValue || (!force && activePointerId != pointerId)) return;
		if (!force && (activeKind == TouchKind.BasicAttack || activeKind == TouchKind.DirectionalAttack) && attackRoutine != null)
		{
			pointerReleased = true;
			return;
		}
		CompleteAction();
	}

	private IEnumerator KeepTapAttackAlive()
	{
		int ownerPointer = activePointerId;
		yield return new WaitForSecondsRealtime(.13f);
		if (activePointerId == ownerPointer && (activeKind == TouchKind.BasicAttack || activeKind == TouchKind.DirectionalAttack))
		{
			CompleteAttackPulse();
			if (pointerReleased) CompleteAction();
		}
	}

	private void CompleteAttackPulse()
	{
		attackRoutine = null;
		var game = GameController.i;
		if (attackWasStarted && game) game.Attacking(false);
		attackWasStarted = false;
		SetAxes(Vector2.zero);
		var mobileInputs = GetMobileInputs(game);
		if (mobileInputs) mobileInputs.suppressTouchMovement = false;
	}

	private void CompleteAction()
	{
		var kind = activeKind;
		activePointerId = int.MinValue;
		activeKind = TouchKind.None;
		if (dodgeRoutine != null) { StopCoroutine(dodgeRoutine); dodgeRoutine = null; }
		if (attackRoutine != null) { StopCoroutine(attackRoutine); attackRoutine = null; }
		if (inputReader)
		{
			inputReader.SetAButton(false);
			inputReader.SetBButton(false);
			inputReader.SetSButton(false);
			inputReader.SetXpos(0f);
			inputReader.SetYpos(0f);
		}
		var game = GameController.i;
		if (game)
		{
			if (attackWasStarted) game.Attacking(false);
			if (kind == TouchKind.DirectionalAttack)
			{
				var mobileInputs = GetMobileInputs(game);
				if (mobileInputs) mobileInputs.suppressTouchMovement = false;
			}
			if (kind == TouchKind.Shield && game.animMainChar) game.animMainChar.SetBool("Blocking", false);
			if (kind == TouchKind.Dodge && !dodgeFinished) game.Jumping(false);
		}
		attackWasStarted = false;
		dodgeFinished = false;
		pointerReleased = false;
	}

	private void SetAxes(Vector2 value)
	{
		if (!inputReader) return;
		inputReader.SetXpos(value.x);
		inputReader.SetYpos(value.y);
	}

	private static MobileInputs GetMobileInputs(GameController game)
	{
		return game && game.MainCharacter ? game.MainCharacter.GetComponent<MobileInputs>() : null;
	}

	private static float DpToPixels(float value)
	{
		float scale = Screen.dpi > 0f ? Screen.dpi / 160f : Screen.height / ReferenceHeight;
		return value * Mathf.Max(.75f, scale);
	}

	private void OnApplicationFocus(bool focused)
	{
		if (!focused && activePointerId != int.MinValue) EndAction(activePointerId, true);
	}

	private void OnDestroy()
	{
		if (canvasObject) Destroy(canvasObject);
	}

}
