using UnityEngine;

public class Chest : MonoBehaviour
{
    // Tipos de cofre disponibles
    // Normal genera un power up
    // Mimic hace daño al jugador
    public enum ChestType { Normal, Mimic }
    public ChestType chestType = ChestType.Normal;

    [Header("References")]
    // Prefab del power up que se instanciara si el cofre es normal
    public GameObject powerUpPrefab;

    // Sprite que aparece encima del cofre cuando el jugador puede interactuar
    // Por ejemplo un icono de pulsar un boton
    [Tooltip("Sprite que aparece encima del cofre para indicar Presiona tecla")]
    public SpriteRenderer pressKeyUI;

    // Animator que controla las animaciones de abrir cofre
    [SerializeField] private Animator animator;

    // Indica si el jugador esta dentro del area de deteccion del cofre
    private bool playerInRange = false;

    // Indica si el cofre ya ha sido abierto
    private bool isOpened = false;

    // Referencia al script del jugador para leer la tecla de interactuar
    private PlayerMovementNewInputSystem playerInputScript;

    // Referencia a la vida del jugador si el cofre es un mimic
    private PlayerHealth playerHealth;

    private void Start()
    {
        // Al iniciar la escena, ocultamos el icono de presionar tecla
        if (pressKeyUI != null)
            pressKeyUI.enabled = false;
    }

    private void Update()
    {
        // Si el jugador no esta cerca o el cofre ya esta abierto, no hacemos nada
        if (!playerInRange || isOpened) return;

        // Aqui comprobamos si el jugador ha pulsado la tecla de interactuar
        // interactInput viene del script PlayerMovementNewInputSystem
        if (playerInputScript != null && playerInputScript.interactInput)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        // Marcamos el cofre como abierto para que no vuelva a abrirse
        isOpened = true;

        // Ocultamos el icono de pulsar tecla
        if (pressKeyUI != null)
            pressKeyUI.enabled = false;

        // Activamos la animacion correcta segun el tipo de cofre
        if (animator != null)
        {
            if (chestType == ChestType.Normal)
                animator.SetTrigger("Chest_Open"); // animacion de cofre normal
            else
                animator.SetTrigger("Mimo_Open"); // animacion de mimic
        }

        // Si es un cofre normal, generamos el power up
        if (chestType == ChestType.Normal)
        {
            SpawnPowerUp();
        }
        // Si es un mimic, le hacemos dano al jugador
        else if (chestType == ChestType.Mimic)
        {
            if (playerHealth != null)
                playerHealth.TakeDamage();
        }

        // Destruimos el cofre despues de 3 segundos para que la animacion pueda reproducirse
        Destroy(gameObject, 3f);
    }

    private void SpawnPowerUp()
    {
        // Si no hay prefab no hacemos nada
        if (powerUpPrefab == null) return;

        // Instanciamos el power up un poco por encima del cofre
        GameObject powerUp = Instantiate(powerUpPrefab,
                                         transform.position + Vector3.up * 0.5f,
                                         Quaternion.identity);

        // Hacemos que el power up salte hacia arriba
        Rigidbody2D rb = powerUp.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            // Si el prefab no tiene rigidbody, se lo anadimos
            rb = powerUp.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;
        }

        // Impulso aleatorio hacia arriba y con un poco de inclinacion horizontal
        float randomX = Random.Range(-0.5f, 0.5f);
        rb.AddForce(new Vector2(randomX, 5f), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detectamos cuando el jugador entra al area del cofre
        if (!collision.CompareTag("Player")) return;

        playerInRange = true;

        // Guardamos referencias al script de movimiento y al de vida del jugador
        playerInputScript = collision.GetComponent<PlayerMovementNewInputSystem>();
        playerHealth = collision.GetComponent<PlayerHealth>();

        // Activamos el icono de pulsar tecla
        if (pressKeyUI != null)
            pressKeyUI.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Detectamos cuando el jugador se aleja del cofre
        if (!collision.CompareTag("Player")) return;

        playerInRange = false;
        playerInputScript = null;
        playerHealth = null;

        // Ocultamos el icono de pulsar tecla
        if (pressKeyUI != null)
            pressKeyUI.enabled = false;
    }
}
