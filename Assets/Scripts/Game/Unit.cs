using System;
using System.Collections.Generic;
using RedBjorn.ProtoTiles;
using RedBjorn.ProtoTiles.Example;
using RedBjorn.Utils;
using UnityEngine;

namespace DefaultNamespace.Player
{
    public class Unit : MonoBehaviour
    {
        public float Speed = 5;
        public float Range = 10f;
        public Transform RotationNode;
        public AreaOutline AreaPrefab;
        public PathDrawer PathPrefab;

        private PathDrawer _path;
        private AreaOutline _area;
        private MapEntity _fieldEntity;

        public void Init(MapEntity fieldEntity)
        {
            _fieldEntity = fieldEntity;
            _area = Instantiate(AreaPrefab, Vector3.zero, Quaternion.identity);

            _area.Hide();
            _area.Show(_fieldEntity.WalkableBorder(transform.position, Range), _fieldEntity);


            _path = Instantiate(PathPrefab, Vector3.zero, Quaternion.identity);
            _path.Show(new List<Vector3>() { }, _fieldEntity);
            _path.InactiveState();
            _path.IsEnabled = true;
        }

        private void Update()
        {
            PathUpdate();
        }

        private void PathUpdate()
        {
            if (_path && _path.IsEnabled)
            {
                var tile = _fieldEntity.Tile(MyInput.GroundPosition(_fieldEntity.Settings.Plane()));
                if (tile != null && tile.Vacant)
                {
                    var path = _fieldEntity.PathPoints(transform.position, _fieldEntity.WorldPosition(tile.Position),
                        Range);
                    _path.Show(path, _fieldEntity);
                    _path.ActiveState();
                    _area.ActiveState();
                }
                else
                {
                    _path.InactiveState();
                    _area.InactiveState();
                }
            }
        }
    }
}