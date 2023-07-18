using System;
using Cinemachine.Examples;
using RedBjorn.ProtoTiles;
using RedBjorn.ProtoTiles.Example;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Battle.Skills
{
    public class MouseSelector : TargetSelector
    {
        public override void Init(MapEntity fieldEntity)
        {
            _fieldEntity = fieldEntity;
        }

        public override void Activate(SkillCommandHandler skill)
        {
            _skill = skill;
        }

        public override void Deactivate()
        {
            _skill = null;
        }

        private void Update()
        {
            if (_skill == null) return;
            if (_fieldEntity == null) return;

            var mouthPos = MyInput.GroundPosition(_fieldEntity.Settings.Plane());
            var targetOverTile = _fieldEntity.Tile(mouthPos);

            if (targetOverTile == null) return;

            _skill.OverTarget(targetOverTile);
            Debug.LogWarning($"{targetOverTile.Position} - dragTile");

            if (MyInput.GetOnWorldUp(_fieldEntity.Settings.Plane()))
            {
                var clickPos = MyInput.GroundPosition(_fieldEntity.Settings.Plane());
                var targetClickTile = _fieldEntity.Tile(clickPos);

                if (targetClickTile != null)
                {
                    Debug.LogWarning($"{targetClickTile.Position} - clickTile");
                    _skill.SelectTarget(targetClickTile);
                }

            }
        }
    }
}