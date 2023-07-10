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

        public void Init(MapEntity battleFieldFieldEntity)
        {
            _handlers = GetComponentsInChildren<SkillCommandHandler>().ToList();
            _model = Globals.Global.Model;
            foreach (var handler in _handlers)
            {
                handler.onHandlerEnd += OnHandlerEnd;
                handler.OnTileOccupied += OccupyTile;
                handler.Init(battleFieldFieldEntity);
                handler.Deactivate();
            }

            Debug.Log($"Init {gameObject.name}. AP - {_currentActionPoints}");

            //TODO: bug here with position 
            OccupyTile(battleFieldFieldEntity.Tile(transform.position));

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
            _occupiedTile = tile;
            _occupiedTile.Occupant = this;
        }

        public void Activate()
        {
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
            Health -= damage;
            if (Health > 0) return;

            Debug.Log($"Dead - {gameObject.name}");
            transform.rotation = Quaternion.Euler(0, 90, 90);
            OnUnitDead?.Invoke(this);
        }
    }
}