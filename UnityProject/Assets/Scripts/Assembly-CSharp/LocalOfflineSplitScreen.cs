using UnityEngine;
using UnityEngine.UI;

/// <summary>Maintains one independent vertical camera viewport for each local fighter.</summary>
public sealed class LocalOfflineSplitScreen : MonoBehaviour
{
    private Transform firstPlayer;
    private Transform secondPlayer;
    private Camera leftCamera;
    private Camera rightCamera;
    private Canvas overlay;
    private bool showingWinner;

    public void ShowWinner()
    {
        showingWinner = true;
        if (leftCamera) { leftCamera.enabled = true; leftCamera.rect = new Rect(0f, 0f, 1f, 1f); }
        if (rightCamera) rightCamera.enabled = false;
        if (overlay) overlay.enabled = false;
    }

    public void Configure(GameObject player1, GameObject player2)
    {
        if (!player1 || !player2) return;
        firstPlayer = player1.transform;
        secondPlayer = player2.transform;
        var cameraManager = FindAnyObjectByType<CameraManager>();
        leftCamera = cameraManager ? cameraManager.GetComponent<Camera>() : Camera.main;
        if (!leftCamera)
        {
            Debug.LogError("Local Offline split screen needs a scene camera.", this);
            return;
        }
        leftCamera.rect = new Rect(0f, 0f, .5f, 1f);
        var rightObject = new GameObject("LocalOfflinePlayer2Camera", typeof(Camera));
        rightCamera = rightObject.GetComponent<Camera>();
        rightCamera.CopyFrom(leftCamera);
        rightCamera.rect = new Rect(.5f, 0f, .5f, 1f);
        rightCamera.depth = leftCamera.depth + .01f;
        rightCamera.tag = "Untagged";
        rightCamera.transform.SetParent(transform, false);
        CreateDivider();
        UpdateViews();
    }

    private void LateUpdate() { UpdateViews(); }

    private void UpdateViews()
    {
        if (showingWinner || !leftCamera || !rightCamera || !firstPlayer || !secondPlayer) return;
        UpdateCamera(leftCamera.transform, firstPlayer, secondPlayer);
        UpdateCamera(rightCamera.transform, secondPlayer, firstPlayer);
    }

    private static void UpdateCamera(Transform cameraTransform, Transform player, Transform opponent)
    {
        Vector3 direction = opponent.position - player.position;
        direction.y = 0f;
        if (direction.sqrMagnitude < .01f) direction = player.forward;
        direction.Normalize();
        bool farCamera = PlayerPrefs.GetInt("Cam", 0) != 0;
        float distance = (farCamera ? 6f : 4f) * 1.6f * 1.25f;
        float cameraHeight = distance * (farCamera ? 4f / 6f : 3f / 4f);
        Vector3 target = (player.position + opponent.position) * .5f + Vector3.up * 1.2f;
        cameraTransform.position = player.position - direction * distance + Vector3.up * cameraHeight;
        cameraTransform.rotation = Quaternion.LookRotation(target - cameraTransform.position, Vector3.up);
    }

    private void CreateDivider()
    {
        var canvasObject = new GameObject("LocalOfflineSplitOverlay", typeof(Canvas), typeof(CanvasScaler));
        overlay = canvasObject.GetComponent<Canvas>();
        overlay.renderMode = RenderMode.ScreenSpaceOverlay;
        overlay.sortingOrder = 150;
        var scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        var divider = new GameObject("PlayerSplitDivider", typeof(RectTransform), typeof(Image));
        divider.transform.SetParent(canvasObject.transform, false);
        var rect = divider.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(.5f, 0f);
        rect.anchorMax = new Vector2(.5f, 1f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(3f, 0f);
        divider.GetComponent<Image>().color = new Color(.03f, .06f, .09f, .9f);
    }

    private void OnDestroy()
    {
        if (rightCamera) Destroy(rightCamera.gameObject);
        if (overlay) Destroy(overlay.gameObject);
        if (leftCamera) leftCamera.rect = new Rect(0f, 0f, 1f, 1f);
    }
}
