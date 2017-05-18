using UnityEngine;

public class SoundBoss : MonoBehaviour
{
    public static SoundBoss Instance;
    private AudioSource audioSource;

    public bool playBgMusic = true;

    public AudioClip bgMusic;
    
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            return;
        }

        Instance = this;

        audioSource = GetComponent<AudioSource>();

        if(playBgMusic)
        {
            PlayBgMusic();
        }
    }

    public void PlayBgMusic()
    {
        audioSource.clip = bgMusic;
        audioSource.Play();
    }
}
