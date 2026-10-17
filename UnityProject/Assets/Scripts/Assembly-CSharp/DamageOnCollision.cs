using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Offline reconstruction from mapped native ARM64; see Knowledge/projectile-reconstruction.md.
public class DamageOnCollision : MonoBehaviour
{
    public float Damage;
    public float shieldDamage;
    public float specialCharge;
    private Animator anim;
    private PhotonView PV;
    public GameObject _shieldExplotion;
    public bool unfrozable;
    public float unTrap;
    private bool restoringShield;
    public float frontBackRatio = .9f;
    public float frontRatio = .9f;
    public UnityEvent onDamageRecived;
    public List<string> onDamageEventExeptions;
    private bool Online => GameController.i && GameController.i.online;
    private bool Opponent => GameController.i && gameObject != GameController.i.MainCharacter;
    private bool Awakening => GetComponent<AnimatorStatesManager>() && GetComponent<AnimatorStatesManager>().awakening;
    private bool State(string name) => anim && anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    private bool CurrentOrNext(string name) => State(name) || (anim && anim.GetNextAnimatorStateInfo(0).IsName(name));
    private bool Blocking => anim && anim.GetBool("Blocking");

    private void Awake()
    {
        anim = GetComponent<Animator>(); PV = GetComponent<PhotonView>();
        if (onDamageEventExeptions == null) onDamageEventExeptions = new List<string>();
        if (!onDamageEventExeptions.Contains("branchPrison")) onDamageEventExeptions.Add("branchPrison");
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (Online || !collider || Validations(collider.tag)) return;
        var description = collider.GetComponent<Description>();
        if (!description) return;
        // Native 0x1284058 reads Description +0x40 (effect), not +0x38
        // (ReleaseBy). Both reaction dispatch and exception lookup use it.
        string effect = description.effect;
        HitAnimation(description.Damage, effect, collider.name,
            Vector3.Distance(collider.transform.position, transform.position), collider);
        if (!onDamageEventExeptions.Contains(effect)) onDamageRecived?.Invoke();
    }
    [PunRPC] public void InvokeOnDamageOnline() { onDamageRecived?.Invoke(); }
    // True means reject the collision; see the native caller at 0x1284018.
    public bool Validations(string colliderTag)
    {
        if (Online || !anim) return true;
        if (CurrentOrNext("FallingToLanding") || CurrentOrNext("Falling") || colliderTag != "Attack"
            || CurrentOrNext("Awakening")) return true;
        var description = GetComponent<Description>();
        if (State("Special") && description && description.element == "Leaf") return true;
        if (transform.Find("ShadowHandGrab(Clone)")) DestroyShadowHandGrab();
        var disable = GetComponent<DisableColliderAnim>();
        float time = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        return disable && State("UpAttack2") && time > .2f && time < .6f;
    }
    public void HitAnimation(float attackDamage, string effect, string name, float distance, Collider collider)
    {
        if (effect == "No" || !anim || Online) return;
        if (effect == "" || effect == "RightAttack" || effect == "LeftAttack")
        {
            float angle = Utilities.i.GetAngleDiference(gameObject, collider.gameObject);
            effect = angle < 45f && angle > -45f ? "BackHit"
                : angle > 40f && angle < 135f ? "RightAttack"
                : angle > -135f && angle < -40f ? "LeftAttack" : "";
            if (State("Freeze")) effect = "";
        }
        if (effect == "Pos")
        {
            string position = Utilities.GetRelativePositionByAngle(transform, collider.transform, frontBackRatio, frontRatio);
            effect = position == "Behind" ? "BackHit" : position == "Left" ? "RightAttack"
                : position == "Right" ? "LeftAttack" : "";
        }
        if (effect == "Front") effect = "";
        if (attackDamage < 4f && Awakening)
        {
            ShakeCamera();
            if (effect == "RightAttack") RightAtkAnim(attackDamage);
            else if (effect == "LeftAttack") LeftAtkAnim(attackDamage);
            else if (Blocking)
            {
                Push(0f, attackDamage == 1f ? -.14f : -.17f); ShieldBlockAnim();
            }
            else
            {
                GetDamage(.5f); PlayHitAnimation(attackDamage == 1f ? "hitBlocking" : "hit1"); DestroyShield();
            }
            return;
        }
        GetDamage(.5f);
        if (effect != "") { EffectAnimation(effect, name, distance, attackDamage); ShakeCamera(); return; }
        if (anim.GetBool("Weak")) { WeakAnimation(attackDamage); ShakeCamera(); return; }
        float strength = attackDamage - (Blocking ? 1f : 0f);
        if (Damage > 5f) strength++;
        if (Damage > 10f) strength++;
        if (Damage > 30f) strength++;
        if (strength == 0f || (strength == 1f && Blocking))
        { Push(0f, -.17f); ShieldBlockAnim(); }
        else PlayHitAnimation(strength == 1f || strength == 2f ? "hit1" : strength == 3f ? "hit2" : "Fall 3");
        if (strength > 1f && Blocking)
        {
            ShieldExplotion(); anim.SetBool("Blocking", false); ShakeCamera(); GetDamage(.5f);
        }
        if (Blocking) { ShakeCamera(4f, 2f); return; }
        GetDamage(.5f);
        if (strength <= 1f) ShakeCamera(5f, 2f);
        if (strength <= 2f) ShakeCamera(); else ShakeCamera(6f, 4f);
    }
    public void EffectAnimation(string effect, string name, float distance, float attackDamage)
    {
        switch (effect)
        {
            case "RightAttack": RightAtkAnim(attackDamage); break;
            case "LeftAttack": LeftAtkAnim(attackDamage); break;
            case "BackHit": BackAtkAnim(attackDamage); break;
            case "freeze": FreezeAnimation(name); break;
            case "fall3": PlayHitAnimation("Fall 3"); break;
            case "hitBlocking": case "hit1": case "hit2": case "dizzy": PlayHitAnimation(effect); break;
            case "flip": PlayHitAnimation("hitBackFlip"); break;
            case "poison": PlayHitAnimation("cof"); break;
            case "branchPrison": PlayHitAnimation("PrisonFree"); break;
            case "NoHit": if (!anim.GetBool("Weak")) PlayHitAnimation(Damage < 10f ? "hit1" : "hit2"); break;
            case "hit12": PlayHitAnimation(Damage < 10f ? "hit1" : "hit2"); break;
            case "hit23": PlayHitAnimation(Damage < 5f ? "hit2" : "Fall 3"); break;
            case "ShadowTramp":
                var hand = transform.Find("ShadowHandGrab(Clone)");
                if (hand)
                {
                    var controller = hand.GetComponent<ShadowHandGrabController>();
                    if (controller) controller.Explote();
                    PlayHitAnimation("hitBackFlip");
                }
                else PlayHitAnimation("ShadowTrampDamage");
                break;
            case "prison":
                if (distance > 1.2f || unTrap > 0f)
                {
                    if (GameEvents.i) GameEvents.i.PlayerPrisonHit(gameObject.layer);
                    PlayHitAnimation("hitBackFlip"); unTrap = 0f;
                }
                else
                {
                    PlayHitAnimation("PrisonFree");
                    if (GameEvents.i) GameEvents.i.PlayerPrisonTrap(gameObject.layer);
                    unTrap++; Invoke(nameof(RecoverUnTrap), 10f);
                }
                break;
        }
    }
    public void WeakAnimation(float attackDamage)
    {
        if (Damage > 20f) { PlayHitAnimation("Fall 3"); return; }
        if (State("Fall 3") || State("Stand 1") || State("StandFast")) { PlayHitAnimation("hit2"); return; }
        if (Damage > 25f) { PlayHitAnimation("Fall 3"); return; }
        if (State("hit2") || State("Kip") || State("StandUp")) { PlayHitAnimation("hitBackFlip"); return; }
        if (State("hitBackFlip")) { PlayHitAnimation("hit1"); return; }
        if (State("hit1")) { PlayHitAnimation(attackDamage >= 3f ? "Fall 3" : "hitBlocking"); return; }
        if (State("hitBlocking")) { PlayHitAnimation("hit1"); return; }
        PlayHitAnimation(attackDamage < 2f ? "Fall 3" : "hitBackFlip");
    }
    public void FreezeAnimation(string colliderName)
    {
        if (State("Freeze")) return;
        if (colliderName == "FreezeUpAtk(Clone)")
        {
            if (Blocking && Damage < 10f) { Push(0f, -.17f); return; }
            if (unfrozable) { PlayHitAnimation(Damage < 10f ? "hit1" : "hit2"); return; }
            GetDamage(.5f); anim.SetTrigger("Freeze"); StartCoroutine(TryToFreeze());
        }
        else if (colliderName == "FreezeAtk2(Clone)")
        {
            anim.SetTrigger("Freeze"); StartCoroutine(TryToFreeze()); DropAllAtks(); ResetSpecialFreeze();
        }
    }
    public void RightAtkAnim(float attackDamage) { SideAtkAnim(attackDamage, -1f, "LeftHit", "LeftHit2"); }
    public void LeftAtkAnim(float attackDamage) { SideAtkAnim(attackDamage, 1f, "RightHit", "RightHit2"); }
    private void SideAtkAnim(float attackDamage, float sign, string light, string heavy)
    {
        if (attackDamage > 2f && !Awakening) { PlayHitAnimation(heavy); return; }
        bool airShield = GetComponent<Description>().element == "Air" && Blocking;
        if (airShield)
        {
            if (Damage >= 10f && !Awakening) { PlayHitAnimation(heavy); return; }
            float strength = Damage >= 5f && !Awakening ? -.3f : -.2f;
            var movement = GetComponent<AnimationMovement>();
            Push(sign * -strength, Mathf.Max(strength / movement.DistanceFromPlayer(), -.15f));
            ShieldExplotion(); ShieldBlockAnim(); return;
        }
        PlayHitAnimation(Damage >= 5f && !Awakening ? heavy : light); GetDamage(.5f);
    }
    public void BackAtkAnim(float attackDamage)
    {
        if (attackDamage > 2f && !Awakening) { PlayHitAnimation("BackHit"); return; }
        if (GetComponent<Description>().element == "Air" && Blocking)
        {
            Push(0f, Damage >= 5f && !Awakening ? .2f : .15f); ShieldExplotion(); ShieldBlockAnim(); return;
        }
        PlayHitAnimation("BackHit"); GetDamage(.5f);
    }
    private void Push(float horizontal, float vertical)
    {
        var movement = GetComponent<AnimationMovement>();
        if (!movement) return;
        movement.horizontal = horizontal; movement.vertical = vertical; movement.moving = true;
    }
    public void ShakeCamera(float magnitude = 4f, float roughness = 3f, float fadeInTime = 0f, float fadeOutTime = 2f)
    { if (CameraShake.i) CameraShake.i.Shake(.5f); }
    [PunRPC] public void ShakeCameraOnline(float magnitude = 4f, float roughness = 3f, float fadeInTime = 0f, float fadeOutTime = 2f)
    { if (CameraShake.i) CameraShake.i.Shake(.5f); }
    public void RecoverUnTrap() { unTrap = 0f; }
    public void ShieldBlockAnim() { PlayHitAnimation("BlockHit"); }
    public void MakeUnfrozable() { unfrozable = true; Invoke(nameof(MakeFrozable), 12f); }
    private void MakeFrozable() { unfrozable = false; }
    public void DropAllAtks()
    {
        if (!AttacksManager.i) return;
        // Snapshot: DestroyAttack may unregister an attack while iterating.
        foreach (var attack in new List<GameObject>(AttacksManager.i.GetOwnAttacks(gameObject.layer)))
        {
            if (!attack || attack.layer != gameObject.layer) continue;
            var configuration = attack.GetComponent<ConfigAttackLayer>();
            if (!configuration) continue;
            configuration.playerWeak = true;
            var description = attack.GetComponent<Description>();
            if (description && description.category == "shield") configuration.DestroyAttack();
        }
    }
    public void PlayHitAnimation(string animation)
    { if (!Online && GetComponent<AnimatorStatesManager>()) GetComponent<AnimatorStatesManager>().PlayAnimation(animation); }
    public void ShieldExplotion() { if (!Online) DestroyShield(); }
    [PunRPC] public void DestroyShield() { if (_shieldExplotion) Instantiate(_shieldExplotion, transform.position, transform.rotation); }
    [PunRPC] public void DestroyShadowHandGrab()
    {
        var hand = transform.Find("ShadowHandGrab(Clone)");
        if (hand && hand.GetComponent<ShadowHandGrabController>()) hand.GetComponent<ShadowHandGrabController>().Explote();
    }
    public void blocking() { if (anim) anim.SetBool("Blocking", true); }
    public void StopBlocking() { if (anim) anim.SetBool("Blocking", false); }
    private void DamageShield(int damage)
    {
        if (restoringShield) return;
        shieldDamage += damage; SetShieldbarDamage();
        if (shieldDamage >= 5f && !restoringShield)
        { StartCoroutine(RestoreShield()); restoringShield = true; ChangeButton(1, "B"); }
    }
    private void SetShieldbarDamage() { if (StatsController.i) StatsController.i.SetShieldBar(shieldDamage, Opponent); }
    public void ChangeButton(int color, string button)
    {
        if (StatsController.i) StatsController.i.ChangeColorIcon(color, button, Opponent);
        if (!Opponent && ButtonsController.i) ButtonsController.i.ChangeColor(color, button);
    }
    private IEnumerator RestoreShield()
    {
        yield return new WaitForSeconds(30f);
        shieldDamage = 0f; restoringShield = false; SetShieldbarDamage(); ChangeButton(0, "B");
    }
    [PunRPC] public void SetShieldBarOnline(float shieldDamage)
    { if (StatsController.i) StatsController.i.SetShieldBar(shieldDamage, true); }
    public void GetDamage(float amount, string effect = "")
    {
        if (Online) return;
        if (GameEvents.i) GameEvents.i.PlayerDamaged(gameObject.layer);
        if (LocalOfflinePlayerInput.IsLocalBattle) LocalOfflinePlayerInput.AddSpecialCharge(gameObject, .75f);
        else specialCharge += .75f;
        Damage += amount;
        if (anim) anim.SetFloat("Damage", Damage);
        DropAllAtks(); Invoke(nameof(DropAllAtks), .2f);
        if (!LocalOfflinePlayerInput.IsLocalBattle && !Opponent && SpecialAtk.specialAtk) SpecialAtk.specialAtk.ChargeSpecial(.05f);
        if (StatsController.i) StatsController.i.SetHealth(Damage, Opponent);
    }
    [PunRPC] public void SetDamageOnline(float damage) { Damage = damage; if (anim) anim.SetFloat("Damage", Damage); }
    public void ResetSpecialAtk()
    {
        if (LocalOfflinePlayerInput.IsLocalBattle) { specialCharge = 0f; return; }
        if (Opponent) return;
        if (SystemManager.i && SystemManager.i.mode == GameMode.Survival)
        { specialCharge = 0f; if (SpecialAtk.specialAtk) SpecialAtk.specialAtk.ResetCharge(); }
        else if (GameController.i) GameController.i.DisableSpecial();
    }
    public void ResetSpecialFreeze()
    {
        if (LocalOfflinePlayerInput.IsLocalBattle) { specialCharge = 0f; return; }
        if (!Opponent && SpecialAtk.specialAtk) SpecialAtk.specialAtk.ResetCharge();
    }
    public void RestoreDamage()
    {
        Damage = 0f; shieldDamage = 0f; restoringShield = false;
        if (anim) anim.SetFloat("Damage", 0f); SetShieldbarDamage();
    }
    public void ResetDamage()
    {
        if (Online) return;
        StopAllCoroutines(); RestoreDamage(); ChangeButton(0, "B");
        if (StatsController.i) StatsController.i.RestoreHealth(Opponent);
    }
    [PunRPC] public void ResetDamageOnline(int player) { Debug.LogWarning("Online damage reset is unavailable.", this); }
    public void weak() { if (anim) anim.SetBool("Weak", true); }
    public void ShieldOff() { StopBlocking(); }
    public void Recover() { StartCoroutine(RecoverStats()); }
    private IEnumerator RecoverStats()
    {
        yield return new WaitForSeconds(.1f);
        if (!anim) yield break;
        anim.SetBool("Stunned", false); anim.SetBool("Weak", false); anim.SetBool("Blocking", false); anim.ResetTrigger("Block");
        var controller = anim.GetComponentInParent<ColliderController>(); if (controller) controller.recoverCollider();
    }
    private IEnumerator TryToFreeze()
    {
        yield return new WaitForSeconds(.5f);
        while (State("Freeze"))
        {
            var opponent = Opponent ? GameController.i.MainCharacter : GameController.i.OpponentCharacter;
            if (opponent && opponent.GetComponent<Animator>().GetBool("Weak")) PlayHitAnimation("hit1");
            yield return new WaitForFixedUpdate();
        }
    }
}
