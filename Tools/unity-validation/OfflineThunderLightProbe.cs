#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class OfflineThunderLightProbe
{
    [Serializable] private sealed class LightSample
    {
        public string name;
        public float born, peak, extinguishedAge = -1;
        [NonSerialized] public Light light;
    }

    public static IEnumerator Run(GameController game, string output, Action<string,string> observe, Action<string> fail)
    {
        var reader = BattleSceneSingleton.i.inputReader;
        var animator = game.animMainChar;
        // Desktop hides the old mobile UGUI zones. Use their production input callbacks.
        Action<bool> press = active => { reader.SetAButton(active); game.Attacking(active); };
        bool pressed = false;
        var samples = new Dictionary<LightOn, LightSample>();
        bool summonedAtTarget = false;
        Action<EntityId> summoned = id => {
            var marker = UnityEngine.Object.FindObjectsByType<SumonInOpponent>(FindObjectsInactive.Exclude)
                .FirstOrDefault(s => s.name == "SumonThunderAtk2(Clone)" && s.gameObject.GetEntityId().Equals(id));
            if (marker) summonedAtTarget = Vector3.Distance(marker.transform.position, game.OpponentCharacter.transform.position) < .05f;
        };
        var events = GameEvents.i;
        events.onSummonedInOpp += summoned;
        try
        {
            for (int variant = 0; variant < 2; variant++)
            {
                string state = variant == 0 ? "DownAttack" : "DownAttack2";
                reader.SetYpos(-1);
                yield return new WaitForSeconds(.07f);
                press(true); pressed = true;
                float deadline = Time.time + 7f;
                while (!animator.GetCurrentAnimatorStateInfo(0).IsName(state) && Time.time < deadline) yield return null;
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName(state)) { fail("Thunder actual DOWN+A failed to enter " + state); yield break; }
                press(false); pressed = false;
                reader.SetYpos(0);
                if (variant == 0)
                {
                    yield return new WaitForSeconds(.1f);
                    while ((animator.GetCurrentAnimatorStateInfo(0).IsName(state) || animator.GetBool("Stunned") || animator.IsInTransition(0)) && Time.time < deadline) yield return null;
                    if (animator.GetInteger("DownAttack") != 1) { fail("Thunder primer did not naturally alternate to variant 2"); yield break; }
                    // Let the primer effects expire before observing the summoned strike.
                    yield return new WaitForSeconds(3f);
                }
            }
            bool sawImpact = false;
            float until = Time.time + 4.5f;
            while (Time.time < until)
            {
                sawImpact |= UnityEngine.Object.FindObjectsByType<Description>(FindObjectsInactive.Exclude).Any(d => d.name == "SumonThunderAtk(Clone)");
                foreach (var owner in UnityEngine.Object.FindObjectsByType<LightOn>(FindObjectsInactive.Exclude))
                {
                    if (!owner.off) continue;
                    var light = owner.GetComponent<Light>();
                    if (!light) continue;
                    if (!samples.TryGetValue(owner, out var sample))
                    {
                        sample = new LightSample { name = owner.transform.parent.name + "/" + owner.name, born = Time.time, light = light };
                        samples.Add(owner, sample);
                    }
                    sample.peak = Mathf.Max(sample.peak, light.intensity);
                    float age = Time.time - sample.born;
                    if (sample.peak > .1f && age > owner.delayOff && light.intensity <= .001f && sample.extinguishedAge < 0) sample.extinguishedAge = age;
                    if (age > 2.25f && light.intensity > .001f) { fail("Thunder ground light still illuminated after 2.25 scaled seconds: " + sample.name); yield break; }
                }
                yield return null;
            }
            if (!summonedAtTarget || !sawImpact || samples.Count < 2 || samples.Values.Any(s => s.peak <= .1f || s.extinguishedAge < 0 || s.extinguishedAge > 2.25f))
            { fail("Thunder target/impact/light fade was not observed: target=" + summonedAtTarget + "; impact=" + sawImpact + "; lights=" + samples.Count + "; " + string.Join("; ", samples.Values.Select(s => s.name + " peak=" + s.peak + " zeroAt=" + s.extinguishedAge))); yield break; }
            observe("thunder-light-fade", game.MainCharacter.name + "; actual DownAttack->DownAttack2; target=" + summonedAtTarget + "; strike=" + sawImpact + "; " + string.Join("; ", samples.Values.Select(s => s.name + " peak=" + s.peak + " zeroAt=" + s.extinguishedAge)));
            ScreenCapture.CaptureScreenshot(System.IO.Path.ChangeExtension(output, ".thunder-" + game.MainCharacter.name + ".png"));
            yield return new WaitForEndOfFrame(); yield return null;
        }
        finally
        {
            if (pressed) press(false);
            reader.SetYpos(0);
            events.onSummonedInOpp -= summoned;
        }
    }
}
#endif
