using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Instancia estática del GameManager para usarlo como Singleton.
    // Permite acceder al GameManager desde cualquier script sin buscar componentes.
    public static GameManager Instance;

    // Almacena los puntos del jugador.
    private int playerPoints;

    // Propiedad pública para leer y modificar los puntos del jugador.
    public int PlayerPoints { get => playerPoints; set => playerPoints = value; }

    private void Awake()
    {
        // Si ya existe un GameManager en la escena, destruir este para evitar duplicados.
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // Si no existe, este objeto será la instancia global.
            Instance = this;

            // Evita que este objeto se destruya al cambiar de escena.
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
