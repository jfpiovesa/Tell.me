using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager managerAudio;


    [Header("Audio Manager")]
    public AudioClip[] audioList;
    public AudioClip[] audioListTemas;
    public AudioSource audioSource; 
    public AudioSource audioSourceTema;

    private void Awake()
    {
        if (managerAudio == null)
        {
            managerAudio = this;
        }
        else if (managerAudio != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSourceTema = GetComponent<AudioSource>();
        PlayAudioBlackground(0);
    }

   public void PlayAudio(int audio)
    {
        audioSource.PlayOneShot(audioList[audio]);
    }
    public void PlayAudioBlackground(int audio)
    {
        audioSourceTema.clip = audioListTemas[audio];
        audioSourceTema.Play();
    }
}
