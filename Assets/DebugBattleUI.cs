using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugBattleUI : MonoBehaviour
{
    public TMP_Text Round;
    public TMP_Text UnitName;
    public TMP_Text AP;
    public Button Move;
    public Button Attack;

    private Model _model;
    private void Start()
    {
        _model = Globals.Global.Model;
        
        _model.OnNewBattleRound.Subscribe(round => { Round.text = $"Round: {round}"; });
        _model.OnNewUnitTern.Subscribe(unitName => { UnitName.text = $"{unitName}"; });
        _model.OnChangeUnitActionPoints.Subscribe(ap => { AP.text = $"AP: {ap}"; });

    }
}