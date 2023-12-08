using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private Image barImage;
    public Color fullColor = Color.white;
    public Color lowColor = Color.red;
    public int health;
    public int maxHealth;
    public float width;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1.1f);
        float percentage = Mathf.Clamp01((float)health / maxHealth);
        width = percentage * 7f;
        healthBar.sizeDelta = new Vector2(width, healthBar.sizeDelta.y);
    }
}
