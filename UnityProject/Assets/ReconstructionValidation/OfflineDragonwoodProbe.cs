#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

// Opt-in acceptance helper. Caller owns the scene, disabled CPU and saved fixture.
public static class OfflineDragonwoodProbe
{
    static bool Calls(EventTrigger trigger, EventTriggerType type, string method)
    {
        return trigger.triggers.Any(e => e.eventID == type &&
            Enumerable.Range(0, e.callback.GetPersistentEventCount()).Any(i => e.callback.GetPersistentMethodName(i) == method));
    }
    static void Enter(EventTrigger trigger)
    {
        ExecuteEvents.Execute(trigger.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
    }
    static void Exit(EventTrigger trigger)
    {
        if (trigger && EventSystem.current)
            ExecuteEvents.Execute(trigger.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
    }
    static bool Recovering(Animator anim)
    {
        return anim.GetBool("Stunned") || anim.IsInTransition(0) ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("DownAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("DownAttack2");
    }
    public static IEnumerator Run(GameController game, Action<string, string> observe, Action<string> fail)
    {
        if (!game || !game.MainCharacter || !game.MainCharacter.name.StartsWith("Maya", StringComparison.Ordinal) ||
            !game.gameStarted || !BattleSceneSingleton.i || !EventSystem.current)
        { fail("DragonWood fixture requires a running Maya scene and EventSystem."); yield break; }
        var reader = BattleSceneSingleton.i.inputReader;
        var anim = game.animMainChar;
        var triggers = UnityEngine.Object.FindObjectsByType<EventTrigger>(FindObjectsInactive.Exclude);
        var attacks = triggers.Where(t => Calls(t, EventTriggerType.PointerEnter, "SetAButton")).ToArray();
        if (!reader || !anim || attacks.Length != 1)
        { fail("DragonWood expected one active SetAButton zone; got " + attacks.Length); yield break; }
        var attack = attacks[0];
        bool oldDpad = reader.dPad, oldKeyboard = reader.keyboard;
        float oldX = reader.inputSet.x, oldY = reader.inputSet.y;
        GameObject dragon = null;
        GameEvents eventOwner = null;
        Action<int> damageObserver = null;
        DamageOnCollision collisionOwner = null;
        UnityAction collisionObserver = null;
        GameObject overlapAttack = null;
        try
        {
            reader.dPad = true;
            reader.keyboard = false;
            reader.SetXpos(0f); reader.SetYpos(0f);
            observe("dragonwood-scope", "InputReader.SetYpos(-1) and actual SetAButton UI; native alternation0 then1. No attack transform/state/variant mutation. CPU and save fixture owned by caller.");
            for (int variant = 0; variant < 2; variant++)
            {
                double deadline = Time.realtimeSinceStartupAsDouble + 16;
                while (Recovering(anim) && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
                if (Recovering(anim) || anim.GetInteger("DownAttack") != variant)
                { fail("DragonWood did not naturally recover with DownAttack variant " + variant + "; actual=" + anim.GetInteger("DownAttack")); yield break; }
                reader.SetYpos(-1f);
                yield return null;
                Enter(attack);
                int wanted = Animator.StringToHash(variant == 0 ? "DownAttack" : "DownAttack2");
                bool entered = false;
                deadline = Time.realtimeSinceStartupAsDouble + 4;
                while (Time.realtimeSinceStartupAsDouble < deadline)
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).shortNameHash == wanted) { entered = true; break; }
                    yield return null;
                }
                Exit(attack); reader.SetYpos(0f);
                if (!entered) { fail("DragonWood actual input did not enter downward variant " + variant); yield break; }
                observe("dragonwood-input", "variant=" + variant + "; state=" + (variant == 0 ? "DownAttack" : "DownAttack2") + "; input y=" + reader.inputSet.y);
                if (variant == 0)
                {
                    deadline = Time.realtimeSinceStartupAsDouble + 16;
                    while (Recovering(anim) && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
                    if (Recovering(anim)) { fail("Maya did not leave first downward attack before alternation."); yield break; }
                }
            }
            double spawnDeadline = Time.realtimeSinceStartupAsDouble + 5;
            MoveForward forward = null;
            while (!forward && Time.realtimeSinceStartupAsDouble < spawnDeadline)
            {
                forward = UnityEngine.Object.FindObjectsByType<MoveForward>(FindObjectsInactive.Exclude)
                    .FirstOrDefault(m => m.name == "DragonWoodATK(Clone)" && m.gameObject.layer == game.MainCharacter.layer);
                if (!forward) yield return null;
            }
            if (!forward) { fail("DownAttack2 did not produce the real DragonWoodATK."); yield break; }
            dragon = forward.gameObject;
            var move = dragon.GetComponentInChildren<MovePath>();
            var history = dragon.GetComponentInChildren<PathManager>();
            var parts = dragon.GetComponentsInChildren<FollowPath>();
            if (!move || !history || parts.Length != 12)
            { fail("DragonWood missing path owners or twelve authored followers."); yield break; }
            float initialSpeed = forward.speed;
            if (Mathf.Abs(initialSpeed) > .001f)
            { fail("DragonWood initial speed0 was missed or changed: " + initialSpeed); yield break; }
            Vector3 rootBefore = dragon.transform.position, rootPrevious = rootBefore;
            Vector3 headBefore = move.transform.localPosition, headPrevious = move.transform.position;
            var partPrevious = parts.ToDictionary(p => p.GetEntityId(), p => p.transform.position);
            var partTravel = parts.ToDictionary(p => p.GetEntityId(), p => 0f);
            var minPositionError = parts.ToDictionary(p => p.GetEntityId(), p => float.PositiveInfinity);
            var minRotationError = parts.ToDictionary(p => p.GetEntityId(), p => float.PositiveInfinity);
            var movingPoseMatches = parts.ToDictionary(p => p.GetEntityId(), p => 0);
            float rootTravel = 0f, headTravel = 0f, headLocalRange = 0f, maxLocalX = headBefore.x, minLocalX = headBefore.x;
            float maxHeadY = headBefore.y, minHeadY = headBefore.y;
            float observedAt = Time.time, speedAt = -1f;
            int samples = 0;
            double movementDeadline = Time.realtimeSinceStartupAsDouble + 35;
            while (Time.time - observedAt < 5.2f && Time.realtimeSinceStartupAsDouble < movementDeadline)
            {
                yield return null;
                if (!dragon || !move || !history) { fail("DragonWood disappeared before the bounded path observation completed."); yield break; }
                if (history.points == null || history.points.Count != 60 || parts.Any(p => !p || p.pathManager != history || p.pathPoint < 0 || p.pathPoint >= 60))
                { fail("DragonWood history/assigned follower index contract changed."); yield break; }
                if (speedAt < 0f && Mathf.Abs(forward.speed - 13f) < .001f) speedAt = Time.time - observedAt;
                rootTravel += Vector3.Distance(rootPrevious, dragon.transform.position); rootPrevious = dragon.transform.position;
                headTravel += Vector3.Distance(headPrevious, move.transform.position); headPrevious = move.transform.position;
                headLocalRange = Mathf.Max(headLocalRange, Vector3.Distance(headBefore, move.transform.localPosition));
                minLocalX = Mathf.Min(minLocalX, move.transform.localPosition.x); maxLocalX = Mathf.Max(maxLocalX, move.transform.localPosition.x);
                minHeadY = Mathf.Min(minHeadY, move.transform.localPosition.y); maxHeadY = Mathf.Max(maxHeadY, move.transform.localPosition.y);
                foreach (var part in parts)
                {
                    EntityId id = part.GetEntityId();
                    partTravel[id] += Vector3.Distance(partPrevious[id], part.transform.position);
                    partPrevious[id] = part.transform.position;
                    var point = history.points[part.pathPoint];
                    float positionError = Vector3.Distance(part.transform.position, point.position);
                    float rotationError = Quaternion.Angle(part.transform.rotation, point.rotation);
                    minPositionError[id] = Mathf.Min(minPositionError[id], positionError);
                    minRotationError[id] = Mathf.Min(minRotationError[id], rotationError);
                    if (rootTravel > 1f && positionError <= .05f && rotationError <= 1f) movingPoseMatches[id]++;
                }
                samples++;
            }
            observe("dragonwood-motion", "scaledSeconds=" + (Time.time-observedAt) + "; samples=" + samples + "; speed0=" + initialSpeed + "; speed13At=" + speedAt +
                "; rootTravel=" + rootTravel + "; headTravel=" + headTravel + "; headLocalRange=" + headLocalRange + "; localXRange=" + (maxLocalX-minLocalX) +
                "; localYRange=" + (maxHeadY-minHeadY) + "; history=" + history.points.Count + "; jumps=" + move.numberOfJumps);
            observe("dragonwood-followers", string.Join(";", parts.Select(p => p.name + " index=" + p.pathPoint + " travel=" + partTravel[p.GetEntityId()] +
                " bestPositionError=" + minPositionError[p.GetEntityId()] + " bestRotationError=" + minRotationError[p.GetEntityId()] + " movingPoseMatches=" + movingPoseMatches[p.GetEntityId()])));
            if (Time.time-observedAt < 5f || speedAt < 0f || speedAt > 2f || Mathf.Abs(forward.speed-13f) > .001f || rootTravel <= 1f ||
                headTravel <= 1f || headLocalRange <= 1f || maxLocalX-minLocalX <= 2f || maxHeadY-minHeadY <= .5f || move.numberOfJumps < 2 ||
                parts.Any(p => partTravel[p.GetEntityId()] <= 1f || movingPoseMatches[p.GetEntityId()] < 3))
            { fail("DragonWood bounded speed, oscillation or follower pose acceptance failed."); yield break; }

            // GetDamage has a real GameEvents event but does NOT invoke the
            // onDamageRecived UnityEvent used by StopMovePath. Do not synthesize it.
            var damage = game.MainCharacter.GetComponent<DamageOnCollision>();
            var config = dragon.GetComponent<ConfigAttackLayer>();
            eventOwner = GameEvents.i;
            int damageEvents = 0;
            damageObserver = layer => { if (game.MainCharacter && layer == game.MainCharacter.layer) damageEvents++; };
            if (!damage || !config || !eventOwner) { fail("DragonWood damage fixture dependencies absent."); yield break; }
            eventOwner.OnPlayerDamaged += damageObserver;
            float damageBefore = damage.Damage;
            damage.GetDamage(.5f);
            yield return null;
            observe("dragonwood-synthetic-owner-damage", "GetDamage(.5); delta=" + (damage.Damage-damageBefore) + "; realGameEvents=" + damageEvents +
                "; playerWeak=" + config.playerWeak + "; jumps=" + move.numberOfJumps + "; max=" + move.maxJumps +
                "; limitation=GetDamage alone does not emit onDamageRecived; collision fixture follows");
            if (damageEvents != 1 || Mathf.Abs(damage.Damage-damageBefore-.5f) > .001f || !config.playerWeak)
            { fail("Synthetic GetDamage did not emit its real owner event and weaken DragonWood."); yield break; }
            var airPrefab = Resources.Load<GameObject>("PhotonPrefabs/AirAtk");
            var targetCollider = game.MainCharacter.GetComponent<Collider>();
            if (!airPrefab || !targetCollider || !targetCollider.enabled || damage.onDamageRecived == null)
            { fail("DragonWood collision fixture requires recovered AirAtk, enabled owner collider and damage event."); yield break; }
            double collisionDeadline = Time.realtimeSinceStartupAsDouble + 3;
            while (damage.Validations("Attack") && Time.realtimeSinceStartupAsDouble < collisionDeadline) yield return null;
            if (damage.Validations("Attack") || !move || move.numberOfJumps >= move.maxJumps)
            { fail("DragonWood owner collision remained rejected or jump limit was reached before the fixture."); yield break; }
            int jumpsBeforeCollision = move.numberOfJumps;
            int collisionEvents = 0, jumpsAtCollision = -1;
            collisionOwner = damage;
            collisionObserver = () =>
            {
                collisionEvents++;
                if (move) jumpsAtCollision = move.numberOfJumps;
            };
            collisionOwner.onDamageRecived.AddListener(collisionObserver);
            overlapAttack = UnityEngine.Object.Instantiate(airPrefab, targetCollider.bounds.center, game.OpponentCharacter.transform.rotation);
            overlapAttack.layer = 8;
            eventOwner.AttackLayerSetUp(overlapAttack.GetEntityId());
            observe("dragonwood-synthetic-owner-overlap", "Recovered AirAtk instantiated inside Maya collider; opposing layer8; normal AttackLayerSetUp emitted; jumpsBefore=" + jumpsBeforeCollision);
            collisionDeadline = Time.realtimeSinceStartupAsDouble + 2;
            while (collisionEvents == 0 && move && Time.realtimeSinceStartupAsDouble < collisionDeadline) yield return null;
            if (collisionEvents == 0 || !move || jumpsAtCollision != move.maxJumps || move.numberOfJumps != move.maxJumps)
            { fail("Real OnTriggerEnter/onDamageRecived did not interrupt DragonWood to maxJumps."); yield break; }
            float fallBefore = move.transform.localPosition.y;
            bool disabledThroughout = true;
            for (int frame = 0; frame < 3; frame++)
            {
                yield return null;
                if (!move) { fail("DragonWood disappeared during post-collision falling samples."); yield break; }
                disabledThroughout &= move.disableJump;
            }
            float fallDelta = move.transform.localPosition.y - fallBefore;
            observe("dragonwood-collision-interruption", "Synthetic recovered-projectile overlap; natural hit callback events=" + collisionEvents +
                "; jumpsBefore=" + jumpsBeforeCollision + "; jumpsAtCallback=" + jumpsAtCollision + "; max=" + move.maxJumps +
                "; disableJump=" + move.disableJump + "; localYDelta3Updates=" + fallDelta + "; expected=" + (move.dropSpeed * 3f));
            if (!disabledThroughout || Mathf.Abs(fallDelta - move.dropSpeed * 3f) > .002f || fallDelta >= 0f)
            { fail("StopMovePath collision interruption did not produce native per-Update falling without the jump curve."); yield break; }
            observe("dragonwood-limits", "Bounded real input, delayed movement, authored chain poses and synthetic-overlap collision interruption checked; natural aiming, miss variants, ten-second lifetime, post-destruction listener behavior and visual parity remain unverified.");
        }
        finally
        {
            if (eventOwner && damageObserver != null) eventOwner.OnPlayerDamaged -= damageObserver;
            if (collisionOwner && collisionObserver != null) collisionOwner.onDamageRecived.RemoveListener(collisionObserver);
            if (overlapAttack) UnityEngine.Object.Destroy(overlapAttack);
            Exit(attack);
            if (reader)
            {
                reader.SetAButton(false);
                reader.SetXpos(oldX); reader.SetYpos(oldY);
                reader.dPad = oldDpad; reader.keyboard = oldKeyboard;
            }
            if (dragon) UnityEngine.Object.Destroy(dragon);
        }
    }
}
#endif
