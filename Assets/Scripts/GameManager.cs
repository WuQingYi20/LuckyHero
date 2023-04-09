using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public GameObject winPage;
    public GameObject uploadPage;
    public GameObject failPage;
    public GameObject collectPage;
    public static int countstoWinGame = 5;
    public static int badgetoUpload = 80;
    public static int countstoUploadBadge = 5;
    private int currentCountstoUploadMoney;



    private void Start()
    {
        currentCountstoUploadMoney = countstoUploadBadge;
    }

    private void Update()
    {
        if(countstoWinGame == 0)
        {
            WinGame();
        }
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
            //需要生成的卡牌

            //获取三个btn的引用并且给他们随机值
            int sumPossibility = CalculateTotalPossibility();
            List<string> itemNameList = new List<string>();
            var btnList = collectPage.GetComponentsInChildren<Button>();
            for (int i =0; i < 3; i++)
            {
                itemNameList.Add(GenerateRandomCardFromDatabase(sumPossibility));
                //给btn的属性赋值：symbolname, baseValue, description and image
                var textListInBtn = btnList[i].GetComponentsInChildren<TextMeshProUGUI>();
                textListInBtn[0].text = itemNameList[i]; //name
                textListInBtn[1].text = CSVLoad.symbolsDict[itemNameList[i]].baseValue + " badge";
                textListInBtn[2].text = CSVLoad.symbolsDict[itemNameList[i]].description;
                var images = btnList[i].GetComponentsInChildren<Image>();
                images[1].sprite = Resources.Load<Sprite>(itemNameList[i]);
            }


            var textList = collectPage.GetComponentsInChildren<TextMeshProUGUI>();
            //next needed badge, turns to upload
            textList[0].text = "Badge needed for next upload: " + badgetoUpload;
            textList[1].text = "Turns to upload badge: " + currentCountstoUploadMoney;
        }
    }
    
    private int CalculateTotalPossibility()
    {
        int sumPossibility = 0;
        foreach(var symbol in CSVLoad.symbols)
        {
            sumPossibility += symbol.possibility; 
        }
        return sumPossibility;
    }

    private string GenerateRandomCardFromDatabase(int sumPossibility)
    {
        int randValue = Random.Range(0, sumPossibility);
        foreach(var symbol in CSVLoad.symbols)
        {
            randValue -= symbol.possibility;
            if(randValue <= 0)
            {
                return symbol.itemName;
            }
        }
        //impossible
        return null;
    }

    public void uploadBadge()
    {
        SlotMachine.currentBadge -= badgetoUpload;
        uploadPage.SetActive(false);
    }

    private void WinGame()
    {
        winPage.SetActive(true);
    }

    private void NewBadgetoUpload()
    {
        switch (countstoWinGame)
        {
            case 4:
                badgetoUpload = 130;
                break;
            case 3:
                badgetoUpload = 200;
                break;
            case 2:
                badgetoUpload = 300;
                break;
            case 1:
                badgetoUpload = 450;
                break;
            default:
                break;
        }
    }
}
