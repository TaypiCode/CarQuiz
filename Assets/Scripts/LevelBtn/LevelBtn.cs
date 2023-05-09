using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelBtn : MonoBehaviour
{
    [SerializeField] private List<QuestionScriptable> _questions = new List<QuestionScriptable>();
    [SerializeField] private GameObject _availableBtn;
    [SerializeField] private GameObject _unavailableBtn;
    [SerializeField] private TextMeshProUGUI _scoreText;
    private int _idInMenu;
    private int _score;
    private bool _isAvailable;
    public void StartLevel()
    {
        if (_isAvailable)
        {
            GameData.SetChoosedLevel(_idInMenu, _questions);
            FindObjectOfType<MainMenu>().ShowDifficult();
        }
    }
    public void SetAvailable(bool value)
    {
        _isAvailable = value;
        if (_unavailableBtn != null)
        {
            _unavailableBtn.SetActive(!_isAvailable);
        }
        _availableBtn.SetActive(_isAvailable);
    }
    public void SetLevel(int id, int score)
    {
        _idInMenu = id;
        _score = score;
        _scoreText.text = _score + "/" + GetMaxScore();
    }
    public int GetScore()
    {
        return _score;
    }
    public int GetMaxScore()
    {
        return GameData.GetMaxScore(_questions.Count);
    }
}
