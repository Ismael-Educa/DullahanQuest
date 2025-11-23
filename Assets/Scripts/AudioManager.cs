using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Instancia estática del AudioManager para usarlo como Singleton
    public static AudioManager Instance;

    // Referencias a los sonidos que se reproducirán desde el inspector
    [SerializeField] private AudioSource powerUpAudio;
    [SerializeField] private AudioSource damageAudio;

    private void Awake()
    {
        // Si ya existe un AudioManager en la escena, destruir este para evitar duplicados
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // Si no existe, este será el AudioManager global
            Instance = this;

            // No destruir este objeto al cambiar de escena (permite mantener la música o sonidos globales)
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Reproduce el sonido de recibir daño
    public void PlayDamageSound()
    {
        damageAudio.Play();
    }

    // Reproduce el sonido al recoger un PowerUp
    public void PlayPowerUpSound()
    {
        powerUpAudio.Play();
    }
}
