using System.Collections.Generic;
using UnityEngine;
// Native localScale/localPosition interpolation .01 per physics step.
public class ZoomCanvas : MonoBehaviour
{
 public List<RectTransform> views;
 public RectTransform currentView;
 private void Start(){currentView=views[0];}
 private void FixedUpdate(){var rect=GetComponent<RectTransform>();rect.localScale=Vector3.Lerp(rect.localScale,currentView.localScale,.01f);rect.localPosition=Vector3.Lerp(rect.localPosition,currentView.localPosition,.01f);}
}
