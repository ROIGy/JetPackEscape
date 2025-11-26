using System.Collections;
using UnityEngine;
using static GameManager;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Configuració d'Obstacles")]
    public GameObject[] obstaclePrefabs;

    [Header("Posicions (Carrils)")]
    // Definim 4 alçades fixes. Ajusta aquests valors a l'Inspector per quadrar amb la teva pantalla.
    // Exemple: 2.5 (Dalt), 1.0 (Mig-Dalt), -1.0 (Mig-Baix), -2.5 (Baix)
    public float[] yPositions = new float[] { 3f, 0f, -3.3f};

    [Header("Rotacions")]
    // 0 = Horitzontal, 90 = Vertical, 45 = Diagonal /, -45 = Diagonal \
    public float[] possibleRotations = new float[] { 0f, 90f, 45f, -45f };

    [Header("Temps de Spawn")]
    public float spawnX = 12f;             // Posició X on neixen (fora pantalla)
    public float initialInterval = 2.0f;   // Temps inicial entre obstacles
    public float minInterval = 0.8f;       // Temps mínim (màxima dificultat)
    public float decreaseRate = 0.05f;     // Quant baixa l'interval cada vegada (acceleració)

    private float currentInterval;

    void Start()
    {
        currentInterval = initialInterval;
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        // Petit retard inicial per no començar de cop
        yield return new WaitForSeconds(1f);

        while (!isGameOver)
        {
            SpawnObstacle();

            // Esperem el temps que toqui
            yield return new WaitForSeconds(currentInterval);

            // Reduïm l'interval per fer-ho més difícil (fins al límit)
            if (currentInterval > minInterval)
            {
                currentInterval -= decreaseRate;
            }
        }
    }

    void SpawnObstacle()
    {
        // 1. Instanciar prefab aleatori
        if (obstaclePrefabs.Length == 0) return;
        int prefabIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject obj = Instantiate(obstaclePrefabs[prefabIndex]);

        // 2. Triar un Carril (Y) aleatori dels 4 disponibles
        // Això garanteix que mai surti "una mica més amunt", sempre al lloc exacte.
        int laneIndex = Random.Range(0, yPositions.Length);
        float chosenY = yPositions[laneIndex];

        // 3. Triar una Rotació aleatòria de les 4 disponibles
        int rotIndex = Random.Range(0, possibleRotations.Length);
        float chosenRot = possibleRotations[rotIndex];

        // 4. Assignar posició i rotació
        obj.transform.position = new Vector3(spawnX, chosenY, 0);
        obj.transform.rotation = Quaternion.Euler(0, 0, chosenRot);

        // Opcional: Assignar Tag per seguretat (si no el té el prefab)
        obj.tag = "Obstacle";
    }
}