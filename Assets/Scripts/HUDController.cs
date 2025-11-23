using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    // Referencias a los textos del HUD
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI powerUpText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI winPointsText;
    [SerializeField] private TextMeshProUGUI gameOverPointsText;

    // Referencia al sistema de vida del jugador
    private PlayerHealth playerHealth;

    private void Awake()
    {
        // Busca al jugador en la escena y obtiene su componente PlayerHealth
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        // Muestra la vida inicial del jugador
        healthText.text = playerHealth.Health.ToString("00");

        // Muestra la cantidad inicial de PowerUps que faltan en el nivel
        powerUpText.text = LevelManager.Instance.RemainingPowerUps.ToString("00");
    }

    private void Update()
    {
        // Actualiza la vida del jugador en el HUD
        healthText.text = playerHealth.Health.ToString("00");

        // Actualiza los PowerUps restantes
        powerUpText.text = LevelManager.Instance.RemainingPowerUps.ToString("00");

        // Convierte el tiempo total del nivel en minutos y segundos
        int minutes = (int)LevelManager.Instance.InternalLevelTime / 60;
        int seconds = (int)LevelManager.Instance.InternalLevelTime % 60;

        // Actualiza el HUD del tiempo
        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        // Muestra los puntos solo cuando se gana o se pierde
        winPointsText.text = GameManager.Instance?.PlayerPoints.ToString("00000");
        gameOverPointsText.text = GameManager.Instance?.PlayerPoints.ToString("00000");
    }
}
