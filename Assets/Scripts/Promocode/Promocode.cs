using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Promocode : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private TextMeshProUGUI _answerText;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameObject _mobileInputs;
    [SerializeField] private FlexibleGridLayout _mobileInputsGrid;
    [SerializeField] private List<PromocodeScriptable> _promocodes;
    [SerializeField] private PromocodeScriptable _additionalLevelsPromocode;

    private string _enteredCodeText;
    private List<string> _activatedPromocodes = new List<string>();
    private PromocodeScriptable _enteredPromocode;
    private bool _activatedPerSession = false;
    private bool _additionalLevelsPromocodeActivated;

    public bool AdditionalLevelsPromocodeActivated { get => _additionalLevelsPromocodeActivated; }

    public void FillActivatedPromocodes(string[] list)
    {
        _activatedPromocodes.AddRange(list);
        foreach(string code in _activatedPromocodes)
        {
            if(code == _additionalLevelsPromocode.itemId)
            {
                _additionalLevelsPromocodeActivated = true;
                break;
            }
        }
    }
    public void ShowCanvas()
    {
        _canvas.SetActive(true);
        _enteredCodeText = "";
        _answerText.text = "";
        HideMobileInputs();
    }
    public void TryAcceptPromocode()
    {        
        if (GetEnteredPromocode())
        {
            if (CanActivate() && _activatedPerSession == false)
            {
                Reward();
                _activatedPromocodes.Add(_enteredPromocode.itemId);
                _activatedPerSession = true;
                FindObjectOfType<SaveGame>().SaveProgress();
            }
            else
            {
                _answerText.text = "Промокод уже активирован";
            }
        }
        else
        {
            _answerText.text = "Не верный промокод";
        }
    }
    private void Reward()
    {
        if (_additionalLevelsPromocode == _enteredPromocode)
        {
            PromocodeLevelBtn[] levels = FindObjectsOfType<PromocodeLevelBtn>();
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i].SetAvailable(true);
            }
            _additionalLevelsPromocodeActivated = true;
            _answerText.text = "Открыт дополнительный уровень";
            FindObjectOfType<SaveGame>().SaveProgress();
        }
    }
    private bool GetEnteredPromocode()
    {

        for (int i = 0; i < _promocodes.Count; i++)
        {
            if (_promocodes[i].Code == _enteredCodeText)
            {
                _enteredPromocode = _promocodes[i];
                return true;
            }
        }
        return false;
    }
    private bool CanActivate()
    {
        for (int i = 0; i < _activatedPromocodes.Count; i++)
        {
            if (_activatedPromocodes[i] == _enteredPromocode.itemId)
            {
                return false;
            }
        }
        return true;
    }
    public void FillEnterCodeText(string s)
    {
        _enteredCodeText = s;
    }
    public string[] GetActivatedPromocodes()
    {
        return _activatedPromocodes.ToArray();
    }
    public void ShowMobileInputs()
    {
        _mobileInputs.SetActive(true);
        _mobileInputsGrid.Reload(FlexibleGridLayout.FitType.FixedRows, 4);
    }
    public void HideMobileInputs()
    {
        _mobileInputs.SetActive(false);
    }
    public void AddLetter(string val)
    {
        _inputField.text += val;
    }
    public void RemoveLetter()
    {
        if (_inputField.text.Length > 0)
        {
            _inputField.text = _inputField.text.Remove(_inputField.text.Length - 1);
        }
    }
}
