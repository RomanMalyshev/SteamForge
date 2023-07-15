using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using RedBjorn.ProtoTiles;
using UnityEngine;
using Unit = DefaultNamespace.Player.Unit;

namespace Game.Battle.Skills
{
    public class Battle : MonoBehaviour
    {
        public DebugBattleUI DebugBattleUI;
        public BattleField BattleField;
        public Unit UnitsPrefab;
        private Unit _currentUnitMove;

        private readonly List<Unit> _units = new();
        private List<Unit> _unitTurnOrder = new();
        private Model _model;
        private View _view;
        private int _round;
        private MapSettings _mapSettings;

        private void Start()
        {
            _model = Globals.Global.Model;
            _view = Globals.Global.View;

            //TODO:remake init from globals
            if (DebugBattleUI != null)
                DebugBattleUI.Init();
            else
                Debug.LogWarning("no debug element!");

            _view.ActiveBattle.Subscribe(mapSettings =>
            {
                if (mapSettings == null) return;
                Reset();
                _mapSettings = mapSettings;
                BattleField.InitField(_mapSettings);
            });

            BattleField.OnFieldReady += InitUnits;

            Globals.Global.View.OnRestartBattle.Subscribe(() =>
            {
                if (_mapSettings == null) return;
                Reset();
                BattleField.InitField(_mapSettings);
            });
        }

        private void Reset()
        {
            foreach (var unit in _units)
            {
                unit.OnActionPointsEnd -= NextUnitMove;
                unit.OnUnitDead -= RemoveUnit;
                Destroy(unit.gameObject);
            }

            _units.Clear();

            _round = 0;
            _currentUnitMove = null;
            _unitTurnOrder.Clear();
        }

        private void InitUnits(MapEntity mapEntity, List<List<TileEntity>> opponentsTiles)
        {
            foreach (var opponentTiles in opponentsTiles)
            {
                foreach (var tile in opponentTiles)
                {
                    var position = new Vector3(mapEntity.WorldPosition(tile).x, UnitsPrefab.transform.position.y,
                        mapEntity.WorldPosition(tile).z);
                    var unit = Instantiate(UnitsPrefab, position, Quaternion.identity);
                    unit.Init(mapEntity);
                    unit.OnActionPointsEnd += NextUnitMove;
                    unit.OnUnitDead += RemoveUnit;
                    _units.Add(unit);
                }
            }

            StartBattle();
        }

        private void StartBattle()
        {
            _unitTurnOrder = _units.OrderBy(unit => unit.InitiativeTest).ToList();
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

        private void RemoveUnit(Unit unit)
        {
            _unitTurnOrder.Remove(unit);

            if (_currentUnitMove == unit)
                NextUnitMove();
        }
    }
}