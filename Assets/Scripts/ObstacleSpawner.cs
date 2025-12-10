using System.Collections;
using UnityEngine;
using static GameManager;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Elements per Spawnejar")]
    public GameObject[] obstaclePrefabs;
    public GameObject[] coinPatternPrefabs; // Els patrons de monedes (Pattern_Line)

    [Header("Posicions (Carrils)")]
    // Definim 4 alçades fixes. Ajusta aquests valors a l'Inspector per quadrar amb la teva pantalla.
    // Exemple: 2.5 (Dalt), 1.0 (Mig-Dalt), -1.0 (Mig-Baix), -2.5 (Baix)
    public float[] yPositions = new float[] { 3f, 0f, -3.4f };

    [Header("Rotacions")]
    // 0 = Horitzontal, 90 = Vertical, 45 = Diagonal /, -45 = Diagonal \
    public float[] possibleRotations = new float[] { 0f, 90f, 45f, -45f };

    [Header("Temps de Spawn (Increasing difficulty)")]
    public float spawnX = 12f;             // Posició X on neixen (fora pantalla)
    public float initialInterval = 2.0f;   // Temps inicial entre obstacles
    public float minInterval = 0.8f;       // Temps mínim (màxima dificultat)
    public float decreaseRate = 0.05f;     // Quant baixa l'interval cada vegada (acceleració)

    [Header("Probabilitat")]
    [Range(0, 100)]
    public int coinChance = 18; // 18% de probabilitat que surtin monedes

    [Header("Posicions Bonus Coins")]
    public float topLaneY = 4.8f;             // Posició X on neixen (fora pantalla)
    public float bottomLaneY = -4f;
    public float bonusCoinOffsetX = -2f;


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
            //True si el random de 0 a 100 surt X(%) o inferior
            bool spawnCoins = Random.Range(0, 100) < coinChance;

            // Si toca monedes i n'hi ha patrons a l'array
            if (spawnCoins && coinPatternPrefabs.Length > 0)
            {
                SpawnCoinPattern();
            }
            // Si no, o si no hi ha patrons de monedes, spawnejem un obstacle normal
            else
            {
                SpawnObstacle();
            }


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

        
        // 2. Triar un Carril (Y) aleatori dels 3 disponibles
        // Això garanteix que mai surti "una mica més amunt", sempre al lloc exacte.
        int laneIndex = Random.Range(0, yPositions.Length);
        float chosenY = yPositions[laneIndex];

        // 3. Triar una Rotació aleatòria de les 4 disponibles
        int rotIndex = Random.Range(0, possibleRotations.Length);
        float chosenRot = possibleRotations[rotIndex];

        // 4. Assignar posició i rotació
        obj.transform.position = new Vector3(spawnX, chosenY, 0);
        obj.transform.rotation = Quaternion.Euler(0, 0, chosenRot);

        // Opcional: Assignar Tag
        obj.tag = "Obstacle";

        //Creem el boolean que ens dirà si l'obstacle està en posició horitzontal (90º 2a posició de l'array)
        bool isHorizontal = (rotIndex == 1);

        // Mirem si l'obstacle al carril de Dalt (0) o al de Baix (2)
        bool isTopLane = (laneIndex == 0);
        bool isBottomLane = (laneIndex == 2);

        // Si l'obstacle és horitzontal I està en un dels extrems...
        if (isHorizontal && (isTopLane || isBottomLane))
        {
            // Posem 15% de probabilitat que, en cas que es compleixin les condicions, spawnejin monedes al forat
            if (Random.Range(0, 100) < 90) 
            {
                if (coinPatternPrefabs.Length > 0)
                {
                    // Triem on van les monedes segons el carril de l'obstacle
                    float bonusY = 0f;

                    if (isTopLane)
                    {
                        // Si l'obstacle és a dalt, monedes al forat del sostre
                        bonusY = topLaneY;
                    }
                    else if (isBottomLane)
                    {
                        // Si l'obstacle és a baix, monedes al forat del terra
                        bonusY = bottomLaneY;
                    }

                    // Instanciem les monedes (sense xOffset, just a sobre/sota)
                    GameObject coinPattern = coinPatternPrefabs[0]; // Agafem el primer patró (línia)

                    Instantiate(coinPattern, new Vector3(spawnX + bonusCoinOffsetX, bonusY, 0), Quaternion.identity);

                    // Debug.Log("Bonus Gap Coins!");
                }
            }
        }
    }

    void SpawnCoinPattern()
    {
        if (coinPatternPrefabs.Length == 0) return;

        // 1. Triar Patró (Línia, Quadrat, V...) tot i que encara tenim un
        GameObject patternPrefab = coinPatternPrefabs[Random.Range(0, coinPatternPrefabs.Length)];

        // 2. Triar Carril (Y)
        // Les monedes també han de sortir als carrils perquè el jugador hi arribi!
        float laneY = yPositions[Random.Range(0, yPositions.Length)];

        // 3. Instanciar sense rotació 
        Instantiate(patternPrefab, new Vector3(spawnX, laneY, 0), Quaternion.identity);
    }
}