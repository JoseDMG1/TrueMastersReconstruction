#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// The early Update supplies a controlled opponent path. LateUpdate samples the
// actual Follow/GoForward output after their ordinary Update methods have run.
[DefaultExecutionOrder(-31000)]
public sealed class OfflineMovingTargetProbe : MonoBehaviour
{
    [Serializable] public class FrameSample {
        public int frame;
        public float time, deltaTime, turnDot, speed, requestedTurn, rotationError, targetDrift, damage;
        public Vector3 commandedTarget, actualTarget, projectileBefore, projectileAfter, forwardBefore, forwardAfter;
        public bool correctTarget;
    }
    [Serializable] public class HitSample { public int frame; public float damage; }
    [Serializable] public class FlightReport {
        public string result, projectile, limitation;
        public Vector3 targetStart, targetEnd;
        public float initialDamage, finalDamage, maximumTargetDrift, maximumRotationError, targetTravel;
        public int turnFrames;
        public bool restored;
        public List<FrameSample> samples=new List<FrameSample>();
        public List<int> contacts=new List<int>();
        public List<HitSample> damageEvents=new List<HitSample>();
        public List<string> errors=new List<string>();
    }
    readonly FlightReport report=new FlightReport();
    readonly Dictionary<Behaviour,bool> movementOwners=new Dictionary<Behaviour,bool>();
    GameObject target;
    Follow follow;
    DamageOnCollision damage;
    Rigidbody body;
    Animator animator;
    Vector3 savedPosition, savedVelocity, savedAngularVelocity, commanded;
    bool savedKinematic, savedRootMotion, restored, bound, sampling;
    double began;
    Quaternion beforeRotation, expectedRotation;
    FrameSample pending;
    string output;

    public static OfflineMovingTargetProbe Prepare(GameController game,string output) {
        var fixture=new GameObject("MovingTargetFlightObserver").AddComponent<OfflineMovingTargetProbe>();
        fixture.output=output;
        fixture.target=game.OpponentCharacter.gameObject;
        fixture.damage=fixture.target.GetComponent<DamageOnCollision>();
        fixture.body=fixture.target.GetComponent<Rigidbody>();
        fixture.animator=fixture.target.GetComponent<Animator>();
        fixture.savedPosition=fixture.target.transform.position;
        fixture.savedRootMotion=fixture.animator.applyRootMotion;
        fixture.animator.applyRootMotion=false;
        foreach(var owner in fixture.target.GetComponents<Behaviour>()) {
            if(owner is AI || owner is AnimationMovement || owner is MobileInputs || owner is DesktopInputs || owner is jumpSimulator) {
                fixture.movementOwners.Add(owner,owner.enabled); owner.enabled=false;
            }
        }
        if(fixture.body) {
            fixture.savedKinematic=fixture.body.isKinematic;
            fixture.savedVelocity=fixture.body.linearVelocity;
            fixture.savedAngularVelocity=fixture.body.angularVelocity;
            if(!fixture.body.isKinematic) { fixture.body.linearVelocity=Vector3.zero; fixture.body.angularVelocity=Vector3.zero; }
            fixture.body.isKinematic=true;
        }
        fixture.damage.onDamageRecived.AddListener(fixture.RecordDamage);
        fixture.report.initialDamage=fixture.damage.Damage;
        fixture.report.limitation="Actual UGUI Sora UpAttack2 and authored projectile; CPU locomotion/root motion/rigidbody dynamics held by fixture. Opponent collider, damage dispatch and projectile movement/collision remain active. Controlled lateral path, not CPU AI or every fighter/height/edge acceptance.";
        return fixture;
    }

    public void PlaceTarget(Vector3 position) {
        commanded=position;
        if(body) body.position=position;
        target.transform.position=position;
        Physics.SyncTransforms();
    }

    public void Bind(GameObject projectile) {
        if(bound || projectile.name!="AirAtkStrong2(Clone)") return;
        follow=projectile.GetComponent<Follow>();
        if(!follow || !projectile.GetComponent<GoForward>()) { report.errors.Add("Natural projectile lacks Follow/GoForward"); return; }
        bound=true; began=Time.timeAsDouble;
        report.projectile=projectile.name;
        report.targetStart=target.transform.position;
        report.targetEnd=report.targetStart+Vector3.right*2f;
        projectile.AddComponent<OfflineMovingTargetContact>().Setup(this,target);
    }

    void Update() {
        if(restored || !bound || !target) return;
        // Smooth 2m lateral movement while the actual projectile is in flight.
        float elapsed=(float)(Time.timeAsDouble-began);
        PlaceTarget(Vector3.Lerp(report.targetStart,report.targetEnd,Mathf.Clamp01(elapsed/.3f)));
        report.targetTravel=Mathf.Max(report.targetTravel,Vector3.Distance(report.targetStart,target.transform.position));
        if(!follow || !follow.target || !follow.isActiveAndEnabled) return;
        beforeRotation=follow.transform.rotation;
        float dot=follow.AngleDir(follow.transform.forward,follow.target.transform.position-follow.transform.position,follow.transform.up);
        float turn=(dot>.1f?1:dot<-.1f?-1:0)*follow.speed*Time.deltaTime;
        expectedRotation=beforeRotation*Quaternion.Euler(0,turn,0);
        pending=new FrameSample {frame=Time.frameCount,time=elapsed,deltaTime=Time.deltaTime,turnDot=dot,speed=follow.speed,requestedTurn=turn,
            commandedTarget=commanded,projectileBefore=follow.transform.position,forwardBefore=follow.transform.forward,
            correctTarget=follow.target==target};
        sampling=true;
    }

    void LateUpdate() {
        if(!sampling) return;
        sampling=false;
        if(!follow || !target) return;
        pending.actualTarget=target.transform.position;
        pending.projectileAfter=follow.transform.position;
        pending.forwardAfter=follow.transform.forward;
        pending.rotationError=Quaternion.Angle(expectedRotation,follow.transform.rotation);
        pending.targetDrift=Vector3.Distance(commanded,target.transform.position);
        pending.damage=damage.Damage;
        report.maximumTargetDrift=Mathf.Max(report.maximumTargetDrift,pending.targetDrift);
        report.maximumRotationError=Mathf.Max(report.maximumRotationError,pending.rotationError);
        if(Mathf.Abs(pending.requestedTurn)>.01f) report.turnFrames++;
        report.samples.Add(pending);
    }

    void RecordDamage() { report.damageEvents.Add(new HitSample {frame=Time.frameCount,damage=damage.Damage}); }
    public void RecordContact() { report.contacts.Add(Time.frameCount); }

    public string Complete(Action<string,string> observe) {
        report.finalDamage=damage ? damage.Damage : report.initialDamage;
        if(!bound) report.errors.Add("Natural AirAtkStrong2 was not bound");
        if(report.samples.Count<5 || report.turnFrames<3) report.errors.Add("Insufficient live turning frames");
        if(report.samples.Any(s=>!s.correctTarget)) report.errors.Add("Follow selected a different target");
        if(report.maximumRotationError>.15f) report.errors.Add("Live steering differs from native signed-dot turn contract");
        if(report.maximumTargetDrift>.01f || report.targetTravel<1.9f) report.errors.Add("Controlled target path drifted or did not complete");
        bool attributed=report.damageEvents.Any(h=>h.damage>report.initialDamage && report.contacts.Any(c=>Math.Abs(c-h.frame)<=1));
        if(!attributed || report.finalDamage<=report.initialDamage) report.errors.Add("No projectile contact correlated with natural damage");
        Restore();
        report.restored=restored;
        report.result=report.errors.Count==0?"passed":"probe_failed";
        File.WriteAllText(output,JsonUtility.ToJson(report,true));
        observe("moving-target-flight",report.result+"; samples="+report.samples.Count+"; turnFrames="+report.turnFrames+"; targetTravel="+report.targetTravel+"; targetDrift="+report.maximumTargetDrift+"; rotationError="+report.maximumRotationError+"; contacts="+report.contacts.Count+"; damage="+report.initialDamage+"->"+report.finalDamage+"; restored="+report.restored+"; trace="+output);
        return report.errors.Count==0?null:string.Join("; ",report.errors);
    }

    void Restore() {
        if(restored) return;
        if(damage) damage.onDamageRecived.RemoveListener(RecordDamage);
        if(target) {
            if(body) { body.position=savedPosition; body.isKinematic=savedKinematic; if(!savedKinematic) {body.linearVelocity=savedVelocity;body.angularVelocity=savedAngularVelocity;} }
            target.transform.position=savedPosition;
        }
        if(animator) animator.applyRootMotion=savedRootMotion;
        foreach(var pair in movementOwners) if(pair.Key) pair.Key.enabled=pair.Value;
        Physics.SyncTransforms();
        restored=true;
    }
    void OnDestroy() { Restore(); }
}

public sealed class OfflineMovingTargetContact : MonoBehaviour
{
    OfflineMovingTargetProbe observer;
    GameObject target;
    public void Setup(OfflineMovingTargetProbe observer,GameObject target) {this.observer=observer;this.target=target;}
    void OnTriggerEnter(Collider other) {
        if(observer && other.GetComponentInParent<DamageOnCollision>()==target.GetComponent<DamageOnCollision>()) observer.RecordContact();
    }
}
#endif
