using DG.Tweening;
using DG.Tweening.Core.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    //animation
    private Sequence detectSequence;

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

    private SlotMachineAnimation slotMachineAnimation;
    
    public void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    private void Awake()
    {
        slotMachineAnimation = SlotMachineAnimation.Instance;
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
        detectSequence = DOTween.Sequence();
        detectSequence.Pause();
        SlotMachineInitialize();
        randPoitionSymbol();
        LoadSpritebySymbolName(4);
        List<P> intList = new List<P> { new P(0, 0), new P(1, 1) };
        P[,] ints = new P[4,2];
        ints[0,0] = intList[0];
        ints[0,1] = intList[1];
        intList.RemoveAt(0);
        Debug.Log(intList[0].x);
        Debug.Log(ints[0,0].x);
        Debug.Log(" " + ints[0,1].x);
    }


    private void SlotMachineInitialize()
    {
        AddSymbolstoPlayerCount("Hrothgar", 9);
        AddSymbolstoPlayerCount("Hrothgar's wife", 8);
        AddSymbolstoPlayerCount("Dragen sleeping", 1);
        AddSymbolstoPlayerCount("Killing", 1);
    }

    private int DebugGetSymbolNameCountInList(string name, List<Symbol> symbolList)
    {
        int count = 0;
        foreach(Symbol symbol in symbolList)
        {
            if(symbol.itemName == name)
            {
                count++;
            }
        }
        return count;
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

    

    private void RemoveSymbolinHand(int x, int y)
    {
        //��������ȥ���������
        foreach(var symbol in symbolsListInHand)
        {
            if (symbol.points[0] == x && symbol.points[1] == y)
            {
                symbolsListInHand.Remove(symbol);
            }
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

    //��һ�������ڰ�symbol�Ż���֮��
    private int GetRandSymbolIndexfromHand()
    {
        //�Ѿ��ڳ��ϵ�symbol���ں���
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
        //Debug.Log("breakPoint");
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

    private int SearchSymbolIndexbyPoints(int x, int y)
    {
        for(int i=0; i<symbolsListInHand.Count; i++)
        {
            if (symbolsListInHand[i].points[0] == x && symbolsListInHand[i].points[1] == y)
            {
                return i;
            }
        }
        return - 1;
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
        //exchange hand's symbol and slot's symbol
        //��������ߣ��м�������ƶ����������������Ƴ���
        //�ѵ�4������
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

        //�����ĸ�����Ƴ���
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
            //����
        }
    }

    IEnumerator CaculateMoneywithDelay()
    {
        yield return new WaitForSeconds(3f);
        CaculateMoney();//����Detect
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
        //Debug.Log("Slots.name: "+ slots[row, colum].itemName);
        //Debug.Log("row: " + row + "colum: " + colum);
        List<Point> points = new List<Point>();
        if(row + 1 < 4)
        {
            points.Add(new Point(row+1, colum));
            //Debug.Log("1: " + (row + 1));
        }
        if (row - 1 >= 0)
        {
            points.Add(new Point(row - 1, colum));
            //Debug.Log("2: " + (row - 1));
        }
        if (colum + 1 < 5)
        {
            points.Add(new Point(row, colum + 1));
            //Debug.Log("3: " + (colum + 1));
        }
        if (colum - 1 >= 0)
        {
            points.Add(new Point(row, colum - 1));
            //Debug.Log("4: " + (colum - 1));
        }
        return points;
    }

    //hide?
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


    


    //������effect,�������¼���value�����һ��extra��Ǯ
    private int Detect()
    {
        int extraBadge = 0;
        for(int i = 0; i< 4; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                List<Point> pointsNeighbours = GetNeighborPoints(i, j);
                foreach(Point point in pointsNeighbours)
                {
                    //ADO buff
                    foreach (var ADODBuffObjectName in slots[i, j].ADOBuffObject)
                    {
                        if (ADODBuffObjectName == slots[point.x, point.y].itemName)
                        {
                            slots[point.x, point.y].caculatedValue += slots[i, j].ADOBuffValue;

                            detectSequence.Append(images[point.x, point.y].transform.DOSpiral(1f, Vector3.up,
                                    SpiralMode.ExpandThenContract, 1, 10, 1, true).OnComplete(() =>
                                    {

                                    }
                                
                                ));
                            detectSequence.Join(images[i, j].transform.DOSpiral(1f, Vector3.forward,
                                    SpiralMode.ExpandThenContract, 1, 10, 10).OnComplete(() =>
                                    {

                                    }

                                ));
                        }
                    }

                   //ADO destroy
                    foreach (var ADODestroyObjectName in slots[i, j].ADODestroyObject)
                    {
                        if (ADODestroyObjectName == slots[point.x, point.y].itemName)
                        {
                            slots[point.x, point.y].markedDestruction = true;
                            //Ҫ������Ǯ��ʲôʱ�����ٶ��������
                            //��һ�ֶ���
                            detectSequence.Append(images[point.x, point.y].transform.DOSpiral(1f, Vector3.up,
                                    SpiralMode.ExpandThenContract, 1, 10, 1, true).OnComplete(() =>
                                    {
                                        slots[point.x, point.y] = null;
                                        symbolsListPlayerTotal.Remove(slots[point.x, point.y]);
                                    }

                                ));
                            detectSequence.Join(images[i, j].transform.DOSpiral(1f, Vector3.forward,
                                    SpiralMode.ExpandThenContract, 1, 10, 10).OnComplete(() =>
                                    {

                                    }

                                ));
                        }
                    }
                }

                //after x spins,self destroy
                if (slots[i, j].spinToDestroy == 1)
                {
                    slots[i,j] = null;
                    symbolsListPlayerTotal.Remove(slots[i, j]);
                }
                else
                {
                    slots[i, j].effectCountDestroy--;
                }
                //after x spins,add something
                if (slots[i,j].spinsToAddSTH == 1)
                {
                    symbolsListPlayerTotal.Add(CSVLoad.symbolsDict[slots[i, j].objectAddEveryXTurnsORSpins]);
                }
                else
                {
                    slots[i, j].effectCountDestroy--;
                }

                //after x spins, turn into sth
                if (slots[i, j].turnsToAddSTH == 1)
                {
                    symbolsListPlayerTotal.Add(CSVLoad.symbolsDict[slots[i, j].objectTurnInto]);
                    slots[i, j] = null;
                    symbolsListPlayerTotal.Remove(slots[i, j]);
                }
                else
                {
                    slots[i, j].turnsToAddSTH--;
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



    //��Ǯ����͸���UI
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
        detectSequence.Play().OnComplete(() =>
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    slots[i, j].caculatedValue = slots[i, j].baseValue;
                }
            }
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
    //���Ƴ�ȡ
    public void ClickNewSymbol(string tag)
    {
        //�ѿ��Ƽ��뵽hand�У���ô��ȡ���ֺͶ��󣬿��Ƶ����ɣ�������UI�ϵ�����
        GameObject btn = GameObject.FindGameObjectWithTag(tag);
        var textName = btn.GetComponentInChildren<TextMeshProUGUI>();
        AddSymboltoPlayer(textName.text);
        //����������
        //HideAllItemBadge();
        AddSymbolPanel.SetActive(false);
    }
}
