using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private RectTransform healthBar;
    public int health;
    public int maxHealth;
    public int width;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 7;
        health = 7;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1.1f);
    }
}
