using UnityEngine;
using UnityEngine.UI;

public class AnimationTest : MonoBehaviour
{
    private SlotMachineAnimation slotMachineAnimation;

    public Button excitementButton;
    public Button surpiseButton;
    public Button loveButton;
    public Button reliefButton;
    public Button disappointmentButton;
    public Button angerButton;
    public Button fearButton;
    public Button sadnessButton;

    public Button flipButton;
    public Button fadeInButton;
    public Button fadeoutButton;
    public Image symbolImage;
    public Image symbolChange;

    void Start()
    {
        // Get the instance of the SlotMachineAnimation class
        slotMachineAnimation = SlotMachineAnimation.Instance;

        //emotion buttons
        excitementButton.onClick.AddListener(PlayExcitementAnimation);
        surpiseButton.onClick.AddListener(PlaySurpriseAnimation);
        loveButton.onClick.AddListener(PlayLoveAnimation);
        reliefButton.onClick.AddListener(PlayReliefAnimation);
        disappointmentButton.onClick.AddListener(PlayDisappointmentAnimation);
        angerButton.onClick.AddListener(PlayAngerAnimation);
        fearButton.onClick.AddListener(PlayFearAnimation);
        sadnessButton.onClick.AddListener(PlaySadnessAnimation);

        // Add click listeners to the buttons;
        flipButton.onClick.AddListener(PlayFlipAnimation);
        fadeInButton.onClick.AddListener(PlayFadeInAnimation);
        fadeoutButton.onClick.AddListener(PlayFadeOutAnimation);
    }

    //play excitement fun from SlotMachineAnimation
    void PlayExcitementAnimation()
    {
        // Play the excitement animation
        slotMachineAnimation.PlayExcitementAnimation(symbolImage);
    }

    //surprise fun from SlotMachineAnimation
    void PlaySurpriseAnimation()
    {
        // Play the surprise animation
        slotMachineAnimation.PlaySurpriseAnimation(symbolImage);
    }

    //love fun from SlotMachineAnimation
    void PlayLoveAnimation()
    {
        // Play the love animation
        slotMachineAnimation.PlayLoveAnimation(symbolImage);
    }

    //relief fun from SlotMachineAnimation
    void PlayReliefAnimation()
    {
        // Play the relief animation
        slotMachineAnimation.PlayReliefAnimation(symbolImage);
    }

    //disappointment fun from SlotMachineAnimation
    void PlayDisappointmentAnimation()
    {
        // Play the disappointment animation
        slotMachineAnimation.PlayDisappointmentAnimation(symbolImage);
    }

    //anger fun from SlotMachineAnimation
    void PlayAngerAnimation()
    {
        // Play the anger animation
        slotMachineAnimation.PlayAngerAnimation(symbolImage);
    }

    //fear fun from SlotMachineAnimation
    void PlayFearAnimation()
    {
        // Play the fear animation
        slotMachineAnimation.PlayFearAnimation(symbolImage);
    }

    //sadness
    void PlaySadnessAnimation()
    {
        // Play the sadness animation
        slotMachineAnimation.PlaySadnessAnimation(symbolImage);
    }


    void PlayFlipAnimation()
    {
        // Play the flip animation
        slotMachineAnimation.PlayFlipAnimation(symbolImage, symbolChange);
    }

    void PlayFadeInAnimation()
    {
        // Play the fade in animation
        slotMachineAnimation.PlayFadeInAnimation(symbolImage);
    }

    void PlayFadeOutAnimation()
    {
        // Play the fade out animation
        slotMachineAnimation.PlayFadeOutAnimation(symbolImage);
    }


}
