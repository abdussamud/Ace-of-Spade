using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AlphaKnight
{
    public class Helper
    {
        public  void Shuffle<T>(List<T> list)
        {
            System.Random rand = new();
            int n = list.Count;
            while (n > 1)
            {
                int k = rand.Next(n);
                n--;
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }

    public class TransformSet
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformSet(Vector3 position = new(), Quaternion rotation = new(), Vector3 scale = new())
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }
}

[Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    public AudioMixerGroup audioMixerGroup;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

    public bool playOnAwake;
    public bool loop;
}

[SerializeField]
public enum Mode { AI, Online, }
