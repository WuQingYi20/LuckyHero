using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Symbol : MonoBehaviour, IEquatable<Symbol>
{
    private static int sequence = 0;
    public Sprite sprite;
    public string itemName, description, cardType, objectAddWhenDestroyed, objectAddEveryXTurnsORSpins, objectTurnInto, addItembyChance, transformItemAdjacent, addItemAdjacentCondition, addItembyAdjacent;
    public int baseValue, valueDestroy, caculatedValue, ADOBuffValue, spinToDestroy, effectCountDestroy, turnsToAddSTH, spinsToAddSTH, turnsToTurnInto, percentage, ID, addItemChance, transformItemChance, destroyAgricultureChance, price;
    public bool markedDestruction = false;
    public int[] points = new int[2] { -1, -1 };
    public List<string> ADOBuffObjects = new List<string>(), ADODestroyObjects = new List<string>(), transformItems = new List<string>();

    public Symbol() {}

    public Symbol(Symbol symbol)
    {
        sequence++;
        ID = sequence;
        Debug.Log("ID: " + ID);
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
        addItemChance = symbol.addItemChance;
        addItembyChance = symbol.addItembyChance;
        transformItemChance = symbol.transformItemChance;
        transformItemAdjacent = symbol.transformItemAdjacent;
        destroyAgricultureChance = symbol.destroyAgricultureChance;
        addItemAdjacentCondition = symbol.addItemAdjacentCondition;
        addItembyAdjacent = symbol.addItembyAdjacent;
        price = symbol.price;
        points = new int[2] { -1, -1 };
        transformItems = new List<string>(symbol.transformItems);
        ADOBuffObjects = new List<string>(symbol.ADOBuffObjects);
        ADODestroyObjects = new List<string>(symbol.ADODestroyObjects);
    }

    public bool Equals(Symbol other)
    {
        return ID == other.ID;
    }
}

