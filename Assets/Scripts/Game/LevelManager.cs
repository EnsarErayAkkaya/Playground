using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
   // Level geçildiğinide ödülleri verecek oyun sonu ekranını tetikleyecek
   public RailType[] levelRailPrize;
   
   void OnTriggerEnter(Collider other)
   {
       if(other.tag == "Train")
       {
           Debug.Log("Level finished - train on tunnel");
           GivePrizes();
       }
   }
   void GivePrizes()
   {
        foreach (RailType item in levelRailPrize)
        {
            GameDataManager.instance.AddNewPlayerRail(item);
        }
   }
}
