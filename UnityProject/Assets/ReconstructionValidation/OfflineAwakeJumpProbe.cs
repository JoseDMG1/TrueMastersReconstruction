#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public static class OfflineAwakeJumpProbe
{
    [Serializable] public class Report {
        public string fighter,state,expected,result,limitation;
        public bool entered,awakeningActive,recovered,restored;
        public float spawnPositionError,targetDamage;
        public OfflineFlightMatrixProbe.Case flight;
        public List<string> errors=new List<string>();
    }
    public static IEnumerator Run(GameController game,int index,string output,Action activateSpecial,Action<bool> defense,Action<string,string> observe,Action<string> fail) {
        var inventory=OfflineFlightMatrixProbe.Load();var route=inventory.jumps[index];
        var report=new Report{fighter=route.fighter,state=route.state,expected=route.name,limitation="Real Special charge/UI awakening and real evade UI plus authored CreateAwakeAtk event. Charge accelerated; target path/root motion/rigidbody held by fixture. Raven Follow yaw and Maya stationary trap checked separately. Contacts/damage observed, not required for airborne shots or floor traps; not every awakened normal or original VFX parity."};
        var special=SpecialAtk.specialAtk;var anim=game.animMainChar;var manager=game.MainCharacter.GetComponent<AnimatorStatesManager>();
        special.ChargeSpecial(1);special.ChargeSpecial(1);activateSpecial();
        double deadline=Time.realtimeSinceStartupAsDouble+9;
        while((!manager.awakening||anim.GetBool("Stunned")||anim.IsInTransition(0))&&Time.realtimeSinceStartupAsDouble<deadline)yield return null;
        if(!manager.awakening||!special.decharging){fail("Awakening did not arm awake-jump route");yield break;}
        var reader=BattleSceneSingleton.i.inputReader;
        var observer=new GameObject("AwakeJumpFlightObserver").AddComponent<OfflineFlightMatrixProbe>();
        var hold=new OfflineFlightMatrixProbe.Hold(game.OpponentCharacter.gameObject);
        var eventOwner=GameEvents.i;GameObject created=null;bool bound=false;
        Action<EntityId> creation=id=>{
            var obj=UnityEngine.Object.FindObjectsByType<Description>(FindObjectsInactive.Exclude).FirstOrDefault(d=>d.gameObject.GetEntityId().Equals(id));
            if(!obj||obj.name!=route.name+"(Clone)"||obj.gameObject.layer!=game.MainCharacter.layer)return;
            created=obj.gameObject;
            var offset=Resources.Load<GameObject>(route.resource).GetComponent<ChangeStartPosition>();
            Vector3 expectedPosition=game.MainCharacter.transform.position;
            if(offset)expectedPosition+=game.MainCharacter.transform.rotation*new Vector3(offset.right,offset.up,offset.forward);
            report.spawnPositionError=Vector3.Distance(created.transform.position,expectedPosition);
            var profile=inventory.profiles.FirstOrDefault(p=>p.name==route.name)??new OfflineFlightMatrixProbe.Profile{name=route.name};
            report.flight=new OfflineFlightMatrixProbe.Case{profile=route.name,scenario="awake",side=0};
            observer.Begin(profile,report.flight,created,game.MainCharacter.gameObject,game.OpponentCharacter.gameObject,hold,game.OpponentCharacter.transform.position);bound=true;
        };
        try {
            eventOwner.onAttackLayerSetUp+=creation;
            game.MainCharacter.transform.position=new Vector3(0,0,-8);
            var body=game.MainCharacter.GetComponent<Rigidbody>();if(body){body.position=game.MainCharacter.transform.position;if(!body.isKinematic){body.linearVelocity=Vector3.zero;body.angularVelocity=Vector3.zero;}}
            hold.Place(new Vector3(0,0,17));Physics.SyncTransforms();
            reader.SetXpos(route.state=="JumpL"?-1:route.state=="JumpR"?1:0);reader.SetYpos(route.state=="roll"?1:0);
            yield return new WaitForSeconds(.06f);yield return null;
            defense(true);
            deadline=Time.realtimeSinceStartupAsDouble+3;
            while(Time.realtimeSinceStartupAsDouble<deadline) {if(anim.GetCurrentAnimatorStateInfo(0).IsName(route.state)){report.entered=true;break;}yield return null;}
            defense(false);reader.SetXpos(0);reader.SetYpos(0);
            deadline=Time.realtimeSinceStartupAsDouble+6;
            while(Time.realtimeSinceStartupAsDouble<deadline) {
                if(bound && !anim.GetCurrentAnimatorStateInfo(0).IsName(route.state)&&!anim.GetBool("Stunned")&&!anim.IsInTransition(0)){report.recovered=true;break;}
                yield return null;
            }
            yield return new WaitForSeconds(.7f);
            report.awakeningActive=manager.awakening&&special.decharging;
            if(bound)observer.End();
            if(!report.entered||!report.recovered||!report.awakeningActive)report.errors.Add("Awake evade state/active window/recovery failed");
            if(!bound)report.errors.Add("Authored awake object did not spawn");
            if(report.spawnPositionError>.001f)report.errors.Add("CreateAwakeAtk spawned away from owner pose");
            if(report.flight!=null){report.errors.AddRange(report.flight.errors);if(route.fighter=="Maya"&&report.flight.horizontalTravel>.01f)report.errors.Add("Maya floor trap moved toward target");}
            report.targetDamage=game.OpponentCharacter.GetComponent<DamageOnCollision>().Damage;
            ScreenCapture.CaptureScreenshot(Path.ChangeExtension(output,".png"));yield return new WaitForEndOfFrame();
        } finally {
            eventOwner.onAttackLayerSetUp-=creation;defense(false);reader.SetXpos(0);reader.SetYpos(0);hold.Dispose();UnityEngine.Object.Destroy(observer.gameObject);report.restored=true;if(report.flight!=null)report.flight.restored=true;
        }
        report.result=report.errors.Count==0?"passed":"probe_failed";File.WriteAllText(output,JsonUtility.ToJson(report,true));
        observe("awake-jump",route.fighter+"; state="+route.state+"; object="+route.name+"; result="+report.result+"; frames="+(report.flight?.frames??0)+"; turns="+(report.flight?.turns??0)+"; contacts="+(report.flight?.contacts??0)+"; damage="+report.targetDamage+"; trace="+output);
        if(report.result!="passed")fail("Awake jump "+route.fighter+" "+route.state+": "+string.Join("; ",report.errors));
    }
}
#endif
