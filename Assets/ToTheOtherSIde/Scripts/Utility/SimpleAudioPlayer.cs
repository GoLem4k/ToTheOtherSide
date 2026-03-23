using UnityEngine;

public class SimpleAudioPlayer : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip myAudio;
    private AudioSource audioSource;
    private bool isPlaying = false;
    
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public InteractionResult Interact(InteractionContext context)
    {
        PlayMySound();
        return default;
    }

    public void PlayMySound()
    {
        if (isPlaying)
        {
            Debug.Log("Звук уже играет!");
            return;
        }
        
        audioSource.PlayOneShot(myAudio);
        isPlaying = true;
        Invoke(nameof(ResetPlayingFlag), myAudio.length);
    }
    
    private void ResetPlayingFlag()
    {
        isPlaying = false;
    }
}