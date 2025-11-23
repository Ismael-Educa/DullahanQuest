using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    // Texto donde se mostrará el High Score guardado
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Awake()
    {
        // Si existe un High Score guardado en PlayerPrefs, mostrarlo en pantalla
        if (PlayerPrefs.HasKey(GameConstants.HIGHSCORE_KEY))
            highScoreText.text = "High Score :" + PlayerPrefs.GetInt(GameConstants.HIGHSCORE_KEY).ToString("00000");
    }

    private void Start()
    {
        // Obtiene el objeto llamado ButtonContainer que contiene todos los botones de niveles
        GameObject buttonContainer = GameObject.Find("ButtonContainer");

        // Recorre todos los hijos del contenedor, cada uno es un botón de nivel
        for (int i = 0; i < buttonContainer.transform.childCount; i++)
        {
            int index = i;

            // Si el numero del nivel es menor o igual al maximo nivel desbloqueado por el jugador
            if (index + 1 <= PlayerPrefs.GetInt(GameConstants.MAXLEVEL_KEY))
            {
                // Activa el botón
                buttonContainer.transform.GetChild(index).GetComponent<Button>().interactable = true;

                // Le agrega un listener para cargar el nivel correspondiente al pulsarlo
                buttonContainer.transform.GetChild(index).GetComponent<Button>().onClick.AddListener(() => LoadLevelSelection(index + 1));
            }
            else
            {
                // Si el nivel aún no está desbloqueado, desactiva el botón
                buttonContainer.transform.GetChild(index).GetComponent<Button>().interactable = false;
            }
        }
    }

    // Este método carga la escena del nivel en base a su número
    // Por ejemplo, si index es 3, se carga la escena Level3
    void LoadLevelSelection(int index)
    {
        string levelName = "Level" + index;
        SceneManager.LoadScene(levelName);
    }
}
