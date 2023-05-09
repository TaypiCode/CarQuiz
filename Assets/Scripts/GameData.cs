using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static readonly int[] _scorePerSecByLevelDifficult = new int[3] { 1, 4, 16 };
    private static readonly int[] _timeByLevelDifficult = new int[3] { 120, 60, 30 };
    private static int _currentLevelDifficult; //1-3 easy norm hard
    private static int _launchedLevelId;
    private static int _scoreReached;
    private static List<QuestionScriptable> _questions =  new List<QuestionScriptable>();
    private static int _completedLevels;
    private static bool _startedLevelComplete;

    public static int LaunchedLevelId { get => _launchedLevelId; }
    public static int ScoreReached { get => _scoreReached;  }
    public static int CurrentLevelDifficult { get => _currentLevelDifficult;  }
    public static int CompletedLevels { get => _completedLevels; }
    public static bool StartedLevelComplete { get => _startedLevelComplete;
    }
    public static List<QuestionScriptable> Questions { get => _questions;  }

    public static void SetLevelDifficult(int levelDifficult)
    {
        _currentLevelDifficult = levelDifficult;
    }
    public static void SetChoosedLevel(int id, List<QuestionScriptable> questions)
    {
        _questions.Clear();
        _questions.AddRange(questions);
        _launchedLevelId = id;
        _startedLevelComplete = false;
    }
    public static int GetMaxScore(int questionCount)
    {
        return _scorePerSecByLevelDifficult[_scorePerSecByLevelDifficult.Length - 1] * _timeByLevelDifficult[_timeByLevelDifficult.Length - 1] * questionCount;
    }
    public static int GetLevelTime()
    {
        return GetLevelTime(_currentLevelDifficult);
    }
    public static int GetLevelTime(int levelDifficult)
    {
        return _timeByLevelDifficult[levelDifficult - 1];
    }
    public static int CalculateScore(float timeLeft, int questionCount)
    {
        return CalculateScore(timeLeft, questionCount, _currentLevelDifficult);
    }
    public static int CalculateScore(float timeLeft, int questionCount, int levelDifficult)
    {
        int time = Mathf.CeilToInt(timeLeft);
        return _scorePerSecByLevelDifficult[levelDifficult - 1] * time * questionCount;
    }
    public static void SetCompleteLevel(bool playerWin, float timeLeft)
    {
        _startedLevelComplete = playerWin;
        if (playerWin)
        {
            _completedLevels++;
            _scoreReached = CalculateScore(timeLeft, _questions.Count);
        }
        else
        {
            _scoreReached = 0;
        }
    }
}
