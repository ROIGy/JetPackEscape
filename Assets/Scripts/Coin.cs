using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // Quantes monedes dóna
    
    // Pots afegir un so de "pickup" aquí si vols
    // public AudioClip pickupSound; 

    private GameManager gameManager;

    void Start()
    {
        // La forma més senzilla de trobar el GameManager
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Primer, assegura't que el teu jugador tingui el Tag "Player"
        if (other.CompareTag("Player"))
        {
            // 1. Cridem al GameManager (que ja tens fet)
            if (gameManager != null)
            {
                gameManager.AddCoin(coinValue);
            }

            // 2. (Opcional) Reproduir so
            // if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // 3. Destruïm la moneda
            Destroy(gameObject);
        }
    }
}