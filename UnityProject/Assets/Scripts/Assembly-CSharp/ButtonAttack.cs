using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ButtonAttack : MonoBehaviour
{
 public string letter;
 public int attackType;
 public float recoveryTime;
 public Image currentButtonCircle;
 public Text currentButtonLetter;
 public Image currentHitZone;
 public RadialUIFX currentRadialFX;
 public bool reloading;
 private float currentTime;
 private void Start() { if (currentButtonLetter) currentButtonLetter.text = letter; }
 public void OnButtonPress()
 {
  if (!GameController.i || !GameController.i.animMainChar || GameController.i.animMainChar.GetBool("Stunned") || reloading) return;
  if (ButtonsController.i) ButtonsController.i.lastAttackType = attackType;
  GameController.i.AttackingButton(attackType);
  Invoke(nameof(StopAttacking), .1f);
 }
 public void ReloadButton()
 {
  if (reloading) return;
  reloading = true;
  if (currentButtonCircle) currentButtonCircle.color = Color.black;
  if (currentButtonLetter) currentButtonLetter.color = Color.black;
  if (currentRadialFX) currentRadialFX.ShowRadial(recoveryTime);
  StartCoroutine(ShowTimeRecovery(recoveryTime));
 }
 public void StopAttacking() { if (GameController.i) GameController.i.Attacking(false); }
 public IEnumerator ShowTimeRecovery(float time)
 {
  currentTime = time;
  while (currentTime > 0f)
  {
   if (currentButtonLetter) currentButtonLetter.text = currentTime.ToString();
   currentTime -= 1f;
   yield return new WaitForSeconds(1f);
  }
  if (currentButtonLetter) { currentButtonLetter.text = letter; currentButtonLetter.color = new Color(1f, 1f, 1f, .5f); }
  if (currentButtonCircle) currentButtonCircle.color = new Color(1f, 1f, 1f, .5f);
  reloading = false;
 }
 public void RestoreButton()
 {
  if (!reloading) return;
  currentTime = 0f;
  if (currentRadialFX && currentRadialFX.radialImage) currentRadialFX.radialImage.fillAmount = 1f;
 }
 public void ChangeToA()
 {
  if (currentButtonLetter) { currentButtonLetter.text = "A"; currentButtonLetter.color = new Color(1f, 1f, 1f, .5f); }
  if (currentButtonCircle) currentButtonCircle.color = new Color(1f, 1f, 1f, .5f);
  if (currentRadialFX && currentRadialFX.radialImage) currentRadialFX.radialImage.fillAmount = 0f;
 }
 public void ChangeToX()
 {
  if (currentButtonLetter) { currentButtonLetter.text = "X"; currentButtonLetter.color = Color.black; }
  if (currentButtonCircle) currentButtonCircle.color = Color.black;
  if (currentRadialFX && currentRadialFX.radialImage)
  {
   currentRadialFX.radialImage.color = new Color(1f, 1f, 1f, .5f);
   currentRadialFX.radialImage.fillAmount = 1f;
  }
 }
}
