using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth
{
   // Fields
   int currentHealth;
   int currentMaxHealth;

   // Properties
   public int Health()
   {
        get
        { return currentHealth;} 
        set 
        {currentHealth = value;}
   }

}
