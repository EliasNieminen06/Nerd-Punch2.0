using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] musicClips;
    public GameManager gameManager;
    private float fadeTime = 2f;
    private void Start()
    {
        audioSource.volume = 0;
    }
    void Update()
    {
        if (gameManager.gameStarted)
        {
            if (!audioSource.isPlaying)
            {
                int musicClip = Random.Range(0, musicClips.Length);
                audioSource.clip = musicClips[musicClip];
                audioSource.Play();
                StartCoroutine(FadeIn());
            }
        }
        if (gameManager.musicOut)
        {
            StartCoroutine(FadeOut());
        }
    }
    public IEnumerator FadeOut()
    {

        while (audioSource.volume <= 0.25f)
        {
            audioSource.volume -= 0.2f * Time.deltaTime / (fadeTime * 1000);

            yield return null;
        }
    }
    public IEnumerator FadeIn()
    {

        while (audioSource.volume < 0.20f)
        {
            audioSource.volume += 100 * Time.deltaTime / (fadeTime * 1000);

            yield return null;
        }
    }
}
