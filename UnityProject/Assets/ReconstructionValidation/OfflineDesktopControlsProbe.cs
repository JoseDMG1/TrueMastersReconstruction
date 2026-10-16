#if (UNITY_EDITOR || DEVELOPMENT_BUILD) && UNITY_STANDALONE_WIN
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Deliberately sends OS input to this player's focused window. No InputSet or animator input injection.
public static class OfflineDesktopControlsProbe
{
    [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr handle);
    [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")] static extern bool ShowWindow(IntPtr handle, int mode);
    [DllImport("user32.dll")] static extern void keybd_event(byte key, byte scan, uint flags, UIntPtr extra);
    [DllImport("user32.dll")] static extern void mouse_event(uint flags, uint x, uint y, uint data, UIntPtr extra);
    [DllImport("user32.dll")] static extern bool SetCursorPos(int x, int y);
    [DllImport("user32.dll")] static extern bool ClientToScreen(IntPtr window, ref Point point);
    [DllImport("user32.dll")] static extern bool GetClientRect(IntPtr window, out Rect rect);
    [DllImport("user32.dll")] static extern uint MapVirtualKey(uint key, uint type);
    [StructLayout(LayoutKind.Sequential)] struct Point { public int x,y; }
    [StructLayout(LayoutKind.Sequential)] struct Rect { public int left,top,right,bottom; }
    static readonly byte[] Keys = {0x25,0x26,0x27,0x28,0x58,0x20,0x5A,0x43,0x41,0x57,0x44,0x53,0xA0};
    static IntPtr ownWindow;
    static void Focus()
    {
        if(GetForegroundWindow()!=ownWindow) SetForegroundWindow(ownWindow);
        if(GetForegroundWindow()!=ownWindow) throw new InvalidOperationException("Own player lost OS focus; input not sent");
    }
    static void Key(byte key, bool down) { if(down) Focus(); keybd_event(key, (byte)MapVirtualKey(key,0), (key >= 0x25 && key <= 0x28 ? 1u : 0u) | (down ? 0u : 2u), UIntPtr.Zero); }
    static void Mouse(int button, bool down) { if(down) Focus(); mouse_event(button == 0 ? (down ? 2u : 4u) : button == 1 ? (down ? 8u : 16u) : (down ? 32u : 64u),0,0,0,UIntPtr.Zero); }
    static void Release() { foreach (byte key in Keys) Key(key,false); for(int i=0;i<3;i++) Mouse(i,false); }
    static IEnumerator Sample(Animator anim, float seconds, HashSet<string> clips)
    {
        double until = Time.realtimeSinceStartupAsDouble + seconds;
        while (Time.realtimeSinceStartupAsDouble < until)
        {
            foreach(var item in anim.GetCurrentAnimatorClipInfo(0)) if(item.clip) clips.Add(item.clip.name);
            yield return null;
        }
    }
    static void Action(bool alternate, int action, bool down)
    {
        if(alternate)
        {
            if(action == 1) Key(0x20,down);
            else if(action == 2) Mouse(1,down);
            else Mouse(0,down);
        }
        else Key(action == 0 ? (byte)0x5A : action == 1 ? (byte)0x58 : (byte)0x43,down);
    }
    static void ClickToggle(Toggle toggle)
    {
        ExecuteEvents.Execute(toggle.gameObject,new PointerEventData(EventSystem.current){button=PointerEventData.InputButton.Left},ExecuteEvents.pointerClickHandler);
    }
    public static IEnumerator Run(GameController game, bool alternate, string screenshot, Action<string,string> observe, Action<string> fail)
    {
        var reader = BattleSceneSingleton.i.inputReader;
        var anim = game.animMainChar;
        var previousWindow = GetForegroundWindow();
        IntPtr window = Process.GetCurrentProcess().MainWindowHandle;
        ownWindow = window;
        try
        {
            if(window == IntPtr.Zero) { fail("Desktop player has no OS window"); yield break; }
            ShowWindow(window,5); SetForegroundWindow(window);
            double focusDeadline = Time.realtimeSinceStartupAsDouble + 5;
            while(GetForegroundWindow() != window && Time.realtimeSinceStartupAsDouble < focusDeadline) yield return null;
            if(GetForegroundWindow() != window) { fail("Cannot focus own player; no OS input sent"); yield break; }
            GetClientRect(window,out var client);
            var point = new Point{x=client.right/2,y=client.bottom/2}; ClientToScreen(window,ref point); SetCursorPos(point.x,point.y);
            Release(); reader.ResetControls(); reader.keyboard = true;
            observe("desktop-scope","OS keybd_event/mouse_event into this focused Windows player. CPU disabled. Super charge accelerated through native ChargeSpecial. Layout="+(alternate ? "WASD/left click/Space/right click" : "arrows/Z/X/C"));
            var toggle = UnityEngine.Object.FindObjectsByType<Toggle>(FindObjectsInactive.Include).Single(t=>t.name=="AlternateKeyboardControlsToggle");
            var settings = UnityEngine.Object.FindFirstObjectByType<JoystickSettings>();
            int inversion = PlayerPrefs.GetInt("ControllerInverted",0); bool inverted = settings.inverted;
            // Open the authored settings button and exercise the actual Toggle pointer callback.
            var open = UnityEngine.Object.FindObjectsByType<Button>(FindObjectsInactive.Exclude).Single(b=>Enumerable.Range(0,b.onClick.GetPersistentEventCount()).Any(i=>b.onClick.GetPersistentMethodName(i)=="OpenSettings"));
            open.onClick.Invoke(); yield return new WaitForSecondsRealtime(1.2f);
            bool starting = toggle.isOn;
            if(alternate && (!starting || PlayerPrefs.GetInt("AlternateKeyboardControls",0)!=1)) { fail("Alternate layout did not survive scene reload"); yield break; }
            ClickToggle(toggle); yield return null;
            if(toggle.isOn==starting || PlayerPrefs.GetInt("AlternateKeyboardControls",0)!=(toggle.isOn?1:0) || settings.inverted!=inverted || PlayerPrefs.GetInt("ControllerInverted",0)!=inversion)
            { fail("Alternate controls toggle did not persist independently of joystick inversion"); yield break; }
            if(toggle.isOn != alternate) ClickToggle(toggle);
            yield return null;
            ScreenCapture.CaptureScreenshot(screenshot); yield return new WaitForEndOfFrame();
            observe("desktop-toggle","isOn="+toggle.isOn+"; pref="+PlayerPrefs.GetInt("AlternateKeyboardControls",0)+"; inversion unchanged; screenshot="+screenshot);
            var close = UnityEngine.Object.FindObjectsByType<EventTrigger>(FindObjectsInactive.Exclude).FirstOrDefault(t=>t.triggers.Any(e=>Enumerable.Range(0,e.callback.GetPersistentEventCount()).Any(i=>e.callback.GetPersistentMethodName(i)=="CloseSettings")));
            if(close) ExecuteEvents.Execute(close.gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerClickHandler);
            else { fail("Settings close pointer route not found"); yield break; }
            yield return new WaitForSecondsRealtime(1.2f);
            observe("desktop-pointer","client="+client.right+"x"+client.bottom+"; render="+Screen.width+"x"+Screen.height+"; mouse="+Input.mousePosition);
            var hoverZone = UnityEngine.Object.FindObjectsByType<EventTrigger>(FindObjectsInactive.Exclude).Single(t=>t.triggers.Any(e=>Enumerable.Range(0,e.callback.GetPersistentEventCount()).Any(i=>e.callback.GetPersistentMethodName(i)=="SetAButton")));
            var canvas = hoverZone.GetComponentInParent<Canvas>();
            var hoverPosition = RectTransformUtility.WorldToScreenPoint(canvas.renderMode==RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,hoverZone.transform.position);
            var hoverPoint = new Point{x=(int)(hoverPosition.x/Screen.width*client.right),y=(int)((Screen.height-hoverPosition.y)/Screen.height*client.bottom)};
            ClientToScreen(window,ref hoverPoint); SetCursorPos(hoverPoint.x,hoverPoint.y);
            yield return new WaitForSecondsRealtime(.3f);
            if(reader.inputSet.a || anim.GetBool("Attack")) { fail("Desktop hover activated attack without pressing a button"); yield break; }
            observe("desktop-hover","Actual OS cursor over A touch zone without mouse press: no attack");
            SetCursorPos(point.x,point.y); yield return new WaitForSecondsRealtime(.1f);
            // Readiness and release are tested through actual polled Input, never direct InputSet mutation.
            byte[] directions = alternate ? new byte[]{0x41,0x44,0x57,0x53} : new byte[]{0x25,0x27,0x26,0x28};
            Vector2[] expected = {Vector2.left,Vector2.right,Vector2.up,Vector2.down};
            for(int i=0;i<directions.Length;i++)
            {
                Vector3 before = game.MainCharacter.transform.position;
                Key(directions[i],true); yield return new WaitForSecondsRealtime(.25f);
                float travel = Vector3.Distance(before,game.MainCharacter.transform.position);
                bool axis = reader.inputSet.x==expected[i].x && reader.inputSet.y==expected[i].y;
                Key(directions[i],false); yield return new WaitForSecondsRealtime(.1f);
                observe("desktop-move","key="+directions[i]+"; expected="+expected[i]+"; axesMatch="+axis+"; travel="+travel+"; release="+reader.inputSet.x+","+reader.inputSet.y);
                if(!axis || travel<.3f || reader.inputSet.x!=0 || reader.inputSet.y!=0) { fail("OS movement or release failed"); yield break; }
            }
            var clips = new HashSet<string>();
            Action(alternate,0,true); yield return new WaitForSecondsRealtime(.12f);
            bool attack = reader.inputSet.a; Action(alternate,0,false);
            yield return Sample(anim,2f,clips);
            observe("desktop-attack","polled="+attack+"; clips="+string.Join(",",clips));
            if(!attack || !clips.Any(c=>c.IndexOf("Attack",StringComparison.OrdinalIgnoreCase)>=0 || c.IndexOf("Atk",StringComparison.OrdinalIgnoreCase)>=0)) { fail("OS attack did not enter attack animation"); yield break; }
            double recover = Time.realtimeSinceStartupAsDouble+12;
            while(anim.GetBool("Stunned") && Time.realtimeSinceStartupAsDouble<recover) yield return null;
            clips.Clear(); Key(directions[0],true); Action(alternate,1,true);
            yield return new WaitForSecondsRealtime(.12f);
            bool dodge = reader.inputSet.b; Key(directions[0],false); Action(alternate,1,false);
            yield return Sample(anim,2f,clips);
            observe("desktop-evade","polled="+dodge+"; clips="+string.Join(",",clips));
            if(!dodge || !clips.Any(c=>c.IndexOf("Jump",StringComparison.OrdinalIgnoreCase)>=0 || c.IndexOf("Roll",StringComparison.OrdinalIgnoreCase)>=0)) { fail("OS dodge did not enter directional evade animation"); yield break; }
            recover = Time.realtimeSinceStartupAsDouble+12;
            while(anim.GetBool("Stunned") && Time.realtimeSinceStartupAsDouble<recover) yield return null;
            var special = SpecialAtk.specialAtk;
            if(anim.GetBool("Stunned")) { fail("Actor still stunned before super fixture"); yield break; }
            // Charging while already holding super must not count as a new press.
            Action(alternate,2,true); yield return new WaitForSecondsRealtime(.15f);
            special.ChargeSpecial(1f); special.ChargeSpecial(1f);
            yield return new WaitForSecondsRealtime(.25f);
            if(special.decharging || anim.GetBool("Stunned")) { fail("Super retriggered while held as charge became available"); yield break; }
            Action(alternate,2,false); yield return new WaitForSecondsRealtime(.12f);
            clips.Clear(); Action(alternate,2,true); yield return new WaitForSecondsRealtime(.12f); Action(alternate,2,false);
            yield return Sample(anim,4f,clips);
            var manager = game.MainCharacter.GetComponent<AnimatorStatesManager>();
            observe("desktop-awakening","clips="+string.Join(",",clips)+"; awakening="+manager.awakening+"; decharging="+special.decharging);
            if(!manager.awakening || !special.decharging) { fail("OS super did not activate awakening"); yield break; }
            double deadline = Time.realtimeSinceStartupAsDouble+45;
            while((special.decharging || anim.GetInteger("NumSpecials")!=1) && Time.realtimeSinceStartupAsDouble<deadline) yield return null;
            if(special.decharging || anim.GetInteger("NumSpecials")!=1) { fail("Awakening did not complete naturally"); yield break; }
            recover = Time.realtimeSinceStartupAsDouble+12;
            while(anim.GetBool("Stunned") && Time.realtimeSinceStartupAsDouble<recover) yield return null;
            if(anim.GetBool("Stunned")) { fail("Actor still stunned after awakening; input="+JsonUtility.ToJson(reader.inputSet)); yield break; }
            special.ChargeSpecial(1f); special.ChargeSpecial(1f);
            observe("desktop-super-ready","charged="+special.charged+"; fill="+special.chargeImg.fillAmount+"; count="+special.numberOfSpecial+"; active="+game.SpecialButton.activeSelf+"; stunned="+anim.GetBool("Stunned")+"; input="+JsonUtility.ToJson(reader.inputSet)+"; focus="+(GetForegroundWindow()==window));
            if(!special.charged) { fail("Accelerated phase1 charge fixture did not arm special"); yield break; }
            clips.Clear(); Action(alternate,2,true); yield return new WaitForSecondsRealtime(.12f); Action(alternate,2,false);
            observe("desktop-super-press","polled="+reader.inputSet.s+"; stunned="+anim.GetBool("Stunned")+"; phase="+anim.GetInteger("NumSpecials")+"; focus="+(GetForegroundWindow()==window));
            yield return Sample(anim,5f,clips);
            observe("desktop-super","clips="+string.Join(",",clips));
            if(!clips.Any(c=>c.IndexOf("Special",StringComparison.OrdinalIgnoreCase)>=0)) { fail("OS super did not enter charged phase1 special"); yield break; }
            observe("desktop-accepted",alternate ? "WASD/left click/Space/right click movement, attack, evade and super" : "Arrows/Z/X/C movement, attack, evade and super");
            if(!alternate) { ClickToggle(toggle); observe("desktop-reload-layout","Alternate selected via Toggle before reloading scene"); }
        }
        finally
        {
            Release(); reader.ResetControls();
            if(GetForegroundWindow()==window && previousWindow!=IntPtr.Zero) SetForegroundWindow(previousWindow);
        }
    }
}
#endif
