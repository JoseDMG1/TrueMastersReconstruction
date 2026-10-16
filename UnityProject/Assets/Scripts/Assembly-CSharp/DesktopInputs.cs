using Photon.Pun;
using UnityEngine;
public class DesktopInputs : MonoBehaviour
{
 private float speed = 7f;
 private Animator anim;
 public PhotonView PV;
 private string horizontal = "Horizontal";
 private string vertical = "Vertical";
 private string fire = "Fire1";
 private string jump = "Fire2";
 private string Special = "Fire3";
 private void Start() { anim = GetComponent<Animator>(); PV = GetComponent<PhotonView>(); }
 private void Update()
 {
  if (!GameController.i || (GameController.i.online && PV && !PV.IsMine) || gameObject == GameController.i.OpponentCharacter) return;
  // Native Update 0x1288EB8..0x1288F08: key-up shortcuts are separate
  // from the axis-driven actions below. Taunt2 is a state, not a trigger.
  if (Input.GetKeyUp(KeyCode.J)) GameController.i.Special();
  if (Input.GetKeyUp(KeyCode.K)) anim.Play("Taunt2");
  DetectInputs();
 }
 private void DetectInputs() { Actions(Input.GetAxis(horizontal), Input.GetAxis(vertical), Input.GetAxis(fire), Input.GetAxis(jump), Input.GetAxis(Special)); }
 [PunRPC]
 public void Actions(float x, float z, float a, float j, float s)
 {
  if (!anim) anim = GetComponent<Animator>();
  if (!anim) return;
  anim.SetFloat("Horizontal", x);
  anim.SetFloat("Vertical", z);
  FighterLocomotion.Move(transform, anim, speed);
  // Native 0x12894CC follows BattleSceneSingleton.counter (+0x50), then
  // Counter.value (+0x20). The countdown reaches -1 when GO! is displayed.
  if (a == 1f)
  {
   // Read the managed field even after Counter destroys its GameObject. Unity's
   // overloaded object truth test becomes false then, but the native method
   // still reads this retained component's negative value.
   if (BattleSceneSingleton.i && !ReferenceEquals(BattleSceneSingleton.i.counter, null) && BattleSceneSingleton.i.counter.value < 0f) anim.SetBool("Attack", true);
  }
  else StopAttacking();
  if (s == 1f && GameController.i) GameController.i.Special();
  if (j == 1f) anim.SetBool("Jump", true); else StopJumping();
 }
 private void StopAttacking() { if (anim) anim.SetBool("Attack", false); }
 private void StopJumping() { if (anim) anim.SetBool("Jump", false); }
}

