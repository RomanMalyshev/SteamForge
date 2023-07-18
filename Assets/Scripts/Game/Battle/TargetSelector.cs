using Game.Battle.Skills;
using RedBjorn.ProtoTiles;
using UnityEngine;

namespace Game.Battle
{
    public abstract class TargetSelector : MonoBehaviour
    {
        internal MapEntity _fieldEntity;
        internal SkillCommandHandler _skill;

        public abstract void Init(MapEntity fieldEntity);

        public abstract void Activate(SkillCommandHandler skill);

        public abstract void Deactivate();
    }
}