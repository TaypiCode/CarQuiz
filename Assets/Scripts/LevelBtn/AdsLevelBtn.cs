using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsLevelBtn : LevelBtn
{
    private bool _activated;

    public bool Activated
    {
        get => _activated;
        set
        {
            _activated = value;
            SetAvailable(value);
        }

    }

    public void ShowAds()
    {
        FindObjectOfType<AdsScript>().ShowAdsForLevelActivate(this);
    }
}
