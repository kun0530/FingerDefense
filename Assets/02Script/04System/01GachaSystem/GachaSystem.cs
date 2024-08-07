using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GachaSystem : MonoBehaviour
{
   public RectTransform gachaSlotParent;
   public GachaResultSlot resultSlot;
   
   private GachaTable gachaTable;
   private AssetListTable aasetListTable;
   private StringTable stringTable;
   
   private void Start()
   {
      gachaTable = DataTableManager.Get<GachaTable>(DataTableIds.Gacha);
      aasetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
      stringTable = DataTableManager.Get<StringTable>(DataTableIds.String);
   }


   public void PerformGacha(int i)
   {
      
   }
}
