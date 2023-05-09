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
    [SerializeField] private GameObject _textQuestCanvas;
    [SerializeField] private GameObject _audioQuestCanvas;
    [SerializeField] private GameObject _imageQuestCanvas;
    [SerializeField] private TextMeshProUGUI _questText;
    [SerializeField] private AudioSource _questAudioSource;
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
        _questAudioSource.Stop();
        _currentQuestNumber++;
        if(_currentQuestNumber > _randomizedQuestions.Count)
        {
            Win();
            return;
        }
        _questHeaderText.text = "Задание "+_currentQuestNumber.ToString() +"/"+_randomizedQuestions.Count;
        int questId = _currentQuestNumber - 1;
        if(_randomizedQuestions[questId] is TextQuestScriptable)
        {
            _textQuestCanvas.SetActive(true);
            _audioQuestCanvas.SetActive(false);
            _imageQuestCanvas.SetActive(false);
            TextQuestScriptable quest = _randomizedQuestions[questId] as TextQuestScriptable;
            _questText.text = quest.Text;
        }
        else if (_randomizedQuestions[questId] is AudioQuestScriptable)
        {
            _textQuestCanvas.SetActive(false);
            _audioQuestCanvas.SetActive(true);
            _imageQuestCanvas.SetActive(false);
            AudioQuestScriptable quest = _randomizedQuestions[questId] as AudioQuestScriptable;
            _questAudioSource.clip = quest.Audio;
        }
        else if (_randomizedQuestions[questId] is ImageQuestScriptable)
        {
            _textQuestCanvas.SetActive(false);
            _audioQuestCanvas.SetActive(false);
            _imageQuestCanvas.SetActive(true);
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
        for (int i = 0; i < _randomizedQuestions[questId].Answer.Length; i++)
        {
            ids.Add(i);
        }
        for (int i = 0; i < _randomizedQuestions[questId].Answer.Length; i++)
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
            btn.GetComponentInChildren<TextMeshProUGUI>().text = _randomizedQuestions[questId].Answer[randomizedAnswersId[i]];
            bool isRight = randomizedAnswersId[i] == _randomizedQuestions[questId].RightAnswerId ? true : false;
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
    public void PlayAudio()
    {
        _questAudioSource.Play();
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
