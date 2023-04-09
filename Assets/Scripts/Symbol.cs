using System.Collections.Generic;
using UnityEngine;

public class Symbol : MonoBehaviour
{
    public Sprite sprite;
    public string itemName, description, cardType, objectAddWhenDestroyed, objectAddEveryXTurnsORSpins, objectTurnInto, addItembyChance, transformItem, transformItemAdjacent, addItemAdjacentCondition, addItembyAdjacent;
    public int baseValue, valueDestroy, caculatedValue, ADOBuffValue, spinToDestroy, effectCountDestroy, turnsToAddSTH, spinsToAddSTH, turnsToTurnInto, percentage, ID, addItemChance, transformItemCance, destroyAgricultureChance, price;
    public bool markedDestruction = false;
    public int[] points = new int[2] { -1, -1 };
    public List<string> ADOBuffObject = new List<string>(), ADODestroyObject = new List<string>();

    public Symbol() {}

    public Symbol(Symbol symbol)
    {
        sprite = symbol.sprite;
        itemName = symbol.itemName;
        description = symbol.description;
        baseValue = symbol.baseValue;
        valueDestroy = symbol.valueDestroy;
        markedDestruction = symbol.markedDestruction;
        caculatedValue = symbol.caculatedValue;
        spinToDestroy = symbol.spinToDestroy;
        effectCountDestroy = symbol.effectCountDestroy;
        ADOBuffValue = symbol.ADOBuffValue;
        objectAddWhenDestroyed = symbol.objectAddWhenDestroyed;
        objectAddEveryXTurnsORSpins = symbol.objectAddEveryXTurnsORSpins;
        turnsToAddSTH = symbol.turnsToAddSTH;
        spinsToAddSTH = symbol.spinsToAddSTH;
        objectTurnInto = symbol.objectTurnInto;
        turnsToTurnInto = symbol.turnsToTurnInto;
        percentage = symbol.percentage;
        ID = symbol.ID;
        addItemChance = symbol.addItemChance;
        addItembyChance = symbol.addItembyChance;
        transformItemCance = symbol.transformItemCance;
        transformItem = symbol.transformItem;
        transformItemAdjacent = symbol.transformItemAdjacent;
        destroyAgricultureChance = symbol.destroyAgricultureChance;
        addItemAdjacentCondition = symbol.addItemAdjacentCondition;
        addItembyAdjacent = symbol.addItembyAdjacent;
        price = symbol.price;
        points = new int[2] { -1, -1 };
        ADOBuffObject = new List<string>(symbol.ADOBuffObject);
        ADODestroyObject = new List<string>(symbol.ADODestroyObject);
    }
}

