using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private SlotMachine slotMachine;
    public GameObject winPage;
    public GameObject uploadPage;
    public GameObject failPage;
    public GameObject collectPage;

    public static int badgetoUpload = 30;
    public static int countstoUploadBadge = 5;
    private int uploadCount = 0;
    private int currentCountstoUploadMoney;
    //再杀死巨魔妈妈之后会转变
    private int currentStage = 1;
    public event Action WinEvent;
    public event Action BeowulfExistEvent;
    private int beowulfExistFlag = 0;

    private void Start()
    {
        currentCountstoUploadMoney = countstoUploadBadge;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        WinEvent += WinGame;
        BeowulfExistEvent += BeowulfExist;
        slotMachine = SlotMachine.Instance;
    }

    public void OnWin()
    {
        WinEvent?.Invoke();
    }

    public void SetGamePanelActive(int playerBadge)
    {
        currentCountstoUploadMoney--;
        if (currentCountstoUploadMoney == 0)
        {
            currentCountstoUploadMoney = countstoUploadBadge;
            if (badgetoUpload <= playerBadge)
            {
                uploadPage.SetActive(true);
                var textList = uploadPage.GetComponentsInChildren<TextMeshProUGUI>();
                textList[0].text = "your current badge: " + SlotMachine.currentBadge;
                textList[1].text = "you need to upload " + badgetoUpload + " badges";

            }
            else
            {
                failPage.SetActive(true);
            }
        }
        else
        {
            collectPage.SetActive(true);
            int sumPossibility = CalculateTotalPossibility();
            List<string> itemNameList = new List<string>();
            var btnList = collectPage.GetComponentsInChildren<Button>();
            for (int i = 0; i < 3; i++)
            {
                if (beowulfExistFlag == 1)
                {
                    itemNameList.Add("Beowulf");
                }
                else
                {
                    itemNameList.Add(GenerateRandomCardFromDatabase(sumPossibility));
                }
                PutValuetoCard(btnList[i], itemNameList[i]);
            }

            if (beowulfExistFlag == 1)
            {
                beowulfExistFlag++;
            }
            var textList = collectPage.GetComponentsInChildren<TextMeshProUGUI>();
            //next needed badge, turns to upload
            textList[0].text = "Badge needed for next upload: " + badgetoUpload;
            textList[1].text = "Turns to upload badge: " + currentCountstoUploadMoney;
        }
    }

    private void PutValuetoCard(Button btn, string cardName, bool beowulfDisable = false)
    {
        var textListInBtn = btn.GetComponentsInChildren<TextMeshProUGUI>();
        textListInBtn[0].text = cardName; //name
        textListInBtn[1].text = "price: " + CSVLoad.symbolsDict[cardName].price;
        textListInBtn[2].text = CSVLoad.symbolsDict[cardName].description;
        var images = btn.GetComponentsInChildren<Image>();
        images[2].sprite = Resources.Load<Sprite>(cardName);
        //钱不够，禁止买卖，不够的情况下要改变btn的颜色
        if (CSVLoad.symbolsDict[cardName].price > SlotMachine.currentBadge || beowulfDisable)
        {
            btn.interactable = false;
            images[1].color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
        else
        {
            btn.interactable = true;
            images[1].color = new Color(1f, 1f, 1f, 1f);
        }
    }

    private int CalculateTotalPossibility()
    {
        //according to current stage
        int sumPossibility = 0;
        foreach(var symbol in CSVLoad.symbols)
        {
            if (currentStage >= symbol.stage)
            {
                sumPossibility += symbol.percentage;
            } 
        }
        return sumPossibility;
    }

    private string GenerateRandomCardFromDatabase(int sumPossibility)
    {
        int randValue = Random.Range(0, sumPossibility);
        foreach(var symbol in CSVLoad.symbols)
        {
            if (currentStage >= symbol.stage)
            {
                randValue -= symbol.percentage;
                if (randValue <= 0)
                {
                    return symbol.itemName;
                }
            }
        }
        //impossible
        return null;
    }

    public void uploadBadge()
    {
        slotMachine.UpdateBadge(-badgetoUpload);
        uploadPage.SetActive(false);
        uploadCount++;
        countstoUploadBadge = 5;
        UpdateBadgetoUploadaccordingtoCount();
    }

    private void WinGame()
    {
        winPage.SetActive(true);
    }

    private void UpdateBadgetoUploadaccordingtoCount()
    {
        if(badgetoUpload < 6)
        {
            badgetoUpload = 36 + 10 * uploadCount;
        }
        else if(badgetoUpload < 10)
        {
            badgetoUpload = 56 + 6 * uploadCount;
        }
        else
        {
            badgetoUpload = 74 + 4 * uploadCount;
        }
    }

    internal void OnBeowulfExist()
    {
        BeowulfExistEvent?.Invoke();
    }

    private void BeowulfExist()
    {
        beowulfExistFlag++;
    }
}

