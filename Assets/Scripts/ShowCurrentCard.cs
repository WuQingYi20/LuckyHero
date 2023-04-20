using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowCurrentCard : MonoBehaviour
{
    private SlotMachine slotMachine;
    public GameObject cardPrefab;
    public Transform contentPanel;
    public GameObject ShowCardPanel;

    // Start is called before the first frame update
    void Start()
    {
        slotMachine = SlotMachine.Instance;
    }

    private Dictionary<string, int> GetCurrentCardstoShow()
    {
        var symbolNameinTotalDict = new Dictionary<string, int>();
        foreach(var symbolinTotal in slotMachine.SymbolsListPlayerTotal)
        {
            if (symbolNameinTotalDict.ContainsKey(symbolinTotal.itemName))
            {
                symbolNameinTotalDict[symbolinTotal.itemName] += 1;
            }
            else
            {
                symbolNameinTotalDict[symbolinTotal.itemName] = 1;
            }
        }
        return symbolNameinTotalDict;
    }

    void LoadCards()
    {
        foreach (var cardName in GetCurrentCardstoShow())
        {
            GameObject newCard = Instantiate(cardPrefab) as GameObject;
            newCard.GetComponent<Image>().sprite = Resources.Load<Sprite>(cardName.Key);
            newCard.GetComponent<CardUI>().cardName = cardName.Key;
            newCard.GetComponentInChildren<TextMeshProUGUI>().text = "* " + cardName.Value;
            //还要处理数量
            newCard.transform.SetParent(contentPanel, false);
        }
    }

    //toggle current cards UI panel
    public void ToggleCurrentCards()
    {
        if (ShowCardPanel.activeSelf)
        {
            DeleteCards();
            ShowCardPanel.SetActive(false);
        }
        else
        {
            ShowCardPanel.SetActive(true);
            LoadCards();
        }
    }

    private void DeleteCards()
    {
        for (int i = contentPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }
    }
}
