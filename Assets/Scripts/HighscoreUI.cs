using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // Necessari per fer servir TextMeshPro

public class HighscoreUI : MonoBehaviour
{
    [Header("Referències UI")]
    public Transform container;      // On posarem les files (el Content del ScrollView)
    public GameObject entryTemplate; // El Prefab de la fila (Nom - Punts)
    public TextMeshProUGUI sortButtonText; // El text del botó per saber com ordenem

    private bool sortByMixed = false; // true = Mitjana (Metres+Monedes), false = Només Metres

    // Aquesta funció s'executa automàticament cada cop que actives el panell
    void OnEnable()
    {
        RefreshHighscores();
    }

    // Funció pel botó d'ordenar
    public void ToggleSortMode()
    {
        sortByMixed = !sortByMixed;
        RefreshHighscores();
    }

    private void RefreshHighscores()
    {
        // 1. NETEJA: Esborrem les files antigues per no duplicar-les
        foreach (Transform child in container)
        {
            // Si el template està dins del container, vigilem de no esborrar-lo!
            if (child.gameObject == entryTemplate) continue;
            Destroy(child.gameObject);
        }

        // 2. CÀRREGA: Recuperem les dades del disc dur
        HighscoreManager.HighscoreList data = HighscoreManager.LoadHighscores();
        List<HighscoreManager.HighscoreEntry> sortedList = data.list;

        // 3. ORDENACIÓ: Matemàtiques per ordenar la llista
        if (sortByMixed)
        {
            // Ordenar de més a menys segons la fórmula: (Metres + Monedes) / 2
            sortedList.Sort((p1, p2) => p2.GetMixedScore().CompareTo(p1.GetMixedScore()));
            if (sortButtonText) sortButtonText.text = "Ordered by: Coins / meters";
        }
        else
        {
            // Ordenar de més a menys només per Metres
            sortedList.Sort((p1, p2) => p2.meters.CompareTo(p1.meters));
            if (sortButtonText) sortButtonText.text = "Ordered by: Distance";
        }

        // 4. DIBUIXAR: Creem una fila per cada rècord
        int rank = 1;
        foreach (var entry in sortedList)
        {
            // Creem la còpia del prefab
            GameObject row = Instantiate(entryTemplate, container);
            row.SetActive(true); // Assegurem que sigui visible

            // Busquem els textos dins de la fila per omplir-los
            // IMPORTANT: L'ordre dels textos al prefab ha de ser: 0:Rank, 1:Nom, 2:Score
            TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>();
            
            if (texts.Length >= 3)
            {
                texts[0].text = "#" + rank.ToString(); // Posició (#1, #2...)
                texts[1].text = entry.name;            // Nom del jugador
                
                if (sortByMixed)
                    texts[2].text = entry.GetMixedScore().ToString("F0") + " ¢/m";
                else
                    texts[2].text = entry.meters.ToString("F0") + " m";
            }
            
            rank++;
        }
    }
}