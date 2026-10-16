#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using DynamicShadowProjector;

public static class OfflineDynamicShadowProbe
{
    static Color32[] Read(RenderTexture source,string png=null)
    {
        var previous=RenderTexture.active;
        var resolved=RenderTexture.GetTemporary(source.width,source.height,0,RenderTextureFormat.ARGB32);
        Texture2D pixels=null;
        try
        {
            Graphics.Blit(source,resolved);RenderTexture.active=resolved;
            pixels=new Texture2D(source.width,source.height,TextureFormat.RGBA32,false,true);
            pixels.ReadPixels(new Rect(0,0,source.width,source.height),0,0);pixels.Apply();
            if(png!=null)File.WriteAllBytes(png,pixels.EncodeToPNG());
            return pixels.GetPixels32();
        }
        finally {RenderTexture.active=previous;RenderTexture.ReleaseTemporary(resolved);if(pixels)UnityEngine.Object.Destroy(pixels);}
    }
    public static IEnumerator Run(string output,Action<string,string> observe,Action<string> fail,Animator preferredAnimator=null)
    {
        var owners=UnityEngine.Object.FindObjectsByType<ShadowTextureRenderer>(FindObjectsInactive.Exclude).Where(o=>o.enabled&&o.GetComponent<Projector>().enabled).ToArray();
        observe("dynamic-shadow-scope","Live authored Built-In profiles, GPU cookie readback and controlled animation poses; approximate mask/projector shaders, not original visual parity. Owners="+owners.Length);
        yield return null;yield return new WaitForEndOfFrame();
        if(owners.Length<2) {fail("Expected shadows for both fighters");yield break;}
        var textures=owners.Select(o=>o.shadowTexture).ToArray();
        if(textures.Any(t=>!t)||textures.Distinct().Count()!=owners.Length) {fail("Shadow textures missing or shared across projectors");yield break;}
        for(int i=0;i<owners.Length;i++)
        {
            var owner=owners[i];var projector=owner.GetComponent<Projector>();var texture=owner.shadowTexture;
            if(!owner.supportsAuthoredProfile||!texture.IsCreated()||projector.material.GetTexture("_ShadowTex")!=texture) {fail("Shadow texture/profile/binding mismatch");yield break;}
            var pixels=Read(texture,Path.ChangeExtension(output,".dynamic-shadow-"+(i+1)+".png"));
            int dark=pixels.Count(p=>p.r<128&&p.g<128&&p.b<128),white=pixels.Count(p=>p.r>240&&p.g>240&&p.b>240);
            observe("dynamic-shadow-cookie","owner="+owner.name+"; size="+texture.width+"x"+texture.height+"; dark="+dark+"; white="+white+"; bound=True; unique=True");
            if(dark<10||white<10) {fail("GPU shadow cookie lacks silhouette or neutral background");yield break;}
        }
        var selected=owners.FirstOrDefault(o=>o.GetComponent<DrawTargetObject>()?.target && o.GetComponent<DrawTargetObject>().target.GetComponentsInParent<Animator>().Any(a=>a.runtimeAnimatorController && (!preferredAnimator || a==preferredAnimator)));
        if(!selected) {fail("No shadow target animator available");yield break;}
        var animator=preferredAnimator ? preferredAnimator : selected.GetComponent<DrawTargetObject>().target.GetComponentsInParent<Animator>().First(a=>a.runtimeAnimatorController);
        var state=animator.GetCurrentAnimatorStateInfo(0);bool events=animator.fireEvents;float speed=animator.speed,scale=Time.timeScale;var culling=animator.cullingMode;
        try
        {
            Time.timeScale=0;animator.fireEvents=false;animator.speed=1;animator.cullingMode=AnimatorCullingMode.AlwaysAnimate;
            animator.Play("BasicAttack",0,.1f);animator.Update(.001f);yield return null;yield return new WaitForEndOfFrame();
            var bones=animator.GetComponentsInChildren<SkinnedMeshRenderer>().SelectMany(r=>r.bones).Where(b=>b).Distinct().ToArray();
            var rotations=bones.Select(b=>b.localRotation).ToArray();
            var firstState=animator.GetCurrentAnimatorStateInfo(0);
            var first=Read(selected.shadowTexture,Path.ChangeExtension(output,".dynamic-pose-1.png"));
            animator.Play("BasicAttack",0,.6f);animator.Update(.001f);yield return null;yield return new WaitForEndOfFrame();
            var secondState=animator.GetCurrentAnimatorStateInfo(0);
            int changedBones=bones.Where((b,i)=>Quaternion.Angle(rotations[i],b.localRotation)>.1f).Count();
            var second=Read(selected.shadowTexture,Path.ChangeExtension(output,".dynamic-pose-2.png"));
            int changed=first.Zip(second,(a,b)=>Mathf.Abs(a.r-b.r)>32).Count(v=>v);
            observe("dynamic-shadow-animation","actor="+animator.name+"; controlled BasicAttack .1/.6; automatic camera rendering; states="+firstState.shortNameHash+","+secondState.shortNameHash+"; times="+firstState.normalizedTime+","+secondState.normalizedTime+"; changedBones="+changedBones+"; changedPixels="+changed+"; cameraEnabled="+selected.projectorCamera.enabled);
            if(!firstState.IsName("BasicAttack")||!secondState.IsName("BasicAttack")||changedBones==0) {fail("Controlled shadow animation fixture did not establish two distinct combat poses");yield break;}
            if(changed<10) {fail("Shadow cookie did not follow animated geometry");yield break;}
            var old=selected.shadowTexture; selected.enabled=false;
            bool released=!old||!old.IsCreated();bool cameraStopped=!selected.projectorCamera.enabled;
            selected.enabled=true;yield return null;yield return new WaitForEndOfFrame();
            bool recreated=selected.shadowTexture&&selected.shadowTexture.IsCreated()&&selected.shadowTexture!=old;
            observe("dynamic-shadow-lifecycle","released="+released+"; cameraStopped="+cameraStopped+"; recreated="+recreated);
            if(!released||!cameraStopped||!recreated)fail("Shadow disable/enable lifecycle failed");
        }
        finally
        {
            animator.Play(state.fullPathHash,0,state.normalizedTime);animator.Update(0);animator.speed=speed;animator.fireEvents=events;animator.cullingMode=culling;Time.timeScale=scale;
            if(selected&&!selected.enabled)selected.enabled=true;
        }
    }
}
#endif
