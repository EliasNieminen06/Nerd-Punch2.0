using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] musicClips;
    public AudioClip menuMusic;
    public GameManager gameManager;
    public bool menuMusicIsPlaying;

    private void Start()
    {
        audioSource.volume = 0.2f;
    }
    void Update()
    {
        if (gameManager.gameStarted)
        {   
            if (menuMusicIsPlaying)
            {
                menuMusicIsPlaying = false;
                audioSource.Stop();
            }
            if (!audioSource.isPlaying)
            {
                int musicClip = Random.Range(0, musicClips.Length);
                audioSource.clip = musicClips[musicClip];
                audioSource.Play();
            }
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                menuMusicIsPlaying = true;
                audioSource.clip = menuMusic;
                audioSource.Play();
            }
        }
        if (gameManager.musicOut)
        {
            audioSource.Stop();
        }
    }
}
