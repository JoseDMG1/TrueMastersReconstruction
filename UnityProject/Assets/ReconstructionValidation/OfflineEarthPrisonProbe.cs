#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

// Opt-in test helper; parent supplies a fresh Diana/Ren arena and disables CPU AI.
public static class OfflineEarthPrisonProbe
{
    public static IEnumerator Run(GameController game, Action<string, string> observe, Action<string> fail, bool miss)
    {
        var reader = BattleSceneSingleton.i.inputReader;
        var anim = game.animMainChar;
        var enemy = game.OpponentCharacter;
        var enemyAnim = enemy.GetComponent<Animator>();
        var zones = UnityEngine.Object.FindObjectsByType<EventTrigger>(FindObjectsInactive.Exclude)
            .Where(t => t.triggers.Any(e => e.eventID == EventTriggerType.PointerEnter &&
                Enumerable.Range(0, e.callback.GetPersistentEventCount()).Any(i => e.callback.GetPersistentMethodName(i) == "SetAButton"))).ToArray();
        if (zones.Length != 1 || !EventSystem.current) { fail("Earth prison requires one active SetAButton pointer zone"); yield break; }
        var zone = zones[0];
        var events = GameEvents.i;
        var priorInput = reader.inputSet.Clone();
        bool priorDpad = reader.dPad, priorKeyboard = reader.keyboard, pressed = false;
        PrisonController prison = null;
        Vector3[] wallOrigins = null, reverseOrigins = null;
        float[] closingTravel = null, reverseTravel = null;
        float born = -1f, destroyedAge = -1f, colliderOpenedAge = -1f, colliderClosedAge = -1f;
        bool positioned = false, trapped = false, sawPrisonFree = false, sawReverse = false, sawClosedCaught = false;
        string observerError = null;
        Action<EntityId> summoned = id =>
        {
            var found = UnityEngine.Object.FindObjectsByType<PrisonController>(FindObjectsInactive.Exclude)
                .FirstOrDefault(p => p.gameObject.GetEntityId().Equals(id) && p.gameObject.layer == game.MainCharacter.layer);
            if (!found) return;
            if (prison) { observerError = "Multiple EarthPrison roots were created"; return; }
            prison = found;
            born = Time.time;
            positioned = Vector3.Distance(found.transform.position, enemy.transform.position) < .05f;
            if (found.walls == null || found.walls.Count != 4 || found.walls.Any(w => !w || w.GetComponents<MoveObject>().Length != 2))
            { observerError = "EarthPrison must own four walls with two ordered MoveObject components each"; return; }
            wallOrigins = found.walls.Select(w => w.transform.localPosition).ToArray();
            closingTravel = new float[4]; reverseTravel = new float[4];
            if (miss)
            {
                enemy.transform.position += enemy.transform.right * 10f;
                var body = enemy.GetComponent<Rigidbody>();
                if (body) { body.linearVelocity = Vector3.zero; body.angularVelocity = Vector3.zero; }
                observe("earth-prison-miss-fixture", "CPU moved sideways10 after real SummonedInOpp, before collider opens; no forced animator/event/destruction");
            }
        };
        Action<int> trap = layer => { if (prison && layer == enemy.layer) trapped = true; };
        try
        {
            reader.dPad = true; reader.keyboard = false;
            reader.SetXpos(0); reader.SetYpos(0);
            double deadline = Time.realtimeSinceStartupAsDouble + 8;
            while ((anim.GetBool("Stunned") || anim.IsInTransition(0)) && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
            if (anim.GetInteger("DownAttack") != 0 || anim.GetBool("Stunned"))
            { fail("Fresh Earth prison arena must be ready with natural DownAttack variant0"); yield break; }

            reader.SetYpos(-1);
            yield return new WaitForSeconds(.06f);
            ExecuteEvents.Execute(zone.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
            pressed = true;
            bool entered = false;
            while (Time.realtimeSinceStartupAsDouble < deadline)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("DownAttack")) { entered = true; break; }
                yield return null;
            }
            ExecuteEvents.Execute(zone.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            pressed = false; reader.SetYpos(0);
            if (!entered) { fail("Actual DOWN+A did not enter primer DownAttack"); yield break; }
            while (Time.realtimeSinceStartupAsDouble < deadline)
            {
                bool primerAlive = UnityEngine.Object.FindObjectsByType<Description>(FindObjectsInactive.Exclude)
                    .Any(d => d.name == "RocksJump(Clone)" && d.gameObject.layer == game.MainCharacter.layer);
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("DownAttack") && !anim.GetBool("Stunned") && !anim.IsInTransition(0)
                    && !enemyAnim.GetBool("Stunned") && !enemyAnim.IsInTransition(0) && !primerAlive) break;
                yield return null;
            }
            if (Time.realtimeSinceStartupAsDouble >= deadline || anim.GetInteger("DownAttack") != 1)
            { fail("DownAttack primer did not recover/expire and naturally alternate to variant1 within8s"); yield break; }
            observe("earth-prison-primer", "Real PointerEnter/Exit DOWN+A entered DownAttack, recovered and changed variant0→1; primer rocks expired naturally; preparation bounded8s");

            events.onSummonedInOpp += summoned;
            events.OnPlayerPrisonTrap += trap;
            reader.SetYpos(-1);
            yield return new WaitForSeconds(.06f);
            deadline = Time.realtimeSinceStartupAsDouble + 8;
            ExecuteEvents.Execute(zone.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
            pressed = true; entered = false;
            while (Time.realtimeSinceStartupAsDouble < deadline)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("DownAttack2")) { entered = true; break; }
                yield return null;
            }
            ExecuteEvents.Execute(zone.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            pressed = false; reader.SetYpos(0);
            if (!entered) { fail("Actual DOWN+X did not enter DownAttack2 after native alternation"); yield break; }
            bool sawRoot = false;
            while (Time.realtimeSinceStartupAsDouble < deadline)
            {
                if (observerError != null) { fail(observerError); yield break; }
                if (prison)
                {
                    sawRoot = true;
                    float age = Time.time - born;
                    var box = prison.GetComponent<BoxCollider>();
                    if (!box || Vector3.Distance(box.size, new Vector3(7, 10, 7)) > .01f)
                    { fail("EarthPrison root collider dimensions differ from authored7x10x7"); yield break; }
                    if (box.enabled && colliderOpenedAge < 0) colliderOpenedAge = age;
                    if (!box.enabled && colliderOpenedAge >= 0 && colliderClosedAge < 0) colliderClosedAge = age;
                    sawPrisonFree |= enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("PrisonFree");
                    bool allReverse = true, anyReverse = false;
                    for (int i = 0; i < 4; i++)
                    {
                        var moves = prison.walls[i].GetComponents<MoveObject>();
                        bool reversed = !moves[0].enabled && moves[1].enabled;
                        allReverse &= reversed; anyReverse |= moves[1].enabled;
                        if (age <= 1.55f && !moves[1].enabled)
                            closingTravel[i] = Mathf.Max(closingTravel[i], Vector3.Distance(wallOrigins[i], prison.walls[i].transform.localPosition));
                    }
                    if (anyReverse && !miss) { fail("Caught EarthPrison unexpectedly enabled a reverse wall"); yield break; }
                    if (allReverse)
                    {
                        if (!sawReverse) reverseOrigins = prison.walls.Select(w => w.transform.localPosition).ToArray();
                        sawReverse = true;
                        for (int i = 0; i < 4; i++)
                            reverseTravel[i] = Mathf.Max(reverseTravel[i], Vector3.Distance(reverseOrigins[i], prison.walls[i].transform.localPosition));
                    }
                    if (!miss && age >= 1.6f && trapped && !box.enabled && !anyReverse) sawClosedCaught = true;
                }
                else if (sawRoot) { destroyedAge = Time.time - born; break; }
                yield return null;
            }
            observe("earth-prison-lifecycle", "miss=" + miss + "; positioned=" + positioned + "; trap=" + trapped + "; PrisonFree=" + sawPrisonFree
                + "; colliderOpenAge=" + colliderOpenedAge + "; colliderClosedAge=" + colliderClosedAge + "; reverse=" + sawReverse
                + "; closureTravel=" + (closingTravel == null ? "none" : string.Join(",", closingTravel.Select(x => x.ToString("F3"))))
                + "; reverseTravel=" + (reverseTravel == null ? "none" : string.Join(",", reverseTravel.Select(x => x.ToString("F3"))))
                + "; destroyedAge=" + destroyedAge + "; case observation bounded8s; real physics/events, synthetic CPU displacement only for miss");
            if (!sawRoot || !positioned || colliderOpenedAge < .9f || colliderOpenedAge > 1.3f || colliderClosedAge < 1.4f || colliderClosedAge > 1.8f
                || closingTravel == null || closingTravel.Any(x => x < .5f))
            { fail("EarthPrison did not show real positioning, timed collider and closure movement on all four walls"); yield break; }
            if (miss)
            {
                if (trapped || sawPrisonFree || !sawReverse || reverseTravel.Any(x => x < .25f) || destroyedAge < 3.8f || destroyedAge > 4.6f)
                { fail("Missed EarthPrison did not reverse all walls and expire naturally near4s"); yield break; }
            }
            else if (!trapped || !sawPrisonFree || !sawClosedCaught || sawReverse || destroyedAge < 5.5f || destroyedAge > 6.4f)
            { fail("Caught EarthPrison did not trap through physics, retain closed walls and expire naturally near5.75s"); yield break; }
            observe("earth-prison-accepted", miss ? "Miss/reverse/real wall travel/natural deletion accepted" : "Centered trap/PrisonFree/closed wall retention/natural expiry accepted");
        }
        finally
        {
            if (events != null) { events.onSummonedInOpp -= summoned; events.OnPlayerPrisonTrap -= trap; }
            if (pressed && zone && EventSystem.current)
                ExecuteEvents.Execute(zone.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            if (reader)
            {
                reader.SetXpos(priorInput.x); reader.SetYpos(priorInput.y);
                reader.SetAButton(priorInput.a); reader.SetBButton(priorInput.b); reader.SetSButton(priorInput.s);
                reader.dPad = priorDpad; reader.keyboard = priorKeyboard;
            }
        }
    }
}
#endif
