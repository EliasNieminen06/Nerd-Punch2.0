using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject camera;
    public Canvas menu;

    private void Start()
    {
        menu.enabled = true;
        camera.GetComponent<camera>().enabled = false;
        camera.transform.position = new Vector2(0, 50);
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
        camera.GetComponent<camera>().enabled = true;
    }
}
