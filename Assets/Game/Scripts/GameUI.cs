using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private RectTransform p1healthBar;
    [SerializeField] private Image p1barImage;
    [SerializeField] private RectTransform p2healthBar;
    [SerializeField] private Image p2barImage;
    [SerializeField] private TextMeshProUGUI p1htxt;
    [SerializeField] private TextMeshProUGUI p2htxt;
    [SerializeField] private TextMeshProUGUI p1score;
    [SerializeField] private TextMeshProUGUI p2score;
    [SerializeField] private TextMeshProUGUI round;
    [SerializeField] private player player1;
    [SerializeField] private player player2;
    [SerializeField] private RectTransform p1dash;
    [SerializeField] private RectTransform p2dash;
    public int p1health;
    public int p2health;
    public int maxHealth;
    public float p1width;
    public float p2width;
    public Color32 p1barColor;
    public Color32 p2barColor;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        p1health = 100;
        p2health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the percentage of current health from the max health
        float p1percentage = Mathf.Clamp01((float)p1health / maxHealth);
        // Set the width variable to be a float of the health percentage
        p1width = p1percentage * 1000f;
        // set the health bar size to width variable's float number
        p1healthBar.sizeDelta = new Vector2(p1width, p1healthBar.sizeDelta.y);
        // float colorPercentage = percentage * 2.55f;
        p1barColor = new Color(255, p1percentage, p1percentage, 255);
        p1barImage.color = p1barColor;
        p1htxt.text = p1health.ToString();

        // Calculate the percentage of current health from the max health
        float p2percentage = Mathf.Clamp01((float)p2health / maxHealth);
        // Set the width variable to be a float of the health percentage
        p2width = p2percentage * 1000f;
        // set the health bar size to width variable's float number
        p2healthBar.sizeDelta = new Vector2(p2width, p2healthBar.sizeDelta.y);
        // float colorPercentage = percentage * 2.55f;
        p2barColor = new Color(255, p2percentage, p2percentage, 255);
        p2barImage.color = p2barColor;
        p2htxt.text = p2health.ToString();

        p1score.text = gameManager.p1Wins.ToString();
        p2score.text = gameManager.p2Wins.ToString();
        round.text = gameManager.currentRound.ToString();
    }
}
