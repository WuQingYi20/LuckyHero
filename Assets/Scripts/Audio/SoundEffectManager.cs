using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    public enum Emotion { Happiness, Sadness, Fear, Anger, Surprise, Disgust }

    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip clip;
        public Emotion emotion;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(.1f, 3f)] public float pitch = 1f;
        [HideInInspector] public AudioSource source;
    }

    public GameObject emotionEffect; // Assign a common GameObject for all animations
    public List<SoundEffect> soundEffects;

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

        foreach (SoundEffect sfx in soundEffects)
        {
            sfx.source = gameObject.AddComponent<AudioSource>();
            sfx.source.clip = sfx.clip;
            sfx.source.volume = sfx.volume;
            sfx.source.pitch = sfx.pitch;
        }
    }

    public void Play(string name, float speed = 1f)
    {
        SoundEffect sfx = soundEffects.Find(s => s.name == name);
        if (sfx != null)
        {
            sfx.source.pitch = speed;
            sfx.source.Play();
            PlayEmotionEffect(sfx.emotion);
        }
        else
        {
            Debug.LogWarning("Sound effect: " + name + " not found!");
        }
    }

    private void PlayEmotionEffect(Emotion emotion)
    {
        switch (emotion)
        {
            case Emotion.Happiness:
                emotionEffect.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f).SetLoops(2, LoopType.Yoyo);
                break;
            case Emotion.Sadness:
                emotionEffect.transform.DOLocalMoveY(-0.5f, 1f).SetLoops(2, LoopType.Yoyo);
                break;
            case Emotion.Fear:
                emotionEffect.transform.DOShakePosition(1f, 0.5f, 20, 90, false, true);
                break;
            case Emotion.Anger:
                emotionEffect.transform.DOShakeScale(1f, 0.5f, 20, 90, true);
                break;
            case Emotion.Surprise:
                emotionEffect.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 1f, 5, 0.5f);
                break;
            case Emotion.Disgust:
                emotionEffect.transform.DOShakeRotation(1f, 30, 20, 90, false);
                break;
        }
    }
}
