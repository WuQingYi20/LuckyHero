using DG.Tweening;
using DG.Tweening.Core.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
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
        //AddSymbolstoPlayerCount("The mead hall", 1);
        //AddSymbolstoPlayerCount("Hrothgar", 1);
        //AddSymbolstoPlayerCount("Hrothgar's wife", 1);
        //AddSymbolstoPlayerCount("Seedling", 3);
        AddSymbolstoPlayerCount("Honey", 6);
        AddSymbolstoPlayerCount("Wheat", 6);

    }

    private void AddSymbolstoPlayerCount(string symbolName, int count)
    {
        for (int i = 0; i < count; i++)
        {
            //Debug.Log("ID: " + idCount);
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
        for(int i = 0; i< 4; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                List<Point> pointsNeighbours = GetNeighborPoints(i, j);
                foreach(var ADODestroyObject in slots[i,j].ADODestroyObjects){
                    foreach(Point point in pointsNeighbours){
                        if (ADODestroyObject == slots[point.x, point.y].itemName){
                            slots[point.x, point.y].markedDestruction = true;
                            Debug.Log("This item has been destroyed: "+ slots[point.x, point.y].itemName);
                            
                            //这里可能是逐步的，放在动画结束后
                            //slotMachineAnimation.PlaySadnessAnimation(images[point.x, point.y]);
                            emotionAnimation.AnimateExcitement(images[i, j].rectTransform).OnComplete(() =>
                            {
                                symbolsListInHand.Remove(slots[point.x, point.y]);
                                symbolsListPlayerTotal.Remove(slots[point.x, point.y]);
                                images[point.x, point.y].sprite = Resources.Load<Sprite>("Empty");
                            });
                            //add animation and sound
                        }
                    }
                }

                if(slots[i,j].transformItemChance != 0){
                    if(slots[i,j].transformItemAdjacent == null){
                        int roll = Random.Range(1, 101);
                        if(roll < slots[i,j].transformItemChance * slots[i, j].transformItems.Count)
                        {
                            //remove original card and add new transform card: slots, hand and intotal
                            var itemID = roll / slots[i, j].transformItemChance;
                            TransformSymbol(i, j, slots[i, j].transformItems[itemID]);
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
                                    TransformSymbol(i, j, slots[i, j].transformItems[itemID]);
                                    //effect
                                }
                            }
                        }
                    }
                }

                if(slots[i,j].destroyAgricultureChance != 0){
                    foreach(Point point in pointsNeighbours){
                        if(slots[point.x, point.y].cardType.Equals("Agricultural")){
                            symbolsListInHand.Remove(slots[point.x, point.y]);
                            symbolsListPlayerTotal.Remove(slots[point.x, point.y]);
                            slots[i, j] = CSVLoad.symbolsDict["empty"];
                            //effect
                        }
                    }
                }

                if (slots[i, j].addItembyAdjacent != null)
                {
                    foreach (Point point in pointsNeighbours)
                    {
                        if (slots[i, j].addItembyAdjacent == slots[point.x, point.y].itemName)
                        {
                            //add addItembyAdjacent to hand and intotal
                            symbolsListInHand.Add(CSVLoad.symbolsDict[slots[i, j].addItembyAdjacent]);
                            symbolsListPlayerTotal.Add(CSVLoad.symbolsDict[slots[i, j].addItembyAdjacent]);
                            //effect
                        }
                    }
                }

                //after x spins,self destroy
                if (slots[i, j].effectCountDestroy == 1)
                {
                    slots[i,j] = null;
                    symbolsListInHand.Remove(slots[i, j]);
                    symbolsListPlayerTotal.Remove(slots[i, j]);
                }
                else
                {
                    slots[i, j].effectCountDestroy--;
                }
            }
        }

        //整体的检测
        

        for (int i = 0; i< 4; i++)
        {
            for(int j = 0; j<5; j++)
            {
                ShowItemBadge(i, j);
            }
        }
        return extraBadge;
    }

    private void TransformSymbol(int i, int j, string newSymbolName)
    {
        //remove original card and add new transform card: slots, hand and intotal
        Symbol oldSymbol = slots[i, j];
        symbolsListInHand.Remove(oldSymbol);
        symbolsListPlayerTotal.Remove(oldSymbol);

        Symbol newSymbol = CSVLoad.symbolsDict[newSymbolName];
        slots[i, j] = newSymbol;
        symbolsListInHand.Add(newSymbol);
        symbolsListPlayerTotal.Add(newSymbol);
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
        emotionAnimation.sequence.Play().OnComplete(() => {
            Debug.Log("Start animation emotion");
            emotionAnimation.sequence = DOTween.Sequence();
            emotionAnimation.sequence.Rewind();
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
