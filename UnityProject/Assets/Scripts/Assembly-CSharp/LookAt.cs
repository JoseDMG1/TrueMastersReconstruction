using UnityEngine;
public class LookAt : MonoBehaviour
{
 public Transform target;
 private void Start() { Setup(); }
 public void Setup()
 {
  if (!BattleSceneSingleton.i || !BattleSceneSingleton.i.MainCharacter) return;
  // Native Setup compares fighter layers, allowing child aiming owners to
  // resolve the same opponent as their character root.
  GameObject other = BattleSceneSingleton.i.MainCharacter.layer == gameObject.layer ? BattleSceneSingleton.i.Opponent : BattleSceneSingleton.i.MainCharacter;
  target = other ? other.transform : null;
 }
 private void LateUpdate()
 {
  if (!target) Setup();
  if (!target) return;
  var anim = GetComponent<Animator>();
  if (anim && anim.GetCurrentAnimatorStateInfo(0).IsName("Freeze")) return;
  transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
 }
}
