using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<LevelBtn> _levelBtns = new List<LevelBtn>();
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    [SerializeField] private GameObject _difficultCanvas;
    public void SetClearLevelBtns(bool additionLevelByPromocodeActivated =false, bool additionLevelByAdsActivated = false)
    {
        int[] score = new int[_levelBtns.Count];
        for(int i = 0; i < score.Length; i++)
        {
            score[i] = 0;
        }
        SetLevelBtns(score, additionLevelByPromocodeActivated, additionLevelByAdsActivated);
    }
    public void SetLevelBtns(int[] score, bool additionLevelByPromocodeActivated, bool additionLevelByAdsActivated) //if add new lvl btn then add in end of list to make save&load correct;
    {
        for(int i = 0; i < _levelBtns.Count; i++)
        {
            int lvlScore = 0;
            if (i < score.Length)
            {
                lvlScore = score[i];
            }
            _levelBtns[i].SetLevel(i, lvlScore);
            if (_levelBtns[i] is PromocodeLevelBtn)
            {
                if (additionLevelByPromocodeActivated)
                {
                    _levelBtns[i].SetAvailable(true);
                }
                else
                {
                    _levelBtns[i].SetAvailable(false);
                }
            }
            else if(_levelBtns[i] is AdsLevelBtn)
            {
                if (additionLevelByAdsActivated)
                {
                    _levelBtns[i].SetAvailable(true);
                }
                else
                {
                    _levelBtns[i].SetAvailable(false);
                }
            }
            else
            {
                _levelBtns[i].SetAvailable(true);
            }
        }
        UpdateTotalScoreText();
    }
    public int[] GetScoreByLevel()
    {
        int[] score = new int[_levelBtns.Count];
        for(int i = 0; i < score.Length; i++)
        {
            score[i] = _levelBtns[i].GetScore();
        }
        return score;
    }
    public void UpdateTotalScoreText()
    {
        int score = 0;
        int maxScore = 0;
        foreach (LevelBtn level in _levelBtns)
        {
            score += level.GetScore();
            maxScore += level.GetMaxScore();
        }
        _totalScoreText.text = "Всего очков: " + score + " / " + maxScore;
    }
    public void ShowDifficult()
    {
        _difficultCanvas.SetActive(true);
    }
    public void StartLevel(int difficultNum) //1-3
    {
        FindObjectOfType<SaveGame>().SaveProgress();
        GameData.SetLevelDifficult(difficultNum);
        SceneManager.LoadScene(1);
    }
}
