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

    private void Start()
    {
        menu.enabled = true;
        scamera.GetComponent<camera>().enabled = false;
        scamera.transform.position = new Vector2(0, 50);
        gameStarted = false;
        winnerCam = false;
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
    }
}
