using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SlotMachineAnimation : MonoBehaviour
{
    private Sequence detectSequence;
    private void Start()
    {
        detectSequence = DOTween.Sequence();
        detectSequence.Pause();
    }
    private static SlotMachineAnimation instance;

    public static SlotMachineAnimation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SlotMachineAnimation>();
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
    }

    //animation for emotionsL positive up; negative down
    public void PlayExcitementAnimation(Image symbolImage)
    {
        //scale version
        //symbolImage.rectTransform.DOPunchScale(new Vector3(0.3f, 0.3f, 0f), 0.3f, 1, 0.5f).SetEase(Ease.InOutQuad);
        //symbolImage.rectTransform.DORotate(new Vector3(0f, 0f, -5f), 0.1f).SetLoops(10, LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(0.3f);
        //symbolImage.rectTransform.DOShakePosition(0.5f, new Vector3(5f, 0f, 0f), 10, 90, false, true).SetEase(Ease.OutElastic).SetDelay(0.3f);

        //position :horizental
        symbolImage.rectTransform.DOPunchPosition(new Vector3(10f, 0f, 0f), 0.5f, 1, 0.5f).SetEase(Ease.InOutQuad);
        symbolImage.rectTransform.DORotate(new Vector3(0f, 0f, -10f), 0.1f).SetLoops(10, LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(0.5f);
        symbolImage.rectTransform.DOShakePosition(0.5f, new Vector3(5f, 0f, 0f), 10, 90, false, true).SetEase(Ease.OutElastic).SetDelay(0.5f);
    }

    public void PlaySurpriseAnimation(Image symbolImage)
    {
        symbolImage.rectTransform.DOPunchPosition(new Vector3(0f, 20f, 0f), 0.5f, 1, 0.5f).SetEase(Ease.OutQuad);
        symbolImage.rectTransform.DORotate(new Vector3(0f, 0f, 30f), 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutElastic).SetDelay(0.5f);
    }

    public void PlayLoveAnimation(Image symbolImage)
    {
        symbolImage.rectTransform.DOJump(symbolImage.rectTransform.position, 20f, 1, 0.5f).SetEase(Ease.OutQuad);
        symbolImage.rectTransform.DOPunchRotation(new Vector3(0f, 0f, 30f), 0.5f, 1, 0.5f).SetEase(Ease.InOutQuad).SetDelay(0.5f);
    }

    public void PlayReliefAnimation(Image symbolImage)
    {
        symbolImage.rectTransform.DOPunchRotation(new Vector3(0f, 0f, 5f), 0.5f, 1, 0.5f).SetEase(Ease.InOutQuad);
        symbolImage.rectTransform.DOShakePosition(0.5f, new Vector3(2f, 0f, 0f), 10, 90, false, true).SetEase(Ease.OutElastic).SetDelay(0.5f);
    }

    public void PlayDisappointmentAnimation(Image symbolImage)
    {
        symbolImage.DOColor(new Color(0.5f, 0.5f, 0.5f, 0f), 0.5f).SetEase(Ease.InOutQuad).SetLoops(2, LoopType.Yoyo);
        symbolImage.rectTransform.DOShakePosition(0.5f, new Vector3(10f, 0f, 0f), 10, 90, false, true).SetEase(Ease.OutElastic).SetDelay(0.5f);
    } 

    public void PlayAngerAnimation(Image symbolImage)
    {
        symbolImage.rectTransform.DOPunchPosition(new Vector3(0f, -5f, 0f), 0.5f, 1, 0.5f).SetEase(Ease.InOutQuad);
        symbolImage.rectTransform.DOPunchRotation(new Vector3(0f, 0f, 3f), 0.3f, 1, 0.5f).SetEase(Ease.InOutQuad).SetDelay(0.5f);
    }

    public void PlayFearAnimation(Image symbolImage)
    {
        symbolImage.rectTransform.DOShakePosition(0.5f, new Vector3(10f, 0f, 0f), 10, 90, false, true).SetEase(Ease.OutElastic);
        symbolImage.rectTransform.DOPunchRotation(new Vector3(0f, 0f, 30f), 0.5f, 1, 0.5f).SetEase(Ease.InOutQuad).SetDelay(0.5f);
    }

    

    public void PlaySadnessAnimation(Image symbolImage)
    {
        symbolImage.DOColor(new Color(0.5f, 0.5f, 0.5f, 0f), 0.5f).SetEase(Ease.InOutQuad).SetLoops(2, LoopType.Yoyo);
        PlayShakeRotation(symbolImage.rectTransform);
    }


    // This function shakes the UI element to create a sense of surprise and anticipation.
    public void PlayShakeRotation(Transform symbolTransform)
    {
        symbolTransform.DORotate(new Vector3(0, 0, 10), 0.1f).SetLoops(10, LoopType.Yoyo);
    }

    // This function flips the UI element to create a sense of surprise and anticipation.
    public void PlayFlipAnimation(Image symbolImage, Image symbolImageChange)
    {
        symbolImage.transform.DOScaleX(0f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            symbolImage.sprite = symbolImageChange.sprite;
            symbolImage.transform.DOScaleX(1f, 0.5f).SetEase(Ease.OutBack);
        });
    }

    //This function fades out the UI element to create a sense of disappearance and transition.
    public void PlayFadeInAnimation(Image symbolImage)
    {
        symbolImage.color = new Color(1f, 1f, 1f, 0f);
        symbolImage.DOFade(1f, 0.5f).SetEase(Ease.Linear);
    }

    //This function fades out the UI element to create a sense of disappearance and transition.
    public void PlayFadeOutAnimation(Image symbolImage)
    {
        symbolImage.DOFade(0f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            symbolImage.gameObject.SetActive(false);
        });
    }
}
