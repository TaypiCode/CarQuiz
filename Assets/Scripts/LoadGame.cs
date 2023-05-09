using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private Promocode _promocode; 
    private Save _save;
    private static bool _firstLoad = true;
    private static bool _isMobile;

    public static bool IsMobile { get => _isMobile;  }

    [DllImport("__Internal")]
    private static extern void FirstLoadInSession(); //call js from plugin UnityScriptToJS.jslib

    private void Awake()
    {
        if (PlayerPrefs.HasKey("SV"))
        {
            _save = new Save();
            _save = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("SV"));
        }
        if (!_firstLoad)
        {
            LoadData();
        }

    }
    private void Start()
    {
        if (_firstLoad)
        {
            if (TestMode.Value == true)
            {
                LoadData();
            }
            else
            {
                FirstLoadInSession();
            }
        }
    }
    public void LoadFromYandex(string data)
    {
        _save = new Save();
        _save = JsonUtility.FromJson<Save>(data);
        FindObjectOfType<SaveGame>().SaveJson(_save);
        LoadData();
    }

    public void LoadData()
    {
        if (_save != null)
        {
            if (GameData.StartedLevelComplete)
            {
                if (_save.score[GameData.LaunchedLevelId] < GameData.ScoreReached)
                {
                    _save.score[GameData.LaunchedLevelId] = GameData.ScoreReached;
                }
            }
            print(GameData.ScoreReached);
            _promocode.FillActivatedPromocodes(_save.activatedPromocodes);
            AdsScript.AdditionalLevelActivated = _save.additionalLevelsByAdsIsAvailable;
            _mainMenu.SetLevelBtns(_save.score, _save.additionalLevelsByPromocodeIsAvailable, _save.additionalLevelsByAdsIsAvailable);
        }
        else//no save
        {
            _mainMenu.SetClearLevelBtns();
        }
        _firstLoad = false;
        if(GameData.CompletedLevels % 2 == 0 && GameData.CompletedLevels > 0)
        {
            RateUsScript.ShowRateUs();
        }
        FindObjectOfType<AdsScript>().ShowNonRewardAd();
    }
    public void SetIsMobile()
    {
        _isMobile = true;
    }
}
