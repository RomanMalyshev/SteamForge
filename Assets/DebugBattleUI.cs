using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DebugBattleUI : MonoBehaviour
{
    public TMP_Text Round;
    public TMP_Text UnitName;
    public TMP_Text AP;
    public Button Prefab; 
    public Button Restart; 
    public Transform ButtonsParents;
    
    private Model _model;
    private View _view;
    private readonly List<Button> _unitCommands = new();
    
    public void Init()
    {
        _model = Globals.Global.Model;
        _view = Globals.Global.View;
        _model.OnNewBattleRound.Subscribe(round => { Round.text = $"Round: {round}"; });
        _model.OnNewUnitTern.Subscribe((unitName,commands )=>
        {
            //TODO:remake on pool
            foreach (var unitCommand in _unitCommands)
                Destroy(unitCommand.gameObject);
            
            _unitCommands.Clear();
            foreach (var command in commands)
            {
                var button = Instantiate(Prefab, ButtonsParents);
                _unitCommands.Add(button);
                button.onClick.AddListener(() => {_view.OnCommandSelect.Invoke(command); });
            }
            
            UnitName.text = $"{unitName}";
        });
        _model.OnChangeUnitActionPoints.Subscribe(ap => { AP.text = $"AP: {ap}"; });

        Restart.onClick.AddListener(() => { _view.OnRestartBattle.Invoke(); });
    }
}