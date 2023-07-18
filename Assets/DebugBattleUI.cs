using System.Collections.Generic;
using DefaultNamespace;
using RedBjorn.ProtoTiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugBattleUI : MonoBehaviour
{
    public TMP_Text Round;
    public TMP_Text UnitName;
    public TMP_Text AP;
    public Button Prefab; 
    public Button Restart; 
    public Button RunTestField1; 
    public Button RunTestField2; 
    public Button RunTestField3; 
   
    public MapSettings Field1;
    public MapSettings Field2;
    public MapSettings Field3;
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
        
        RunTestField1.onClick.AddListener(() => {_view.ActiveBattle.Value = Field1;});
        RunTestField2.onClick.AddListener(() => {_view.ActiveBattle.Value = Field2;});
        RunTestField3.onClick.AddListener(() => {_view.ActiveBattle.Value = Field3;});
    }
}