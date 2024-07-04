using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public GameObject scamera;
    public Canvas menu;
    public Canvas creditsMenu;
    public bool gameStarted;
    public bool winnerCam;
    public GameObject winner;
    public Canvas winnerCanvas;
    public int p1Wins;
    public int p2Wins;
    public AudioSource audioSource;
    public AudioClip buttonClick;
    public AudioClip p1WinSound;
    public AudioClip p2WinSound;
    public AudioClip beginSound;
    public int currentRound;
    public Image crown;
    public bool musicOut = false;
    public Canvas gameUI;
    public Button playB;
    public Button creditB;
    public Button closeCreditB;
    public GameObject keyHelp;
    public GameObject conHelp;

    private void Start()
    {
        menu.gameObject.SetActive(true);
        scamera.GetComponent<camera>().camFollow = false;
        scamera.transform.position = new Vector3(0, 10, -10);
        gameStarted = false;
        winnerCam = false;
        p1Wins = 0;
        p2Wins = 0;
        currentRound = 1;
        gameUI.enabled = false;
        winnerCanvas.enabled = false;
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
        if (winnerCanvas == enabled)
        {
            winnerCanvas.transform.position = new Vector2(winner.transform.position.x, winner.transform.position.y + 2f);
        }
    }

    public void PlayButton()
    {
        buttonClickSound();
        menu.gameObject.SetActive(false);
        StartGame();
    }

    public void CreditsButton()
    {
        buttonClickSound();
        creditsMenu.enabled = true;
        menu.enabled = false;
        closeCreditB.Select();
        EventSystem.current.SetSelectedGameObject(closeCreditB.gameObject);
    }

    public void KeyboardButton()
    {
        buttonClickSound();
        conHelp.active = false;
        keyHelp.active = true;
    }

    public void ControllerButton()
    {
        buttonClickSound();
        keyHelp.active = false;
        conHelp.active = true;
    }

    public void CloseCreditsButton()
    {
        buttonClickSound();
        creditsMenu.enabled = false;
        menu.enabled = true;
        EventSystem.current.SetSelectedGameObject(playB.gameObject);
    }

    public void QuitButton()
    {
        buttonClickSound();
        Application.Quit();
    }

    public void StartGame()
    {
        buttonClickSound();
        audioSource.clip = beginSound;
        audioSource.volume = 1;
        audioSource.Play();
        scamera.GetComponent<camera>().camFollow = true;
        gameStarted = true;
        gameUI.enabled = true;
    }

    public void EndRound(GameObject opponent)
    {
        winner = opponent;
        gameStarted = false;
        scamera.GetComponent<camera>().camFollow = false;
        winnerCam = true;
        winnerCanvas.transform.position = new Vector2(winner.transform.position.x, winner.transform.position.y + 2f);
        winnerCanvas.enabled = true;
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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<player>().respawn();
        }
        winnerCanvas.enabled = false;
        winnerCam = false;
        gameUI.enabled = false;
        StartGame();
    }

    public void buttonClickSound()
    {
        audioSource.clip = buttonClick;
        audioSource.Play();
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
        yield return new WaitForSeconds(5);
        currentRound++;
        RestartMap();
    }
}
