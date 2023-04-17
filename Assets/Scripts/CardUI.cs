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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("mouse hovers card");
        TooltipManager.Instance.ShowTooltip(GetCardDescription(), GetComponent<RectTransform>()); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
