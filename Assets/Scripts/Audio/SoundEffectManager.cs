using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    public enum Emotion { None, Happy, Sad, Angry, Fear, Surprise }

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

    public void Play(string name, bool useEmotion = true, float speed = 1f)
    {
        SoundEffect sfx = soundEffects.Find(s => s.name == name);
        if (sfx != null)
        {
            sfx.source.pitch = speed;
            sfx.source.Play();

            if (useEmotion)
            {
                PlayEmotionEffect(sfx.emotion, sfx.source);
            }
        }
        else
        {
            Debug.LogWarning("Sound effect: " + name + " not found!");
        }
    }

    private void PlayEmotionEffect(Emotion emotion, AudioSource audioSource)
    {
        // Example: Sound changes using DOTween based on emotion

        switch (emotion)
        {
            case Emotion.Happy:
                // Happy sound: Increase pitch and volume slightly
                DOTween.Sequence()
                    .Append(audioSource.DOPitch(1.1f, 0.5f))
                    .Join(audioSource.DOFade(1.1f, 0.5f));
                break;
            case Emotion.Sad:
                // Sad sound: Decrease pitch and volume slightly
                DOTween.Sequence()
                    .Append(audioSource.DOPitch(0.9f, 0.5f))
                    .Join(audioSource.DOFade(0.9f, 0.5f));
                break;
            case Emotion.Angry:
                // Angry sound: Increase pitch and volume
                DOTween.Sequence()
                    .Append(audioSource.DOPitch(1.2f, 0.5f))
                    .Join(audioSource.DOFade(1.2f, 0.5f));
                break;
            case Emotion.Fear:
                // Fear sound: Decrease pitch, increase volume
                DOTween.Sequence()
                    .Append(audioSource.DOPitch(0.8f, 0.5f))
                    .Join(audioSource.DOFade(1.2f, 0.5f));
                break;
            case Emotion.Surprise:
                // Surprise sound: Rapid pitch and volume fluctuations
                DOTween.Sequence()
                    .Append(audioSource.DOPitch(1.1f, 0.1f))
                    .Join(audioSource.DOFade(1.1f, 0.1f))
                    .Append(audioSource.DOPitch(0.9f, 0.1f))
                    .Join(audioSource.DOFade(0.9f, 0.1f))
                    .SetLoops(4, LoopType.Yoyo);
                break;
            default:
                // No emotion: Do not change sound properties
                break;
        }
    }
}
