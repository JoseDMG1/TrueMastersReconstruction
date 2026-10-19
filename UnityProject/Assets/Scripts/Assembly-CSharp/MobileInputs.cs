using Photon.Pun;
using UnityEngine;

public class MobileInputs : MonoBehaviour
{
 public Joystick joystick;
 public Animator anim;
 private float speed = 7f;
 private PhotonView PV;
 public float x;
 public float z;
 public bool suppressTouchMovement;
 private void Start()
 {
  anim = GetComponent<Animator>();
  PV = GetComponent<PhotonView>();
  if (!joystick && BattleSceneSingleton.i && BattleSceneSingleton.i.inputReader) joystick = BattleSceneSingleton.i.inputReader.joystick;
 }
 private void Update()
 {
  if (!GameController.i || (GameController.i.online && PV && !PV.IsMine)) return;
  DetectInputs();
 }
 public void DetectInputs()
 {
  if (!anim || !BattleSceneSingleton.i || !BattleSceneSingleton.i.inputReader) return;
  var input = BattleSceneSingleton.i.inputReader.inputSet;
  // 0x129A9E0..0x129AA9C: these are native locals, not the legacy x/z fields.
  float horizontal = input.x > .3f ? 1f : input.x < -.3f ? -1f : 0f;
  float vertical = input.y > .3f ? 1f : input.y < -.3f ? -1f : 0f;
  anim.SetFloat("SHorizontal", horizontal, .2f, Time.deltaTime);
  anim.SetFloat("SVertical", vertical, .2f, Time.deltaTime);
  if (!suppressTouchMovement) FighterLocomotion.Move(transform, anim, speed);
  anim.SetFloat("Horizontal", horizontal);
  anim.SetFloat("Vertical", vertical);
 }
}

// Shared owner for the same movement calculation recovered in MobileInputs and DesktopInputs.
internal static class FighterLocomotion
{
 internal static void Move(Transform body, Animator anim, float speed)
 {
  if (anim.GetBool("Stunned")) return;
  float horizontal = anim.GetFloat("Horizontal");
  float vertical = anim.GetFloat("Vertical");
  if (GameController.i)
  {
   float distance = GameController.i.GetDistancesBetweenPlayers();
   if (distance < 2.5f && anim.GetFloat("SVertical") > 0f) vertical = 0f;
   if (distance < 4f && anim.GetFloat("SVertical") > 0f && GameController.i.OpponentCharacter)
   {
    var opponent = GameController.i.OpponentCharacter.GetComponent<Animator>();
    if (opponent && opponent.GetCurrentAnimatorStateInfo(0).IsName("PrisonFree")) vertical = 0f;
   }
  }
  body.position += (body.forward * vertical + body.right * horizontal).normalized * speed * Time.deltaTime;
 }
}

