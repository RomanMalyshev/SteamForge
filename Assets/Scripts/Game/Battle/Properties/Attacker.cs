using System.Collections.Generic;
using RedBjorn.ProtoTiles;
using RedBjorn.ProtoTiles.Example;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.WSA;

namespace Game.Battle.Skills
{
    public class Attacker : SkillCommandHandler
    {
        public int Damage = 5;

        public AreaOutline AreaPrefab;
        public PathDrawer PathPrefab;

        private PathDrawer _path;
        private AreaOutline _area;

        private MapEntity _fieldEntity;
        private bool _active;

        
        public override void Init(MapEntity mapEntity)
        {
            _fieldEntity = mapEntity;
            _area = Instantiate(AreaPrefab, Vector3.zero, Quaternion.identity);
            _path = Instantiate(PathPrefab, Vector3.zero, Quaternion.identity);
        }

        public override void Activate()
        {
            _path.gameObject.SetActive(true);
            _area.gameObject.SetActive(true);

            _area.Hide();
            _area.Show(_fieldEntity.Border(transform.position, Range), _fieldEntity);
            
            _area.InactiveState();
            _path.Show(new List<Vector3>() { }, _fieldEntity);
            _path.InactiveState();
            _path.IsEnabled = true;

            _active = true;
        }

        public override void Handle()
        {
            var clickPos = MyInput.GroundPosition(_fieldEntity.Settings.Plane());
            var targetTile = _fieldEntity.Tile(clickPos);

            if (targetTile == null) return;
            if (targetTile.Occupant == null) return;
            
            var currentTile = _fieldEntity.Tile(transform.position);

            if (_fieldEntity.IsSameTile(targetTile, currentTile.Position)) return;
            if (_fieldEntity.Distance(currentTile, targetTile) > Range) return;
            
            targetTile.Occupant.GetHit(Damage);
            onHandlerEnd?.Invoke();
        }

        public override void Deactivate()
        {
            _active = false;
            _area.Hide();
            _path.Hide();
            _path.IsEnabled = false;

            _path.gameObject.SetActive(false);
            _area.gameObject.SetActive(false);
        }


        private void Update()
        {
            if (!_active) return;
            if (_fieldEntity == null) return;

            if (MyInput.GetOnWorldUp(_fieldEntity.Settings.Plane()))
            {
                Handle();
            }
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