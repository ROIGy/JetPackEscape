using UnityEngine;
using UnityEngine.SceneManagement; // Per reiniciar escenes
using UnityEngine.UI; // Si vols UI per marcador
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton (només una instància del GameManager)

    [Header("Player")]
    public PlayerController player;

    [Header("UI")]
    public TextMeshProUGUI scoreText;       // Text per mostrar puntuació
    public TextMeshProUGUI coinText;        // Text per mostrar monedes
    public GameObject gameOverPanel; // Panel Game Over
    public TextMeshProUGUI finalScoreText; //Score final

    [Header("Score Settings")]
    public float scorePerSecond = 10f; // Punts per segon de supervivència
    private float score;
    private int coins;

    public static bool isGameOver = false;

    void Start()
    {
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
        // Parar spawners (si n'hi ha)
        //ObstacleSpawner[] spawners = FindObjectsByType<ObstacleSpawner>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        //foreach (var s in spawners) s.StopSpawning();

        // Parar tots els obstacles
        ObstacleMover[] movers = FindObjectsByType<ObstacleMover>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var m in movers) m.StopMoving();

        // Mostrar UI Game Over
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (finalScoreText != null) finalScoreText.text = "Score: " + Mathf.FloorToInt(score) + "\nCoins: " + coins;
    }

    // Cridat per UI button "Replay"
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isGameOver = false;
    }

    // Cridat quan el jugador recull una moneda
    public void AddCoin(int amount = 1)
    {
        coins += amount;
    }
}
