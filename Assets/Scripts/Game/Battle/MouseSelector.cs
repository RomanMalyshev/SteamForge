using RedBjorn.ProtoTiles;
using RedBjorn.ProtoTiles.Example;

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

       
            var tilePosition = MyInput.GroundPosition(_fieldEntity.Settings.Plane());
            if(tilePosition == null)return;
            
            var targetOverTile = _fieldEntity.Tile(tilePosition.Value);
            if (targetOverTile == null) return;

            _skill.OverTarget(targetOverTile);

            if (MyInput.GetOnWorldUp(_fieldEntity.Settings.Plane()))
            {
                var clickPos = MyInput.GroundPosition(_fieldEntity.Settings.Plane());
                if (clickPos == null) return;
                var targetClickTile = _fieldEntity.Tile(clickPos.Value);

                if (targetClickTile != null)
                    _skill.SelectTarget(targetClickTile);
            }
        }
    }
}