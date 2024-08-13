using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GachaSystem : MonoBehaviour
{
   public RectTransform gachaSlotParent;
   public GachaResultSlot resultSlot;
   
   private GachaTable gachaTable;
   private AssetListTable aasetListTable;
   private StringTable stringTable;
   
   public Button closeButton;
   private List<GachaResultSlot> spawnedSlots = new List<GachaResultSlot>();
   
   [SerializeField, Range(0f, 10f),Tooltip("고급 등급 확률이 해당 값에 따라서 변경됩니다."),Header("고급 등급 확률")]
   private float highGradeProbability = 3f;

   [SerializeField, Range(0f, 30f),Tooltip("중급 등급 확률이 해당 값에 따라서 변경됩니다."),Header("중급 등급 확률")]
   private float midGradeProbability = 20f;
   
   
   private void Start()
   {
      gachaTable = DataTableManager.Get<GachaTable>(DataTableIds.Gacha);
      aasetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
      stringTable = DataTableManager.Get<StringTable>(DataTableIds.String);
      if (closeButton != null)
      {
         closeButton.onClick.AddListener(ClearGachaResults);
      }
   }


   public void PerformGacha(int times)
   {
      for (int i = 0; i < times; i++)
      {
         GachaData result = GetRandomGachaResult();
         if (result != null)
         {
            bool isNew = !GameManager.instance.GameData.ObtainedGachaIDs.Contains(result.Id);
                
            if (isNew)
            {
               GameManager.instance.GameData.ObtainedGachaIDs.Add(result.Id);
               Logger.Log($"Obtained Gacha ID: {result.Id}");
               DataManager.SaveFile(GameManager.instance.GameData);
                
               // 새로운 캐릭터가 추가되었으므로, DeckSlotController를 새로고침
               DeckSlotController deckSlotController = FindObjectOfType<DeckSlotController>();
               if (deckSlotController != null)
               {
                  deckSlotController.RefreshCharacterSlots();
               }
            }
            else
            {
               switch (gachaTable.table[result.Id].Grade)
               {
                  case 0:
                     GameManager.instance.GameData.Mileage += 100;
                     break;
                  case 1:
                     GameManager.instance.GameData.Mileage += 300;
                     break;
                  case 2:
                     GameManager.instance.GameData.Mileage += 500;
                     break;   
               }
               DataManager.SaveFile(GameManager.instance.GameData);
            }
            SpawnResultSlot(result, isNew);
         }
      }
   }
   
   private GachaData GetRandomGachaResult()
   {
      float rand = UnityEngine.Random.value * 100f;
      int grade;

      if (rand <= highGradeProbability)
      {
         grade = 2;
      }
      else if (rand <= highGradeProbability + midGradeProbability)
      {
         grade = 1;
      }
      else
      {
         grade = 0;
      }

      List<GachaData> possibleResults = new List<GachaData>();

      foreach (var gachaData in gachaTable.table.Values)
      {
         if (gachaData.Grade == grade)
         {
            possibleResults.Add(gachaData);
         }
      }
      
      int index = UnityEngine.Random.Range(0, possibleResults.Count);
      return possibleResults[index];
   }
   private void SpawnResultSlot(GachaData data,bool isNew)
   {
      var slot = Instantiate(resultSlot, gachaSlotParent);
      slot.Setup(data, aasetListTable, stringTable,isNew);
      spawnedSlots.Add(slot);
   }
   private void ClearGachaResults()
   {
      foreach (var slot in spawnedSlots)
      {
         Destroy(slot.gameObject);
      }
      spawnedSlots.Clear();
   }
}
