using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices; //need to call js

public class AdsScript : MonoBehaviour
{
    private float _noRewardAdsDelay = 185; //min 180(3 min) in yandex
    private static float _noRewardAdsTimer;
    private static bool _timerStarted;
    private static bool _additionalLevelActivated;

    public static bool AdditionalLevelActivated { get => _additionalLevelActivated; set => _additionalLevelActivated = value; }

    public enum Placements
    {
        AddLevel,
        Continue
    }


    [DllImport("__Internal")]
    private static extern void ShowRewardAd(string id); //call js from plugin UnityScriptToJS.jslib
    [DllImport("__Internal")]
    private static extern void ShowInterstitialAd(); //call js from plugin UnityScriptToJS.jslib
    private void Start()
    {
        if (_timerStarted == false)
        {
            _timerStarted = true;
            _noRewardAdsTimer = _noRewardAdsDelay;
        }
    }
    private void Update()
    {
        _noRewardAdsTimer -= Time.deltaTime;
    }
    private void ShowAd(Placements placement)
    {

        if (TestMode.Value == false)
        {
            ShowRewardAd(placement.ToString());
        }
        else
        {
            OnAdsReward(placement.ToString());
        }
    }
    public void ShowNonRewardAd()
    {
        if (_noRewardAdsTimer < 0)
        {
            if (TestMode.Value == false)
            {
                ShowInterstitialAd();
            }
            else
            {
                OnNonRewardAdsShowed();
            }
        }
    }
    public void OnNonRewardAdsShowed()
    {
        _noRewardAdsTimer = _noRewardAdsDelay;
    }
    public void OnAdsReward(string placement)
    {
        if (placement == Placements.AddLevel.ToString())
        {
            AdsLevelBtn[] levels = FindObjectsOfType<AdsLevelBtn>();
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i].SetAvailable(true);
            }
            _additionalLevelActivated = true;
            FindObjectOfType<SaveGame>().SaveProgress();
        }
        if (placement == Placements.Continue.ToString())
        {
            FindObjectOfType<EndGame>().Continue();
        }
    }
    public void ShowAdsForLevelActivate()
    {
        ShowAd(Placements.AddLevel);
    }
    public void ShowAdsForContinue()
    {
        ShowAd(Placements.Continue);
    }
    public void OnAdsError()
    {
        SetTimeScale(1);
    }
    public void OnAdsOpen()
    {

        SetTimeScale(0);
    }
    public void OnAdsClose()
    {
        SetTimeScale(1);
    }
    private void SetTimeScale(float val)
    {
        Time.timeScale = val;
        AudioListener.volume = val;
    }
}