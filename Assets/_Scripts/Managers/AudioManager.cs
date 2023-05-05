using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //public static AudioManager Instance;
    public Sound[] sounds;

    private void Awake()
    {
        #region Singleton
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        //
        //DontDestroyOnLoad(gameObject);
        #endregion

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = s.playOnAwake;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        //Play("BGMusic");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sounds name: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}
