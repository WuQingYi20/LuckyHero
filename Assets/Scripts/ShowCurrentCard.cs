using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ShowCurrentCard : MonoBehaviour
{
    private SlotMachine slotMachine;
    public GameObject cardPrefab;
    public Transform contentPanel;

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
            if (symbolNameinTotalDict.ContainsKey(symbolinTotal.name))
            {
                symbolNameinTotalDict[symbolinTotal.name] += 1;
            }
            else
            {
                symbolNameinTotalDict[symbolinTotal.name] = 1;
            }
        }
        return symbolNameinTotalDict;
    }

    void LoadCards()
    {
        foreach (var cardName in GetCurrentCardstoShow())
        {
            for(int count = 0; count < cardName.Value;  count++){
                GameObject newCard = Instantiate(cardPrefab) as GameObject;
                newCard.GetComponent<Image>().sprite = Resources.Load<Sprite>(cardName.Key);
                newCard.transform.SetParent(contentPanel, false);
            } 
        }
    }
}
