using System.Collections.Generic;
using UnityEngine;

public class CSVLoad : MonoBehaviour
{
    public static List<Symbol> symbols = new();
    public static Dictionary<string, Symbol> symbolsDict = new();

    private void Start() => ReadCSV("ItemValueStory");

    public static void ReadCSV(string CSVPath)
    {
        TextAsset temp = Resources.Load<TextAsset>(CSVPath);
        string[] splitText = temp.text.Split('\n');

        for (int i = 1; i < splitText.Length - 1; i++)
        {
            string[] row = splitText[i].Split(',');
            Symbol symbolTemp = new Symbol()
            {
                itemName = row[0],
                description = row[1],
                cardType = row[2],
                baseValue = int.Parse(row[3]),
                caculatedValue = int.Parse(row[3]),
                valueDestroy = int.Parse(row[4]),
                ADODestroyObject = new List<string>(row[5].Split(';')),
                objectAddWhenDestroyed = row[6],
                effectCountDestroy = int.Parse(row[7]),
                spinToDestroy = int.Parse(row[8]),
                addItemChance = int.Parse(row[9]),
                addItembyChance = row[10],
                transformItemCance = int.Parse(row[11]),
                transformItem = row[12],
                transformItemAdjacent = row[13],
                destroyAgricultureChance = int.Parse(row[14]),
                addItemAdjacentCondition = row[15],
                addItembyAdjacent = row[16],
                percentage = int.Parse(row[17]),
                price = int.Parse(row[18]),
                points = new int[2] { -1, -1 },
                ID = i
            };
            symbols.Add(symbolTemp);
            symbolsDict.Add(row[0], symbolTemp);
        }
    }
}




