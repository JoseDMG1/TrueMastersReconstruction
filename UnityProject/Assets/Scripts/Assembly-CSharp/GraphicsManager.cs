using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Reconstructed from mapped ARM64 + metadata; not original C# source.
// Evidence/native-disassembly-GraphicsManager.txt and graphics-manager-native-constants.json.
public class GraphicsManager : MonoBehaviour
{
    public static GraphicsManager i;
    public float fps;
    public float deltaTime;
    private int originalHeight;
    private int originalWidth;
    private int medResH;
    private int medResW;
    private int minResH;
    private int minResW;
    public Text max;
    public Text med;
    public Text min;
    public Toggle frameCapToogle;
    public Text fpsText;
    public Text resText;
    public Text qualityText;
    private int userQualityGraphics;
    private int userFrameCap;
    public GameObject debug;
    public UnityAction graphicsChange;

    // Native VA 0x12966D0. Original duplicate path destroys this component and continues Start.
    private void Start()
    {
        if (i == null)
        {
            i = this;
        }
        else if (i != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        Time.timeScale = 1.25f;
        switch (PlayerPrefs.GetInt("GameSpeed"))
        {
            // Constants read from native float table VA 0x67E0E0.
            case 0: Time.timeScale = 1.15f; break;
            case 1: Time.timeScale = 1.275f; break;
            case 2: Time.timeScale = 1.4f; break;
        }
        print(Time.timeScale);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        setResolutions();
        setInitialQualityGraphics();
        setInitialFrameCap();
    }

    // Native VA 0x1296B00. Original does not explicitly clear singleton i.
    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }

    // Native VA 0x1296B0C; smoothing constant float0.1 from native VA 0x651BA8.
    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = Mathf.Ceil(Time.timeScale / deltaTime);
        fpsText.text = fps.ToString();
    }

    // Native VA 0x1296BCC. Branch constants are35 and50, distinct from GameManager.
    private void SetInitialRes()
    {
        if (fps < 35f)
            setMinRes();
        else if (fps < 50f)
            setMedRes();
        else
            setOriginalRes();
    }

    // Native VA 0x1296BFC. Callback runs before restoring vSync and changing resolution.
    public void setMinRes()
    {
        if (Screen.currentResolution.height != minResH)
        {
            int previousVSync = QualitySettings.vSyncCount;
            QualitySettings.SetQualityLevel(1, true);
            PlayerPrefs.SetInt("userQualityGraphics", 1);
            graphicsChange?.Invoke();
            QualitySettings.vSyncCount = previousVSync;
            Screen.SetResolution(minResW, minResH, true);
        }
        restartButtonsColor();
        min.color = Color.white;
    }

    // Native VA 0x1296D00.
    public void setMedRes()
    {
        if (Screen.currentResolution.height != medResH)
        {
            int previousVSync = QualitySettings.vSyncCount;
            QualitySettings.SetQualityLevel(2, true);
            PlayerPrefs.SetInt("userQualityGraphics", 2);
            graphicsChange?.Invoke();
            QualitySettings.vSyncCount = previousVSync;
            Screen.SetResolution(medResW, medResH, true);
        }
        restartButtonsColor();
        med.color = Color.white;
    }

    // Native VA 0x1296E04. Unlike min/medium, original always reapplies quality/resolution.
    public void setOriginalRes()
    {
        int previousVSync = QualitySettings.vSyncCount;
        QualitySettings.SetQualityLevel(3, true);
        PlayerPrefs.SetInt("userQualityGraphics", 3);
        graphicsChange?.Invoke();
        QualitySettings.vSyncCount = previousVSync;
        Screen.SetResolution(originalWidth, originalHeight, true);
        restartButtonsColor();
        max.color = Color.white;
    }

    // Native VA 0x1296F68. No online-state dependency in this class.
    public void CapFrameRate(bool cap)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = cap ? 30 : 60;
        PlayerPrefs.SetInt("userFrameCap", cap ? 1 : 0);
    }

    // Native VA 0x1296FFC; exact Animator trigger literal from metadata.
    public void toogleSettings()
    {
        debug.GetComponent<Animator>().SetTrigger("toogle");
    }

    // Native VA 0x1296A68. Default is0, unlike GameManager's-1.
    private void setInitialFrameCap()
    {
        userFrameCap = PlayerPrefs.GetInt("userFrameCap", 0);
        if (userFrameCap == 0)
        {
            CapFrameRate(false);
        }
        else if (userFrameCap == 1)
        {
            CapFrameRate(true);
            frameCapToogle.isOn = true;
        }
    }

    // Native VA 0x12969A8.
    private void setInitialQualityGraphics()
    {
        userQualityGraphics = PlayerPrefs.GetInt("userQualityGraphics", 0);
        switch (userQualityGraphics)
        {
            case 1: setMinRes(); break;
            case 2: setMedRes(); break;
            case 3: setOriginalRes(); break;
            default: Invoke(nameof(SetInitialRes), 5f); break;
        }
    }

    // Native VA 0x12968C0. Vector constants VA0x652560:0.75,0.75,0.5,0.5.
    private void setResolutions()
    {
        originalHeight = Display.main.systemHeight;
        originalWidth = Display.main.systemWidth;
        medResH = (int)(originalHeight * 0.75f);
        medResW = (int)(originalWidth * 0.75f);
        minResH = (int)(originalHeight * 0.5f);
        minResW = (int)(originalWidth * 0.5f);
        if (originalHeight < 720 || originalWidth < 720)
        {
            medResH = minResH = originalHeight;
            medResW = minResW = originalWidth;
        }
    }

    // Native VA 0x1296ED4. MOVI RGB0 and FMOV alpha1 verified independently of ISIL.
    private void restartButtonsColor()
    {
        max.color = Color.black;
        med.color = Color.black;
        min.color = Color.black;
    }
}
