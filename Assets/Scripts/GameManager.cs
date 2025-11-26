using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // O UnityEngine.UI si fas servir Text normal

public class GameManager : MonoBehaviour
{
    // --- SINGLETON (Per accedir-hi des de qualsevol lloc) ---
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;//pAN
        else Destroy(gameObject);
    }
    // -------------------------------------------------------

    [Header("Configuració de Velocitat Global")]
    public float gameSpeed = 5f;          // Velocitat inicial (metres/segon)
    public float maxGameSpeed = 15f;      // Velocitat màxima
    public float speedIncreaseRate = 0.1f; // Quant augmenta la velocitat cada segon

    [Header("Configuració de Puntuació")]
    // Si poses això a 1, el score són metres reals.
    // Si vols que pugi més lent, posa 0.5. Si vols més ràpid, posa 2.
    public float scoreMultiplier = 1f; 

    [Header("Referències UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    // Estat del joc
    private float score;
    private int coins;
    public static bool isGameOver = false;

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        score = 0f;
        coins = 0;
        isGameOver = false;
        
        // Assegurem que el joc no estigui pausat al reiniciar
        Time.timeScale = 1f; 
    }

    void Update()
    {
        if (isGameOver) return;

        // 1. AUGMENTAR LA VELOCITAT DEL JOC PROGRESSIVAMENT
        if (gameSpeed < maxGameSpeed)
        {
            gameSpeed += speedIncreaseRate * Time.deltaTime;
        }

        // 2. AUGMENTAR EL SCORE BASAT EN LA VELOCITAT
        // Distància = Velocitat * Temps. Això és físicament correcte.
        score += gameSpeed * scoreMultiplier * Time.deltaTime;

        // Actualitzar UI
        if (scoreText != null) scoreText.text = $"{Mathf.FloorToInt(score)} M" ;
        if (coinText != null) coinText.text = coins  + " ₵";
    }

    public void PlayerDied()
    {
        isGameOver = true;

        // Opcional: Si vols efecte "càmera lenta" al morir
        // Time.timeScale = 0.5f; 

        // Aturar obstacles visualment ja no cal fer-ho un per un si el mover llegeix isGameOver,
        // però mantenim la lògica de UI.
        
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (finalScoreText != null) 
            finalScoreText.text = "SCORE: " + Mathf.FloorToInt(score) + "M\nCOINS : " + coins;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddCoin(int amount = 1)
    {
        coins += amount;
        if (coinText != null) coinText.text = "Coins: " + coins;
    }
}