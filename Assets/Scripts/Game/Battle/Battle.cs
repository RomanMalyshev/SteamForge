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
        [Header("Tags")] public TileTag PlayerTag;
        public TileTag EnemyTag;


        [Header("Refs")] public DebugBattleUI DebugBattleUI;
        public BattleField BattleField;
        public Unit UnitsPrefab;
        public Unit UnitsEnemyPrefab;
        public AISkillsSelector _aiSelector;

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

            //TODO:remake init from globals
            _aiSelector.Init();

            _view.ActiveBattle.Subscribe(mapSettings =>
            {
                if (mapSettings == null) return;
                Reset();
                _mapSettings = mapSettings;
                BattleField.InitField(_mapSettings);
            });

            BattleField.OnFieldReady += InitUnits;

            _view.OnRestartBattle.Subscribe(() =>
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

            //TODO:call from globals
            DebugBattleUI.Reset();
        }

        private void InitUnits(MapEntity mapEntity, List<List<TileEntity>> opponentsTiles)
        {
            _aiSelector.InitNewMap(mapEntity);

            for (var i = 0; i < opponentsTiles.Count; i++)
            {
                for (var j = 0; j < opponentsTiles[i].Count; j++)
                {
                    var tile = opponentsTiles[i][j];

                    var position = new Vector3(mapEntity.WorldPosition(tile).x, UnitsPrefab.transform.position.y,
                        mapEntity.WorldPosition(tile).z);

                    var isPlayer = tile.Preset.Tags.Contains(PlayerTag);
                    var prefab = isPlayer ? UnitsPrefab : UnitsEnemyPrefab;
                    var unit = Instantiate(prefab, position, Quaternion.identity);

                    var side = isPlayer ? UnitSide.Player : UnitSide.Enemy;
                    unit.Init(mapEntity, side);
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
            _model.UnitBattleOrder.Value = _unitTurnOrder;

            _currentUnitMove = _unitTurnOrder[0];
            _currentUnitMove.Activate();
            _round++;
            _model.OnNewBattleRound.Invoke(_round);
        }

        private void NextUnitMove()
        {
            _currentUnitMove.Deactivate();
            _model.OnUnitEndTern.Invoke(_currentUnitMove);

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

            var enemyUnitsCount = _unitTurnOrder.Count(unitTurn => unitTurn.UnitSide == UnitSide.Enemy);
            var playerUnitsCount = _unitTurnOrder.Count(unitTurn => unitTurn.UnitSide == UnitSide.Player);
            
            if (enemyUnitsCount <= 0)
            {
                _model.OnBattleEnd.Invoke(UnitSide.Player);
                Reset();
            }
            
            if (playerUnitsCount <= 0){
                _model.OnBattleEnd.Invoke(UnitSide.Enemy); 
                Reset();
            }
            
        }
    }
}