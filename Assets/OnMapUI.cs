using DefaultNamespace.Player;
using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class OnMapUI : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> _names;
    [SerializeField] private List<TMP_Text> _levels;
    [SerializeField] private List<TMP_Text> _exps;
    [SerializeField] private Slider _moralMarker;

    private Player _player;
    private View _view;    

    private void Start()
    {
        _view = Globals.Global.View;

        Globals.Global.Model.Plyer.Subscribe(player =>
        {
            _player = player;
            SetupInfo();
        }); 

        if (Globals.Global.Model.Plyer.Value != null && Globals.Global.Model.Plyer.Value.Party.Count > 0)
        {
            _player = Globals.Global.Model.Plyer.Value;
            SetupInfo();
        }
    }

    private void SetupInfo()
    {
        for (var i = 0; i < _player.Party.Count; i++)
        {
            _names[i].text = _player.Party[i].Name;
            _levels[i].text = $"AP: {_player.Party[i].Level}";
            _exps[i].text = $"Exp: {_player.Party[i].Exp}";            
        }
        

        _moralMarker.minValue = -5;
        _moralMarker.maxValue = 5;
        _moralMarker.value = _player.Moral;
    }

   
}
