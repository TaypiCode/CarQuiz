using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _questHeaderText;
    [SerializeField] private TimeSlider _timeSlider;
    [SerializeField] private EndGame _endGame;

    [Header("Answer")]
    [SerializeField] private GameObject _answerBtnPrefub;
    [SerializeField] private Transform _answerBtnsListSpawn;
    [SerializeField] private float _answerShowDelay;

    [Header("Quest")]
    [SerializeField] private GameObject _imageQuestCanvas;
    [SerializeField] private Image _questImage;

    private List<QuestionScriptable> _randomizedQuestions = new List<QuestionScriptable>();
    private List<GameObject> _answerBtns = new List<GameObject>();
    private Timer _levelTimer;
    private int _currentQuestNumber = 0;
    private void Start()
    {
        _levelTimer = gameObject.AddComponent<Timer>();
        ResetTimer();
        RandomizeQuests();
    }
    private void Update()
    {
        if (_levelTimer != null)
        {
            float time = _levelTimer.GetTime();
            if (time > 0)
            {
                _timeSlider.UpdateSlider(time);
            }
            else
            {
                _timeSlider.UpdateSlider(0);
                Loose();
            }
        }
    }
    private void RandomizeQuests()
    {
        _randomizedQuestions.Clear();
        List<QuestionScriptable> quests = new List<QuestionScriptable>();
        quests.AddRange(GameData.Questions);
        for(int i = 0; i < GameData.Questions.Count; i++)
        {
            int r = Random.Range(0, quests.Count);
            QuestionScriptable quest = quests[r];
            quests.Remove(quest);
            _randomizedQuestions.Add(quest);
        }
        _currentQuestNumber = 0;
        CreateQuest();
    }
    private void CreateQuest()
    {
        _currentQuestNumber++;
        if(_currentQuestNumber > _randomizedQuestions.Count)
        {
            Win();
            return;
        }
        _questHeaderText.text = "Задание "+_currentQuestNumber.ToString() +"/"+_randomizedQuestions.Count;
        int questId = _currentQuestNumber - 1;
        if (_randomizedQuestions[questId] is ImageQuestScriptable)
        {
            ImageQuestScriptable quest = _randomizedQuestions[questId] as ImageQuestScriptable;
            _questImage.sprite = quest.Sprite;
            _questImage.preserveAspect = true;
        }
        CreateRandomizedAnswers(questId);
    }
    private void CreateRandomizedAnswers(int questId)
    {
        for (int i = 0; i < _answerBtns.Count; i++)
        {
            Destroy(_answerBtns[i]);
        }
        _answerBtns.Clear();
        List<int> ids = new List<int>();
        List<int> randomizedAnswersId = new List<int>();
        List<QuestionScriptable> availableAnswers = new List<QuestionScriptable>();
        availableAnswers.AddRange(GameData.Questions);
        QuestionScriptable currentCar = _randomizedQuestions[questId];
        availableAnswers.Remove(currentCar);
        List<QuestionScriptable> randomizedAnswers = new List<QuestionScriptable>();
        randomizedAnswers.Add(currentCar);
        for (int i = 0; i < 3; i++)
        {
            if (i < availableAnswers.Count)
            {
                int r = Random.Range(0, availableAnswers.Count);
                randomizedAnswers.Add(availableAnswers[r]);
                availableAnswers.Remove(availableAnswers[r]);
            }
        }
        for (int i = 0; i < randomizedAnswers.Count; i++)
        {
            ids.Add(i);
        }
        for (int i = 0; i < randomizedAnswers.Count; i++)
        {
            int r = Random.Range(0, ids.Count);
            int id = ids[r];
            ids.Remove(id);
            randomizedAnswersId.Add(id);
        }
        for (int i = 0; i < randomizedAnswersId.Count; i++)
        {
            GameObject btnObj = Instantiate(_answerBtnPrefub, _answerBtnsListSpawn);
            Button btn = btnObj.GetComponent<Button>();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = randomizedAnswers[randomizedAnswersId[i]].Answer;
            bool isRight = randomizedAnswers[randomizedAnswersId[i]] == currentCar ? true : false;
            btn.onClick.AddListener(delegate { Answer(isRight); });
            _answerBtns.Add(btnObj);
            btnObj.GetComponent<ShowEffect>().Show(_answerShowDelay * i);
        }
    }
    private void Answer(bool isRightAnswer)
    {
        if (isRightAnswer)
        {
            CreateQuest();
        }
        else
        {
            Loose();
        }
    }
    private void ResetTimer()
    {
        float levelTime = GameData.GetLevelTime();
        _levelTimer.SetTimer(null, levelTime);
        _timeSlider.SetSlider(levelTime);
    }
    private void Loose()
    {
        GameData.SetCompleteLevel(false, _levelTimer.GetTime());
        _endGame.ShowCanvas(false);
    }
    private void Win()
    {
        GameData.SetCompleteLevel(true, _levelTimer.GetTime());
        _endGame.ShowCanvas(true);
    }
    public void AddTime()
    {
        float maxTime = GameData.GetLevelTime();
        float time = _levelTimer.GetTime() + GameData.GetLevelTime() * .5f;
        if (time > maxTime)
        {
            time = maxTime;
        }
        _levelTimer.SetTimer(null, time);
    }
}
