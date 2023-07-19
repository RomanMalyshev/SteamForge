﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.Battle;
using Game.Battle.Skills;
using RedBjorn.ProtoTiles;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Player
{
    public class Unit : MonoBehaviour
    {
        public UnitSide UnitSide;
        
        public int Health = 100;

        public int InitiativeTest = 1;
        public int ActionPoints = 20;

        public Action OnActionPointsEnd;
        public Action<Unit> OnUnitDead;

        public Game.Battle.TargetSelector Selector;

        public List<SkillCommandHandler> _handlers = new();
        private int _currentActionPoints;
        private Model _model;

        private TileEntity _occupiedTile;

        private bool _isActiveState;

        private SkillCommandHandler _currentHandler;
        private GameObject _stand;
        private MapEntity _mapEntity;
        public int _currentHealth { get; private set; }
    
        private Vector3 _startPosition;

        public void Init(MapEntity battleFieldFieldEntity, UnitSide side)
        {
            UnitSide = side;
            Selector.Init(battleFieldFieldEntity);

            InitiativeTest = Random.Range(0, 3);
            transform.rotation = Quaternion.Euler(0, UnitSide == UnitSide.Player ? 180 : 0, 0);

            _currentHealth = Health;
            
            _startPosition = transform.position;
            _mapEntity = battleFieldFieldEntity;
            _model = Globals.Global.Model;

            foreach (var handler in _handlers)
            {
                handler.onHandlerEnd += OnHandlerEnd;
                handler.OnTileOccupied += OccupyTile;
                handler.Init(battleFieldFieldEntity);
                handler.Deactivate();
            }

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

            Selector.Activate(skillCommand);
        }

        private void OccupyTile(TileEntity tile)
        {
            if (_occupiedTile != null)
                _occupiedTile.Occupant = null;

            _occupiedTile = tile;
            _occupiedTile.Occupant = this;
        }

        public void Activate()
        {
            _isActiveState = true;
            
            
            _currentActionPoints = ActionPoints;
            _model.OnUnitStartTern.Invoke(this, _handlers);
            _model.OnChangeUnitActionPoints.Invoke(_currentActionPoints);

            Debug.Log("Activate " + UnitSide.ToString());
        }

        public void Deactivate()
        {
            Selector.Deactivate();
            _isActiveState = false;
            _currentHandler.Deactivate();
        }

        private void OnHandlerEnd()
        {
            _currentActionPoints -= 5;
            _model.OnChangeUnitActionPoints.Invoke(_currentActionPoints);

            if (_currentActionPoints <= 0)
                OnActionPointsEnd?.Invoke();
            
            Debug.Log(" HANDLE END " +gameObject.name + " "+ gameObject.transform.GetSiblingIndex());
        }

        public void GetHit(int damage)
        {
            _currentHealth -= damage;
           _model.OnUnitHealthChange.Invoke(this);
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