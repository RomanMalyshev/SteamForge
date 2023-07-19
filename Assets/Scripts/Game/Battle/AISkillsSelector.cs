﻿using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Player;
using Game.Battle.Skills;
using RedBjorn.ProtoTiles;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Battle
{
    public class AISkillsSelector : MonoBehaviour
    {
        private Model _model;
        private View _view;

        private List<SkillCommandHandler> _skills;
        private Unit _unit;
        private MapEntity _mapEntity;

        private SkillCommandHandler _selectedSkill;
        
        public void Init()
        {
            _model = Globals.Global.Model;
            _view = Globals.Global.View;

            _model.OnUnitStartTern.Subscribe((unit, skills) =>
            {
                if (unit.UnitSide == UnitSide.Player) return;

                _skills = skills;
                _unit = unit;
                _unit.OnActionPointsEnd += DisableAI;
                _unit.OnUnitDead += ResetSelecting;

                SelectSkill();
            });
        }

        public void InitNewMap(MapEntity fieldEntity)
        {
            if (_unit != null)
                ResetSelecting(_unit);
        }

        private void SelectSkill()
        {
            if (_selectedSkill != null)
                _selectedSkill.onHandlerEnd -= SelectSkill;

            _selectedSkill = _skills[Random.Range(0, _skills.Count - 1)];
            _selectedSkill.onHandlerEnd += SelectSkill;
            _view.OnCommandSelect.Invoke(_selectedSkill);
        }

        private void DisableAI()
        {
            ResetSelecting(_unit);
        }


        private void ResetSelecting(Unit unit)
        {
            if (_selectedSkill != null)
                _selectedSkill.onHandlerEnd -= SelectSkill;

            _unit.OnActionPointsEnd -= DisableAI;
            _unit.OnUnitDead -= ResetSelecting;
            _skills = null;
            _unit = null;
            _selectedSkill = null;
        }
    }
}