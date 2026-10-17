using UnityEngine;
using UnityEngine.UI;

public class JoystickSettings : MonoBehaviour
{
	public Toggle dpadToggle;

	public Toggle joystickToggle;

	public Toggle controllerToggle;

	public Toggle invertedToggle;

	private Toggle alternateKeysToggle;

	private bool desktopControls;

	private void Awake()
	{
		desktopControls = !Application.isMobilePlatform;
		if (!desktopControls) return;
		HideObject(dpad);
		HideObject(joystick);
		HideObject(buttonA);
		HideObject(buttonB);
	}

	public GameObject dpad;

	public GameObject joystick;

	public GameObject buttonA;

	public GameObject buttonB;

	public bool inverted;

	private void Start()
	{
		if (desktopControls)
		{
			CreateAlternateKeysToggle();
			HideObject(dpadToggle ? dpadToggle.gameObject : null);
			HideObject(joystickToggle ? joystickToggle.gameObject : null);
			HideObject(controllerToggle ? controllerToggle.gameObject : null);
			HideObject(invertedToggle ? invertedToggle.gameObject : null);
			return;
		}
		if (Photon.Pun.PhotonNetwork.IsConnected && OnlineManager.i && OnlineManager.i.IsSpectator()) return;
		string mode = PlayerPrefs.GetString("Joystick", "Joystick");
		if (mode == "Joystick") JoystickOn();
		else if (mode == "Dpad") DpadOn();
		else if (mode == "Controller") ControllerOn();
		if (PlayerPrefs.GetInt("ControllerInverted", 0) == 1) invertedToggle.isOn = true;
		CreateAlternateKeysToggle();
	}

	private void CreateAlternateKeysToggle()
	{
		if (!invertedToggle || alternateKeysToggle) return;
		var option = Instantiate(invertedToggle.gameObject, invertedToggle.transform.parent, false);
		option.name = "AlternateKeyboardControlsToggle";
		var rect = option.GetComponent<RectTransform>();
		if (rect)
		{
			rect.anchoredPosition = new Vector2(((RectTransform)joystickToggle.transform).anchoredPosition.x, rect.anchoredPosition.y - 76f);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 600f);
		}
		var background = invertedToggle.transform.parent.Find("BG2") as RectTransform;
		if (background)
		{
			background.sizeDelta += Vector2.up * 86f;
			background.anchoredPosition += Vector2.down * 43f;
		}
		alternateKeysToggle = option.GetComponent<Toggle>();
		if (!alternateKeysToggle) { Destroy(option); return; }
		alternateKeysToggle.onValueChanged = new Toggle.ToggleEvent();
		var label = option.GetComponentInChildren<Text>();
		if (label) { label.text = "WASD + ratón"; label.fontSize = 42; }
		alternateKeysToggle.isOn = PlayerPrefs.GetInt("AlternateKeyboardControls", 0) == 1;
		alternateKeysToggle.onValueChanged.AddListener(UseAlternateKeyboardControls);
	}

	public void UseAlternateKeyboardControls(bool alternate)
	{
		LocalControls.SetAlternateActive(alternate);
		if (BattleSceneSingleton.i && BattleSceneSingleton.i.inputReader) BattleSceneSingleton.i.inputReader.ResetControls();
	}

	private static void HideObject(GameObject target)
	{
		if (target) target.SetActive(false);
	}

	public void Invert(bool inv)
	{
		inverted = inv;
		PlayerPrefs.SetInt("ControllerInverted", inv ? 1 : 0);
	}

	public void DpadOn()
	{
		PlayerPrefs.SetString("Joystick", "Dpad");
		dpadToggle.isOn = true;
		joystickToggle.isOn = false;
		controllerToggle.isOn = false;
		dpad.SetActive(true);
		joystick.SetActive(false);
		buttonA.SetActive(true);
		buttonB.SetActive(true);
	}

	public void JoystickOn()
	{
		PlayerPrefs.SetString("Joystick", "Joystick");
		dpadToggle.isOn = false;
		joystickToggle.isOn = true;
		controllerToggle.isOn = false;
		dpad.SetActive(false);
		buttonA.SetActive(true);
		buttonB.SetActive(true);
		joystick.SetActive(true);
	}

	public void ControllerOn()
	{
		PlayerPrefs.SetString("Joystick", "Controller");
		dpadToggle.isOn = false;
		joystickToggle.isOn = false;
		controllerToggle.isOn = true;
		dpad.SetActive(false);
		joystick.SetActive(false);
	}

	private void Update()
	{
		if (desktopControls) return;
		var game = GameController.i;
		if (!game || !game.MainCharacter) return;
		if (BattleSceneSingleton.i.counter && BattleSceneSingleton.i.counter.value > 0f) return;
		if (Input.GetAxis("Start") == 1f) ControllerOn();
		if (PlayerPrefs.GetString("Joystick", "Dpad") == "Dpad") return;
		float horizontal = 0f;
		float vertical = 0f;
		if (PlayerPrefs.GetString("Joystick", "Dpad") == "Controller")
		{
			horizontal = Input.GetAxis("P2Horizontal");
			vertical = Input.GetAxis("P2Vertical");
			if (Input.GetAxis("P3Horizontal") != 0f) horizontal = Input.GetAxis("P3Horizontal");
			if (Input.GetAxis("P3Vertical") != 0f) vertical = Input.GetAxis("P3Vertical");
			if (Input.GetAxis("P4Horizontal") != 0f) horizontal = Input.GetAxis("P4Horizontal");
			if (Input.GetAxis("P4Vertical") != 0f) vertical = Input.GetAxis("P4Vertical");
			if (inverted) vertical = -vertical;
			game.Attacking(Input.GetAxis("Fire1") == 1f);
			game.Jumping(Input.GetAxis("Fire2") == 1f);
			if (Input.GetAxis("Fire3") == 1f) game.Special();
		}
		else if (PlayerPrefs.GetString("Joystick", "dpad") == "Joystick")
		{
			var stick = joystick.GetComponent<VariableJoystick>();
			horizontal = stick.Horizontal;
			vertical = stick.Vertical;
		}
		// Native cardinal routing favors a strong vertical axis, then a strong horizontal one.
		if (vertical > .8f || vertical < -.8f)
		{
			game.UpDown(vertical > .8f ? 1f : -1f);
			game.RightLeft(0f);
			return;
		}
		game.UpDown(vertical > .2f ? 1f : vertical < -.2f ? -1f : 0f);
		if (horizontal > .9f || horizontal < -.9f)
		{
			game.RightLeft(horizontal > .9f ? 1f : -1f);
			game.UpDown(0f);
			return;
		}
		game.RightLeft(horizontal > .2f ? 1f : horizontal < -.2f ? -1f : 0f);
	}
}
