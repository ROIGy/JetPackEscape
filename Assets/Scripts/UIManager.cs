using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text scoreText;
    public Text coinText;
    public GameObject gameOverPanel;
    public Text finalScoreText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateScore(int distance, int coins)
    {
        if (scoreText) scoreText.text = "Score: " + distance.ToString();
        if (coinText) coinText.text = "Coins: " + coins.ToString();
    }

    public void ShowGameOver(int distance, int coins)
    {
        if (gameOverPanel) gameOverPanel.SetActive(true);
        if (finalScoreText) finalScoreText.text = "Score: " + distance + "\nCoins: " + coins;
    }
}

