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
                textList[1].text = "you need to upload " + badgetoUpload+" badges";
                
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
            for (int i =0; i < 3; i++)
            {
                itemNameList.Add(GenerateRandomCardFromDatabase(sumPossibility));
                //��btn�����Ը�ֵ��symbolname, baseValue, description and image
                var textListInBtn = btnList[i].GetComponentsInChildren<TextMeshProUGUI>();
                textListInBtn[0].text = itemNameList[i]; //name
                textListInBtn[1].text = "price: "+ CSVLoad.symbolsDict[itemNameList[i]].price;
                textListInBtn[2].text = CSVLoad.symbolsDict[itemNameList[i]].description;
                var images = btnList[i].GetComponentsInChildren<Image>();
                images[2].sprite = Resources.Load<Sprite>(itemNameList[i]);
                //钱不够，禁止买卖，不够的情况下要改变btn的颜色
                if (CSVLoad.symbolsDict[itemNameList[i]].price > SlotMachine.currentBadge)
                {
                    btnList[i].interactable = false;
                    images[1].color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                }
                else
                {
                    btnList[i].interactable = true;
                    images[1].color = new Color(1f, 1f, 1f, 1f);
                }
            }

            var textList = collectPage.GetComponentsInChildren<TextMeshProUGUI>();
            //next needed badge, turns to upload
            textList[0].text = "Badge needed for next upload: " + badgetoUpload;
            textList[1].text = "Turns to upload badge: " + currentCountstoUploadMoney;
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
        if(badgetoUpload < 5)
        {
            badgetoUpload = 30 + 10 * uploadCount;
        }
        else
        {
            badgetoUpload = 54 + 4 * uploadCount;
        }
    }
}

