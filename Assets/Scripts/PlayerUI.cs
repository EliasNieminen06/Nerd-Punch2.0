using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private Image barImage;
    public int health;
    public int maxHealth;
    public float width;
    public Color32 barColor;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        // Place the ui above the player
        this.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1.1f);
        // Calculate the percentage of current health from the max health
        float percentage = Mathf.Clamp01((float)health / maxHealth);
        // Set the width variable to be a float of the health percentage
        width = percentage * 7f;
        // set the health bar size to width variable's float number
        healthBar.sizeDelta = new Vector2(width, healthBar.sizeDelta.y);
        // float colorPercentage = percentage * 2.55f;
        barColor = new Color(255, percentage, percentage, 255);
        barImage.color = barColor;
    }
}
