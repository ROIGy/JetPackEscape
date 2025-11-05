using System.Collections;
using UnityEngine;
using static GameManager; // Ja ho tenies

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;

    [Header("Posició de Spawn")]
    public float minY = -3.5f;
    public float maxY = 3.5f;
    public float spawnX = 10f;
    public float spawnIntervalVariance = 0.2f; // He baixat la variància per a més control

    [Header("Dificultat Dinàmica")]
    // NOU: L'interval quan comença el joc (ex: 3 segons)
    public float initialSpawnInterval = 3.0f;
    
    // NOU: L'interval mínim al que arribarà (ex: 0.8 segons)
    public float minSpawnInterval = 0.8f;
    
    // NOU: Quants segons triga a passar de 'initial' a 'min' (ex: 60 segons)
    public float timeToReachMaxDifficulty = 60f;

    // NOU: Registrarem quan ha començat el joc
    private float startTime;
    
    void Start()
    {
        // NOU: Guardem el moment exacte en què comença el joc
        startTime = Time.time; 
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (!isGameOver)
        {
            // NOU: Ja no cridem a SpawnObstacle() aquí directament,
            // ho fem després de calcular el temps.

            // --- CÀLCUL DE DIFICULTAT ---

            // 1. Calculem el temps que ha passat des de l'inici
            float elapsedTime = Time.time - startTime;

            // 2. Calculem el percentatge de dificultat (un valor de 0.0 a 1.0)
            // Si han passat 30s de 60s, 'difficultyPercent' serà 0.5
            float difficultyPercent = Mathf.Clamp01(elapsedTime / timeToReachMaxDifficulty);

            // 3. Calculem l'interval actual
            // Mathf.Lerp interpola entre A i B, basat en el percentatge C.
            // Si el percentatge és 0.0, retorna initialSpawnInterval (3.0)
            // Si el percentatge és 1.0, retorna minSpawnInterval (0.8)
            // Si el percentatge és 0.5, retorna el punt mig (aprox 1.9)
            float currentInterval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, difficultyPercent);

            // 4. Afegim la variància
            float interval = currentInterval + Random.Range(-spawnIntervalVariance, spawnIntervalVariance);
            // Assegurem que mai sigui menys de 0.1s (per seguretat)
            interval = Mathf.Max(0.1f, interval);

            // --- FI DEL CÀLCUL ---

            // LÍNIA DE COMPROVACIÓ:
            Debug.Log("Dificultat: " + (difficultyPercent * 100).ToString("F0") + "% | Pròxim spawn en: " + interval.ToString("F2") + "s");

            // 5. Ara SÍ, esperem el temps calculat
            yield return new WaitForSeconds(interval);

            // 6. I (després d'esperar) fem spawn de l'obstacle
            // Això evita que el primer obstacle surti al segon 0.
            if (!isGameOver) // Comprovem de nou per si el jugador ha mort durant l'espera
            {
                SpawnObstacle();
            }
        }
    }

    void SpawnObstacle()
    {
        int index = Random.Range(0, obstaclePrefabs.Length);
        GameObject obstacle = Instantiate(obstaclePrefabs[index]);

        float yPos = Random.Range(minY, maxY);
        float randomRotation = Random.Range(-45f, 45f);

        obstacle.transform.position = new Vector3(spawnX, yPos, 0);
        obstacle.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        obstacle.tag = "Obstacle";
    }
}