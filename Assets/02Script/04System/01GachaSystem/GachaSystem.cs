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
            if (!GameManager.instance.ObtainedGachaIDs.Contains(result.Id))
            {
               GameManager.instance.ObtainedGachaIDs.Add(result.Id);
               Logger.Log($"Obtained Gacha ID: {result.Id}");
               GameManager.instance.SaveGameData();
            }
            else
            {
               switch (gachaTable.table[result.Id].Grade)
               {
                  case 0:
                     GameManager.instance.Mileage += 1;
                     break;
                  case 1:
                     GameManager.instance.Mileage += 5;
                     break;
                  case 2:
                     GameManager.instance.Mileage += 10;
                     break;   
               } 
               GameManager.instance.SaveGameData();
            }
            SpawnResultSlot(result);
         }
      }
   }
   
   private GachaData GetRandomGachaResult()
   {
      float rand = UnityEngine.Random.value * 100f;
      int grade;

      if (rand <= 3f)
      {
         grade = 2;
      }
      else if (rand <= 23f)
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
   private void SpawnResultSlot(GachaData data)
   {
      var slot = Instantiate(resultSlot, gachaSlotParent);
      slot.Setup(data, aasetListTable, stringTable);
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
