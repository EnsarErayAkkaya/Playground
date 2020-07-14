using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
   // Level geçildiğinide ödülleri verecek oyun sonu ekranını tetikleyecek
   public RailType[] levelRailPrize;
   
   // geçici
   void Update()
   {
       if(Input.GetMouseButtonDown(0))
       {
           foreach (RailType item in levelRailPrize)
           {
               GameDataManager.instance.AddNewPlayerRail(item);
           }
           SaveAndLoadGameData.instance.Save();
       }
   }
}
