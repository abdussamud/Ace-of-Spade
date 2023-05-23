using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;


    private void Awake()
    {
        Instance = this;
        if (sounds != null)
        {
            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.outputAudioMixerGroup = sound.audioMixerGroup;
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.playOnAwake = sound.playOnAwake;
                sound.source.loop = sound.loop;
            }
        }
    }

    public static AudioManager Instance { get; private set; }

    private void MyStart()
    {
        //Play("BGMusic");
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sounds name: " + name + " not found!");
            return;
        }
        sound.source.Play();
    }
}
