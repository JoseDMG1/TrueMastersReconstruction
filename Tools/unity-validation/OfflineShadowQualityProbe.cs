#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using DynamicShadowProjector;

public static class OfflineShadowQualityProbe
{
    public static IEnumerator Run(string fighter,Action<string,string> observe,Action<string> fail,bool listenerFixture)
    {
        var manager=GraphicsManager.i;
        var allControllers=UnityEngine.Object.FindObjectsByType<ShadowController>(FindObjectsInactive.Include);
        var controllers=(manager?.graphicsChange?.GetInvocationList()??Array.Empty<Delegate>()).Select(d=>d.Target).OfType<ShadowController>().Where(c=>c).Distinct().ToArray();
        var dormant=allControllers.Where(c=>!controllers.Contains(c)).ToArray();
        observe("shadow-quality-scope",fighter+"; subscribedControllers="+controllers.Length+"; dormantNotStarted="+dormant.Length);
        if(!manager||controllers.Length<2) {fail("Expected graphics publisher and both shadow controllers");yield break;}
        int previous=PlayerPrefs.GetInt("userQualityGraphics",0);
        bool hadPreference=PlayerPrefs.HasKey("userQualityGraphics");
        try
        {
            foreach(int quality in new[]{0,1,2,3,1})
            {
                PlayerPrefs.SetInt("userQualityGraphics",quality);manager.graphicsChange?.Invoke();
                yield return null;yield return new WaitForEndOfFrame();
                bool active=controllers.All(c=>c.gameObject.activeSelf==(quality==1));
                bool direction=controllers.All(c=>c.GetComponent<DrawTargetObject>().targetDirection && c.GetComponent<DrawTargetObject>().targetDirection.CompareTag("MainLight"));
                bool orientation=quality!=1 || controllers.All(c=>Vector3.Angle(c.transform.forward,c.GetComponent<DrawTargetObject>().targetDirection.forward)<.01f);
                active &= dormant.All(c=>!c.gameObject.activeSelf);
                bool resources=controllers.All(c=>quality==1
                    ? c.GetComponent<ShadowTextureRenderer>().shadowTexture && c.GetComponent<ShadowTextureRenderer>().shadowTexture.IsCreated()
                    : !c.GetComponent<ShadowTextureRenderer>().shadowTexture);
                observe("shadow-quality-case",fighter+"; preference="+quality+"; controllers="+controllers.Length+"; activeMatches="+active+"; MainLight="+direction+"; forwardMatchesLight="+orientation+"; resourceMatches="+resources);
                if(!active||!direction||!orientation||!resources) {fail("Native shadow quality matrix/orientation mismatch for "+fighter);yield break;}
            }
            var own=controllers.First(c=>c.transform.IsChildOf(GameController.i.MainCharacter.transform)).GetComponent<DrawTargetObject>();
            var target=own.target;Vector3 position=target.position,relative=own.transform.position-target.position;
            try
            {
                PlayerPrefs.SetInt("userQualityGraphics",3);manager.graphicsChange?.Invoke();
                target.position+=Vector3.right*.5f;
                PlayerPrefs.SetInt("userQualityGraphics",1);manager.graphicsChange?.Invoke();
                yield return null;yield return new WaitForEndOfFrame();
                float error=Vector3.Distance(own.transform.position-target.position,relative);
                observe("shadow-quality-follow",fighter+"; controlled target displacement while disabled; relativeOffsetError="+error);
                if(error>.001f) {fail("Shadow lost target offset after quality reactivation");yield break;}
            }
            finally {if(target)target.position=position;}
            yield return null;
            if(listenerFixture)
            {
                int before=manager.graphicsChange?.GetInvocationList().Length??0;
                GameObject fixture=null;
                try
                {
                    fixture=new GameObject("Shadow listener fixture");
                    fixture.AddComponent<DrawTargetObject>();fixture.AddComponent<ShadowController>();
                    yield return null;
                    int live=manager.graphicsChange?.GetInvocationList().Length??0;
                    UnityEngine.Object.Destroy(fixture);yield return null;
                    int after=manager.graphicsChange?.GetInvocationList().Length??0;
                    observe("shadow-quality-listener","before="+before+"; live="+live+"; afterDestroy="+after);
                    if(live!=before+1||after!=before)fail("Shadow listener lifecycle mismatch");
                }
                finally {if(fixture)UnityEngine.Object.Destroy(fixture);}
            }
        }
        finally
        {
            if(hadPreference)PlayerPrefs.SetInt("userQualityGraphics",previous);else PlayerPrefs.DeleteKey("userQualityGraphics");
            if(manager)manager.graphicsChange?.Invoke();
        }
        yield return null;
    }
}
#endif
