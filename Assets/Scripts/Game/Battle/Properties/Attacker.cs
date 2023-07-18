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

        public override void OverTarget(TileEntity tile)
        {
            if (tile.Occupant == null) return;

            _path.IsEnabled = true;
            _path.Show(_fieldEntity.WorldPosition(tile), _fieldEntity);
            _path.ActiveState();
        }

        public override void SelectTarget(TileEntity tile)
        {
            if (tile.Occupant == null) return;

            var currentTile = _fieldEntity.Tile(transform.position);

            if (_fieldEntity.IsSameTile(tile, currentTile.Position)) return;
            if (_fieldEntity.Distance(currentTile, tile) > Range) return;

            tile.Occupant.GetHit(Damage);
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

        private void OnDestroy()
        {
            if (_area != null)
                Destroy(_area.gameObject);

            if (_path != null)
                Destroy(_path.gameObject);
        }
    }
}