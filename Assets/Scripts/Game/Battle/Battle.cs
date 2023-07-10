using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Unit = DefaultNamespace.Player.Unit;

namespace Game.Battle.Skills
{
    public class Battle : MonoBehaviour
    {
        public DebugBattleUI DebugBattleUI;
        public BattleField BattleField;
        public List<Unit> Units;
        private Unit _currentUnitMove;

        private List<Unit> _unitTurnOrder = new ();
        private Model _model;
        private int _round;

        private void Start()
        {
            _model = Globals.Global.Model;
            Globals.Global.View.OnRestartBattle.Subscribe(Reset);
            DebugBattleUI.Init();
            BattleField.OnFieldReady += InitUnits;
            BattleField.TestInitField();
        }

        private void Reset()
        {
            BattleField.Reset();
            foreach (var unit in Units)
                unit.Reset();

            _round = 0;
            _currentUnitMove = null;
            _unitTurnOrder.Clear();
            StartBattle();
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
                unit.OnActionPointsEnd += NextUnitMove;
                unit.OnUnitDead += RemoveUnit;
            }

            StartBattle();
        }

        private void StartBattle()
        {
            _unitTurnOrder = Units.OrderBy(unit => unit.InitiativeTest).ToList();
            _currentUnitMove = _unitTurnOrder[0];
            _currentUnitMove.Activate();
            _round++;
            _model.OnNewBattleRound.Invoke(_round);
        }

        private void RemoveUnit(Unit unit)
        {
            _unitTurnOrder.Remove(unit);
            if (_currentUnitMove == unit)
                NextUnitMove();
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
}