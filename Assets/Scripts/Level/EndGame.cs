using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private ShowEffect _showEffect;
    [SerializeField] private GameObject _adsBtn;
    [SerializeField] private GameObject _reloadLevelBtn;
    public void ShowCanvas(bool isWin)
    {
        if (isWin)
        {
            _headerText.text = "Вы выиграли";
        }
        else
        {
            _headerText.text = "Вы проиграли";
        }
        _scoreText.text = "Вы получили " + GameData.ScoreReached + " очков";
        _canvas.SetActive(true);
        _showEffect.Show(0);
        _adsBtn.SetActive(!GameData.StartedLevelComplete);
        _reloadLevelBtn.SetActive(!isWin);
    }
    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ContinueForAds()
    {
        FindObjectOfType<AdsScript>().ShowAdsForContinue();
    }
    public void Continue()
    {
        FindObjectOfType<LevelManager>().AddTime();
        _canvas.SetActive(false);
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
