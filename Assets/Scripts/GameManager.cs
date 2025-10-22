using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Speed")]
    public float GameSpeed = 5f;
    public float speedRamp = 0.02f; // quant augmenta per segon

    [Header("Score")]
    public float distanceScore = 0f;
    public int coinScore = 0;
    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (isGameOver) return;

        // Augmentar velocitat amb el temps (progressió)
        GameSpeed += speedRamp * Time.deltaTime;

        // Comptar puntuació per distància (per exemple 10 punts per segon)
        distanceScore += Time.deltaTime * 10f;
        UIManager.Instance.UpdateScore((int)distanceScore, coinScore);
    }

    public void PlayerDied()
    {
        isGameOver = true;
        // Pausar la física opcionalment: Time.timeScale = 0f;
        UIManager.Instance.ShowGameOver((int)distanceScore, coinScore);
    }

    public void AddCoin(int amount = 1)
    {
        coinScore += amount;
        UIManager.Instance.UpdateScore((int)distanceScore, coinScore);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

