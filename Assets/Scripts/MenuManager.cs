using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Referencia al slider que controla el volumen general
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        // Verifica si es la primera vez que se inicia la aplicación
        if (!PlayerPrefs.HasKey("AppStarted"))
        {
            // Reinicia el High Score solo la primera vez
            PlayerPrefs.SetInt(GameConstants.HIGHSCORE_KEY, 0);

            // Marca que la aplicación ya se inició
            PlayerPrefs.SetInt("AppStarted", 1);
        }

        // --- Configuración de volumen ---
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = savedVolume;

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        // Guardar primer nivel desbloqueado si no existe
        if (!PlayerPrefs.HasKey(GameConstants.MAXLEVEL_KEY))
        {
            PlayerPrefs.SetInt(GameConstants.MAXLEVEL_KEY, 1);
        }
    }

    // Método que se llama cuando el slider cambia de valor
    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;           // Cambia el volumen global
        PlayerPrefs.SetFloat("MasterVolume", value); // Guarda el valor en PlayerPrefs
    }

    // Carga la siguiente escena en el índice del build
    public void SelectLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Cierra el juego
    public void ExitGame()
    {
        Application.Quit();
    }
}
