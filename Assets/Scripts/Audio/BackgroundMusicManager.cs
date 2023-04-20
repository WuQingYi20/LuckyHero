using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance;

    [System.Serializable]
    public class BackgroundMusic
    {
        public string scenario;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume;
        [Range(.1f, 3f)] public float pitch;
        [HideInInspector] public AudioSource source;
    }

    public List<BackgroundMusic> backgroundMusics;
    private AudioSource currentSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (BackgroundMusic bgm in backgroundMusics)
        {
            bgm.source = gameObject.AddComponent<AudioSource>();
            bgm.source.clip = bgm.clip;
            bgm.source.volume = bgm.volume;
            bgm.source.pitch = bgm.pitch;
            bgm.source.loop = true;
        }
    }

    public void Play(string scenario)
    {
        BackgroundMusic bgm = backgroundMusics.Find(b => b.scenario == scenario);
        if (bgm != null)
        {
            if (currentSource != null)
            {
                currentSource.Stop();
            }
            bgm.source.Play();
            currentSource = bgm.source;
        }
        else
        {
            Debug.LogError("Background music for scenario: " + scenario + " not found!");
        }
    }
}
