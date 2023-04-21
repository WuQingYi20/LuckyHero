using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public TextMeshProUGUI tooltipContent;
    public TextMeshProUGUI tooltipTitle;
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

    public void ShowTooltip(string description, string title, RectTransform cardRect)
    {
        cardRectTransform = cardRect;
        tooltipRectTransform.gameObject.SetActive(true);
        tooltipContent.text = description;
        tooltipTitle.text = title;
        //Vector2 cardAnchoredPosition = cardRectTransform.anchoredPosition;
        //Vector2 targetAnchoredPosition = cardAnchoredPosition + offset;
        //tooltipRectTransform.anchoredPosition = targetAnchoredPosition;
        UpdateTooltipPosition();
    }

    public void HideTooltip()
    {
        tooltipRectTransform.gameObject.SetActive(false);
    }

    private void UpdateTooltipPosition()
    {
        Vector2 cardAnchoredPosition = cardRectTransform.anchoredPosition;
        Vector2 targetAnchoredPosition = cardAnchoredPosition + offset;
        tooltipRectTransform.anchoredPosition = targetAnchoredPosition;
    }
}
