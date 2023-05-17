using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
   Image healthSlider;

   private void Start() {
    healthSlider = GetComponent<Image>();
   }


   public void SetHealth(int health)
   {
        healthSlider.fillAmount = ((float)health/100f);
   }
}
