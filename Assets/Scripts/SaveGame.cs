using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private MainMenu _menu;
    [SerializeField] private Promocode _promocode;
    private Save save = new Save();

    [DllImport("__Internal")]
    private static extern void SaveData(string data); //call js from plugin UnityScriptToJS.jslib

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(){
        SaveProgress();
    }
#endif
    private void OnApplicationQuit()
    {
        SaveProgress();
    }

    public void SaveProgress()
    {
        CreateSaveData();
        SavePlayerPrefs();
        if (TestMode.Value == false)
        {
            SaveInYandex();
        }
        
    }
    private void CreateSaveData()
    {
        save.score = _menu.GetScoreByLevel();
        save.additionalLevelsByPromocodeIsAvailable = _promocode.AdditionalLevelsPromocodeActivated;
        List<bool> adsLevelBtnActivatedStatus = new List<bool>();
        AdsLevelBtn[] adsLevelBtns = FindObjectsOfType<AdsLevelBtn>();
        foreach(AdsLevelBtn btn in adsLevelBtns)
        {
            adsLevelBtnActivatedStatus.Add(btn.Activated);
        }
        save.additionalLevelsByAdsIsAvailable = adsLevelBtnActivatedStatus.ToArray();
        save.activatedPromocodes = _promocode.GetActivatedPromocodes();

        int totalScore = save.score.Sum();
        LeaderboardScript.SetLeaderboardValue(LeaderboardScript.Names.Score, totalScore);
    }
    private void SavePlayerPrefs()
    {
        SaveJson(save);
    }
    private void SaveInYandex()
    {
        SaveData(JsonUtility.ToJson(save));
    }
    public void SaveJson(Save save)
    {
        PlayerPrefs.SetString("SV", JsonUtility.ToJson(save));
        PlayerPrefs.Save();
    }
}
[Serializable]
public class Save
{
    public int[] score;
    public string[] activatedPromocodes;
    public bool additionalLevelsByPromocodeIsAvailable;
    public bool[] additionalLevelsByAdsIsAvailable;
}