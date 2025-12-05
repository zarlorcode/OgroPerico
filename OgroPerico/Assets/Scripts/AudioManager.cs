using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Música")]
    [SerializeField] private AudioClip musicaInicio;
    [SerializeField] private AudioClip musicaJuego;
    [SerializeField] private AudioClip musicaGameOver;

    [Header("Efectos")]
    [SerializeField] private AudioClip efectoRecibirDaño;
    [SerializeField] private AudioClip efectoDañar;
    [SerializeField] private AudioClip efectoPalanca;

    [Header("Volumen Global")]
    [Range(0f, 1f)]
    public float volumen = 1f;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Configurar volumen inicial
        musicSource.volume = volumen;
        sfxSource.volume = volumen;
    }

    private void Update()
    {
        // Mantener el volumen actualizado en tiempo real si cambia
        musicSource.volume = volumen;
        sfxSource.volume = volumen;
    }

    // ==================== MÚSICA ====================
    private void ReproducirMusica(AudioClip clip)
    {
        if (clip == null) return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void reproducirMusicaInicio()
    {
        ReproducirMusica(musicaInicio);
    }

    public void reproducirMusicaJuego()
    {
        ReproducirMusica(musicaJuego);
    }

    public void reproducirMusicaGameOver()
    {
        ReproducirMusica(musicaGameOver);
    }

    // ==================== EFECTOS ====================
    private void ReproducirEfecto(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    public void reproducirEfectoRecibirDaño()
    {
        ReproducirEfecto(efectoRecibirDaño);
    }

    public void reproducirEfectoDañar()
    {
        ReproducirEfecto(efectoDañar);
    }

    public void reproducirEfectoPalanca()
    {
        ReproducirEfecto(efectoPalanca);
    }
}
