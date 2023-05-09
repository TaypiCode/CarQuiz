using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromocodeLevelBtn : LevelBtn
{
    public void ShowPromocode()
    {
        FindObjectOfType<Promocode>().ShowCanvas();
    }
    
}
