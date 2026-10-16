using UnityEngine;

public class HowToPlayManager : MonoBehaviour
{
    public void Continue()
    {
        // Native VA 0x123C508: exact recovered Character Selection build index.
        LevelChanger.i.FadeToLevel(4);
    }
}

