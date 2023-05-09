using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsLevelBtn : LevelBtn
{
    public void ShowAds()
    {
        FindObjectOfType<AdsScript>().ShowAdsForLevelActivate();
    }
}
