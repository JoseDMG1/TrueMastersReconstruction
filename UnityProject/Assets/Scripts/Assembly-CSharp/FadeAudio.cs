using System.Collections;
using UnityEngine;

// Corrected ARM64 FadeAudio 0x122B1E8 and iterator bodies0x122B530/0x122B66C.
public class FadeAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 3f;
    [Header("Debug Info")][SerializeField] private float clipTimer;
    [SerializeField] private float songTime;
    private float targetVolume;
    public bool isFading;

    private void Start()
    {
        if (audioSource == null) return;
        targetVolume = audioSource.volume;
        audioSource.volume = 0f;
        audioSource.loop = true;
        audioSource.Play();
        songTime = audioSource.clip.length;
    }
    private void Update()
    {
        if (audioSource == null || !audioSource.isPlaying) return;
        clipTimer = audioSource.time;
        if (clipTimer <= fadeDuration && !isFading)
            StartCoroutine(FadeIn(audioSource, fadeDuration, targetVolume));
        if (clipTimer >= songTime - fadeDuration && !isFading)
            StartCoroutine(FadeOut(audioSource, fadeDuration));
    }
    public IEnumerator FadeIn(AudioSource source, float duration, float targetVolume)
    {
        if (isFading) yield break;
        isFading = true;
        try
        {
            while (source.volume < targetVolume)
            {
                source.volume += Time.deltaTime / duration * targetVolume;
                yield return null;
            }
            source.volume = targetVolume;
        }
        finally { isFading = false; }
    }
    public IEnumerator FadeOut(AudioSource source, float duration)
    {
        if (isFading) yield break;
        isFading = true;
        float startVolume = source.volume;
        try
        {
            while (source.volume > 0f)
            {
                source.volume -= Time.deltaTime / duration * startVolume;
                yield return null;
            }
            source.volume = 0f;
        }
        finally { isFading = false; }
    }
}
