#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-31000)]
public sealed class OfflineFlightMatrixProbe : MonoBehaviour
{
    [Serializable] public class Profile { public string name,resource; public bool follow,dormantFollow,light,forward,edge,startFaster; public float followSpeed,startSpeed,startTime,delay,lightSpeed,forwardSpeed,rotationDelay; }
    [Serializable] public class Jump { public string fighter,state,name,resource; public float eventTime; }
    [Serializable] public class Inventory { public Profile[] profiles; public Jump[] jumps; }
    [Serializable] public class Case {
        public string profile,scenario; public int side,frames,turns,fixedMoves,weakFrames,contacts,launchResets,fixedHeadingFrames,targetFrames;
        public float rotationError,translationError,targetDrift,maximumRadius,ownerDamageBefore,ownerDamageAfter,horizontalTravel,targetDamageBefore,targetDamageAfter;
        public bool correctTarget=true,weakObserved,destroyed,restored,attributedHit,registeredAtHit,dormantFollowChecked;
        public Vector3 fixedExpected,fixedActual,fixedBeforeForward,fixedActualForward;
        public bool fixedExpectedWeak,fixedActualWeak;
        public List<int> contactFrames=new List<int>(),damageFrames=new List<int>();
        public List<string> errors=new List<string>();
    }
    [Serializable] public class Report {public string result,limitation; public List<Case> cases=new List<Case>();}
    public sealed class Hold : IDisposable {
        public readonly GameObject root;
        readonly Rigidbody body; readonly Animator anim;
        readonly Dictionary<Behaviour,bool> owners=new Dictionary<Behaviour,bool>();
        readonly bool kinematic,rootMotion; readonly Vector3 position,velocity,angular;
        public Hold(GameObject root) {
            this.root=root;position=root.transform.position;body=root.GetComponent<Rigidbody>();anim=root.GetComponent<Animator>();
            if(anim){rootMotion=anim.applyRootMotion;anim.applyRootMotion=false;}
            foreach(var b in root.GetComponents<Behaviour>()) if(b is AI||b is AnimationMovement||b is MobileInputs||b is DesktopInputs||b is jumpSimulator) {owners[b]=b.enabled;b.enabled=false;}
            if(body){kinematic=body.isKinematic;velocity=body.linearVelocity;angular=body.angularVelocity;if(!kinematic){body.linearVelocity=Vector3.zero;body.angularVelocity=Vector3.zero;}body.isKinematic=true;}
        }
        public void Place(Vector3 p){if(body)body.position=p;root.transform.position=p;}
        public void Dispose(){if(!root)return;Place(position);if(body){body.isKinematic=kinematic;if(!kinematic){body.linearVelocity=velocity;body.angularVelocity=angular;}}if(anim)anim.applyRootMotion=rootMotion;foreach(var p in owners)if(p.Key)p.Key.enabled=p.Value;Physics.SyncTransforms();}
    }
    Case current;
    Profile profile;
    GameObject projectile,owner,target;
    Follow follow;
    GoForward forward;
    MoveLight light;
    MoveWater water;
    MoveRock rock;
    MoveAttack moveAttack;
    ConfigAttackLayer config;
    Hold targetHold;
    Vector3 targetStart,commanded,beforePosition,beforeUp,beforeForward,expectedFixed,spawnPosition;
    Quaternion expectedRotation;
    Quaternion resetRotation,resetThenTurnRotation,straightHeading;
    bool pending,pendingFixed,lightPrimed,resetting,straightHeadingSet,beforeWeak,beforeForwardStopped,predictedWeak;
    Vector3 predictedForward;
    static readonly System.Reflection.FieldInfo waterReady=typeof(MoveWater).GetField("ready",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic);
    double began;
    DamageOnCollision targetDamage;

    public static Inventory Load(){return JsonUtility.FromJson<Inventory>(Resources.Load<TextAsset>("FlightMatrixProfiles").text);}
    public static IEnumerator Run(GameController game,string output,Action<string,string> observe,Action<string> fail) {
        var inventory=Load();var report=new Report{limitation="Authored prefab production fixtures across both fighter layers. Real component Start/Update/FixedUpdate and physics, controlled moving/elevated target, projectile stage edge, synthetic enemy attack overlap for interruption. Not natural UGUI routes, every clip/VFX, or original-runtime parity. Contacts are observations; hits are not required for every geometry."};
        var args=Environment.GetCommandLineArgs();int filter=Array.IndexOf(args,"-rea-flight-profile");
        if(filter>=0){var names=args[filter+1].Split('|');inventory.profiles=inventory.profiles.Where(p=>names.Contains(p.name)).ToArray();}
        var fixture=new GameObject("FlightMatrixObserver").AddComponent<OfflineFlightMatrixProbe>();
        var roots=new[]{game.MainCharacter.gameObject,game.OpponentCharacter.gameObject};
        var score=Score.score;int originalTime=score.time;score.time=3600;
        try {
            using(var main=new Hold(roots[0])) using(var opponent=new Hold(roots[1])) {
                var holds=new[]{main,opponent};
                foreach(var p in inventory.profiles) for(int side=0;side<2;side++) foreach(string scenario in new[]{"moving","height","edge","interrupt"}) {
                    var originalRoots=SceneManager.GetActiveScene().GetRootGameObjects().Select(o=>o.GetEntityId()).ToHashSet();
                    var c=new Case{profile=p.name,side=side,scenario=scenario}; report.cases.Add(c);
                    var own=roots[side];var enemy=roots[1-side];
                    foreach(var actor in roots) {actor.GetComponent<DamageOnCollision>().ResetDamage();actor.GetComponent<Animator>().Rebind();actor.GetComponent<ColliderController>().recoverCollider();}
                    holds[side].Place(scenario=="edge"?new Vector3(15,0,0):new Vector3(0,0,-8));
                    Vector3 destination=scenario=="edge"?new Vector3(18,0,3):new Vector3(0,scenario=="height"?7:0,17);
                    holds[1-side].Place(destination); Physics.SyncTransforms();
                    yield return null;
                    var template=Resources.Load<GameObject>(p.resource);
                    if(!template){c.errors.Add("Missing authored resource");continue;}
                    Vector3 spawn=scenario=="edge"?new Vector3(32,0,0):new Vector3(0,0,-2);
                    var attack=Instantiate(template,spawn,Quaternion.Euler(0,scenario=="edge"?90:p.follow?-15:0,0));attack.layer=own.layer;
                    GameEvents.i.AttackLayerSetUp(attack.GetEntityId());
                    fixture.Begin(p,c,attack,own,enemy,holds[1-side],destination);
                    bool interrupted=false;double until=Time.realtimeSinceStartupAsDouble+Math.Max(.7,Math.Max(p.delay,p.rotationDelay)+.6);
                    while(Time.realtimeSinceStartupAsDouble<until) {
                        if(scenario=="interrupt" && !interrupted && Time.timeAsDouble-fixture.began>.13) {
                            c.registeredAtHit=AttacksManager.i.GetOwnAttacks(own.layer).Contains(attack);
                            // Authored enemy AirAtk collider, explicitly overlapped with the
                            // owner. DamageOnCollision must dispatch naturally via physics.
                            var hit=Instantiate(Resources.Load<GameObject>("photonprefabs/AirAtk"));hit.layer=enemy.layer;
                            var go=hit.GetComponent<GoForward>();if(go)go.Stop();
                            var box=hit.GetComponent<BoxCollider>();
                            hit.transform.position=own.GetComponent<Collider>().bounds.center-hit.transform.TransformVector(box.center);
                            GameEvents.i.AttackLayerSetUp(hit.GetEntityId());Physics.SyncTransforms();interrupted=true;
                        }
                        yield return null;
                    }
                    fixture.End();
                    foreach(var obj in SceneManager.GetActiveScene().GetRootGameObjects()) if(!originalRoots.Contains(obj.GetEntityId()))Destroy(obj);
                    yield return null;
                    c.restored=true;
                    observe("flight-profile",p.name+"; side="+side+"; case="+scenario+"; frames="+c.frames+"; turns="+c.turns+"; fixed="+c.fixedMoves+"; rotationError="+c.rotationError+"; translationError="+c.translationError+"; weak="+c.weakObserved+"; ownerDamage="+c.ownerDamageAfter+"; destroyed="+c.destroyed+"; errors="+string.Join(",",c.errors));
                }
            }
        } finally {if(score)score.time=originalTime;Destroy(fixture.gameObject);}
        report.result=report.cases.Any(c=>c.errors.Count>0)?"probe_failed":"passed";
        File.WriteAllText(output,JsonUtility.ToJson(report,true));
        observe("flight-matrix",report.result+"; profiles="+inventory.profiles.Length+"; cases="+report.cases.Count+"; failed="+report.cases.Count(c=>c.errors.Count>0)+"; trace="+output);
        if(report.result!="passed")fail("Authored flight profile matrix has failed cases; inspect "+output);
    }
    public void Begin(Profile p,Case c,GameObject attack,GameObject own,GameObject enemy,Hold hold,Vector3 destination) {
        profile=p;current=c;projectile=attack;owner=own;target=enemy;targetHold=hold;targetStart=destination;commanded=destination;
        follow=attack.GetComponent<Follow>();forward=attack.GetComponent<GoForward>();light=attack.GetComponent<MoveLight>();config=attack.GetComponent<ConfigAttackLayer>();
        water=attack.GetComponent<MoveWater>();rock=attack.GetComponent<MoveRock>();moveAttack=attack.GetComponent<MoveAttack>();
        began=Time.timeAsDouble;pending=false;pendingFixed=false;lightPrimed=false;straightHeadingSet=false;spawnPosition=attack.transform.position;c.ownerDamageBefore=own.GetComponent<DamageOnCollision>().Damage;
        targetDamage=enemy.GetComponent<DamageOnCollision>();c.targetDamageBefore=targetDamage.Damage;targetDamage.onDamageRecived.AddListener(RecordDamage);
        attack.AddComponent<OfflineFlightMatrixContact>().Setup(this,enemy);
    }
    void CheckFixed(){
        if(!pendingFixed)return;if(!projectile||!projectile.activeInHierarchy){pendingFixed=false;return;}
        float error=Vector3.Distance(projectile.transform.position,expectedFixed);
        if(error>current.translationError){current.fixedExpected=expectedFixed;current.fixedActual=projectile.transform.position;current.fixedBeforeForward=predictedForward;current.fixedActualForward=projectile.transform.forward;current.fixedExpectedWeak=predictedWeak;current.fixedActualWeak=config&&config.playerWeak;current.translationError=error;}
        current.fixedMoves++;pendingFixed=false;
    }
    void FixedUpdate() {
        if(current==null||!projectile||!projectile.activeInHierarchy)return;
        CheckFixed();
        predictedForward=projectile.transform.forward;predictedWeak=config&&config.playerWeak;
        if(light&&light.isActiveAndEnabled) {
            if(!light.moving && !(config&&config.playerWeak))return;
            if(!lightPrimed){lightPrimed=true;return;}
            expectedFixed=projectile.transform.position+(config&&config.playerWeak?(-projectile.transform.up+projectile.transform.forward*.1f):projectile.transform.forward)*light.speed;
            pendingFixed=true;
        } else if(moveAttack&&moveAttack.isActiveAndEnabled && config&&config.playerWeak) {
            expectedFixed=projectile.transform.position+(-projectile.transform.up*.35f+projectile.transform.right*.1f)*moveAttack.speed;
            pendingFixed=true;current.weakFrames++;
        } else if(rock&&rock.isActiveAndEnabled && config&&config.playerWeak) {
            expectedFixed=projectile.transform.position+(-projectile.transform.up*.35f+projectile.transform.forward*.25f)*rock.speed;
            pendingFixed=true;
        }
    }
    void Update() {
        if(current==null)return;
        float elapsed=(float)(Time.timeAsDouble-began);
        commanded=targetStart+Vector3.right*2*Mathf.Clamp01((elapsed-profile.delay)/.3f);targetHold.Place(commanded);Physics.SyncTransforms();
        if(!projectile||!projectile.activeInHierarchy)return;
        CheckFixed();
        if(config&&config.playerWeak)current.weakObserved=true;
        beforePosition=projectile.transform.position;beforeUp=projectile.transform.up;beforeForward=projectile.transform.forward;
        beforeWeak=config&&config.playerWeak;beforeForwardStopped=forward&&forward.stop;
        expectedRotation=projectile.transform.rotation;
        resetting=water&&water.isActiveAndEnabled&&water.changeDirectionAtStart&&water.moving&&!(bool)waterReady.GetValue(water)&&!(config&&config.playerWeak);
        if(follow&&follow.isActiveAndEnabled&&follow.target) {
            current.targetFrames++;
            current.correctTarget&=follow.target==target;
            float dot=Vector3.Dot(Vector3.Cross(beforeForward,target.transform.position-beforePosition),beforeUp);
            float turn=(dot>.1f?1:dot<-.1f?-1:0)*follow.speed*Time.deltaTime;
            expectedRotation*=Quaternion.Euler(0,turn,0);if(Mathf.Abs(turn)>.01)current.turns++;
            if(resetting){
                // Native delayed MoveWater may copy owner yaw before or after Follow
                // in the same Update phase. Both original compositions are valid.
                resetRotation=owner.transform.rotation;
                var delta=target.transform.position-beforePosition;
                float resetDot=Vector3.Dot(Vector3.Cross(resetRotation*Vector3.forward,delta),resetRotation*Vector3.up);
                resetThenTurnRotation=resetRotation*Quaternion.Euler(0,(resetDot>.1f?1:resetDot<-.1f?-1:0)*follow.speed*Time.deltaTime,0);
                current.launchResets++;
            }
        }
        pending=true;
    }
    void LateUpdate() {
        if(current==null)return;
        current.targetDrift=Mathf.Max(current.targetDrift,Vector3.Distance(commanded,target.transform.position));
        if(!pending||!projectile||!projectile.activeInHierarchy){pending=false;return;}pending=false;
        // Ignore the initial launch frame, where other authored Start methods may set orientation.
        if(Time.timeAsDouble-began>.06 && follow&&follow.isActiveAndEnabled&&follow.target) {
            float error=Quaternion.Angle(expectedRotation,projectile.transform.rotation);
            if(resetting)error=Mathf.Min(error,Quaternion.Angle(resetRotation,projectile.transform.rotation),Quaternion.Angle(resetThenTurnRotation,projectile.transform.rotation));
            current.rotationError=Mathf.Max(current.rotationError,error);
        }
        if(light&&light.isActiveAndEnabled&&(!follow||!follow.isActiveAndEnabled)&&Time.timeAsDouble-began>Math.Max(profile.delay,profile.rotationDelay)+.12) {
            if(!straightHeadingSet){straightHeading=projectile.transform.rotation;straightHeadingSet=true;}
            current.rotationError=Mathf.Max(current.rotationError,Quaternion.Angle(straightHeading,projectile.transform.rotation));current.fixedHeadingFrames++;
        }
        if(beforeWeak) {
            float? displacement=null;
            if(forward&&forward.isActiveAndEnabled&&!beforeForwardStopped&&beforeForwardStopped==forward.stop)displacement=(-beforeUp.y*.35f+beforeForward.y*.25f)*forward.speed*Time.deltaTime;
            else if(water&&water.isActiveAndEnabled)displacement=(-beforeUp.y*.35f+beforeForward.y*.1f)*water.speed*Time.deltaTime;
            if(displacement.HasValue){current.translationError=Mathf.Max(current.translationError,Mathf.Abs(projectile.transform.position.y-beforePosition.y-displacement.Value));current.weakFrames++;}
        }
        current.frames++;current.maximumRadius=Mathf.Max(current.maximumRadius,projectile.transform.position.magnitude);
        var travel=projectile.transform.position-spawnPosition;travel.y=0;current.horizontalTravel=Mathf.Max(current.horizontalTravel,travel.magnitude);
    }
    public void End() {
        current.ownerDamageAfter=owner.GetComponent<DamageOnCollision>().Damage;
        current.targetDamageAfter=targetDamage.Damage;
        current.attributedHit=current.targetDamageAfter>current.targetDamageBefore&&current.damageFrames.Any(d=>current.contactFrames.Any(c=>Math.Abs(c-d)<=1));
        targetDamage.onDamageRecived.RemoveListener(RecordDamage);targetDamage=null;
        if(current.frames<4 && !(current.scenario=="edge"&&profile.edge&&current.destroyed))current.errors.Add("Insufficient live frames");
        if(!current.correctTarget)current.errors.Add("Incorrect Follow target");
        if(profile.dormantFollow){current.dormantFollowChecked=projectile&&projectile.GetComponentsInChildren<Follow>(true).All(f=>!f.isActiveAndEnabled);if(!current.dormantFollowChecked)current.errors.Add("Authored dormant Follow activated unexpectedly");}
        else if(profile.follow&&current.targetFrames<3&&!(current.scenario=="edge"&&profile.edge&&current.destroyed))current.errors.Add("Insufficient active Follow target samples");
        if(current.rotationError>.15f)current.errors.Add("Native Follow yaw mismatch");
        if(current.translationError>.002f)current.errors.Add("Native displacement mismatch");
        if(current.targetDrift>.01f)current.errors.Add("Target path drift");
        if(profile.light&&!profile.follow&&current.scenario!="edge"&&current.fixedMoves<3)current.errors.Add("Insufficient MoveLight fixed steps");
        if(current.scenario=="interrupt" && (current.ownerDamageAfter<=current.ownerDamageBefore||(current.registeredAtHit&&!current.weakObserved)))current.errors.Add("Physical owner interruption not observed");
        if(current.scenario=="edge"&&profile.edge&&!current.destroyed)current.errors.Add("Authored stage edge did not destroy projectile");
        current=null;projectile=null;pending=false;pendingFixed=false;
    }
    void RecordDamage(){if(current!=null&&targetDamage.Damage>current.targetDamageBefore)current.damageFrames.Add(Time.frameCount);}
    public void Contact(){if(current!=null){current.contacts++;current.contactFrames.Add(Time.frameCount);}}
    public void Expired(Vector3 position){if(current!=null){current.destroyed=true;current.maximumRadius=Mathf.Max(current.maximumRadius,position.magnitude);}}
    void OnDestroy(){if(targetDamage)targetDamage.onDamageRecived.RemoveListener(RecordDamage);}
}
public sealed class OfflineFlightMatrixContact : MonoBehaviour {
    OfflineFlightMatrixProbe observer;GameObject target;
    public void Setup(OfflineFlightMatrixProbe observer,GameObject target){this.observer=observer;this.target=target;}
    void OnTriggerEnter(Collider c){if(observer&&target&&c.GetComponentInParent<DamageOnCollision>()==target.GetComponent<DamageOnCollision>())observer.Contact();}
    void OnDestroy(){if(observer)observer.Expired(transform.position);}
}
#endif
