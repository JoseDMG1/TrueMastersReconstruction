#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

// Controlled physics fixture using recovered prefabs; no direct trigger invocation.
public static class OfflineMayaProductionProbe
{
    public static IEnumerator Run(GameController game, Action<string,string> observe, Action<string> fail)
    {
        yield return OfflineAnimationColliderProbe.Run(game,observe,fail);
        yield return OfflineColliderGeometryProbe.Run(observe,fail);
        yield return OfflineColliderDisableProbe.Run(observe,fail);
        observe("maya-production-scope", "Recovered prefab instances, synthetic remote collider targets, real physics. Not natural aiming or attack input coverage.");
        foreach (string name in new[] {"LeafFrontHit", "LeafAtkL", "LeafAtkR", "FlowerBeam"})
        {
            var prefab = Resources.Load<GameObject>("photonprefabs/" + name);
            if (!prefab || !prefab.GetComponent<sumonOnCollider>()) { fail("Missing recovered producer " + name); yield break; }
            GameObject source = null, target = null;
            try
            {
                var origin = new Vector3(300, 60, 300);
                source = UnityEngine.Object.Instantiate(prefab, origin, Quaternion.Euler(0, 37, 0));
                var producer = source.GetComponent<sumonOnCollider>();
                string childName = producer.sumon.name + "(Clone)";
                var existing = UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsInactive.Include)
                    .Where(t => t.name == childName).Select(t => t.GetEntityId()).ToHashSet();
                // Give the prefab its normal Start before arranging one controlled overlap.
                yield return null;
                var collider = source.GetComponent<Collider>();
                if (name == "FlowerBeam" && collider && !collider.enabled)
                {
                    observe("maya-flower-fixture", "Isolated FlowerBeam prefab authors a disabled root trigger and has no local enable owner. Synthetic trigger enable tests producer only; natural activation remains unaccepted.");
                    collider.enabled = true;
                }
                double activationDeadline = Time.realtimeSinceStartupAsDouble + 3;
                while (source && collider && !collider.enabled && Time.realtimeSinceStartupAsDouble < activationDeadline) yield return null;
                if (!source || !collider || !collider.enabled || !collider.isTrigger)
                { fail(name + " root trigger did not activate naturally"); yield break; }
                target = new GameObject("Maya production physics target");
                source.layer = 0; // Default-layer self collisions are enabled in the recovered physics matrix.
                target.layer = source.layer; // Native producer deliberately has no same-layer filter.
                target.transform.SetPositionAndRotation(collider.bounds.center, Quaternion.Euler(0, 113, 0));
                var sphere = target.AddComponent<SphereCollider>(); sphere.radius = .25f;
                var body = target.AddComponent<Rigidbody>(); body.isKinematic = true; body.useGravity = false;
                Physics.SyncTransforms();
                yield return new WaitForFixedUpdate(); yield return new WaitForFixedUpdate(); yield return null;
                Func<Transform[]> children = () => UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsInactive.Include)
                    .Where(t => t.name == childName && !existing.Contains(t.GetEntityId())).ToArray();
                int rejected = children().Length;
                if (rejected != 0) { fail(name + " created child for target without Description"); yield break; }
                // Exit/re-enter after adding Description; do not call OnTriggerEnter directly.
                sphere.enabled = false;
                yield return new WaitForFixedUpdate(); yield return new WaitForFixedUpdate();
                target.AddComponent<Description>();
                if (!source) { fail(name + " expired before positive collision fixture"); yield break; }
                target.transform.position = collider.bounds.center;
                Vector3 expectedPosition = target.transform.position;
                Quaternion expectedRotation = target.transform.rotation;
                sphere.enabled = true; Physics.SyncTransforms();
                Transform child = null;
                double deadline = Time.realtimeSinceStartupAsDouble + .3;
                while (Time.realtimeSinceStartupAsDouble < deadline)
                {
                    child = children().FirstOrDefault();
                    if (child) break;
                    yield return null;
                }
                if (!child) { fail(name + " did not create " + childName + " from real collision"); yield break; }
                float positionError = Vector3.Distance(child.position, expectedPosition);
                float rotationError = Quaternion.Angle(child.rotation, expectedRotation);
                observe("maya-collision-child", name + " -> " + childName + "; missingDescriptionRejected=" + (rejected == 0)
                    + "; sameLayer=" + (target.layer == source.layer) + "; positionError=" + positionError + "; rotationError=" + rotationError + "; parent=" + (child.parent ? child.parent.name : "none"));
                if (positionError > .05f || rotationError > .05f || child.parent) { fail(name + " child pose/parent differs from native target pose"); yield break; }
                UnityEngine.Object.Destroy(child.gameObject);
            }
            finally { if (source) UnityEngine.Object.Destroy(source); if (target) UnityEngine.Object.Destroy(target); }
            yield return null;
        }
        foreach (string name in new[] {"FloorBeam", "FloorBeam 1"})
        {
            var prefab = Resources.Load<GameObject>("photonprefabs/" + name);
            var beam = UnityEngine.Object.Instantiate(prefab, new Vector3(300, 60, 300), Quaternion.Euler(0, 37, 0));
            try
            {
                var zigzag = beam.GetComponentInChildren<ZigzagController>();
                if (!zigzag) { fail(name + " has no recovered ZigzagController"); yield break; }
                var witness = zigzag.gameObject.AddComponent<OfflineMayaZigzagWitness>(); witness.owner = zigzag;
                double deadline = Time.realtimeSinceStartupAsDouble + .8;
                while (beam && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
                observe("maya-zigzag", name + "; frames=" + witness.samples + "; reversals=" + witness.reversals + "; maxLateralError=" + witness.maxError);
                if (witness.samples < 12 || witness.reversals < 2 || witness.maxError > .002f)
                { fail(name + " did not match native per-frame lateral motion/reversals"); yield break; }
            }
            finally { if (beam) UnityEngine.Object.Destroy(beam); }
        }
    }
}

[DefaultExecutionOrder(-30000)]
public sealed class OfflineMayaZigzagWitness : MonoBehaviour
{
    public ZigzagController owner;
    public int samples, reversals;
    public float maxError;
    Vector3 before, right;
    float timer;
    int direction;
    bool sampled;
    static readonly System.Reflection.FieldInfo directionField = typeof(ZigzagController).GetField("direction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    static readonly System.Reflection.FieldInfo timerField = typeof(ZigzagController).GetField("timer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    void Update()
    {
        before = transform.position; right = transform.right;
        direction = (int)directionField.GetValue(owner);
        timer = (float)timerField.GetValue(owner);
        sampled = true;
    }
    void LateUpdate()
    {
        if (!sampled) return;
        float actual = Vector3.Dot(transform.position - before, right);
        maxError = Mathf.Max(maxError, Mathf.Abs(actual - owner.moveDistance * direction));
        samples++;
        timer -= Time.deltaTime;
        if (timer <= 0) { direction = -direction; timer = owner.switchTime; reversals++; }
        if ((int)directionField.GetValue(owner) != direction || Mathf.Abs((float)timerField.GetValue(owner) - timer) > .0001f)
            maxError = Mathf.Max(maxError, 100f);
        sampled = false;
    }
}
public static class OfflineColliderDisableProbe
{
    public static IEnumerator Run(Action<string,string> observe, Action<string> fail)
    {
        observe("collider-disable-scope","Synthetic sources/targets with actual trigger delivery; layer pair enabled temporarily and restored; no direct OnTriggerEnter calls.");
        // Layer gates, own Description filtering and a deliberately separate referenced box.
        var cases = new[] {
            ("non-player-layer",8,0,false,(string)null,"Char",false),
            ("same-layer",8,8,false,(string)null,"Char",false),
            ("opposite-layer-9",8,9,false,(string)null,"Char",true),
            ("opposite-layer-8",9,8,false,(string)null,"Char",true),
            ("unrestricted-owner-layer",0,8,false,(string)null,"Char",true),
            ("layer-10-rejected",8,10,false,(string)null,"Char",false),
            ("flag-no-own-description",8,9,true,(string)null,"Char",false),
            ("flag-own-char",8,9,true,"Char","Attack",false),
            ("flag-own-attack",8,9,true,"Attack","Char",true),
            ("flag-own-attack-no-incoming-description",8,9,true,"Attack",(string)null,true)
        };
        foreach(var item in cases)
            yield return Contact(item.Item1,item.Item2,item.Item3,item.Item4,item.Item5,item.Item6,item.Item7,false,"Untagged","Untagged",observe,fail);
        yield return Contact("null-prefab-reference",8,9,false,null,"Char",false,false,"Untagged","Untagged",observe,fail,true);
        yield return Contact("lm-own-attack",0,0,false,null,null,true,true,"Attack","Untagged",observe,fail);
        yield return Contact("lm-own-player",0,0,false,null,null,true,true,"Player","Untagged",observe,fail);
        yield return Contact("lm-incoming-tag-ignored",0,0,false,null,null,false,true,"Untagged","Attack",observe,fail);
    }
    static IEnumerator Contact(string label,int ownLayer,int incomingLayer,bool flag,string ownType,string incomingType,bool disables,bool lm,string ownTag,string incomingTag,Action<string,string> observe,Action<string> fail,bool missingReference=false)
    {
        GameObject source=null,target=null;
        bool ignored=Physics.GetIgnoreLayerCollision(ownLayer,incomingLayer);
        try
        {
            Physics.IgnoreLayerCollision(ownLayer,incomingLayer,false);
            source=new GameObject("Collider-disable fixture "+label); source.layer=ownLayer; source.tag=ownTag;
            source.transform.position=new Vector3(400,60,400);
            var trigger=source.AddComponent<BoxCollider>(); trigger.isTrigger=true; trigger.size=Vector3.one*2;
            var witness=source.AddComponent<OfflineColliderDisableWitness>();
            if(ownType!=null) source.AddComponent<Description>().Type=ownType;
            BoxCollider controlled=trigger;
            if(lm) source.AddComponent<DisableColliderOnCollition_LM>();
            else
            {
                var child=new GameObject("Referenced box"); child.transform.SetParent(source.transform,false); child.transform.localPosition=Vector3.right*10;
                controlled=child.AddComponent<BoxCollider>();
                var owner=source.AddComponent<DisableColliderOnCollition>(); owner.col=missingReference ? null : controlled; owner.onlyByPlayers=flag;
            }
            target=new GameObject("Incoming fixture"); target.SetActive(false); target.layer=incomingLayer; target.tag=incomingTag;
            target.transform.position=source.transform.position;
            if(incomingType!=null) target.AddComponent<Description>().Type=incomingType;
            target.AddComponent<SphereCollider>().radius=.25f;
            var body=target.AddComponent<Rigidbody>(); body.isKinematic=true; body.useGravity=false;
            yield return null;
            target.SetActive(true); Physics.SyncTransforms();
            double deadline=Time.realtimeSinceStartupAsDouble+.5;
            while(witness.contacts==0 && Time.realtimeSinceStartupAsDouble<deadline) yield return null;
            bool matches=controlled.enabled==!disables && (lm || trigger.enabled);
            observe("collider-disable-case",label+"; contacts="+witness.contacts+"; expectedDisabled="+disables+"; enabled="+controlled.enabled+"; sourceTriggerEnabled="+trigger.enabled+"; matches="+matches);
            if(witness.contacts==0 || !matches) fail("Collider-disable native branch mismatch: "+label);
        }
        finally
        {
            if(target) { target.SetActive(false); UnityEngine.Object.Destroy(target); }
            if(source) { source.SetActive(false); UnityEngine.Object.Destroy(source); }
            Physics.IgnoreLayerCollision(ownLayer,incomingLayer,ignored);
        }
        yield return null;
    }
}
public sealed class OfflineColliderDisableWitness : MonoBehaviour
{
    public int contacts;
    void OnTriggerEnter(Collider other) { contacts++; }
}
#endif
