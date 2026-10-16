#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class OfflineAbilityAimProbe
{
    [Serializable] public class Profile { public string resource,type,id; public float delay,horizontalSpeed,verticalSpeed; }
    [Serializable] public class Profiles { public Profile[] profiles; }
    static readonly BindingFlags Private = BindingFlags.Instance|BindingFlags.NonPublic;
    public static IEnumerator Run(GameController game,string fighter,Action<string,string> observe,Action<string> fail,bool components)
    {
        var roots=new[]{game.MainCharacter,game.OpponentCharacter};
        Vector3[] original={roots[0].transform.position,roots[1].transform.position};
        try {
            for(int side=0;side<2;side++) for(int bearing=0;bearing<4;bearing++) {
                var root=roots[side];var other=roots[1-side];
                root.transform.position=new Vector3(0,0,0);
                other.transform.position=Quaternion.Euler(0,bearing*90+35,0)*Vector3.forward*9;
                root.transform.rotation=Quaternion.Euler(0,bearing*90+150,0);
                var look=root.GetComponent<LookAt>();
                if(!look) {fail(fighter+" missing LookAt side="+side);yield break;}
                look.Setup();
                yield return new WaitForEndOfFrame();
                Vector3 direction=other.transform.position-root.transform.position;direction.y=0;
                float error=Vector3.Angle(root.transform.forward,direction);
                if(look.target!=other.transform || error>.1f) {fail(fighter+" root facing error="+error+" side="+side);yield break;}
                // A same-layer child must resolve the same opposing root as native Setup.
                var child=new GameObject("AimLayerFixture");child.layer=root.layer;
                var childLook=child.AddComponent<LookAt>();childLook.Setup();
                bool childOk=childLook.target==other.transform;UnityEngine.Object.Destroy(child);
                observe("aim-root",fighter+"; side="+side+"; bearing="+bearing+"; degrees="+error+"; sameLayerChild="+childOk);
                if(!childOk){fail("Layer-based target selection failed");yield break;}
            }
            if(!components)yield break;
            roots[0].transform.position=new Vector3(-7,0,4);roots[1].transform.position=new Vector3(7,0,4);
            var asset=Resources.Load<TextAsset>("AbilityAimProfiles");
            if(!asset){fail("Missing authored aiming profile inventory");yield break;}
            var profiles=JsonUtility.FromJson<Profiles>(asset.text).profiles;
            foreach(var p in profiles) for(int side=0;side<4;side++) {
                var target=roots[1-side%2];
                var fixture=new GameObject("AimFixture-"+p.resource+"-"+side);
                fixture.layer=side<2?roots[side].layer:LayerMask.NameToLayer(side==2?"PlayerAtk":"EnemyAtk");
                fixture.transform.position=new Vector3(0,0,-9);
                try {
                    if(p.type=="HomingAttack") {
                        var h=fixture.AddComponent<HomingAttack>();h.enabled=false;
                        h.delay=p.delay;h.horizontalSpeed=p.horizontalSpeed;h.verticalSpeed=p.verticalSpeed;
                        typeof(HomingAttack).GetMethod("Start",Private).Invoke(h,null);
                        if(h.target!=target){fail("Homing owner selection "+p.resource);yield break;}
                        // Deterministic Update oracle, using the actual frame delta and signed dot thresholds.
                        typeof(HomingAttack).GetMethod("Update",Private).Invoke(h,null);
                        if(Quaternion.Angle(fixture.transform.rotation,Quaternion.identity)>.001f){fail("Homing rotated before activation");yield break;}
                        h.CancelInvoke();h.StartHoming();
                        Quaternion expected=fixture.transform.rotation;
                        var delta=target.transform.position-fixture.transform.position;
                        for(int step=0;step<5;step++) {
                            float dot=Vector3.Dot(Vector3.Cross(expected*Vector3.forward,delta),expected*Vector3.up);
                            if(p.horizontalSpeed>0 && Mathf.Abs(dot)>.1f)expected*=Quaternion.Euler(0,Mathf.Sign(dot)*p.horizontalSpeed*Time.deltaTime,0);
                            dot=Vector3.Dot(Vector3.Cross(expected*Vector3.forward,delta),expected*Vector3.right);
                            if(p.verticalSpeed>0)expected*=Quaternion.Euler((dot<.1f?-1:dot>-.1f?1:0)*p.verticalSpeed*Time.deltaTime,0,0);
                            typeof(HomingAttack).GetMethod("Update",Private).Invoke(h,null);
                        }
                        float error=Quaternion.Angle(expected,fixture.transform.rotation);
                        observe("aim-homing-profile",p.resource+"#"+p.id+"; side="+side+"; degrees="+error+"; delay="+p.delay);
                        if(error>.1f){fail("Homing rotation contract "+p.resource);yield break;}
                    } else if(p.type=="LockToTarget") {
                        var l=fixture.AddComponent<LockToTarget>();l.enabled=false;l.delay=p.delay;
                        typeof(LockToTarget).GetMethod("Start",Private).Invoke(l,null);
                        l.CancelInvoke();typeof(LockToTarget).GetMethod("Lock",Private).Invoke(l,null);
                        var delta=target.transform.position-fixture.transform.position;delta.y=0;
                        float error=Vector3.Angle(fixture.transform.forward,delta);
                        var rotation=fixture.transform.rotation;
                        target.transform.position+=Vector3.right*2;
                        yield return new WaitForEndOfFrame();
                        bool oneShot=Quaternion.Angle(rotation,fixture.transform.rotation)<.001f;
                        target.transform.position-=Vector3.right*2;
                        observe("aim-lock-profile",p.resource+"#"+p.id+"; side="+side+"; degrees="+error+"; oneShot="+oneShot);
                        if(error>.1f||!oneShot){fail("Lock contract "+p.resource);yield break;}
                    } else {
                        var prefab=Resources.Load<GameObject>(p.resource);
                        var authored=prefab?prefab.GetComponentInChildren<MoveToTarget>(true):null;
                        if(!authored){fail("Missing curve source "+p.resource);yield break;}
                        var m=fixture.AddComponent<MoveToTarget>();m.enabled=false;
                        m.moveDuration=authored.moveDuration;m.xCurve=authored.xCurve;m.yCurve=authored.yCurve;m.zCurve=authored.zCurve;
                        m.XForce=authored.XForce;m.YForce=authored.YForce;m.ZForce=authored.ZForce;
                        Vector3 origin=fixture.transform.position;
                        typeof(MoveToTarget).GetField("startPosition",Private).SetValue(m,origin);
                        var iterator=(IEnumerator)typeof(MoveToTarget).GetMethod("MoveObjectToTarget",Private).Invoke(m,null);
                        float elapsed=0,maxError=0;
                        for(int step=0;step<8 && elapsed<m.moveDuration;step++) {
                            fixture.transform.rotation=Quaternion.Euler(0,side*90+step*11,0);
                            elapsed+=Time.deltaTime;float t=elapsed/m.moveDuration;
                            Vector3 expected=origin+fixture.transform.TransformDirection(new Vector3(m.xCurve.Evaluate(t)*m.XForce,m.yCurve.Evaluate(t)*m.YForce,m.zCurve.Evaluate(t)*m.ZForce));
                            if(!iterator.MoveNext()){fail("Curve stopped early "+p.resource);yield break;}
                            maxError=Mathf.Max(maxError,Vector3.Distance(expected,fixture.transform.position));
                        }
                        observe("aim-curve-profile",p.resource+"; side="+side+"; error="+maxError);
                        if(maxError>.0001f){fail("Curve frame semantics "+p.resource);yield break;}
                    }
                } finally {UnityEngine.Object.Destroy(fixture);}
            }
            for(int side=0;side<2;side++) {
                var fixture=new GameObject("AimSupplemental");fixture.layer=roots[side].layer;
                var template=new GameObject("AimSpawnTemplate");
                try {
                    fixture.transform.position=new Vector3(1,2,-5);
                    var leaf=fixture.AddComponent<RotateToOpponent>();leaf.enabled=false;
                    typeof(RotateToOpponent).GetMethod("Start",Private).Invoke(leaf,null);
                    float leafError=Vector3.Angle(fixture.transform.forward,roots[1-side].transform.position-fixture.transform.position);
                    if(leafError>.1f){fail("Leaf launch target selection");yield break;}
                    var config=fixture.AddComponent<ConfigAttackLayer>();
                    var creator=fixture.AddComponent<CreateObject>();creator.enabled=false;creator.obj=template;
                    creator.addPosition=new Vector3(2,3,4);
                    Vector3 expected=fixture.transform.position+fixture.transform.TransformDirection(creator.addPosition);
                    typeof(CreateObject).GetMethod("Create",Private).Invoke(creator,null);
                    var clone=GameObject.Find("AimSpawnTemplate(Clone)");
                    bool spawnOk=clone && clone.layer==fixture.layer && Vector3.Distance(clone.transform.position,expected)<.0001f;
                    if(clone)UnityEngine.Object.DestroyImmediate(clone);
                    config.playerWeak=true;
                    typeof(CreateObject).GetMethod("Create",Private).Invoke(creator,null);
                    bool weakSkipped=!GameObject.Find("AimSpawnTemplate(Clone)");
                    fixture.AddComponent<CreateObjAnim>().CreateObj(template);
                    clone=GameObject.Find("AimSpawnTemplate(Clone)");
                    bool animSpawnOk=clone && Vector3.Distance(clone.transform.position,fixture.transform.position)<.0001f && Quaternion.Angle(clone.transform.rotation,fixture.transform.rotation)<.1f;
                    if(clone)UnityEngine.Object.DestroyImmediate(clone);
                    var m=fixture.AddComponent<MoveRelativeToRotation>();m.enabled=false;
                    m.moveDuration=Time.deltaTime*4.5f;m.targetOffset=new Vector3(2,3,5);m.delay=.2f;
                    Vector3 origin=fixture.transform.position,final=origin+fixture.transform.TransformDirection(m.targetOffset);
                    typeof(MoveRelativeToRotation).GetField("startPosition",Private).SetValue(m,origin);
                    typeof(MoveRelativeToRotation).GetField("finalPosition",Private).SetValue(m,final);
                    var iterator=(IEnumerator)typeof(MoveRelativeToRotation).GetMethod("MoveToPosition",Private).Invoke(m,null);
                    bool waited=iterator.MoveNext() && iterator.Current is WaitForSeconds;
                    fixture.transform.rotation=Quaternion.identity;float elapsed=0,maxError=0;
                    while(iterator.MoveNext()) {
                        elapsed+=Time.deltaTime;
                        maxError=Mathf.Max(maxError,Vector3.Distance(fixture.transform.position,Vector3.Lerp(origin,final,elapsed/m.moveDuration)));
                    }
                    bool snapped=Vector3.Distance(fixture.transform.position,final)<.0001f;
                    observe("aim-supplemental","side="+side+"; leafDegrees="+leafError+"; spawn="+spawnOk+"; weakSkipped="+weakSkipped+"; animationSpawn="+animSpawnOk+"; curveWait="+waited+"; curveError="+maxError+"; finalSnap="+snapped);
                    if(!spawnOk||!weakSkipped||!animSpawnOk||!waited||maxError>.0001f||!snapped){fail("Supplemental aiming/motion/spawn contract");yield break;}
                    var follow=fixture.AddComponent<FollowOwner>();follow.enabled=false;
                    typeof(FollowOwner).GetMethod("Update",Private).Invoke(follow,null);
                    bool follows=Vector3.Distance(fixture.transform.position,roots[side].transform.position)<.0001f && Quaternion.Angle(fixture.transform.rotation,roots[side].transform.rotation)<.1f;
                    var rotate=fixture.AddComponent<Rotate>();rotate.enabled=false;rotate.speed=3;
                    Quaternion before=fixture.transform.rotation;
                    typeof(Rotate).GetMethod("FixedUpdate",Private).Invoke(rotate,null);
                    bool waits=Quaternion.Angle(before,fixture.transform.rotation)<.001f;
                    rotate.Init();typeof(Rotate).GetMethod("FixedUpdate",Private).Invoke(rotate,null);
                    bool perTick=Quaternion.Angle(before*Quaternion.Euler(0,3,0),fixture.transform.rotation)<.1f;
                    var turn=fixture.AddComponent<TurnAttack>();turn.enabled=false;turn.speed=2;turn.delay=.3f;turn.duration=2;turn.right=side==0;
                    typeof(TurnAttack).GetMethod("Start",Private).Invoke(turn,null);turn.CancelInvoke();
                    before=fixture.transform.rotation;turn.Init();typeof(TurnAttack).GetMethod("Update",Private).Invoke(turn,null);
                    bool turns=Quaternion.Angle(before*Quaternion.Euler(0,(side==0?1:-1)*100*Time.deltaTime,0),fixture.transform.rotation)<.1f;
                    before=fixture.transform.rotation;turn.finish();typeof(TurnAttack).GetMethod("Update",Private).Invoke(turn,null);
                    bool stops=Quaternion.Angle(before,fixture.transform.rotation)<.001f;
                    var drop=fixture.AddComponent<DropAtk>();drop.enabled=false;drop.speed=.8f;
                    config.playerWeak=false;origin=fixture.transform.position;
                    typeof(DropAtk).GetMethod("FixedUpdate",Private).Invoke(drop,null);
                    bool healthyStationary=Vector3.Distance(origin,fixture.transform.position)<.0001f;
                    config.playerWeak=true;typeof(DropAtk).GetMethod("FixedUpdate",Private).Invoke(drop,null);
                    bool weakDrops=Vector3.Distance(origin-fixture.transform.up*.2f,fixture.transform.position)<.0001f;
                    var distance=fixture.AddComponent<EnableByDistance>();distance.enabled=false;distance.objRef=template;distance.objToChange=template;distance.distance=20;
                    template.transform.position=Vector3.right*21;
                    typeof(EnableByDistance).GetMethod("Update",Private).Invoke(distance,null);bool outside=template.activeSelf;
                    template.transform.position=Vector3.right*20;
                    typeof(EnableByDistance).GetMethod("Update",Private).Invoke(distance,null);bool boundary=!template.activeSelf;
                    observe("aim-motion-contracts","side="+side+"; follow="+follows+"; rotationWait="+waits+"; perTick="+perTick+"; turn="+turns+"; stop="+stops+"; healthyStationary="+healthyStationary+"; weakDrops="+weakDrops+"; stageOutside="+outside+"; strictBoundary="+boundary);
                    if(!follows||!waits||!perTick||!turns||!stops||!healthyStationary||!weakDrops||!outside||!boundary){fail("Motion/owner/stage contract");yield break;}
                    var steer=fixture.AddComponent<Follow>();steer.enabled=false;steer.speed=.5f;
                    fixture.transform.position=new Vector3(0,0,-9);fixture.transform.rotation=Quaternion.identity;
                    typeof(Follow).GetMethod("Start",Private).Invoke(steer,null);
                    bool followTarget=steer.target==roots[1-side];
                    float signed=Vector3.Dot(Vector3.Cross(Vector3.forward,roots[1-side].transform.position-fixture.transform.position),Vector3.up);
                    before=Quaternion.Euler(0,Mathf.Sign(signed)*.5f*52.75f*Time.deltaTime,0);
                    typeof(Follow).GetMethod("Update",Private).Invoke(steer,null);
                    float followError=Quaternion.Angle(before,fixture.transform.rotation);
                    var forward=fixture.AddComponent<GoForward>();forward.enabled=false;forward.speed=.25f;
                    typeof(GoForward).GetMethod("Start",Private).Invoke(forward,null);
                    config.playerWeak=false;origin=fixture.transform.position;
                    typeof(GoForward).GetMethod("Update",Private).Invoke(forward,null);
                    float forwardError=Vector3.Distance(fixture.transform.position,origin+fixture.transform.forward*.25f*52.75f*Time.deltaTime);
                    observe("aim-follow-contracts","side="+side+"; target="+followTarget+"; steeringDegrees="+followError+"; forwardError="+forwardError);
                    if(!followTarget||followError>.1f||forwardError>.0001f){fail("Current Follow/GoForward contract");yield break;}
                    var startRotation=fixture.AddComponent<ChangeStartRotation>();startRotation.enabled=false;startRotation.addRotation=new Vector3(3,4,5);
                    before=fixture.transform.rotation;
                    typeof(ChangeStartRotation).GetMethod("Start",Private).Invoke(startRotation,null);
                    bool relativeRotation=Quaternion.Angle(before*Quaternion.Euler(startRotation.addRotation),fixture.transform.rotation)<.1f;
                    var angle=fixture.AddComponent<RotateByAngle>();angle.enabled=false;angle.duration=Time.deltaTime*4.5f;angle.delay=.2f;angle.targetRotation=new Vector3(15,20,0);
                    before=fixture.transform.localRotation;Quaternion end=before*Quaternion.Euler(angle.targetRotation);
                    iterator=(IEnumerator)typeof(RotateByAngle).GetMethod("RotateAfterDelay",Private).Invoke(angle,null);
                    bool angleWait=iterator.MoveNext() && iterator.Current is WaitForSeconds;
                    elapsed=0;float angleError=0;
                    while(iterator.MoveNext()) {
                        elapsed+=Time.deltaTime;
                        angleError=Mathf.Max(angleError,Quaternion.Angle(Quaternion.Slerp(before,end,Mathf.Clamp01(elapsed/angle.duration)),fixture.transform.localRotation));
                    }
                    bool angleSnap=Quaternion.Angle(fixture.transform.localRotation,end)<.1f;
                    var local=fixture.AddComponent<MoveObjectLocal2>();local.enabled=false;local.duration=Time.deltaTime*4.5f;local.AddVector=new Vector3(2,3,4);
                    origin=fixture.transform.localPosition;final=origin+local.AddVector;
                    typeof(MoveObjectLocal2).GetField("startPosition",Private).SetValue(local,origin);
                    typeof(MoveObjectLocal2).GetField("positionEnd",Private).SetValue(local,final);
                    iterator=(IEnumerator)typeof(MoveObjectLocal2).GetMethod("MoveOverTime",Private).Invoke(local,null);
                    elapsed=0;maxError=0;
                    while(iterator.MoveNext()) {
                        elapsed+=Time.deltaTime;
                        maxError=Mathf.Max(maxError,Vector3.Distance(fixture.transform.localPosition,Vector3.Lerp(origin,final,Mathf.Clamp01(elapsed/local.duration))));
                    }
                    bool localSnap=Vector3.Distance(fixture.transform.localPosition,final)<.0001f;
                    var weakDrop=fixture.AddComponent<DropWhenOwnerWeak>();weakDrop.enabled=false;weakDrop.speed=1;
                    follow.enabled=true;
                    var ownerAnimator=roots[side].GetComponent<Animator>();bool wasWeak=ownerAnimator.GetBool("Weak");
                    bool dropsCorrectly=false;
                    try {
                        ownerAnimator.SetBool("Weak",true);weakDrop.CheckOwner();origin=fixture.transform.position;
                        typeof(DropWhenOwnerWeak).GetMethod("Update",Private).Invoke(weakDrop,null);
                        dropsCorrectly=!follow.enabled && Vector3.Distance(fixture.transform.position,origin-fixture.transform.up*.35f)<.0001f;
                    } finally {ownerAnimator.SetBool("Weak",wasWeak);}
                    observe("aim-local-contracts","side="+side+"; relativeRotation="+relativeRotation+"; angleWait="+angleWait+"; angleError="+angleError+"; angleSnap="+angleSnap+"; localError="+maxError+"; localSnap="+localSnap+"; ownerWeakDrop="+dropsCorrectly);
                    if(!relativeRotation||!angleWait||angleError>.1f||!angleSnap||maxError>.0001f||!localSnap||!dropsCorrectly){fail("Relative rotation/local movement/weak owner contract");yield break;}
                } finally {UnityEngine.Object.Destroy(fixture);UnityEngine.Object.Destroy(template);}
            }
            var pingFixture=new GameObject("AimPingPongFixture");
            try {
                var ping=pingFixture.AddComponent<PingPongMove>();ping.start=new Vector3(0,0,-6);ping.finish=new Vector3(0,0,6);ping.duration=.12f;
                float high=-6,low=6;bool seenHigh=false,seenReturn=false;
                double until=Time.realtimeSinceStartupAsDouble+.5;
                while(Time.realtimeSinceStartupAsDouble<until) {
                    yield return null;
                    float z=pingFixture.transform.localPosition.z;high=Mathf.Max(high,z);
                    if(z>2)seenHigh=true;
                    if(seenHigh){low=Mathf.Min(low,z);if(z< -2)seenReturn=true;}
                }
                observe("aim-pingpong","forward="+seenHigh+"; return="+seenReturn+"; high="+high+"; low="+low);
                if(!seenHigh||!seenReturn){fail("Authored local ping-pong did not move both directions");yield break;}
            } finally {UnityEngine.Object.Destroy(pingFixture);}
        } finally {
            for(int side=0;side<2;side++)if(roots[side])roots[side].transform.position=original[side];
        }
    }
}
#endif
