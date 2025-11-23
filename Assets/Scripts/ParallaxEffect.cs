using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    // Velocidad a la que se mueve el fondo en relación con el movimiento de la cámara
    [SerializeField] private float speedBackground;

    // Referencia a la Transform de la cámara principal
    private Transform cameraTransform;

    // Guarda la posición de la cámara en el frame anterior
    private Vector3 lastCameraPosition;

    private void Start()
    {
        // Obtener la cámara principal automáticamente
        cameraTransform = Camera.main.transform;

        // Inicializar la posición anterior con la posición actual de la cámara
        lastCameraPosition = cameraTransform.position;
    }

    // LateUpdate se llama después de todos los Update, ideal para mover fondos
    private void LateUpdate()
    {
        // Calcular cuánto se ha movido la cámara desde el último frame
        Vector3 backgroundMovement = cameraTransform.position - lastCameraPosition;

        // Aplicar el movimiento al fondo, multiplicando por la velocidad deseada
        // El eje Y se mueve completamente, X se multiplica por speedBackground
        transform.position += new Vector3(backgroundMovement.x * speedBackground, backgroundMovement.y, 0);

        // Actualizar la posición anterior para el próximo frame
        lastCameraPosition = cameraTransform.position;
    }
}
