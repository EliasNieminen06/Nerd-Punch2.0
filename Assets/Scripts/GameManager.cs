using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject scamera;
    public Canvas menu;
    public bool gameStarted;
    public bool winnerCam;
    public GameObject winner;
    public Canvas winnerCanvas;
    public int p1Wins;
    public int p2Wins;
    public AudioSource audioSource;
    public AudioClip p1WinSound;
    public AudioClip p2WinSound;
    public AudioClip beginSound;
    public int currentRound;
    public Image crown;
    public bool musicOut = false;

    private void Start()
    {
        Debug.Log("GM.Start()");
        menu.enabled = true;
        scamera.GetComponent<camera>().camFollow = false;
        scamera.transform.position = new Vector2(0, 50);
        gameStarted = false;
        winnerCam = false;
        p1Wins = 0;
        p2Wins = 0;
        currentRound = 1;
    }

    private void Update()
    {
        if (winnerCam)
        {
            scamera.transform.position = new Vector3(winner.transform.position.x, winner.transform.position.y, -10);
        }
        if (currentRound > 5 || p1Wins == 3 || p2Wins == 3)
        {
            StartCoroutine(RestartGame());
        }
    }

    public void PlayButton()
    {
        Debug.Log("GM.PlayButton()");
        menu.enabled = false;
        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("GM.StartGame()");
        audioSource.clip = beginSound;
        audioSource.volume = 1;
        audioSource.Play();
        scamera.GetComponent<camera>().camFollow = true;
        gameStarted = true;
    }

    public void EndRound(GameObject opponent)
    {
        Debug.Log("GM.EndGame()");
        winner = opponent;
        gameStarted = false;
        scamera.GetComponent<camera>().camFollow = false;
        winnerCam = true;
        winnerCanvas.enabled = true;
        winnerCanvas.transform.position = new Vector2(winner.transform.position.x, winner.transform.position.y + 2f);
        if (opponent.gameObject.name == "PlayerOne")
        {
            p1Wins += 1;
            audioSource.clip = p1WinSound;
            audioSource.volume = 1;
            audioSource.Play();
        }
        else
        {
            p2Wins += 1;
            audioSource.clip = p2WinSound;
            audioSource.volume = 1;
            audioSource.Play();
        }

        StartCoroutine(NextRound());
    }

    public void RestartMap()
    {
        Debug.Log("GM.RestartMap()");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<player>().respawn();
        }
        winnerCanvas.enabled = false;
        winnerCam = false;
        StartGame();
    }

    public IEnumerator RestartGame()
    {
        crown.enabled = true;
        musicOut = true;
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator NextRound()
    {
        Debug.Log("GM.NextRound()");
        yield return new WaitForSeconds(5);
        currentRound++;
        RestartMap();
    }
}
