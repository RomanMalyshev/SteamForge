using System;
using RedBjorn.ProtoTiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battle.Skills
{
    public abstract class SkillCommandHandler:MonoBehaviour
    {
        public UnitSide TargetSide;
        public int Weight;
        [Space(2)]
        public float Range;
        public  Action onHandlerEnd { get; set; }
        public Action<TileEntity> OnTileOccupied;
        public abstract void Init(MapEntity mapEntity, UnitSide unitSide);
        
        public abstract void Activate();

        public abstract void OverTarget(TileEntity tile);
        public abstract void SelectTarget(TileEntity tile);

        public abstract void Deactivate();
    }
}