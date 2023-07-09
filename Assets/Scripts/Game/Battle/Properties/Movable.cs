using System;
using System.Collections.Generic;
using DefaultNamespace.Battle;
using RedBjorn.ProtoTiles;
using RedBjorn.ProtoTiles.Example;
using UnityEngine;

namespace DefaultNamespace.Player
{
    public class Movable : CommandHandler
    {
        public float Speed = 5;
        public float Range = 10f;
        public Transform RotationNode;
        public AreaOutline AreaPrefab;
        public PathDrawer PathPrefab;

       // public Action onHandlerEnd;
        
        private PathDrawer _path;
        private AreaOutline _area;
        private MapEntity _fieldEntity;
        private bool _active;

        public override Action onHandlerEnd { get; set; }

        public override void Init(MapEntity fieldEntity)
        {
            _fieldEntity = fieldEntity;
            _area = Instantiate(AreaPrefab, Vector3.zero, Quaternion.identity);
            _path = Instantiate(PathPrefab, Vector3.zero, Quaternion.identity);
            var tile = _fieldEntity.Tile(transform.position);
            if (tile != null)
                tile.Occupied = true;
        }
        
        public override void Activate()
        {
            _active = true;
            
            _path.gameObject.SetActive(true);
            _area.gameObject.SetActive(true);
            
            _area.Hide();
            _area.Show(_fieldEntity.WalkableBorder(transform.position, Range), _fieldEntity);
            
            _path.Show(new List<Vector3>() { }, _fieldEntity);
            _path.InactiveState();
            _path.IsEnabled = true;
        }

        public override void Handle()
        {
            throw new NotImplementedException();
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
                var clickPos = MyInput.GroundPosition(_fieldEntity.Settings.Plane());
                var tile = _fieldEntity.Tile(clickPos);
                if (tile != null && tile.Vacant)
                {
                    var path = _fieldEntity.PathTiles(transform.position, clickPos, Range);
                    if (path != null)
                    {
                        var startMoveTile = _fieldEntity.Tile(transform.position);
                        if (startMoveTile != null)
                            startMoveTile.Occupied = false;

                        _path.Hide();
                        var endPoint = _fieldEntity.WorldPosition(path[^1]);
                        transform.position = new Vector3(endPoint.x, transform.position.y, endPoint.z);

                        tile.Occupied = true;
                        onHandlerEnd?.Invoke();
                    }

                    _area.Show(_fieldEntity.WalkableBorder(transform.position, Range), _fieldEntity);
                }
                
            }

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