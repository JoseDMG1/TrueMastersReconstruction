using System.Collections;
using UnityEngine;

// Only singleton, boot timing and offline mode state have been reconstructed.
public class SystemManager : MonoBehaviour
{
    public static SystemManager i;
    public float timesPlayed;
    public GameMode mode;
    private AsyncOperation async;
    public bool unlock;
    public bool newUpdate;
    public float bonus = 5f;
    public TMPlayer tmPlayer;
    public float playedTime;
    public bool howToPlayShowed;
    public PlayerType playerType;

    private void Awake()
    {
        if (i == null) i = this;
        else if (i != this)
        {
            Destroy(this);
            return;
        }

        LoadDataFromDevice();
        mode = GameMode.StartMenu;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        if (tmPlayer != null) playedTime = tmPlayer.playedTime;
        StartCoroutine(NextScene(0.1f));
    }

    public void ChangePlayingMode(GameMode playingMode)
    {
        mode = playingMode;
        RefreshPlayingMode();
    }

    public void RefreshPlayingMode()
    {
        // Original updates Firebase presence when connected. No backend is contacted.
    }

    private IEnumerator NextScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        LevelChanger.i.FadeToNextLevel();
    }

    // Original used BinaryFormatter at /player.save. Reconstruction owns a
    // separate JSON file; it never attempts unsafe legacy deserialization.
    private string SavePath => System.IO.Path.Combine(Application.persistentDataPath, "reconstruction-player.json");
    public void SaveData(string data)
    {
        tmPlayer.playedTime = playedTime;
        SavePlayerData(tmPlayer);
    }
    public void SaveDataInDevice() { SavePlayerData(tmPlayer); }
    public void LoadDataFromDevice() { tmPlayer = LoadPlayerData(); }
    public void SavePlayerData(TMPlayer player)
    {
        if (player == null) throw new System.ArgumentNullException(nameof(player));
        string next = SavePath + ".tmp";
        System.IO.Directory.CreateDirectory(Application.persistentDataPath);
        System.IO.File.WriteAllText(next, JsonUtility.ToJson(player, true));
        if (System.IO.File.Exists(SavePath))
            System.IO.File.Replace(next, SavePath, SavePath + ".bak");
        else System.IO.File.Move(next, SavePath);
    }

    public TMPlayer LoadPlayerData()
    {
        if (!System.IO.File.Exists(SavePath)) return new TMPlayer();
        var player = JsonUtility.FromJson<TMPlayer>(System.IO.File.ReadAllText(SavePath));
        if (player == null || player.characters == null || player.stages == null)
            throw new System.IO.InvalidDataException("Incomplete reconstruction player save; original file preserved.");
        return player;
    }

    private void Update() { playedTime += Time.deltaTime; }
    public void CheckForUpdate() { Unavailable(nameof(CheckForUpdate)); }
    public void CheckBonus() { Unavailable(nameof(CheckBonus)); }
    private void OnApplicationQuit() { }

    private void Unavailable(string method)
    {
        Debug.LogError("SystemManager." + method + " has not been reconstructed; stored data was not changed.", this);
    }
}

