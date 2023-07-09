using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Battle;
using DefaultNamespace.Player;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public BattleField BattleField;
    public List<Unit> Units;
    private Unit _currentUnitMove;

    private List<Unit> _unitTurnOrder;

    private void Start()
    {
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
    }

    private void NextUnitMove()
    {
        _currentUnitMove.Deactivate();
        var currentUnitIndex = _unitTurnOrder.IndexOf(_currentUnitMove);
        _currentUnitMove = currentUnitIndex + 1 < _unitTurnOrder.Count ? _unitTurnOrder[currentUnitIndex + 1] : _unitTurnOrder[0];
        _currentUnitMove.Activate();
    }
}