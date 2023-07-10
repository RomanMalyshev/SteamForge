using System;
using RedBjorn.ProtoTiles;
using UnityEngine;

namespace Game.Battle.Skills
{
    public abstract class SkillCommandHandler:MonoBehaviour
    {
        public float Range;
        public  Action onHandlerEnd { get; set; }
        public Action<TileEntity> OnTileOccupied;
        public abstract void Init(MapEntity mapEntity);
        
        public abstract void Activate();
        public abstract void Handle();
        public abstract void Deactivate();
    }
}