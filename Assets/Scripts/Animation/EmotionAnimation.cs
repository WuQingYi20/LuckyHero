using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class EmotionAnimation : MonoBehaviour
{
    public Sequence sequenceCollection;

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

        sequenceCollection = DOTween.Sequence();
        sequenceCollection.Pause();
    }


    public void StartAnimation()
    {
        sequenceCollection.Play();
    }

    public void PauseAnimation()
    {
        sequenceCollection.Pause();
    }

    public void RestartAnimation()
    {
        sequenceCollection.Restart();
    }

    public Sequence AnimateExcitement(RectTransform targetRectTransform)
    {
        Sequence groupSequence = DOTween.Sequence();
        // Main effect: Quick scale change
        groupSequence.Append(targetRectTransform.DOPunchScale(new Vector3(0.2f, -0.2f, 0), 0.4f, 1, 1));

        // Side effect: Bounce up and down
        //sequenceCollection.Join(targetRectTransform.DOLocalMoveY(10, 0.4f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo));
        Tween tween = targetRectTransform.DOLocalJump(new Vector3(targetRectTransform.localPosition.x, targetRectTransform.localPosition.y, targetRectTransform.localPosition.z), 10, 2, 0.4f, false);
        //Tween tween = targetRectTransform.DOJumpAnchorPos(new Vector2(0, 10), 10, 2, 0.4f, false);
        groupSequence.Join(tween);

        groupSequence.AppendInterval(0.3f);
        ///sequenceCollection.SetLoops(1);
        return groupSequence;
    }

    public Sequence AnimateSadness(RectTransform targetRectTransform)
    {
        Sequence groupSequence = DOTween.Sequence();
        // Main effect: Slow up and down movement
        groupSequence.Append(targetRectTransform.DOLocalMoveY(targetRectTransform.localPosition.y - 5, 1f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo));

        // Side effect: Gentle rotation
        groupSequence.Join(targetRectTransform.DORotate(new Vector3(0, 0, 2), 1f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo));

        return groupSequence;
    }

    public Sequence AnimateAnger(RectTransform targetRectTransform)
    {
        Sequence groupSequence = DOTween.Sequence();
        // Main effect: Intense shaking
        sequenceCollection.Append(targetRectTransform.DOShakeAnchorPos(0.5f, new Vector2(10, 0), 10, 90, true));

        // Side effect: Slight scale change
        sequenceCollection.Join(targetRectTransform.DOPunchScale(new Vector3(-0.05f, 0.05f, 0), 0.5f, 1, 0));

        return groupSequence;
    }

    public Sequence AnimateHappiness(RectTransform targetRectTransform)
    {
        Sequence groupSequence = DOTween.Sequence();
        // Main effect: Bouncing up and down (local jump)
        sequenceCollection.Append(targetRectTransform.DOLocalJump(new Vector3(targetRectTransform.localPosition.x, targetRectTransform.localPosition.y, targetRectTransform.localPosition.z), 20, 3, 1f, false));

        // Side effect: Slight rotation
        sequenceCollection.Join(targetRectTransform.DORotate(new Vector3(0, 0, -5), 0.5f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo));

        return groupSequence;
    }

    public Sequence AnimateFear(RectTransform targetRectTransform)
    {
        Sequence groupSequence = DOTween.Sequence();
        // Main effect: Quick scale down and up (punch effect)
        sequenceCollection.Append(targetRectTransform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0), 0.4f, 1, 1));

        // Side effect: Slight horizontal movement
        sequenceCollection.Join(targetRectTransform.DOShakeAnchorPos(0.4f, new Vector2(5, 0), 10, 90, true));

        return groupSequence;
    }

    public Sequence AnimateSurprise(RectTransform targetRectTransform)
    {
        Sequence groupSequence = DOTween.Sequence();
        // Main effect: Quick scaling
        groupSequence.Append(targetRectTransform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.4f, 1, 1));

        // Side effect: Slight vertical movement
        groupSequence.Join(targetRectTransform.DOShakeAnchorPos(0.4f, new Vector2(0, 5), 10, 90, true));

        return groupSequence;
    }

    public Sequence AnimateAnticipation(RectTransform targetRectTransform)
    {
        Sequence groupSequence = DOTween.Sequence();
        // Main effect: Slow scaling
        groupSequence.Append(targetRectTransform.DOScale(new Vector3(1.1f, 1.1f, 1), 0.7f).SetEase(Ease.InOutSine));
        groupSequence.Append(targetRectTransform.DOScale(new Vector3(1, 1, 1), 0.7f).SetEase(Ease.InOutSine));

        // Side effect: Gentle rotation
        groupSequence.Join(targetRectTransform.DORotate(new Vector3(0, 0, 3), 0.7f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo));

        return groupSequence;
    }

    public Sequence AnimateConfusion(RectTransform targetRectTransform)
    {
        Sequence groupSequence = DOTween.Sequence();
        // Main effect: Random movement
        groupSequence.Append(targetRectTransform.DOShakeAnchorPos(0.5f, 5, 10, 90, false, true));

        // Side effect: Random rotation
        groupSequence.Join(targetRectTransform.DOShakeRotation(0.5f, 5, 10, 90, false));


        return groupSequence;
    }
}

