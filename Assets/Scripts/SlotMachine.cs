using DG.Tweening;
using DG.Tweening.Core.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

class P
{
    public int x;
    public int y;
    string xx;
    public P(int x, int y)
    {
        this.x = x;
        this.y = y;
        xx = " s";
    }
}

public class SlotMachine : MonoBehaviour
{
    public GameObject badgeShowText;
    public static int currentBadge = 0;
    public TextMeshProUGUI badgetUI;
    public GameObject AddSymbolPanel;
    public GameManager gameManager;
    //magic value4, 5
    private Symbol[,] slots = new Symbol[4, 5];
    private Image[,] images = new Image[4, 5];
    private GameObject slotMachine;
    private int idCount = 0;
    private int musicCombooCount = 1;
    
    //List<Symbol> symbolsListInPlayer = new List<Symbol>();
    List<Symbol> symbolsListInHand = new List<Symbol>();
    List<Symbol> symbolsListPlayerTotal = new List<Symbol>();

    private EmotionAnimation emotionAnimation;
    
    public void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    private void Awake()
    {
        emotionAnimation = EmotionAnimation.Instance;
        slotMachine = GameObject.FindGameObjectWithTag("SlotMachine");
        Image[] imagesList = slotMachine.GetComponentsInChildren<Image>();
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                images[i,j] = imagesList[5*i+j];
            }
        }
    }

    private void Start()
    {
        SlotMachineInitialize();
        randPoitionSymbol();
        LoadSpritebySymbolName(4);
        List<P> intList = new List<P> { new P(0, 0), new P(1, 1) };
        P[,] ints = new P[4,2];
        ints[0,0] = intList[0];
        ints[0,1] = intList[1];
        intList.RemoveAt(0);
    }


    private void SlotMachineInitialize()
    {
        AddSymbolstoPlayerCount("The mead hall", 1);
        AddSymbolstoPlayerCount("Hrothgar", 1);
        AddSymbolstoPlayerCount("Hrothgar's wife", 1);
        AddSymbolstoPlayerCount("Seedling", 10);

        //AddSymbolstoPlayerCount("Honey", 6);
        //AddSymbolstoPlayerCount("Wheat", 10);
        //AddSymbolstoPlayerCount("Underwater lair", 6);
        //AddSymbolstoPlayerCount("Seedling", 15);
        //AddSymbolstoPlayerCount("Music", 10);
        //AddSymbolstoPlayerCount("Hrothgar", 5);
        //AddSymbolstoPlayerCount("Hrothgar's wife", 5);
        //AddSymbolstoPlayerCount("Beowulf", 5);
        //AddSymbolstoPlayerCount("Grendel 3", 5);
        //AddSymbolstoPlayerCount("Grendel 2", 5);
        //AddSymbolstoPlayerCount("Grendel 1", 5);
    }

    private void AddSymbolstoPlayerCount(string symbolName, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Symbol symbolTemp = new Symbol(CSVLoad.symbolsDict[symbolName]);
            symbolTemp.ID = idCount++;
            symbolsListPlayerTotal.Add(symbolTemp);
            symbolsListInHand.Add(symbolTemp);
        }
    }

    private void AddSymboltoHand(string symbolName)
    {
        symbolsListInHand.Add(CSVLoad.symbolsDict[symbolName]);
    }

    private void AddSymboltoPlayer(string symbolName)
    {
        symbolsListPlayerTotal.Add(CSVLoad.symbolsDict[symbolName]);
    }

    private int GetRandSymbolIndexfromHand()
    {
        int symbolSize = symbolsListInHand.Count;
        //Random.seed = System.DateTime.Now.Second;
        int randomValue = Random.Range(0, symbolSize);
        return randomValue;
    }

    private void randPoitionSymbol()
    {
        if (symbolsListInHand.Count < 20)
        {
            int emptyCount = 20 - symbolsListPlayerTotal.Count;
            for(int i = 0; i<emptyCount; i++)
            {
                AddSymboltoHand("Empty");
            }
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                //Debug.Log("Killing count: "+ DebugGetSymbolNameCountInList("Killing", symbolsListInHand));
                int symbolIndex = GetRandSymbolIndexfromHand();
                //also change position(function?)
                AddSymboltoSlotFromHand(i, j, symbolIndex);
            }
        }
    }

    private void LoadSpritebySymbolName(int rowTotal)
    {
        for(int i = 0; i < rowTotal; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                images[i, j].sprite = Resources.Load<Sprite>(slots[i,j].itemName);
            }
        }
    }

    private void AddSymboltoSlotFromHand(int x, int y, int symbolIndex)
    {
        slots[x, y] = symbolsListInHand[symbolIndex];
        //Debug.Log("slot"+x+y+": "+ symbolsListInHand[symbolIndex].itemName);
        slots[x, y].points = new int[] { x, y };
        //slots[x, y].name = x.ToString() + " " + y.ToString();
        symbolsListInHand.RemoveAt(symbolIndex);
        
    }

    public void AddSymboltoHandfromSlot(int x, int y)
    {
        symbolsListInHand.Add(slots[x, y]);
        slots[x, y].points = new int[] { -1, -1 };
        //slots[x, y] = null;
    }

    public IEnumerator Spin(int count, float waitTime)
    {
        ResetSymbolsinHand();
        randPoitionSymbol();
        for (int i = 0; i < 5; i++)
        {
            AddSymboltoHandfromSlot(3, i);
        }

        for (int i = 3; i>=1; i--)
        {
            for(int j =0; j < 5; j++)
            {
                slots[i, j] = slots[i - 1, j];
            }
        }

        for (int i = 0; i < 5; i++)
        {
            int indexRand = GetRandSymbolIndexfromHand();
            //add
            AddSymboltoSlotFromHand(0, i, indexRand);
        }
        //have qucikeer way
        LoadSpritebySymbolName(4);

        yield return new WaitForSeconds(waitTime/3.5f);
        count--;
        if(count > 0)
        {
            StartCoroutine(Spin(count * 2, waitTime));
        }
        if(count <= 0)
        {
            StartCoroutine(CaculateMoneywithDelay());
        }
    }

    IEnumerator CaculateMoneywithDelay()
    {
        yield return new WaitForSeconds(3f);
        CaculateMoney();
    }

    public void StartCoroutineSpin(float waitTime)
    {
        StartCoroutine(Spin(20, waitTime));
    }

    private struct Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    private List<Point> GetNeighborPoints(int row, int colum)
    {
        List<Point> points = new List<Point>();
        if(row + 1 < 4)
        {
            points.Add(new Point(row+1, colum));
        }
        if (row - 1 >= 0)
        {
            points.Add(new Point(row - 1, colum));
        }
        if (colum + 1 < 5)
        {
            points.Add(new Point(row, colum + 1));
        }
        if (colum - 1 >= 0)
        {
            points.Add(new Point(row, colum - 1));
        }
        return points;
    }

    private void ShowItemBadge(int imageX, int imageY)
    {
        images[imageX, imageY].GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
        images[imageX, imageY].GetComponentInChildren<TextMeshProUGUI>(true).text = slots[imageX, imageY].caculatedValue.ToString();
    }

    private void HideAllItemBadge()
    {
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 5; j++)
            images[i, j].GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
        }
    }

    private int Detect()
    {
        int extraBadge = 0;
        // Check if there are 3 music symbols in a row or column
        if (musicCombooCount <= 3)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (musicCombooCount > 3)
                    {
                        break;
                    }
                    if (j < 3 && slots[i, j].itemName == "Music" && slots[i, j + 1].itemName == "Music" && slots[i, j + 2].itemName == "Music")
                    {
                        Sequence musicSequence = DOTween.Sequence();
                        musicSequence.Append(emotionAnimation.AnimateHappiness(images[i, j].rectTransform));
                        musicSequence.Join(emotionAnimation.AnimateHappiness(images[i, j + 1].rectTransform));
                        musicSequence.Join(emotionAnimation.AnimateHappiness(images[i, j + 2].rectTransform));
                        emotionAnimation.sequenceCollection.Join(musicSequence.OnComplete(() =>
                        {
                            if (musicCombooCount <= 3)
                            {
                                ExecuteComboAction(musicCombooCount);
                                IncrementMusicComboCount();
                            }
                        }));
                    }
                    else if (i < 2 && slots[i, j].itemName == "Music" && slots[i + 1, j].itemName == "Music" && slots[i + 2, j].itemName == "Music")
                    {
                        Sequence musicSequence = DOTween.Sequence();
                        musicSequence.Append(emotionAnimation.AnimateHappiness(images[i, j].rectTransform));
                        musicSequence.Join(emotionAnimation.AnimateHappiness(images[i + 1, j].rectTransform));
                        musicSequence.Join(emotionAnimation.AnimateHappiness(images[i + 2, j].rectTransform));
                        emotionAnimation.sequenceCollection.Join(musicSequence.OnComplete(() =>
                        {
                            if (musicCombooCount <= 3)
                            {
                                ExecuteComboAction(musicCombooCount);
                                IncrementMusicComboCount();
                            }
                        }));
                    }
                }
            }
        }

        for (int i = 0; i< 4; i++)
        {
            for(int j = 0; j < 5; j++)
            {  
                List<Point> pointsNeighbours = GetNeighborPoints(i, j);
                if (slots[i, j].itemName == "Beowulf")
                {
                    Debug.Log("its beowulf");
                    //if neighboor's card's name contains "Grandel", then downgrade it. if it's grandel 3, transform it to grandel 2. if it's grandel 1, destroy it
                    foreach (Point point in pointsNeighbours)
                    {
                        if (slots[point.x, point.y].itemName.Contains("Grendel"))
                        {
                            Debug.Log("attack Grendel");
                            int tempX = point.x;
                            int tempY = point.y;
                            int tempRow = i;
                            int tempColum = j;
                            if (slots[point.x, point.y].itemName == "Grendel 3")
                            {
                                Debug.Log("attack Grendel 32");
                                //emotionAnimation.sequenceCollection.Append(emotionAnimation.FlashImage(images[point.x, point.y])).SetAutoKill(false).OnComplete(
                                //    () =>
                                //    {
                                //        Debug.Log("Transform Grendel 3");
                                //        TransformSymbol(tempX, tempY, "Grendel 3", "Grendel 2");
                                //    }
                                //    );
                                emotionAnimation.sequenceCollection.Append(emotionAnimation.FlashImage(images[tempX, tempY]).SetAutoKill(false).OnComplete(
                                    () =>
                                    {
                                        Debug.Log("Transform Grendel 3");
                                        TransformSymbol(tempX, tempY, "Grendel 3", "Grendel 2");
                                    }
                                    ));
                            }
                            else if (slots[point.x, point.y].itemName == "Grendel 2")
                            {
                                Debug.Log("attack Grendel 21");



                                emotionAnimation.sequenceCollection.Append(emotionAnimation.FlashImage(images[tempX, tempY]).SetAutoKill(false).OnComplete(
                                    () =>
                                    {
                                        Debug.Log("Transform Grendel 2");
                                        TransformSymbol(tempX, tempY, "Grendel 2", "Grendel 1");
                                    }
                                    ));
                            }
                            else if (slots[point.x, point.y].itemName == "Grendel 1")
                            {
                                Debug.Log("attack Grendel 10");
                                emotionAnimation.sequenceCollection.Append(emotionAnimation.FlashImage(images[tempX, tempY]).SetAutoKill(false).OnComplete(
                                    () =>
                                    {
                                        Debug.Log("Transform Grendel 1");
                                        DestroySymbol(tempRow, tempColum, tempX, tempY);
                                    }
                                    ));
                            }
                        }
                    }
                }

                //destroy neighboors
                foreach (var ADODestroyObject in slots[i,j].ADODestroyObjects){
                    foreach(Point point in pointsNeighbours){
                        if (ADODestroyObject == slots[point.x, point.y].itemName){
                            slots[point.x, point.y].markedDestruction = true;
                            emotionAnimation.sequenceCollection.Append(
                                emotionAnimation.AnimateExcitement(images[i, j].rectTransform).OnComplete(() =>
                            {
                                symbolsListInHand.Remove(slots[point.x, point.y]);
                                symbolsListPlayerTotal.Remove(slots[point.x, point.y]);
                                images[point.x, point.y].sprite = Resources.Load<Sprite>("Empty");
                                //also need to add destroyed value
                                slots[point.x, point.y].caculatedValue += slots[point.x, point.y].valueDestroy;
                                //add sth,maybe also animation
                                symbolsListPlayerTotal.Add(CSVLoad.symbolsDict[slots[point.x, point.y].objectAddWhenDestroyed]);
                            }));
                            emotionAnimation.sequenceCollection.Join(
                                emotionAnimation.AnimateSadness(images[point.x, point.y].rectTransform)
                                );
                            //add animation and sound
                        }
                    }
                }

                if(slots[i,j].transformItemChance != 0){
                    if(slots[i,j].transformItemAdjacent.Length == 0){
                        int roll = Random.Range(1, 101);
                        if(roll < slots[i,j].transformItemChance * slots[i, j].transformItems.Count)
                        {
                            //remove original card and add new transform card: slots, hand and intotal
                            var itemID = roll / slots[i, j].transformItemChance;
                            Debug.Log("TransformedItem: "+ slots[i, j].transformItems[itemID]);
                            var newItem = slots[i, j].transformItems[itemID];
                            var oldItem = slots[i, j].itemName;
                            int tempRow = i;
                            int tempColum = j;
                            emotionAnimation.sequenceCollection.Append(
                                                               emotionAnimation.AnimateSurprise(images[i, j].rectTransform).OnComplete(() =>
                                                               {
                                                                   TransformSymbol(tempRow, tempColum, oldItem, newItem); 
                                                               }));
                            //effect
                        }
                    }
                    else{
                        foreach(Point point in pointsNeighbours){
                            if(slots[i,j].transformItemAdjacent == slots[point.x, point.y].itemName){
                                int roll = Random.Range(1, 101);
                                if (roll < slots[i, j].transformItemChance * slots[i, j].transformItems.Count)
                                {
                                    //remove original card and add new transform card: slots, hand and intotal
                                    var itemID = roll / slots[i, j].transformItemChance;
                                    var newItem = slots[i, j].transformItems[itemID];
                                    var oldItem = slots[i, j].itemName;
                                    int tempRow = i;
                                    int tempColum = j;
                                    emotionAnimation.sequenceCollection.Append(
                                                                       emotionAnimation.AnimateSurprise(images[i, j].rectTransform).OnComplete(() =>
                                                                       {
                                                                           TransformSymbol(tempRow, tempColum, oldItem, newItem);

                                                                       }));
                                    //effect
                                }
                            }
                        }
                    }
                }

                if(slots[i,j].destroyAgricultureChance != 0){
                    foreach(Point point in pointsNeighbours){
                        if(slots[point.x, point.y].cardType.Equals("Agricultural")){
                            emotionAnimation.sequenceCollection.Append(
                                emotionAnimation.AnimateExcitement(images[i, j].rectTransform).OnComplete(() =>
                                {
                                    symbolsListInHand.Remove(slots[point.x, point.y]);
                                    symbolsListPlayerTotal.Remove(slots[point.x, point.y]);
                                    images[point.x, point.y].sprite = Resources.Load<Sprite>("Empty");
                                    //also need to add destroyed value
                                    slots[point.x, point.y].caculatedValue += slots[point.x, point.y].valueDestroy;
                                    //add sth,maybe also animation
                                    symbolsListPlayerTotal.Add(CSVLoad.symbolsDict[slots[point.x, point.y].objectAddWhenDestroyed]);
                                }));
                            emotionAnimation.sequenceCollection.Join(
                                emotionAnimation.AnimateSadness(images[point.x, point.y].rectTransform)
                                );
                            //effect
                        }
                    }
                }

                if (slots[i,j].addItemChance != 0)
                {
                    int roll = Random.Range(1, 101);
                    if (roll <= slots[i, j].addItemChance)
                    {
                        var addItem = slots[i, j].addItembyChance;
                        //sound effect, animation
                        emotionAnimation.sequenceCollection.Append(
                            emotionAnimation.AnimateHappiness(images[i, j].rectTransform).OnComplete(() =>
                        {
                            symbolsListPlayerTotal.Add(CSVLoad.symbolsDict[addItem]);
                            symbolsListInHand.Add(CSVLoad.symbolsDict[addItem]);
                        }));
                        
                    }
                }

                if (slots[i, j].addItembyAdjacent != null)
                {
                    foreach (Point point in pointsNeighbours)
                    {
                        if (slots[i, j].addItemAdjacentCondition == slots[point.x, point.y].itemName)
                        {
                            //add addItembyAdjacent to hand and intotal

                            var addItem = slots[i, j].addItembyAdjacent;
                            //sound effect, animation
                            emotionAnimation.sequenceCollection.Append(
                                emotionAnimation.AnimateHappiness(images[i, j].rectTransform).OnComplete(() =>
                                {
                                    symbolsListPlayerTotal.Add(CSVLoad.symbolsDict[addItem]);
                                    symbolsListInHand.Add(CSVLoad.symbolsDict[addItem]);
                                }));
                        }
                    }
                }

                //after x spins,self destroy
                if (slots[i, j].effectCountDestroy == 1)
                {
                    var tempRow = i;
                    var tempColum = j;
                    emotionAnimation.sequenceCollection.Append(
                                emotionAnimation.AnimateSadness(images[i, j].rectTransform).OnComplete(() =>
                                {
                                    symbolsListInHand.Remove(slots[tempRow, tempColum]);
                                    symbolsListPlayerTotal.Remove(slots[tempRow, tempColum]);
                                    images[tempRow, tempColum].sprite = Resources.Load<Sprite>("Empty");
                                    slots[tempRow, tempColum] = CSVLoad.symbolsDict["Empty"];
                                    //also need to add destroyed value
                                    slots[tempRow, tempColum].caculatedValue += slots[tempRow, tempColum].valueDestroy;
                                    //add sth,maybe also animation
                                    symbolsListPlayerTotal.Add(CSVLoad.symbolsDict[slots[tempRow, tempColum].objectAddWhenDestroyed]);
                                }));
                }
                else
                {
                    //Debug.Log("current effect count: "+ slots[row, colum].effectCountDestroy);
                    slots[i, j].effectCountDestroy--;
                }
            }
        }


        for (int i = 0; i< 4; i++)
        {
            for(int j = 0; j<5; j++)
            {
                ShowItemBadge(i, j);
            }
        }
        return extraBadge;
    }

    private void ExecuteComboAction(int comboCount)
    {
        string symbolName = "Grendel " + comboCount;
        Debug.Log("添加了巨魔");
        Debug.Log("添加了巨魔：" + symbolName);
        if (comboCount == 1)
        {
            symbolsListInHand.Add(CSVLoad.symbolsDict[symbolName]);
            symbolsListPlayerTotal.Add(CSVLoad.symbolsDict[symbolName]);
        }
        else
        {
            string preSymbolNmae = "Grendel " + (comboCount - 1);
            symbolsListInHand.Remove(CSVLoad.symbolsDict[preSymbolNmae]);
            symbolsListPlayerTotal.Remove(CSVLoad.symbolsDict[preSymbolNmae]);
            symbolsListInHand.Add(CSVLoad.symbolsDict[symbolName]);
            symbolsListPlayerTotal.Add(CSVLoad.symbolsDict[symbolName]);
        }
    }

    private void IncrementMusicComboCount()
    {
        musicCombooCount++;
    }

    private void TransformSymbol(int row, int colum, string oldSymbolName, string newSymbolName)
    {

        symbolsListInHand.Remove(CSVLoad.symbolsDict[oldSymbolName]);
        symbolsListPlayerTotal.Remove(CSVLoad.symbolsDict[oldSymbolName]);

        Symbol newSymbol = CSVLoad.symbolsDict[newSymbolName];
        symbolsListInHand.Add(newSymbol);
        symbolsListPlayerTotal.Add(newSymbol);

        slots[row, colum] = CSVLoad.symbolsDict[newSymbolName];
        images[row, colum].sprite = Resources.Load<Sprite>(newSymbolName);
    }

    private void DestroySymbol(int attackerX, int attackerY, int defendX, int defendY)
    {
        slots[defendX, defendY].markedDestruction = true;
        emotionAnimation.sequenceCollection.Append(
            emotionAnimation.AnimateExcitement(images[attackerX, attackerX].rectTransform).OnComplete(() =>
            {
                symbolsListInHand.Remove(slots[defendX, defendY]);
                symbolsListPlayerTotal.Remove(slots[defendX, defendY]);
                images[defendX, defendY].sprite = Resources.Load<Sprite>("Empty");
                //also need to add destroyed value
                int originalValue = slots[defendX, defendY].baseValue + slots[defendX, defendY].valueDestroy;
                slots[defendX, defendY].caculatedValue = originalValue;
                //add sth,maybe also animation
                symbolsListPlayerTotal.Add(CSVLoad.symbolsDict[slots[defendX, defendY].objectAddWhenDestroyed]);
            }));
        emotionAnimation.sequenceCollection.Join(
            emotionAnimation.AnimateSadness(images[defendX, defendY].rectTransform)
            );
        //add sound
    }

    private void CaculateMoney()
    {
        int earnedBadge = 0;
        earnedBadge += Detect();
        for (int i = 0; i< 3; i++)
        {
            for(int j = 0; j<4; j++)
            {
                earnedBadge += slots[i, j].caculatedValue;
            }
        }

        currentBadge += earnedBadge;
        badgetUI.text = "badge: " + currentBadge;
        Debug.Log("Update UI");
        emotionAnimation.sequenceCollection.Play().OnComplete(() => {
            Debug.Log("Start animation emotion");
            emotionAnimation.sequenceCollection = DOTween.Sequence();
            emotionAnimation.sequenceCollection.Rewind();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    slots[i, j].caculatedValue = slots[i, j].baseValue;
                }
            }
            Debug.Log("Start activating panel");
            StartCoroutine(SetPanelActivewithDelay(1f));
        });
    }

    private void ResetSymbolsinHand()
    {
        symbolsListInHand.Clear();
        foreach(var symbol in symbolsListPlayerTotal)
        {
            symbolsListInHand.Add(symbol);
        }
    }

    IEnumerator SetPanelActivewithDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameManager.SetGamePanelActive(currentBadge);
        ResetSymbolsinHand();
    }

    //reset value and symbols
    public void ClickNewSymbol(string tag)
    {
        GameObject btn = GameObject.FindGameObjectWithTag(tag);
        var textName = btn.GetComponentInChildren<TextMeshProUGUI>();
        AddSymboltoPlayer(textName.text);
        //HideAllItemBadge();
        AddSymbolPanel.SetActive(false);
    }
}
