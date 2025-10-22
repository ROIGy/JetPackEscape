using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float spawnX = 12f;
    public float minY = -3f;
    public float maxY = 3f;

    public float initialSpawnInterval = 1.6f;
    float spawnTimer = 0f;
    float spawnInterval;

    void Start()
    {
        spawnInterval = initialSpawnInterval;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnObstacle();
            spawnTimer = 0f;

            // Incrementa la dificultat: reduir interval mínim, però amb un límit
            spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.002f);
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0) return;

        GameObject pref = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        float y = Random.Range(minY, maxY);
        GameObject ob = Instantiate(pref, new Vector3(spawnX, y, 0), Quaternion.identity);
        // Connectar la velocitat del GameManager (si s'usa)
        ObstacleMover mover = ob.GetComponent<ObstacleMover>();
        if (mover != null) mover.SetSpeed(GameManager.Instance.GameSpeed);
    }
}

