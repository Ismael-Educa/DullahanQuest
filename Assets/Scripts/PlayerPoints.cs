using System;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    // Cantidad de puntos que el jugador obtiene por cada PowerUp
    [Range(100, 500)]
    [SerializeField] private int pointsPerPowerUp = 250;

    // Método llamado automáticamente cuando el collider del jugador choca con otro collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprobar si el objeto colisionado tiene la etiqueta "PowerUp"
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            RegisterPowerUpPoints(); // Registrar los puntos
            Destroy(collision.gameObject); // Destruir el PowerUp al recogerlo
        }
    }

    /// <summary>
    /// Otorga los puntos al jugador tras recoger un PowerUp
    /// </summary>
    private void RegisterPowerUpPoints()
    {
        // Reproducir sonido de PowerUp, si AudioManager existe
        // El operador ?. significa "si AudioManager.Instance no es nulo, llama a PlayPowerUpSound()"
        AudioManager.Instance?.PlayPowerUpSound();

        // Actualizar contador de PowerUps en el nivel
        LevelManager.Instance.CurrentPlayerPowerUps++;
        LevelManager.Instance.RemainingPowerUps--;

        // Sumar puntos al total del jugador en GameManager
        GameManager.Instance.PlayerPoints += pointsPerPowerUp;
    }
}
