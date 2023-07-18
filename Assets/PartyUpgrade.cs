using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PartyUpgrade : MonoBehaviour
{
    private Player _player;

    public TMP_Text PartyStats;
    public Button AllPArtyUpgrade;
    public Button Confirm;
    public Button Cancel;

    private int _preUpgradePoints;
    
    private void Start()
    {
        Globals.Global.Model.Plyer.Subscribe(player =>
        {
            _player = player;
            SetupParty();
        });

        if (Globals.Global.Model.Plyer.Value != null && Globals.Global.Model.Plyer.Value.Party.Count > 0)
        {
            _player = Globals.Global.Model.Plyer.Value;
            SetupParty();
        }


        AllPArtyUpgrade.onClick.AddListener(UpgradeMyParty);
        Confirm.onClick.AddListener(OnConfirm);
        Cancel.onClick.AddListener(OnCancelButton);
    }

    private void OnCancelButton()
    {
        
        Globals.Global.Model.Plyer.Value = _player;
    }

    private void OnConfirm()
    {
        Globals.Global.Model.Plyer.Value = _player;
    }


    private void UpgradeMyParty()
    {
        if (_player == null) return;
        if (_player.Party.Count == 0) return;
        
        
        _player.Party[0].Balance.Value += _player.Party[0].UpPoints;
        _player.Party[0].UpPoints = 0;

        SetupParty();
    }

    private void SetupParty()
    {
        PartyStats.text = "";
        for (var i = 0; i < _player.Party.Count; i++)
        {
            PartyStats.text +=
                $"Name: {_player.Party[i].Name} Level:{_player.Party[i].Level} AP: {_player.Party[i].UpPoints} Balance: {_player.Party[i].Balance.Value}\n";
        }
    }
}