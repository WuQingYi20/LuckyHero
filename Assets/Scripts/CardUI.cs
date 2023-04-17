using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour
{
    public string cardName;

    private string GetCardDescription()
    {
        return CSVLoad.symbolsDict[cardName].description;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(GetCardDescription(), GetComponent<RectTransform>()); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
