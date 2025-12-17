using UnityEngine;

public class MusicSpeedController : MonoBehaviour
{
    AudioSource audioSource;
    float initialPitch = 1f;
    float initialGameSpeed;

    [Header("Settings")]
    [Tooltip("Com més petit, menys accelerarà la música. Prova 0.05 o 0.1")]
    public float pitchIntensity = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialPitch = audioSource.pitch;

        // Agafem la velocitat inicial del GameManager per tenir la referència
        if (GameManager.Instance != null)
        {
            initialGameSpeed = GameManager.Instance.gameSpeed;
        }
        else 
        {
            initialGameSpeed = 5f; // Valor de seguretat
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && audioSource != null)
        {
            // 1. Calculem quants punts ha pujat la velocitat respecte a l'inici
            // Ex: Si anavem a 5 i ara anem a 15, l'increment és 10.
            float speedIncrease = GameManager.Instance.gameSpeed - initialGameSpeed;

            // 2. Apliquem el factor de suavitzat (Intensity)
            // Ex: 10 * 0.05 = 0.5 (Només pugem 0.5 el pitch, no 10!)
            float pitchBoost = speedIncrease * pitchIntensity;

            // 3. Sumem això al pitch original
            float finalPitch = initialPitch + pitchBoost;

            // 4. Li posem un límit (Clamp) perquè no passi de 2.0 (massa agut)
            audioSource.pitch = Mathf.Clamp(finalPitch, initialPitch, 2.0f);
        }
    }
}