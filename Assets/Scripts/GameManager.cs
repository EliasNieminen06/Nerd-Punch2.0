using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject scamera;
    public Canvas menu;

    private void Start()
    {
        menu.enabled = true;
        scamera.GetComponent<camera>().enabled = false;
        scamera.transform.position = new Vector2(0, 50);
    }

    private void Update()
    {
        
    }

    public void PlayButton()
    {
        menu.enabled = false;
        StartGame();
    }

    private void StartGame()
    {
        scamera.GetComponent<camera>().enabled = true;
    }
}
