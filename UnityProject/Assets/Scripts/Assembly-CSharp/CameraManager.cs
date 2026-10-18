using System.Collections;
using UnityEngine;

// Native CameraManager fields, local camera poses and Cam preference retained.
public class CameraManager : MonoBehaviour
{
    public static CameraManager cameraM;
    public float transitionSpeed;
    public float transitionDuration;
    private float elapsedTime;
    private Vector3 ViewCameraClose = new Vector3(0f, 3f, -4f);
    private Vector3 ViewCameraFar = new Vector3(0f, 4f, -6f);
    private Quaternion ViewCamaraRot = Quaternion.Euler(15f, 0f, 0f);
    private Vector3 ViewWinCamera = new Vector3(0f, 1.5f, 3.5f);
    private Quaternion ViewWinCamaraRot = Quaternion.Euler(0f, 180f, 0f);
    public Vector3 SceneCameraPos = new Vector3(20f, 2f, -20f);
    public Quaternion SceneCameraRot = Quaternion.Euler(2f, -45f, 0f);
    private Vector3 SceneCameraPosFar = new Vector3(24f, 7.5f, -24f);
    private Quaternion SceneCameraRotFar = Quaternion.Euler(15f, -45f, 0f);

    private void Start() { cameraM = this; }
    private bool IsSplitScreenMode => SystemManager.i && SystemManager.i.mode == GameMode.LocalOffline;
    public void ChangeCameraParent(GameObject newParent) { if (!IsSplitScreenMode) transform.parent = newParent.transform; }
    public void ChangeCameraToClose() { if (!IsSplitScreenMode) BeginTransition(ViewCameraClose * 1.25f, ViewCamaraRot); }
    public void ChangeCameraToFar() { if (!IsSplitScreenMode) BeginTransition(ViewCameraFar * 1.25f, ViewCamaraRot); }
    public void ChangeToSceneCamera()
    {
        if (IsSplitScreenMode) return;
        if (PlayerPrefs.GetInt("Cam", 0) != 0) ChangeToSceneCameraFar();
        else ChangeToSceneCameraClose();
    }
    public void ChangeToSceneCameraClose()
    {
        if (IsSplitScreenMode) return;
        transform.parent = null;
        BeginTransition(SceneCameraPos, SceneCameraRot);
    }
    public void ChangeToSceneCameraFar()
    {
        if (IsSplitScreenMode) return;
        transform.parent = null;
        BeginTransition(SceneCameraPosFar, SceneCameraRotFar);
    }
    public void ChangeCameraToWinner(GameObject winner)
    {
        if (!winner) return;
        if (IsSplitScreenMode)
        {
            var splitScreen = FindAnyObjectByType<LocalOfflineSplitScreen>();
            if (splitScreen) splitScreen.ShowWinner();
        }
        transform.SetParent(winner.transform, true);
        BeginTransition(ViewWinCamera, ViewWinCamaraRot);
    }
    private void BeginTransition(Vector3 position, Quaternion rotation)
    {
        StopAllCoroutines();
        elapsedTime = 0f;
        StartCoroutine(ChangeCameraSmooth(position, rotation));
    }
    private IEnumerator ChangeCameraSmooth(Vector3 targetPosition, Quaternion targetRotation)
    {
        // Reconstruction correction: native 0x128104C increments elapsedTime only on
        // initial state, never on resume. Advance on each frame so the transition completes.
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, t);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, t);
            yield return new WaitForEndOfFrame();
        }
        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;
        elapsedTime = 0f;
    }
    public void SwitchCamera()
    {
        if (IsSplitScreenMode) return;
        PlayerPrefs.SetInt("Cam", PlayerPrefs.GetInt("Cam", 0) == 0 ? 1 : 0);
        if (transform.parent == null) ChangeToSceneCamera();
        else if (PlayerPrefs.GetInt("Cam", 0) != 0) ChangeCameraToFar();
        else ChangeCameraToClose();
    }
}
