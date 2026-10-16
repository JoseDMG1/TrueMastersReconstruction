using Photon.Pun;
using UnityEngine;
public class AnimationMovement : MonoBehaviour
{
 public bool moving;
 public float horizontal = .5f;
 public float vertical = .5f;
 public PhotonView PV;
 public Vector3 netPosition;
 private void Start() { PV = GetComponent<PhotonView>(); }
 private void FixedUpdate()
 {
  if (!moving || (GameController.i && GameController.i.online && PV && !PV.IsMine)) return;
  if (DistanceFromPlayer() < 2f && vertical > 0f) vertical = 0f;
  if (horizontal > 0f) horizontal = Mathf.Max(0f, horizontal - .005f);
  else if (horizontal < 0f) horizontal = Mathf.Min(0f, horizontal + .006f);
  if (vertical > 0f) vertical = Mathf.Max(0f, vertical - .004f);
  else if (vertical < 0f) vertical = Mathf.Min(0f, vertical + .004f);
  transform.position += transform.TransformDirection(horizontal, 0f, vertical);
  if (horizontal == 0f && vertical == 0f) moving = false;
 }
 [PunRPC] public void Move(Vector3 newPosition) { netPosition = newPosition; }
 public void SendBlock()
 {
  if (!GameController.i || !GameController.i.online || !PV || !PV.IsMine) return;
  var anim = GetComponent<Animator>();
  if (anim && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block")) PV.RPC("NetBlock", RpcTarget.Others);
 }
 [PunRPC] public void NetBlock() { var anim = GetComponent<Animator>(); if (anim) anim.SetBool("Jump", true); }
 [PunRPC] public void StopNetBlock() { var anim = GetComponent<Animator>(); if (anim) anim.SetBool("Jump", false); }
 public void goHorizontal(float speed) { horizontal = speed; vertical = Mathf.Max(-Mathf.Abs(speed) / DistanceFromPlayer(), -.15f); moving = true; }
 public void goDiagonal(float speed) { horizontal = speed; vertical = -.15f; moving = true; }
 public void goVertical(float speed) { horizontal = 0f; vertical = speed; moving = true; }
 public void goHorizontal2(float speed) { if (speed != 0f) { horizontal = speed; moving = true; } }
 public void goVertical2(float speed) { if (speed != 0f) { vertical = speed; moving = true; } }
 public void stopGo() { moving = false; }
 public float DistanceFromPlayer()
 {
  if (!GameController.i || !GameController.i.MainCharacter || !GameController.i.OpponentCharacter) return 1f;
  return Vector3.Distance(GameController.i.MainCharacter.transform.position, GameController.i.OpponentCharacter.transform.position);
 }
}
