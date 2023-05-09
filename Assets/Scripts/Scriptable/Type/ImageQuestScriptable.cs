using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ImageQuestScriptable", menuName = "ScriptableObjects/ImageQuest", order = 1)]
public class ImageQuestScriptable : QuestionScriptable
{
    [SerializeField] private Sprite _sprite;

    public Sprite Sprite { get => _sprite; }
}
