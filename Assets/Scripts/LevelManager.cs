using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Instancia estática para usar como Singleton
    public static LevelManager Instance;

    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winLevelPanel;

    [Space]
    [Tooltip("Tiempo total del nivel en segundos")]
    [Range(1f, 300f)]
    [SerializeField] private float levelTime;

    // Tiempo interno que va disminuyendo durante la partida
    private float internalLevelTime;
    public float InternalLevelTime { get => internalLevelTime; set => internalLevelTime = value; }

    // Cantidad total de PowerUps en el nivel
    private int totalLevelPowerUps;

    // PowerUps recogidos por el jugador
    private int currentPlayerPowerUps;
    public int CurrentPlayerPowerUps { get => currentPlayerPowerUps; set => currentPlayerPowerUps = value; }

    // PowerUps que faltan por recoger
    private int remainingPowerUps;
    public int RemainingPowerUps { get => remainingPowerUps; set => remainingPowerUps = value; }

    // Referencia al jugador para acceder a su vida
    PlayerHealth playerHealth;

    // Indica si el nivel ha terminado
    bool endLevel = false;

    private void Awake()
    {
        // Reinicia flag de fin de nivel
        endLevel = false;

        // Asigna esta instancia al Singleton
        Instance = this;

        // Oculta los paneles de Game Over y Victoria al inicio
        gameOverPanel.SetActive(false);
        winLevelPanel.SetActive(false);

        // Asegura que el tiempo corre normalmente
        Time.timeScale = 1;

        // Inicializa el temporizador del nivel
        internalLevelTime = levelTime;
    }

    private void Start()
    {
        // Busca todos los cofres en la escena
        Chest[] allChests = Object.FindObjectsByType<Chest>(FindObjectsSortMode.None);
        totalLevelPowerUps = 0;

        // Cuenta solo los cofres normales
        foreach (Chest chest in allChests)
        {
            if (chest.chestType == Chest.ChestType.Normal)
                totalLevelPowerUps++;
        }

        // Inicializa contadores
        currentPlayerPowerUps = 0;
        remainingPowerUps = totalLevelPowerUps;

        // Obtiene la referencia al jugador y su vida
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        // Si se han recogido todos los PowerUps y aún no se ha terminado el nivel
        if (!endLevel && currentPlayerPowerUps >= totalLevelPowerUps)
        {
            endLevel = true;
            WinLevel();
        }

        // Reduce el tiempo del nivel cada frame
        internalLevelTime -= Time.deltaTime;

        // Si el tiempo llega a cero, activar Game Over
        if (internalLevelTime <= 0.9f)
            GameOver();

        // Calcula cuantos PowerUps faltan
        remainingPowerUps = totalLevelPowerUps - currentPlayerPowerUps;
    }

    public void GameOver()
    {
        // Muestra el panel de Game Over
        gameOverPanel.SetActive(true);

        // Pausa el juego
        Time.timeScale = 0;
    }

    public void WinLevel()
    {
        // Si existe el GameManager, añade puntos por tiempo y salud
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerPoints += (int)internalLevelTime * 100;
            GameManager.Instance.PlayerPoints += playerHealth.Health * 250;
        }

        // Muestra panel de victoria
        winLevelPanel.SetActive(true);

        // Pausa el juego
        Time.timeScale = 0;
    }

    public void MainMenuScene()
    {
        // Guarda highscore y nivel máximo desbloqueado
        CheckHighScore();
        CheckMaxLevel();

        // Reinicia puntos del jugador
        GameManager.Instance.PlayerPoints = 0;

        // Carga el menú principal
        SceneManager.LoadScene(GameConstants.MAINMENU_LEVEL);
    }

    public void ReloadScene()
    {
        // Guarda progreso
        CheckHighScore();
        CheckMaxLevel();

        // Reinicia puntos
        GameManager.Instance.PlayerPoints = 0;

        // Recarga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        // Guarda progreso
        CheckMaxLevel();

        // Número total de niveles disponibles
        int levelCount = SceneManager.sceneCountInBuildSettings - 1;

        // Si no estamos en el último nivel, cargar el siguiente
        if (SceneManager.GetActiveScene().buildIndex < levelCount)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
        {
            // Si era el último nivel, revisar highscore
            CheckHighScore();
            GameManager.Instance.PlayerPoints = 0;
        }
    }

    void CheckMaxLevel()
    {
        // Obtiene el número del nivel desde el nombre "LevelX"
        int currentLevel = int.Parse(SceneManager.GetActiveScene().name.Substring(5));

        // Si no existe el registro de nivel máximo, lo crea
        if (!PlayerPrefs.HasKey(GameConstants.MAXLEVEL_KEY))
            PlayerPrefs.SetInt(GameConstants.MAXLEVEL_KEY, currentLevel + 1);
        else if (PlayerPrefs.GetInt(GameConstants.MAXLEVEL_KEY) < currentLevel + 1)
            PlayerPrefs.SetInt(GameConstants.MAXLEVEL_KEY, currentLevel + 1);
    }

    private void CheckHighScore()
    {
        // Si ya existe highscore, lo compara
        if (PlayerPrefs.HasKey(GameConstants.HIGHSCORE_KEY))
        {
            int highScore = PlayerPrefs.GetInt(GameConstants.HIGHSCORE_KEY);

            if (GameManager.Instance.PlayerPoints > highScore)
                PlayerPrefs.SetInt(GameConstants.HIGHSCORE_KEY, GameManager.Instance.PlayerPoints);
        }
        else
        {
            // Si no existía, lo crea
            PlayerPrefs.SetInt(GameConstants.HIGHSCORE_KEY, GameManager.Instance.PlayerPoints);
        }
    }
}
