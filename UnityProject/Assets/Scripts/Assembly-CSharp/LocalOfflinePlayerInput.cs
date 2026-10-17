using UnityEngine;

/// <summary>Reads player 2's independently saved bindings during a local offline battle.</summary>
public sealed class LocalOfflinePlayerInput : MonoBehaviour
{
    private Animator anim;
    private bool specialWasHeld;
    private const float MoveSpeed = 7f;

    private void Start() { anim = GetComponent<Animator>(); }

    private void Update()
    {
        var game = GameController.i;
        if (!game || !game.gameStarted || !anim || Time.timeScale <= 0f || game.online) return;
        InputSet input = LocalControls.ReadPlayer(1);
        float horizontal = input.x > .3f ? 1f : input.x < -.3f ? -1f : 0f;
        float vertical = input.y > .3f ? 1f : input.y < -.3f ? -1f : 0f;
        anim.SetFloat("SHorizontal", horizontal, .2f, Time.deltaTime);
        anim.SetFloat("SVertical", vertical, .2f, Time.deltaTime);
        anim.SetFloat("Horizontal", horizontal);
        anim.SetFloat("Vertical", vertical);
        if (!anim.GetBool("Stunned"))
            transform.position += (transform.forward * vertical + transform.right * horizontal).normalized * MoveSpeed * Time.deltaTime;

        anim.SetBool("Attack", input.a && BattleCanStart());
        anim.SetBool("Jump", input.b);
        if (input.s && !specialWasHeld) TrySpecial();
        specialWasHeld = input.s;
    }

    private static bool BattleCanStart()
    {
        var counter = BattleSceneSingleton.i ? BattleSceneSingleton.i.counter : null;
        return object.ReferenceEquals(counter, null) || counter.value < 0f;
    }

    private void TrySpecial()
    {
        TryActivateSpecial(gameObject);
    }

    public static bool IsLocalBattle => SystemManager.i && SystemManager.i.mode == GameMode.LocalOffline;
    public const float SpecialCapacity = 10f;

    public static bool IsSpecialReady(GameObject fighter)
    {
        var damage = fighter ? fighter.GetComponent<DamageOnCollision>() : null;
        var states = fighter ? fighter.GetComponent<AnimatorStatesManager>() : null;
        return damage && damage.specialCharge >= SpecialCapacity && (!states || !states.awakening);
    }

    public static void AddSpecialCharge(GameObject fighter, float amount)
    {
        var damage = fighter ? fighter.GetComponent<DamageOnCollision>() : null;
        var states = fighter ? fighter.GetComponent<AnimatorStatesManager>() : null;
        if (!damage || (states && states.awakening)) return;
        damage.specialCharge = Mathf.Clamp(damage.specialCharge + amount, 0f, SpecialCapacity);
    }

    public static bool TryActivateSpecial(GameObject fighter)
    {
        var game = GameController.i;
        var animator = fighter ? fighter.GetComponent<Animator>() : null;
        if (!IsLocalBattle || !game || !game.gameStarted || Time.timeScale <= 0f || !BattleCanStart() ||
            !animator || animator.GetBool("Stunned") || !IsSpecialReady(fighter)) return false;
        // Local versus gives both players the same access, independently of campaign unlocks.
        fighter.GetComponent<DamageOnCollision>().specialCharge = 0f;
        animator.SetTrigger("Special");
        animator.SetBool("Stunned", true);
        return true;
    }
}
