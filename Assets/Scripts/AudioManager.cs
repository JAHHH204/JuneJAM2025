using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("AudioSource")]
   public AudioSource audioSource;

     public AudioClip destroySFX;
     public AudioClip createSFX;
     public AudioClip grabSFX;
     public AudioClip jumpSFX;
    public AudioClip collectSFX;


    private void Awake()
    {

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;

        audioSource.PlayOneShot(clip);
    }


}
