using UnityEngine;

public class PlayerAudioControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource audioSource;
    public AudioClip[] soundClips;
    public int currentClipIndex = 0;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (soundClips.Length > 0) 
        { 
            audioSource.clip = soundClips[currentClipIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeClip();
        }
    }

    void ChangeClip() // Changes the audio clip
    {
        if (soundClips.Length == 0)        
            return;

        currentClipIndex = (currentClipIndex + 1) % soundClips.Length;
        audioSource.clip = soundClips[currentClipIndex];
        audioSource.Play();
        
    }
}
