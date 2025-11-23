using UnityEngine;
using System.Collections;

public class WaitChest : MonoBehaviour
{
    // Referencia al collider del objeto (PowerUp)
    private Collider2D col;

    private void Awake()
    {
        // Obtenemos el collider que está en el mismo GameObject
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        // Cada vez que se activa el objeto, inicia la coroutine para habilitar el collider
        StartCoroutine(EnableColliderAfterDelay());
    }

    private IEnumerator EnableColliderAfterDelay()
    {
        // Espera 1 segundo antes de habilitar el collider
        yield return new WaitForSeconds(1f);

        // Si el collider existe, lo activamos
        if (col != null)
            col.enabled = true;
    }
}
