using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AudioQuestScriptable", menuName = "ScriptableObjects/AudioQuest", order = 1)]
public class AudioQuestScriptable : QuestionScriptable
{
    [SerializeField] private AudioClip _audio;

    public AudioClip Audio { get => _audio; }
}
