using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Instancia única del AudioManager.
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        // Evitamos que existan dos AudioManager en la escena.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Reproduce el sonido configurado en el AudioSource de efectos.
    /// </summary>
    public void PlayPickupSound()
    {
        sfxSource.PlayOneShot(sfxSource.clip);
    }
}