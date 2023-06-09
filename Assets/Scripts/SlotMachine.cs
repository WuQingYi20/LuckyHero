using DG.Tweening;
using DG.Tweening.Core.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;
using Sequence = DG.Tweening.Sequence;
using Random = UnityEngine.Random;
using System.Threading;
using Unity.VisualScripting;
using System.Linq;

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
    public TextMeshProUGUI badgeShowText;
    public static int currentBadge = 0;
    public TextMeshProUGUI badgetUI;
    public GameObject AddSymbolPanel;
    private GameManager gameManager;
    private SoundEffectManager soundEffectManager;
    private BackgroundMusicManager backgroundMusicManager;
    //magic value4, 5
    private Symbol[,] slots = new Symbol[4, 5];
    private Image[,] images = new Image[4, 5];
    private GameObject slotMachine;
    private int idCount = 0;
    private int musicCombooCount = 1;
    private bool grendelExist = false;
    private bool goldCupExist = false;
    private bool wilglafExist = false;
    private bool spinAvaliable = true;
    public Button spinBtn;


    //List<Symbol> symbolsListInPlayer = new List<Symbol>();
    List<Symbol> symbolsListInHand = new List<Symbol>();
    List<Symbol> symbolsListPlayerTotal = new List<Symbol>();
    public List<Symbol> SymbolsListPlayerTotal => symbolsListPlayerTotal;

    private EmotionAnimation emotionAnimation;
    private LevelTransformData levelTransformData;
    public Image backgroundImage;
    
    public void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    Predicate<int> isSuccess = percentage =>
    {
        if (percentage < 0 || percentage > 100)
        {
            throw new ArgumentOutOfRangeException("Percentage should be between 0 and 100.");
        }

        int randomNumber = UnityEngine.Random.Range(0, 100);
        return randomNumber < percentage;
    };

    private static SlotMachine instance;

    public static SlotMachine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SlotMachine>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        
        slotMachine = GameObject.FindGameObjectWithTag("SlotMachine");
        Image[] imagesList = slotMachine.GetComponentsInChildren<Image>();
        levelTransformData = new LevelTransformData();
        for (int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                images[i,j] = imagesList[5*i+j];
            }
        }

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        
    }

    private void Start()
    {
        backgroundMusicManager = BackgroundMusicManager.Instance;
        backgroundMusicManager.Play("bgm");
        emotionAnimation = EmotionAnimation.Instance;
        soundEffectManager = SoundEffectManager.Instance;
        gameManager = GameManager.instance;
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
        AddSymbolstoPlayerCount("Seedling", 8);
        AddSymbolstoPlayerCount("Honey", 5);

        /*AddSymbolstoPlayerCount("Seedling", 15)*/
        ;
        //AddSymbolstoPlayerCount("Mead", 5);
        //AddSymbolstoPlayerCount("Underwater lair", 1);
        //AddSymbolstoPlayerCount("Seedling", 15);
        //AddSymbolstoPlayerCount("Music", 8);
        //AddSymbolstoPlayerCount("Hrothgar", 3);
        //AddSymbolstoPlayerCount("Hrothgar's wife", 3);
        //AddSymbolstoPlayerCount("Seedling", 10);
        //AddSymbolstoPlayerCount("The mead hall", 1);
        //AddSymbolstoPlayerCount("Beowulf", 5);
        //AddSymbolstoPlayerCount("Beowulf with the sword", 2);
        //AddSymbolstoPlayerCount("Grendel 1", 1);
        //AddSymbolstoPlayerCount("Grendel 3", 5);
        //AddSymbolstoPlayerCount("Grendel 1", 5);
        //AddSymbolstoPlayerCount("Wilglaf", 6);
        //AddSymbolstoPlayerCount("Warrior", 19);
        //AddSymbolstoPlayerCount("Old King Beowulf", 5);
        //AddSymbolstoPlayerCount("Villager", 5);
        //AddSymbolstoPlayerCount("Dragon awake", 5);
        //AddSymbolstoPlayerCount("Dragon sleeping", 1);
    }

    private void AddSymbolstoPlayerCount(string symbolName, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            Symbol symbolTemp = new Symbol(CSVLoad.symbolsDict[symbolName]);
            //symbolTemp.ID = idCount++;
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
        var newSymbol = CSVLoad.symbolsDict[symbolName];
        symbolsListPlayerTotal.Add(newSymbol);
        Debug.Log($"symbolname: {newSymbol.itemName} {newSymbol.markedTransform}");
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
        yield return new WaitForSeconds(2.5f);
        CaculateMoney();
    }

    public void StartCoroutineSpin(float waitTime)
    {
        if (spinAvaliable)
        {
            spinAvaliable = false;
            //make spinBtn unavaliable
            spinBtn.interactable = false;
            StartCoroutine(Spin(20, waitTime));
        }
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

        // The list of possible moves in 8 directions
        int[] dr = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dc = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < 8; i++)
        {
            int newRow = row + dr[i];
            int newColum = colum + dc[i];

            if (newRow >= 0 && newRow < 4 && newColum >= 0 && newColum < 5)
            {
                points.Add(new Point(newRow, newColum));
            }
        }

        return points;
    }


    private void ShowItemBadge(int imageX, int imageY)
    {
        images[imageX, imageY].GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
        images[imageX, imageY].GetComponentInChildren<TextMeshProUGUI>(true).text = slots[imageX, imageY].caculatedValue.ToString();
        images[imageX, imageY].GetComponentInChildren<TextMeshProUGUI>(true).color = UnityEngine.Color.yellow;
    }

    private void HideAllItemBadge()
    {
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 5; j++)
            images[i, j].GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
        }
    }

    private void ShowSelfDestroySpins()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                
                if (slots[i, j].effectCountsDestroy < 100)
                {
                    images[i, j].GetComponentsInChildren<TextMeshProUGUI>(true)[1].gameObject.SetActive(true);
                    images[i, j].GetComponentsInChildren<TextMeshProUGUI>(true)[1].text = slots[i, j].effectCountsDestroy.ToString();
                    images[i, j].GetComponentsInChildren<TextMeshProUGUI>(true)[1].color = UnityEngine.Color.red;
                }
                images[i, j].GetComponentsInChildren<TextMeshProUGUI>(true)[1].color = UnityEngine.Color.red;
            }      
        }
    }

    private void HideSelfDestroySpins()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (images[i, j].GetComponentsInChildren<TextMeshProUGUI>(true)[1].gameObject.activeSelf)
                {
                    images[i, j].GetComponentsInChildren<TextMeshProUGUI>(true)[1].gameObject.SetActive(false);
                }
            }
        }
    }

    private int Detect()
    {
        int extraBadge = 0;
        // Check if there are 3 music symbols in a row or column
        //改成两个巨魔地版本
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
                        Sequence musicComboSequence = DOTween.Sequence();

                        musicComboSequence.Append(emotionAnimation.AnimateHappiness(images[i, j].rectTransform));
                        musicComboSequence.Join(emotionAnimation.AnimateHappiness(images[i, j + 1].rectTransform));
                        musicComboSequence.Join(emotionAnimation.AnimateHappiness(images[i, j + 2].rectTransform).OnStart(() => {
                            soundEffectManager.Play("oh");
                        }).OnComplete(() =>
                        {
                            if (musicCombooCount <= 3)
                            {
                                ExecuteComboAction(musicCombooCount);
                                IncrementMusicComboCount();
                            }
                        }));
                        emotionAnimation.sequenceCollection.Append(musicComboSequence);
                    }
                    else if (i < 2 && slots[i, j].itemName == "Music" && slots[i + 1, j].itemName == "Music" && slots[i + 2, j].itemName == "Music")
                    {
                        emotionAnimation.sequenceCollection.Append(emotionAnimation.AnimateHappiness(images[i, j].rectTransform));
                        emotionAnimation.sequenceCollection.Join(emotionAnimation.AnimateHappiness(images[i + 1, j].rectTransform));
                        emotionAnimation.sequenceCollection.Join(emotionAnimation.AnimateHappiness(images[i + 2, j].rectTransform).OnStart(() => {
                            soundEffectManager.Play("oh");
                        }).OnComplete(
                            () =>
                            {
                                if (musicCombooCount <= 3)
                                {
                                    ExecuteComboAction(musicCombooCount);
                                    IncrementMusicComboCount();
                                }
                            }
                            ));
                    }
                }
            }
        }

        for (int i = 0; i< 4; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                List<Point> pointsNeighbours = GetNeighborPoints(i, j);
                //dragon awake

                if (slots[i, j].itemName == "Dragon awake")
                {
                    pointsNeighbours
                        .Where(point => slots[point.x, point.y].itemName == "Old King Beowulf")
                        .ToList()
                        .ForEach(point =>
                        {
                            emotionAnimation.sequenceCollection.Append(emotionAnimation.FlashImage(images[point.x, point.y]).OnStart(() =>
                            {
                                soundEffectManager.Play("dying");
                            }).SetAutoKill(false).OnComplete(
                                () =>
                                {
                                    slots[point.x, point.y].baseValue = 0;
                                }
                            ));
                        });
                }



                //Beowulf fight
                if (slots[i, j].itemName == "Beowulf")
                {
                    foreach (Point point in pointsNeighbours)
                    {
                        if (slots[point.x, point.y].itemName.Contains("Grendel"))
                        {
                            int tempX = point.x;
                            int tempY = point.y;
                            int tempRow = i;
                            int tempColum = j;
                            if (slots[point.x, point.y].itemName == "Grendel 3")
                            {
                                emotionAnimation.sequenceCollection.Append(emotionAnimation.FlashImage(images[tempX, tempY]).OnStart(() => {
                                    soundEffectManager.Play("monster");
                                }).SetAutoKill(false).OnComplete(
                                    () =>
                                    {
                                        Debug.Log("Transform Grendel 3");
                                        TransformSymbol(tempX, tempY, "Grendel 3", "Grendel 2");
                                    }
                                    ));
                            }
                            else if (slots[point.x, point.y].itemName == "Grendel 2")
                            {
                                emotionAnimation.sequenceCollection.Append(emotionAnimation.FlashImage(images[tempX, tempY]).SetAutoKill(false).OnStart(() => {
                                    soundEffectManager.Play("monster");
                                }).OnComplete(
                                    () =>
                                    {
                                        TransformSymbol(tempX, tempY, "Grendel 2", "Grendel 1");
                                    }
                                    ));
                            }
                            else if (slots[point.x, point.y].itemName == "Grendel 1")
                            {
                                emotionAnimation.sequenceCollection.Append(emotionAnimation.FlashImage(images[tempX, tempY]).SetAutoKill(false).OnStart(() => {
                                    soundEffectManager.Play("monster2");
                                }).OnComplete(
                                    () =>
                                    {
                                        AddSymbolstoPlayerCount("Underwater lair");
                                        AddSymbolstoPlayerCount("Grendel's arm");
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

                            if (!isSuccess(slots[i, j].destroyadjacentChance)) break;
                            if (slots[i, j].itemName == "Villager")
                            {
                                slots[i, j].effectCountsDestroy += 3;
                            }

                            slots[point.x, point.y].markedDestruction = true;
                            emotionAnimation.sequenceCollection.Append(
                                emotionAnimation.AnimateExcitement(images[i, j].rectTransform).OnStart(() => {
                                    soundEffectManager.Play("dying");
                                }).OnComplete(() =>
                            {
                                var tempX = point.x;
                                var tempY = point.y;
                                var tempRow = i;
                                var tempColum = j;
                                //win check
                                if (slots[tempX, tempY].itemName == "Dragon awake")
                                {
                                    StartCoroutine(KillDragon(tempX, tempY));
                                    //onWin应该加到coroutine中去
                                }

                                RemoveCardFromPlayerIntoal(slots[tempX, tempY]);
                                images[tempX, tempY].sprite = Resources.Load<Sprite>("Empty");
                                //also need to add destroyed value
                                slots[tempX, tempY].caculatedValue += slots[tempX, tempY].valueDestroy;
                                //add sth,maybe also animation
                                if (slots[tempX, tempY].objectAddWhenDestroyed != "")
                                {
                                    soundEffectManager.Play("add");
                                    AddSymbolstoPlayerCount(slots[tempX, tempY].objectAddWhenDestroyed);
                                }
                                

                                
                            }));
                            emotionAnimation.sequenceCollection.Join(
                                emotionAnimation.AnimateSadness(images[point.x, point.y].rectTransform)
                                );
                            //add animation and sound
                        }
                    }
                }

                //transform item
                if(slots[i,j].transformItemChance != 0){
                    if (slots[i, j].transformItemAdjacent.Length == 0) {
                        int roll = Random.Range(1, 101);
                        if (roll < slots[i, j].transformItemChance * slots[i, j].transformItems.Count)
                        {
                            Debug.Log($"try to transform: {roll}  {slots[i, j].transformItemChance * slots[i, j].transformItems.Count}  {slots[i, j].markedTransform}");
                            if (!slots[i, j].markedTransform)
                            {
                                //remove original card and add new transform card: slots, hand and intotal
                                var itemID = roll / slots[i, j].transformItemChance;
                                var newItem = slots[i, j].transformItems[itemID];
                                var oldItem = slots[i, j].itemName;
                                int tempRow = i;
                                int tempColum = j;
                                slots[i, j].markedTransform = true;
                                emotionAnimation.sequenceCollection.Append(
                                                                   emotionAnimation.AnimateSurprise(images[i, j].rectTransform).OnStart(() => {
                                                                       Debug.Log("start tween");
                                                                       soundEffectManager.Play("surpise");
                                                                   }).OnComplete(() =>
                                                                   {
                                                                       Debug.Log("After completetion");
                                                                       TransformSymbol(tempRow, tempColum, oldItem, newItem);
                                                                   }));
                                //effect
                            }
                        }
                    }
                    else{
                        foreach(Point point in pointsNeighbours){
                            if(slots[i,j].transformItemAdjacent == slots[point.x, point.y].itemName){
                                if (!slots[i, j].markedTransform)
                                {
                                    slots[i, j].markedTransform = true;
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
                                                                           emotionAnimation.AnimateSurprise(images[i, j].rectTransform).OnStart(() => {
                                                                               soundEffectManager.Play("surpise");
                                                                           }).OnComplete(() =>
                                                                           {
                                                                               TransformSymbol(tempRow, tempColum, oldItem, newItem);

                                                                           }));
                                        //effect
                                    }
                                }
                            }
                        }
                    }
                }

                //if beowulf is not appear, mother appear and has effect
                if (slots[i, j].itemName == "Underwater lair")
                {
                    Debug.Log("its Underwater lair");
                    var beowulfExistFlag = false;
                    //如果有beowulf就不变身
                    foreach(var point in pointsNeighbours)
                    {
                        if (slots[point.x, point.y].itemName == "Beowulf")
                        {
                            beowulfExistFlag = true;
                        }
                    }
                    if (!beowulfExistFlag)
                    {
                        Sprite motherSprite = Resources.Load<Sprite>("Grendel's mother");
                        emotionAnimation.sequenceCollection.Append(emotionAnimation.AnimateHorizontalFlip(images[i, j].rectTransform, motherSprite).OnStart(() => {
                            soundEffectManager.Play("sword");
                        }));
                    }
                }


                if(slots[i,j].destroyAgricultureChance != 0){
                    foreach(Point point in pointsNeighbours){
                        if(slots[point.x, point.y].cardType.Equals("Agricultural")){
                            int roll = Random.Range(1, 101);
                            if (roll < slots[i, j].destroyAgricultureChance)
                            {
                                emotionAnimation.sequenceCollection.Append(
                                emotionAnimation.AnimateExcitement(images[i, j].rectTransform).OnComplete(() =>
                                {
                                    symbolsListInHand.Remove(slots[point.x, point.y]);
                                    RemoveCardFromPlayerIntoal(slots[point.x, point.y]);
                                    images[point.x, point.y].sprite = Resources.Load<Sprite>("Empty");
                                    //also need to add destroyed value
                                    slots[point.x, point.y].caculatedValue += slots[point.x, point.y].valueDestroy;
                                    //add sth,maybe also animation
                                    AddSymbolstoPlayerCount(slots[point.x, point.y].objectAddWhenDestroyed);
                                }));
                                emotionAnimation.sequenceCollection.Join(
                                    emotionAnimation.AnimateSadness(images[point.x, point.y].rectTransform)
                                    );
                                //effect
                            }
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
                            emotionAnimation.AnimateHappiness(images[i, j].rectTransform).OnStart(() => {
                                soundEffectManager.Play("add");
                            }).OnComplete(() =>
                        {
                            AddSymbolstoPlayerCount(addItem);
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
                            if (addItem == "Gold Cup" && goldCupExist)
                            {
                                break;
                            }
                            else if(addItem == "Gold Cup")
                            {
                                goldCupExist = true;
                            }
                                //sound effect, animation
                                emotionAnimation.sequenceCollection.Append(
                                emotionAnimation.AnimateHappiness(images[i, j].rectTransform).OnStart(() => {
                                    soundEffectManager.Play("add");
                                }).OnComplete(() =>
                                {
                                    AddSymbolstoPlayerCount(addItem);
                                    symbolsListInHand.Add(CSVLoad.symbolsDict[addItem]);
                                }));
                        }
                    }
                }

                //after x spins,self destroy
                if (slots[i, j].effectCountsDestroy == 1)
                {

                    var tempRow = i;
                    var tempColum = j;
                    emotionAnimation.sequenceCollection.Append(
                                emotionAnimation.AnimateSadness(images[i, j].rectTransform).OnStart(() => {
                                    soundEffectManager.Play("dying");
                                }).OnComplete(() =>
                                {
                                    symbolsListInHand.Remove(slots[tempRow, tempColum]);
                                    RemoveCardFromPlayerIntoal(slots[tempRow, tempColum]);
                                    images[tempRow, tempColum].sprite = Resources.Load<Sprite>("Empty");
                                    slots[tempRow, tempColum] = CSVLoad.symbolsDict["Empty"];
                                    //also need to add destroyed value
                                    slots[tempRow, tempColum].caculatedValue += slots[tempRow, tempColum].valueDestroy;
                                    //add sth,maybe also animation
                                    AddSymbolstoPlayerCount(slots[tempRow, tempColum].objectAddWhenDestroyed);
                                }));
                }
                else
                {
                    //Debug.Log("current effect count: "+ slots[row, colum].effectCountsDestroy);
                    slots[i, j].effectCountsDestroy--;
                }


                //beowulf with sword
                if (slots[i, j].itemName == "Beowulf with the sword")
                {

                    foreach (var point in GetNeighborPoints(i, j))
                    {
                        if (slots[point.x, point.y].itemName.Contains("Underwater lair"))
                        {
                            Debug.Log("attack Underwater lair");
                            int tempX = point.x;
                            int tempY = point.y;
                            int tempRow = i;
                            int tempColum = j;
                            if (slots[point.x, point.y].itemName == "Underwater lair")
                            {
                                emotionAnimation.sequenceCollection.Append(emotionAnimation.FlashImage(images[tempX, tempY]).OnStart(() => {
                                    soundEffectManager.Play("sword");
                                }).SetAutoKill(false).OnComplete(
                                    () =>
                                    {
                                        Debug.Log("Transform Underwater lair");
                                        DestroySymbol(tempRow, tempColum, tempX, tempY);
                                        TransformLevel2toLevel3();
                                    }
                                    ));
                            }
                        }
                    }
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

    IEnumerator  KillDragon(int tempX, int tempY)
    {
        emotionAnimation.FlashImage(images[tempX, tempY]);
        yield return new WaitForSeconds(2f);
        gameManager.OnWin();
    }

    private void TransformLevel2toLevel3()
    {
        //add items, destroy items and change background
        foreach(var additem in levelTransformData.addItems)
        {
            AddSymbolstoPlayerCount(additem);
            symbolsListInHand.Add(CSVLoad.symbolsDict[additem]);
        }

        symbolsListPlayerTotal.RemoveAll(item => levelTransformData.destroyItems.Contains(item.itemName));
        backgroundImage.sprite = Resources.Load<Sprite>("BackgroundStage2");
        gameManager.currentStage = 3;
    }

    private void ExecuteComboAction(int comboCount)
    {
        string symbolName = "Grendel " + comboCount;
        if (comboCount == 1)
        {
            if (grendelExist)
            {
                return;
            }
            AddSymbolstoPlayerCount(symbolName);
            grendelExist = true;
        }
        else
        {
            string preSymbolNmae = "Grendel " + (comboCount - 1);
            symbolsListPlayerTotal.RemoveAll(symbol => symbol.itemName == preSymbolNmae);
            AddSymbolstoPlayerCount(symbolName);
            //下一轮抽卡肯定能是Beowulf，通过call GameManager里面的事件
            if (comboCount == 3)
            {
                gameManager.OnBeowulfExist();
            }
        }
    }

    private void IncrementMusicComboCount()
    {
        musicCombooCount++;
    }

    private void TransformSymbol(int row, int colum, string oldSymbolName, string newSymbolName)
    {
        Debug.Log($"enter transform: {oldSymbolName}");
        if (newSymbolName == "Wilglaf")
        {
            if (wilglafExist)
            {
                return;
            }
            else
            {
                wilglafExist = true;
                soundEffectManager.Play("sword");
            }
        }
        RemoveCardFromPlayerIntoal(slots[row, colum]);
        Symbol newSymbol = CSVLoad.symbolsDict[newSymbolName];

        symbolsListInHand.Add(newSymbol);
        AddSymbolstoPlayerCount(newSymbolName);

        slots[row, colum] = CSVLoad.symbolsDict[newSymbolName];
        images[row, colum].sprite = Resources.Load<Sprite>(newSymbolName);
        Debug.Log($"finish transform: {newSymbolName}");
    }

    private void DestroySymbol(int attackerX, int attackerY, int defendX, int defendY)
    {
        slots[defendX, defendY].markedDestruction = true;
        emotionAnimation.sequenceCollection.Append(
            emotionAnimation.AnimateExcitement(images[attackerX, attackerX].rectTransform).OnComplete(() =>
            {
                symbolsListInHand.Remove(slots[defendX, defendY]);
                RemoveCardFromPlayerIntoal(slots[defendX, defendY]);
                images[defendX, defendY].sprite = Resources.Load<Sprite>("Empty");
                //also need to add destroyed value
                int originalValue = slots[defendX, defendY].baseValue + slots[defendX, defendY].valueDestroy;
                slots[defendX, defendY].caculatedValue = originalValue;
                //add sth,maybe also animation
                AddSymbolstoPlayerCount(slots[defendX, defendY].objectAddWhenDestroyed);
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
        ShowSelfDestroySpins();
        emotionAnimation.sequenceCollection.Play().OnComplete(() => {
            emotionAnimation.sequenceCollection = DOTween.Sequence();
            emotionAnimation.sequenceCollection.Rewind();
            emotionAnimation.sequenceCollection.timeScale = 1.38f; ;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    slots[i, j].caculatedValue = slots[i, j].baseValue;
                }
            }
            soundEffectManager.Play("coin");
            HideAllItemBadge();
            HideSelfDestroySpins();
            UpdateBadge(earnedBadge);
            UpdateMarked();
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
        spinAvaliable = true;
        spinBtn.interactable = true;
        ResetSymbolsinHand();
    }


    private void RemoveCardFromPlayerIntoal(Symbol cardtobeRemove)
    {
        symbolsListPlayerTotal.Remove(cardtobeRemove);
    }

    //reset value and symbols
    public void ClickNewSymbol(string tag)
    {
        GameObject btn = GameObject.FindGameObjectWithTag(tag);
        var textName = btn.GetComponentInChildren<TextMeshProUGUI>();
        AddSymboltoPlayer(textName.text);
        //update badge
        UpdateBadge(-CSVLoad.symbolsDict[textName.text].price);
        //HideAllItemBadge();
        AddSymbolPanel.SetActive(false);
    }

    public void SkipBtn()
    {
        AddSymbolPanel.SetActive(false);
    }

    public void UpdateBadge(int badgeOffset)
    {
        currentBadge += badgeOffset;
        //Debug.Log("current badge:" + currentBadge);
        badgeShowText.text = currentBadge.ToString();
    }

    //solve markedTransform issue, but not solve add issues
    private void UpdateMarked()
    {
        foreach(var symbol in symbolsListPlayerTotal)
        {
            symbol.markedDestruction = false;
            symbol.markedTransform = false;
        }
    }
}
