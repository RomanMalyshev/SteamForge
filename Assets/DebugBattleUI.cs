using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Player;
using Game.Battle;
using RedBjorn.ProtoTiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugBattleUI : MonoBehaviour
{
    public TMP_Text Round;
    public TMP_Text UnitName;
    public TMP_Text AP;

    public TMP_Text Side;

    public Button Prefab;
    public Button Restart;
    public Button RunTestField1;
    public Button RunTestField2;
    public Button RunTestField3;

    public MapSettings Field1;
    public MapSettings Field2;
    public MapSettings Field3;
    public Transform ButtonsParents;

    [Header("Battle line")]
    public UIUnitLine UnitLinePrefab;
    public Transform Content;


    private Dictionary<Unit,UIUnitLine> _unitsLine = new();

    private Model _model;
    private View _view;
    private readonly List<Button> _unitCommands = new();

    public void Init()
    {
        _model = Globals.Global.Model;
        _view = Globals.Global.View;

        _model.OnNewBattleRound.Subscribe(round => { Round.text = $"Round: {round}"; });

        _model.OnUnitStartTern.Subscribe((unit, commands) =>
        {
            foreach (var unitCommand in _unitCommands)
                Destroy(unitCommand.gameObject);

            _unitCommands.Clear();

            UnitName.text = $"{unit.gameObject.name}";
            Side.text = unit.UnitSide.ToString();

            if (unit.UnitSide != UnitSide.Player) return;

            foreach (var command in commands)
            {
                var button = Instantiate(Prefab, ButtonsParents);
                _unitCommands.Add(button);
                button.onClick.AddListener(() => { _view.OnCommandSelect.Invoke(command); });
            }
        });

        _model.UnitBattleOrder.Subscribe(units =>
        {
            for (var i = 0; i < units.Count; i++)
            {
                var unitLine = Instantiate(UnitLinePrefab, Content);
                var unit = units[i];


                unitLine.Button.onClick.AddListener(() => _view.OnUnitInBattleSelect.Invoke(unit));
                unitLine.Background.color = unit.UnitSide == UnitSide.Player ? Color.green : Color.red;
                unitLine.Label.text = unit.UnitSide.ToString();
                _unitsLine.Add(unit,unitLine);

                unit.OnUnitDead += deadUnit =>
                {
                    _unitsLine.Remove(deadUnit);
                    Destroy(unitLine.gameObject);
                };
            }
        });
        
        
        _model.OnUnitEndTern.Subscribe(unit =>
        {
            if (_unitsLine.TryGetValue(unit, out var value))
            {
                Destroy(value.gameObject);
                _unitsLine.Remove(unit);
            }
            else
                Debug.LogWarning("Unit has not line prefab on battle UI!");
        });

        _model.OnChangeUnitActionPoints.Subscribe(ap => { AP.text = $"AP: {ap}"; });

        Restart.onClick.AddListener(() => { _view.OnRestartBattle.Invoke(); });

        RunTestField1.onClick.AddListener(() => { _view.ActiveBattle.Value = Field1; });
        RunTestField2.onClick.AddListener(() => { _view.ActiveBattle.Value = Field2; });
        RunTestField3.onClick.AddListener(() => { _view.ActiveBattle.Value = Field3; });
    }

    public void Reset()
    {
        foreach (var unitsLine in _unitsLine)
        {
            Destroy(unitsLine.Value.gameObject);
        }

        _unitsLine.Clear();
    }
}