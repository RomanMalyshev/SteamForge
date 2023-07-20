using System;
using System.Collections.Generic;
using RedBjorn.ProtoTiles;
using RedBjorn.ProtoTiles.Example;
using UnityEngine;

namespace Game.Battle.Skills
{
    public class Attacker : SkillCommandHandler
    {
        public int Damage = 5;

        public AreaOutline AreaPrefab;
        public PathDrawer PathPrefab;
        public Transform _rotationNode;
        public AnimationEventHandler AnimationHandler;

        private PathDrawer _path;
        private AreaOutline _area;

        private MapEntity _fieldEntity;
        private UnitSide _side;
        private TileEntity _tile;

        public override void Init(MapEntity mapEntity, UnitSide unitSide)
        {
            _side = unitSide;
            _fieldEntity = mapEntity;
            _area = Instantiate(AreaPrefab, Vector3.zero, Quaternion.identity);
            _path = Instantiate(PathPrefab, Vector3.zero, Quaternion.identity);

            AnimationHandler.Attack += ReduceHealth;
            AnimationHandler.AttackAnimationEnd += EndSkill;
        }


        public override void Activate()
        {
            _skillInProcces = false;
            
            _path.gameObject.SetActive(true);
            _area.gameObject.SetActive(true);

            _area.Hide();
            _area.Show(_fieldEntity.Border(transform.position, Range), _fieldEntity);

            _area.InactiveState();
            _path.Show(new List<Vector3>() { }, _fieldEntity);
            _path.InactiveState();
            _path.IsEnabled = true;

        }

        public override void OverTarget(TileEntity tile)
        {
            if (_skillInProcces) return;
            if (tile.Occupant == null) return;
            if (_side == tile.Occupant.UnitSide) return;
            
            _path.IsEnabled = true;
            _path.Show(_fieldEntity.WorldPosition(tile), _fieldEntity);
            _path.ActiveState();
        }

        public override void SelectTarget(TileEntity tile)
        {
            if (_skillInProcces) return;
            if (tile.Occupant == null) return;
            if (_side == tile.Occupant.UnitSide) return;

            var currentTile = _fieldEntity.Tile(transform.position);

            if (_fieldEntity.IsSameTile(tile, currentTile.Position)) return;
            if (_fieldEntity.Distance(currentTile, tile) > Range) return;

            _skillInProcces = true;
            
            //Rotate in target direction
            var targetPosition = _fieldEntity.WorldPosition(tile);
            var targetPoint = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            var stepDir = (targetPoint - transform.position);
            _rotationNode.rotation = Quaternion.LookRotation(stepDir, Vector3.up);
            
            _tile = tile;
            AnimationHandler.PlayMeleeAttackAnimation();
        }

        private void ReduceHealth()
        {
            if (_tile == null)
            {
                Debug.LogWarning($"Try to reduce null tile! {gameObject.name} {gameObject.transform.GetSiblingIndex()}");
                return;
            }

            _tile.Occupant.GetHit(Damage);
        }

        private void EndSkill()
        {
            _tile = null;
            onHandlerEnd?.Invoke();
            _skillInProcces = false;
        }


        public override void Deactivate()
        {
            _skillInProcces = false;
            _area.Hide();
            _path.Hide();
            _path.IsEnabled = false;

            _path.gameObject.SetActive(false);
            _area.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_area != null)
                Destroy(_area.gameObject);

            if (_path != null)
                Destroy(_path.gameObject);
        }
    }
}