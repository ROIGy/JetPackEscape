using UnityEngine;
using UnityEngine.SceneManagement; // Necessari per canviar d'escena

public class MainMenuController : MonoBehaviour
{
    [Header("Panells UI")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject highScoresPanel;

    // --- FUNCIONS DELS BOTONS PRINCIPALS ---

    public void PlayGame()
    {
        // Carrega l'escena del joc (assegura't que té index 1 al Build Settings)
        SceneManager.LoadScene(1); 
        // O per nom: SceneManager.LoadScene("GameLevel");
    }

    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenHighScores()
    {
        mainPanel.SetActive(false);
        highScoresPanel.SetActive(true);
        // Aquí més endavant cridarem a "CarregarPuntuacions()"
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    // --- FUNCIONS PER TORNAR ENRERE ---

    public void BackToMenu()
    {
        settingsPanel.SetActive(false);
        highScoresPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}