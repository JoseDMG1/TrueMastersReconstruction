using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class RadialUIFX : MonoBehaviour
{
 public Image radialImage;
 public void ShowRadial(float time) { StartCoroutine(ChargeShieldBar(time)); }
 public IEnumerator ChargeShieldBar(float time)
 {
  if (!radialImage) yield break;
  radialImage.fillAmount = 0f;
  while (radialImage && radialImage.fillAmount < 1f)
  {
   radialImage.fillAmount += .042f / time;
   yield return new WaitForSeconds(.04f);
  }
 }
}
