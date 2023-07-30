using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Player;
using JetBrains.Annotations;
using RedBjorn.ProtoTiles;
using RedBjorn.ProtoTiles.Example;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Battle.Skills
{
    public class Movable : SkillCommandHandler
    {
        public AreaOutline AreaPrefab;
        public PathDrawer PathPrefab;
        public Transform _rotationNode;

        private PathDrawer _path;
        private AreaOutline _area;
        private MapEntity _fieldEntity;

        private Coroutine _moveRoutine;

        public override void Init(MapEntity fieldEntity, UnitSide unitSide, Character character)
        {
            Range = character.MoveRange.Value;
            _fieldEntity = fieldEntity;
            _area = Instantiate(AreaPrefab, Vector3.zero, Quaternion.identity);
            _path = Instantiate(PathPrefab, Vector3.zero, Quaternion.identity);
        }

        public override void Activate()
        {

            _skillInProcces = false;
            _path.gameObject.SetActive(true);
            _area.gameObject.SetActive(true);

            _area.Hide();
            _area.Show(_fieldEntity.WalkableBorder(transform.position, Range), _fieldEntity);

            _path.Show(new List<Vector3>() { }, _fieldEntity);
            _path.InactiveState();
            _path.IsEnabled = true;
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

        public override void OverTarget(TileEntity tile)
        {
            PathUpdate();
        }

        public override void SelectTarget(TileEntity tile)
        {
            if (_skillInProcces) return;
            if (!tile.Vacant) return;

            var tileWorldPosition = _fieldEntity.WorldPosition(tile);
            var path = _fieldEntity.PathTiles(transform.position, tileWorldPosition, Range);
           
            if (path == null) return;

            _path.IsEnabled = false;
            _path.Hide();
            _area.Hide();
            
            if (_moveRoutine != null)
                StopCoroutine(_moveRoutine);

            _skillInProcces = true;
            path.RemoveAt(0);
            _moveRoutine = StartCoroutine(Move(path));
        }

        private IEnumerator Move(List<TileEntity> path)
        {
            var nexTileIndex = 0;
            while (nexTileIndex < path.Count)
            {
                var tile = path[nexTileIndex];

                OnTileOccupied?.Invoke(tile);

                var startPointXZ = transform.position;
                var endPointXZ = _fieldEntity.WorldPosition(tile);
                var positionY = transform.position.y;

                var targetPoint = new Vector3(endPointXZ.x, transform.position.y, endPointXZ.z);
                var stepDir = (targetPoint - transform.position);

                _rotationNode.rotation = Quaternion.LookRotation(stepDir, Vector3.up);

                var time = 0f;
                while (time < 0.5f)
                {
                    var currentPositionXZ = Vector3.Lerp(startPointXZ, endPointXZ, time / 0.5f);

                    float currentYPosition;

                    if (time < 0.25f)
                        currentYPosition = Mathf.Lerp(positionY, positionY + 0.7f, time / 0.25f);
                    else
                        currentYPosition = Mathf.Lerp(positionY + 0.7f, positionY, time / 0.5f);

                    transform.position = new Vector3(currentPositionXZ.x, currentYPosition, currentPositionXZ.z);
                    time += Time.deltaTime;
                    yield return null;
                }


                nexTileIndex++;
            }

            _area.Show(_fieldEntity.WalkableBorder(transform.position, Range), _fieldEntity);
            _path.ActiveState();
            _path.IsEnabled = true;
            onHandlerEnd?.Invoke();
            _skillInProcces = false;
        }

        private void PathUpdate()
        {
            if (!_path) return;
            if (!_path.IsEnabled) return;

            var tilePosition = MyInput.GroundPosition(_fieldEntity.Settings.Plane());
            if(tilePosition == null)return;
            var tile = _fieldEntity.Tile(tilePosition.Value);
            if (tile != null && tile.Vacant)
            {
                var path = _fieldEntity.PathPoints(transform.position, _fieldEntity.WorldPosition(tile.Position),
                    Range);
                _path.Show(path, _fieldEntity);
                _path.ActiveState();
                _area.ActiveState();
                return;
            }

            _path.InactiveState();
            _area.InactiveState();
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