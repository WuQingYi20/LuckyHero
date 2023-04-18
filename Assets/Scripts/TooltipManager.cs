using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public TextMeshProUGUI tooltipText;
    public RectTransform tooltipRectTransform;
    public Canvas canvas;
    public Vector2 offset;

    private RectTransform cardRectTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowTooltip(string description, RectTransform cardRect)
    {
        cardRectTransform = cardRect;
        tooltipText.gameObject.SetActive(true);
        tooltipText.text = description;
        //Vector2 cardAnchoredPosition = cardRectTransform.anchoredPosition;
        //Vector2 targetAnchoredPosition = cardAnchoredPosition + offset;
        //tooltipRectTransform.anchoredPosition = targetAnchoredPosition;
        UpdateTooltipPosition();
    }

    public void HideTooltip()
    {
        tooltipText.gameObject.SetActive(false);
    }

    //private void Update()
    //{
    //    if (gameObject.activeSelf)
    //    {
    //        UpdateTooltipPosition();
    //    }
    //}

    private void UpdateTooltipPosition()
    {
        Vector2 cardAnchoredPosition = cardRectTransform.anchoredPosition;
        Vector2 targetAnchoredPosition = cardAnchoredPosition + offset;
        tooltipRectTransform.anchoredPosition = targetAnchoredPosition;
    }
}
