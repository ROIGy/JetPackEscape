using System.Collections;
using UnityEngine;
using static GameManager;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] obstaclePrefabs;   // Prefabs d'obstacles
    public float spawnInterval = 120f;       // Temps entre spawns
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
        //El valor isGameOver de GameManager fa que s'executi el spawn o no spawn d'obstacles
        while (!isGameOver)
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

        // Rotació aleatòria i posició aleatòria en Y dins del rang
        float yPos = Random.Range(minY, maxY);
        float randomRotation = Random.Range(-45f, 45f); //Angles aleatoris de rotació dels obstacles quan spawnejen

        obstacle.transform.position = new Vector3(spawnX, yPos, 0);
        obstacle.transform.rotation = Quaternion.Euler(0, 0, randomRotation); //Transformar rotació amb valor aleatori
        

        // Opcional: assignar Tag “Obstacle” i Collider amb IsTrigger
        obstacle.tag = "Obstacle";

        // Si vols, pots afegir un Rigidbody2D kinemàtic aquí per moure-ho cap a l'esquerra, o fer-ho amb script separat
    }
}
