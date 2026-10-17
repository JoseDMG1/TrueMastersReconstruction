using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
[DefaultExecutionOrder(-100)]
public class InputReader : MonoBehaviour
{
 public InputSet inputSet = new InputSet();
 public UnityEvent<InputSet> onInputSet = new UnityEvent<InputSet>();
 public VariableJoystick joystick;
 public bool blockInputs = true;
 public bool dPad;
 public bool keyboard;
 private readonly InputSet screenInput = new InputSet();
 private bool superHeld;
 private readonly List<RaycastResult> pointerHits = new List<RaycastResult>();
 private void Start() { keyboard = !Application.isMobilePlatform; }
 private void Update()
 {
  if (!dPad) SetJoystick();
  else { inputSet.x = screenInput.x; inputSet.y = screenInput.y; }
  inputSet.a = screenInput.a; inputSet.b = screenInput.b; inputSet.s = screenInput.s;
  if (blockInputs || Time.timeScale <= 0f) { ResetControls(); return; }
  if (keyboard && !Application.isMobilePlatform)
  {
   // Local-only mappings support keyboard or a specific legacy-input joystick.
   // Touch controls can still contribute through screenInput.
   var localInput = LocalControls.ReadPlayerOne();
   inputSet.x = localInput.x; inputSet.y = localInput.y;
   inputSet.a |= localInput.a; inputSet.b |= localInput.b; inputSet.s |= localInput.s;
   var game = GameController.i;
   if (game)
   {
    game.Attacking(inputSet.a);
    game.Jumping(inputSet.b);
    if (inputSet.s && !superHeld) game.Special();
   }
  }
  superHeld = inputSet.s;
  onInputSet?.Invoke(inputSet);
 }
 private bool PointerOverControl()
 {
  if (!EventSystem.current) return false;
  pointerHits.Clear();
  EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = Input.mousePosition }, pointerHits);
  if (pointerHits.Count == 0) return false;
  var hit = pointerHits[0].gameObject;
  return hit.GetComponentInParent<Selectable>() || hit.GetComponentInParent<EventTrigger>();
 }
 public void SetJoystick() { if (joystick) { inputSet.x = joystick.Horizontal; inputSet.y = joystick.Vertical; } }
 public void ActivateDpad(bool isActive) { dPad = isActive; }
 public void UpDownDpad(int value) { if (dPad) SetYpos(value); }
 public void LeftRightDpad(int value) { if (dPad) SetXpos(value); }
 public void SetAButton(bool active) { screenInput.a = inputSet.a = active; }
 public void SetBButton(bool active) { screenInput.b = inputSet.b = active; }
 public void SetSButton(bool active) { screenInput.s = inputSet.s = active; }
 public void SetXpos(float x) { screenInput.x = inputSet.x = x; }
 public void SetYpos(float y) { screenInput.y = inputSet.y = y; }
 public void ResetControls()
 {
  screenInput.x = screenInput.y = inputSet.x = inputSet.y = 0f;
  screenInput.a = screenInput.b = screenInput.s = inputSet.a = inputSet.b = inputSet.s = superHeld = false;
  if (GameController.i) { GameController.i.Attacking(false); GameController.i.Jumping(false); }
 }
 private void OnApplicationFocus(bool focused) { if (!focused) ResetControls(); }
 private void OnDisable() { ResetControls(); }
}
