#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Opt-in checks for the user's 2026-10-18 correction list, in the real battle scene.
public static class OfflineOctoberCorrectionsProbe
{
    public static IEnumerator Run(GameController game, int index, string output, Action<string,string> observe, Action<string> fail)
    {
        string prefix = System.IO.Path.ChangeExtension(output, null) + ".case-" + index;
        var actor = game.MainCharacter;
        var enemy = game.OpponentCharacter;
        var reader = BattleSceneSingleton.i.inputReader;
        foreach (var label in UnityEngine.Object.FindObjectsByType<Text>(FindObjectsInactive.Include))
            if (label.text.Trim().Equals("play again", StringComparison.OrdinalIgnoreCase)) fail("Stale result/pause label: " + label.name);
        foreach (var label in UnityEngine.Object.FindObjectsByType<TMP_Text>(FindObjectsInactive.Include))
            if (label.text.Trim().Equals("play again", StringComparison.OrdinalIgnoreCase)) fail("Stale TMP result/pause label: " + label.name);
        var cameraManager = CameraManager.cameraM;
        bool local = SystemManager.i.mode == GameMode.LocalOffline;
        if (local)
        {
            yield return null;
            var cameras = UnityEngine.Object.FindObjectsByType<Camera>(FindObjectsInactive.Exclude)
                .Where(c => c.enabled && Mathf.Abs(c.rect.width - .5f) < .001f).ToArray();
            if (cameras.Length != 2) fail("Expected two battle camera viewports");
            foreach (var camera in cameras)
            {
                var owner = camera.rect.x < .25f ? actor : enemy;
                float actual = Vector3.ProjectOnPlane(camera.transform.position - owner.transform.position, Vector3.up).magnitude;
                float expected = (PlayerPrefs.GetInt("Cam", 0) == 0 ? 4f : 6f) * 2f;
                if (Mathf.Abs(actual - expected) > .02f) fail("Split zoom distance " + actual + " expected " + expected);
            }
        }
        else
        {
            cameraManager.ChangeCameraParent(actor);
            foreach (bool far in new[] {false,true})
            {
                if (far) cameraManager.ChangeCameraToFar(); else cameraManager.ChangeCameraToClose();
                yield return new WaitForSeconds(cameraManager.transitionDuration + .1f);
                Vector3 expected = (far ? new Vector3(0,4,-6) : new Vector3(0,3,-4)) * 1.25f;
                if (Vector3.Distance(cameraManager.transform.localPosition, expected) > .02f) fail("Single camera zoom mismatch");
            }
        }
        observe("corrections-camera-" + index, "Verified battle camera zoom; local=" + local);

        if (index < 2)
        {
            foreach (int side in new[] {-1,1})
            {
                actor.transform.position = new Vector3(0,0,-8); enemy.transform.position = new Vector3(0,0,8);
                actor.transform.rotation = Quaternion.identity;
                var animator = game.animMainChar;
                animator.Rebind(); animator.Update(0);
                reader.SetXpos(side); reader.SetYpos(0);
                yield return new WaitForSeconds(.1f);
                var origin = actor.transform.position; var right = actor.transform.right;
                game.Jumping(true);
                yield return new WaitForSeconds(.18f);
                game.Jumping(false);
                bool entered = false;
                float until = Time.time + 1.6f;
                while (Time.time < until)
                {
                    entered |= animator.GetCurrentAnimatorStateInfo(0).IsName(side > 0 ? "JumpR" : "JumpL");
                    reader.SetXpos(0);
                    yield return null;
                }
                float travel = Vector3.Dot(actor.transform.position - origin, right) * side;
                if (!entered || travel < .4f) fail("Lightning evade wrong state/direction: " + actor.name + " side=" + side + " travel=" + travel);
                observe("lightning-evade", actor.name + " side=" + side + " correctState=" + entered + " directionalTravel=" + travel);
            }
        }
        if (index == 2 || index == 3)
        {
            foreach (var owner in new[] {actor,enemy}) foreach (int side in new[] {-1,1})
            {
                owner.transform.rotation = Quaternion.Euler(0, owner == actor ? 35 : 215, 0);
                var prefab = Resources.Load<GameObject>("photonprefabs/AirAtk" + (side < 0 ? "L2" : "R2"));
                actor.transform.position = new Vector3(0,0,-8); enemy.transform.position = new Vector3(0,0,8);
                owner.GetComponent<SummonAttack>().CreateKickAttack(prefab);
                var shot = UnityEngine.Object.FindObjectsByType<MoveAttack>(FindObjectsInactive.Exclude)
                    .Single(m => m.name == prefab.name + "(Clone)" && m.gameObject.layer == owner.layer).gameObject;
                foreach (var collider in shot.GetComponentsInChildren<Collider>()) collider.enabled = false;
                var origin = shot.transform.position;
                Vector3 expected = owner.transform.right * side;
                yield return new WaitForSeconds(.5f);
                if (Vector3.Distance(shot.transform.position, origin) > .001f) fail("Tornado moved before charge finished");
                yield return new WaitForSeconds(.4f);
                var launch = shot.transform.position;
                // Cross the rival and turn the owner after release: neither may bend the flight.
                enemy.transform.position = new Vector3(-12,0,5);
                owner.transform.rotation *= Quaternion.Euler(0,120,0);
                yield return new WaitForSeconds(.3f);
                Vector3 delta = shot.transform.position - launch;
                float along = Vector3.Dot(delta, expected);
                float sideways = (delta - expected * along).magnitude;
                var follow = shot.GetComponent<Follow>();
                if (along < 1f || sideways > .01f || (follow && follow.enabled)) fail("Tornado changed trajectory after launch");
                observe("tornado-fixed-flight", actor.name + " ownerLayer=" + owner.layer + " side=" + side + " along=" + along + " offAxis=" + sideways);
                UnityEngine.Object.Destroy(shot);
                yield return null;
            }
        }
        if (index == 4)
        {
            var original = typeof(IncreaseSize).GetField("originalSize", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (string animal in new[] {"Bat","Bird","Wolf","Spider","Snake","Shark"})
            {
                var prefab = Resources.Load<GameObject>("photonprefabs/Shadow" + animal + "Atk");
                var shot = UnityEngine.Object.Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
                shot.layer = actor.layer;
                foreach (var collider in shot.GetComponentsInChildren<Collider>()) collider.enabled = false;
                var growth = shot.GetComponentsInChildren<IncreaseSize>().Where(g => g.enabled).ToArray();
                int samples = 0;
                for (int frame = 0; frame < 20; frame++)
                {
                    yield return new WaitForSeconds(.02f);
                    foreach (var g in growth)
                    {
                        if (!g || !g.gameObject.activeInHierarchy) continue;
                        Vector3 full = (Vector3)original.GetValue(g), current = g.transform.localScale;
                        if (Mathf.Abs(full.x * full.y * full.z) < 1e-10f) continue;
                        Vector3 ratio = new Vector3(current.x/full.x,current.y/full.y,current.z/full.z);
                        if (Mathf.Abs(ratio.x-ratio.y) > .001f || Mathf.Abs(ratio.y-ratio.z) > .001f)
                            fail("Animal growth distorted proportions: " + animal + "/" + g.name + " " + ratio);
                        samples++;
                    }
                }
                if (samples == 0) fail("No growth observations for " + animal);
                observe("raven-proportions", animal + " uniform normalized growth samples=" + samples);
                ScreenCapture.CaptureScreenshot(prefix + "." + animal + ".png");
                yield return new WaitForEndOfFrame(); yield return null;
                UnityEngine.Object.Destroy(shot);
            }
        }
        if (local)
        {
            bool hostWins = index == 5;
            var loser = hostWins ? enemy : actor;
            var winner = hostWins ? actor : enemy;
            if (hostWins) Score.score.playerPoints = 2; else Score.score.enemyPoints = 2;
            var scoring = loser.GetComponent<ScoreController>();
            scoring.AddPoints(); scoring.CheckPoints();
            yield return new WaitForSeconds(2f + cameraManager.transitionDuration);
            var main = cameraManager.GetComponent<Camera>();
            var right = GameObject.Find("LocalOfflinePlayer2Camera").GetComponent<Camera>();
            if (!main.enabled || main.rect != new Rect(0,0,1,1) || right.enabled || cameraManager.transform.parent != winner.transform)
                fail("Final fall did not switch to winner-only camera");
            if (GameObject.Find("LocalOfflineSplitOverlay").GetComponent<Canvas>().enabled) fail("Divider remains on winner view");
            observe("winner-camera", "Final third fall; hostWins=" + hostWins + "; winner=" + winner.name + "; full-screen camera parent=" + cameraManager.transform.parent.name);
            ScreenCapture.CaptureScreenshot(prefix + ".winner.png");
            yield return new WaitForEndOfFrame(); yield return null;
            // Check the visible selection button and its new longer label after results appear.
            yield return new WaitForSeconds(3f);
            Canvas.ForceUpdateCanvases();
            foreach (var label in UnityEngine.Object.FindObjectsByType<Text>(FindObjectsInactive.Exclude))
                if (label.text == "SELECT CHARACTER" && label.rectTransform.rect.width > 0 && label.preferredWidth > label.rectTransform.rect.width + 1 && !label.resizeTextForBestFit)
                    fail("Selection label does not fit its button");
        }
    }
}
#endif
