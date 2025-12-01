using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // Quantes monedes dóna
    public AudioClip collectSound; // Opcional per més endavant
    // Pots afegir un so de "pickup" aquí si vols
    // public AudioClip pickupSound; 

    //private GameManager gameManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Primer, assegura't que el teu jugador tingui el Tag "Player"
        if (other.CompareTag("Player"))
        {
            // 1. Cridem al GameManager (que ja tens fet)
            GameManager.Instance.AddCoin(coinValue);
            // 2. Mirarem de reudir so més endavant
            // if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // 3. Destruïm la moneda
            Destroy(gameObject);
        }
    }
}