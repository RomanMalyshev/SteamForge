using System.Linq;
using Game.Battle.Skills;
using RedBjorn.ProtoTiles;
using UnityEngine;

namespace Game.Battle
{
    public class AutoSelector:TargetSelector
    {
        //TODO: remove this
        public override void Init(MapEntity fieldEntity)
        {
            _fieldEntity = fieldEntity;
        }

        public override void Activate(SkillCommandHandler skill)
        {
           // _skill = skill;
           // SearchRandomTarget();
            
        }

        public override void Deactivate()
        {
            //_skill = null;
        }

        private void SearchRandomTarget()
        {
            var tiles = _fieldEntity.Tiles.Where(tiles => tiles.Value.Vacant).Select(tiles => tiles.Value).ToArray();
            var randomTile = tiles[Random.Range(0, tiles.Length - 1)];
            Debug.LogWarning($"Algo select {randomTile.Position}");
            _skill.SelectTarget(randomTile);
        }
        
        
    }
}