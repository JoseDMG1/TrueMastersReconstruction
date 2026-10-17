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
        string key = SpecialPreferenceKey(gameObject.name);
        var damage = GetComponent<DamageOnCollision>();
        if (string.IsNullOrEmpty(key) || !PlayerPrefs.HasKey(key) || !damage || damage.specialCharge <= 10f) return;
        damage.specialCharge = 0f;
        anim.SetTrigger("Special");
        anim.SetBool("Stunned", true);
    }

    private static string SpecialPreferenceKey(string fighter)
    {
        switch (fighter)
        {
            case "Rina(Clone)": case "Amon(Clone)": return "Rina";
            case "Dante(Clone)": case "Agata(Clone)": return "Dante";
            case "Eliot(Clone)": case "Vinea(Clone)": return "Eliot";
            case "Sora(Clone)": case "Aura(Clone)": return "Aura";
            case "Diana(Clone)": case "Ren(Clone)": return "Ren";
            default: return null;
        }
    }
}
