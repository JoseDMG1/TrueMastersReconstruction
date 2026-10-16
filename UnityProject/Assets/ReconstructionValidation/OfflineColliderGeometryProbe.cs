#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public static class OfflineColliderGeometryProbe
{
    public static IEnumerator Run(Action<string,string> observe,Action<string> fail)
    {
        observe("collider-geometry-scope","Authored prefab fields copied to isolated live components/boxes; actual Start, Invoke, Update, delayed component destruction. Not natural attack overlap or ordering of concurrent writers.");
        foreach(string name in new[]{"WaterWaveAtk","WaterAtkStrong","ShadowSnakeAtk"})
        {
            var prefab=Resources.Load<GameObject>("photonprefabs/"+name);
            foreach(var source in prefab.GetComponentsInChildren<ChangeColliderCenter>(true))
                yield return Case(name,source,null,observe,fail);
            foreach(var source in prefab.GetComponentsInChildren<ChangeColliderScale>(true))
                yield return Case(name,null,source,observe,fail);
        }
    }
    static IEnumerator Case(string name,ChangeColliderCenter center,ChangeColliderScale scale,Action<string,string> observe,Action<string> fail)
    {
        var fixture=new GameObject("Collider geometry fixture"); fixture.transform.position=new Vector3(600,60,600);
        try
        {
            var box=fixture.AddComponent<BoxCollider>();
            var authoredBox=center ? center.col : scale.col;
            if(!authoredBox) { fail("Authored geometry component has no box: "+name); yield break; }
            box.center=authoredBox.center; box.size=authoredBox.size;
            var witness=fixture.AddComponent<OfflineColliderGeometryWitness>(); witness.box=box; witness.centerMode=center;
            float delay,duration; Vector3 target; MonoBehaviour owner;
            if(center)
            {
                var live=fixture.AddComponent<ChangeColliderCenter>(); live.col=box; live.startCenter=center.startCenter; live.targetCenter=center.targetCenter; live.duration=center.duration; live.delay=center.delay;
                owner=live; delay=live.delay; duration=live.duration; target=live.targetCenter; witness.start=live.startCenter;
            }
            else
            {
                var live=fixture.AddComponent<ChangeColliderScale>(); live.col=box; live.startScale=scale.startScale; live.targetScale=scale.targetScale; live.duration=scale.duration; live.delay=scale.delay;
                owner=live; delay=live.delay; duration=live.duration; target=live.targetScale; witness.start=live.startScale;
            }
            witness.owner=owner; witness.target=target; witness.duration=duration;
            witness.initial=center ? box.center : box.size;
            float began=Time.time;
            double deadline=Time.realtimeSinceStartupAsDouble+delay+duration+2;
            while(owner && Time.realtimeSinceStartupAsDouble<deadline) yield return null;
            float finalError=Vector3.Distance(center ? box.center : box.size,target);
            observe("collider-geometry-case",name+"/"+(center ? "center" : "size")+"; delay="+delay+"; duration="+duration+"; samples="+witness.samples+"; disabledSamples="+witness.disabledSamples+"; maxError="+witness.maxError+"; finalError="+finalError+"; componentDestroyed="+(!owner)+"; boxRetained="+(bool)box+"; elapsed="+(Time.time-began));
            if(owner||!box||witness.samples<2||witness.maxError>.0001f||finalError>.0001f||(delay>.1f&&witness.disabledSamples<1)) fail("Collider geometry mismatch: "+name);
        }
        finally { if(fixture) UnityEngine.Object.Destroy(fixture); }
        yield return null;
    }
}
[DefaultExecutionOrder(-30000)]
public sealed class OfflineColliderGeometryWitness : MonoBehaviour
{
    public MonoBehaviour owner;
    public BoxCollider box;
    public bool centerMode;
    public Vector3 start,target,initial;
    public float duration,maxError;
    public int samples,disabledSamples;
    Vector3 expected;
    bool sampled;
    FieldInfo elapsedField;
    void Update()
    {
        sampled=false;
        if(!owner||!box) return;
        if(!owner.enabled)
        {
            disabledSamples++; maxError=Mathf.Max(maxError,Vector3.Distance(centerMode?box.center:box.size,initial)); return;
        }
        if(elapsedField==null) elapsedField=owner.GetType().GetField("timeElapsed",BindingFlags.NonPublic|BindingFlags.Instance);
        float elapsed=(float)elapsedField.GetValue(owner);
        expected=Vector3.Lerp(start,target,elapsed/duration); sampled=true;
    }
    void LateUpdate()
    {
        if(!sampled||!box) return;
        samples++; maxError=Mathf.Max(maxError,Vector3.Distance(centerMode?box.center:box.size,expected));
    }
}
#endif
