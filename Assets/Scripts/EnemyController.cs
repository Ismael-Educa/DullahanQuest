using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

// Tipos posibles de enemigos
public enum EnemyType
{
    Crab,
    Octopus,
    Jumper
}

public class EnemyController : MonoBehaviour
{
    // Tipo de enemigo asignado desde el inspector
    public EnemyType enemyType;

    // Referencias internas
    private Transform groundDetection;       // Transform que se usa como punto de origen para los rayos
    private SpriteRenderer spriteRenderer;   // Renderer del sprite para poder voltearlo

    // Capa del suelo para detectar colisiones
    private LayerMask layer;

    // Configuracion del movimiento general del enemigo
    [Header("Enemy movement")]
    [SerializeField] private Vector2 direction;         // Direccion de movimiento
    [Range(0.5f, 2f)]
    [SerializeField] private float moveSpeed = 1f;      // Velocidad de movimiento

    // Configuracion exclusiva del enemigo tipo Jumper
    [Header("Jumper move configuration")]
    [Range(0.5f, 3f)]
    [SerializeField] private float sinAmplitude;        // Altura del salto sinusoidal
    [Range(0.5f, 3f)]
    [SerializeField] private float sinFrecuency;        // Frecuencia del movimiento sinusoidal

    private float sinCenterY;                           // Altura base del movimiento sinusoidal

    private void Awake()
    {
        // Segundo hijo del enemigo usado para detectar suelo o paredes
        groundDetection = transform.GetChild(1);

        // Obtener el SpriteRenderer del enemigo
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Localizar la capa Ground para las detecciones con raycast
        layer = LayerMask.GetMask("Ground");
    }

    private void Start()
    {
        // Guardar la posicion Y original para usarla como referencia
        sinCenterY = transform.position.y;
    }

    private void Update()
    {
        // Actualizar comportamiento del enemigo segun su tipo
        EnemyMove();
    }

    // Decide el tipo de movimiento segun el enemigo
    private void EnemyMove()
    {
        switch (enemyType)
        {
            case EnemyType.Crab:
                CrabMove();
                break;
            case EnemyType.Octopus:
                OctopusMove();
                break;
            case EnemyType.Jumper:
                JumperMove();
                break;
        }
    }

    // Movimiento del cangrejo
    private void CrabMove()
    {
        // Si no detecta suelo adelante o detecta pared, cambiar direccion
        if (!GeneralDetection(1.5f, Vector3.down, Color.yellow) || GeneralDetection(0.5f, direction, Color.green))
        {
            direction = -direction;                // Invertir direccion
            spriteRenderer.flipX = !spriteRenderer.flipX;

            // Invertir el punto de deteccion del suelo
            groundDetection.localPosition = new Vector3(
                -groundDetection.localPosition.x,
                groundDetection.localPosition.y,
                groundDetection.localPosition.z
            );
        }

        // Movimiento del cangrejo
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    // Movimiento del pulpo
    private void OctopusMove()
    {
        // Detecta si choca con el techo o suelo
        if (GeneralDetection(1f, direction, Color.magenta))
        {
            direction = -direction;    // Invertir direccion
        }

        // Aplicar movimiento
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    // Movimiento sinusoidal del enemigo Jumper
    private void JumperMove()
    {
        // Si detecta obstaculo adelante, cambiar direccion
        if (GeneralDetection(1f, direction, Color.magenta))
        {
            direction = -direction;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        Vector3 enemyPosition = transform.position;

        // Crear un movimiento sinusoidal en el eje Y
        float sin = Mathf.Sin(enemyPosition.x * sinFrecuency) * sinAmplitude;

        // Aplicar altura base mas el salto sinusoidal
        enemyPosition.y = sinCenterY + sin;

        // Movimiento horizontal
        enemyPosition.x += direction.x * moveSpeed * Time.deltaTime;

        // Actualizar posicion
        transform.position = enemyPosition;
    }

    // Realiza un raycast para detectar suelos, paredes o techos
    private bool GeneralDetection(float rayLength, Vector3 direction, Color color)
    {
        Debug.DrawRay(groundDetection.position, direction * rayLength, color);
        return Physics2D.Raycast(groundDetection.position, direction, rayLength, layer);
    }

    // Si el jugador permanece en contacto con el enemigo, recibe daño
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();
        }
    }
}
