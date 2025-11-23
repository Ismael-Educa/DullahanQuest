using UnityEngine;

public class Cartel : MonoBehaviour
{
    // Referencia al SpriteRenderer que se mostrara encima del cartel cuando el jugador se acerque
    // Este sprite se asigna desde el inspector en Unity
    [Header("Sprite que aparecera encima del cartel")]
    public SpriteRenderer signSprite;

    private void Start()
    {
        // Al iniciar el juego, si el sprite existe, lo ocultamos
        if (signSprite != null)
            signSprite.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cuando un objeto entra en la zona de colision del cartel
        // Comprobamos si ese objeto es el jugador usando su etiqueta Player
        if (collision.CompareTag("Player"))
        {
            // Si el sprite existe, lo activamos para que se vea
            if (signSprite != null)
                signSprite.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Cuando el jugador sale de la zona de colision del cartel
        if (collision.CompareTag("Player"))
        {
            // Ocultamos el sprite otra vez
            if (signSprite != null)
                signSprite.enabled = false;
        }
    }
}
