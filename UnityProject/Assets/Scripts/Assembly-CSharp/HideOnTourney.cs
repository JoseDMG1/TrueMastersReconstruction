using UnityEngine;
public class HideOnTourney : MonoBehaviour
{
 private void Start(){if(SystemManager.i.mode==GameMode.Tournament)Destroy(gameObject);}
}
