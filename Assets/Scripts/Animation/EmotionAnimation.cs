using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class EmotionAnimation : MonoBehaviour
{
    public Sequence sequence;

    public static EmotionAnimation instance;

    public static EmotionAnimation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EmotionAnimation>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        sequence = DOTween.Sequence();
        sequence.Pause();
    }


    public void StartAnimation()
    {
        sequence.Play();
    }

    public void PauseAnimation()
    {
        sequence.Pause();
    }

    public void RestartAnimation()
    {
        sequence.Restart();
    }

    public Tween AnimateExcitement(RectTransform targetRectTransform)
    {
        // Main effect: Quick scale change
        sequence.Append(targetRectTransform.DOPunchScale(new Vector3(0.2f, -0.2f, 0), 0.4f, 1, 1));

        // Side effect: Bounce up and down
        //sequence.Join(targetRectTransform.DOLocalMoveY(10, 0.4f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo));
        Tween tween = targetRectTransform.DOLocalJump(new Vector3(targetRectTransform.localPosition.x, targetRectTransform.localPosition.y, targetRectTransform.localPosition.z), 10, 2, 0.4f, false);
        //Tween tween = targetRectTransform.DOJumpAnchorPos(new Vector2(0, 10), 10, 2, 0.4f, false);
        sequence.Join(tween);

        sequence.AppendInterval(0.3f);
        ///sequence.SetLoops(1);
        return tween;
    }

    public void AnimateSadness(RectTransform targetRectTransform)
    {
        // Main effect: Slow up and down movement
        sequence.Append(targetRectTransform.DOMoveY(targetRectTransform.position.y - 0.05f, 1f).SetEase(Ease.InOutSine));
        sequence.Append(targetRectTransform.DOMoveY(targetRectTransform.position.y, 1f).SetEase(Ease.InOutSine));

        // Side effect: Gentle rotation
        sequence.Join(targetRectTransform.DORotate(new Vector3(0, 0, 2), 1f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo));

        sequence.AppendInterval(1);
        sequence.SetLoops(-1);
    }

    public void AnimateAnger(RectTransform targetRectTransform)
    {
        // Main effect: Intense shaking
        sequence.Append(targetRectTransform.DOShakeAnchorPos(0.5f, new Vector2(10, 0), 10, 90, true));

        // Side effect: Slight scale change
        sequence.Join(targetRectTransform.DOPunchScale(new Vector3(-0.05f, 0.05f, 0), 0.5f, 1, 0));

        sequence.AppendInterval(1);
        sequence.SetLoops(-1);
    }

    public void AnimateHappiness(RectTransform targetRectTransform)
    {
        // Main effect: Bouncing up and down
        sequence.Append(targetRectTransform.DOJumpAnchorPos(new Vector2(0, 20), 20, 3, 1f, false));

        // Side effect: Slight rotation
        sequence.Join(targetRectTransform.DORotate(new Vector3(0, 0, -5), 0.5f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo));

        sequence.AppendInterval(1);
        sequence.SetLoops(-1);
    }

    public void AnimateFear(RectTransform targetRectTransform)
    {
        // Main effect: Quick scale down and up
        sequence.Append(targetRectTransform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0), 0.4f, 1, 1));

        // Side effect: Slight horizontal movement
        sequence.Join(targetRectTransform.DOShakeAnchorPos(0.4f, new Vector2(5, 0), 10, 90, true));

        sequence.AppendInterval(1);
        sequence.SetLoops(-1);
    }
}

