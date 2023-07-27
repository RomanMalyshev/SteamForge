using System;
using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Skills;
using RedBjorn.ProtoTiles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Player
{
    public class Unit : MonoBehaviour
    {
        public TileEntity OccupiedTile { get; private set; }
        public int CurrentHealth { get; private set; }

        public UnitSide UnitSide;

        public int Health = 100;

        public int InitiativeTest = 1;
        public int ActionPoints = 20;

        public Action OnActionPointsEnd;
        public Action<Unit> OnUnitDead;

        public TargetSelector Selector;
        public List<SkillCommandHandler> _handlers = new();

        public Image HealthBar;
        public GameObject HealthBarParent;
        public UnitMoseDetector _mouseDetector;

        public Material StandMaterial;
        public Material TurnStandMaterial;
        public MeshRenderer StandMeshRenderer;

        public Animator Animator;

        private int _currentActionPoints;
        private Model _model;

        private bool _isActiveState;

        private SkillCommandHandler _currentSkill;
        private GameObject _stand;
        private MapEntity _mapEntity;

        public void Init(MapEntity battleFieldFieldEntity, UnitSide side, Character character)
        {
            UnitSide = side;
            Selector.Init(battleFieldFieldEntity);

            transform.rotation = Quaternion.Euler(0, UnitSide == UnitSide.Player ? 180 : 0, 0);
            Health = character.Health.Value;
            CurrentHealth = Health;
            ActionPoints = character.ActionPoints.Value;
            InitiativeTest = character.Initiative.Value;

            HealthBarParent.gameObject.SetActive(true);
            HealthBar.fillAmount = (float)CurrentHealth / Health;
            _mapEntity = battleFieldFieldEntity;
            _model = Globals.Global.Model;

            foreach (var handler in _handlers)
            {
                handler.onHandlerEnd += OnHandlerEnd;
                handler.OnTileOccupied += OccupyTile;
                handler.Init(battleFieldFieldEntity, UnitSide, character);
                handler.Deactivate();
            }

            OccupyTile(_mapEntity.Tile(transform.position));
            _mouseDetector.Init(this);
        }

        public void Activate()
        {
            _isActiveState = true;

            _currentActionPoints = ActionPoints;
            _model.OnUnitStartTern.Invoke(this, _handlers);
            _model.OnChangeUnitActionPoints.Invoke(_currentActionPoints);

            StandMeshRenderer.material = TurnStandMaterial;
            Globals.Global.View.onBattleSkipTurn.Subscribe(SkipTurn);
            Globals.Global.View.OnCommandSelect.Subscribe(ActivateCommand);
        }

        private void ActivateCommand(SkillCommandHandler skillCommand)
        {
            if (!_isActiveState) return;
            if (!_handlers.Contains(skillCommand)) return;

            if (_currentSkill != null)
                _currentSkill.Deactivate();

            _currentSkill = skillCommand;
            _currentSkill.Activate();

            Selector.Activate(skillCommand);
        }

        private void SkipTurn()
        {
            if (!_isActiveState) return;

            _currentActionPoints = 0;
            _model.OnChangeUnitActionPoints.Invoke(_currentActionPoints);
            Deactivate();
            OnActionPointsEnd?.Invoke();
        }

        private void OccupyTile(TileEntity tile)
        {
            if (OccupiedTile != null)
                OccupiedTile.Occupant = null;

            OccupiedTile = tile;
            OccupiedTile.Occupant = this;
        }

        private void Deactivate()
        {
            Globals.Global.View.onBattleSkipTurn.Unsubscribe(SkipTurn);
            Globals.Global.View.OnCommandSelect.Unsubscribe(ActivateCommand);

            StandMeshRenderer.material = StandMaterial;

            Selector.Deactivate();
            _isActiveState = false;

            if (_currentSkill == null)
            {
                Debug.LogWarning("some shit hap");
                return;
            }

            _currentSkill.Deactivate();
        }

        private void OnHandlerEnd()
        {
            _currentActionPoints -= 5;
            _model.OnChangeUnitActionPoints.Invoke(_currentActionPoints);
            Debug.Log(" HANDLE END " + gameObject.name + " " + gameObject.transform.GetSiblingIndex());

            if (_currentActionPoints > 0) return;
            
            Deactivate();
            Debug.Log("OnActionPointsEnd " + gameObject.name + " " + gameObject.transform.GetSiblingIndex());
            OnActionPointsEnd?.Invoke();
        }

        public void GetHit(int damage)
        {
            CurrentHealth -= damage;
            _model.OnUnitHealthChange.Invoke(this);

            HealthBar.fillAmount = (float)CurrentHealth / Health;

            if (CurrentHealth > 0) return;
            SetDeadState();
        }

        private void SetDeadState()
        {
            transform.rotation = Quaternion.Euler(0, 90, 90);
            transform.position -= new Vector3(0, 1.2f, 0);
            OccupiedTile.Occupant = null;
            OnUnitDead?.Invoke(this);
            StandMeshRenderer.GameObject().SetActive(false);
            HealthBarParent.gameObject.SetActive(false);

            if (Animator != null)
                Animator.enabled = false;
        }

        public void OnDestroy()
        {
            foreach (var handler in _handlers)
            {
                handler.onHandlerEnd -= OnHandlerEnd;
                handler.OnTileOccupied -= OccupyTile;
            }

            Globals.Global.View.onBattleSkipTurn.Unsubscribe(SkipTurn);
            Globals.Global.View.OnCommandSelect.Unsubscribe(ActivateCommand);
        }
    }
}