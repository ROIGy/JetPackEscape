using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq; // Necessari per ordenar llistes fàcilment

public class HighscoreManager : MonoBehaviour
{
    // --- ESTRUCTURA DE DADES ---
    [System.Serializable]
    public class HighscoreEntry
    {
        public string name;
        public float meters;
        public int coins;

        // Funció auxiliar per calcular la puntuació mixta (Mètode A)
        public float GetMixedScore()
        {
            return (meters + coins) / 2f; 
        }
    }

    [System.Serializable]
    public class HighscoreList
    {
        public List<HighscoreEntry> list = new List<HighscoreEntry>();
    }

    // --- GUARDAT I CÀRREGA ---
    
    // Afegim una nova entrada i guardem
    public static void AddHighscore(string name, float meters, int coins)
    {
        // 1. Carreguem la llista existent
        HighscoreList data = LoadHighscores();

        // 2. Creem la nova entrada
        HighscoreEntry newEntry = new HighscoreEntry
        {
            name = name,
            meters = meters,
            coins = coins
        };

        // 3. L'afegim a la llista
        data.list.Add(newEntry);

        // 4. Guardem de nou
        SaveHighscores(data);
    }

    public static HighscoreList LoadHighscores()
    {
        // Fem servir PlayerPrefs per simplicitat (guarda un string JSON gegant)
        string json = PlayerPrefs.GetString("HighscoreTable", "{}");
        
        HighscoreList data = JsonUtility.FromJson<HighscoreList>(json);
        if (data == null) data = new HighscoreList(); // Si és buit, en creem un de nou
        
        return data;
    }

    private static void SaveHighscores(HighscoreList data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("HighscoreTable", json);
        PlayerPrefs.Save();
    }
}