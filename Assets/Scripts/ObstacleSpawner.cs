using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] obstaclePrefabs;   // Prefabs d'obstacles
    public float spawnInterval = 2f;       // Temps entre spawns
    public float spawnIntervalVariance = 0.5f; // Aleatori ±
    public float minY = -3.5f;             // Altura mínima
    public float maxY = 3.5f;              // Altura màxima
    public float spawnX = 10f;             // Posició X fora de la pantalla

    void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            SpawnObstacle();

            // Espera interval amb una mica de variació aleatòria
            float interval = spawnInterval + Random.Range(-spawnIntervalVariance, spawnIntervalVariance);
            interval = Mathf.Max(0.1f, interval); // evitar temps negatiu
            yield return new WaitForSeconds(interval);
        }
    }

    void SpawnObstacle()
    {
        // Triar prefab aleatori
        int index = Random.Range(0, obstaclePrefabs.Length);
        GameObject obstacle = Instantiate(obstaclePrefabs[index]);

        // Posició aleatòria en Y dins del rang
        float yPos = Random.Range(minY, maxY);
        obstacle.transform.position = new Vector3(spawnX, yPos, 0);

        // Opcional: assignar Tag “Obstacle” i Collider amb IsTrigger
        obstacle.tag = "Obstacle";

        // Si vols, pots afegir un Rigidbody2D kinemàtic aquí per moure-ho cap a l'esquerra, o fer-ho amb script separat
    }
}
