using System.Collections.Generic;
using UnityEngine;

public class CSVLoad : MonoBehaviour
{
    public static List<Symbol> symbols = new();
    public static Dictionary<string, Symbol> symbolsDict = new();

    private void Awake()
    {
        ReadCSV("ItemValueStory");
        Debug.Log("CSV has read");
    }
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

                //destroy detection
                valueDestroy = int.Parse(row[4]),
                ADODestroyObjects = new List<string>(row[5].Split(';')),
                objectAddWhenDestroyed = row[6],

                //count-- destroy itself
                effectCountsDestroy = int.Parse(row[7]),
                //outside square
                spinsToDestroy = int.Parse(row[8]),

                //based on chance to generate new things
                addItemChance = int.Parse(row[9]),
                addItembyChance = row[10],

                //transform into sth based on chance and adj. if adj is null, only chance works
                transformItemChance = int.Parse(row[11]),
                transformItems = new List<string>(row[12].Split(';')),
                transformItemAdjacent = row[13],

                //remove agriculture if percentage is not 0 
                destroyAgricultureChance = int.Parse(row[14]),

                //add item if it meets condition
                addItemAdjacentCondition = row[15],
                addItembyAdjacent = row[16],

                //use it in collection
                percentage = int.Parse(row[17]),
                price = int.Parse(row[18]),
                stage = int.Parse(row[19]),
                destroyadjacentChance = int.Parse(row[20]),
                //update after generating
                points = new int[2] { -1, -1 },
                markedDestruction = false,
                markedTransform = false,
            };

            symbols.Add(symbolTemp);
            symbolsDict.Add(row[0], symbolTemp);
        }
    }
}




