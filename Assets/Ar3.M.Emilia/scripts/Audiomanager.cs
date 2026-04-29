using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayGemSound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // Nueva función para silenciar/activar
    public void SetMute(bool isMuted)
    {
        AudioListener.pause = isMuted; // Pausa todo el audio del juego
    }
}