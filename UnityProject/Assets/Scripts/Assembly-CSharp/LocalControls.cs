using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class LocalControlProfile
{
    public string up = "UpArrow", down = "DownArrow", left = "LeftArrow", right = "RightArrow";
    public string attack = "Z", jump = "X", special = "C";
    public int upButton = -1, downButton = -1, leftButton = -1, rightButton = -1;
    public int attackButton = 0, jumpButton = 1, specialButton = 3;
}

[Serializable]
public class LocalPlayerControls
{
    public bool useGamepad;
    // False for profiles saved before the device-mode toggle was introduced.
    // In that case Player 1 can select a connected Xbox pad automatically.
    public bool inputModeConfigured;
    public bool useAlternate;
    public int joystick = 1;
    public LocalControlProfile defaults = new LocalControlProfile();
    public LocalControlProfile alternate = new LocalControlProfile { up = "W", down = "S", left = "A", right = "D", attack = "Mouse0", jump = "Space", special = "Mouse1" };
}

[Serializable]
public class LocalControlsData
{
    public LocalPlayerControls player1 = new LocalPlayerControls();
    public LocalPlayerControls player2 = new LocalPlayerControls { joystick = 2, defaults = new LocalControlProfile { up = "I", down = "K", left = "J", right = "L", attack = "U", jump = "O", special = "P" }, alternate = new LocalControlProfile { up = "UpArrow", down = "DownArrow", left = "LeftArrow", right = "RightArrow", attack = "O", jump = "P", special = "RightShift" } };
    public bool alternateActive;
}

/// <summary>Independent keyboard and per-device gamepad bindings for local players.</summary>
public static class LocalControls
{
    private const string PrefKey = "LocalControls.v1";
    private static LocalControlsData cached;
    public static LocalControlsData Data
    {
        get
        {
            if (cached != null) return cached;
            string json = PlayerPrefs.GetString(PrefKey, "");
            cached = string.IsNullOrEmpty(json) ? new LocalControlsData() : JsonUtility.FromJson<LocalControlsData>(json);
            if (cached == null) cached = new LocalControlsData();
            if (string.IsNullOrEmpty(json)) cached.alternateActive = PlayerPrefs.GetInt("AlternateKeyboardControls", 0) == 1;
            if (cached.player1 == null) cached.player1 = new LocalPlayerControls();
            if (cached.player2 == null) cached.player2 = new LocalPlayerControls { joystick = 2 };
            if (string.IsNullOrEmpty(json)) cached.player1.useAlternate = cached.alternateActive;
            else if (cached.alternateActive && !cached.player1.useAlternate) cached.player1.useAlternate = true;
            return cached;
        }
    }
    public static void Save()
    {
        PlayerPrefs.SetString(PrefKey, JsonUtility.ToJson(Data));
        PlayerPrefs.SetInt("AlternateKeyboardControls", Data.alternateActive ? 1 : 0);
        PlayerPrefs.Save();
    }
    public static void SetAlternateActive(bool active)
    {
        Data.alternateActive = active;
        Data.player1.useAlternate = active;
        Save();
    }
    public static LocalControlProfile Profile(int player)
    {
        LocalPlayerControls p = player == 1 ? Data.player2 : Data.player1;
        if (p == null) p = new LocalPlayerControls();
        if (p.defaults == null) p.defaults = new LocalControlProfile();
        if (p.alternate == null) p.alternate = new LocalControlProfile();
        return p.useAlternate ? p.alternate : p.defaults;
    }
    public static InputSet ReadPlayerOne() { return ReadPlayer(0); }
    public static InputSet ReadPlayer(int player)
    {
        var p = player == 1 ? Data.player2 : Data.player1;
        if (player == 0 && !p.inputModeConfigured && !p.useGamepad &&
            WindowsXInput.IsConnected(Mathf.Clamp(p.joystick, 1, 8)))
        {
            p.useGamepad = true;
            Save();
        }
        var map = Profile(player);
        var value = new InputSet();
        if (p.useGamepad)
        {
            int id = Mathf.Clamp(p.joystick, 1, 8);
            // Unity's legacy joystick polling can enumerate an XInput pad by
            // name while returning zero axes/buttons for it on some drivers.
            // Read Xbox pads through XInput first, retaining the configured
            // Unity joystick axes/buttons as a fallback for other controllers.
            bool nativePad = WindowsXInput.TryRead(id, out float padX, out float padY, out ushort padButtons);
            value.x = nativePad ? padX : Input.GetAxisRaw("LocalP" + id + "Horizontal");
            value.y = nativePad ? padY : Input.GetAxisRaw("LocalP" + id + "Vertical");
            // Button bindings supplement the stick; an idle D-pad must never
            // replace a nonzero analog axis with zero.
            value.x = CombinePadDirection(value.x,
                map.leftButton >= 0 && BoundPadButton(id, map.leftButton, nativePad, padButtons),
                map.rightButton >= 0 && BoundPadButton(id, map.rightButton, nativePad, padButtons));
            value.y = CombinePadDirection(value.y,
                map.downButton >= 0 && BoundPadButton(id, map.downButton, nativePad, padButtons),
                map.upButton >= 0 && BoundPadButton(id, map.upButton, nativePad, padButtons));
            value.a = BoundPadButton(id, map.attackButton, nativePad, padButtons);
            value.b = BoundPadButton(id, map.jumpButton, nativePad, padButtons);
            value.s = BoundPadButton(id, map.specialButton, nativePad, padButtons);
        }
        else
        {
            value.x = (Key(map.right) ? 1f : 0f) - (Key(map.left) ? 1f : 0f);
            value.y = (Key(map.up) ? 1f : 0f) - (Key(map.down) ? 1f : 0f);
            value.a = Key(map.attack); value.b = Key(map.jump); value.s = Key(map.special);
        }
        return value;
    }
    private static float CombinePadDirection(float axis, bool negative, bool positive)
    {
        float digital = (positive ? 1f : 0f) - (negative ? 1f : 0f);
        return digital != 0f ? digital : axis;
    }
    public static bool Key(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        if (name == "Mouse0") return Input.GetMouseButton(0);
        if (name == "Mouse1") return Input.GetMouseButton(1);
        if (name == "Mouse2") return Input.GetMouseButton(2);
        KeyCode key;
        return Enum.TryParse(name, out key) && Input.GetKey(key);
    }
    public static bool PadButton(int joystick, int button)
    {
        KeyCode key;
        return Enum.TryParse("Joystick" + joystick + "Button" + Mathf.Clamp(button, 0, 19), out key) && Input.GetKey(key);
    }
    public static bool PadButtonDown(int joystick, int button)
    {
        if (WindowsXInput.TryGetButtonDown(joystick, button)) return true;
        KeyCode key;
        return Enum.TryParse("Joystick" + joystick + "Button" + Mathf.Clamp(button, 0, 19), out key) && Input.GetKeyDown(key);
    }
    public static Vector2 ReadPadAxes(int joystick)
    {
        if (WindowsXInput.TryRead(joystick, out float x, out float y, out ushort _)) return new Vector2(x, y);
        return new Vector2(Input.GetAxisRaw("LocalP" + joystick + "Horizontal"), Input.GetAxisRaw("LocalP" + joystick + "Vertical"));
    }
    public static bool TryGetPressedPadButton(int joystick, out int button)
    {
        if (WindowsXInput.TryRead(joystick, out float _, out float _, out ushort nativeButtons))
            return WindowsXInput.TryFindPressedButton(nativeButtons, out button);
        for (int i = 0; i < 20; i++)
        {
            if (PadButton(joystick, i)) { button = i; return true; }
        }
        button = -1;
        return false;
    }
    public static bool IsGamepadConnected(int joystick)
    {
        if (WindowsXInput.IsConnected(joystick)) return true;
        string[] names = ConnectedJoysticks();
        return joystick > 0 && joystick <= names.Length && !string.IsNullOrEmpty(names[joystick - 1]);
    }
    public static string GamepadButtonLabel(int button)
    {
        switch (button)
        {
            case 0: return "A"; case 1: return "B"; case 2: return "X"; case 3: return "Y";
            case 4: return "LB"; case 5: return "RB"; case 6: return "Back"; case 7: return "Start";
            case 8: return "L3"; case 9: return "R3"; case 10: return "Arriba"; case 11: return "Abajo";
            case 12: return "Izquierda"; case 13: return "Derecha"; default: return "Boton " + button;
        }
    }
    private static bool BoundPadButton(int joystick, int button, bool nativePad, ushort nativeButtons)
    {
        return nativePad ? WindowsXInput.IsPressed(nativeButtons, button) : PadButton(joystick, button);
    }
    public static string[] ConnectedJoysticks() { return Input.GetJoystickNames(); }

    /// <summary>Direct XInput fallback for Xbox controllers on Windows desktop.</summary>
    private static class WindowsXInput
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        [StructLayout(LayoutKind.Sequential)]
        private struct GamepadState
        {
            public ushort buttons;
            public byte leftTrigger, rightTrigger;
            public short leftX, leftY, rightX, rightY;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct State { public uint packet; public GamepadState gamepad; }
        [DllImport("xinput1_4.dll", EntryPoint = "XInputGetState", ExactSpelling = true)]
        private static extern uint GetState14(uint userIndex, out State state);
        [DllImport("xinput9_1_0.dll", EntryPoint = "XInputGetState", ExactSpelling = true)]
        private static extern uint GetState91(uint userIndex, out State state);
        private const ushort DpadUp = 0x0001, DpadDown = 0x0002, DpadLeft = 0x0004, DpadRight = 0x0008;
        private const ushort Start = 0x0010, Back = 0x0020, LeftThumb = 0x0040, RightThumb = 0x0080;
        private const ushort LeftShoulder = 0x0100, RightShoulder = 0x0200;
        private const ushort A = 0x1000, B = 0x2000, X = 0x4000, Y = 0x8000;
        private static readonly ushort[] buttonMasks = { A, B, X, Y, LeftShoulder, RightShoulder, Back, Start, LeftThumb, RightThumb, DpadUp, DpadDown, DpadLeft, DpadRight };
        private static readonly ushort[] previousButtons = new ushort[4];
        private static readonly ushort[] buttonsDownThisFrame = new ushort[4];
        private static readonly int[] lastButtonPollFrame = { -1, -1, -1, -1 };
        private static bool useLegacyDll;

        public static bool TryRead(int joystick, out float x, out float y, out ushort buttons)
        {
            x = y = 0f; buttons = 0;
            if (!CanUseXInput(joystick)) return false;
            int preferred = Mathf.Clamp(joystick - 1, 0, 3);
            if (!ReadState(preferred, out State state))
            {
                // A single connected Xbox pad can be assigned a different
                // XInput user slot after reconnecting. Keep Player 1 usable.
                if (joystick != 1) return false;
                bool found = false;
                for (int i = 0; i < 4 && !found; i++)
                    if (ReadState(i, out state)) { preferred = i; found = true; }
                if (!found) return false;
            }
            x = NormalizeAxis(state.gamepad.leftX);
            y = NormalizeAxis(state.gamepad.leftY);
            buttons = state.gamepad.buttons;
            return true;
        }
        public static bool IsConnected(int joystick)
        {
            if (!CanUseXInput(joystick)) return false;
            int preferred = Mathf.Clamp(joystick - 1, 0, 3);
            if (ReadState(preferred, out State _)) return true;
            if (joystick != 1) return false;
            for (int i = 0; i < 4; i++) if (ReadState(i, out State _)) return true;
            return false;
        }
        public static bool TryGetButtonDown(int joystick, int button)
        {
            if (!CanUseXInput(joystick)) return false;
            int index = Mathf.Clamp(joystick - 1, 0, 3);
            if (!ReadState(index, out State state)) return false;
            if (lastButtonPollFrame[index] != Time.frameCount)
            {
                buttonsDownThisFrame[index] = (ushort)(state.gamepad.buttons & ~previousButtons[index]);
                previousButtons[index] = state.gamepad.buttons;
                lastButtonPollFrame[index] = Time.frameCount;
            }
            ushort mask = Mask(button);
            return mask != 0 && (buttonsDownThisFrame[index] & mask) != 0;
        }
        public static bool IsPressed(ushort buttons, int button)
        {
            ushort mask = Mask(button);
            return mask != 0 && (buttons & mask) != 0;
        }
        public static bool TryFindPressedButton(ushort buttons, out int button)
        {
            for (int i = 0; i < buttonMasks.Length; i++)
                if ((buttons & buttonMasks[i]) != 0) { button = i; return true; }
            button = -1;
            return false;
        }
        private static ushort Mask(int button) { return button >= 0 && button < buttonMasks.Length ? buttonMasks[button] : (ushort)0; }
        private static bool CanUseXInput(int joystick)
        {
            if (joystick < 1 || joystick > 4) return false;
            string[] names = Input.GetJoystickNames();
            if (joystick > names.Length || string.IsNullOrEmpty(names[joystick - 1])) return true;
            string name = names[joystick - 1];
            return name.IndexOf("xbox", StringComparison.OrdinalIgnoreCase) >= 0 ||
                name.IndexOf("xinput", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        private static float NormalizeAxis(short value)
        {
            const int deadzone = 7849;
            if (Math.Abs((int)value) < deadzone) return 0f;
            return Mathf.Clamp(value / 32767f, -1f, 1f);
        }
        private static bool ReadState(int index, out State state)
        {
            try
            {
                uint result = useLegacyDll ? GetState91((uint)index, out state) : GetState14((uint)index, out state);
                return result == 0;
            }
            catch (DllNotFoundException)
            {
                useLegacyDll = true;
                try { return GetState91((uint)index, out state) == 0; }
                catch { state = default(State); return false; }
            }
            catch (EntryPointNotFoundException)
            {
                useLegacyDll = true;
                try { return GetState91((uint)index, out state) == 0; }
                catch { state = default(State); return false; }
            }
        }
#else
        public static bool TryRead(int joystick, out float x, out float y, out ushort buttons) { x = y = 0f; buttons = 0; return false; }
        public static bool IsConnected(int joystick) { return false; }
        public static bool TryGetButtonDown(int joystick, int button) { return false; }
        public static Vector2 ReadPadAxes(int joystick) { return new Vector2(Input.GetAxisRaw("LocalP" + joystick + "Horizontal"), Input.GetAxisRaw("LocalP" + joystick + "Vertical")); }
        public static bool TryFindPressedButton(ushort buttons, out int button) { button = -1; return false; }
        public static bool IsPressed(ushort buttons, int button) { return false; }
#endif
    }
}
