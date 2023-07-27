using System;
using DefaultNamespace.Player;
using RedBjorn.ProtoTiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battle.Skills
{
    public abstract class SkillCommandHandler:MonoBehaviour
    {
        public UnitSide TargetSide;
        public int Weight;
        public float Range;

        public  Action onHandlerEnd { get; set; }
        public Action<TileEntity> OnTileOccupied;

        internal bool _skillInProcces;
        
        public abstract void Init(MapEntity mapEntity, UnitSide unitSide, Character character);
        
        public abstract void Activate();

        public abstract void OverTarget(TileEntity tile);
        public abstract void SelectTarget(TileEntity tile);

        public abstract void Deactivate();
    }
}