using UnityEngine;
using UnityEngine.SceneManagement; // Per reiniciar escenes
using UnityEngine.UI; // Si vols UI per marcador

public class GameManager : MonoBehaviour
{
    // Singleton (només una instància del GameManager)
    public static GameManager Instance;

    [Header("Player")]
    public PlayerController player;

    [Header("UI")]
    public Text scoreText;       // Text per mostrar puntuació
    public Text coinText;        // Text per mostrar monedes
    public GameObject gameOverPanel; // Panel Game Over

    [Header("Score Settings")]
    public float scorePerSecond = 10f; // Punts per segon de supervivència
    private float score;
    private int coins;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        gameOverPanel.SetActive(false);
        score = 0f;
        coins = 0;
    }

    void Update()
    {
        if (isGameOver) return;

        // Incrementar puntuació amb el temps
        score += scorePerSecond * Time.deltaTime;

        // Actualitzar UI
        if (scoreText != null) scoreText.text = "Score: " + Mathf.FloorToInt(score);
        if (coinText != null) coinText.text = "Coins: " + coins;
    }

    // Cridat pel PlayerController quan mor
    public void PlayerDied()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
    }

    // Cridat per UI button "Replay"
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Cridat quan el jugador recull una moneda
    public void AddCoin(int amount = 1)
    {
        coins += amount;
    }
}
