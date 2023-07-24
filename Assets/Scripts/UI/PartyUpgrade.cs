using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyUpgrade : MonoBehaviour
{
    [SerializeField] private List<AttributeLine> _attributeLines;
    [SerializeField] private List<StatLine> _statLines;

    [SerializeField] private TMP_Text _characterAP;
    [SerializeField] private TMP_Text _characterName;

    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _nextCaracterButton;
    [SerializeField] private Button _previosCaracterButton;
    [SerializeField] private Button _returnToMapButton;


    private Player _player;

    private int _currentCharacter;
    private PreUpgradePlayer _preUpgradePlayer = new();

    private void Start()
    {
        Globals.Global.Model.Plyer.Subscribe(player =>
        {
            _player = player;
            SetupParty();
        });
        
        _returnToMapButton.onClick.AddListener(() => Globals.Global.View.ReturnToMap.Invoke());

        if (Globals.Global.Model.Plyer.Value != null && Globals.Global.Model.Plyer.Value.Party.Count > 0)
        {
            _player = Globals.Global.Model.Plyer.Value;
            SetupParty();
        }

        for (var i = 0; i < _attributeLines.Count; i++)
        {
            var a = i;
            _attributeLines[i]._upperAttributeButton.onClick.AddListener(() => RaiseAttribute(a));
            _attributeLines[i]._lowerAttributeButton.onClick.AddListener(() => LowAttribute(a));
        }

        for (var i = 0; i < _player.Party.Count; i++)
        {
            _preUpgradePlayer._upPoints.Add(i);
            PreUpgradeAttributes n = new();
            _preUpgradePlayer._characterAtributes.Add(n);
            var attributes = _player.Party[i].GetAllAttributes();

            for (var a = 0; a < attributes.Count; a++)
            {
                _preUpgradePlayer._characterAtributes[i]._atributes.Add(a);
            }
        }

        _applyButton.onClick.AddListener(ApplyLevelUp);
        _cancelButton.onClick.AddListener(CancelLevelUp);
        _nextCaracterButton.onClick.AddListener(NextCharacter);
        _previosCaracterButton.onClick.AddListener(PreviosCharacter);

        _currentCharacter = 0;

        StartingLevelUping();
    }

    private void CancelLevelUp()
    {
        _player.Party[_currentCharacter].UpPoints = _preUpgradePlayer._upPoints[_currentCharacter];

        for (var i = 0; i < _preUpgradePlayer._characterAtributes[_currentCharacter]._atributes.Count; i++)
        {
            var atribute = _player.Party[_currentCharacter].GetAllAttributes()[i];
            atribute.Value = _preUpgradePlayer._characterAtributes[_currentCharacter]._atributes[i];
        }

        SetupParty();
    }

    private void ApplyLevelUp()
    {
        _preUpgradePlayer._upPoints[_currentCharacter] = _player.Party[_currentCharacter].UpPoints;

        var stats = _player.Party[_currentCharacter].GetAllAttributes();

        for (var i = 0; i < stats.Count; i++)
        {
            _preUpgradePlayer._characterAtributes[_currentCharacter]._atributes[i] = stats[i].Value;
        }

        SetupParty();
    }

    private void RaiseAttribute(int attributeIndex)
    {
        if (_player == null) return;
        if (_player.Party.Count == 0) return;
        if (_player.Party[_currentCharacter].UpPoints <= 0) return;

        var attribute = _player.Party[_currentCharacter].GetAllAttributes()[attributeIndex];
        attribute.Value++;
        _player.Party[_currentCharacter].UpPoints--;

        SetupParty();
    }

    private void LowAttribute(int attributeIndex)
    {
        if (_player == null) return;
        if (_player.Party.Count == 0) return;

        var attribute = _player.Party[_currentCharacter].GetAllAttributes()[attributeIndex];

        if (attribute.Value <= _preUpgradePlayer._characterAtributes[_currentCharacter]._atributes[attributeIndex]) return;
        attribute.Value--;
        _player.Party[_currentCharacter].UpPoints++;

        SetupParty();
    }

    private void NextCharacter()
    {
        CancelLevelUp();

        if (_currentCharacter == _player.Party.Count - 1)
        {
            _currentCharacter = 0;
        }
        else
        {
            _currentCharacter++;
        }

        SetupParty();
    }

    private void PreviosCharacter()
    {
        CancelLevelUp();

        if (_currentCharacter == 0)
        {
            _currentCharacter = _player.Party.Count - 1;
        }
        else
        {
            _currentCharacter--;
        }

        SetupParty();
    }

    private void SetupParty()
    {
        _characterName.text = _player.Party[_currentCharacter].Name;
        _characterAP.text = $"AP: {_player.Party[_currentCharacter].UpPoints}";

        var stats = _player.Party[_currentCharacter].GetAllStats();
        var atributes = _player.Party[_currentCharacter].GetAllAttributes();

        for (var i = 0; i < stats.Count; i++)
        {
            _statLines[i]._statName.text = stats[i].Name;
            _statLines[i]._statValue.text = stats[i].Value.ToString();
        }

        for (var i = 0; i < atributes.Count; i++)
        {
            _attributeLines[i]._attributeName.text = atributes[i].Name;
            _attributeLines[i]._attributeValue.text = atributes[i].Value.ToString();
        }
    }
    
    public void StartingLevelUping()
    {
        for (var i = 0; i < _player.Party.Count; i++)
        {
            _preUpgradePlayer._upPoints[i] = _player.Party[i].UpPoints;

            var attributes = _player.Party[i].GetAllAttributes();

            for (var a = 0; a < attributes.Count; a++)
            {
                _preUpgradePlayer._characterAtributes[i]._atributes[a] = attributes[a].Value;
            }
        }
    }
}

    [Serializable]
    public class AttributeLine
    {
        public TMP_Text _attributeName;
        public TMP_Text _attributeValue;
        public Button _upperAttributeButton;
        public Button _lowerAttributeButton;

    }

    [Serializable]
    public class StatLine
    {
        public TMP_Text _statName;
        public TMP_Text _statValue;
    }

    [Serializable]
    public class PreUpgradePlayer
    {
        public List<PreUpgradeAttributes> _characterAtributes = new();
        public List<int> _upPoints = new();
    }

    [Serializable]
    public class PreUpgradeAttributes
    {
        public List<int> _atributes = new();

    }
