using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    private AudioSource bgSource;
    private AudioSource efxSource;

    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }

    }

    void Awake()
    {
        _instance = this;
        bgSource = GetComponents<AudioSource>()[0];
        efxSource =GetComponents<AudioSource>()[1];
    }

    public void RandomPlay(params AudioClip[] clips )
    {
        float pitch = Random.Range(minPitch, maxPitch);
        int index = Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        efxSource.clip = clip;
        efxSource.pitch = pitch;
        efxSource.Play();

    }

    public void StopBgAudio()
    {
        bgSource.Stop();
    }
	
}
