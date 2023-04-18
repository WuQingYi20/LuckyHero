using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(.1f, 3f)] public float pitch = 1f;
        [HideInInspector] public AudioSource source;
    }

    public List<SoundEffect> soundEffects;

    private void Awake()
    {
        foreach (SoundEffect sfx in soundEffects)
        {
            sfx.source = gameObject.AddComponent<AudioSource>();
            sfx.source.clip = sfx.clip;
            sfx.source.volume = sfx.volume;
            sfx.source.pitch = sfx.pitch;
        }
    }

    public void Play(string name)
    {
        SoundEffect sfx = soundEffects.Find(s => s.name == name);
        if (sfx != null)
        {
            sfx.source.Play();
        }
        else
        {
            Debug.LogWarning("Sound effect: " + name + " not found!");
        }
    }
}
