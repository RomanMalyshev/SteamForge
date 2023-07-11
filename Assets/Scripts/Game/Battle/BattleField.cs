using System;
using System.Collections.Generic;
using System.Linq;
using RedBjorn.ProtoTiles;
using RedBjorn.Utils;
using UnityEngine;

namespace Game.Battle
{
    public class BattleField : MonoBehaviour
    {
        public List<Tag> _opponentsTag = new();

        public Action<MapEntity, List<List<TileEntity>>> OnFieldReady;

        private MapSettings _field;
        private MapView _mapView;

        //0 index for player, all nex for enemy
        private List<List<TileEntity>> _opponentsTiles = new();

        public void InitField(MapSettings field)
        {
            if (_mapView != null)
                Destroy(_mapView.gameObject);

            _opponentsTiles.Clear();

            _field = field;

            _mapView = Instantiate(_field.MapViewPrefab, transform);
            var fieldEntity = new MapEntity(_field, _mapView);
            _mapView.Init(fieldEntity);

            Debug.Log($"Setup Field - {field.name} ");

            var tiles = fieldEntity.Tiles.Select(tile => tile.Value).ToList();

            foreach (var tag in _opponentsTag)
            {
                _opponentsTiles.Add(tiles.Where(tile => tile.Preset.Tags.Contains(tag)).ToList());
            }

            if (_opponentsTiles.Count < 2)
            {
                Debug.LogWarning("Not enough init tiles for units! Check tile map tags!");
                return;
            }

            if (_opponentsTiles[0] == null ||_opponentsTiles[0].Count < 4)
            {
                Debug.LogWarning("Not enough tiles for player units! Check tile map tags!");
                return;
            }

            OnFieldReady?.Invoke(fieldEntity, _opponentsTiles);
        }
    }
}