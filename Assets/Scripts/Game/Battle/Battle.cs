using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Battle;
using DefaultNamespace.Player;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public BattleField BattleField;
    public List<Unit> Units;
    private Unit _currentUnitMove;

    private List<Unit> _unitTurnOrder;
    private Model _model;
    private int _round;

    private void Start()
    {
        _model = Globals.Global.Model;
        BattleField.OnFieldReady += InitUnits;
        BattleField.TestInitField();
    }

    private void InitUnits()
    {
        if (Units.Count == 0)
        {
            Debug.LogWarning("No unit on field!");
            return;
        }

        foreach (var unit in Units)
        {
            unit.Init(BattleField.FieldEntity);
            unit.Deactivate();
            unit.OnActionPointsEnd += NextUnitMove;
        }

        _unitTurnOrder = Units.OrderBy(unit => unit.InitiativeTest).ToList();
        _currentUnitMove = _unitTurnOrder[0];
        _currentUnitMove.Activate();
        _round++;
        _model.OnNewBattleRound.Invoke(_round);
    }

    private void NextUnitMove()
    {
        _currentUnitMove.Deactivate();
        var currentUnitIndex = _unitTurnOrder.IndexOf(_currentUnitMove);
        var nexUnitIndex = currentUnitIndex + 1 < _unitTurnOrder.Count
            ? currentUnitIndex + 1
            : 0;
        _currentUnitMove = _unitTurnOrder[nexUnitIndex];
        _currentUnitMove.Activate();

        if (nexUnitIndex == 0)
        {
            _round++;
            _model.OnNewBattleRound.Invoke(_round);
        }
    }
}