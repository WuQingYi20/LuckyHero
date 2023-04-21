using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string cardName;

    private string GetCardDescription()
    {
        return CSVLoad.symbolsDict[cardName].description;
    }

    private string GetCardTitle()
    {
        return CSVLoad.symbolsDict[cardName].itemName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("mouse hovers card");
        TooltipManager.Instance.ShowTooltip(GetCardDescription(), GetCardTitle(), GetComponent<RectTransform>()); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
