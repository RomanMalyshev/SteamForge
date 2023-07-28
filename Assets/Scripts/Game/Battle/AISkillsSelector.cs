using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Player;
using Game.Battle.Skills;
using RedBjorn.ProtoTiles;
using UnityEngine;

namespace Game.Battle
{
    public class AISkillsSelector : MonoBehaviour
    {
        public GameObject TargetTest;
        private Model _model;
        private View _view;

        private List<SkillCommandHandler> _skills;
        private Unit _unit;
        private MapEntity _mapEntity;

        private SkillCommandHandler _selectedSkill;

        private Attacker _attacker;
        private Movable _movable;

        private Coroutine _waitForSkillRoutine;

        public void Init()
        {
            _model = Globals.Global.Model;
            _view = Globals.Global.View;

            _model.OnUnitStartTern.Subscribe((unit, skills) =>
            {
                Debug.Log(unit.gameObject.name);
                if (unit.UnitSide == UnitSide.Player) return;

                if (_unit != null)
                    ResetSelecting(_unit);

                _skills = skills;
                _unit = unit;
                _unit.OnUnitDead += ResetSelecting;
                Debug.LogWarning("OnUnitStartTern");
                SelectSkill();
            });
        }

        public void InitNewMap(MapEntity mapEntity)
        {
            if (_unit != null)
                ResetSelecting(_unit);

            _mapEntity = mapEntity;
        }

        private void SelectSkill()
        {
            if (_skills == null) return;
            if (_unit.ActionPoints <= 0)
            {
                Debug.LogWarning("Some shit whith actions onHandlerEnd");
                ResetSelecting(_unit);
                return;
            }

            if (_selectedSkill != null)
                _selectedSkill.onHandlerEnd -= SelectSkill;
            _selectedSkill = null;

            var units = _mapEntity.Tiles
                .Where(tilePosition => tilePosition.Value.Occupant)
                .Select(tile => tile.Value)
                .ToList();

            var orderedSkills = _skills.OrderByDescending(skill => skill.Weight).ToList();
            TileEntity targetTile = null;
            while (_selectedSkill == null && orderedSkills.Count > 0)
            {
                var skill = orderedSkills.First();

                orderedSkills.RemoveAt(0);

                var range = skill.Range;

                //target can be choose from all field
                if (skill as Movable)
                    range = 100;

                var tilesInRange = _mapEntity.AreaExistedTiles(_unit.OccupiedTile, range);

                var validTargetInRange = units
                    .Where(unitTile => unitTile.Occupant.UnitSide == skill.TargetSide && tilesInRange
                        .Contains(unitTile))
                    .ToList();

                if (validTargetInRange.Count == 0) continue;


                if (skill as Movable)
                {
                    var rangeAroundTarget = 1;
                    var sortByDistance = validTargetInRange.OrderBy(tile =>
                        _mapEntity.Distance(tile.Position, _unit.OccupiedTile.Position)).ToArray();
                    var targetToMoveTile = sortByDistance[0];
                    while (rangeAroundTarget < 4)
                    {
                        var walkable = _mapEntity.AreaExistedTiles(targetToMoveTile, rangeAroundTarget)
                            .Where(tile => tile.Vacant).ToList();
                        rangeAroundTarget++;

                        if (walkable.Count == 0) continue;
                        var sortWalkableByDistance = walkable.OrderByDescending(tile =>
                            _mapEntity.Distance(tile.Position, _unit.OccupiedTile.Position)).ToArray();
                        targetTile = sortWalkableByDistance[0];
                        break;
                    }
                }
                else
                    targetTile = validTargetInRange[Random.Range(0, validTargetInRange.Count - 1)];

                if (targetTile == null) continue;

                _selectedSkill = skill;
            }

            if (_selectedSkill == null || targetTile == null)
            {
                Debug.LogWarning("Bot has not selected any skill! Next Tern");
                _unit.OnActionPointsEnd?.Invoke();
                return;
            }
            
            Debug.LogWarning($" Bot select {_mapEntity.WorldPosition(targetTile)} ");
            TargetTest.transform.position = _mapEntity.WorldPosition(targetTile);
            if (_waitForSkillRoutine != null)
                StopCoroutine(_waitForSkillRoutine);
            _waitForSkillRoutine = StartCoroutine(WaitForSkillUse(targetTile));
        }

        private IEnumerator WaitForSkillUse(TileEntity tile)
        {
            yield return new WaitForSeconds(2f);

            Debug.LogWarning("Select skill");
            _selectedSkill.onHandlerEnd += SelectSkill;
            _view.OnCommandSelect.Invoke(_selectedSkill);
            _selectedSkill.SelectTarget(tile);
        }

        public void DisableAI()
        {
            Debug.LogWarning("DisableAI");
            ResetSelecting(_unit);
        }

        private void ResetSelecting(Unit unit)
        {
            if (_waitForSkillRoutine != null)
                StopCoroutine(_waitForSkillRoutine);
            if (_selectedSkill != null)
                _selectedSkill.onHandlerEnd -= SelectSkill;
            if (_unit != null)
                _unit.OnUnitDead -= ResetSelecting;
            _skills = null;
            _unit = null;
            _selectedSkill = null;
        }
    }
}