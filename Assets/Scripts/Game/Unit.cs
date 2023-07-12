using System;
using System.Collections.Generic;
using System.Linq;
using Game.Battle.Skills;
using RedBjorn.ProtoTiles;
using UnityEngine;

namespace DefaultNamespace.Player
{
    public class Unit : MonoBehaviour
    {
        public int Health = 100;

        public int InitiativeTest = 1;
        public int ActionPoints = 20;

        public Action OnActionPointsEnd;
        
        private int _currentActionPoints;
        private Model _model;

        private TileEntity _occupiedTile;

        public Action<Unit> OnUnitDead;

        private bool _isActiveState;

        private List<SkillCommandHandler> _handlers = new();
        private SkillCommandHandler _currentHandler;
        private GameObject _stand;
        private MapEntity _mapEntity;
        private int _currentHealth;
        private Vector3 _startPosition;

        public void Init(MapEntity battleFieldFieldEntity)
        {
            _startPosition = transform.position;
            _mapEntity = battleFieldFieldEntity;
            _handlers = GetComponentsInChildren<SkillCommandHandler>().ToList();
            _model = Globals.Global.Model;
            foreach (var handler in _handlers)
            {
                handler.onHandlerEnd += OnHandlerEnd;
                handler.OnTileOccupied += OccupyTile;
                handler.Init(battleFieldFieldEntity);
                handler.Deactivate();
            }

            Debug.Log($"Init {gameObject.name}.");

            OccupyTile(_mapEntity.Tile(transform.position));

            Globals.Global.View.OnCommandSelect.Subscribe(ActivateCommand);
        }

        private void ActivateCommand(SkillCommandHandler skillCommand)
        {
            if (!_isActiveState) return;
            if (!_handlers.Contains(skillCommand)) return;

            if (_currentHandler != null)
                _currentHandler.Deactivate();

            _currentHandler = skillCommand;
            _currentHandler.Activate();
        }

        private void OccupyTile(TileEntity tile)
        {
            Debug.LogWarning($"Occupy tile - {tile.Position} by - {gameObject.name}");

            if (_occupiedTile != null)
            {
                Debug.LogWarning($"Deoccupy tile - {_occupiedTile.Position} by - {gameObject.name}!!");
                _occupiedTile.Occupant = null;
            }
            
            _occupiedTile = tile;
            _occupiedTile.Occupant = this;
            
        }

        public void Activate()
        {
            _currentHealth = Health;
            _currentActionPoints = ActionPoints;
            _model.OnNewUnitTern.Invoke(gameObject.name, _handlers);
            _model.OnChangeUnitActionPoints.Invoke(_currentActionPoints);

            _isActiveState = true;
            Debug.Log($"Activate {gameObject.name}. AP - {_currentActionPoints}");
        }

        public void Deactivate()
        {
            Debug.Log($"Deactivate {gameObject.name}. AP - {_currentActionPoints}");
            _isActiveState = false;
            _currentHandler.Deactivate();
        }

        private void OnHandlerEnd()
        {
            _currentActionPoints -= 5;
            _model.OnChangeUnitActionPoints.Invoke(_currentActionPoints);
            Debug.Log($"End handle {gameObject.name}. AP - {_currentActionPoints}");
            if (_currentActionPoints <= 0)
                OnActionPointsEnd?.Invoke();
        }

        public void GetHit(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth > 0) return;

            Debug.Log($"Dead - {gameObject.name}");
            transform.rotation = Quaternion.Euler(0, 90, 90);
            transform.position -= new Vector3(0, 1.2f, 0);
            _occupiedTile.Occupant = null;
            OnUnitDead?.Invoke(this);
        }

        public void Reset()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position += new Vector3(0, 1.2f, 0);
            transform.position = _startPosition;

            _currentHealth = Health;

            foreach (var handler in _handlers)
                handler.Deactivate();
            
            if (_mapEntity != null)
                OccupyTile(_mapEntity.Tile(transform.position));
            
        }

        public void OnDestroy()
        {
            foreach (var handler in _handlers)
            {
                handler.onHandlerEnd -= OnHandlerEnd;
                handler.OnTileOccupied -= OccupyTile;
            }
            
            Globals.Global.View.OnCommandSelect.Unsubscribe(ActivateCommand);
        }
    }
}