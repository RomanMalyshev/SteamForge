using System;
using RedBjorn.ProtoTiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battle.Skills
{
    public abstract class SkillCommandHandler:MonoBehaviour
    {
        
        public int Weight;
        [Space(2)]
        public float Range;
        public  Action onHandlerEnd { get; set; }
        public Action<TileEntity> OnTileOccupied;
        public abstract void Init(MapEntity mapEntity);
        
        public abstract void Activate();
        public abstract void Handle();
        public abstract void Deactivate();
    }
}