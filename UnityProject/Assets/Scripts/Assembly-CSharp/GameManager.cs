using UnityEngine;
using UnityEngine.UI;

// Reconstructed from mapped ARM64 + metadata, not recovered C# source.
// Evidence/native-disassembly-GameManager.txt and game-manager-native-constants.json.
public class GameManager : MonoBehaviour
{
    public GameObject resumeButton;
    public float fps;
    public float deltaTime;
    private int originalHeight;
    private int originalWidth;
    private int medResH;
    private int medResW;
    private int minResH;
    private int minResW;
    public Slider qualitySlider;
    public Text max;
    public Text med;
    public Text min;
    public Toggle frameCapToogle;
    public Text fpsText;
    public Text resText;
    public Text qualityText;
    private int userQualityGraphics;
    private int userFrameCap;

    // Native VA 0x12946AC.
    private void Start()
    {
        Time.timeScale = 1.25f;
        setResolutions();
        setInitialQualityGraphics();
        setInitialFrameCap();
    }

    // Native VA 0x12948F0. Corrected FCMP/FDIV/FRINTP using mapped Capstone bytes.
    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = Mathf.Ceil(Time.timeScale / deltaTime);
        fpsText.text = fps.ToString();
    }

    // Native VA 0x12949B0. Startup sampling is scheduled five seconds after Start.
    private void SetInitialRes()
    {
        if (fps < 10f)
        {
            setMinRes();
            CapFrameRate(true);
            frameCapToogle.isOn = true;
        }
        else if (fps < 30f)
        {
            setMedRes();
            CapFrameRate(true);
            frameCapToogle.isOn = true;
        }
        else if (fps < 45f)
        {
            setMedRes();
        }
        else
        {
            setOriginalRes();
        }
    }

    // Native VA 0x1294A30. Native code compares height only before applying quality.
    public void setMinRes()
    {
        if (Screen.currentResolution.height != minResH)
        {
            int previousVSync = QualitySettings.vSyncCount;
            QualitySettings.SetQualityLevel(1, true);
            PlayerPrefs.SetInt("userQualityGraphics", 1);
            QualitySettings.vSyncCount = previousVSync;
            Screen.SetResolution(minResW, minResH, true);
        }
        restartButtonsColor();
        min.color = Color.white;
    }

    // Native VA 0x1294BFC.
    public void setMedRes()
    {
        if (Screen.currentResolution.height != medResH)
        {
            int previousVSync = QualitySettings.vSyncCount;
            QualitySettings.SetQualityLevel(2, true);
            PlayerPrefs.SetInt("userQualityGraphics", 2);
            QualitySettings.vSyncCount = previousVSync;
            Screen.SetResolution(medResW, medResH, true);
        }
        restartButtonsColor();
        med.color = Color.white;
    }

    // Native VA 0x1294CE8.
    public void setOriginalRes()
    {
        if (Screen.currentResolution.height != originalHeight)
        {
            int previousVSync = QualitySettings.vSyncCount;
            QualitySettings.SetQualityLevel(3, true);
            PlayerPrefs.SetInt("userQualityGraphics", 3);
            QualitySettings.vSyncCount = previousVSync;
            Screen.SetResolution(originalWidth, originalHeight, true);
        }
        restartButtonsColor();
        max.color = Color.white;
    }

    // Native VA 0x1294B1C. Online mode forces the original uncapped/60fps branch.
    public void CapFrameRate(bool cap)
    {
        bool useThirtyFps = !GameController.i.online && cap;
        QualitySettings.vSyncCount = useThirtyFps ? 0 : 1;
        Application.targetFrameRate = useThirtyFps ? 30 : 60;
        PlayerPrefs.SetInt("userFrameCap", useThirtyFps ? 1 : 0);
    }

    // Native VA 0x1294E68. Normal UI slider values truncate toward zero.
    public void changeQuality(float value)
    {
        QualitySettings.SetQualityLevel(unchecked((int)value), true);
    }

    // Native VA 0x1294E8C. Metadata usage resolves GraphicsManager.i.debug and "toogle".
    public void toogleSettings()
    {
        GraphicsManager.i.debug.GetComponent<Animator>().SetTrigger("toogle");
    }

    // Native VA 0x1294858. Missing preference (-1) intentionally performs no action.
    private void setInitialFrameCap()
    {
        userFrameCap = PlayerPrefs.GetInt("userFrameCap", -1);
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

    // Native VA 0x1294798.
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

    // Native VA 0x12946DC. Ratios decoded from ELF VA 0x6525A0 (SIMD float vector).
    private void setResolutions()
    {
        originalHeight = Display.main.systemHeight;
        originalWidth = Display.main.systemWidth;
        medResH = (int)(originalHeight * 0.6667f);
        medResW = (int)(originalWidth * 0.6667f);
        minResH = (int)(originalHeight * 0.3337f);
        minResW = (int)(originalWidth * 0.3337f);
    }

    // Native VA 0x1294DD4. Mapped MOVI instructions zero RGB with alpha=1.
    private void restartButtonsColor()
    {
        max.color = Color.black;
        med.color = Color.black;
        min.color = Color.black;
    }
}
