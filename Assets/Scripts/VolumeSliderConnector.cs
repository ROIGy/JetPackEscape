using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderConnector : MonoBehaviour
{
    private Slider mySlider;

    void Start()
    {
        mySlider = GetComponent<Slider>();

        // 1. Recuperar el volum guardat per posar la boleta al lloc correcte visualment
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        mySlider.value = savedVolume;

        // 2. Subscriure's als canvis
        // Això diu: "Quan el valor canviï, crida a la funció del GameManager"
        mySlider.onValueChanged.AddListener(HandleSliderChange);
    }

    void HandleSliderChange(float value)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetMasterVolume(value);
        }
        else
        {
            // Opcional: Si proves el menú sense el GameManager carregat, 
            // almenys que funcioni el volum encara que no es guardi al GM.
            AudioListener.volume = value;
        }
    }
}