#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

// Native branch fixture: actual Animator states and timed coroutine/InvokeRepeating.
// Actor element and normalized time are controlled; this is not natural combat input.
public static class OfflineAnimationColliderProbe
{
    static readonly FieldInfo boundAnimator=typeof(DisableColliderAnim).GetField("anim",BindingFlags.NonPublic|BindingFlags.Instance);
    public static IEnumerator Run(GameController game,Action<string,string> observe,Action<string> fail)
    {
        yield return FlowerRoute(game,observe,fail);
        observe("anim-collider-scope","Controlled actor element/state with animation events disabled; real delayed binding and repeated polling. Not natural Light attack acceptance.");
        foreach(int layer in new[]{8,9})
        {
            var actor=layer==8 ? HostPlayer.hostPlayer.gameObject : GuestPlayer.guestPlayer.gameObject;
            var animator=actor.GetComponent<Animator>(); var description=actor.GetComponent<Description>();
            var originalState=animator.GetCurrentAnimatorStateInfo(0);
            string originalElement=description.element; float originalSpeed=animator.speed; bool originalEvents=animator.fireEvents;
            GameObject source=null;
            try
            {
                animator.speed=0; animator.fireEvents=false;
                source=new GameObject("Animation collider fixture"); source.layer=1; source.transform.position=new Vector3(500,60,500);
                var box=source.AddComponent<BoxCollider>(); box.isTrigger=true;
                var owner=source.AddComponent<DisableColliderAnim>(); owner.delay=.15f;
                yield return new WaitForSecondsRealtime(.05f);
                bool initialDelay=boundAnimator.GetValue(owner)==null;
                yield return new WaitForSecondsRealtime(.2f);
                bool waitsForLayer=boundAnimator.GetValue(owner)==null && !owner.IsInvoking("CheckAttack");
                source.layer=layer; yield return new WaitForSecondsRealtime(.12f);
                bool binds=(Animator)boundAnimator.GetValue(owner)==animator && owner.IsInvoking("CheckAttack");
                observe("anim-collider-binding","sourceLayer="+layer+"; expectedActor="+actor.name+"; initialDelay="+initialDelay+"; waitsForLayer1="+waitsForLayer+"; binds="+binds);
                if(!initialDelay||!waitsForLayer||!binds) { fail("Animation collider delayed/layer binding mismatch"); yield break; }
                description.element="Light";
                foreach(float time in new[]{.1f,.3f,.6f})
                {
                    animator.Play("UpAttack2",0,time); animator.Update(0);
                    var state=animator.GetCurrentAnimatorStateInfo(0);
                    if(!state.IsName("UpAttack2")) { fail("Fixture actor has no UpAttack2 state"); yield break; }
                    bool expectedEnabled=time!=.3f;
                    box.enabled=!expectedEnabled;
                    yield return new WaitForSecondsRealtime(.12f);
                    observe("anim-collider-window","sourceLayer="+layer+"; actualTime="+animator.GetCurrentAnimatorStateInfo(0).normalizedTime+"; expectedEnabled="+expectedEnabled+"; enabled="+box.enabled);
                    if(box.enabled!=expectedEnabled) { fail("Light UpAttack2 collider window mismatch"); yield break; }
                }
                description.element="Fire"; animator.Play("UpAttack2",0,.3f); animator.Update(0); box.enabled=true;
                yield return new WaitForSecondsRealtime(.12f);
                bool ignoresOtherElement=box.enabled;
                description.element="Light"; animator.Play(originalState.fullPathHash,0,.1f); animator.Update(0); box.enabled=false;
                yield return new WaitForSecondsRealtime(.12f);
                bool ignoresOtherState=!box.enabled;
                observe("anim-collider-filter","sourceLayer="+layer+"; nonLightUnchanged="+ignoresOtherElement+"; otherStateUnchanged="+ignoresOtherState);
                if(!ignoresOtherElement||!ignoresOtherState) { fail("Animation collider changed a nonmatching state/element"); yield break; }
            }
            finally
            {
                if(source) { source.SetActive(false); UnityEngine.Object.Destroy(source); }
                description.element=originalElement;
                animator.Play(originalState.fullPathHash,0,originalState.normalizedTime); animator.Update(0);
                animator.speed=originalSpeed; animator.fireEvents=originalEvents;
            }
            yield return null;
        }
    }
    static IEnumerator FlowerRoute(GameController game,Action<string,string> observe,Action<string> fail)
    {
        var reader=BattleSceneSingleton.i.inputReader;
        var zones=UnityEngine.Object.FindObjectsByType<EventTrigger>(FindObjectsInactive.Exclude).Where(t=>t.triggers.Any(e=>e.eventID==EventTriggerType.PointerEnter && Enumerable.Range(0,e.callback.GetPersistentEventCount()).Any(i=>e.callback.GetPersistentMethodName(i)=="SetAButton"))).ToArray();
        if(zones.Length!=1||!EventSystem.current) { fail("Flower route requires one authored attack pointer zone"); yield break; }
        bool entered=false,sawBeam=false,beamDisabled=true,beamDescriptionDisabled=true,correctParent=true,sawDamage=false;
        int samples=0; GameObject beam=null;
        try
        {
            // Controlled starting poses, actual directional/button route and unmodified clip events.
            game.MainCharacter.transform.position=new Vector3(0,0,5);
            game.OpponentCharacter.transform.position=new Vector3(0,0,-18);
            reader.SetYpos(1); yield return new WaitForSeconds(.06f);
            ExecuteEvents.Execute(zones[0].gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerEnterHandler);
            double deadline=Time.realtimeSinceStartupAsDouble+3;
            while(Time.realtimeSinceStartupAsDouble<deadline)
            {
                if(game.animMainChar.GetCurrentAnimatorStateInfo(0).IsName("UpAttack")) { entered=true; break; }
                yield return null;
            }
            ExecuteEvents.Execute(zones[0].gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerExitHandler); reader.SetYpos(0);
            if(!entered) { fail("Maya pointer input did not enter authored UpAttack"); yield break; }
            deadline=Time.realtimeSinceStartupAsDouble+12;
            while(Time.realtimeSinceStartupAsDouble<deadline)
            {
                if(!sawBeam)
                {
                    var producer=UnityEngine.Object.FindObjectsByType<sumonOnCollider>(FindObjectsInactive.Exclude).FirstOrDefault(p=>p.name=="FlowerBeam(Clone)"&&p.gameObject.layer==game.MainCharacter.layer);
                    if(producer) { beam=producer.gameObject; sawBeam=true; correctParent=beam.transform.parent==game.MainCharacter.transform; }
                }
                if(beam)
                {
                    samples++; beamDisabled &= !beam.GetComponent<BoxCollider>().enabled;
                    beamDescriptionDisabled &= !beam.GetComponent<Description>().enabled;
                }
                if(!sawDamage)
                    sawDamage=UnityEngine.Object.FindObjectsByType<Description>(FindObjectsInactive.Exclude).Any(d=>d.name=="FlowerSpecial(Clone)"&&d.gameObject.layer==game.MainCharacter.layer&&d.enabled&&d.GetComponent<Collider>()&&d.GetComponent<Collider>().enabled);
                if(sawBeam&&!beam) break;
                yield return null;
            }
            observe("flower-natural-route","UpAttack via directional InputReader and authored attack PointerEnter; beamSeen="+sawBeam+"; parentIsActor="+correctParent+"; samples="+samples+"; rootColliderStayedDisabled="+beamDisabled+"; rootDescriptionStayedDisabled="+beamDescriptionDisabled+"; naturalBeamExpiry="+(sawBeam&&!beam)+"; activeFlowerSpecial="+sawDamage);
            if(!sawBeam||samples<20||!correctParent||!beamDisabled||!beamDescriptionDisabled||beam||!sawDamage) fail("Natural Maya flower route did not match authored visual/damage split");
        }
        finally
        {
            ExecuteEvents.Execute(zones[0].gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerExitHandler); reader.ResetControls();
        }
    }
}
#endif
