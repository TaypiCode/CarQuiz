using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TextQuestScriptable", menuName = "ScriptableObjects/TextQuest", order = 1)]
public class TextQuestScriptable : QuestionScriptable
{
    [SerializeField] private string _text;

    public string Text { get => _text; }
}
