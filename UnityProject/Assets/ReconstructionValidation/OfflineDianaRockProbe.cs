#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class OfflineDianaRockProbe
{
    [Serializable] public class Sample { public float time, rootY, visibleTop; }
    [Serializable] public class Case {
        public string prefab, route = "isolated prefab";
        public bool destroyed, passed;
        public List<Sample> samples = new List<Sample>();
    }
    [Serializable] public class Report {
        public string scope = "Isolated real animation-event prefabs at world origin, normal scaled-time Start/FixedUpdate/Destroy lifecycle. Does not establish natural evade input or original-game visual parity.";
        public List<Case> cases = new List<Case>();
    }
    public static IEnumerator Run(GameController game, string output, Action<string,string> observe, Action<string> fail)
    {
        var report = new Report();
        var prefabs = game.animMainChar.runtimeAnimatorController.animationClips
            .SelectMany(c => c.events).Select(e => e.objectReferenceParameter as GameObject)
            .Where(p => p && p.name.StartsWith("RockJumpFX", StringComparison.Ordinal)).Distinct().ToArray();
        if (prefabs.Length < 2) { fail("Diana rock probe needs Diana controller and both lateral animation-event references"); yield break; }
        foreach (var prefab in prefabs)
        {
            var item = new Case { prefab = prefab.name }; report.cases.Add(item);
            GameObject rock = UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            float started = Time.time;
            float[] sampleTimes = { .46f, .62f, 1f, 1.56f, 3.16f };
            try
            {
                foreach (float target in sampleTimes)
                {
                    while (Time.time - started < target) yield return null;
                    if (!rock) { item.destroyed = true; continue; }
                    var renderers = rock.GetComponentsInChildren<Renderer>();
                    item.samples.Add(new Sample { time = Time.time-started, rootY = rock.transform.position.y,
                        visibleTop = renderers.Length == 0 ? float.NaN : renderers.Max(r => r.bounds.max.y) });
                }
                item.passed = item.destroyed && item.samples.Count >= 4
                    && item.samples[0].rootY > 2f
                    && item.samples[0].rootY - item.samples[3].rootY > 2.5f
                    && item.samples[3].visibleTop < 0f;
                observe("diana-rock-lifecycle", JsonUtility.ToJson(item));
                if (!item.passed) fail("Diana rock did not rise, sink fully below floor by 1.56s and expire: " + prefab.name);
            }
            finally { if (rock) UnityEngine.Object.Destroy(rock); }
        }
        foreach (string state in new[] { "JumpL", "JumpR" })
        {
            var item = new Case { route = "Animator.Play " + state + "; real animation event, no physical input" };
            report.cases.Add(item);
            var existing = new HashSet<EntityId>(UnityEngine.Object.FindObjectsByType<DestroyByTime>(FindObjectsInactive.Exclude)
                .Select(o => o.gameObject.GetEntityId()));
            game.animMainChar.Play(state, 0, 0f);
            GameObject rock = null;
            double deadline = Time.realtimeSinceStartupAsDouble + 2;
            while (!rock && Time.realtimeSinceStartupAsDouble < deadline)
            {
                var found = UnityEngine.Object.FindObjectsByType<DestroyByTime>(FindObjectsInactive.Exclude)
                    .FirstOrDefault(o => o.name.StartsWith("RockJumpFX", StringComparison.Ordinal)
                        && !existing.Contains(o.gameObject.GetEntityId()));
                if (found) rock = found.gameObject;
                else yield return null;
            }
            if (!rock) { fail("Diana " + state + " animation did not create a lateral rock"); continue; }
            item.prefab = rock.name;
            // SummonAttack.Create places the root at the owner's world pose. Read current owner
            // ground level separately; the captured clone may already have ticked its first rise.
            float floorY = game.MainCharacter.transform.position.y;
            float started = Time.time;
            try
            {
                foreach (float target in new[] { .46f, .62f, 1f, 1.56f, 3.16f })
                {
                    while (Time.time-started < target) yield return null;
                    if (!rock) { item.destroyed = true; continue; }
                    item.samples.Add(new Sample { time = Time.time-started, rootY = rock.transform.position.y-floorY,
                        visibleTop = rock.GetComponentsInChildren<Renderer>().Max(r => r.bounds.max.y)-floorY });
                }
                item.passed = item.destroyed && item.samples.Count >= 4
                    && item.samples[0].rootY > 2f
                    && item.samples[0].rootY-item.samples[3].rootY > 2.5f
                    && item.samples[3].visibleTop < 0;
                observe("diana-rock-animation-event", JsonUtility.ToJson(item));
                if (!item.passed) fail("Diana real " + state + " rock lifecycle failed");
            }
            finally { if (rock) UnityEngine.Object.Destroy(rock); }
        }
        File.WriteAllText(output, JsonUtility.ToJson(report, true));
    }
}
#endif
