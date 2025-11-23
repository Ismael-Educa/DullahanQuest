using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida del jugador")]
    [Range(0, 10)]
    [SerializeField] private int health; // Vida actual del jugador

    [Header("Parpadeo al recibir daño")]
    [SerializeField] private float blinkSeconds = 0.2f; // Duración de cada parpadeo
    [SerializeField] private Color blinkColor = Color.red; // Color al parpadear

    private bool canTakeDamage = true; // Evita recibir daño mientras se está parpadeando

    // Componente del sprite del jugador para cambiar el color
    private SpriteRenderer spriteRenderer;

    // Propiedad pública para acceder o modificar la vida
    public int Health { get => health; set => health = value; }

    private void Awake()
    {
        // Obtener el SpriteRenderer del hijo del jugador
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Método que reduce la vida del jugador
    public void TakeDamage()
    {
        if (canTakeDamage)
        {
            // Reducir la vida
            Health--;

            // Reproducir sonido de daño si existe AudioManager
            AudioManager.Instance?.PlayDamageSound();

            // Si la vida llega a 0, llamar a GameOver en LevelManager
            if (Health <= 0)
            {
                LevelManager.Instance.GameOver();
            }

            // Iniciar coroutine para parpadear el sprite
            StartCoroutine(BlinkSprite(4));
        }
    }

    /// <summary>
    /// Parpadea el sprite del jugador un número de veces
    /// </summary>
    /// <param name="blinkTimes">Número de parpadeos</param>
    /// <returns></returns>
    private IEnumerator BlinkSprite(int blinkTimes)
    {
        canTakeDamage = false; // Evitar daño mientras se parpadea

        do
        {
            spriteRenderer.color = blinkColor; // Cambiar a color de daño
            yield return new WaitForSeconds(blinkSeconds);

            spriteRenderer.color = Color.white; // Volver al color original
            yield return new WaitForSeconds(blinkSeconds);

            blinkTimes--;
        } while (blinkTimes > 0);

        canTakeDamage = true; // Permitir recibir daño nuevamente
    }
}
