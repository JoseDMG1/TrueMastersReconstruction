using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ButtonsController : MonoBehaviour
{
 public static ButtonsController i;
 public Image buttonACircle;
 public Text buttonALetter;
 public Image AHitZone;
 public RadialUIFX ARadialFX;
 public Image buttonXCircle;
 public Text buttonXLetter;
 public Image XHitZone;
 public RadialUIFX XRadialFX;
 public Image buttonBCircle;
 public Text buttonBLetter;
 public Image currentButtonCircle;
 public Text currentButtonLetter;
 public Image currentHitZone;
 public RadialUIFX currentRadialFX;
 public ButtonAttack buttonAttackA;
 public ButtonAttack buttonAttackX;
 public GameObject blockInputs;
 public int lastAttackType;
 private void Awake() { if (!i) i = this; }
 private void Start() { if (!i) i = this; }
 public void ChangeColor(int color, string button)
 {
  Image circle = button == "A" ? buttonACircle : button == "B" ? buttonBCircle : null;
  Text text = button == "A" ? buttonALetter : button == "B" ? buttonBLetter : null;
  Color tint = color == 0 ? new Color(1f, 1f, 1f, .5f) : new Color(0f, 0f, 0f, .5f);
  if (circle) circle.color = tint;
  if (text) text.color = tint;
 }
 public void StartButtonRecovery(float time, string button)
 {
  bool a = button == "A";
  currentButtonCircle = a ? buttonACircle : buttonXCircle;
  currentButtonLetter = a ? buttonALetter : buttonXLetter;
  currentHitZone = a ? AHitZone : XHitZone;
  currentRadialFX = a ? ARadialFX : XRadialFX;
  if (currentButtonCircle) currentButtonCircle.color = Color.black;
  if (currentButtonLetter) currentButtonLetter.color = Color.black;
  if (currentHitZone) currentHitZone.gameObject.SetActive(false);
  if (currentRadialFX) currentRadialFX.ShowRadial(time);
  StartCoroutine(ShowTimeRecovery(currentButtonLetter, time));
  if (GameController.i) GameController.i.Attacking(false);
 }
 public IEnumerator ShowTimeRecovery(Text buttonLetter, float time)
 {
  string original = buttonLetter ? buttonLetter.text : "";
  // Capture ownership before yielding: simultaneous button recoveries must not redirect each other's UI.
  Image circle = currentButtonCircle;
  Image hitZone = currentHitZone;
  for (float remaining = time; remaining > 0f; remaining -= 1f)
  {
   if (buttonLetter) buttonLetter.text = remaining.ToString();
   yield return new WaitForSeconds(1f);
  }
  if (buttonLetter) { buttonLetter.text = original; buttonLetter.color = new Color(1f, 1f, 1f, .5f); }
  if (hitZone) hitZone.gameObject.SetActive(true);
  if (circle) circle.color = new Color(1f, 1f, 1f, .5f);
 }
 private void OnDestroy() { if (i == this) i = null; }
}
