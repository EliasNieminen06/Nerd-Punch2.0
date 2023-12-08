using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        menu.enabled = true;
        scamera.GetComponent<camera>().enabled = false;
        scamera.transform.position = new Vector2(0, 50);
        gameStarted = false;
        winnerCam = false;
        p1Wins = 0;
        p2Wins = 0;
    }

    private void Update()
    {
        if (winnerCam)
        {
            scamera.transform.position = new Vector3(winner.transform.position.x, winner.transform.position.y, -10);
        }
    }

    public void PlayButton()
    {
        menu.enabled = false;
        StartGame();
    }

    public void StartGame()
    {
        scamera.GetComponent<camera>().enabled = true;
        gameStarted = true;
    }

    public void EndGame(GameObject opponent)
    {
        winner = opponent;
        gameStarted = false;
        scamera.GetComponent<camera>().enabled = false;
        winnerCam = true;
        winnerCanvas.transform.position = new Vector2(winner.transform.position.x, winner.transform.position.y + 2f);
        winnerCanvas.enabled = true;
        if (opponent.gameObject.name == "PlayerOne")
        {
            p1Wins += 1;
        }
        else
        {
            p2Wins += 1;
        }
    }
}
