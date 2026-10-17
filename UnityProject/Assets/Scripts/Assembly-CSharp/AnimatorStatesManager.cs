using Photon.Pun;
using UnityEngine;
public class AnimatorStatesManager : MonoBehaviour
{
 public Animator anim;
 private bool enterTransition;
 public PhotonView PV;
 public GameObject LandAtk;
 public bool death;
 public bool awakening;
 private void Awake() { anim = GetComponent<Animator>(); PV = GetComponent<PhotonView>(); }
 public void ChangeAtk(string atk)
 {
  if (!anim) return;
  int next = anim.GetInteger(atk) + 1;
  if (next >= 2) next = 0;
  anim.SetInteger(atk, next);
  foreach (string parameter in new[] { "BasicAttack", "UpAttack", "DownAttack", "LeftAttack", "RightAttack" }) anim.SetInteger(parameter, next);
  bool opponent = GameController.i && gameObject != GameController.i.MainCharacter;
  if (!opponent && ButtonsController.i && ButtonsController.i.buttonAttackA)
  {
   if (next == 0) ButtonsController.i.buttonAttackA.ChangeToA(); else ButtonsController.i.buttonAttackA.ChangeToX();
  }
  if (BattleSceneSingleton.i && BattleSceneSingleton.i.statsController) BattleSceneSingleton.i.statsController.ChangeColorIcon(next, "A", opponent);
 }
 [PunRPC] public void OnlineChangeColorIcon(int color, string button)
 {
  if (BattleSceneSingleton.i && BattleSceneSingleton.i.statsController) BattleSceneSingleton.i.statsController.ChangeColorIcon(color, button, true);
 }
 public void Stunned() { if (anim) anim.SetBool("Stunned", true); }
 public void AwakeningStart()
 {
  awakening = true;
  var aura = GetComponent<AuraController>();
  if (aura) aura.StartAura();
  var damage = GetComponent<DamageOnCollision>();
  if (damage) damage.ShakeCamera(4f, 3f, 0f, 2f);
  if (LocalOfflinePlayerInput.IsLocalBattle) StartCoroutine(FinishLocalAwakening());
  else if (GameController.i && gameObject == GameController.i.MainCharacter && BattleSceneSingleton.i && BattleSceneSingleton.i.SpecialAtk)
   StartCoroutine(BattleSceneSingleton.i.SpecialAtk.DechargeSpecial(20f));
 }
 private System.Collections.IEnumerator FinishLocalAwakening()
 {
  yield return new WaitForSeconds(20f);
  var aura = GetComponent<AuraController>();
  if (aura) aura.StopAura();
  awakening = false;
  if (anim) anim.SetInteger("NumSpecials", 1);
 }
 [PunRPC] public void Hit() { if (anim) anim.SetTrigger("Hit"); }
 [PunRPC] public void Falling() { if (anim) anim.SetTrigger("Falling"); }
 [PunRPC] public void Block() { if (anim) anim.SetTrigger("Block"); }
 [PunRPC] public void SetAnimatorParameters(bool atk, bool jump, float hor, float ver)
 {
  if (!anim) return;
  anim.SetBool("Stunned", false);
  anim.SetBool("Attack", atk);
  anim.SetBool("Jump", jump);
  anim.SetFloat("Horizontal", hor);
  anim.SetFloat("Vertical", ver);
 }
 [PunRPC] public void PlayAnimation(string animation) { if (anim) anim.Play(animation, -1, 0f); }
 public void SendAnimation()
 {
  if (!GameController.i || !GameController.i.online || !PV || !PV.IsMine || !anim) return;
  var clips = anim.GetCurrentAnimatorClipInfo(0);
  if (clips.Length > 0) PV.RPC("PlayAnimation", RpcTarget.Others, clips[0].clip.name);
 }
 public void SumonLandAttack()
 {
  var summon = GetComponent<SummonAttack>();
  if (LandAtk && summon) summon.Create(LandAtk);
 }
 public void NoDelay() { if (awakening && anim && (!GameController.i || !GameController.i.online || (PV && PV.IsMine))) anim.SetTrigger("NoDelay"); }
}
